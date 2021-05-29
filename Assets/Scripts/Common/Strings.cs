using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tags
{
    public const string minimi = "minimi";
    public const string enemy = "enemy";
    public const string player = "Player";
}

public static class Layers
{
    public static readonly int enemy = LayerMask.NameToLayer("enemy");
    public static readonly int player = LayerMask.NameToLayer("Player");

    public static readonly int minimi = LayerMask.NameToLayer("Minimi");
    public static readonly int obj = LayerMask.NameToLayer("Object");
}
