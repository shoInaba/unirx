using UnityEngine;
using System;
using System.Collections;
using UniRx;

public class MessageTest : MonoBehaviour {

    // Subject作成
    Subject<string> subject = new Subject<string>();

	void Start () {
        test5();
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

    // オペレーターテスト
    private void operatorTest(){
        subject
            .Where(x => x == "Enemy") // 条件式に合う物を通す
            .Subscribe(x => Debug.Log(string.Format("プレイヤーが{0}に衝突", x)));

        subject.OnNext("Enemy");
        subject.OnNext("Wall");
    }

    // イベントタイミングを通知するメッセージテスト
    private void test3(){
        var subjectUnit = new Subject<Unit>();

        subjectUnit
            .Subscribe(x => Debug.Log(x));

        subjectUnit.OnNext(Unit.Default);
    }

    // OnErrorの動作テスト
    // エラーに入った場合はその後の処理は行われない
    private void test4(){
        subject
            .Select(str => int.Parse(str))
            .Subscribe(
                x => Debug.Log("成功:" + x), 
                ex => Debug.Log("例外発生:" + ex)
            );

        subject.OnNext("1");
        subject.OnNext("2");
        subject.OnNext("AAAA");
        subject.OnNext("4");
        subject.OnCompleted();
    }

    // OnError発生後も再購読する処理
    // エラーが発生するとErrorRetryでストリームを再構築して続きを購読する
    private void test5(){
        subject
            .Select(str => int.Parse(str))
            .OnErrorRetry((FormatException ex) => { 
                Debug.Log("例外発生、再購読します");
            })
            .Subscribe(x => Debug.Log("成功:" + x), ex => Debug.Log("例外発生:" + ex));

        subject.OnNext("1");
        subject.OnNext("2");
        subject.OnNext("AAAA");
        subject.OnNext("4");
        subject.OnNext("5");
        subject.OnCompleted();
    }
}
