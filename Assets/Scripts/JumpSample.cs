using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class JumpSample : MonoBehaviour {
    private CharacterController characterController;

    // ジャンプ中フラグ
    private BoolReactiveProperty isJumping = new BoolReactiveProperty();

    void Start(){
        characterController = GetComponent<CharacterController>();

        // ジャンプ中でなければ移動
        this.UpdateAsObservable()
            .Where(_ => !isJumping.Value)
            .Select(_ => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")))
            .Where(x => x.magnitude > 0.1f) // ベクトルの長さを返す
            .Subscribe(x => Move(x.normalized));

        // ジャンプ中でない場合ジャンプ
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space) && !isJumping.Value && characterController.isGrounded)
            .Subscribe(_ => {
                Jump();
                isJumping.Value = true;
            });

        // 着地フラグが変化した時にジャンプ中フラグ戻す
        characterController
            .ObserveEveryValueChanged(x => x.isGrounded)
            .Where(x => x && isJumping.Value)
            .Subscribe(_ => isJumping.Value = false)
            .AddTo(gameObject);

        // ジャンプ中フラグがfalseになったら効果音を鳴らす
        isJumping.Where(x => !x)
            .Subscribe(_ => PlaySoundEffect());
    }

    private void Jump(){
        Debug.Log("Jump");
    }

    private void PlaySoundEffect(){
        Debug.Log("Sound");
    }

    private void Move(Vector3 direction){
        Debug.Log("Move");
        transform.position += direction * Time.deltaTime;
    }
}
