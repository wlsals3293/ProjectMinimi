using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SimpleManager<PlayerManager>
{
    private PlayerController playerCtrl = null;

    public PlayerController PlayerCtrl { get => playerCtrl; }

    private Dictionary<PlayerState, SimpleBehaviour> playerBehaviours = new Dictionary<PlayerState, SimpleBehaviour>();


    protected override void Awake()
    {
        base.Awake();

        CreatePlayer();
    }


    public void CreatePlayer()
    {
        playerCtrl = ResourceManager.Instance.CreatePrefab<PlayerController>(PrefabNames.Player);
        playerCtrl.SetLocalPosition(new Vector3(40f, 0, 50f));
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
