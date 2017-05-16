using UnityEngine;
using System.Collections;
using UniRx;

public class MessageTest : MonoBehaviour {

    // Subject作成
    Subject<string> subject = new Subject<string>();

	void Start () {
        operatorTest();
	}

    private void test1(){
        // subjectに登録
        subject
            .Subscribe(msg => Debug.Log("Subscribe1:" + msg));

        subject
            .Subscribe(msg => Debug.Log("Subscribe2:" + msg));

        subject
            .Subscribe(msg => Debug.Log("Subscribe3:" + msg));

        // イベントメッセージ発行
        subject
            .OnNext("TEST1");
        
        subject
            .OnNext("TEST2");
    }

    private void test2(){
        // Subscribe省略
        subject
            .Subscribe(
                msg => Debug.Log("Subscribe4:" + msg),
                error => Debug.Log("error"));
        
        subject
            .Subscribe(
                msg => Debug.Log("Subscribe5:" + msg),
                () => Debug.Log("complete1"));
        
        subject
            .Subscribe(
                msg => Debug.Log("Subscribe6:" + msg),
                error => Debug.Log("error"),
                () => Debug.Log("complete2"));

        // イベントメッセージ発行
        subject.OnCompleted();
    }

    private void operatorTest(){
        subject
            .Where(x => x == "Enemy") // 条件式に合う物を通す
            .Subscribe(x => Debug.Log(string.Format("プレイヤーが{0}に衝突", x)));

        subject.OnNext("Enemy");
        subject.OnNext("Wall");
    }
}
