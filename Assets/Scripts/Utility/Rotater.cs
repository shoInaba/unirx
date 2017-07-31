using UnityEngine;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

public class Rotater : MonoBehaviour {
    public Vector3 speed;

    void Start(){
        // OnComplete処理も必要
        this.UpdateAsObservable()
            .Subscribe(_ => transform.Rotate(speed * Time.deltaTime));
    }
}
