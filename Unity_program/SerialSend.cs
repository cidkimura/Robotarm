using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 
public class SerialSend : MonoBehaviour
{
    // ここでオブジェクト参照のためのフィールドを作成
    public SerialHandler serialHandler;
    public VisibleBallHand visivleballhand;
    public HandRSP handrsp;




    // 前後の関数で計算した本体上部と下部のモーターの角度を格納するグローバル変数
    public float FB_UD = 0;
    public string FB = "null";




    // VisibleBallHandファイルから手のモデルの座標データを取得する関数----------------------------------------------------------------------------
    // 引数に渡す数値によって法線、方向、位置のどれを取得するかを選択できる
    public float[] CoordinateGet(string reFlag)
    {
        // 約1秒ごとに実行
        // 位置座標の情報を取得
        // 座標データを取得 取れるデータ例↓
        // 手の法線ベクトル: (0.1,0.2,0.3):
        // 手の方向ベクトル: (0.1,0.2,0.3):
        // 手の位置ベクトル: (0.1,0.2,0.3):
        string handData = visivleballhand.scoreText.text;
 
        // 法線、方向、位置ごとの配列の定義
        float[] dataNormal = new float[3];
        float[] dataDirection = new float[3];
        float[] dataPosition = new float[3];
 
        // リターン用変数
        float[] ansArr = new float[3];
 
        // 文字と数値部分を分割
        //temp_arr = [右手の法線ベクトル],[(0.1,0.2,0.3)],[右手の方向ベクトル],[(0.1,0.2,0.3)],[右手の位置ベクトル],[(0.1,0.2,0.3)]
        string[] tempArr = handData.Split(':');
 
        // 数値のみを取り出した配列を定義
        // handDtaaArr = [(0.1,0.2,0.3)], [(0.1,0.2,0.3)], [(0.1,0.2,0.3)]
        string[] handDtaaArr = new string[] { tempArr[1], tempArr[3], tempArr[5] };
 
        // ()の取り外し
        // handDtaaArr = [0.1,0.2,0.3], [0.1,0.2,0.3], [0.1,0.2,0.3]
        for (int i = 0; i < handDtaaArr.Length; i++)
        {
            string tempJoky = handDtaaArr[i].Replace('(', ',');
            handDtaaArr[i] = tempJoky.Replace(')', ',');
        }
 
        // 数値を個々に分割し、double型に変換して法線、方向、位置のそれぞれの配列に格納
        // [空白], [0.1], [0.2], [0.3], [空白]
        string[] tempSplit0 = handDtaaArr[0].Split(',');
        string[] tempSplit1 = handDtaaArr[1].Split(',');
        string[] tempSplit2 = handDtaaArr[2].Split(',');
 
        // 三つの数値をそれぞれの配列に分割して格納する
        // [0.1], [0.2], [0.3]
        for (int i = 1; i < 4; i++)
        {
            // string → floatに変換
            float temp_Normal = float.Parse(tempSplit0[i]);
            float temp_Direction = float.Parse(tempSplit1[i]);
            float temp_Position = float.Parse(tempSplit2[i]);
 
            /*
            法線、方向、位置の配列にそれぞれデータを格納
            格納時に任意の位で切り捨てをして、それを整数値にしている
            掛けたり割ったりしている数値が100の時はcm単位で座標数値が返ってくる
            */
            dataNormal[i - 1] = (Mathf.Floor(temp_Normal * 100) / 100) * 100;
            dataDirection[i - 1] = (Mathf.Floor(temp_Direction * 100) / 100) * 100;
            dataPosition[i - 1] = (Mathf.Floor(temp_Position * 100) / 100) * 100;
        }
 
        // 受け取った引数の文字列を使って法線、方向、位置のどれを返すか判定
        if (reFlag.Equals("Normal"))
        {
            ansArr = dataNormal;
        }
        else if (reFlag.Equals("Direction"))
        {
            ansArr = dataDirection;
        }
        else if (reFlag.Equals("Position"))
        {
            ansArr = dataPosition;
        }
 
        return ansArr;
    }
   
    // HandRSPファイルから手がグーチョキパーのどの状態にあるのかを取得する--------------------------------------------------------------
    public string RSPGet()
    {
         string str_gupa = handrsp.rsp.ToString();
         return str_gupa;
    }
 
 
    // 受け取った引数のfloat値をstring型に変換して返す関数----------------------------------------------------------------------------
    public string changeText(float floatNum)
    {
        // 小数点を切り捨て
        float temp_math = Mathf.FloorToInt(floatNum);
        // 文字列に変換
        string str = temp_math.ToString();
 
        return str;
    }
 
