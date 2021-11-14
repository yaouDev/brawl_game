using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    //VV looks a bit weird
    public float offset = 0.35f;
    public float moveSpeed = 5f;
    public float rotationMultiplier = 3f;
    public AnimationCurve moveCurve;
    public float lerpTime = 0.5f;
    private float timer;
    private PlayerControls pc;


    private void Awake()
    {
        pc = gameObject.transform.parent?.parent?.GetComponent<PlayerControls>();
    }

    private void Update()
    {
        if(timer < lerpTime)
        {
            timer += Time.deltaTime;
        }

        if(timer > lerpTime)
        {
            timer = lerpTime;
        }

        Vector3 desiredPos = pc.aim * offset;

        float lerpRatio = moveCurve.Evaluate(timer / lerpTime);

        if (transform.localPosition != desiredPos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, desiredPos, (moveSpeed * lerpRatio) * 0.01f);
        }
        else
        {
            if(timer == lerpTime)
            {
                timer = 0f;
            }
        }

        if (transform.localRotation != pc.GetAngle())
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, pc.GetAngle(), (moveSpeed * rotationMultiplier) * lerpRatio);
        }


        //transform.localPosition = pc.aim * offset;

        //transform.localRotation = pc.GetAngle();
        //-1, 0 = -90
        //1, 0 = 90
        //0, 1 = 180
        //-1, 0 = 0;
    }
}
