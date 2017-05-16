using UnityEngine;
using System.Collections;
using UniRx;

public class TimeCounter : MonoBehaviour {

    // イベントを発行する核となるインスタンス
    private Subject<int> timerSubject = new Subject<int>();

    // イベントの購読側のみ公開
    public IObservable<int> OnTimeChanged { 
        get { 
            return timerSubject; 
        }
    }

    void Start(){
        StartCoroutine(TimerCoroutine());
    }

    //*************************************************************************************************
    /// <summary>
    /// タイマーカウントのコルーチン
    /// </summary>
    //*************************************************************************************************
    IEnumerator TimerCoroutine(){
        var time = 100;
        while(time > 0) {
            time--;

            // イベント発行
            timerSubject.OnNext(time);

            // 1秒待機
            yield return new WaitForSeconds(1);
        }
    }
}