    //　左右の角度を算出する関数(LR = Left and Rightの意味)--------------------------------------------------------------------------------
    public string getAngle_LR(float position_LR)
    {
        // 左右の角度の基準となる数値を格納(左右の場合は中心となる0度)
        float baseAngle = 0;
        // 中心からどれだけ傾いているかを知るために絶対値を取得
        float absoluteNum = (float)Math.Abs(position_LR);
        // 傾き(左右にどれだけ傾いているか)の角度を算出
        float tilt = 3 * absoluteNum;
        // 座標数値の符号を判定して、足すか引くか(右回転か左回転か)を選択する
        if (position_LR > 0) {          // 正の数値
            baseAngle = 90 - tilt;
        } else if (position_LR < 0) {   // 負の数値
            baseAngle = 90 + tilt;
        } else {                    // 0の場合
            baseAngle = 90;
        }
 
 
        // 角度が0~180度の範囲外になっていれば調整
        if (baseAngle > 180) {
            baseAngle = 180;
        }  else if (baseAngle < 0) {
            baseAngle = 0;
        }
        // 算出した角度を返す
        return changeText(baseAngle);
 
    }
    // 手の前後の動きに対応してアームを動かすための本体上部と下部モータの角度算出関数
    public void getAngle_FBandUD(float position_FB)
    {
        // 基準となるサーボモーターの角度を格納
        float baseAngle_UD = 0;
        float baseAngle_FB = 90;
        // 本体からどれだけ前に進んでいるかを知るために座標数値の絶対値を取得(今回は無し)
        // float absoluteNum = (float)Math.Abs(position_FB);
        // 傾きを算出
        float tilt_UD = 3 * position_FB;
        float tilt_FB = 3 * position_FB;
        // 奥行を算出
        baseAngle_UD += tilt_UD;
        baseAngle_FB += tilt_FB;


        // ここに範囲外の数値になった時の調整処理を加える
        if (baseAngle_UD >= 90) {
            baseAngle_UD = 90;
        }
        if (baseAngle_UD < 30) {
            baseAngle_UD = 30;
        }


        if (baseAngle_FB >= 180) {
            baseAngle_FB = 180;
        }
        if (baseAngle_FB < 60) {
            baseAngle_FB = 60;
        }
       
        // 算出した角度を返す(この関数に関してはreturnではなく、予め用意しておいた変数に格納する方式)
        FB_UD = baseAngle_UD;
        FB = changeText(baseAngle_FB);
    }  



    //　アーム下部のサーボモータ(上下)の角度を算出する関数(UD = Up and Downの意味)--------------------------------------------------------------------
    public float getAngle_UD(float position_UD)
    {
        // 基準となるサーボモーターの角度を格納(上下の場合は水平方向を向いている状態の90度)
        float baseAngle = 180;
        // 水平方向からどれだけ上に傾いているかを知るために絶対値を取得
        float absoluteNum = (float)Math.Abs(position_UD);
        // 傾きを算出
        float tilt = 2 * absoluteNum;
        // 高さの計算
        baseAngle -= tilt;
 
        // ここに範囲外の数値になった時の調整処理を加える
        if(baseAngle >= 170){
            baseAngle = 170;
        }
        if(baseAngle < 0){
            baseAngle = 0;
        }
        // 算出した角度を返す
        return baseAngle;
       
    }
 
    // カウント用変数
    int i = 0;
 
    void FixedUpdate() //ここはTimeの数値(単位：秒)ごとに実行されるループ文
    {
        i = i + 1;   //iを加算していって任意の秒数ごとに"1"のシリアル送信を実行
        if (i > 100)
        {  
            // 座標データを配列に格納
            float[] data = CoordinateGet("Position");


            // 各モーターの角度を算出
            // 左右のモーター
            string LR = getAngle_LR(data[0]);

            // 上下のモーター
            float UD = getAngle_UD(data[1]);

            // 前後のロジックによる上下と前後のモーター
            getAngle_FBandUD(data[2]);


            // 前後のロジックと上下のロジックで算出したUDの数値の調整
            // 最終的に大きい数値を算出した方を優先させる
             float lastUD = UD - (90 - FB_UD);
            // // もし前後ロジックで算出した上下数値の方が大きかったらそっちに入れ替える

           
            Debug.Log(data[2]);
    

            // Arduinoにシリアル通信でデータを送信
            serialHandler.Write(LR);
            serialHandler.Write("\n");
            serialHandler.Write(changeText(lastUD));
            serialHandler.Write("\n");
            serialHandler.Write(FB);
            serialHandler.Write("\n");
            string gupa = RSPGet();
            if ( gupa == "Rock" ) {
                serialHandler.Write("1");
                serialHandler.Write("\n");
            }else{
                serialHandler.Write("0");
                serialHandler.Write("\n");
            }
            
 
            // ー－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－ー－－－－－－ーーーー－
 
            i = 0;
        }
    }
}



