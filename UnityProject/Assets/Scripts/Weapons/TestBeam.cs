using UnityEngine;
using System.Collections;

public class TestBeam : MonoBehaviour {
    public GameObject test;
    private Object target;
    void Awake ()
    {
        target = Resources.Load("Targeting");
        
        //(Instantiate (m_Prefab, position, rotation) as GameObject).transform.parent = parentGameObject.transform;
        //test.gameObject.AddComponent<BoxCollider2D>();
        //test.GetComponent<BoxCollider2D>().isTrigger = true;
        //test.GetComponent<BoxCollider2D>().size = new Vector2(1,1);
    }

    void Start ()
    {
        var a = Instantiate(target, this.transform.position, test.transform.rotation) as GameObject;
        a.transform.parent = test.transform;
        a.GetComponent<SpriteRenderer>().color = Color.red;
        //this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
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
