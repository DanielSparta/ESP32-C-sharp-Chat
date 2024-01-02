#include <WiFi.h>
#include <WiFiClient.h>
#define LED 2

const char *ssid = "Ariel";         // Change to your WiFi network SSID
const char *password = "12345678"; // Change to your WiFi network password
const char *serverIP = "172.0.2.131";   // Change to the server IP address
const int serverPort = 81;              // Change to the server port
int tryAmount = 0;

WiFiClient client;

void setup() {
  Serial.begin(115200);
  pinMode(LED, OUTPUT);
  // Connect to Wi-Fi
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
    tryAmount++;
    //adding reset
  }
  Serial.println("Connected to WiFi");

  // Connect to server
  Serial.println("Connecting to server...");
  if (client.connect(serverIP, serverPort)) {
    Serial.println("Connected to server");
    // You can send data or perform other actions here
  } else {
    Serial.println("Connection to server failed");
  }
}

void loop() {
  
  
  
  
  if (Serial.available() > 0)
    client.println(Serial.readStringUntil('\n'));
    
  if (client.available()) {
    String receivedMessage = client.readStringUntil('\n');
    Serial.println("Received message from server: " + receivedMessage);

    // Check if the received message is "data"
    if (receivedMessage.equals("on")) {
      Serial.println("Received 'on' from server. Sending 'right' back.");
      client.println("right");
      digitalWrite(LED, HIGH);
    }

    if (receivedMessage.equals("off")) {
      Serial.println("Received 'off' from server. Sending 'right' back.");
      client.println("right");
      digitalWrite(LED, LOW);
    }

    if (receivedMessage.equals(""))
      Serial.println("Received 'NULLBYTE' from server (Connection test).");
  }

  // Your main code goes here
  // You can send/receive data from the server in this loop
}
