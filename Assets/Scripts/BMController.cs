using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool currentlyOnBoss = false;
    public AudioClip bossMusic;
    // Update is called once per frame
    void Update()
    {
        if (GoToScene.sceneNow == "Win")
            Destroy(gameObject);
        if (currentlyOnBoss)
            return;
        if (GoToScene.sceneNow == "BossScene")
        {
            currentlyOnBoss = true;
            GetComponent<AudioSource>().clip = bossMusic;
            GetComponent<AudioSource>().Play();
        }
    }
}
