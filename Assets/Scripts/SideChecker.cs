using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public int counter = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlatformEffector2D>() == null)
            counter++;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlatformEffector2D>() == null)
            counter--;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
