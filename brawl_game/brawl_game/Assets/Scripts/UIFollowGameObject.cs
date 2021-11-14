using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowGameObject : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    private float moveSpeed;
    [SerializeField] private float originalSpeed = 4f;
    private Camera cam;
    private Rigidbody2D targetRb;

    private void Start()
    {
        moveSpeed = originalSpeed;

        cam = Camera.main;
        transform.position = cam.WorldToScreenPoint(target.position + offset);

        if (target.gameObject.TryGetComponent(out Rigidbody2D rb)) targetRb = rb;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPos = cam.WorldToScreenPoint(target.position + offset);

        if (targetRb != null) moveSpeed = originalSpeed * (targetRb.velocity.magnitude * 0.1f);
        if (moveSpeed < originalSpeed) moveSpeed = originalSpeed;

        if (transform.position != desiredPos) transform.position = Vector3.MoveTowards(transform.position, desiredPos, moveSpeed);
    }
}
