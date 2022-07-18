#include <WiFi.h>
 
const char* ssid = "CHIP";
const char* password =  "Samsung2233";
 
const uint16_t port = 65433;
const char * host = "10.1.1.1";

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
 
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println("...");
  }
 
  Serial.print("WiFi connected with IP: ");
  Serial.println(WiFi.localIP());
 
}
 
void loop()
{
    WiFiClient client;
 
    if (!client.connect(host, port)) {
 
        Serial.println("Connection to host failed");
 
        delay(1000);
        return;
    }
 
    Serial.println("Connected to server successful!");
  while(true){
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
      client.flush();
      delay(30);
  }
  
 
    Serial.println("Disconnecting...");
    client.stop();
 
    delay(10000);
}
