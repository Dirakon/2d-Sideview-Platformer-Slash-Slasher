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
    public LayerMask whoToStopBefore;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (whoToStopBefore == (whoToStopBefore | (1 << collision.gameObject.layer)))
        {
            counter++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (whoToStopBefore == (whoToStopBefore | (1 << collision.gameObject.layer)))
        {
            counter--;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
