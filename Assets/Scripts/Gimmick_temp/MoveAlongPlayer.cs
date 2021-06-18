using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongPlayer : MonoBehaviour
{
    

    //플레이어와 발판이 같이 가게 만들기 위한 스크립트
    //움직이는 발판에 넣어야함

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Minimi")
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Minimi")
        {
            collision.gameObject.transform.parent = null;
        }
    }

}
