using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritAbility : PlayerAbility
{
    private const float PROJECTILE_DISTANCE = 50f;


    [Tooltip("�߻�ü �ӵ�")]
    [SerializeField]
    private float projectileSpeed = 30f;

    private int projectileindex = 0;


    private bool aiming = false;


    [Tooltip("�߻� ����")]
    private float shotInterval = 0.5f;

    /// <summary>
    /// �߻� ���� ����
    /// </summary>
    private bool shotDelay = false;

    /// <summary>
    /// �߻� �� ����ð�
    /// </summary>
    private float elapsedShotTime;




    // �Ƹ��� �ӽ�?
    [SerializeField]
    private SpiritProjectile[] projectiles;


    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    public override void AbilityUpdate()
    {
        if (shotDelay)
        {
            elapsedShotTime += Time.deltaTime;

            if (elapsedShotTime >= shotInterval)
            {
                shotDelay = false;
            }
        }
    }

    public override void MainAction1(KeyInfo key)
    {
        if (key.down && !shotDelay)
            MainAction1_Down();
    }

    public override void MainAction2(KeyInfo key)
    {
        if (key.down)
        {
            aiming = true;
            pc.ChangeState(PlayerState.Aim);
        }
        else if (key.up && aiming)
        {
            aiming = false;
            pc.ChangeState(PlayerState.Idle);
        }
    }

    private void MainAction1_Down()
    {
        if (aiming)
        {
            Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
            Ray ray = CameraManager.Instance.MainCam.ScreenPointToRay(screenCenter);
            Vector3 fireDirection;
            Vector3 firePosition = pc.firePoint.position;

            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f,
                LayerMasks.GO, QueryTriggerInteraction.Ignore))
            {
                fireDirection = (hit.point - firePosition).normalized;
            }
            else
            {
                fireDirection = (ray.GetPoint(PROJECTILE_DISTANCE) - firePosition).normalized;
            }


            if (projectileindex >= projectiles.Length)
            {
                Debug.LogError("�߻�ü �迭 �ε����� ������ϴ�.");
                return;
            }

            // TODO: ���߿� ������Ʈ Ǯ������ �ٲ���ҵ�
            SpiritProjectile proj = Instantiate<SpiritProjectile>(
                projectiles[projectileindex], firePosition, pc.CachedRigidbody.rotation);

            proj.Set(fireDirection * projectileSpeed);

            shotDelay = true;
            elapsedShotTime = 0f;
        }
    }

    public override void NumAction1(KeyInfo key)
    {
        if (key.down)
            projectileindex = 0;
    }

    public override void NumAction2(KeyInfo key)
    {
        if (key.down)
            projectileindex = 1;
    }

    public override void NumAction3(KeyInfo key)
    {
        if (key.down)
            projectileindex = 2;
    }
}
