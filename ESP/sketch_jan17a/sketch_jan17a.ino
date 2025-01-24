#include <ESP8266WiFi.h>
#include <PubSubClient.h>

// لیست تمامی پایه‌های GPIO در ESP8266
const int gpioPins[] = {0, 1, 2, 3, 4, 5, 12, 13, 14, 15, 16};

// اطلاعات اتصال به Wi-Fi
const char* ssid = "saman";      // نام Wi-Fi
const char* password = "Saman166"; // رمز Wi-Fi

// اطلاعات اتصال به سرور MQTT
const char* mqttServer = "services.irn9.chabokan.net"; // آدرس سرور MQTT
const int mqttPort = 38042;                           // پورت MQTT
const char* mqttUser = "james";                      // نام کاربری MQTT
const char* mqttPassword = "sgBsCDIb0vTMwdxC";       // رمز عبور MQTT

WiFiClient espClient;
PubSubClient client(espClient);

// پایه‌های GPIO برای رله‌ها
const int relay1Pin = 14; // GPIO14 (D5)
const int relay2Pin = 12; // GPIO12 (D6)
const int relay3Pin = 13; // GPIO13 (D7)

// پایه‌های RGB
const int redPin = 15;    // GPIO5
const int greenPin = 4;  // GPIO4
const int bluePin = 5;   // GPIO0

// تابع callback برای مدیریت پیام‌های دریافتی از MQTT
void callback(char* topic, byte* payload, unsigned int length) {
  String message;
  for (unsigned int i = 0; i < length; i++) {
    message += (char)payload[i];
  }

  Serial.print("Message received [");
  Serial.print(topic);
  Serial.print("]: ");
  Serial.println(message);

  // مدیریت پیام‌های دریافتی
  if (String(topic) == "relay1") {
    bool status = (message == "1");
    digitalWrite(relay1Pin, status ? HIGH : LOW);
    sendLampStatus(1, status); // ارسال وضعیت لامپ 1
  } else if (String(topic) == "relay2") {
    bool status = (message == "1");
    digitalWrite(relay2Pin, status ? HIGH : LOW);
    sendLampStatus(2, status); // ارسال وضعیت لامپ 2
  } else if (String(topic) == "relay3") {
    bool status = (message == "1");
    digitalWrite(relay3Pin, status ? HIGH : LOW);
    sendLampStatus(3, status); // ارسال وضعیت لامپ 3
  } else if (String(topic) == "rgb") {
    if(message == "off")
    {
      analogWrite(redPin, 0);
      analogWrite(greenPin, 0);
      analogWrite(bluePin, 0);
      Serial.println("RGB turned off");
    } else{
    int r, g, b;
    if (sscanf(message.c_str(), "%d,%d,%d", &r, &g, &b) == 3) {

     analogWrite(redPin,r);   // تنظیم شدت نور قرمز
      analogWrite(greenPin, b); // تنظیم شدت نور سبز
      analogWrite(bluePin, g);  // تنظیم شدت نور آبی
      Serial.printf("RGB set to: R=%d, G=%d, B=%d\n", r, g, b);
    } else {
      Serial.println("Invalid RGB format. Expected format: R,G,B");
    }
    }
  }
}

// تابع برای ارسال پیام به MQTT زمانی که لامپ روشن می‌شود
void sendLampStatus(int lampNumber, bool status) {
  // ساخت پیام
  String message = "لامپ " + String(lampNumber) + (status ? " روشن شد" : " خاموش شد");
  String topic = "lamp" + String(lampNumber);  // موضوع مرتبط با لامپ

  // ارسال پیام به سرور MQTT
  if (client.publish(topic.c_str(), message.c_str())) {
    // نمایش پیام در سریال مانیتور برای اشکال‌زدایی
    Serial.println("پیام ارسال شد: " + message);
  } else {
    // در صورت بروز مشکل در ارسال پیام
    Serial.println("خطا در ارسال پیام: " + message);
  }
}

