using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();
        if (doMonologue && GoToScene.timesOnThatScene == 0)
        {
            cam.orthographicSize = startSize;

        }
        character.restartOnDeath = true;
    }
    public bool doMonologue = true;
    public float startSize = 1f;
    public float endSize = 27.59f;
    public float increaseSpeed = 1f;
    public CharacterScript character;
    Camera cam;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (doMonologue)
        {
            cam.orthographicSize += increaseSpeed * Time.deltaTime;
            if (cam.orthographicSize >= endSize)
                doMonologue = false;
            return;
        }
        Vector2 mouseCoords = cam.ScreenToWorldPoint(Input.mousePosition);
        character.Move(Input.GetAxis("Horizontal"));
        if (Input.GetKey(KeyCode.Space))
        {
            character.Jump();
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            character.DashOn();
        }
        character.Fall(Input.GetKey(KeyCode.S));
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("BUTTON");
            character.Attack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("BUTTON");
            character.CoolAttack(mouseCoords);
        }
        character.RotateTowards(mouseCoords);
    }
}
