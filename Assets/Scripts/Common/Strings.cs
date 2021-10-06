using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tags
{
    public const string Minimi = "Minimi";
    public const string Enemy = "Enemy";
    public const string Player = "Player";
    public const string Object = "Object";
    public const string StageInfo = "StageInfo";
    public const string SwallowableObject = "SwallowableObject";
    public const string DistructibleObject = "DistructibleObject";
    public const string Wagon = "Wagon";
    public const string Conductor = "Conductor";
}

public static class Layers
{
    public const int None = -1;
    public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    public static readonly int Player = LayerMask.NameToLayer("Player");

    public static readonly int Minimi = LayerMask.NameToLayer("Minimi");
    public static readonly int Obj = LayerMask.NameToLayer("Object");
}

public static class LayerMasks
{
    public static readonly LayerMask Player = LayerMask.GetMask("Player");
    public static readonly LayerMask Ground = LayerMask.GetMask("Ground");
    public static readonly LayerMask Object = LayerMask.GetMask("Object");
    public static readonly LayerMask Enemy = LayerMask.GetMask("Enemy");


    /// <summary>
    /// Layers: Player, Ground, Object, Enemy
    /// </summary>
    public static readonly LayerMask All = LayerMask.GetMask("Player", "Ground", "Object", "Enemy");

    /// <summary>
    /// Layers: Ground, Object
    /// </summary>
    public static readonly LayerMask GO = LayerMask.GetMask("Ground", "Object");

    /// <summary>
    /// Layers: Ground, Object, Enemy
    /// </summary>
    public static readonly LayerMask GOE = LayerMask.GetMask("Ground", "Object", "Enemy");

    /// <summary>
    /// Layers: Player, Object
    /// </summary>
    public static readonly LayerMask PO = LayerMask.GetMask("Player", "Object");

    /// <summary>
    /// Layers: Player, Object, Enemy
    /// </summary>
    public static readonly LayerMask POE = LayerMask.GetMask("Player", "Object", "Enemy");

    /// <summary>
    /// Layers: Player, Ground, Object
    /// </summary>
    public static readonly LayerMask PGO = LayerMask.GetMask("Player", "Ground", "Object");

    /// <summary>
    /// Layers: Object, Enemy
    /// </summary>
    public static readonly LayerMask OE = LayerMask.GetMask("Object", "Enemy");
}
