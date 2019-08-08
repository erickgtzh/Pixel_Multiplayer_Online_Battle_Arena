using UnityEngine;

public class CameraMov : MonoBehaviour
{
    public Transform playerTransform;
    public int depth = -20;

    //************************
    public float speedH = 1.0f;
    public float speedV = 1.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    //************************


    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + new Vector3(0, 10, depth);
        }

        //************************
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        //************************
    }

    public void setTarget(Transform target)
    {
        playerTransform = target;



    }
}