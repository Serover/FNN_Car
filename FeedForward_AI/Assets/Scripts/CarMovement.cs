using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    Rigidbody2D rigidbody;

    [Header("Settings")]
    public float speed;

    [Header("Data Stuff")]
    public Vector2 mouseDir;


    [Header("Random Stuff")]
    [Range(0, 1)] public float gas;          // gas 0 = no gas, 1 = full gas.
    [Range(0, 1)] public float turn;        // turn 0 = left, 0.5 = forward 1 = right
    public float maxTurnAngle = 180;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        FixRotation();
    }

    public float MouseToTurn()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseDir = -((Vector2)transform.position - mousePos).normalized;

        float mouseAngle = VectorToDegree(mouseDir);
        float startAngleRemoval = VectorToDegree(transform.right);

        // dont ask why, but rather ask why not ?
        float deltaAngle =  mouseAngle - startAngleRemoval;

        deltaAngle = FromDoubleToSingleDegree(deltaAngle);
        deltaAngle = FromSingleToDouble(deltaAngle);


        if (deltaAngle > maxTurnAngle / 2)
        {
            deltaAngle = maxTurnAngle / 2;
        }
        else if (deltaAngle < -maxTurnAngle / 2)
        {
            deltaAngle = -maxTurnAngle / 2;
        }
        
        float angleToValue = deltaAngle + maxTurnAngle / 2;
        angleToValue = angleToValue / maxTurnAngle;

        // Invert ofc
        angleToValue = 1 - angleToValue;

        return angleToValue;
    }


    public void DoCarMovement(float gas, float turn)
    {
        // direction based on turn

        Vector2 startVector = Quaternion.Euler(0, 0, maxTurnAngle / 2 * 1) * transform.right;
        Vector2 turnVector = Quaternion.Euler(0, 0, maxTurnAngle * -turn) * startVector;

        Debug.DrawLine(transform.position, (Vector2)transform.position + turnVector);

     //   this.gas = gas;
     //   this.turn = turn;

        rigidbody.velocity += (turnVector * speed * gas * Time.deltaTime);
    }
    private void FixRotation()
    {
        if (rigidbody.velocity.magnitude > 0)
        {


            float x = rigidbody.velocity.x;
            float y = rigidbody.velocity.y;

            float angle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
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

        if(degree < 0)
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
        for (int i = -1; i < 2; i += 2)
        {
            // do circle add force thingie
            Vector2 center;
            UnityEditor.Handles.color = Color.magenta;
            center = transform.position;



            Vector2 vector = Quaternion.Euler(0, 0, maxTurnAngle / 2 * i) * transform.right * 2f;

            UnityEditor.Handles.DrawLine(center, center + (vector));
        }
    }
#endif
}
