using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastCameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tarObj;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(tarObj.transform.position.x, tarObj.transform.position.y, transform.position.z);
    }
}
