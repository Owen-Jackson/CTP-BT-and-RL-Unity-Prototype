using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Actor {
    private float rotSpeed = 5.0f;
    private Vector3 acceleration;

    [SerializeField]
    private bool isUnderAttack = false;
    public bool IsUnderAttack
    {
        get
        {
            return isUnderAttack;
        }
        set
        {
            isUnderAttack = value;
        }
    }

    void Update()
    {
        //basic keyboard and mouse movement
        float rotAngle = Input.GetAxisRaw("Mouse X") * rotSpeed;
        transform.Rotate(Vector3.up, rotAngle);

        float right = Input.GetAxis("Horizontal");
        float forward = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(right, 0, forward) * Time.deltaTime * moveSpeed;

        transform.Translate(movement);
    }
}
