using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : ManagerBase<UIManager>
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


    public UnityAction onScreenCoverComplete;
    public UnityAction onScreenRevealComplete;


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
        // Event System Ȯ��
        if (GameObject.Find("EventSystem") == null)
        {
            ResourceManager.Instance.CreatePrefab("EventSystem", PrefabPath.UI, null, true);
        }

        // Main Canvas Ȯ��
        if (mainCanvas == null)
        {
            GameObject canvas = GameObject.Find("MainCanvas");

            if (canvas != null)
            {
                mainCanvas = canvas.transform;
            }
            else
            {
                mainCanvas = ResourceManager.Instance.CreatePrefab("MainCanvas", PrefabPath.UI, null, true).transform;
            }

            DontDestroyOnLoad(mainCanvas.gameObject);
        }

        // �ε���ũ�� Ȯ��
        if (loadingScreen == null)
        {
            loadingScreen = ResourceManager.Instance.CreatePrefab<UI_LoadingScreen>("Panel_LoadingScreen", PrefabPath.UI, mainCanvas, false);
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
        hud = ResourceManager.Instance.CreatePrefab<UI_HUD>("HUDCanvas", PrefabPath.UI, null);
        hud.Init();
    }

    /// <summary>
    /// HUD�� ���ü��� �ٲߴϴ�.
    /// </summary>
    public void ToggleHUD()
    {
        if (hud.gameObject.activeSelf)
            hud.gameObject.SetActive(false);
        else
            hud.gameObject.SetActive(true);
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
                newView = ResourceManager.Instance.CreatePrefab<UIView>("Panel_EndStage", PrefabPath.UI, mainCanvas, false);
                break;
            case EUIView.Death:
                newView = ResourceManager.Instance.CreatePrefab<UIView>("Panel_Death", PrefabPath.UI, mainCanvas, false);
                break;
            case EUIView.EscMenu:
                newView = ResourceManager.Instance.CreatePrefab<UIView>("Panel_ESC", PrefabPath.UI, mainCanvas, false);
                break;
            default:
                return null;
        }

        return newView;
    }

}
