using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public GameObject myBoss;
    public SpriteRenderer sr;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    List<CharacterScript> chars = new List<CharacterScript>();
    List<int> idsAttacked = null;
    public float dmg = 0;
    public void BeginAttack(float dmg)
    {
        this.dmg = dmg;
        sr.enabled = true;
        idsAttacked = new List<int>();
    }
    public void EndAttack()
    {
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

    }

    // Update is called once per frame
    void Update()
    {
    }
}
