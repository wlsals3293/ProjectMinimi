using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tags
{
    public const string minimi = "Minimi";
    public const string enemy = "Enemy";
    public const string player = "Player";
}

public static class Layers
{
    public const int none = -1;
    public static readonly int enemy = LayerMask.NameToLayer("Enemy");
    public static readonly int player = LayerMask.NameToLayer("Player");

    public static readonly int minimi = LayerMask.NameToLayer("Minimi");
    public static readonly int obj = LayerMask.NameToLayer("Object");
}
