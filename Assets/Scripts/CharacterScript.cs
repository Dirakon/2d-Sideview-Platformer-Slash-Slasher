using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        prevLayer = LayerMask.LayerToName(gameObject.layer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Unclimable>() != null)
            return;
        jumpReloaded = true;
    }
    public GameObject thingRotator;
    public SpriteRenderer[] selectedItems;
    Rigidbody2D rb;
    public bool jumpReloaded = true;
    public float JumpReloadTime;
    float jumpReload = 0;
    Vector2[] normals = new Vector2[] { Vector2.up };



    public void RotateTowards(Vector2 coords)
    {

        // thingRotator.transform.LookAt(new Vector3(coords.x, coords.y, thingRotator.transform.position.z));
        coords.x = coords.x - thingRotator.transform.position.x;
        coords.y = coords.y - thingRotator.transform.position.y;

        float angle = Mathf.Atan2(coords.y, coords.x) * Mathf.Rad2Deg;
        if (angle < -135 || angle > 142)
        {
            //left
            foreach (var spriteR in selectedItems)
            {
                float rem = spriteR.transform.localEulerAngles.z;
                if (rem > 180)
                    rem -= 360;
           //     Debug.Log(spriteR.transform.localEulerAngles.z);
               // spriteR.flipX = true;
            //    spriteR.transform.localEulerAngles = new Vector3(spriteR.transform.localEulerAngles.x, spriteR.transform.localEulerAngles.y, -Mathf.Abs(rem));
            }
        }
        else if (angle < 42 && angle > -35)
        {
            //right
            foreach (var spriteR in selectedItems)
            {
                float rem = spriteR.transform.localEulerAngles.z;
                if (rem > 180)
                    rem -= 360;
              //  spriteR.flipX = false;
             //   spriteR.transform.localEulerAngles = new Vector3(spriteR.transform.localEulerAngles.x, spriteR.transform.localEulerAngles.y, Mathf.Abs(rem));
            }
        }
        if (angle > -90 && angle < 90)
        {
            thingRotator.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
           // Debug.Log(angle);
            if (angle < -90)
            {
                angle = -180 - angle;
                angle = -angle;
            }
            if (angle > 90)
            {
                angle = 180 - angle;
                angle = -angle;
            }
            thingRotator.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (angle > 42)
            angle = 42;
        if (angle < -42)
            angle = -42;
        thingRotator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        // thingRotator.transform.LookAt(coords, Vector3.right);
        // thingRotator.transform.eulerAngles = new Vector3(0, 0, thingRotator.transform.eulerAngles.z);
        // Debug.Log(coords);
    }

    IEnumerator hideAndRehideInTime(float whatTime,float kd, SpriteRenderer what)
    {
        if (!attackGoing)
        {
            attackGoing = true;
            what.color = Color.red;
            yield return new WaitForSeconds(whatTime);
            what.color = Color.white;
            yield return new WaitForSeconds(kd);
            attackGoing = false;
        }
    }
    public float bulletStrenght = 10f;
    public float bulletLifetime = 2f;
    public float dashMultiplier = 0.5f;
     bool dashIsOn = false;
    bool dashIsAvaliable = true;
    public float dashDuration=0.25f, dashKD=5f;
    IEnumerator dash(float duration,float kd)
    {
        if (dashIsAvaliable)
        {
            float prev = rb.gravityScale;
            dashIsAvaliable = false;
            rb.gravityScale = 0;
            dashSide = -1029;
            dashIsOn = true;
            //  what.color = Color.white;
            yield return new WaitForSeconds(duration);
            dashIsOn = false;
            rb.gravityScale = prev;
            yield return new WaitForSeconds(kd);
            dashIsAvaliable = true;
        }
    }
    public void DashOn()
    {
        StartCoroutine(dash(dashDuration,dashKD));
    }
    IEnumerator coolAttack(float kd,Vector2 pos)
    {
        if (!coolAttackGoing)
        {
            coolAttackGoing = true;
            var it = Instantiate(bulletPrefab, selectedItems[0].transform.position, Quaternion.identity).GetComponent<Bullet>();
            it.runTime = bulletLifetime;
            Vector2 mov = pos;
            mov.x = mov.x - thingRotator.transform.position.x;
            mov.y = mov.y - thingRotator.transform.position.y;
            it.rb.velocity = mov.normalized * bulletStrenght;
            //  what.color = Color.white;
            yield return new WaitForSeconds(kd);
            coolAttackGoing = false;
        }
    }
    bool attackGoing = false;
    bool coolAttackGoing = false;
    public GameObject bulletPrefab;
    public float meleeKD=1f;
    public void Attack()
    {
        StartCoroutine(hideAndRehideInTime(meleeKD,duration, selectedItems[0]));
    }
    public void CoolAttack(Vector2 pos)
    {
        StartCoroutine(coolAttack(duration,pos));
    }
    public float duration = 1f;
    public float coolDuration = 2f;


    public void Jump(float jumpStrenghter = 1f)
    {
        if (!jumpReloaded || jumpReload > 0)
            return;
        // rb.velocity += new Vector2(0, Time.deltaTime * jumpStrenghter * jumpStrenght);
        float strenght = Time.deltaTime * jumpStrenghter * jumpStrenght / normals.Length;
        //  for 
        // rb.AddForce(new Vector2(0, ), ForceMode2D.Impulse);
        if (rb.velocity.y < strenght)
            rb.velocity = new Vector2(rb.velocity.x, strenght);
        //rb.AddForce( ForceMode2D.Impulse);
        // rb.MovePosition(rb.position + new Vector2(0,Time.deltaTime * jumpStrenghter * moveSpeed));
        jumpReload = JumpReloadTime;
        jumpReloaded = false;
    }

    public float jumpStrenght, moveSpeed;
    public SideChecker left, right;
    string prevLayer;
    float dashSide=-1029;
    // void 
    public void Move(float whereHowMuch)
    {
        if (dashIsOn)
        {
            if (dashSide == -1029)
            {
                if (whereHowMuch == 0)
                    return;
                dashSide = whereHowMuch/Mathf.Abs(whereHowMuch);
            }
            whereHowMuch = dashSide * dashMultiplier;
            Debug.Log(whereHowMuch);
        }
        //  rb.AddForce(new Vector2(Time.deltaTime * whereHowMuch * moveSpeed,0), ForceMode2D.Impulse);
        //rb.AddForce(new Vector2(Time.deltaTime * whereHowMuch * moveSpeed, 0), ForceMode2D.Impulse);
        float change = Time.deltaTime * whereHowMuch * moveSpeed;
        if (change > 0 && right.counter > 0)
            change = 0;
        else if (change < 0 && left.counter > 0)
            change = 0;
        rb.velocity = new Vector2(change, rb.velocity.y);
        //  rb.MovePosition(rb.transform.position+new Vector3(Time.deltaTime * whereHowMuch * moveSpeed, 0));
    }

    public void Fall(bool fall)
    {
        if (fall)
        {
            gameObject.layer = LayerMask.NameToLayer("ThruPlatforms");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer(prevLayer);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpReload > 0)
            jumpReload -= Time.deltaTime;
    }
}
