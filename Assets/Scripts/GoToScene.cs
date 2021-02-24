using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GoToScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        gameObject.GetComponent<Button>().onClick.AddListener(clicked);
    }
    public static void LoadAScene(string name)
    {
        CharacterScript.curentlyAlive = 0;
        timesOnThatScene = 0;
        sceneNow = name;
        SceneManager.LoadScene(name);
    }
    public static void ReloadAScene()
    {
        CharacterScript.curentlyAlive = 0;
        timesOnThatScene++;
        SceneManager.LoadScene(GoToScene.sceneNow);
    }
    public static string sceneNow = "SampleScene";
    public static int timesOnThatScene = 0;
    void clicked()
    {
        LoadAScene("SampleScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
