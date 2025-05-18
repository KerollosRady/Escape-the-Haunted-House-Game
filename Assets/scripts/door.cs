using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public GameObject door_closed, intText, lockedText, door_key;
    public AudioSource open, close;
    public bool opened = false, locked = false;
    private GameObject errorSoundObject;
    public AudioSource errorSound;
    private Quaternion _closedRotation, _openRotation;
    private Coroutine _currentCorotine;
    public float openAngle = 80f;
    public float openSpeed = 3f;
    private PlayerInputActions inputActions;
    private bool interactPressed = false;

    public bool isMainDoor = false, missionFinished = false;
    public AudioSource missionCompletedSound;
    public GameObject missionCompletedImage;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Interact.performed += OnDoorInteract;
        inputActions.Player.Interact.canceled += ctx => interactPressed = false;
    }
    private void Start()
    {
        _closedRotation = transform.rotation;
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        errorSoundObject = GameObject.Find("error sound");
        if (errorSoundObject != null && errorSound == null)
            errorSound = errorSoundObject.GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnDoorInteract(InputAction.CallbackContext context)
    {
        interactPressed = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (opened) return;
        if (other.CompareTag("MainCamera"))
        {
            if (locked && door_key.activeSelf)
            {
                lockedText.SetActive(true);
                if (interactPressed && errorSound != null) errorSound.Play();
            }
            else
            {
                intText.SetActive(true);
                if (interactPressed)
                {
                    if (_currentCorotine != null) StopCoroutine(_currentCorotine);
                    _currentCorotine = StartCoroutine(ToggleDoor());
                    interactPressed = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(false);
            if (lockedText != null)
                lockedText.SetActive(false);
        }
    }

    private IEnumerator ToggleDoor()
    {
        Quaternion targetRotation;
        if (!opened)
        {
            targetRotation = _openRotation;
            if (open != null) open.Play();
            Debug.Log($"Open door isMainDoor: {isMainDoor} missionFinised: {missionFinished}");

            if (isMainDoor && !missionFinished)
            {
                Debug.Log("Mission Completed");
                missionFinished = true;
                missionCompletedSound.Play();
                missionCompletedImage.SetActive(true);
            }
        }
        else
        {
            targetRotation = _closedRotation;
            if (close != null) close.Play();
        }
        opened = !opened;
        intText.SetActive(false);
        // door_closed.transform.Find("door (closed)").GetComponent<Collider>().enabled = false;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }
        // door_closed.transform.Find("door (closed)").GetComponent<Collider>().enabled = true;
        transform.rotation = targetRotation;
    }
    void Update()
    {
        if (isMainDoor && !missionCompletedSound.isPlaying) missionCompletedImage.SetActive(false);
    }
}