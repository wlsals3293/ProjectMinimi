using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadingScreen : UIView
{
    private Animator anim;

    public delegate void VisibleChangeCompleteDelegate();

    public VisibleChangeCompleteDelegate onShowComplete;
    public VisibleChangeCompleteDelegate onHideComplete;

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

        anim.SetBool(0, true);
    }

    public override void Hide()
    {
        base.Hide();

        anim.SetBool(0, false);
    }

    public void AnimtationCompleteEvent(AnimationType type)
    {
        if (type == AnimationType.Show)
        {
            if (onShowComplete != null)
                onShowComplete();
        }
        else if (type == AnimationType.Hide)
        {
            if (onHideComplete != null)
                onHideComplete();
        }
    }
}
