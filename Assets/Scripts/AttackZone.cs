﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public GameObject myBoss;
    public SpriteRenderer[] srs;
    private void Awake()
    {
        //  sr = GetComponent<SpriteRenderer>();
        foreach (var sr in srs)
        {
            sr.enabled = false;
        }
    }
    public AudioSource audi;
    public AudioClip clip;
    List<CharacterScript> chars = new List<CharacterScript>();
    List<int> idsAttacked = null;
    public float dmg = 0;
    public void BeginAttack(float dmg)
    {
        audi.Play();
        this.dmg = dmg;
        foreach (var sr in srs)
            sr.enabled = true;
        idsAttacked = new List<int>();
    }
    public void EndAttack()
    {
        foreach (var sr in srs)
            sr.enabled = false;
        foreach (var p in chars)
        {
            if (p == null)
                continue;
            if (!p.getHurt(dmg))
                idsAttacked.Add(p.id);
        }
        idsAttacked = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterScript charSet = collision.gameObject.GetComponent<CharacterScript>();
        if (charSet == null || charSet.gameObject.layer == myBoss.layer)
            return;
        chars.Add(charSet);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CharacterScript charSet = collision.gameObject.GetComponent<CharacterScript>();
        if (charSet == null || charSet.gameObject.layer == myBoss.layer)
            return;
        chars.Remove(charSet);
    }

    // Start is called before the first frame update
    void Start()
    {
        audi.clip = clip;

    }

    // Update is called once per frame
    void Update()
    {
    }
}
