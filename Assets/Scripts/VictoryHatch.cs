using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryHatch : MonoBehaviour
{

    [SerializeField] SceneField victoryScene;



    void Start()
    {
        Chest chest = GetComponent<Chest>();
        chest.OnOpened += Chest_OnOpened;
    }

    private void Chest_OnOpened()
    {
        Debug.Log("Victory");
        SceneManager.LoadScene(victoryScene);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
