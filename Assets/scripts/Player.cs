using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    private PlayerInputActions inputActions;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool isJumping;
    private bool isRunning;
    [HideInInspector]
    public bool canMove = true;
    public GameObject exitWindow;
    Button exitYesButton, exitNoButton;
    bool exitOn = false;
    public AudioSource popSound;
    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => movementInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Jump.started += ctx => isJumping = true;
        inputActions.Player.Jump.canceled += ctx => isJumping = false;

        inputActions.Player.Sprint.started += ctx => isRunning = true;
        inputActions.Player.Sprint.canceled += ctx => isRunning = false;

        inputActions.UI.Esc.performed += escape;
        exitYesButton = exitWindow.transform.Find("Yes").GetComponent<Button>();
        exitNoButton = exitWindow.transform.Find("No").GetComponent<Button>();

        exitYesButton.onClick.AddListener(ExitYES);
        exitNoButton.onClick.AddListener(ExitNO);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        exitWindow.SetActive(false);
    }
    private void escape(InputAction.CallbackContext context){
        exitOn = !exitOn;
        if (exitOn) {
            if (popSound != null) popSound.Play();
            exitWindow.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inputActions.Player.Disable();
        } else {
            ExitNO();
        }
    }
    private void ExitYES() {
        if (popSound != null) popSound.Play();
        SceneManager.LoadScene("MainMenu");
    }
    private void ExitNO() {
        if (popSound != null) popSound.Play();
        exitOn = false;
        exitWindow.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputActions.Player.Enable();
        EventSystem.current.SetSelectedGameObject(null);
    }
    void Update()
    {
        // Debug.Log($"Move: {movementInput}, Look: {lookInput}, Jump: {isJumping}, Run: {isRunning}");
        // Calculate direction
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        float speed = isRunning ? runningSpeed : walkingSpeed;
        Vector3 desiredMove = (forward * movementInput.y + right * movementInput.x) * speed;

        float movementDirectionY = moveDirection.y;
        moveDirection = desiredMove;

        if (characterController.isGrounded)
        {
            if (isJumping && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = 0f;
            }
        }
        else
        {
            moveDirection.y = movementDirectionY;
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -lookInput.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.Rotate(Vector3.up * lookInput.x * lookSpeed);
        }
    }
}
