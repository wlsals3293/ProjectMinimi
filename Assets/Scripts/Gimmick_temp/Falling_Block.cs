using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Block : MonoBehaviour
{
    bool isCollisioned;
    Coroutine coroutine;
    Renderer renderer;
    public float interval;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
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
        renderer.material.color = Color.yellow;
        yield return new WaitForSeconds(interval);

        renderer.material.color = Color.red;
        yield return new WaitForSeconds(interval);

        this.gameObject.SetActive(false);

        yield return null;
    }

}
