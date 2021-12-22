using System.Collections;
using UnityEngine;


public abstract class ManagerBase<T> : MonoBehaviour where T : class
{
    protected static T _Instance = null;

    public static T Instance 
    { 
        get => _Instance; 
    }

    protected virtual void Awake()
    {
        if(_Instance == null)
        {
            _Instance = GetComponent<T>(); 
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
    }
}
