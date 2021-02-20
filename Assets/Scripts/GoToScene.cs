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
    public static string sceneNow = "SampleScene";
    public static int timesOnThatScene = 0;
    void clicked()
    {
        timesOnThatScene = 0;
        sceneNow = "SampleScene";
        SceneManager.LoadScene("SampleScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
