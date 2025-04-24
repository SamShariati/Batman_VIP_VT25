using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BoneRenderer))]
public class BoneRendererEditor : Editor
{
    public void OnSceneGUI()
    {
        var br = target as BoneRenderer;
        if(!br.enabled) return;
        // draw the detectopm range
        Handles.color = br.color;
        DrawLines(br.transform);
        //Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.ProximityDetectionRange);

        //// draw the hearing range
        //Handles.color = ai.HearingRangeColour;
        //Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.HearingRange);

        //// work out the start point of the vision cone
        //Vector3 startPoint = Mathf.Cos(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.forward +
        //                     Mathf.Sin(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.right;

        //// draw the vision cone
        //Handles.color = ai.VisionConeColour;
        //Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.VisionConeAngle * 2f, ai.VisionConeRange);


        // 3d
        //Vector3 startPoint = Mathf.Cos(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.forward +
        //                      Mathf.Sin(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.right;


        //Handles.color = ai.VisionConeColour;


        //Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.VisionConeAngle * 2f, ai.VisionConeRange);
    }

    public void DrawLines(Transform from)
    {

        for (int i = 0; i < from.childCount; i++)
        {
            Transform to = from.GetChild(i);
            DrawLine(from, to);
            DrawLines(to);
        }
    }


    public void DrawLine(Transform from, Transform to)
    {
        Handles.DrawLine(from.position, to.position, 3);
    }
}
