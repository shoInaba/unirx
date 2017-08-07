using UnityEngine;
using System.Collections;
using UniRx;

//*************************************************************************************************
/// <summary>
/// インゲームに関する処理
/// </summary>
//*************************************************************************************************
public class InGame : MonoBehaviour {

    // イベント完了通知 (中身が何でも良い場合はUnitを使用)
    private Subject<Unit> initializedSubject = new Subject<Unit>();
    public IObservable<Unit> OnInitializedAsync{
        get { return initializedSubject; }
    }

    IEnumerator Start(){
        // シーン遷移完了
        yield return StartCoroutine(SceneStack.Open());
        SceneStack.SetActive(true);

        // 初期化処理開始
        StartCoroutine(GameInitializeCoroutine());

        // 初期化処理完了時の処理 (ゲーム開始など)
        OnInitializedAsync
            .Subscribe(_ => Debug.Log("completed."));
    }

    //*************************************************************************************************
    /// <summary>
    /// インゲーム初期化処理
    /// </summary>
    //*************************************************************************************************
    private IEnumerator GameInitializeCoroutine(){
        // 初期化処理
        yield return null;

        // 初期化処理完了
        initializedSubject.OnNext(Unit.Default);
        initializedSubject.OnCompleted();
    }
}
