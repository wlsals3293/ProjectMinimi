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
}

public static class Layers
{
    public const int None = -1;
    public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    public static readonly int Player = LayerMask.NameToLayer("Player");

    public static readonly int Minimi = LayerMask.NameToLayer("Minimi");
    public static readonly int Obj = LayerMask.NameToLayer("Object");
    public static readonly int Wagon = LayerMask.NameToLayer("Wagon");
}
