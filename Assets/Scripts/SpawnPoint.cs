using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Color color = Color.yellow;
    public bool drawChildSpawns = true;
    public bool drawSpawns = true;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DrawSpawns()
    {
        transform.ForEveryChild(DrawSpawnsInRoom);
    }

    private void DrawSpawnsInRoom(Transform spawn)
    {
        spawn.ForEveryChild((x) =>
        {
            if (Physics.Raycast(x.position+Vector3.up, Vector3.down, out RaycastHit hit, 500))
            {
                Gizmos.color = color;
                Gizmos.DrawLine(x.position, hit.point);
                Gizmos.DrawSphere(hit.point, 1);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(x.position, 1);
            }

        });
    }

    private void OnDrawGizmos()
    {
        if(drawChildSpawns) DrawSpawns();
        if(drawSpawns) DrawSpawnsInRoom(transform);
    }
}
