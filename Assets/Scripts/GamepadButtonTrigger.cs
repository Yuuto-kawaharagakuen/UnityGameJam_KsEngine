using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// このスクリプトが付いたButtonにアタッチする
// ゲームパッドのAボタン(South)を押すと、通常のクリックと同じOnClickが実行される
[RequireComponent(typeof(Button))]
public class GamepadButtonTrigger : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null) return;

        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            button.onClick.Invoke();
        }
    }
}