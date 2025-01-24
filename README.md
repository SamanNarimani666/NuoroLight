Title: Real-Time RGB LED Control with MQTT, ASP.NET Core, SignalR, and ESP8266

Description:
This project implements a real-time RGB LED control system using MQTT, ASP.NET Core, SignalR, and ESP8266. Users can select their desired colors through a Color Picker interface, and the selected color is transmitted to the ESP8266 module via MQTT protocol. The ESP8266 processes the received RGB values and adjusts the LED's brightness using PWM (Pulse Width Modulation).

The system architecture includes:

MQTT Integration: Facilitates lightweight, efficient communication between the server and the ESP8266 module.
ASP.NET Core Backend: Handles the logic for processing user requests, managing MQTT communication, and storing logs in the database.
SignalR Real-Time Updates: Enables instant feedback and seamless communication between the user interface and the backend.
Database Logging: Logs all user actions, including selected colors and timestamps, for monitoring and analysis.
Key Features:

Real-Time Communication: Instant updates on LED colors when users interact with the Color Picker.
Scalable Architecture: Designed for easy extension to support multiple devices and advanced lighting effects.
IoT Integration: Demonstrates the power of combining IoT devices with modern web technologies for smart home automation.
This project showcases the potential of integrating MQTT, ASP.NET Core, and SignalR to create responsive, scalable, and user-friendly IoT applications.
عنوان: کنترل رنگ LED RGB در زمان واقعی با استفاده از MQTT، ASP.NET Core، SignalR و ESP8266

توضیحات:
این پروژه یک سیستم کنترل رنگ LED RGB در زمان واقعی با استفاده از MQTT، ASP.NET Core، SignalR و ESP8266 پیاده‌سازی می‌کند. کاربران می‌توانند رنگ دلخواه خود را از طریق یک رنگ‌سنج (Color Picker) انتخاب کنند و رنگ انتخاب‌شده از طریق پروتکل MQTT به ماژول ESP8266 ارسال می‌شود. ESP8266 مقادیر RGB دریافت‌شده را پردازش کرده و شدت نور LED را با استفاده از PWM (مدولاسیون پهنای پالس) تنظیم می‌کند.

معماری سیستم شامل موارد زیر است:

یکپارچگی MQTT: ارتباط سبک و کارآمد بین سرور و ماژول ESP8266 را فراهم می‌کند.
بک‌اند ASP.NET Core: منطق پردازش درخواست‌های کاربر، مدیریت ارتباطات MQTT و ذخیره لاگ‌ها در پایگاه داده را مدیریت می‌کند.
به‌روزرسانی‌های زمان واقعی SignalR: ارتباط بدون وقفه و بازخورد فوری بین رابط کاربری و بک‌اند را ممکن می‌سازد.
ثبت در پایگاه داده: تمامی اقدامات کاربران از جمله رنگ‌های انتخاب‌شده و زمان آن‌ها برای نظارت و تحلیل ثبت می‌شود.
ویژگی‌های کلیدی:

ارتباط زمان واقعی: به‌روزرسانی فوری رنگ‌های LED هنگام تعامل کاربران با رنگ‌سنج.
معماری مقیاس‌پذیر: طراحی‌شده برای گسترش آسان جهت پشتیبانی از دستگاه‌های متعدد و افکت‌های پیشرفته نورپردازی.
یکپارچگی IoT: قدرت ترکیب دستگاه‌های IoT با فناوری‌های مدرن وب برای اتوماسیون خانه‌های هوشمند را نشان می‌دهد.
این پروژه توانایی ادغام MQTT، ASP.NET Core و SignalR را برای ایجاد برنامه‌های IoT پاسخگو، مقیاس‌پذیر و کاربرپسند به نمایش می‌گذارد.
