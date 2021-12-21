using System;
using UnityEngine;

public static class Tags
{
    [Obsolete]
    public const string Minimi = "Minimi";      // 삭제 예정
    public const string Enemy = "Enemy";
    public const string Player = "Player";
    public const string StageInfo = "StageInfo";
    public const string Conductor = "Conductor";
}

public static class Layers
{
    public const int None = -1;
    public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    public static readonly int Minimi = LayerMask.NameToLayer("Minimi");
    public static readonly int Player = LayerMask.NameToLayer("Player");

    public static readonly int Obj = LayerMask.NameToLayer("Object");
}

public static class LayerMasks
{
    public static readonly LayerMask Enemy = LayerMask.GetMask("Enemy");
    public static readonly LayerMask Ground = LayerMask.GetMask("Ground");
    public static readonly LayerMask Minimi = LayerMask.GetMask("Minimi");
    public static readonly LayerMask Object = LayerMask.GetMask("Object");
    public static readonly LayerMask Player = LayerMask.GetMask("Player");


    /// <summary>
    /// Layers: Enemy, Ground, Minimi, Object, Player
    /// </summary>
    public static readonly LayerMask All = LayerMask.GetMask("Enemy", "Ground", "Minimi", "Object", "Player");

    /// <summary>
    /// Layers: Enemy, Ground, Object
    /// </summary>
    public static readonly LayerMask EGO = LayerMask.GetMask("Enemy", "Ground", "Object");

    /// <summary>
    /// Layers: Enemy, Object
    /// </summary>
    public static readonly LayerMask EO = LayerMask.GetMask("Enemy", "Object");

    /// <summary>
    /// Layers: Enemy, Object, Player
    /// </summary>
    public static readonly LayerMask EOP = LayerMask.GetMask("Enemy", "Object", "Player");

    /// <summary>
    /// Layers: Ground, Object
    /// </summary>
    public static readonly LayerMask GO = LayerMask.GetMask("Ground", "Object");

    /// <summary>
    /// Layers: Ground, Object, Player
    /// </summary>
    public static readonly LayerMask GOP = LayerMask.GetMask("Ground", "Object", "Player");

    /// <summary>
    /// Layers: Object, Player
    /// </summary>
    public static readonly LayerMask OP = LayerMask.GetMask("Object", "Player");

    
}
