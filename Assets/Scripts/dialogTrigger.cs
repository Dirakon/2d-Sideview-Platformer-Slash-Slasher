using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dialogTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public string[] dialog;
    public string choice1, choice2;
    Player pl = null;
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        pl = collision.gameObject.GetComponentInChildren<Player>();
        if (pl != null)
        {
            pl.DoChoice(dialog, choice1, choice2);
        }

    }
    public GameObject parent;
    IEnumerator ignoreInDueTime()
    {
        yield return new WaitForSeconds(1f);
        parent.layer = LayerMask.NameToLayer("Ignore Raycast");

    }
    void Update()
    {
        if (pl != null)
        {
            if (pl.decesion == 1)
            {
                //Abandon
                Debug.Log("Abandon");

                GoToScene.LoadAScene("UnkillScene");

                Destroy(gameObject);
            }
            else if (pl.decesion == 2)
            {
                //Save
                Debug.Log("Save");
                parent.GetComponent<BoxCollider2D>().isTrigger = true;
                Player.singleton.StartCoroutine(ignoreInDueTime());
                Player.singleton.startSize = Player.singleton.endSize; 
                Destroy(gameObject);
            }
        }
    }
}
