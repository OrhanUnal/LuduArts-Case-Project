using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] private float m_Speed = 3.0f;
    [SerializeField] private float m_MaxInteractDistance = 10f;
    [SerializeField] private float m_Drag = 30f;
    [SerializeField] private float m_MaxSpeed = 100f;
    [SerializeField] private float m_JumpSpeed = 1f;
    [SerializeField] private float m_GravityForPlayer = -9.81f;
    [SerializeField] private float m_Sensitivity = 1f;

    [SerializeField] private AnimationCurve m_JumpCurve;

    private CharacterController m_CharacterController;
    private PlayerInput m_PlayerInput;
    private float m_Pitch;

    private bool m_CanMove;

    // Input Actions
    private InputAction m_MouseInput;
    private InputAction m_InteractActionButton;
    private InputAction m_InteractActionHold;
    private InputAction m_WalkAction;
    private InputAction m_JumpAction;

    // Movement variables
    private Vector3 m_Move;
    private bool m_IsInJump;
    private float m_ElapsedJumpTime;
    private float m_VelocityAtJumpStart;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        m_ElapsedJumpTime = 0f;
        m_IsInJump = false;

        m_CharacterController = GetComponent<CharacterController>();
        m_PlayerInput = GetComponent<PlayerInput>();

        m_InteractActionButton = m_PlayerInput.actions["InteractButton"];
        m_InteractActionHold = m_PlayerInput.actions["InteractHold"];
        m_WalkAction = m_PlayerInput.actions["Walk"];
        m_JumpAction = m_PlayerInput.actions["Jump"];
        m_MouseInput = m_PlayerInput.actions["Look"];
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_InteractActionButton.performed += InteractSomething;
        m_InteractActionHold.performed += InteractSomething;
        m_JumpAction.started += HandleJump;
        m_JumpAction.canceled += HandleJump;
    }

    private void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
        HandleGravity();
    }

    #endregion

    #region Movement Logic

    private void HandleRotation()
    {
        Vector2 mouseDelta = m_MouseInput.ReadValue<Vector2>();

        float mouseXRotation = mouseDelta.x * m_Sensitivity;
        transform.Rotate(0f, mouseXRotation, 0f);

        m_Pitch -= mouseDelta.y * m_Sensitivity;
        m_Pitch = Mathf.Clamp(m_Pitch, -180f, 180f);
    }

    private void HandleMovement()
    {
        if (m_IsInJump && m_ElapsedJumpTime < 0.4f)
        {
            m_ElapsedJumpTime += Time.deltaTime;
            m_Move.y = Mathf.Lerp(
                m_VelocityAtJumpStart,
                m_JumpSpeed,
                m_JumpCurve.Evaluate(m_ElapsedJumpTime)
            );
        }

        Vector2 walkInput = m_WalkAction.ReadValue<Vector2>();

        m_Move.x = walkInput.x;
        m_Move.z = walkInput.y;

        m_Move = transform.rotation * m_Move;

        Vector3 newVelocity = m_CharacterController.velocity + m_Move * m_Speed;
        Vector3 currentDrag = newVelocity.normalized * m_Drag * Time.deltaTime;

        newVelocity = (newVelocity.magnitude > m_Drag * Time.deltaTime)
            ? newVelocity - currentDrag
            : Vector3.zero;

        newVelocity = Vector3.ClampMagnitude(newVelocity, m_MaxSpeed);
        m_CharacterController.Move(newVelocity * Time.deltaTime);
    }

    private void HandleGravity()
    {
        if (!m_CharacterController.isGrounded)
            m_Move.y += m_GravityForPlayer * Time.deltaTime;
        else
            m_Move.y = -0.2f;
    }

    #endregion

    #region Input Callbacks

    private void HandleJump(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            m_IsInJump = false;
            m_Move.y = 0f;
            m_ElapsedJumpTime = 0f;
        }
        else
        {
            m_VelocityAtJumpStart = m_CharacterController.velocity.y;
            m_IsInJump = true;
        }
    }

    private void InteractSomething(InputAction.CallbackContext context)
    {
        Ray interactRay = new Ray
        {
            origin = transform.position,
            direction = transform.forward
        };
        if (Physics.Raycast(interactRay, out RaycastHit hitInfo, m_MaxInteractDistance))
        {
            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if (context.action.name == "InteractHold")
                    interactable.InteractLogicHold();
                else
                    interactable.InteractLogicButton();
            }
        }
    }

    #endregion
}