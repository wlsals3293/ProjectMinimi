using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBlock : MonoBehaviour
{
    public float interval;

    private Coroutine coroutine;
    private MeshRenderer _renderer;

    // Start is called before the first frame update
    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Tags.Player) && coroutine == null)
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

        gameObject.SetActive(false);

    }

}
