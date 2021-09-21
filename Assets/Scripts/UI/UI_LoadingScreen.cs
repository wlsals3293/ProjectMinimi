using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadingScreen : UIView
{
    private Animator anim;


    public enum AnimationType
    {
        Show,
        Hide
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public override void Show()
    {
        base.Show();

        anim.SetBool("Visibility", true);
    }

    public override void Hide()
    {
        anim.SetBool("Visibility", false);
    }

    public void AnimtationCompleteEvent(AnimationType type)
    {
        if (type == AnimationType.Show)
        {
            UIManager.Instance.onScreenCoverComplete?.Invoke();
        }
        else if (type == AnimationType.Hide)
        {
            UIManager.Instance.onScreenRevealComplete?.Invoke();

            gameObject.SetActive(false);
        }
    }
}
