using System.Collections;
using UnityEngine;

public class RadialBlurFilter : CameraFilter
{
    /// <summary>
    /// ∫Ì∑Ø ∞≠µµ
    /// </summary>
    public float maxStrength = 4.0f;


    public RadialBlurFilter()
    {
        featureName = "RadialBlurFilter";
    }

    public override void Apply()
    {
        if (!TryGetFeature())
            return;

        if (!transitioning)
        {
            // ¿ÃπÃ Apply µ 
            if (isApplied)
            {
                return;
            }
            else
            {
                feature.SetActive(true);
                CameraManager.Instance.RendererData.SetDirty();
            }
        }

        if (coroutine != null)
        {
            CameraManager.Instance.StopCoroutine(coroutine);
            coroutine = null;
            transitioning = false;
        }

        material.SetFloat("_SampleStrength", maxStrength);

        isApplied = isApplying = true;
        curProgress = 0f;
    }

    public override void Clear()
    {
        // ¿ÃπÃ Clear µ 
        if (!transitioning && !isApplied)
        {
            return;
        }

        if (coroutine != null)
        {
            CameraManager.Instance.StopCoroutine(coroutine);
            coroutine = null;
            transitioning = false;
        }

        material.SetFloat("_SampleStrength", 0f);
        feature.SetActive(false);
        CameraManager.Instance.RendererData.SetDirty();

        isApplied = isApplying = false;
        curProgress = 0f;
    }

    protected override void TransitionUpdate()
    {
        float ratio;

        if (transitionCurve != null)
        {
            ratio = transitionCurve.Evaluate(curProgress);
        }
        else
        {
            ratio = isApplying ? curProgress : 1 - curProgress;
        }

        material.SetFloat("_SampleStrength", maxStrength * ratio);
    }
}
