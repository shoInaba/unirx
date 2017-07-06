﻿using UnityEngine;
using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class MessageTest : MonoBehaviour {

    // Subject作成
    Subject<string> subject = new Subject<string>();

	void Start () {
        test14();
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

    // Disposeテスト
    private void test6(){
        var subject6 = new Subject<int>();

        var disposable = subject6
            .Subscribe(x => Debug.Log(x), () => Debug.Log("OnCompleted"));

        subject6.OnNext(1);
        subject6.OnNext(2);
        disposable.Dispose();

        subject6.OnNext(3);
        subject.OnCompleted();

    }

    // 指定のSubscribeだけ停止する
    private void test7(){
        var subject7 = new Subject<int>();

        var disposable1 = subject7.Subscribe(x => Debug.Log("ストリーム1:" + x), () => Debug.Log(" OnCompleted"));
        var disposable2 = subject7.Subscribe(x => Debug.Log("ストリーム2:" + x), () => Debug.Log(" OnCompleted"));

        subject7.OnNext(1);
        subject7.OnNext(2);

        disposable1.Dispose();

        subject7.OnNext(3);
        subject7.OnCompleted();
    }

    // ReactivePropertyシリーズのテスト
    private void test8() {
        var rp = new ReactiveProperty<int>(10); // 初期値を指定出来る

        // 代入や値を読み取れる
        rp.Value = 20;
        var currentValue = rp.Value;

        // Subscribeも可能
        rp.Subscribe(x => Debug.Log(x));

        // 値を書き換えるとOnNextが発行される
        rp.Value = 30;
    }

    // インスペクター上で値を変えてもログが出る
    [SerializeField] private IntReactiveProperty playerHealth = new IntReactiveProperty(100);
    private void test9(){
        playerHealth.Subscribe(x => Debug.Log(x));
    }

    // ReactiveCollection 状態変化を通知する機能が搭載したList<T>
    private void test10(){
        var collection = new ReactiveCollection<string>();

        // リストに追加した時に呼ばるイベント登録
        collection
            .ObserveAdd()
            .Subscribe(x => {
                Debug.Log(string.Format("Add [{0}] = {1}", x.Index, x.Value));
            });

        // リストから削除された時のイベント登録
        collection
            .ObserveRemove()
            .Subscribe(x => {
                Debug.Log(string.Format("Remove [{0}] = {1}", x.Index, x.Value));    
            });

        collection.Add("Apple");
        collection.Add("Baseball");
        collection.Add("Cherry");
        collection.Remove("Apple");
    }

    // UpdateAsObservableテスト
    private void test11(){
        // UpdateAsObservableはComponectに対する拡張メソッドとして定義されるいるので
        // 呼び出す際はthisが必要
        // UpdateAsObservableは破棄された際に自動的にOnCompletedが呼ばれる
        this.UpdateAsObservable()
            .Subscribe(
                _ => Debug.Log("Update"),
                () => Debug.Log("OnCompleted")
            );

        this.OnDestroyAsObservable()
            .Subscribe(_ => Debug.Log("Destroy"));

        Destroy(gameObject, 1.0f);
    }

    // ObservableEveryUpdateを使う
    private void test12(){
        // 破棄された場合に自らOnCompletedを発行しない
        // 寿命管理には気をつける事
        // メリット: シングルトン上で動作するのでゲーム進行中に存在し続けるストリームを作れる
        // MainThreadDispatcherが生成されるので消したらダメ！
        Observable.EveryUpdate()
            .Subscribe(_ => Debug.Log("Update"));
    }

    // Updateサンプル
    [SerializeField] float intervalSeconds = 0.25f;
    private void test13(){
        this.UpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.Z))
            .ThrottleFirst(TimeSpan.FromSeconds(intervalSeconds)) // 値が来たら一定時間ストリーム遮断
            .Subscribe(_ => Attack());
    }

    private void Attack(){
        Debug.Log("Attack");
    }

    // UniRxとコルーチンサンブル
    private void test14(){
        
        // コルーチン終了タイミングを待つ処理
        // 1:コルーチン 2:yieldしたタイミングでOnNextするか(bool)
        // Trueの場合はコルーチン開始時にOnNextも一度走る
        // falseの場合はコルーチン終了後に一度だけ走る

        // var disposable = 
        Observable.FromCoroutine(token => test14Coroutine(token), publishEveryYield: false)
            .Subscribe(
                _ => Debug.Log("OnNext"),
                () => Debug.Log("OnComplered")
            ).AddTo(gameObject);

        // DisposeするとCancellationTokenのトークンがtrueになる
        // disposable.Dispose();
    }

    // MEMO: Subscribeする度にコルーチンを生成する
    //       SubscribeをDisposeするとコルーチンは自動的に停止する
    private IEnumerator test14Coroutine(CancellationToken token){
        Debug.Log("Coroutine started.");
        Debug.LogWarning("CancellationToken:" + token.IsCancellationRequested);
        yield return new WaitForSeconds(3);
        Debug.Log("Coroutine finished.");
    }



}
