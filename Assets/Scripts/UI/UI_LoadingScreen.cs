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
            if (UIManager.Instance.onScreenCoverComplete != null)
                UIManager.Instance.onScreenCoverComplete();
        }
        else if (type == AnimationType.Hide)
        {
            if (UIManager.Instance.onScreenRevealComplete != null)
                UIManager.Instance.onScreenRevealComplete();

            gameObject.SetActive(false);
        }
    }
}
