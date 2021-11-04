using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public float offset;

    private EdgeCollider2D col;
    private Camera cam;
    Vector2[] points = new Vector2[5];

    private void Awake()
    {
        col = GetComponent<EdgeCollider2D>();
        cam = Camera.main;

        SetCollider();
    }

    private void SetCollider()
    {
        float y = cam.orthographicSize * 2f * offset;
        float x = y * Screen.width / Screen.height;

        points[0] = new Vector2(-(x * 0.5f), y * 0.5f);
        points[1] = new Vector2(x * 0.5f, y * 0.5f);
        points[2] = new Vector2(x * 0.5f, -(y * 0.5f));
        points[3] = new Vector2(-(x * 0.5f), -(y * 0.5f));
        points[4] = points[0];

        col.points = points;
    }
}
