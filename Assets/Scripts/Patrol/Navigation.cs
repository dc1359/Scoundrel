using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour {
    public const int MAX_PATH_RANGE = 7;
    public static Waypoints[] allWaypoints;
    public route currentRoute = new route();
    public Waypoints goal;
    
    public void FindPath(Vector3 target, route currentRoute) {
        route newRoute = new route();

    }

}

public class route {
    private Waypoints[] path;
    private float value;

    public void Add(Waypoints point) {
        Waypoints[] newPath = new Waypoints[path.Length + 1];

        if (path.Length > 0) {
            System.Array.Copy(path, newPath, path.Length);
            newPath[path.Length] = point;
            value += Vector3.Distance(path[path.Length - 1].transform.position, point.transform.position);
        } else {
            newPath[0] = point;
            value = 0f;
        }

        path = newPath;
    }

    public Waypoints GetPoint() {
        if (path.Length > 0) {
            return path[0];
        } 

        return null;
    }

    public bool NextPoint() {
        if (path.Length > 0) {
            Waypoints[] newPath = new Waypoints[path.Length - 1];
            System.Array.Copy(path, 1, newPath, 0, path.Length);
            path = newPath;

            if (path.Length >= 2) {
                value -= Vector3.Distance(path[0].transform.position, path[1].transform.position);
            }
            return true;
        }

        return false;
    }

    public float Value {
        get {
            return value;
        }
    }
}
