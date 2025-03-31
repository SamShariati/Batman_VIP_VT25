using UnityEngine;

public class OpenDoorScript : MonoBehaviour
{
    public string requiredKeyID;
    public float openRange = 5f;
    private AudioSource doorOpeningSound;
    private Animator doorAnimator;

    //[SerializeField] 
    private GameObject player;
    //[SerializeField] 
    private PickUpScript pickUpScript;

    void Start()
    {
        doorOpeningSound = GetComponent<AudioSource>();
        doorAnimator = GetComponent<Animator>();

        // Find the player using the tag if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        // Find the PickUpScript on the Main Camera under Player
        if (pickUpScript == null)
        {
            Transform cameraTransform = player?.transform.Find("Camera Pos/Main Camera");
            if (cameraTransform != null)
            {
                pickUpScript = cameraTransform.GetComponent<PickUpScript>();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        if (player != null && Vector3.Distance(player.transform.position, transform.position) <= openRange)
        {
            if (pickUpScript != null && !string.IsNullOrEmpty(pickUpScript.heldKeyID) && pickUpScript.heldKeyID == requiredKeyID)
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("You need the correct key to open this door!");
            }
        }
    }

    void OpenDoor()
    {

        AudioManager.instance.PlayQuakeSound();

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("OpenDoor");
        }
    }
}
