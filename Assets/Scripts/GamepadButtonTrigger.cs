using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GamepadButtonTrigger : MonoBehaviour, IPointerClickHandler
{
    private Button button;

    // 直前に選択されていたボタンを覚えておく(複数のGamepadButtonTriggerで共有)
    private static GameObject lastSelected;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null) return;

        if (EventSystem.current.currentSelectedGameObject != gameObject) return;

        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            button.onClick.Invoke();
        }
    }

    void LateUpdate()
    {
        // 現在選択されているものがあれば覚えておく
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
        // マウスクリックなどで選択が外れてしまった場合、直前のボタンへ選択を戻す
        else if (lastSelected != null && lastSelected.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 何もしない = マウスクリックを無視
    }
}