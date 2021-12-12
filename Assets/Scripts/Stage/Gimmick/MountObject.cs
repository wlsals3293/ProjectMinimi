using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MountObject : MonoBehaviour, IInteractable
{
    [Tooltip("환경 설정.")]
    [SerializeField]
    private MountObjectConfig config;

    [SerializeField]
    private Renderer emissionRenderer;

    [SerializeField]
    private Light mountLight;


    private bool mountable = true;

    private Vector3 targetPosition;

    private Vector3 currentVelocity;

    private Coroutine coroutine;

    private Rigidbody rbody;

    private MountSwitch mountedSwitch;


    public bool Mountable { get => mountable; }


    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();

        SetLight(false);
    }

    public void Interact(PlayerController player)
    {
        player.Hold(transform);
        UnMount();
    }

    public void Mount(MountSwitch targetSwitch)
    {
        mountedSwitch = targetSwitch;
        rbody.isKinematic = true;
        mountable = false;

        targetPosition = targetSwitch.transform.position;
        targetPosition += targetSwitch.transform.up * config.floatingHeight;

        coroutine = StartCoroutine(ActiveLoop());

        SetLight(true);
    }

    private void UnMount()
    {
        if (mountable)
            return;

        rbody.isKinematic = false;

        if (mountedSwitch != null)
        {
            mountedSwitch.Deactivate();
        }
        mountedSwitch = null;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        Timer.SetTimer(this, () => mountable = true, 1f);

        SetLight(false);
    }

    private IEnumerator ActiveLoop()
    {
        Vector3 smoothPosition = rbody.position;
        float elapsedTime = 0f;
        bool blending = true;
        float currentFloating;

        currentVelocity = rbody.velocity;
        if (currentVelocity.y > 0f)
        {
            currentFloating = 0f;
        }
        else
        {
            currentFloating = 180f * Mathf.Deg2Rad;
        }

        while (true)
        {
            float curHeight = config.floatingRange * Mathf.Sin(currentFloating);
            Vector3 sinePosition = targetPosition + curHeight * mountedSwitch.transform.up;
            currentFloating += Time.deltaTime * config.floatingSpeed;

            Vector3 newPosition;
            if (blending)
            {
                smoothPosition = Vector3.SmoothDamp(
                smoothPosition, targetPosition, ref currentVelocity, config.mountSmoothTime);

                float weight = elapsedTime / config.mountBlendTime;
                elapsedTime += Time.deltaTime;

                if (weight >= 1f)
                    blending = false;

                newPosition = Vector3.Lerp(smoothPosition, sinePosition, weight);
            }
            else
            {
                newPosition = sinePosition;
            }

            rbody.MovePosition(newPosition);

            Quaternion rot = Quaternion.Euler(config.rotationSpeed * Time.deltaTime);
            rbody.MoveRotation(rot * rbody.rotation);

            yield return null;
        }
    }

    private void SetLight(bool value)
    {
        if (value)
        {
            if (emissionRenderer != null)
                emissionRenderer.material.EnableKeyword("_EMISSION");
            if (mountLight != null)
                mountLight.enabled = true;
        }
        else
        {
            if (emissionRenderer != null)
                emissionRenderer.material.DisableKeyword("_EMISSION");
            if (mountLight != null)
                mountLight.enabled = false;
        }
    }
}
