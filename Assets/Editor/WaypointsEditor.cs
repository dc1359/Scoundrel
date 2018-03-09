using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Waypoints))]
public class WaypointsEditor : Editor {
    public void OnSceneGUI() {
        Waypoints wp = (Waypoints)target;
        Handles.color = Color.white;
        
        foreach (Waypoints point in wp.NearPoints) {
            if (!(point == null))
                Handles.DrawLine(wp.transform.position, point.transform.position);
        }
    }
}
