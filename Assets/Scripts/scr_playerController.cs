using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class scr_playerController : MonoBehaviour
{

    Vector3 velocity;
    public Rigidbody body;
    public float mSpd;
    public float defaultSpd;
    public float dTime = 0.5f;
    public Vector3 rotationSetting;

    //public GameObject firePointU;
    //public GameObject firePointD;
    //public GameObject firePointR;
    //public GameObject firePointL;
    public GameObject activeFirePoint;
    public GameObject equippedSpell;
    //public GameObject spellA;
    //public GameObject spellB;
    public GameObject currentAttack;
    // Start is called before the first frame update
    void Start()
    {
        //activeFirePoint = firePointU;
        body = GetComponent<Rigidbody>();
        rotationSetting = new Vector3(0, 0, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector3.zero;
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.z = Input.GetAxisRaw("Vertical"); //We move on X and Z, not Y. Result of moving from 2 to 3 dimensions

        if (velocity.x > 0)
        {
            //activeFirePoint = firePointR;
            rotationSetting = new Vector3(0, 90f, 0); //In 3D we rotate on the Y, not Z
        }
        else if (velocity.x < 0)
        {
            //activeFirePoint = firePointL;
            rotationSetting = new Vector3(0, -90f, 0);
        }
        else if (velocity.z > 0 && velocity.x == 0)
        {
            //activeFirePoint = firePointU;
            rotationSetting = new Vector3(0, 0, 0);
        }
        else if (velocity.z < 0 && velocity.x == 0)
        {
            //activeFirePoint = firePointD;
            rotationSetting = new Vector3(0, 180f, 0f);
        }

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    if (equippedSpell == spellA) { equippedSpell = spellB; }
        //    else if (equippedSpell == spellB) { equippedSpell = spellA; }
        //}
        if (Input.GetKeyDown(KeyCode.J)) { Attack(equippedSpell); }

        if (currentAttack != null)
        {
            if (currentAttack.GetComponent<scr_spells>().PushSpell) { mSpd = 0f; }
            else if (currentAttack.GetComponent<scr_spells>().PullSpell) { mSpd = defaultSpd; }
        }
        else if (currentAttack == null) { mSpd = defaultSpd; }

        if (currentAttack != null)
        {
            if (currentAttack.GetComponent<scr_spells>().PullSpell)
            {
                if (Input.GetKeyUp(KeyCode.J))
                {
                    Destroy(currentAttack.gameObject);
                    currentAttack = null;
                }
            }
        }
    }

    void Attack(GameObject castSpell)
    {
        if (currentAttack == null)
        {
            var copy = Instantiate(castSpell, activeFirePoint.transform.position, Quaternion.identity);
            //copy.transform.eulerAngles = rotationSetting;
            currentAttack = copy;
            if (copy.GetComponent<scr_spells>().PushSpell)
            {
                Destroy(copy, dTime);
            }
            
        }
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + velocity * mSpd * Time.fixedDeltaTime);
        transform.eulerAngles = rotationSetting;
    }
}

