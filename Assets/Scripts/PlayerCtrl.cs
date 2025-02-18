using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerCtrl : MonoBehaviour
{
    Rigidbody rigid;

    [Header("Control")]
    public bool canMove = true;
    public bool canLook = true;
    [SerializeField] Transform duckBase;
    [SerializeField] Vector3 inputDirection;
    [SerializeField] float sensitivity = 1f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpPower = 1f;


    [Space(10f)][Header("Camera")]
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] Transform mainCam;
    [SerializeField] Transform camFollow;
    [SerializeField] Vector2 look;
    float xRot;
    float yRot;



    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CameraRotation();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float speed = 0;
        float targetRotation = 0;
        Vector3 direction = new Vector3(inputDirection.x, 0, inputDirection.y);
        if (inputDirection != Vector3.zero)
        {
            speed = moveSpeed;
            targetRotation = Quaternion.LookRotation(direction).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3);

            Quaternion rotationBase = Quaternion.Euler(inputDirection.y, 0, inputDirection.x);
            duckBase.rotation = Quaternion.Slerp(transform.rotation, rotationBase, Time.deltaTime * 3);
        }
        Vector3 TargetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
        rigid.AddForce(TargetDirection * speed + new Vector3(0, rigid.velocity.y, 0), ForceMode.Acceleration);

        //duckBase.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime);
    }

    void CameraRotation()
    {
        if (!canLook) { return; }
        xRot -= look.y * sensitivity * 0.1f;
        yRot += look.x * sensitivity * 0.1f;
        xRot = Mathf.Clamp(xRot, 10f, 65f);
        Quaternion rot = Quaternion.Euler(xRot, yRot, 0);
        camFollow.rotation = rot;
    }

    void Jump()
    {
        rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    #region INPUT
    public void InputMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }
    public void InputLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
    public void InputJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }
    }
    #endregion
}
