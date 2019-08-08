using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float speedY = 0.088f;
    float speedX = 0.47f;
    int limitX = 0;
    float rot;
    void Update()
    {
        if (limitX<370)
        {
            transform.position = new Vector3(transform.position.x + speedX, transform.position.y+ speedY, transform.position.z);
            //rot += Time.deltaTime * 2;
            rot += .027f;
            transform.rotation = Quaternion.Euler(transform.rotation.x+rot, 90, transform.rotation.z);
            JustWait(.7f);
            limitX++;
        }

    }

    IEnumerator JustWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

}
