using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {

    // Use this for initialization
    static GameObject S0;
    static GameObject S1;
    static GameObject S2;
    static GameObject S3;
    static GameObject S4;
    static GameObject S5;
    static GameObject S6;
    static GameObject[] s = { S0, S1, S2, S3, S4, S5, S6 };

    static GameObject S0s;
    static GameObject S1s;
    static GameObject S2s;
    static GameObject S3s;
    static GameObject S4s;
    static GameObject S5s;
    static GameObject S6s;
    static GameObject[] sih = { S0s, S1s, S2s, S3s, S4s, S5s, S6s };

    void Start () {
        initObject();
        loadPosition();
        GridManager.loadGrid(sih);
    }

    void Update()
    {

    }

    void initObject()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.tag.Equals("Shapes"))
            {
                int index = int.Parse(go.name);
                s[index] = go;
            }
            if (go.tag.Equals("ResultShapes"))
            {
                int index = int.Parse(go.name);
                sih[index] = go;
            }
        }
    }
    void loadPosition()
    {
        // location hardcoded {x,y,direction}
        double[] x0 = new[] { -0.06, -2.34, 45};
        double[] x1 = new[] { 0.71, -1.61, 45};
        double[] x2 = new[] { -0.79, -3.11, 135};
        double[] x3 = new[] { -0.819, -2.3, 270};
        double[] x4 = new[] { -1.19, -3.46, 45};
        double[] x5 = new[] { 1.4, -3.08, 315};
        double[] x6 = new[] { -0.069, -4.54, 225 };
        double[][] x = new[] {x0, x1, x2, x3, x4, x5, x6 };

        double[] x0s = new[] { -1.06, 3.03, 0 };
        double[] x1s = new[] { -0.55, 0.91, 315 };
        double[] x2s = new[] { 0.025, -0.235, 315};
        double[] x3s = new[] { 0.730, -0.245, 0.0};
        double[] x4s = new[] { -0.159, 4.01, 0 };
        double[] x5s = new[] { 0.43, 2.04, 0 };
        double[] x6s = new[] { 0.42, 0.04, 180 };
        double[][] xs = new[] { x0s,x1s, x2s, x3s, x4s, x5s, x6s};

        int i = 0;
        foreach (GameObject sss in s)
        {
            Vector3 position = new Vector3((float)x[i][0], (float)x[i][1], sss.transform.position.z);
            Quaternion rotation = Quaternion.Euler(sss.transform.rotation.eulerAngles.x, sss.transform.rotation.eulerAngles.y, (float)x[i][2]);
            sss.transform.SetPositionAndRotation(position, rotation);
            i++;
        }
        i = 0;
        foreach (GameObject sss in sih)
        {
            Vector3 position = new Vector3((float)xs[i][0], (float)xs[i][1], sss.transform.position.z);
            Quaternion rotation = Quaternion.Euler(sss.transform.rotation.eulerAngles.x, sss.transform.rotation.eulerAngles.y, (float)xs[i][2]);
            sss.transform.SetPositionAndRotation(position, rotation);
            i++;
        }
    }

}
