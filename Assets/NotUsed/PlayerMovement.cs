using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller;
    public Transform GroundCheck;
    private float GroundDistance = 0.4f;
    public LayerMask GroundMask;

    bool Grounded;

    private float speed = 10f;
    private float jumpheight = 1.5f;
    private float gravity = -19.62f;

    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        Grounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (Grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x +transform.forward * z;
        Controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && Grounded)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        Controller.Move(velocity * Time.deltaTime);
    }
}
