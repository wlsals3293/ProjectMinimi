using System.Collections.Generic;
using UnityEngine;

public class ActivatorRadioGroup : MonoBehaviour
{
    [SerializeField]
    private List<Activator> activators = new List<Activator>();



#if UNITY_EDITOR
    private void OnValidate()
    {
        for (int i = activators.Count - 1; i >= 0; i--)
        {
            if (activators[i] == null)
            {
                activators.RemoveAt(i);
                continue;
            }

            if (activators[i].radioGroup == this)
                continue;

            activators[i].radioGroup = this;
        }
    }
#endif


    public void ActivateOnly(Activator activator)
    {
        foreach (Activator acti in activators)
        {
            if (acti != null && acti != activator)
                acti.Deactivate();
        }
    }

}
