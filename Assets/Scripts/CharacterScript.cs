using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        curentlyAlive++;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        hp = maxHp;
        attackEffect.myBoss = gameObject;
        attackEffect.clip = meleeAttackSound;
        prevLayer = LayerMask.LayerToName(gameObject.layer);
    }
    public bool restartOnDeath = false; //maaaaan...
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


        coords.x -= thingRotator.transform.position.x;
        coords.y -= thingRotator.transform.position.y;

        float angle = Mathf.Atan2(coords.y, coords.x) * Mathf.Rad2Deg;
        if (angle < -135 || angle > 142)
        {
            //left
            foreach (var spriteR in selectedItems)
            {
                float rem = spriteR.transform.localEulerAngles.z;
                if (rem > 180)
                    rem -= 360;
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
            }
        }
        if (angle > -90 && angle < 90)
        {
            thingRotator.transform.localScale = new Vector3(1, 1, 1);
            realBody.transform.localScale = new Vector3(Mathf.Abs(realBody.transform.localScale.x), realBody.transform.localScale.y, realBody.transform.localScale.z);
        }
        else
        {
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
            realBody.transform.localScale = new Vector3(-Mathf.Abs(realBody.transform.localScale.x), realBody.transform.localScale.y, realBody.transform.localScale.z);
        }
        if (angle > 42)
            angle = 42;
        if (angle < -42)
            angle = -42;
        thingRotator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    IEnumerator hideAndRehideInTime(float whatTime,float kd, SpriteRenderer what)
    {
        if (!attackGoing)
        {
            attackEffect.BeginAttack(meleeDamage);
            attackGoing = true;
            yield return new WaitForSeconds(whatTime);
            attackEffect.EndAttack();
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

            yield return new WaitForSeconds(duration);

            dashIsOn = false;
            rb.gravityScale = prev;

            yield return new WaitForSeconds(kd);

            dashIsAvaliable = true;
        }
    }
    public GameObject realBody;
    public void DashOn()
    {
        StartCoroutine(dash(dashDuration,dashKD));
    }
    public GameObject hpBar;
    public static int ider = -1;
    public int id = ider++;
    public float maxHp = 100;
    public float hp;
    public float meleeDamage = 50f;
    public float rangedDamage = 50f;
    public bool getHurt(float dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            return true;
        }
        hpBar.transform.localScale = new Vector3(hp/maxHp,1, 1);
        return false;
    }
    IEnumerator rangedAttack(float kd,Vector2 pos)
    {
        if (!rangedAttackGoing)
        {
            rangedAttackGoing = true;
            var it = Instantiate(bulletPrefab, selectedItems[0].transform.position, Quaternion.identity).GetComponent<Bullet>();
            it.runTime = bulletLifetime;
            it.myLayer = gameObject.layer;
            it.myDmg = rangedDamage;
            it.audi.clip = rangedAttackSound;
            it.audi.Play();
            Vector2 mov = pos;
            mov.x -= transform.position.x;
            mov.y -= transform.position.y;
            it.rb.velocity = mov.normalized * bulletStrenght;

            yield return new WaitForSeconds(kd);

            rangedAttackGoing = false;
        }
    }
    public AttackZone attackEffect;
    bool attackGoing = false;
    bool rangedAttackGoing = false;
    public GameObject bulletPrefab;
    public float meleeKD=1f;
    public void Attack()
    {
        StartCoroutine(hideAndRehideInTime(meleeAttackDuration, meleeKD, selectedItems[0]));
    }
    public void RangedAttack(Vector2 pos)
    {
        StartCoroutine(rangedAttack(rangedAttackDuration,pos));
    }
    public float meleeAttackDuration = 1f;
    public float rangedAttackDuration = 2f;


    public void Jump(float jumpStrenghter = 1f)
    {
        if (!jumpReloaded || jumpReload > 0)
            return;
        float strenght = Time.deltaTime * jumpStrenghter * jumpStrenght / normals.Length;
        if (rb.velocity.y < strenght)
            rb.velocity = new Vector2(rb.velocity.x, strenght);
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

        }

        float change = Time.deltaTime * whereHowMuch * moveSpeed;
        if (change > 0 && right.counter > 0)
            change = 0;
        else if (change < 0 && left.counter > 0)
            change = 0;
        rb.velocity = new Vector2(change, rb.velocity.y);

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

    public static int curentlyAlive = 0;
    public AudioClip meleeAttackSound, rangedAttackSound;

    void Update()
    {
        if (hp <= 0)
        {
            if (restartOnDeath)
            {
                GoToScene.ReloadAScene();
            }
            else
            {
                curentlyAlive--;
                if (GoToScene.sceneNow == "BossScene")
                {
                    GoToScene.LoadAScene("Win");
                }
                Destroy(gameObject);
            }
        }
        if (jumpReload > 0)
            jumpReload -= Time.deltaTime;
    }
}
