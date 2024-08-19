using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector; // Thêm namespace Odin Inspector
using UnityEngine;
using UnityEngine.UI;

public class PufferFishController : SerializedMonoBehaviour
{
    #region Serialized Fields

    [TitleGroup("Character Settings")]
    [FoldoutGroup("Character Settings/Movement Settings")]
    [SerializeField] private Rigidbody rb;
    [FoldoutGroup("Character Settings/Movement Settings")]
    [SerializeField] private List<MagnetHover> magnetHover;

    [FoldoutGroup("Character Settings/Rotation Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [FoldoutGroup("Character Settings/Rotation Settings")]
    [SerializeField] private float rotationSpeed = 300f;
    [FoldoutGroup("Character Settings/Rotation Settings")]
    [SerializeField] private Quaternion originalRotation;
    private Quaternion targetRotation;

    [FoldoutGroup("Character Settings/Animation Settings")]
    [SerializeField] private Animator anim;
    [FoldoutGroup("Character Settings/Animation Settings")]
    [SerializeField] private Transform pufferChar;

    [TitleGroup("Camera Settings")]
    [SerializeField] private Camera mainCam;
    [TitleGroup("Camera Settings")]
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    [FoldoutGroup("Camera Settings/Zoom Settings")]
    [SerializeField] private float zoomSpeed = 2f;
    [FoldoutGroup("Camera Settings/Zoom Settings")]
    [SerializeField] private float minZoom = 15f;
    [FoldoutGroup("Camera Settings/Zoom Settings")]
    [SerializeField] private float maxZoom = 90f;
    
    
    [FoldoutGroup("Stats")]
    public float groundCheckDis = 2.5f;
    [FoldoutGroup("Stats")]
    public float popScale = 1.5f;
    [FoldoutGroup("Stats")]
    public float airstrafe = 0.2f;
    [FoldoutGroup("Stats")]
    [SerializeField] private float speed = 5f;
    [FoldoutGroup("Stats")]
    [SerializeField] private float boostForce = 5f;
    [FoldoutGroup("Stats/Jump")]
    [SerializeField] private float jumpForce = 5f;


    [FoldoutGroup("Debug")]
    public bool popping, isGrounded, allowFootPlacement;
    [FoldoutGroup("Debug")]
    public float valueX, valueY;
    [FoldoutGroup("Debug")]
    [ReadOnly] [SerializeField] float currentSpeed;
    private RaycastHit hit;
    private Vector3 defaultScale;
    private CamShake camShaker;

    #endregion



    #region Unity Methods

    private void Awake()
    {
        defaultScale = anim.transform.localScale;
        camShaker = CamShake.Instance;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (rb == null) 
            rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        currentSpeed = rb.velocity.magnitude;
        
        HandleInput();
        HandleZoom();

        GroundChecker();
        if (popping) return;

        MoveCharacter();
        GravityExtra();
        RotateCharacter();

    }


    #endregion

    #region Private Methods

    private void GravityExtra()
    {
        rb.AddForce(Vector3.down * 9.81f, ForceMode.Acceleration);
    }
    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!popping)
                StartPopping();
            else
                StopPopping();
        }
    }

    private void StartPopping()
    {
        //rb.constraints = RigidbodyConstraints.None;

        foreach (var magnet in magnetHover)
        {
            magnet.Activated = false;
        }
        
        popping = true;
        anim.SetBool("Popping", true);
    }

    public void ProcessBouncyTransform()
    {
        anim.transform.localScale = defaultScale * popScale;
        Jump(jumpForce);
        if (valueX != 0 || valueY != 0)
        {
            Vector3 direction = rb.velocity.normalized;
            rb.AddForce(direction * boostForce, ForceMode.Impulse);
        }
    }

    private void StopPopping()
    {
        anim.transform.localScale = defaultScale;
        
        foreach (var magnet in magnetHover)
        {
            magnet.Activated = true;
        }
        
        Jump(4);
        StartCoroutine(Balance());
        popping = false;
        anim.SetBool("Popping", false);
    }

    private void RotateCharacter()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pufferChar.Rotate(Vector3.up * mouseX);
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newFOV = freeLookCamera.m_Lens.FieldOfView - scrollInput * zoomSpeed;
        freeLookCamera.m_Lens.FieldOfView = Mathf.Clamp(newFOV, minZoom, maxZoom);
    }

    private void MoveCharacter()
    {
        valueX = Input.GetAxis("Horizontal");
        valueY = Input.GetAxis("Vertical");

        Vector3 movementInput = new Vector3(valueX, 0f, valueY).normalized;
        Vector3 moveDirection = mainCam.transform.forward.normalized * movementInput.z + mainCam.transform.right.normalized * movementInput.x;
        moveDirection.y = 0f;
        
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            pufferChar.localRotation = Quaternion.Slerp(pufferChar.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        Vector3 moveVelocity = moveDirection * speed;
        Vector3 move = new Vector3(moveVelocity.x, 0, moveVelocity.z);
        
        if (popping)
            move = new Vector3(moveVelocity.x * airstrafe, 0, moveVelocity.z * airstrafe);
        
        rb.AddForce(move, ForceMode.Acceleration);
    }

    public void Jump(float jumpForceParam)
    {
        //if(!isGrounded) return;
        rb.AddForce(Vector3.up * jumpForceParam, ForceMode.Impulse);
        
        rb.AddTorque((transform.right + transform.up) * 2f, ForceMode.Impulse);
    }

    private IEnumerator Balance()
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        Quaternion startRotation = rb.rotation;
        startRotation = Quaternion.Normalize(startRotation);
        Quaternion targetRotation = Quaternion.Normalize(Quaternion.identity);

        while (elapsedTime < duration)
        {
            anim.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            rb.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.rotation = targetRotation;
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    
    void GroundChecker()
    {
        isGrounded = (Physics.SphereCast(transform.position, 0.1f,
                Vector3.down, out hit, groundCheckDis,
                        LayerMask.GetMask("Ground")));

        if (isGrounded && !popping)
        {
            allowFootPlacement = (Physics.SphereCast(transform.position, 0.1f,
                -transform.up, out hit, groundCheckDis,
                LayerMask.GetMask("Ground")));
        }
        else 
            allowFootPlacement = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collide: " + other.gameObject.name);
        if (currentSpeed > 10f && !isGrounded)
        {
            camShaker.ActivateShake();
            Debug.Log("shake");
        }
    }

    #endregion
}
