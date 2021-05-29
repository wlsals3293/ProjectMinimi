using System.Collections;
using UnityEngine;

public class ResourceManager : SimpleManager<ResourceManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public T CreatePrefab<T>(string filename, bool active = true) where T : MonoBehaviour
    {
        T result = null;
        GameObject obj = Resources.Load<GameObject>(ResourcePath.GetPrefabPath(filename));

        if (obj != null)
        {
            result = GameObject.Instantiate(obj).GetComponent<T>();
            if(active == false)
            {
                result.gameObject.SetActive(false);
            }
        }

        return result;
    }
}



