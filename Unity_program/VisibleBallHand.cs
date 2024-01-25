using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;
using System.Collections.Generic;
using UnityEngine.UI;
 
public class VisibleBallHand : MonoBehaviour
{  
    // このプログラムは手の座標データを取得するためのプログラム
 
    [SerializeField]
    GameObject m_ProviderObject;
 
    LeapServiceProvider m_Provider;
 
    public Text scoreText; // 取得した手のトラッキングデータをUIに表示
 
    public GameObject Apple;
 
    void Start(){
    {
        m_Provider = m_ProviderObject.GetComponent<LeapServiceProvider>();
    }
 
    void Update()
    {
        Frame frame = m_Provider.CurrentFrame;
 
        // 手のデータを格納する変数をnullで初期化
        Hand rightHand = null;
        Hand leftHand = null;
 
        // 手のモデル情報を右手と左手の変数にそれぞれ格納
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                rightHand = hand;
            }
            if (hand.IsLeft)
            {
                leftHand = hand;
            }
        }
       
        // 両手のデータが取れなかった時---------------------------------
        if (rightHand == null && leftHand == null)
        {
            return;
        }
 
 
        // 法線、方向、位置ベクトル情報を入れるための変数の定義
        Vector right_normal;
        Vector right_direction;
        Vector right_position;
        Vector left_normal;
        Vector left_direction;
        Vector left_position;
 
 
        // 両手のデータが取れた時---------------------------------------
        if (rightHand != null && leftHand != null)
        {
            right_normal = rightHand.PalmNormal;
            right_direction = rightHand.Direction;
            right_position = rightHand.PalmPosition;
            left_normal = leftHand.PalmNormal;
            left_direction = leftHand.Direction;
            left_position = leftHand.PalmPosition;
            scoreText.text = "右手の法線ベクトル: " + right_normal + "\n" +
                             "右手の方向ベクトル: " + right_direction + "\n" +
                             "右手の位置ベクトル: " + right_position + "\n" +
                             "左手の法線ベクトル: " + left_normal + "\n" +
                             "左手の方向ベクトル: " + left_direction + "\n" +
                             "左手の位置ベクトル: " + left_position;
 
                             
                             
        }
 
        // 右手のデータだけ取れた時----------------------------------------
        if (rightHand != null && leftHand == null)
        {
            right_normal = rightHand.PalmNormal;
            right_direction = rightHand.Direction;
            right_position = rightHand.PalmPosition;
            scoreText.text = "右手の法線ベクトル: " + right_normal + "\n" + ":" +
                             "右手の方向ベクトル: " + right_direction + "\n" + ":" +
                             "右手の位置ベクトル: " + right_position;
        }
       
        // 左手のデータだけ取れた時-----------------------------------------
        if (rightHand == null && leftHand != null)
        {
            left_normal = leftHand.PalmNormal;
            left_direction = leftHand.Direction;
            left_position = leftHand.PalmPosition;
            scoreText.text = "左手の法線ベクトル: " + left_normal + "\n" + ":" +
                             "左手の方向ベクトル: " + left_direction + "\n" + ":" +
                             "左手の位置ベクトル: " + left_position;
        }
    }
}
}

