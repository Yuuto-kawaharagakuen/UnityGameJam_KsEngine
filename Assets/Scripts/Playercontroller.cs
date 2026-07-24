using UnityEngine;
using UnityEngine.InputSystem;

// 必要コンポーネント: CharacterController
// 操作: ゲームパッド専用(左スティック=移動, Aボタン(South)=スライド)
// しゃがみ/隠れシステムは廃止。Aボタンでスライドに置き換え
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 4f;
    public float rotateSpeed = 10f;
    public float gravity = -9.81f;

    [Header("スライド設定")]
    [Tooltip("スライド中の速度倍率(通常速度に対して)")]
    public float slideSpeedMultiplier = 1.2f;
    [Tooltip("スライド中、1フレームあたりスティック方向へ何度まで向きを寄せるか")]
    public float slideTurnDegreesPerFrame = 1f;

    [Header("スライド中の当たり判定")]
    [Tooltip("スライド中のCharacter Controllerの高さ(見た目の低い姿勢に合わせる)")]
    public float slideHeight = 0.9f;
    [Tooltip("スライド中のCharacter Controllerの中心Y座標")]
    public float slideCenterY = 0.45f;
    [Tooltip("高さ・中心を切り替える速さ(大きいほど瞬時に切り替わる)")]
    public float heightLerpSpeed = 12f;
    [Tooltip("しゃがみ姿勢から通常姿勢へ戻る時の速さ(こちらは別枠、遅く感じる場合はここを上げる)")]
    public float standReturnLerpSpeed = 20f;
    [Tooltip("Slide_Midの再生がこの割合(0〜1)まで進んだら、見た目より先に当たり判定を元の大きさへ戻し始める")]
    [Range(0f, 1f)]
    public float collisionRestoreAtSlideMidProgress = 0.7f;

    [Header("参照")]
    [Tooltip("ThirdPersonCameraが付いているTransform。移動方向の基準にする")]
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator; // あれば自動取得。なくてもエラーにならない
    private float verticalVelocity;
    private Vector3 slideDirection;



    // 通常時の高さ・中心(Awakeで記憶しておき、スライド後に元へ戻す)
    private float standHeight;
    private Vector3 standCenter;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        standHeight = controller.height;
        standCenter = controller.center;
    }

    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null) return; // ゲームパッドが無ければ何もしない

        // --- 今、スライドアニメーション再生中かどうか(Animatorのステート名で判定、移動ロック用) ---
        bool isSliding = false;
        // 当たり判定を「低い姿勢」として維持すべきかどうか(移動ロックとは別の判定)
        bool isLowProfile = false;

        if (animator != null)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            isSliding = state.IsName("Slide_Start") || state.IsName("Slide_Mid") || state.IsName("Slide_End");

            if (state.IsName("Slide_Start"))
            {
                isLowProfile = true;
            }
            else if (state.IsName("Slide_Mid"))
            {
                // Slide_Midの再生位置が指定の割合に達するまでは低い当たり判定のまま
                float progress = state.normalizedTime % 1f;
                isLowProfile = progress < collisionRestoreAtSlideMidProgress;
            }
            // Slide_End、LocomotionではisLowProfileはfalse(=通常の当たり判定に戻る)
        }

        // --- 当たり判定(高さ・中心)を低い姿勢に合わせて縮める/戻す ---
        float targetHeight = isLowProfile ? slideHeight : standHeight;
        Vector3 targetCenter = isLowProfile ? new Vector3(standCenter.x, slideCenterY, standCenter.z) : standCenter;
        float currentLerpSpeed = isLowProfile ? heightLerpSpeed : standReturnLerpSpeed;

        // MoveTowardsなら、Lerpと違って最後まできっちり目標値に到達する(近づくほど遅くならない)
        controller.height = Mathf.MoveTowards(controller.height, targetHeight, currentLerpSpeed * Time.deltaTime);
        controller.center = Vector3.MoveTowards(controller.center, targetCenter, currentLerpSpeed * Time.deltaTime);

        // --- スティック入力は常に読む(スライド中の操舵にも使うため) ---
        Vector2 rawInput = gamepad.leftStick.ReadValue();
        Vector3 moveInput = new Vector3(rawInput.x, 0f, rawInput.y);

        Vector3 camForward = cameraTransform.forward; camForward.y = 0f; camForward.Normalize();
        Vector3 camRight = cameraTransform.right; camRight.y = 0f; camRight.Normalize();

        // --- スライド開始 (Aボタン、移動中のみ、スライド中は多重発動しない) ---
        if (!isSliding && gamepad.buttonSouth.wasPressedThisFrame && moveInput.sqrMagnitude > 0.01f)
        {
            slideDirection = (camForward * moveInput.z + camRight * moveInput.x).normalized;

            if (animator != null)
                animator.SetTrigger("Slide");
        }

        if (isSliding)
        {
            // スティックが倒れていれば、その方向へ毎フレーム少しずつ向きを寄せる
            if (moveInput.sqrMagnitude > 0.01f)
            {
                Vector3 desiredDir = (camForward * moveInput.z + camRight * moveInput.x).normalized;
                slideDirection = Vector3.RotateTowards(
                    slideDirection,
                    desiredDir,
                    slideTurnDegreesPerFrame * Mathf.Deg2Rad,
                    0f
                ).normalized;
            }

            controller.Move(slideDirection * moveSpeed * slideSpeedMultiplier * Time.deltaTime);

            // 見た目もスライド方向へ向ける
            transform.rotation = Quaternion.LookRotation(slideDirection);
        }
        else if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 moveDir = camForward * moveInput.z + camRight * moveInput.x;

            controller.Move(moveDir * moveSpeed * Time.deltaTime);

            // 進行方向へ体を向ける
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }

        // --- 重力 ---
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -1f;
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        // Animatorがあれば移動速度を渡す(Idle/Walk/Runの切り替え用)
        if (animator != null)
        {
            animator.SetFloat("Speed", moveInput.magnitude);
        }
    }
}