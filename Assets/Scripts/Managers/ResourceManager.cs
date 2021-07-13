using System.Collections;
using UnityEngine;

public class ResourceManager : BaseManager<ResourceManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public GameObject CreatePrefab(string filename, Transform parent = null,
        PrefabPath path = PrefabPath.Root, bool active = true)
    {
        GameObject result = null;
        GameObject obj = Resources.Load<GameObject>(ResourcePath.GetPrefabPath(filename, path));

        if (obj != null)
        {
            result = GameObject.Instantiate(obj, parent);
            if (active == false)
            {
                result.gameObject.SetActive(false);
            }
        }

        return result;
    }

    public T CreatePrefab<T>(string filename, Transform parent = null,
        PrefabPath path = PrefabPath.Root, bool active = true) where T : MonoBehaviour
    {
        T result = null;
        GameObject obj = Resources.Load<GameObject>(ResourcePath.GetPrefabPath(filename, path));

        if (obj != null)
        {
            result = GameObject.Instantiate(obj, parent).GetComponent<T>();
            if(active == false)
            {
                result.gameObject.SetActive(false);
            }
        }

        return result;
    }
}



