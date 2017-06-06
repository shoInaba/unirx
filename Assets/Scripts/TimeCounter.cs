using UnityEngine;
using System.Collections;
using UniRx;

public class TimeCounter : MonoBehaviour {
    
    [SerializeField] private int timeLife = 3; // タイマー
    private Subject<int> timerSubject = new Subject<int>(); // イベントを発行する核となるインスタンス

    // イベントの購読側のみ公開
    public IObservable<int> OnTimeChanged { 
        get { 
            return timerSubject; 
        }
    }

    void Start(){
        StartCoroutine(TimerCoroutine());

        timerSubject
            .Subscribe(x => Debug.Log(x));
    }

    //*************************************************************************************************
    /// <summary>
    /// タイマーカウントのコルーチン
    /// </summary>
    //*************************************************************************************************
    IEnumerator TimerCoroutine(){
        yield return null;

        var time = timeLife;
        while(time >= 0) {
            // イベント発行、1秒待機
            timerSubject.OnNext(time--);
            yield return new WaitForSeconds(1);
        }
        timerSubject.OnCompleted();
    }
}
