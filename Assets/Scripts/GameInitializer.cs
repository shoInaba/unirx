using UnityEngine;
using System.Collections;
using UniRx;

public class GameInitializer : MonoBehaviour {

    // イベント完了を通知したいが中身はなんでも良い時に使う
    private Subject<Unit> initializedSubject = new Subject<Unit>();

    // 初期化通知処理
    public IObservable<Unit> OnInitializedAsync{
        get { return initializedSubject; }
    }

    void Start(){
        StartCoroutine(GameInitializeCoroutine());
        OnInitializedAsync
            .Subscribe(_ => Debug.Log("complete"));
    }

    private IEnumerator GameInitializeCoroutine(){

        // 初期化処理を行なう

        yield return null;

        // 初期化終了処理を通知
        initializedSubject.OnNext(Unit.Default);
        initializedSubject.OnCompleted();
    }
}
