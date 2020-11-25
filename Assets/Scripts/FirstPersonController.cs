using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class FirstPersonController : MonoBehaviour {
    private float m_WalkSpeed = 7.5f;
    private float m_JumpSpeed = 7.5f;
    private float m_StickToGroundForce = 10;
    private float m_GravityMultiplier = 2;
    private MouseLook m_MouseLook;

    private Camera m_Camera;
    private bool m_Jump;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private bool m_Jumping;

    // Use this for initialization
    private void Start()
    {
        m_MouseLook = GetComponent<MouseLook>();
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_Jumping = false;
		m_MouseLook.Init(transform , m_Camera.transform);
    }


    // Update is called once per frame
    private void Update()
    {
        if (!MouseLook.PauseCameraMovement)
        {
            RotateView();
        }

        // the jump state needs to read here to make sure it is not missed
        if (!m_Jump && !m_Jumping)
        {
            m_Jump = Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space")));
        }

        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            m_MoveDir.y = 0f;
            m_Jumping = false;
        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }


    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

        m_MoveDir.x = desiredMove.x*speed;
        m_MoveDir.z = desiredMove.z*speed;

        if (m_CharacterController.isGrounded)
        {
            m_MoveDir.y = -m_StickToGroundForce;

            if (m_Jump)
            {
                m_MoveDir.y = m_JumpSpeed;
                m_Jump = false;
                m_Jumping = true;
            }
        }
        else
        {
            if (transform.position.y <= -25)
            {
                transform.position = new Vector3(0, 4, 0);
                return;
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
        }
        m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);
    }

    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        speed = m_WalkSpeed;
        m_Input = new Vector2(horizontal, vertical);

    }

    private void RotateView()
    {
        m_MouseLook.LookRotation (transform, m_Camera.transform);
    }
}
