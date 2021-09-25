using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Lib
{
    public static bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) > 0;
    }
}
