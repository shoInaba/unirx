using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

//*************************************************************************************************
/// <summary>
/// タイトルクラス
/// </summary>
//*************************************************************************************************
public class Title : MonoBehaviour {
    public Button StartButton;

    IEnumerator Start(){
        // ボタンタップイベント
        StartButton.OnClickAsObservable()
            .Subscribe(_ => StartCoroutine(Move()));
        yield return null;
    }

    private IEnumerator Move(){
        // falseにすると選択できなくなる
        StartButton.interactable = false;

        // シーン遷移
        SceneStack.MoveScene("InGame");

        yield return null;
    }
}
