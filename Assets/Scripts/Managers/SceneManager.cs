using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityScene = UnityEngine.SceneManagement;


public class SceneManager : ManagerBase<SceneManager>
{
    /// <summary>
    /// �� ��ȯ �غ� ����
    /// </summary>
    [HideInInspector] public bool transitionReady = false;

    /// <summary>
    /// �ε��� ����
    /// </summary>
    private bool isLoading = false;

    /// <summary>
    /// �ε� ���൵
    /// </summary>
    private float loadingProgress;


    public float LoadingProgress { get => loadingProgress; }


    public UnityAction onLoadComplete;


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
        Debug.Log("�ε� ����!");
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
        while(!transitionReady)
        {
            //Debug.Log($"Ready:{transitionReady}");
            yield return null;
        }

        AsyncOperation asyncOperation = UnityScene.SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            //Debug.Log($"Loading:{asyncOperation.progress}");
            loadingProgress = asyncOperation.progress;

            yield return null;
        }

        isLoading = false;
        Debug.Log("�ε� �Ϸ�!");

        onLoadComplete?.Invoke();
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
