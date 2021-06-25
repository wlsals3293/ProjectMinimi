using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SimpleManager<PlayerManager>
{
    

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


    public void CreatePlayer()
    {
        playerCtrl = ResourceManager.Instance.CreatePrefab<PlayerController>(PrefabNames.Player);
    }

    public void InitStagePlayer()
    {
        playerCtrl.SetLocalPosition(StageManager.Instance.StartPosition);
        playerCtrl.Init();
    }

    public void RespawnPlayer()
    {
        Transform checkpoint = StageManager.Instance.GetCurrentCheckpoint();

        if(checkpoint != null)
        {
            playerCtrl.SetLocalPosition(checkpoint.position);
        }
        else
        {
            playerCtrl.SetLocalPosition(StageManager.Instance.StartPosition);
        }
        
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
