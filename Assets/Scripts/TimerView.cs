using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class TimerView : MonoBehaviour {
    [SerializeField] private TimeCounter timeCounter;
    [SerializeField] private Text counterText; // uGUIテキスト

    void Start(){
        // タイマーのカウンタが変化したイベントを受けuGUIText更新
        timeCounter.OnTimeChanged.Subscribe(time => {
            // 現在のタイマー値をUIに表示
            counterText.text = time.ToString();
        });
    }
}