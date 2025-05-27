using UnityEngine;

public class SpiderVision : MonoBehaviour
{
    [Header("Vision Settings")]
    public float visionRange = 10f;
    public float visionDistance = 15f;  
    public float visionHeight = 0.5f;   
    public float visionAngle = 60f;
    public LayerMask obstacleLayerMask = -1;
    public LayerMask playerLayerMask = -1;

    [Header("Debug Visualization")]
    public bool showVisionCone = true;
    public Color visionConeColor = Color.yellow;
    public Color detectionColor = Color.red;

    [Header("Detection Status")]
    [SerializeField] private bool _playerDetected = false;

    public bool playerIsVisible { get { return _playerDetected; } }

    private Transform player;

    void Start()
    {

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player != null)
        {
            CheckPlayerInCone();
        }
    }

    void CheckPlayerInCone()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Step 1: Check if player is within distance
        if (distanceToPlayer > visionDistance)
        {
            _playerDetected = false;
            return;
        }

        // Step 2: Check if player is within the cone angle
        // Uses transform.forward (current enemy facing direction)
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > visionAngle / 2f)
        {
            _playerDetected = false;
            return;
        }

        // Step 3: Raycast to check if we actually hit the player (not blocked by obstacles)
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * visionHeight; // Use vision height offset
        Vector3 rayDirection = (player.position - rayStart).normalized;
        float rayDistance = Vector3.Distance(rayStart, player.position);

        if (Physics.Raycast(rayStart, rayDirection, out hit, rayDistance, obstacleLayerMask | playerLayerMask))
        {
            // Check if the raycast hit the player collider
            if (((1 << hit.collider.gameObject.layer) & playerLayerMask) != 0)
            {
                _playerDetected = true;
            }
            else
            {
                // Hit an obstacle instead of player
                _playerDetected = false;
            }
        }
        else
        {
            // Raycast didn't hit anything
            _playerDetected = false;
        }
    }

    void OnDrawGizmos()
    {
        if (!showVisionCone) return;

        // Change color based on detection
        Gizmos.color = _playerDetected ? detectionColor : visionConeColor;

        // Draw the vision cone based on current transform.forward direction
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward * visionDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward * visionDistance;

        // Adjust cone visualization height
        Vector3 coneStart = transform.position + Vector3.up * visionHeight;

        // Draw cone edges
        Gizmos.DrawLine(coneStart, coneStart + leftBoundary);
        Gizmos.DrawLine(coneStart, coneStart + rightBoundary);

        // Draw the arc at the end of the cone
        Vector3 previousPoint = coneStart + leftBoundary;
        for (int i = 1; i <= 15; i++)
        {
            float angle = Mathf.Lerp(-visionAngle / 2f, visionAngle / 2f, i / 15f);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            Vector3 point = coneStart + direction * visionDistance;
            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }

        // Draw detection ray when player is detected
        if (_playerDetected && player != null)
        {
            Gizmos.color = Color.green;
            Vector3 rayStart = transform.position + Vector3.up * visionHeight;
            Gizmos.DrawLine(rayStart, player.position);
            Gizmos.DrawWireSphere(player.position, 0.3f);
        }

        // Draw range circle for reference
        Gizmos.color = new Color(visionConeColor.r, visionConeColor.g, visionConeColor.b, 0.1f);
        Gizmos.DrawWireSphere(transform.position, visionDistance);
    }
}
