using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ender : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource audi;
    void Start()
    {
        audi = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogError(audi.isPlaying);
        if (!audi.isPlaying)
        {

            Application.Quit();
        }
    }
}
