
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPauseMenu : MonoBehaviour
{
    public SceneField hudScene;
    bool paused = false;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused) Load();
            else Unload();
        }
    }

    [MakeButton]
    public void Load()
    {

        Time.timeScale = 0f;

        Scene scene = SceneManager.GetSceneByName(hudScene);
        if (!scene.IsValid())
        {
            SceneManager.LoadSceneAsync(hudScene, LoadSceneMode.Additive);
            paused = true;
        }
        
        
    }


    [MakeButton]
    public void Unload()
    {

        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync(hudScene);
        paused = false;
        
    }

    private void OnDisable()
    {
        if(paused)
            Unload();
    }

}