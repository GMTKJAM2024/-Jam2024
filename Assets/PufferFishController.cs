using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector; // Thêm namespace Odin Inspector
using UnityEngine;

public class PufferFishController : MonoBehaviour
{
    #region Serialized Fields

    [TitleGroup("Character Settings")]
    [FoldoutGroup("Character Settings/Movement Settings")]
    [SerializeField] private Rigidbody rb;
    [FoldoutGroup("Character Settings/Movement Settings")]
    [SerializeField] private MagnetHover magnetHover;
    [FoldoutGroup("Character Settings/Movement Settings")]
    [SerializeField] private float speed = 5f;
    [FoldoutGroup("Character Settings/Movement Settings")]
    [SerializeField] private float boostForce = 5f;

    [FoldoutGroup("Character Settings/Rotation Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [FoldoutGroup("Character Settings/Rotation Settings")]
    [SerializeField] private float rotationSpeed = 300f;
    [FoldoutGroup("Character Settings/Rotation Settings")]
    [SerializeField] private Quaternion originalRotation;
    private Quaternion targetRotation;

    [FoldoutGroup("Character Settings/Jump Settings")]
    [SerializeField] private float jumpForce = 5f;

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

    #endregion

    #region Private Fields

    private bool isGrounded;
    private bool popping;
    private float horizontal;
    private float vertical;

    #endregion

    #region Unity Methods

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        if (popping)
        {
            return;
        }

        GravityExtra();
        RotateCharacter();
        MoveCharacter();

    }

    private void FixedUpdate()
    {
        HandleInput();
        HandleZoom();
    }

    #endregion

    #region Private Methods

    private void GravityExtra()
    {
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * 9.81f, ForceMode.Acceleration);
        }
    }
    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Pop");
            if (!popping)
            {
                StartPopping();
            }
            else
            {
                StopPopping();
            }
        }
    }

    private void StartPopping()
    {
        rb.constraints = RigidbodyConstraints.None;
        magnetHover.gameObject.SetActive(false);
        popping = true;
    }

    public void ProcessBouncyTransform()
    {
        anim.transform.localScale *= 1.2f;
        if (horizontal != 0 || vertical != 0)
        {
            Jump(jumpForce);
            Vector3 direction = rb.velocity.normalized;
            rb.AddForce(direction * boostForce, ForceMode.Impulse);
        }
        else
        {
            Jump(jumpForce);
        }

    }

    private void StopPopping()
    {
        anim.transform.localScale /= 1.2f;
        magnetHover.gameObject.SetActive(true);
        Jump(4);
        StartCoroutine(RotateBackToOriginal());
        popping = false;
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
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 movementInput = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveDirection = mainCam.transform.forward * movementInput.z + mainCam.transform.right * movementInput.x;
        moveDirection.y = 0f;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            pufferChar.localRotation = Quaternion.Slerp(pufferChar.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        Vector3 moveVelocity = moveDirection * speed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    private void Jump(float jumpForceParam)
    {
        rb.AddForce(Vector3.up * jumpForceParam, ForceMode.Impulse);
    }

    private IEnumerator RotateBackToOriginal()
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
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    #endregion
}
