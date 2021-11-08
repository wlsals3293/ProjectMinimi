using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class CameraFilter
{

    protected string featureName;


    /// <summary>
    /// �� ���Ͱ� ���� ������ ������ �������� true
    /// </summary>
    protected bool isApplied;

    /// <summary>
    /// ���������� �۵���Ų �޼��尡 Apply �Ǵ� SmoothApply�̸� true
    /// </summary>
    protected bool isApplying;

    /// <summary>
    /// ���� �ε巯�� ��ȯ�� �۵����̸� true
    /// </summary>
    protected bool transitioning;


    /// <summary>
    /// ����ð�
    /// </summary>
    protected float elapsedTime;

    /// <summary>
    /// ��ȯ�ð�
    /// </summary>
    protected float transitionTime;

    /// <summary>
    /// ���� ���൵
    /// </summary>
    protected float curProgress;


    /// <summary>
    /// ��ȯ�� ���Ǵ� �ڷ�ƾ
    /// </summary>
    protected Coroutine coroutine;

    /// <summary>
    /// �� ������ ��������
    /// </summary>
    protected ScriptableRendererFeature feature;

    /// <summary>
    /// �� ���Ͱ� ����ϴ� ��Ƽ����
    /// </summary>
    protected Material material;

    /// <summary>
    /// ��ȯ�� ���Ǵ� Ŀ��
    /// </summary>
    protected AnimationCurve transitionCurve;


    /// <summary>
    /// �� ���Ͱ� ���� ������ ������ �������� true
    /// </summary>
    public bool IsApplied { get => isApplied; }

    /// <summary>
    /// ���� �ε巯�� ��ȯ�� �۵����̸� true
    /// </summary>
    public bool Transitioning { get => transitioning; }

    /// <summary>
    /// �� ������ ��������
    /// </summary>
    public ScriptableRendererFeature Feature { get => feature; }

    /// <summary>
    /// �� ���Ͱ� ����ϴ� ��Ƽ����
    /// </summary>
    public Material BlitMaterial { get => material; }



    /// <summary>
    /// ���͸� ȭ�鿡 ��� �����մϴ�.
    /// </summary>
    public abstract void Apply();

    /// <summary>
    /// ���͸� ȭ�鿡�� ��� �����մϴ�.
    /// </summary>
    public abstract void Clear();

    /// <summary>
    /// ���͸� ȭ�鿡 �ε巴�� �����մϴ�.
    /// </summary>
    /// <param name="time">���Ͱ� ������ ����ɶ����� �ɸ��� �ð�</param>
    /// <param name="curve">��ȯ Ŀ��</param>
    public virtual void SmoothApply(float time = 0f, AnimationCurve curve = null)
    {
        // �̹� Apply ��
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
    /// ���͸� ȭ�鿡�� �ε巴�� �����մϴ�.
    /// </summary>
    /// <param name="time">���Ͱ� ������ ���ŵɶ����� �ɸ��� �ð�</param>
    /// <param name="curve">��ȯ Ŀ��</param>
    public virtual void SmoothClear(float time = 0f, AnimationCurve curve = null)
    {
        // �̹� Clear ��
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
    /// �̸����� �������Ŀ� ��Ƽ������ ���� �����մϴ�.
    /// </summary>
    /// <returns>���� ��������</returns>
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
    /// ��ȯ ó�� �ڷ�ƾ
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
    /// ���Ͱ� ��ȯ�ǰ� ���� �� ȣ��Ǵ� ������Ʈ
    /// </summary>
    protected abstract void TransitionUpdate();
}
