﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    public CharacterScript character;
    Camera cam;
    // Update is called once per frame
    void FixedUpdate()
    {
        character.Move(Input.GetAxis("Horizontal"));
        if (Input.GetKey(KeyCode.Space))
        {
            character.Jump();
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
            character.CoolAttack();
        }
        character.RotateTowards(cam.ScreenToWorldPoint(Input.mousePosition));
    }
}
