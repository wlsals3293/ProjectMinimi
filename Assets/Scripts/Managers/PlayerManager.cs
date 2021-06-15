using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SimpleManager<PlayerManager>
{
    /// <summary>
    /// 플레이어가 이 높이보다 낮아지면 사망함
    /// </summary>
    [SerializeField] private float killY = 10.0f;

    private PlayerController playerCtrl = null;

    public PlayerController PlayerCtrl { get => playerCtrl; }

    private PlayerCharacter playerChar = null;

    public PlayerCharacter PlayerChar { get => playerChar; }

    private Dictionary<PlayerState, SimpleBehaviour> playerBehaviours = new Dictionary<PlayerState, SimpleBehaviour>();


    protected override void Awake()
    {
        base.Awake();

        CreatePlayer();
    }

    private void Update()
    {
        /*if(PlayerCtrl.movement.cachedRigidbody.position.y < killY)
        {
            PlayerCtrl.ChangeState(PlayerState.Dead);
        }*/
    }


    public void CreatePlayer()
    {
        playerCtrl = ResourceManager.Instance.CreatePrefab<PlayerController>(PrefabNames.Player);
        InitStagePlayer();
    }

    public void InitStagePlayer()
    {
        playerCtrl.SetLocalPosition(StageManager.Instance.StartPosition);
        playerCtrl.Init();
    }

    public void AddBehaviour(PlayerState state, SimpleBehaviour behaviour)
    {
        if(playerBehaviours.ContainsKey(state))
        {
            playerBehaviours[state] = behaviour;
        }
        else
        {
            playerBehaviours.Add(state, behaviour);
        }
    }

    public SimpleBehaviour GetBehaviour(PlayerState state)
    {
        if (playerBehaviours.ContainsKey(state))
        {
            return playerBehaviours[state];
        }

        return null;
    }
}
