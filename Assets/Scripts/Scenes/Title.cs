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
        // ボタンタップイベント、シーン遷移開始
        StartButton.OnClickAsObservable()
            .Subscribe(_ => StartCoroutine(Move()));

        // フェード
        yield return StartCoroutine(SceneStack.Open());
        SceneStack.SetActive(true);
    }

    private IEnumerator Move(){
        // ボタンをfalseにすると選択できなく出来る
        StartButton.interactable = false;

        // フェード
        SceneStack.SetActive(false);
        yield return StartCoroutine(SceneStack.Close());

        // シーン遷移
        SceneStack.MoveScene("InGame");
    }
}
