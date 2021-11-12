using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowGameObject : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 desiredPos = Camera.main.WorldToScreenPoint(target.position + offset);

        transform.position = desiredPos;
    }
}
