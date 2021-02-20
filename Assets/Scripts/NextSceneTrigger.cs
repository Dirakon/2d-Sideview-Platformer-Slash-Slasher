using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string nextScene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CharacterScript.curentlyAlive < 2)
        {
            CharacterScript.curentlyAlive = 0;
            GoToScene.timesOnThatScene = 0;
            GoToScene.sceneNow = nextScene;
            SceneManager.LoadScene(nextScene);

        }
    }
}
