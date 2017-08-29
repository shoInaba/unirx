using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

//*************************************************************************************************
/// <summary>
/// 攻撃ボタン処理
/// </summary>
//*************************************************************************************************
public class AttackButton : MonoBehaviour {

    public Button attackButton; // 攻撃ボタン
    public Text scoreText; // スコアテキスト
    private int score = 0; // スコア

	// Use this for initialization
	void Start () {
        attackButton.OnClickAsObservable()
            .Subscribe(_ => UpdateAttackLog());
	}

    //*************************************************************************************************
    /// <summary>
    /// 攻撃ログ生成
    /// </summary>
    //*************************************************************************************************
    private void UpdateAttackLog(){
        score++;
        scoreText.text = "Score:" + score.ToString();
    }
}
