<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility {
    private const float MAX_ANGLE_ADJUST = 5;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialAngle"></param>
    /// <param name="desiredAngle"></param>
    /// <param name="maximumAngleAdjustment"></param>
    /// <returns></returns>
    public static float DesireSmoothAngle(float initialAngle, float desiredAngle, float maximumAngleAdjustment) {
        desiredAngle = (desiredAngle + 360) % 360;
        initialAngle = (initialAngle + 360) % 360;
        float diff = desiredAngle - initialAngle;
        
        if (diff > 180) {
            diff -= 360;
        } else if (diff < -180) {
            diff += 360;
        }

        if (Mathf.Abs(diff) <= maximumAngleAdjustment) {
            return diff;
        } else if (diff > 0) {
            return maximumAngleAdjustment;
        } else {
            return -maximumAngleAdjustment;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialAngle"></param>
    /// <param name="desiredAngle"></param>
    /// <returns></returns>
    public static float DesireSmoothAngle(float initialAngle, float desiredAngle) {
        return DesireSmoothAngle(initialAngle, desiredAngle, MAX_ANGLE_ADJUST);
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility {
    private const float MAX_ANGLE_ADJUST = 5;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialAngle"></param>
    /// <param name="desiredAngle"></param>
    /// <param name="maximumAngleAdjustment"></param>
    /// <returns></returns>
    public static float DesireSmoothAngle(float initialAngle, float desiredAngle, float maximumAngleAdjustment) {
        desiredAngle = (desiredAngle + 360) % 360;
        initialAngle = (initialAngle + 360) % 360;
        float diff = desiredAngle - initialAngle;
        
        if (diff > 180) {
            diff -= 360;
        } else if (diff < -180) {
            diff += 360;
        }

        if (Mathf.Abs(diff) <= maximumAngleAdjustment) {
            return diff;
        } else if (diff > 0) {
            return maximumAngleAdjustment;
        } else {
            return -maximumAngleAdjustment;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialAngle"></param>
    /// <param name="desiredAngle"></param>
    /// <returns></returns>
    public static float DesireSmoothAngle(float initialAngle, float desiredAngle) {
        return DesireSmoothAngle(initialAngle, desiredAngle, MAX_ANGLE_ADJUST);
    }
}
>>>>>>> 1c28b964d2d8a375b9618e93c6e3ca78d613fcb8
