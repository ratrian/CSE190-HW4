using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadInHandModeBehaviour : MonoBehaviour
{
    public static bool controllerView;
    public static bool displayPyramids;

    // Start is called before the first frame update
    void Start()
    {
        controllerView = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            controllerView = true;
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            controllerView = false;
        }

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            displayPyramids = true;
        }
        else if (OVRInput.GetUp(OVRInput.Button.One))
        {
            displayPyramids = false;
        }
    }
}
