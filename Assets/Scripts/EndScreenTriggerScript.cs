using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenTriggerScript : MonoBehaviour
{
    [SerializeField] Animator whiteOutPanel;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the trigger: " + other.name);
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player has reached the end of the level!");
            StartCoroutine(WhiteOut());
        }
    }
    IEnumerator WhiteOut()
    {
        Debug.Log("Whteout c");
        whiteOutPanel.SetTrigger("WhiteOut");
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("EndScreen");
    }
}
