using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    public GameObject item;
    public GameObject inticon;
    public GameObject icon;
    public AudioSource pick;
    private PlayerInputActions inputActions;
    private bool interactPressed = false;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.Interact.canceled += ctx => interactPressed = false;

    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        interactPressed = true;
    }

    private void OnTriggerStay(Collider other)
    {
        // pick
        if (other.CompareTag("MainCamera"))
        {
            inticon.SetActive(true);
            if (interactPressed)
            {
                if (pick != null) pick.Play();
                inticon.SetActive(false);
                item.SetActive(false);
                if (icon != null) icon.SetActive(true);
                interactPressed = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            inticon.SetActive(false);
        }
    }
}
