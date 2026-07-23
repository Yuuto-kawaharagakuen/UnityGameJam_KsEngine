using UnityEngine;
using UnityEngine.InputSystem;

// 必要コンポーネント: CharacterController
// 正式操作: 左スティック=移動, Aボタン(South)=しゃがみ/隠れ トグル
// テスト用: WASD/矢印キー=移動, Spaceキー=しゃがみ/隠れ トグル(コントローラーが無くても動作確認できるように)
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 4f;
    public float crouchSpeedMultiplier = 0.5f;
    public float rotateSpeed = 10f;
    public float gravity = -9.81f;

    [Header("参照")]
    [Tooltip("ThirdPersonCameraが付いているTransform。移動方向の基準にする")]
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator; // あれば自動取得。なくてもエラーにならない
    private float verticalVelocity;
    private bool isHiding;

    // 監視カメラ側の検知スクリプトから参照する用
    public bool IsHiding => isHiding;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        var gamepad = Gamepad.current;

        // --- しゃがみ/隠れ (ゲームパッド: Aボタン / キーボード: Space、どちらもトグル) ---
        bool hideKeyPressed =
            (gamepad != null && gamepad.buttonSouth.wasPressedThisFrame);

        if (hideKeyPressed)
        {
            isHiding = !isHiding;
        }

        // --- 移動 (ゲームパッド: 左スティック / キーボード: WASD・矢印キー、カメラの向き基準) ---
        Vector2 input = gamepad != null ? gamepad.leftStick.ReadValue() : Vector2.zero;

        if (input.sqrMagnitude < 0.01f)
        {
            float x = 0f, y = 0f;
            input = new Vector2(x, y).normalized;
        }

        Vector3 moveInput = new Vector3(input.x, 0f, input.y);

        if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 camForward = cameraTransform.forward; camForward.y = 0f; camForward.Normalize();
            Vector3 camRight = cameraTransform.right; camRight.y = 0f; camRight.Normalize();
            Vector3 moveDir = camForward * moveInput.z + camRight * moveInput.x;

            float speed = moveSpeed * (isHiding ? crouchSpeedMultiplier : 1f);
            controller.Move(moveDir * speed * Time.deltaTime);

            // 進行方向へ体を向ける
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }

        // --- 重力 ---
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -1f;
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        // Animatorがあればしゃがみ状態を渡す(モデルの見た目切り替え用)
        if (animator != null)
        {
            animator.SetBool("IsHiding", isHiding);
            animator.SetFloat("Speed", moveInput.magnitude);
        }
    }
}


