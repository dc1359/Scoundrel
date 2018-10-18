using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {
    public Waypoints[] NearPoints;

    public int Count {
        get {
            return NearPoints.Length;
        }
    }
    public Vector2 GetClosestVector(int index) {
        Vector3 target = NearPoints[index].transform.position;
        Vector2 point = new Vector2 {
            x = target.x,
            y = target.z
        };

        return point;
    }
}
