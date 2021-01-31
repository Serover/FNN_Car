using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastFixEdge : MonoBehaviour
{
    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;
    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = edgeCollider.pointCount;

        for (int i = 0; i < edgeCollider.pointCount; i++)
        {
            lineRenderer.SetPosition(i, edgeCollider.points[i]);

        }
    }
}
