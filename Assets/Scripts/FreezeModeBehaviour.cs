using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeModeBehaviour : MonoBehaviour
{
    public static bool freezed;

    // Start is called before the first frame update
    void Start()
    {
        freezed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            freezed = !freezed;
        }
    }
}
