using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    //VV looks a bit weird
    public float offset = 0.35f;
    private PlayerControls pc;

    private void Awake()
    {
        pc = gameObject.transform.parent?.parent?.GetComponent<PlayerControls>();
    }

    private void Update()
    {
        transform.localPosition = pc.aim * offset;

        transform.localRotation = pc.GetAngle();
        //-1, 0 = -90
        //1, 0 = 90
        //0, 1 = 180
        //-1, 0 = 0;
    }
}
