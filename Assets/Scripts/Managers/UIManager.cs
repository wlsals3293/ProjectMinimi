using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager<UIManager>
{
    public enum EUIView
    {
        StageEnd,
        Death
    }


    private UIView currentView = null;

    private Dictionary<EUIView, UIView> viewList = new Dictionary<EUIView, UIView>();

    private Transform mainCanvas = null;

    private UI_LoadingScreen loadingScreen = null;


    public delegate void TransitionDelegate();

    public TransitionDelegate onScreenCoverComplete;
    public TransitionDelegate onScreenRevealComplete;


    protected override void Awake()
    {
        base.Awake();

        onScreenCoverComplete += OnScreenCovered;
        SceneManager.Instance.onLoadComplete += OnSceneLoadComplete;
    }

    private void Start()
    {
        SceneInit();
    }

    private void SceneInit()
    {
        // Event System 확인
        if (GameObject.Find("EventSystem") == null)
        {
            ResourceManager.Instance.CreatePrefab("EventSystem", null, PrefabPath.UI, true);
        }

        // Main Canvas 확인
        if (mainCanvas == null)
        {
            GameObject canvas = GameObject.Find("MainCanvas");

            if (canvas != null)
            {
                mainCanvas = canvas.transform;
            }
            else
            {
                mainCanvas = ResourceManager.Instance.CreatePrefab("MainCanvas", null, PrefabPath.UI, true).transform;
            }

            DontDestroyOnLoad(mainCanvas.gameObject);
        }

        // 로딩스크린 확인
        if(loadingScreen == null)
        {
            loadingScreen = ResourceManager.Instance.CreatePrefab<UI_LoadingScreen>("Panel_LoadingScreen", mainCanvas, PrefabPath.UI, false);
        }

    }

    public void InGameInit()
    {
        viewList.Add(EUIView.StageEnd, CreateView(EUIView.StageEnd));
        viewList.Add(EUIView.Death, CreateView(EUIView.Death));
    }


    public void StartTransition()
    {
        loadingScreen.Show();
    }

    private void OnScreenCovered()
    {
        SceneManager.Instance.transitionReady = true;
    }

    private void OnSceneLoadComplete()
    {
        loadingScreen.Hide();
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
