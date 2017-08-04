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
        
        yield return StartCoroutine(SceneStack.Open());
        SceneStack.SetActive(true);
    }

    private IEnumerator Move(){
        // falseにすると選択できなくなる
        StartButton.interactable = false;

        // 
        SceneStack.SetActive(false);
        yield return StartCoroutine(SceneStack.Close());

        // シーン遷移
        SceneStack.MoveScene("InGame");
    }
}
