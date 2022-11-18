using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public Transform objectTofollow;

    public float ForwardSpeed = 2.0f;
    public float BackwardSpeed = 1.0f;  
    public float StrafeSpeed = 4.0f;    
    public float RunMultiplier = 1.0f;

    public float followSpeed = 30f;
    public float sensitivity = 100f;
    public float clampAngle = 30f;

    private float rotX;
    private float rotY;

    public Transform realCamera;
    public Vector3 dirNormalized;
    public Vector3 finalDir;
    public float minDistance;
    public float maxDistance;
    public float finalDistance;
    public float smoothness = 10;

    private bool aiming;
    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;

        aiming = false;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;

        if(Input.GetMouseButtonDown(1)) // 걷기 중일때는 막아야 한다.
        {
            if (aiming) aiming = false;
            else aiming = true;
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed * Time.deltaTime);

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        RaycastHit hit;

        if(Physics.Linecast(transform.position, finalDir, out hit))
        {
            if (aiming)
            {
                finalDistance = -1.5f;
            }
            else
            {
                finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
        }
        else
        {
            if(aiming)
            {
                finalDistance = -1.5f;
            }
            else
            {
                finalDistance = maxDistance;
            }
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }
}
