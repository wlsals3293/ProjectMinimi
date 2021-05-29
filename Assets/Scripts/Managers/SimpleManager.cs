using System.Collections;
using UnityEngine;


public abstract class SimpleManager<T> : MonoBehaviour where T : class
{
    private static T _Instance = null;

    public static T Instance 
    { 
        get => _Instance; 
    }

    protected virtual void Awake()
    {
        if(_Instance == null)
        {
            _Instance = gameObject.GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
    }
}
