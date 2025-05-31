using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    [SerializeField] private SceneField mainMenu;
    [SerializeField] private GameObject menuButtons;

    private bool active;

    private void Start()
    {
        TogglePauseMenu(false);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f; //just in case
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePauseMenu(!active);
        }
    }

    public void TogglePauseMenu(bool on)
    {
        active = on;
        menuButtons.SetActive(on);
        Time.timeScale = on ? 0f : 1f;
        Cursor.lockState = on ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = on;
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
