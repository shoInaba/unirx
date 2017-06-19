using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

/// <summary> UniRx.Triggers系 Unityのコールバックイベントを変換して提供してくれている </summary>
public class TriggersSample : MonoBehaviour {
    
	void Start () {
        var isForceEnabled = false;
        var rigidBody = GetComponent<Rigidbody>();

        // 毎フレーム呼ばれる
        this.FixedUpdateAsObservable()
            .Where(_ => isForceEnabled)
            .Subscribe(_ => rigidBody.AddForce(Vector3.up * 100.0f));

        this.FixedUpdateAsObservable()
            .Subscribe(_ => Debug.Log("isForceEnabled:" + isForceEnabled));
        
        // WarpZoneに侵入したらフラグ有効にする
        this.OnTriggerEnterAsObservable()
            .Where(x => x.gameObject.tag == "WarpZone")
            .Subscribe(_ => isForceEnabled = true);
        
        this.OnTriggerExitAsObservable()
            .Where(x => x.gameObject.tag == "WarpZone")
            .Subscribe(_ => isForceEnabled = true);
	}
}
