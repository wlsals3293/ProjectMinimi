using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Block : MonoBehaviour
{
    bool isCollisioned;
    Coroutine coroutine;
    MeshRenderer _renderer;
    public float interval;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision other)
    {


        if (other.gameObject.CompareTag("Player"))
        {
            coroutine = StartCoroutine(OnActivate());
        }
    }

    private IEnumerator OnActivate()
    {
        _renderer.material.color = Color.yellow;
        yield return new WaitForSeconds(interval);

        _renderer.material.color = Color.red;
        yield return new WaitForSeconds(interval);

        this.gameObject.SetActive(false);

        yield return null;
    }

}
