using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CamManager : MonoBehaviour
{
    private static CamManager instance = null;
    public static CamManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CamManager>();
            }
            return instance;
        }
    }


    //The object the camera will follow
    public GameObject player;
    public Transform cameraPivot;

    //Camera control area for looking around
    public TouchFieldController touchpadField;


    //Smooth camera
    private Transform targetTransform;
    public float cameraFollowSpeed = 0.2f;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    //Camera looking up and down, left and right
    public float lookAngle = 0;
    public float pivotAngle = 0;

    //Camera rotation limit
    private float minimum = -85f;
    private float maximum = 85f;
    public float camSensitive = 0.5f;


    private void Awake()
    {
        targetTransform = player.transform;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void LateUpdate()
    {
        FollowTarget();
        RorateCamera();
    }
    //Camera follow player
    public void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }
    //Rotate Camera

    public void RorateCamera()
    {
        lookAngle = touchpadField.direction.x;
        pivotAngle = touchpadField.direction.y;
        pivotAngle = Mathf.Clamp(pivotAngle, minimum, maximum);

        //Look left and right
        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle * camSensitive;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        //Look up and down
        rotation = Vector3.zero;
        rotation.x = -pivotAngle * camSensitive;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }
}