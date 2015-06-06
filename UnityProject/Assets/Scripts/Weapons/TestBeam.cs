using UnityEngine;
using System.Collections;

public class TestBeam : MonoBehaviour {
    public GameObject test;
    void Awake ()
    {
        test.gameObject.AddComponent<BoxCollider2D>();
        test.GetComponent<BoxCollider2D>().isTrigger = true;
        test.GetComponent<BoxCollider2D>().size = new Vector2(1,1);
    }
    // Update is called once per frame
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Here");
    }

    void Update () 
    {
    
    }
}
