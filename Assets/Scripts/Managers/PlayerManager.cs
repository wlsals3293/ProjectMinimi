using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager<PlayerManager>
{
    

    private PlayerController playerCtrl = null;

    public PlayerController PlayerCtrl { get => playerCtrl; }

    private PlayerCharacter playerChar = null;

    public PlayerCharacter PlayerChar { get => playerChar; }

    private Dictionary<PlayerState, SimpleBehaviour> playerBehaviours = new Dictionary<PlayerState, SimpleBehaviour>();


    public void Initialize()
    {
        CreatePlayer();
        InitStagePlayer();
    }

    public void CreatePlayer()
    {
        playerCtrl = ResourceManager.Instance.CreatePrefab<PlayerController>(PrefabNames.Player);
        playerChar = playerCtrl.PlayerCharacter;
    }

    public void InitStagePlayer()
    {
        playerCtrl.transform.position = StageManager.Instance.StartPosition;
        playerCtrl.Init();
    }

    public void RespawnPlayer()
    {
        Transform checkpoint = StageManager.Instance.GetLastCheckpoint();

        if(checkpoint != null)
        {
            playerCtrl.transform.position = checkpoint.position;
        }
        else
        {
            playerCtrl.transform.position = StageManager.Instance.StartPosition;
        }
        
        playerCtrl.Init();
        PlayerChar.SetHP(playerChar.MaxHP);
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
