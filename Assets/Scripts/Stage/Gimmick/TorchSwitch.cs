using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSwitch : SwitchBase
{
    public List<Torch> torchs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsActivate)
        {
            int i = 0;
            foreach(Torch torch in torchs)
            {
                if(torch.burn == false)
                {
                    i++;
                }
            }

            if (i == torchs.Count) IsActivate = true;
        }
        
    }
        
}
