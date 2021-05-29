using UnityEngine;

public class PlayerManager : SimpleManager<PlayerManager>
{
    private PlayerController playerCtrl = null;

    public PlayerController PlayerCtrl { get => playerCtrl; }

    protected override void Awake()
    {
        base.Awake();
    }


    public void CreatePlayer()
    {
        //playerCtrl = ctrl;
    }

}
