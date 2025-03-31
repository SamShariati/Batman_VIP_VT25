using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton, menuButtons;

    private bool active;

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

        pauseButton.SetActive(!on);
        menuButtons.SetActive(on);
    }
}
