using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public GameObject point;
    GameObject[] points;
    List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    public int numberOfPoints;
    public float spaceBetweenPoints;

    public Vector2 shotPosition;
    private PlayerControls pc;
    private Rigidbody2D rb;
    private bool freezePoints;

    private void Start()
    {
        pc = gameObject.transform.parent.gameObject.GetComponent<PlayerControls>();
        rb = GetComponent<Rigidbody2D>();
        
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Instantiate(point, shotPosition, Quaternion.identity);
            renderers.Add(points[i].GetComponent<SpriteRenderer>());
        }
    }

    private void Update()
    {
        shotPosition = transform.position;

        if (!freezePoints)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].transform.position = PointPosition(i * spaceBetweenPoints);
            }
        }
    }

    private Vector2 PointPosition(float t)
    {
        Vector2 initVel = rb.velocity * (rb.mass / rb.gravityScale);
        Vector2 acc = Physics2D.gravity;
        Vector2 position = shotPosition + (initVel * t) + 0.5f * acc * (t * t);
        return position;
    }

    public void FreezePoints(bool check)
    {
        freezePoints = check;
    }

    public void ToggleTrail(bool check)
    {
        if (renderers.Count <= 0) return;

        for (int i = 0; i < renderers.Count; i++)
        {
            if (renderers[i].enabled != check) renderers[i].enabled = check;
        }
    }
}
