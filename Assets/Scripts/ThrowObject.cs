using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    [SerializeField] private Transform lookDir;
    //[SerializeField] private Rigidbody throwPrefab;
    [SerializeField] private float throwAngle = 15;
    [SerializeField] private float collideDist = 1.5f;
    [SerializeField] private float throwForce = 15;
    [SerializeField] private float torqueForce = 1;


    bool hasThrown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public T DoThrow<T>(T throwPrefab, float throwForce = 15, float torqueForce = 1) where T : MonoBehaviour
    {
        Debug.Assert(throwPrefab, "Dont call this with null!");
        hasThrown = true;
        // animator.SetBool(throwHash, false);
        //Quaternion q = Quaternion.Euler(-throwAngle, 0f, 0f);
        Quaternion q = Quaternion.AngleAxis(-throwAngle, lookDir.right);
        Vector3 dir = q * lookDir.forward;
        Vector3 pos = lookDir.position;
        if (Physics.Raycast(pos, dir, out RaycastHit hit, collideDist))
        {
            pos = hit.point;
        }
        else
        {
            pos += dir * collideDist;
        }

        T go = Instantiate(throwPrefab, pos, Quaternion.identity);

        Rigidbody rb = go.GetComponent<Rigidbody>();
        //rb.velocity = dir * throwForce;
        if (rb)
        {
            rb.AddForce(dir * throwForce, ForceMode.Impulse);
            rb.AddTorque(new Vector3(torqueForce, 0, torqueForce), ForceMode.Impulse);
            rb.isKinematic = false;
        }
        return go;
    }
}
