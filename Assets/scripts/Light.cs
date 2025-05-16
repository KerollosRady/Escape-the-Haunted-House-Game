using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Light : MonoBehaviour
{
    public GameObject flash;
    public GameObject player_light;
    private PlayerInputActions inputActions;
    private bool turnOn = false;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Flash.performed += OnFlashInteract;
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void Start()
    {
        player_light.SetActive(turnOn);
    }

    private void OnFlashInteract(InputAction.CallbackContext context)
    {
        if (!flash.activeSelf)
        {
            turnOn = !turnOn;
            player_light.SetActive(turnOn);
        }
    }
}
