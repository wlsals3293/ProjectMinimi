using System.Collections;
using UnityEngine;

public class ResourceManager : BaseManager<ResourceManager>
{
    protected override void Awake()
    {
        base.Awake();
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



