using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritAbility : PlayerAbility
{
    private const float PROJECTILE_DISTANCE = 50f;


    private float projectileSpeed = 30f;

    private int projectileindex = 0;


    private bool aiming = false;


    // 아마도 임시?
    [SerializeField]
    private SpiritProjectile[] projectiles;


    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    public override void AbilityUpdate()
    {

    }

    public override void MainAction1(KeyInfo key)
    {
        if (key.down)
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

            // TODO: 나중에 오브젝트 풀링으로 바꿔야할듯
            SpiritProjectile proj = Instantiate<SpiritProjectile>(
                projectiles[projectileindex], firePosition, pc.CachedRigidbody.rotation);

            proj.Set(fireDirection * projectileSpeed);
        }
    }
}
