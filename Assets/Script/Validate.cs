using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Validate : MonoBehaviour {

    //This class is check if the placing of tangram is correct


    public static bool Validation()
    {
        var dist = AppConstant.DISTANCE_FROM_CAMERA_TO_OBJECT;
        float xmin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        float xmax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        float ymin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        float ymax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        int failPixel = 0;
        int totalPixel = 0;
        float pixelInterval = 0.2f;
        for (float x = xmin; x < xmax; x = x + pixelInterval)
        {
            for (float y = ymin; y < ymax; y = y + pixelInterval)
            {
                RaycastHit2D touches = Physics2D.Linecast(new Vector3(x, y, -10f), new Vector3(x, y, 0f));
                var hit = touches;
                if (hit.transform != null)
                {
                    if (hit.transform.gameObject.tag == AppConstant.RESULT_OBJ_TAG)
                    {
                        failPixel++;
                    }
                }
                totalPixel++;
            }
        }
        if (failPixel < AppConstant.PASS_THRESHOLD)
        {
            return true;
        }
        return false;
    }
    
}
