using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private GameObject bar;
    float changeX;
    float tam;
    float paso;

    // Update is called once per frame
    void Start()
    {
        changeX = bar.transform.localScale.x;
    }

    private void Update()
    {
        for (float i = 0; i < 1; i += 0.01f)
        {
            WaitSec();
            bar.transform.localScale = new Vector3(bar.transform.localScale.x - i, bar.transform.localScale.y, bar.transform.localScale.z);
        }
    }

    IEnumerator WaitSec()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
