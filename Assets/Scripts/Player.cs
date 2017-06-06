using UnityEngine;
using System.Collections;
using UniRx;

//*************************************************************************************************
/// <summary>
/// プレイヤークラス
/// </summary>
//*************************************************************************************************
public class Player : MonoBehaviour {

    [SerializeField] private TimeCounter timeCounter;
    private float moveSpeed = 10.0f;

	void Start () {
        // タイマーが0になったら初期座標へ戻る
        timeCounter.OnTimeChanged
            .Where(x => x == 0)
            .Subscribe(_ => transform.position = Vector3.zero)
            .AddTo(this.gameObject); // gameObjectがDestoryした時に自動的にDisposeを呼ぶ
	}
	
	void Update () {
        // 右押したら移動
        if(Input.GetKey(KeyCode.RightArrow)) {
            transform.position += new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;
        }

        // 画面外で削除
        if(transform.position.x > 10){
            Debug.Log("GameOver");
            Destroy(this.gameObject);
        }
	}
}
