/*
  Name:		esp_autonet.ino
  Created:	2018/9/15 11:33:59
  Author:	lpp
  lpp12138@outlook.com
*/

#include <ArduinoJson.h>
#include <WiFiUdp.h>
#include <WiFiServer.h>
#include <WiFiClient.h>
#include <ESP8266WiFi.h>
#include "jsonDataDefs.h"

String deviceName = "de1";
const char* ssid ="LPP_2.4Ghz";
const char* password = "51804000";
const unsigned short localUdpPort = 2333;
String serverIP = "";
unsigned short serverTcpPort = 2334;
WiFiUDP myUdp;
WiFiClient myTcp;

const String recUdpPackage();
const void connectWifi();

void setup()
{
  Serial.begin(115200);
  connectWifi();
  myUdp.begin(localUdpPort);
   WiFi.setSleepMode(WIFI_NONE_SLEEP);
}

void loop()
{
	intKeyPair sensorData;
	sensorData.name = "testData";
	sensorData.data = digitalRead(D5);
  if (WiFi.status() != WL_CONNECTED)
  {
    connectWifi();
    return;
  }
  String jsonData = recUdpPackage();
  bool alert = false;
  if (jsonData != "")
  {
	  StaticJsonBuffer<500> receiveJsonBuffer;
	  JsonObject& root = receiveJsonBuffer.parseObject(jsonData);
	  if (root.success())
	  {
		  Serial.println("json parse success");
		  if (serverIP == "" || root["protocol"] != "myEspNet" || !myTcp.connect(serverIP, serverTcpPort))
		  {
			  Serial.print("connect Failed or no server ip address");
			  return;
		  }
		  else
		  {
			  String command = root.get<String>("command");
			  StaticJsonBuffer<500> sendJsonBuffer;
			  JsonObject& returnData = sendJsonBuffer.createObject();
			  returnData["protocol"] = "myEspNet";
			  returnData["deviceName"] = deviceName;
			  JsonObject& data = returnData.createNestedObject("data");			  
			  //CPP switch不能用String做参数，所以。。。
			  if (command=="ping")
			  {
				  data[sensorData.name] = sensorData.value;
			  }
			  else if (command=="on")
			  {

			  }
			  else if (command=="off")
			  {
				  data["message"] = "This command is undefinded yet";
			  }
			  else if (command=="set")
			  {
				  data["message"] = "This command is undefinded yet";
			  }
			  else
			  {
				  data["message"] = "This command is undefinded yet";
			  }
			  String sendata;
			  returnData.printTo(sendata);
			  myTcp.println(sendata);
			  myTcp.stop();
		  }
	  }
	  serverIP = "";
  }
  delay(500);
}
//解析UDP包-|checked
const String recUdpPackage()
{
	//Serial.println("in fenc \"recUdpPackage\"");
  char incomeingPackage[255] = "0";
  int packageSize = myUdp.parsePacket();
  //Serial.println("packageSize:"+String(packageSize));
  if (packageSize)
  {
    int packageLength = myUdp.read(incomeingPackage, 255);
    if (packageLength > 0)
    {
      //如果是正常包，则在其末尾补\0使其成为完整string
      incomeingPackage[packageLength] = 0;
	  //获取服务器IP，服务器TCP端口(服务器UDP端口+1)
      serverIP = myUdp.remoteIP().toString();
      serverTcpPort = myUdp.remotePort() + 1;
      Serial.println("UDP Pack received");
      Serial.println("UDP_DATA:" + String(incomeingPackage));
      Serial.println("Remote IP:" + myUdp.remoteIP().toString());
      Serial.println("Remote Port:" + String(myUdp.remotePort()));
    }
  }
  return String(incomeingPackage);
}

//连接网络-|checked
const void connectWifi()
{
  delay(10);
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}
