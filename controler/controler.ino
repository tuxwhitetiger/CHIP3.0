#include <WiFi.h>

const char* ssid = "CHIP";
const char* password = "Samsung2233";

const uint16_t port = 65433;
IPAddress host(10,1,1,1);

int uppin = 12;
int downpin = 14;
int leftpin = 27;
int rightpin = 26;
int starpin = 25;

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


WiFiClient client;
void setup()
{
  WiFi.mode(WIFI_STA);
  pinMode(uppin, INPUT);
  pinMode(downpin, INPUT);
  pinMode(leftpin, INPUT);
  pinMode(rightpin, INPUT);
  pinMode(starpin, INPUT);
  Serial.begin(115200);
  Serial.print("ESP Board MAC Address:  ");
  Serial.println(WiFi.macAddress());
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println("...");
  }
  Serial.print("WiFi connected with IP: ");
  Serial.println(WiFi.localIP());
  Serial.print("Connectiong to ");
  Serial.print(host);
  Serial.print(" on ");
  Serial.println(port);
}

void loop()
{
  if (!client.connected()){
    Serial.println();
    Serial.println("disconnected.");
    Serial.print("Connecting to ");
    Serial.print(host);
    Serial.print(" on ");
    Serial.println(port);
    while(!client.connect(host, port)){
      Serial.println("reconnecting.");
      delay(500);
      if (WiFi.status() != WL_CONNECTED){
          Serial.println("WiFi lost");
      }
    }
  }

  up= digitalRead(uppin);
  down=digitalRead(downpin);
  left=digitalRead(leftpin);
  right=digitalRead(rightpin);
  star=digitalRead(starpin);

  String output = "";
  output.concat(up);
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
  delay(50);
}
