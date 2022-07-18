/*
     Example of connection using Static IP
     by Evandro Luis Copercini
     Public domain - 2017
*/

#include <WiFi.h>

const char* ssid     = "CHIP";
const char* password = "Samsung2233";
const char* host     = "10.1.1.1";
const int Port = 65433;
WiFiClient client;
int up = 0;
int down = 0;
int left = 0;
int right = 0;
int sel = 0;
int star = 0;
int a = 0;
int b = 0;
int x = 0;
int y = 0;

void setup()
{
  Serial.begin(115200);
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println(WiFi.status());
    if(WiFi.status() ==0){
        WiFi.begin(ssid, password);
      }
  }
  Serial.println(WiFi.localIP());
  Serial.print("connecting to ");
  Serial.println(host);
  client.connect(host, Port);
}

void loop()
{
  while(true){
//    if (!client.connect(host, Port)) {
//      Serial.println("connection failed");
//      return;
//    }
    // Read all the lines of the reply from server and print them to Serial
    String line;
    while (client.available()) {
      line = client.readStringUntil('\r');
      Serial.print(line);
    }
    if(line == "update"){
      String output = String(up);
      output.concat(",");
      output.concat(down);
      output.concat(",");
      output.concat(left);
      output.concat(",");
      output.concat(right);
      output.concat(",");
      output.concat(sel);
      output.concat(",");
      output.concat(star);
      output.concat(",");
      output.concat(a);
      output.concat(",");
      output.concat(b);
      output.concat(",");
      output.concat(x);
      output.concat(",");
      output.concat(y);
      output.concat("DONE");
      Serial.println(output);
      client.print(output);
    }
  }
}
