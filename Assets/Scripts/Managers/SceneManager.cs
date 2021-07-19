using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScene = UnityEngine.SceneManagement;


public class SceneManager : BaseManager<SceneManager>
{
    /// <summary>
    /// 씬 전환 준비 여부
    /// </summary>
    [HideInInspector] public bool transitionReady = false;

    /// <summary>
    /// 로딩중 여부
    /// </summary>
    private bool isLoading = false;


    public delegate void LoadCompleteDelegate();
    public LoadCompleteDelegate onLoadComplete;


    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadScene(ref string sceneName)
    {
        if (isLoading)
            return;

        isLoading = true;
        transitionReady = false;
        UIManager.Instance.StartTransition();
        Debug.Log("로딩 시작!");
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void ReloadScene()
    {
        string currentScene = UnityScene.SceneManager.GetActiveScene().name;
        LoadScene(ref currentScene);
    }

    public void UnloadStage(string sceneName)
    {
        StartCoroutine(UnloadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncOperation = UnityScene.SceneManager.LoadSceneAsync(sceneName);

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (transitionReady)
                asyncOperation.allowSceneActivation = true;

            //Debug.Log($"Loading:{asyncOperation.progress}, Ready:{transitionReady}");

            yield return null;
        }

        isLoading = false;
        Debug.Log("로딩 완료!");

        if (onLoadComplete != null)
            onLoadComplete();
    }

    private IEnumerator UnloadSceneAsync(string sceneName)
    {
        AsyncOperation asyncOperation = UnityScene.SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            Debug.Log($"Unloading:{asyncOperation.progress}");

            yield return null;
        }

        Debug.Log("Unload Complete!");
    }

}
