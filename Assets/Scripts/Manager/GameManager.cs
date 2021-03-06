﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

//*************************************************************************************************
/// <summary>
/// インゲームマネージャー
/// </summary>
//*************************************************************************************************
public class GameManager : MonoBehaviour {
   
    UiManager ui;

    //*************************************************************************************************
    /// <summary>
    /// 初期化
    /// </summary>
    //*************************************************************************************************
    IEnumerator Start(){
        yield return StartCoroutine(Build());
    }

    //*************************************************************************************************
    /// <summary>
    /// ビルド
    /// </summary>
    //*************************************************************************************************
    IEnumerator Build(){
        // UIマネージャービルド
        ui = GameObject.FindObjectOfType<UiManager>();
        yield return StartCoroutine(ui.Build());
    }
    /*
    //*************************************************************************************************
    /// <summary>
    /// インゲーム開始
    /// </summary>
    //*************************************************************************************************
    IEnumerator StartGame(){
    }

    //*************************************************************************************************
    /// <summary>
    /// インゲーム結果
    /// </summary>
    //*************************************************************************************************
    IEnumerator Result(){
    }
    */
}
