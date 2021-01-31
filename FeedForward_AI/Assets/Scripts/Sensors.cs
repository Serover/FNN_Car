using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour
{
    [Header("Rays")]
    [SerializeField] LayerMask layerMask;
    [Range(0, 10)] public int nrOfRays;
    [Range(0, 45)] public float rayDelta;
    public float rayRange;

    public RaycastHit2D[] sensorRayHits;
    [SerializeField]public float[] sensorRayDist;
    void Start()
    {
        sensorRayHits = new RaycastHit2D[nrOfRays * 3];
        sensorRayDist = new float[nrOfRays * 3];


    }
    private void FixedUpdate()
    {
        
    }

    public float[] FetchDistances()
    {
        float[] tempDist = new float[nrOfRays * 3];
        sensorRayHits = UpdateSensors();
        for (int i = 0; i < tempDist.Length; i++)
        {
            tempDist[i] = Vector2.Distance(transform.position, sensorRayHits[i].point) > rayRange ? rayRange : Vector2.Distance(transform.position, sensorRayHits[i].point);
        }
        return tempDist;
    }

    // make array return ??
    public RaycastHit2D[] UpdateSensors()
    {
        RaycastHit2D[] rayHits = new RaycastHit2D[nrOfRays * 3];

        for (int i = 0; i < 3; i++)
        {
            Vector2 startVector = Quaternion.Euler(0, 0, -90f + (90f * i)) * transform.right;

            for (int j = 0; j < nrOfRays; j++)
            {
                Vector2 startDeltaVector = Quaternion.Euler(0, 0, -rayDelta * nrOfRays) * startVector;
                Vector2 turnVector = Quaternion.Euler(0, 0, rayDelta * j) * startDeltaVector;

                RaycastHit2D rayhit = Physics2D.Raycast(transform.position, turnVector.normalized, rayRange, layerMask);

                rayHits[(i * nrOfRays) + j] = rayhit;
            }
        }
        return rayHits;
    }

    private Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    private Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
    private float VectorToDegree(Vector2 vector)
    {
        float degree = Mathf.Atan2(vector.y, vector.x);
        degree = Mathf.Rad2Deg * degree;

        if (degree < 0)
        {
            degree = (180 - Mathf.Abs(degree)) + 180f;
        }
        return degree;
    }
    private float FromDoubleToSingleDegree(float value)
    {
        float degree = value;

        if (degree < 0)
        {
            degree = (180 - Mathf.Abs(degree)) + 180f;
        }

        return degree;
    }
    private float FromSingleToDouble(float value)
    {
        float degree = value;


        if (degree > 180)
        {
            degree = degree - 180;
            degree = -180 + degree;
        }

        return degree;
    }
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // do circle add force thingie
        Vector2 center;
        UnityEditor.Handles.color = Color.red;
        center = transform.position;

        for (int i = 0; i < 3; i++)
        {
            // calc start delta
            Vector2 startVector = Quaternion.Euler(0, 0, -90f + (90f * i)) * transform.right;

            for (int j = 0; j < nrOfRays; j++)
            {
                Vector2 startDeltaVector = Quaternion.Euler(0, 0, -rayDelta * nrOfRays * 0.5f) * startVector;


                Vector2 turnVector = Quaternion.Euler(0, 0, rayDelta * j) * startDeltaVector;

                UnityEditor.Handles.DrawLine(center, center + (turnVector * rayRange));
            }


        }



    }
#endif


}
