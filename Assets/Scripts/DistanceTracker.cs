using UnityEngine;
using TMPro;
/// <summary>
/// Displays the meters between the human and bat so that they can easily find eachother.
/// </summary>
public class DistanceTracker : MonoBehaviour
{
    [SerializeField] public Transform Bat;
    [SerializeField] public Transform Human;
    public TextMeshProUGUI distanceTextHuman;
    public TextMeshProUGUI distanceTextBat;

    void Update()
    {
        if (Bat != null && Human != null && distanceTextHuman != null)
        {
            float distance = Vector3.Distance(Bat.position, Human.position);

            distanceTextHuman.text = $"Meters from Bat: {distance:F2}m";
            distanceTextBat.text = $"Meters from Human: {distance:F2}m";
        }
        else
        {
            Debug.LogWarning("Assign player and bat");
        }
    }
}
