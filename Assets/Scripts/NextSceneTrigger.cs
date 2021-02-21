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
    public bool dialogBeforeThat = false;
    public string[] dialog;
    // Update is called once per frame
    void Update()
    {
        
    }
    public string nextScene;

    IEnumerator monitorStatusOfDialog()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (Player.singleton.dialogIsOver)
            {
                Player.singleton.dialogIsOver = false;
                CharacterScript.curentlyAlive = 0;
                GoToScene.timesOnThatScene = 0;
                GoToScene.sceneNow = nextScene;
                SceneManager.LoadScene(nextScene);
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CharacterScript.curentlyAlive < 2)
        {
            if (dialogBeforeThat)
            {
                Player.singleton.dialogIsOver = false;
                Player.singleton.DoDialog(dialog);
                Player.maxHp += 50;
                StartCoroutine(monitorStatusOfDialog());
            }
            else
            {
                CharacterScript.curentlyAlive = 0;
                GoToScene.timesOnThatScene = 0;
                GoToScene.sceneNow = nextScene;
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}
