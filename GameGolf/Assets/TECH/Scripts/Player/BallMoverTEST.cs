using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoverTEST : MonoBehaviour
{
    private Rigidbody m_rigidBody = null;
    

    private void Start()
    {
        m_rigidBody = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_rigidBody.AddForce(transform.forward, ForceMode.Impulse);
           
            
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_rigidBody.AddForce(-transform.forward, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_rigidBody.AddForce(transform.right, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_rigidBody.AddForce(-transform.right, ForceMode.Impulse);
        }

    }
}
