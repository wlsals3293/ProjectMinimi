using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchs_Ctrl : MonoBehaviour
{
    public Switch_C_OBJ _connetObj;
    public bool isActivate = false;
    public int collisionCount = 0;
    public Material[] _color;
    public Renderer _thisColor;
    public LayerMask layers;

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
        RaycastHit hit;
        isActivate = Physics.BoxCast(
            new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z),
            transform.lossyScale / 2,Vector3.up, 
            out hit, transform.rotation, 1.2f, 
            layers, QueryTriggerInteraction.Ignore);

        if (isActivate)
        {
            
            _thisColor.material = _color[0];
        }
        if (!isActivate)
        {
            _thisColor.material = _color[1];
        }

        _connetObj.SwitchCheck();
    }

    
    /*void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Respawn")
        {
            collisionCount++;
            isActivate = true;
            _connetDoor.SwichCheck();
            _thisColor.material = _color[0];
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Respawn")
        {
            collisionCount--;
            if(collisionCount == 0)
            {
                isActivate = false;
                _thisColor.material = _color[1];
            }

        }
    }
    */
}