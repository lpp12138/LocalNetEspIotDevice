/*
 Name:		test_debug.ino
 Created:	2018/12/26 15:49:31
 Author:	lpp
 lpp12138@outlook.com
*/

// the setup function runs once when you press reset or power the board
#include <WiFiClient.h>
#include <ESP8266WiFi.h>
#include <ArduinoJson.hpp>
#include <ArduinoJson.h>

void connectWifi();

String deviceName = "device_0";
const char* ssid = "H3C"; //"LPP_network_root";
const char* password = "admin12345";//"51804000";
const char* host = "10.100.0.18";
unsigned short serverTcpPort = 2334;
String jsonData = "";
WiFiClient myTcp;
bool alert = false;
void setup() {
	Serial.begin(115200);
}

// the loop function runs over and over again until power down or reset
void loop() {
	if (Serial.available())
	{
		jsonData = Serial.readString();
	}
	if (jsonData != "")
	{
		StaticJsonBuffer<200> jsonBuffer;
		JsonObject& root = jsonBuffer.parseObject(jsonData);
		if (root.success())
		{
			Serial.println("json parse success");
			if (root["protocol"] == "myEspNet")
			{
				Serial.print("protocol recognized");
				String sendData = "{\"deviceName\":\"" + deviceName + "\",\"alert\":" + String(alert) + "}";
				Serial.println(sendData);
				Serial.println();
			}
		}
		else
		{
			Serial.println("json parse failed");
		}
	}
	jsonData = "";
	//delay(500);
}

void connectWifi()
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