void setup() {
  Serial.begin(9600);
  Serial.println("Setting all GPIO pins to LOW...");

  // تنظیم تمامی پایه‌ها به عنوان خروجی و مقدار LOW
  for (int i = 0; i < sizeof(gpioPins) / sizeof(gpioPins[0]); i++) {
    pinMode(gpioPins[i], OUTPUT);
    digitalWrite(gpioPins[i], LOW);
    Serial.print("GPIO ");
    Serial.print(gpioPins[i]);
    Serial.println(" set to LOW.");
  }

  Serial.println("All GPIO pins are set to LOW.");

  // تنظیم پایه‌های GPIO
  pinMode(relay1Pin, OUTPUT);
  pinMode(relay2Pin, OUTPUT);
  pinMode(relay3Pin, OUTPUT);

  // تنظیم پایه‌های RGB
  pinMode(redPin, OUTPUT);
  pinMode(greenPin, OUTPUT);
  pinMode(bluePin, OUTPUT);

  // خاموش کردن رله‌ها و RGB در ابتدا
  digitalWrite(relay1Pin, LOW);
  digitalWrite(relay2Pin, LOW);
  digitalWrite(relay3Pin, LOW);
  analogWrite(redPin, 0);
  analogWrite(greenPin, 0);
  analogWrite(bluePin, 0);

  // اتصال به Wi-Fi
  Serial.print("Connecting to Wi-Fi");
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.print(".");
  }
  Serial.println("\nWi-Fi connected!");

  // تنظیمات MQTT
  client.setServer(mqttServer, mqttPort);
  client.setCallback(callback);

  // تلاش برای اتصال به سرور MQTT
  connectToMQTT();
}


void loop() {
  // اطمینان از اتصال به سرور MQTT
  if (!client.connected()) {
    connectToMQTT();
  }
  client.loop();
}

void connectToMQTT() {
  while (!client.connected()) {
    Serial.print("Connecting to MQTT...");
    if (client.connect("ESP8266Client", mqttUser, mqttPassword)) {
      Serial.println("connected");

      // اشتراک‌گذاری موضوعات (topics)
      client.subscribe("relay1");
      client.subscribe("relay2");
      client.subscribe("relay3");
      client.subscribe("rgb"); // اشتراک برای RGB
    } else {
      Serial.print("failed with state ");
      Serial.println(client.state());
      delay(2000);
    }
  }
}



// #include <ESP8266WiFi.h>
// #include <PubSubClient.h>

// // لیست تمامی پایه‌های GPIO در ESP8266
// const int gpioPins[] = {0, 1, 2, 3, 4, 5, 12, 13, 14, 15, 16};

// // اطلاعات اتصال به Wi-Fi
// const char* ssid = "1348";      // نام Wi-Fi
// const char* password = "Saman166"; // رمز Wi-Fi

// // اطلاعات اتصال به سرور MQTT
// const char* mqttServer = "services.irn9.chabokan.net"; // آدرس سرور MQTT
// const int mqttPort = 38042;                           // پورت MQTT
// const char* mqttUser = "james";                      // نام کاربری MQTT
// const char* mqttPassword = "sgBsCDIb0vTMwdxC";       // رمز عبور MQTT

// WiFiClient espClient;
// PubSubClient client(espClient);

// // پایه‌های GPIO برای رله‌ها
// const int relay1Pin = 14; // GPIO14 (D5)
// const int relay2Pin = 12; // GPIO12 (D6)
// const int relay3Pin = 13; // GPIO13 (D7)

// // پایه‌های RGB
// const int redPin = 15;    // GPIO5
// const int greenPin = 4;  // GPIO4
// const int bluePin = 5;   // GPIO0

// // تابع callback برای مدیریت پیام‌های دریافتی از MQTT
// void callback(char* topic, byte* payload, unsigned int length) {
//   String message;
//   for (unsigned int i = 0; i < length; i++) {
//     message += (char)payload[i];
//   }

//   Serial.print("Message received [");
//   Serial.print(topic);
//   Serial.print("]: ");
//   Serial.println(message);

