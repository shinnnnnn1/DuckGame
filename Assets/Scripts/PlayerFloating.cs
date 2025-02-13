using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloating : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    void Start()
    {
        //rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(rigid.position.y < 0)
        {
            Debug.Log("up");
            float a = -transform.position.y;
            rigid.AddForce(new Vector3(0, Mathf.Abs(Physics.gravity.y) * a * 2, 0), ForceMode.Acceleration);
        }
    }
}
