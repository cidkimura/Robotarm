using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System.Linq;

public class HandRSP : MonoBehaviour {
    private Controller controller;
    private Finger[] fingers;
    private bool[] isGripFingers;
    public enum RSP
    {
        Rock, Scissors, Paper
    };
    public RSP rsp;
    
    void Start () {
    	// Controllerクラスのインスタンス化
        controller = new Controller();
        
        // 配列の初期化
        fingers = new Finger[5];
        isGripFingers = new bool[5];
    }

    
    void Update () {
        Frame frame = controller.Frame();
        
        // １本以上指が検出されていれば処理に入る
        if(frame.Hands.Count != 0)
        {
            List<Hand> hand = frame.Hands;
            fingers = hand[0].Fingers.ToArray();
            isGripFingers = Array.ConvertAll(fingers, new Converter<Finger, bool>(i => i.IsExtended));
            
            // ログで5本の指がそれぞれTureかfalseかを出力
            //Debug.Log(isGripFingers[0]+","+ isGripFingers[1] + "," + isGripFingers[2] + "," + isGripFingers[3] + "," + isGripFingers[4]);
            
            
            // 検出できた指の本数を格納
            int extendedFingerCount = isGripFingers.Count(n => n == true);
            
            // 指の本数を判定し、グー、チョキ、パーのいずれかを変数に格納
            if(extendedFingerCount == 0)
            {
                rsp = RSP.Rock;
            }
            else if(extendedFingerCount < 4)
            {
                rsp = RSP.Scissors;

            }
            else
            {
                rsp = RSP.Paper;

            }

        }
    }
}