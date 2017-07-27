using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

//*************************************************************************************************
/// <summary>
/// シーン読み込みクラス
/// </summary>
//*************************************************************************************************
public class SceneLoader : MonoBehaviour {
	void Start () {
        // 読み込まれてるシーン合計数が返される
        if(SceneManager.sceneCount == 1){
            SceneStack.MoveScene("Title");
        }
	}

    /// <summary> シーンの非同期読み込み </summary>
    public IEnumerator LoadSceneAsync(string name, bool activate){
        // SceneManager に追加されているシーンの中から、指定した名前のシーンを検索
        var scene = SceneManager.GetSceneByName(name);

        // シーンが読み込まれていない場合は読み込み
        if(!scene.isLoaded){
            // バックグラウンドで非同期にシーン読み込み
            yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            scene = SceneManager.GetSceneByName(name);
        }

        // シーンをアクティブにする
        if(activate){
            SceneManager.SetActiveScene(scene);
        }
    }

    /// <summary> シーンのアンロード </summary>
    private IEnumerator UnLoadScenes(){
        Debug.LogWarning("UnLoadScenes 1");
        // SceneLoader 以外をリスト化
        var scenes = Enumerable.Range(0, SceneManager.sceneCount) // シーン数分のインデックス取得
            .Select(i => SceneManager.GetSceneAt(i)) // SceneManager の追加されたシーンのリストから指定したインデックスのシーンを取得
            .Where(s => s.name != "SceneLoader")
            .ToArray();

        foreach(var s in scenes) {
            yield return SceneManager.UnloadScene(s);
        }
    }

    /// <summary> 指定シーンへ遷移 </summary>
    public IEnumerator MoveSceneAsync(string name){
        yield return null;
        yield return StartCoroutine(UnLoadScenes());

        yield return null;
        yield return StartCoroutine(LoadSceneAsync(name, true));
    }
}

//*************************************************************************************************
/// <summary>
/// シーンスタッククラス
/// </summary>
//*************************************************************************************************
public class SceneStack {
    public static SceneLoader Instance {
        get {
            if(instance == null){
                instance = GameObject.FindObjectOfType<SceneLoader>();
            }
            if(instance == null){
                // シーン読み込み(Additive:現在読み込まれているシーンに新たなシーン追加)
                SceneManager.LoadScene("SceneLoader", LoadSceneMode.Additive);
                instance = GameObject.FindObjectOfType<SceneLoader>();
            }
            return instance;
        }
    }
    static SceneLoader instance; // SceneLoader のインスタンス

    /// <summary> 非同期なシーン読み込み </summary>
    public static IEnumerator LoadSceneAsync(string name, bool activate = false){
        yield return Instance.StartCoroutine(Instance.LoadSceneAsync(name, activate));
    }

    /// <summary> シーン遷移 </summary>
    public static void MoveScene(string name){
        Instance.StartCoroutine(Instance.MoveSceneAsync(name));
    }

    /*
    /// <summary>  </summary>
    public static IEnumerator Open(){
    }

    /// <summary>  </summary>
    public static IEnumerator Close(){
    }

    /// <summary>  </summary>
    public static void SetActive(bool isActive){
        
    }
    */
}