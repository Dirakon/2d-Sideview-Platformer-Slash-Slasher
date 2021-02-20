using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class Enemy : MonoBehaviour
{

    public Transform target;

    public float nextWaypointDistance = 10f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEnd = false;
    Seeker seeker;
    float toGetRidOf;
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    // Start is called before the first frame update
    public float detectionDistance = 100f;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        var p = guy.GetComponent<BoxCollider2D>();
        toGetRidOf = p.size.y / 2;

        InvokeRepeating("UpdatePath", 0f, 1f);
    }

    // Update is called once per frame
    void UpdatePath()
    {
        haveFaceContact = ! ((target.transform.position - transform.position).magnitude > detectionDistance) && Physics2D.Raycast(transform.position, target.transform.position - transform.position, (target.transform.position - transform.position).magnitude, colliderRaycastMask).collider == null;
        if (haveFaceContact)
            seeker.StartPath(transform.position, target.position, OnPathComplete);
    }
    public bool haveFaceContact = false;
    public LayerMask colliderRaycastMask;
    public bool isMelee;
    public bool isRanged;
    public float desiredDistance=10f;
    private void FixedUpdate()
    {
        if (path == null)
            return;
        guy.RotateTowards(target.position);
        if (haveFaceContact)
        {
            if (isRanged)
                guy.CoolAttack(target.position);
            if (Vector2.Distance(transform.position,target.position) < desiredDistance)
            {
                return;
            }
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEnd = true;
            if (haveFaceContact)
            {
                if (isMelee)
                    guy.Attack();
            }
            return;
        }
        else
        {
            reachedEnd = false;
        }
        do
        {
            float dist = Vector2.Distance(transform.position, new Vector2(path.vectorPath[currentWaypoint].x, (path.vectorPath[currentWaypoint].y + transform.position.y) / 2));
            if (dist <= nextWaypointDistance)
            {
                currentWaypoint++;
            }
            else
            {
                break;
            }
        } while (true);
        Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position);

        if (Mathf.Abs(dir.y) - toGetRidOf > 0)
        {
            //  Debug.Log("RIIIIIID");
            if (dir.y < 0)
            {
                dir.y = 0;
                guy.Fall(true);
            }
            else
            {
                guy.Fall(false);
                guy.Jump();
            }
        }
        else
        {

            guy.Fall(false);
        }
        dir = dir.normalized;

        guy.Move(dir.x);

    }
    public CharacterScript guy;
    void Update()
    {

    }
}
