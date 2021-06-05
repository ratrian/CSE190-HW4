using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugModeBehaviour : MonoBehaviour
{
    public static bool displayPyramids;

    // Start is called before the first frame update
    void Start()
    {
        displayPyramids = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            displayPyramids = !displayPyramids;
        }
    }
}
