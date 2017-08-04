using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class InGame : MonoBehaviour {

    IEnumerator Start(){
        yield return StartCoroutine(SceneStack.Open());
        SceneStack.SetActive(true);
    }
}
