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

void WiFiStationConnected(WiFiEvent_t event, WiFiEventInfo_t info){
  Serial.println("Connected to AP successfully!");
}

void WiFiGotIP(WiFiEvent_t event, WiFiEventInfo_t info){
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  client.connect(host,port);
}

void WiFiStationDisconnected(WiFiEvent_t event, WiFiEventInfo_t info){
  Serial.println("Disconnected from WiFi access point");
  Serial.print("WiFi lost connection. Reason: ");
  Serial.println(info.disconnected.reason);
  Serial.println("Trying to Reconnect");
  WiFi.begin(ssid, password);
}

void setup()
{
  WiFi.disconnect(true);

  delay(1000);

  WiFi.onEvent(WiFiStationConnected, SYSTEM_EVENT_STA_CONNECTED);
  WiFi.onEvent(WiFiGotIP, SYSTEM_EVENT_STA_GOT_IP);
  WiFi.onEvent(WiFiStationDisconnected, SYSTEM_EVENT_STA_DISCONNECTED);

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
  client.connect(host,port);
}
void loop()
{
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
  
  //Serial.println(output);
  client.print(output);
  delay(100);
}
