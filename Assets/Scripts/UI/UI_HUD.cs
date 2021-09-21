using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_HUD : MonoBehaviour
{
    [SerializeField]
    private GameObject crossHair;

    [SerializeField]
    private GameObject[] life;

    [SerializeField]
    private GameObject behaviourDesc;

    [SerializeField]
    private Image[] keyDisplay;



    private void Update()
    {
        UpdateKeyDisplay();
    }


    public void Init()
    {
        VisibleCrossHair(false);
        PlayerManager.Instance.PlayerChar.onHpChanged += SetLife;
    }

    public void VisibleCrossHair(bool isVisible)
    {
        crossHair.SetActive(isVisible);
    }

    public void SetLife(int amount)
    {
        for (int i = 0; i < 3; i++)
        {
            life[i].SetActive(i + 1 <= amount);
        }
    }

    private void UpdateKeyDisplay()
    {
        if (keyDisplay.Length < 8)
            return;

        SetKeyDisplay(0, Input.GetMouseButton(0));
        SetKeyDisplay(1, Input.GetMouseButton(1));

        SetKeyDisplay(2, Input.GetKey(KeyCode.Space));
        SetKeyDisplay(3, Input.GetKey(KeyCode.W));
        SetKeyDisplay(4, Input.GetKey(KeyCode.A));
        SetKeyDisplay(5, Input.GetKey(KeyCode.S));
        SetKeyDisplay(6, Input.GetKey(KeyCode.D));
        SetKeyDisplay(7, Input.GetKey(KeyCode.E));

    }

    private void SetKeyDisplay(int idx, bool isActive)
    {
        Color btnColor = isActive ? Color.gray : Color.white;

        keyDisplay[idx].color = btnColor;
    }

    public void SetBehaviourDesc(bool isActive)
    {
        if (behaviourDesc == null)
            return;

        behaviourDesc.SetActive(isActive);
    }
}
