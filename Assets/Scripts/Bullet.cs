using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update

    public int myLayer;
    public float myDmg; 
    void Start()
    {
        
    }
    public AudioSource audi;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterScript>();
        if (character != null && myLayer != character.gameObject.layer)
        {
            character.getHurt(myDmg);
            runTime = 0;
        }
    }
    public float runTime;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public Rigidbody2D rb;
    // Update is called once per frame
    void Update()
    {
        if (runTime > 0)
        {
            runTime -= Time.deltaTime;
        }else
        {
            Destroy(gameObject);
        }
    }
}
