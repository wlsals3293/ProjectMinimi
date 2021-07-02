using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScene = UnityEngine.SceneManagement;


public class SceneManager : BaseManager<SceneManager>
{
    private bool isLoading = false;


    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadStage(string sceneName)
    {
        if (isLoading)
            return;

        isLoading = true;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void UnloadStage(string sceneName)
    {
        StartCoroutine(UnloadSceneAsync(sceneName));
    }

    // TODO 로딩 & 타이틀 기타 UI필요
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncOperation = UnityScene.SceneManager.LoadSceneAsync(sceneName);

        while(!asyncOperation.isDone)
        {
            Debug.Log($"Loading:{asyncOperation.progress}");

            yield return null;
        }

        isLoading = false;
        Debug.Log("Load Complete!");
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
