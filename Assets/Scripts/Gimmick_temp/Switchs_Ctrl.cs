using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchs_Ctrl : MonoBehaviour
{
    public enum SwitchType {Maintain, OnOff}
    public SwitchType _type = SwitchType.OnOff;

    [HideInInspector]
    public Switch_C_OBJ _connetObj;

    [HideInInspector]
    public bool isActivate = false;
    
    public Material[] _color_DontTouch;
    Renderer _thisColor;
    LayerMask layers;

    // Start is called before the first frame update
    void Start()
    {
        _thisColor = GetComponent<Renderer>();
        layers = LayerMask.GetMask("Player", "Object", "Minimi");
        
    }

    public void Connecting(Switch_C_OBJ connectingObj)
    {
        _connetObj = connectingObj;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        

        if ((_type == SwitchType.Maintain && !isActivate) || _type == SwitchType.OnOff)
            {

            RaycastHit hit;

            isActivate = Physics.BoxCast(
                new Vector3(transform.position.x, transform.position.y - (transform.lossyScale.y / 2), transform.position.z),
                transform.lossyScale / 2, 
                Vector3.up,
                out hit, 
                transform.rotation,
                (transform.lossyScale.y/2) + 0.5f,
                layers, 
                QueryTriggerInteraction.Ignore);
            
            }

        


        if (isActivate)
        {
            
            _thisColor.material = _color_DontTouch[0];
        }
        if (!isActivate)
        {
            _thisColor.material = _color_DontTouch[1];
        }

        _connetObj.SwitchCheck();
    }

}