//   // مدیریت پیام‌های دریافتی
//   if (String(topic) == "relay1") {
//     digitalWrite(relay1Pin, message == "1" ? HIGH : LOW);
//     sendRelayStatus("relay1", message == "1" ? "on" : "off"); // ارسال وضعیت رله به سرور
//   } else if (String(topic) == "relay2") {
//     digitalWrite(relay2Pin, message == "1" ? HIGH : LOW);
//     sendRelayStatus("relay2", message == "1" ? "on" : "off"); // ارسال وضعیت رله به سرور
//   } else if (String(topic) == "relay3") {
//     digitalWrite(relay3Pin, message == "1" ? HIGH : LOW);
//     sendRelayStatus("relay3", message == "1" ? "on" : "off"); // ارسال وضعیت رله به سرور
//   } else if (String(topic) == "rgb") {
//     if(message == "off")
//     {
//       analogWrite(redPin, 0);
//       analogWrite(greenPin, 0);
//       analogWrite(bluePin, 0);
//       Serial.println("RGB turned off");
//       sendRGBStatus("off"); // ارسال وضعیت RGB به سرور
//     } else {
//       int r, g, b;
//       if (sscanf(message.c_str(), "%d,%d,%d", &r, &g, &b) == 3) {
//       analogWrite(redPin, r);   // تنظیم شدت نور قرمز
//       analogWrite(greenPin, b); // تنظیم شدت نور سبز
//       analogWrite(bluePin, g);  // تنظیم شدت نور آبی
//       Serial.printf("RGB set to: R=%d, G=%d, B=%d\n", r, g, b);
//         sendRGBStatus(message.c_str()); // ارسال وضعیت RGB به سرور
//       } else {
//         Serial.println("Invalid RGB format. Expected format: R,G,B");
//       }
//     }
//   }
// }

// // تابع برای ارسال وضعیت رله به سرور MQTT
// void sendRelayStatus(const char* relay, const char* status) {
//   String message = String(relay) + " is " + status;
//   client.publish("status", message.c_str(), true); // ارسال پیام با retain
//   Serial.println(message); // نمایش وضعیت در سریال مانیتور
// }

// // تابع برای ارسال وضعیت RGB به سرور MQTT
// void sendRGBStatus(const char* status) {
//   String message = "RGB is " + String(status);
//   client.publish("status", message.c_str(), true); // ارسال پیام با retain
//   Serial.println(message); // نمایش وضعیت در سریال مانیتور
// }

// void setup() {
//   Serial.begin(9600);
//   Serial.println("Setting all GPIO pins to LOW...");

//   // تنظیم تمامی پایه‌ها به عنوان خروجی و مقدار LOW
//   for (int i = 0; i < sizeof(gpioPins) / sizeof(gpioPins[0]); i++) {
//     pinMode(gpioPins[i], OUTPUT);
//     digitalWrite(gpioPins[i], LOW);
//     Serial.print("GPIO ");
//     Serial.print(gpioPins[i]);
//     Serial.println(" set to LOW.");
//   }

//   Serial.println("All GPIO pins are set to LOW.");

//   // تنظیم پایه‌های GPIO
//   pinMode(relay1Pin, OUTPUT);
//   pinMode(relay2Pin, OUTPUT);
//   pinMode(relay3Pin, OUTPUT);

//   // تنظیم پایه‌های RGB
//   pinMode(redPin, OUTPUT);
//   pinMode(greenPin, OUTPUT);
//   pinMode(bluePin, OUTPUT);

//   // خاموش کردن رله‌ها و RGB در ابتدا
//   digitalWrite(relay1Pin, LOW);
//   digitalWrite(relay2Pin, LOW);
//   digitalWrite(relay3Pin, LOW);
//   analogWrite(redPin, 0);
//   analogWrite(greenPin, 0);
//   analogWrite(bluePin, 0);

//   // اتصال به Wi-Fi
//   Serial.print("Connecting to Wi-Fi");
//   WiFi.begin(ssid, password);
//   while (WiFi.status() != WL_CONNECTED) {
//     delay(1000);
//     Serial.print(".");
//   }
//   Serial.println("\nWi-Fi connected!");

//   // تنظیمات MQTT
//   client.setServer(mqttServer, mqttPort);
//   client.setCallback(callback);

//   // تلاش برای اتصال به سرور MQTT
//   connectToMQTT();
// }

// void loop() {
//   // اطمینان از اتصال به سرور MQTT
//   if (!client.connected()) {
//     connectToMQTT();
//   }
//   client.loop();
// }

// void connectToMQTT() {
//   while (!client.connected()) {
//     Serial.print("Connecting to MQTT...");
//     if (client.connect("ESP8266Client", mqttUser, mqttPassword)) {
//       Serial.println("connected");

//       // اشتراک‌گذاری موضوعات (topics)
//       client.subscribe("relay1");
//       client.subscribe("relay2");
//       client.subscribe("relay3");
//       client.subscribe("rgb"); // اشتراک برای RGB
//     } else {
//       Serial.print("failed with state ");
//       Serial.println(client.state());
//       delay(2000);
//     }
//   }
// }

