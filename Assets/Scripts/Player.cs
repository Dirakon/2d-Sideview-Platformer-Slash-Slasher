﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audi;
    public AudioClip in1, out1;
    static public float maxHp = 100f;
    void Awake()
    {
        singleton = this;
        textCanvas.planeDistance = -100;
        b1.enabled = false;
        b1.onClick.AddListener(choice1);
        b1.transform.localPosition = new Vector3(b1.transform.localPosition.x, b1.transform.localPosition.y, -9999);
        b2.onClick.AddListener(choice2);
        b2.enabled = false;
        b2.transform.localPosition = new Vector3(b2.transform.localPosition.x, b2.transform.localPosition.y, -9999);
        cam = GetComponent<Camera>();
        if (doMonologue && GoToScene.timesOnThatScene == 0)
        {
            //   screenFader.fadeState = whatToDo;
            StartCoroutine(closeOutCam(startSize, endSize, increaseSpeed));
        }
        else
        {
            doMonologue = false;
        }
        character.restartOnDeath = true;
    }
    public void Start()
    {
        character.maxHp = maxHp;
        character.hp = maxHp;
    }

    // public ScreenFader.FadeState whatToDo;
    public Button b1, b2;
    public Text diaMessage;
    public bool dialogIsOver = false;
    public void DoDialog(string[] txt)
    {
        StartCoroutine(dialogCoroutine(txt));
    }

    IEnumerator closeOutCam(float start, float end, float speed)
    {
        if (out1 != null && GoToScene.timesOnThatScene == 0)
        {
            audi.clip = out1;
            audi.Play();
            out1 = null;
        }
        controllable++;
        cam.orthographicSize = start;

        while (true)
        {
            cam.orthographicSize += speed * Time.deltaTime;
            if (cam.orthographicSize >= end)
                break;
            yield return new WaitForFixedUpdate();
        }



        controllable--;
    }
    IEnumerator closeInCam(float start, float end, float speed)
    {
        if (in1 != null && GoToScene.timesOnThatScene == 0)
        {
            audi.clip = in1;
            audi.Play();
            in1 = null;
        }
        controllable++;

        cam.orthographicSize = start;
        while (true)
        {
            cam.orthographicSize -= speed * Time.deltaTime;
            if (cam.orthographicSize < end)
                break;
            yield return new WaitForFixedUpdate();
        }



        controllable--;
    }
    public Canvas textCanvas;
    float controllable = 0;
    public int decesion = -1;
    public float dialogClosingInSpeedUnspeeded = 12f, dialogClosingOutSpeedUnspeeded = 12f;
    public float dialogClosingInSpeedSpeeded = 12f, dialogClosingOutSpeedSpeeded = 12f;
    IEnumerator dialogCoroutine(string[] txt)
    {
        controllable++;
        if (GoToScene.timesOnThatScene == 0)
            yield return closeInCam(endSize, startSize, dialogClosingInSpeedUnspeeded);
        else
            yield return closeInCam(endSize, startSize, dialogClosingInSpeedSpeeded);
        sr.enabled = true;
        textCanvas.enabled = true;
        textCanvas.planeDistance = 100;
        spaceClicked = false;
        foreach (string p in txt)
        {
            diaMessage.text = p;
            while (true)
            {
                if (spaceClicked)
                {
                    spaceClicked = false;
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForEndOfFrame();
        }
        textCanvas.planeDistance = -100;
        textCanvas.enabled = false;
        //Choice


        textCanvas.enabled = false;
        sr.enabled = false;
        if (GoToScene.timesOnThatScene == 0)
            yield return closeOutCam(endSize, startSize, dialogClosingOutSpeedUnspeeded);
        else
            yield return closeOutCam(endSize, startSize, dialogClosingOutSpeedSpeeded);


        dialogIsOver = true;
        controllable--;
    }

    void choice1()
    {
        decesion = 1;
    }
    void choice2()
    {
        decesion = 2;
    }
    // public ScreenFader screenFader;

    public float choiceClosingInSpeedUnspeeded = 2f, choiceClosingOutSpeedUnspeeded = 24f;
    public float choiceClosingInSpeedSpeeded = 24f, choiceClosingOutSpeedSpeeded = 24f;
    IEnumerator choiceCoroutine(string[] txt, string ch1, string ch2)
    {
        controllable++;
        if (GoToScene.timesOnThatScene == 0)
            yield return closeInCam(endSize, startSize, choiceClosingInSpeedUnspeeded);
        else
            yield return closeInCam(endSize, startSize, choiceClosingInSpeedSpeeded);
        sr.enabled = true;
        textCanvas.enabled = true;
        textCanvas.planeDistance = 100;
        foreach (string p in txt)
        {
            diaMessage.text = p;
            while (true)
            {
                Debug.Log(Input.GetKeyDown(KeyCode.Space));
                if (Input.GetKeyDown(KeyCode.Space) || p == txt[txt.Length - 1])
                    break;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForEndOfFrame();
        }

        b1.transform.localPosition = new Vector3(b1.transform.localPosition.x, b1.transform.localPosition.y, -0);

        b2.transform.localPosition = new Vector3(b2.transform.localPosition.x, b2.transform.localPosition.y, -0);
        b1.enabled = true;
        b1.GetComponentInChildren<Text>().text = ch1;
        b2.enabled = true;
        b2.GetComponentInChildren<Text>().text = ch2;
        while (decesion == -1)
        {
            yield return new WaitForFixedUpdate();
        }
        Debug.Log(decesion);

        textCanvas.planeDistance = -100;
        b1.enabled = false;
        b2.enabled = false;

        b1.transform.localPosition = new Vector3(b1.transform.localPosition.x, b1.transform.localPosition.y, -9999);

        b2.transform.localPosition = new Vector3(b2.transform.localPosition.x, b2.transform.localPosition.y, -9999);
        //Choice
        sr.enabled = false;
        textCanvas.planeDistance = -100;
        textCanvas.enabled = false;
        if (GoToScene.timesOnThatScene == 0)
            yield return closeOutCam(startSize, endSize, choiceClosingOutSpeedUnspeeded);
        if (GoToScene.timesOnThatScene == 0)
            yield return closeOutCam(startSize, endSize, choiceClosingOutSpeedSpeeded);



        controllable--;
    }
    public SpriteRenderer sr;
    public void DoChoice(string[] txt, string ch1, string ch2)
    {
        StartCoroutine(choiceCoroutine(txt, ch1, ch2));
        decesion = -1;
    }

    public static Player singleton;
    public bool doMonologue = true;
    public float startSize = 1f;
    public float endSize = 27.59f;
    public float increaseSpeed = 1f;
    public CharacterScript character;
    Camera cam;
    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && controllable == 0)
        {
            leftMouse = true;
        }
        if (Input.GetMouseButtonDown(1) && controllable ==0)
        {
            rightMouse = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceClicked = true;
        }
    }
    public bool spaceClicked = false;
    public bool leftMouse = false, rightMouse = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controllable > 0)
            return;
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
        if (leftMouse)
        {
            leftMouse = false;
            Debug.Log("BUTTON");
            character.Attack();
        }
        if (rightMouse)
        {
            rightMouse = false;
            Debug.Log("BUTTON");
            character.RangedAttack(mouseCoords);
        }
        character.RotateTowards(mouseCoords);
    }
}
