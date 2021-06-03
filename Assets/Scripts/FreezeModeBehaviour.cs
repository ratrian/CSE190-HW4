using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeModeBehaviour : MonoBehaviour
{
    public static bool freeze, fail;
    public static int randomChoice; 

    // Start is called before the first frame update
    void Start()
    {
        freeze = false;
        fail = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            freeze = !freeze;
        }

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            fail = !fail;
        }
        if (fail)
        {
            randomChoice = Random.Range(0, 6);
        }
    }
}
