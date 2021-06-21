using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : SimpleManager<SceneManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    // TODO 로딩 & 타이틀 기타 UI필요
    public void LoadSceneAsync(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }


    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
