using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_spells : MonoBehaviour
{
    //This script is intended to work on an object that comes into existence briefly and then goes away
    public float pushForce, pushRadius;
    private GameObject pulled;
    private Vector3 pullVelocity = Vector3.zero;

    public GameObject player, wand;

    //The idea is that we have a bool for each spell. We write them as functions and then use the bools to turn them on and off. Hitboxes are handled by the object we attach this script to
    public bool PushSpell, PullSpell;
    // Start is called before the first frame update
    void Start()
    {
        // Finds the first game object with the 'player' tag. Fairly certain this only works so long as there is only 1 object with said tag. But we're single player anyways
        player = GameObject.FindGameObjectWithTag("Player");
        wand = GameObject.FindGameObjectWithTag("Wand");
    }

    // Update is called once per frame. May or may not be needed
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (PullSpell && pulled != null) 
        {
            //pulled.transform.position = Vector3.SmoothDamp(pulled.transform.position, transform.position, ref pullVelocity, 0.3f);
            pulled.transform.position = Vector3.MoveTowards(pulled.transform.position, wand.transform.position, 0.1f);
        }
    }

    //We look for targets as soon as we touch something
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
        if (other != null && other.gameObject.CompareTag("Target")) 
        {
            if (PushSpell) { Push(other.gameObject); }
            else if (PullSpell) { Pull(other.gameObject); }
        }
    }

    void Push(GameObject pushedObject)
    {
        Debug.Log(pushedObject);
        Rigidbody pushedBody = pushedObject.GetComponent<Rigidbody>();

        // Get direction from your postion toward the object you wish to push
        var direction = pushedBody.transform.position - player.transform.position;
        Debug.Log(direction);

        //Normalize keeps the value of a vector, but reduces it to 1. We use this to determine the direction of the pushed object relative to the player
        pushedBody.AddForce(direction.normalized * pushForce, ForceMode.Force);
    }

    void Pull(GameObject pulledObject)
    {
        if (pulled == null)
        {
            pulled = pulledObject;
        }
    }
}
