#include <LiquidCrystal.h>
#include <VarSpeedServo.h>

// servo宣言
VarSpeedServo mys_LR;
VarSpeedServo mys_UD;
VarSpeedServo mys_GP;
VarSpeedServo mys_FB;

LiquidCrystal lcd(7, 8, 9, 10, 11, 12);

// 文字列受け取り
String label_LR = "null";
String label_UD = "null";
String label_FB = "null";

// 文字列を数値に変換したものを格納
int angle_LR;
int angle_UD;
int angle_FB;
String GP = "null";
int SP1 = 50;
int flag = 0;
int cnt = 0;

void setup() {
  Serial.begin(115200);
  mys_LR.attach(6);
  mys_UD.attach(3);
  mys_GP.attach(5);
  mys_FB.attach(2);
}

void loop() {
  if(Serial.available() > 0){
    flag = 1;
    cnt = 0;
    lcd.begin(16, 2);          // LCDの桁数と行数を指定する(16桁2行)
    lcd.clear();               // LCD画面をクリア
    lcd.setCursor(0, 0);       // カーソルの位置を指定
    
    // 左右
    label_LR = Serial.readStringUntil('\n');
    angle_LR = label_LR.toInt();
    mys_LR.write(angle_LR,SP1);
    // 文字列出力
    lcd.print(angle_LR);       // 文字の表示
    lcd.print("&");            // 文字の表示
    
        
    //上下
    label_UD = Serial.readStringUntil('\n');
    angle_UD = label_UD.toInt();
    mys_UD.write(angle_UD,SP1);
    // 文字列出力
    lcd.print(angle_UD);       // 文字の表示
    lcd.print("&");            // 文字の表示

    // 前後
    label_FB = Serial.readStringUntil('\n');
    angle_FB = label_FB.toInt();
    mys_FB.write(angle_FB,SP1);
    // 文字列出力
    lcd.print(angle_FB);       // 文字の表示
    lcd.print("&");            // 文字の表示
        
    // グーパー
    GP = Serial.readStringUntil('\n');
    if(GP.compareTo("1") == 0) {        // グーの時
       mys_GP.write(90,SP1,true); 
       lcd.print("gpk"); 
    }else if (GP.compareTo("0") == 0){  // パーの時
       mys_GP.write(0,SP1,true); 
       lcd.print("ppk");
    }
         
    lcd.print(GP);              // 文字の表示
    
        
  }else{                        //データがない時下記の初期値を与えている
    cnt += 1;
    if(cnt > 100000){
      flag = 0;
    }
    if (flag == 0){
      mys_LR.write(90);
      mys_UD.write(0);
      mys_FB.write(180);
    }
  }
}
