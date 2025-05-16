using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string level_name;
    public GameObject main_menu;
    Button play_button, controls_button, exist_button;
    public GameObject controls_text;
    bool controlsOn = false;
    private void Awake()
    {
        Debug.Log("start");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        play_button = main_menu.transform.Find("Play").GetComponent<Button>();
        controls_button = main_menu.transform.Find("Controls").GetComponent<Button>();
        exist_button = main_menu.transform.Find("Exit").GetComponent<Button>();

        play_button.onClick.AddListener(OnPlayClicked);
        exist_button.onClick.AddListener(OnExitClicked);
        controls_button.onClick.AddListener(OnControlsCliked);
    }
    private void OnPlayClicked() {
        SceneManager.LoadScene(level_name);
    }
    void ChangeContorlsColor(Color c){
        ColorBlock cb = controls_button.colors;
        cb.selectedColor = cb.normalColor = c;
        controls_button.colors = cb;
    }
    private void OnControlsCliked() {
        controlsOn = !controlsOn;
        if (controlsOn)
            ChangeContorlsColor(new Color(90f / 255f, 45f / 255f, 45f / 255f, 1f));
        else
            ChangeContorlsColor(new Color(230f / 255f, 40f / 255f, 40f / 255f, 1f));
        controls_text.SetActive(controlsOn);
    }
    private void OnExitClicked() {
        Application.Quit();
    }
}