using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class CameraFilter
{

    protected string featureName;


    /// <summary>
    /// 이 필터가 현재 완전히 적용이 돼있으면 true
    /// </summary>
    protected bool isApplied;

    /// <summary>
    /// 마지막으로 작동시킨 메서드가 Apply 또는 SmoothApply이면 true
    /// </summary>
    protected bool isApplying;

    /// <summary>
    /// 현재 부드러운 전환이 작동중이면 true
    /// </summary>
    protected bool transitioning;


    /// <summary>
    /// 경과시간
    /// </summary>
    protected float elapsedTime;

    /// <summary>
    /// 전환시간
    /// </summary>
    protected float transitionTime;

    /// <summary>
    /// 현재 진행도
    /// </summary>
    protected float curProgress;


    /// <summary>
    /// 전환에 사용되는 코루틴
    /// </summary>
    protected Coroutine coroutine;

    /// <summary>
    /// 이 필터의 렌더피쳐
    /// </summary>
    protected ScriptableRendererFeature feature;

    /// <summary>
    /// 이 필터가 사용하는 머티리얼
    /// </summary>
    protected Material material;

    /// <summary>
    /// 전환에 사용되는 커브
    /// </summary>
    protected AnimationCurve transitionCurve;


    /// <summary>
    /// 이 필터가 현재 완전히 적용이 돼있으면 true
    /// </summary>
    public bool IsApplied { get => isApplied; }

    /// <summary>
    /// 현재 부드러운 전환이 작동중이면 true
    /// </summary>
    public bool Transitioning { get => transitioning; }

    /// <summary>
    /// 이 필터의 렌더피쳐
    /// </summary>
    public ScriptableRendererFeature Feature { get => feature; }

    /// <summary>
    /// 이 필터가 사용하는 머티리얼
    /// </summary>
    public Material BlitMaterial { get => material; }



    /// <summary>
    /// 필터를 화면에 즉시 적용합니다.
    /// </summary>
    public abstract void Apply();

    /// <summary>
    /// 필터를 화면에서 즉시 제거합니다.
    /// </summary>
    public abstract void Clear();

    /// <summary>
    /// 필터를 화면에 부드럽게 적용합니다.
    /// </summary>
    /// <param name="time">필터가 완전히 적용될때까지 걸리는 시간</param>
    /// <param name="curve">전환 커브</param>
    public virtual void SmoothApply(float time = 0f, AnimationCurve curve = null)
    {
        // 이미 Apply 됨
        if (isApplied && !transitioning)
            return;


        if (time > 0f)
        {
            if (!TryGetFeature())
                return;

            if (transitioning)
            {
                elapsedTime = time * (isApplying ? curProgress : 1 - curProgress);
            }
            else
            {
                elapsedTime = 0f;
            }
            transitionTime = time;
            isApplying = true;
            transitionCurve = curve;

            if (coroutine == null)
            {
                coroutine = CameraManager.Instance.StartCoroutine(TransitionProcess());
            }
        }
        else
        {
            Apply();
        }
    }

    /// <summary>
    /// 필터를 화면에서 부드럽게 제거합니다.
    /// </summary>
    /// <param name="time">필터가 완전히 제거될때까지 걸리는 시간</param>
    /// <param name="curve">전환 커브</param>
    public virtual void SmoothClear(float time = 0f, AnimationCurve curve = null)
    {
        // 이미 Clear 됨
        if (!isApplied && !transitioning)
            return;


        if (time > 0f)
        {
            if (!TryGetFeature())
                return;

            if (transitioning)
            {
                elapsedTime = time * (!isApplying ? curProgress : 1 - curProgress);
            }
            else
            {
                elapsedTime = 0f;
            }
            transitionTime = time;
            isApplying = false;
            transitionCurve = curve;

            if (coroutine == null)
            {
                coroutine = CameraManager.Instance.StartCoroutine(TransitionProcess());
            }
        }
        else
        {
            Clear();
        }
    }

    /// <summary>
    /// 이름으로 렌더피쳐와 머티리얼을 얻어와 저장합니다.
    /// </summary>
    /// <returns>저장 성공여부</returns>
    protected bool TryGetFeature()
    {
        if (feature == null)
        {
            feature = CameraManager.Instance.RendererData.rendererFeatures
            .Where((f) => f.name == featureName).FirstOrDefault();
        }
        if (feature != null && material == null)
        {
            material = (feature as CameraFilterFeature).BlitMaterial;
        }

        return feature != null && material != null;
    }

    /// <summary>
    /// 전환 처리 코루틴
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator TransitionProcess()
    {
        transitioning = true;
        feature.SetActive(true);
        CameraManager.Instance.RendererData.SetDirty();

        while (elapsedTime <= transitionTime)
        {
            elapsedTime += Time.deltaTime;

            curProgress = Mathf.Clamp01(elapsedTime / transitionTime);

            TransitionUpdate();

            yield return null;
        }

        coroutine = null;

        if (isApplying)
            Apply();
        else
            Clear();

        transitioning = false;
    }

    /// <summary>
    /// 필터가 전환되고 있을 때 호출되는 업데이트
    /// </summary>
    protected abstract void TransitionUpdate();
}
