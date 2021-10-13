using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestBox : MonoBehaviour
{
    public Ease ease;

    private Sequence sequence;

    private void Start()
    {
        float interval = 2f;

        sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(new Vector3(5f, 1f, 5f), 1f).SetEase(ease));
        sequence.AppendInterval(interval);
        sequence.Append(transform.DOScale(new Vector3(7f, 1f, 7f), 1f).SetEase(ease));
        sequence.AppendInterval(interval);
        sequence.Append(transform.DOScale(new Vector3(11f, 1f, 11f), 1f).SetEase(ease));
        sequence.AppendInterval(interval);
        sequence.Append(transform.DOScale(new Vector3(7f, 1f, 7f), 1f).SetEase(ease));
        sequence.AppendInterval(interval);
        sequence.Append(transform.DOScale(new Vector3(5f, 1f, 5f), 1f).SetEase(ease));
        sequence.AppendInterval(interval);
        sequence.Append(transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.InExpo));
        sequence.AppendInterval(interval);

        sequence.SetLoops(-1);
        sequence.Pause();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            sequence.Play();
        }
    }
}