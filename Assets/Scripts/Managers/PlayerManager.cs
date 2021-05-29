using UnityEngine;

public class PlayerManager : SimpleManager<PlayerManager>
{
    private PlayerController playerCtrl = null;

    public PlayerController PlayerCtrl { get => playerCtrl; }

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

    

}
