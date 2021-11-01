using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbility : MonoBehaviour
{

    protected PlayerController pc;


    public abstract void AbilityUpdate();


    public abstract void MainAction1(KeyInfo key);


    public abstract void MainAction2(KeyInfo key);


    public abstract void NumAction1(KeyInfo key);


    public abstract void NumAction2(KeyInfo key);


    public abstract void NumAction3(KeyInfo key);

}
