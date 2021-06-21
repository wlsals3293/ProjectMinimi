using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : SimpleManager<SceneManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    // TODO �ε� & Ÿ��Ʋ ��Ÿ UI�ʿ�
    public void LoadSceneAsync(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }


    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
