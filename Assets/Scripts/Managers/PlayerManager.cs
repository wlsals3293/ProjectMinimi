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
        RespawnPlayer();
    }

    public void CreatePlayer()
    {
        playerCtrl = ResourceManager.Instance.CreatePrefab<PlayerController>(PrefabNames.Player);
        playerChar = playerCtrl.PlayerCharacter;
    }

    public void RespawnPlayer()
    {
        Transform checkpoint = StageManager.Instance.GetLastCheckpoint();

        if(checkpoint != null)
        {
            playerCtrl.transform.SetPositionAndRotation(checkpoint.position, checkpoint.rotation);
        }
        else
        {
            playerCtrl.transform.position = Vector3.zero;
        }
        
        playerCtrl.Init();
        PlayerChar.SetHP(playerChar.MaxHP);
    }

    public bool MovePlayer(int checkpointIndex)
    {
        Transform checkpoint = StageManager.Instance.GetCheckpoint(checkpointIndex);

        if (checkpoint == null)
            return false;

        playerCtrl.transform.SetPositionAndRotation(checkpoint.position, checkpoint.rotation);
        return true;
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
