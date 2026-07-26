using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// このスクリプトが付いたButtonにアタッチする
// マウスでのクリックは無効化し、ゲームパッドのAボタン(South)は
// 「現在選択中のボタン」の時だけOnClickを実行する
[RequireComponent(typeof(Button))]
public class GamepadButtonTrigger : MonoBehaviour, IPointerClickHandler
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

        // 自分が今EventSystem上で選択されているボタンでなければ何もしない
        //if (EventSystem.current.currentSelectedGameObject != gameObject) return;

        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            button.onClick.Invoke();
        }
    }

    // マウスでクリックされた時に呼ばれるイベントを、あえて何もしないことで無効化する
    public void OnPointerClick(PointerEventData eventData)
    {
        // 何もしない = マウスクリックを無視
    }
}