using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongPlayer : MonoBehaviour
{
    

    //�÷��̾�� ������ ���� ���� ����� ���� ��ũ��Ʈ
    //�����̴� ���ǿ� �־����

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
