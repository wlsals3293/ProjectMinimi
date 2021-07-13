using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager<UIManager>
{
    public enum EUIView
    {
        Title,
        LoadingScreen,
        StageEnd,
        Death
    }


    private UIView currentView = null;

    private Dictionary<EUIView, UIView> viewList = new Dictionary<EUIView, UIView>();

    private Transform mainCanvas = null;


    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        SceneInit();
    }

    private void SceneInit()
    {
        if()

        if (mainCanvas == null)
        {
            mainCanvas = GameObject.Find("Canvas").transform;
        }
    }

    public void InGameInit()
    {
        viewList.Add(EUIView.StageEnd, CreateView(EUIView.StageEnd));
        viewList.Add(EUIView.Death, CreateView(EUIView.Death));
    }

    public void OpenView(EUIView view)
    {
        if (currentView != null)
        {
            currentView.Hide();
        }

        currentView = viewList[view];
        currentView.Show();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseView()
    {
        currentView.Hide();
        currentView = null;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private UIView CreateView(EUIView view)
    {
        UIView newView;

        switch (view)
        {
            case EUIView.Title:
                newView = ResourceManager.Instance.CreatePrefab<UIView>("Panel_Title", mainCanvas, PrefabPath.UI, true);
                break;
            case EUIView.LoadingScreen:
                newView = ResourceManager.Instance.CreatePrefab<UIView>("Panel_LoadingScreen", mainCanvas, PrefabPath.UI, false);
                break;
            case EUIView.StageEnd:
                newView = ResourceManager.Instance.CreatePrefab<UIView>("Panel_EndStage", mainCanvas, PrefabPath.UI, false);
                break;
            case EUIView.Death:
                newView = ResourceManager.Instance.CreatePrefab<UIView>("Panel_Death", mainCanvas, PrefabPath.UI, false);
                break;
            default:
                return null;
        }

        return newView;
    }

}
