using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSound : MonoBehaviour
{
    public AudioSource MusicSource;

    // Start is called before the first frame update
    void Start()
    {
        MusicSource.Play();
    }

}
