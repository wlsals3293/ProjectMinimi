using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_HUD : MonoBehaviour
{
    [SerializeField]
    private GameObject crossHair;

    //[SerializeField]
    //private GameObject[] life;

    [SerializeField]
    private GameObject behaviourDesc;

    [SerializeField]
    private Image[] keyDisplay;

    [SerializeField]
    private RectTransform joystickBackground;

    [SerializeField]
    private RectTransform joystickCenterCircle;

    private float joystickMoveRadius;


    private void Awake()
    {
        Rect joystickSize = joystickBackground.rect;
        joystickMoveRadius = joystickSize.width * 0.5f * 0.8f;
    }

    private void Update()
    {
        UpdateInputDisplay();
        UpdateJoystick();
    }


    public void Init()
    {
        VisibleCrossHair(false);
        //PlayerManager.Instance.PlayerChar.onHpChanged += SetLife;
    }

    public void VisibleCrossHair(bool isVisible)
    {
        crossHair.SetActive(isVisible);
    }

    public void SetLife(int amount)
    {
        /*for (int i = 0; i < 3; i++)
        {
            life[i].SetActive(i + 1 <= amount);
        }*/
    }

    public void SetBehaviourDesc(bool isActive)
    {
        if (behaviourDesc == null)
            return;

        behaviourDesc.SetActive(isActive);
    }

    private void UpdateInputDisplay()
    {
        SetKeyDisplay(0, Input.GetMouseButton(0));
        SetKeyDisplay(1, Input.GetMouseButton(1));

        SetKeyDisplay(2, Input.GetKey(KeyCode.Space));
        SetKeyDisplay(3, Input.GetKey(KeyCode.E));
    }

    private void UpdateJoystick()
    {
        Vector2 direction = new Vector2
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };
        direction.Normalize();
        direction *= joystickMoveRadius;

        joystickCenterCircle.anchoredPosition = direction;
    }

    private void SetKeyDisplay(int idx, bool isActive)
    {
        if (keyDisplay[idx] == null)
            return;

        Color btnColor = isActive ? Color.gray : Color.white;

        keyDisplay[idx].color = btnColor;
    }

    
}
