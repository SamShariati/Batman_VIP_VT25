using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CheckLooseCondition : MonoBehaviour
{

    [SerializeField] SceneField looseScene;
    [SerializeField] Transform spider;
    [SerializeField] Transform spiderLookAt; //insane
    public bool ignore = false;
    [SerializeField] float distToLoose = 6; //spider fat -> should be ~6
    [SerializeField] float distToFace = 4; //spider fat -> should be ~6
    [SerializeField] float turnTowardSpiderSpeed = 720; //deg per sec
    [SerializeField] float timeBeforeEndScreen = 4; //deg per sec
    PlayerController playerController;
    Transform cam;

    SpiderBTManager bt;
    NavMeshAgent agent;
    bool dead = false;


    void Start()
    {
        if (!spider)
        {
            bt = FindAnyObjectByType<SpiderBTManager>();
            if (bt)
            {
                spider = bt.transform;
                agent = bt.GetComponent<NavMeshAgent>();
            }
        }
        
        
        if (spider) {
            bt = spider.GetComponent<SpiderBTManager>();
            agent = spider.GetComponent<NavMeshAgent>();
            AttackPlayer.Attack += Die;
        }
        else
        {
            Debug.LogWarning("No spider set = you cannot loose!!");
            ignore = true;
            enabled = false;
        }



        playerController = GetComponent<PlayerController>();
        cam = Camera.main.transform; //pff
    }

    // Update is called once per frame
    void Update()
    {
        if(ignore) return;


        //if (transform.position.InRangeOf(spider.position, distToLoose))
        //{
        //    Die();
        //}
    }

    public void Die()
    {
        if (dead) return;
        //AttackPlayer.Attack -= Die;
        dead = true;
        //Debug.LogError("YOU LOOSE!");
        //do some stuff
        playerController.enabled = false; //player is dead dont do inputs
        ignore = true;

        //Debug.Log("Tell spider to standf still! at correct distance from player");
        if (agent) agent.enabled = false;
        bt.enabled = false;

        //record "optimal offset"
        Vector3 playerPos = transform.position;
        Vector3 spiderPos = spider.position;
        //compute spider to be distToLoose From player
        //av nÂgon anledning ‰r spindel 6m upp i luften som default VARF÷÷÷÷÷R?????!!!?
        Vector3 spiderPos2 = Vector3.ProjectOnPlane(spiderPos - playerPos, Vector3.up).normalized * distToFace; //do alot of math because offset
        spiderPos = new Vector3(playerPos.x + spiderPos2.x, spiderPos.y, playerPos.z + spiderPos2.z);
        spider.position = spiderPos; //ouch the snapping

        //tell the animator to play some clip?

        StartCoroutine(Look());
    }

    IEnumerator Look()
    {
        float t = 0;
        while(t < timeBeforeEndScreen)
        {
            Vector3 fwd = spiderLookAt.position - cam.position;
            Quaternion lookAt = Quaternion.LookRotation(fwd);
            cam.rotation = Quaternion.RotateTowards(cam.rotation, lookAt, Time.deltaTime * turnTowardSpiderSpeed);

            spider.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(-fwd, Vector3.up)); //spider vertical offset is insane
            

            t += Time.deltaTime;
            yield return null;
        }
        //Debug.LogError("YOU LOOSE!"); //emergency pause

        if (!string.IsNullOrEmpty(looseScene))
        {
            SceneManager.LoadScene(looseScene);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload current scene if loose scene not set
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
