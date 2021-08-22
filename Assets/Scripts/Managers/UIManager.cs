using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager<UIManager>
{
    public enum EUIView
    {
        StageEnd,
        Death,
        EscMenu,
        Max
    }


    private Dictionary<EUIView, UIView> viewList = new Dictionary<EUIView, UIView>();

    private Stack<UIView> viewStack = new Stack<UIView>();

    private Transform mainCanvas = null;

    private UI_LoadingScreen loadingScreen = null;

    private UI_HUD hud = null;


    public delegate void TransitionDelegate();

    public TransitionDelegate onScreenCoverComplete;
    public TransitionDelegate onScreenRevealComplete;


    public UI_HUD HUD
    {
        get => hud;
    }


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

    public void SceneInit()
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
        // View
        for (int i = 0; i < (int)EUIView.Max; i++)
        {
            if (!viewList.ContainsKey((EUIView)i))
            {
                viewList.Add((EUIView)i, CreateView((EUIView)i));
            }
        }

        // HUD
        hud = ResourceManager.Instance.CreatePrefab<UI_HUD>("HUDCanvas", null, PrefabPath.UI);
        hud.Init();
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
        if (viewStack.Count > 0)
        {
            viewStack.Peek().Hide();
        }

        UIView ui = viewList[view];
        viewStack.Push(ui);
        ui.Show();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseView()
    {
        UIView ui = viewStack.Pop();
        ui.Hide();

        if (viewStack.Count > 0)
        {
            viewStack.Peek().Show();
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void CloseAllView()
    {
        while (viewStack.Count > 0)
        {
            viewStack.Pop().Hide();
        }

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
            case EUIView.EscMenu:
                newView = ResourceManager.Instance.CreatePrefab<UIView>("Panel_ESC", mainCanvas, PrefabPath.UI, false);
                break;
            default:
                return null;
        }

        return newView;
    }

}
