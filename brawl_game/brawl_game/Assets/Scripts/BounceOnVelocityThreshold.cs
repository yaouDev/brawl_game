using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOnVelocityThreshold : MonoBehaviour
{
    public float threshold = 50f;
    [SerializeField] private PhysicsMaterial2D bounceMaterial;
    private Rigidbody2D rb;
    private PhysicsMaterial2D originalMaterial;

    //take rb mass into account?

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(rb == null)
        {
            Debug.LogWarning("No Rigidbody on " + gameObject.name);
        }

        originalMaterial = rb.sharedMaterial;
    }

    private void Update()
    {
        if(rb.velocity.magnitude > threshold)
        {
            rb.sharedMaterial = bounceMaterial;
        }
        else
        {
            rb.sharedMaterial = originalMaterial;
        }
    }
}
