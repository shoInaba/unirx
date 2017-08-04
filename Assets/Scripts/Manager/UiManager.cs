using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class UiManager : MonoBehaviour {

    [Header("Base")]
    public ImagePosition basePanel;
    public IObservable<Vector2> baseChanged { get; private set; }

    /// <summary> UIビルド </summary>
    public IEnumerator Build(){
        yield return null;
    }
}
