using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform target;

    Camera cam;
    public Transform camTransform;
    float horz = 10.0f; //Horizontal speed
    float vert = 10.0f; //Vertical speed
    float h = 0.0f;
    float v = 0.0f;
    float distance = 3.25f;

    void Start()
    {
        Camera cam = Camera.main;
        Transform camTransform = cam.transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            h += Input.GetAxis("Mouse X") * horz;
            v += -Input.GetAxis("Mouse Y") * vert;
            v = Mathf.Clamp(v, -3.0f, 70.0f);
        }
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Vector3 pos = new Vector3(0, 0.75f, 0);
        Quaternion rotation = Quaternion.Euler(v, h, 0);
        camTransform.position = pos + target.position + rotation * dir;
        camTransform.LookAt(target.position + pos);
    }//
}
