using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadInHandModeBehaviour : MonoBehaviour
{
    public static bool controllerView;

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
    }
}
