using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class scr_playerController : MonoBehaviour
{

    Vector3 velocity;
    public Rigidbody body;
    public float mSpd;
    public float dashSpd;
    public float dashCooldown;
    public float defaultSpd;
    public float dTime = 0.5f;
    public float health;
    public bool dashing;
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
        if (Input.GetKeyDown(KeyCode.Space) && currentAttack == null) { Dash(); }

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
        if (Input.GetKeyDown(KeyCode.J) && !dashing) { Attack(equippedSpell); }

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
        if (health <= 0f) { Death(); }
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

    void Dash()
    {
        dashing = true;
        Debug.Log("Dash");
        //body.MovePosition(body.position + velocity * dashSpd * Time.deltaTime);
        //body.MovePosition(body.position);

        var direction = activeFirePoint.transform.position - body.transform.position;
        body.AddForce(direction.normalized * dashSpd, ForceMode.Impulse);
        Invoke(nameof(ResetDash), dashCooldown);
        //body.velocity = Vector3.zero;
        //body.angularVelocity = Vector3.zero;

        //body.MovePosition(activeFirePoint.transform.position );
    }

    private void ResetDash()
    {
        body.constraints = RigidbodyConstraints.FreezePosition;
        body.constraints = RigidbodyConstraints.None;
        body.constraints = RigidbodyConstraints.FreezeRotationX;
        body.constraints = RigidbodyConstraints.FreezeRotationZ;
        dashing = false;
    }

    void Death()
    {
        Debug.Log("YOU DIED");
        // Code for death, reset scene?
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + velocity * mSpd * Time.fixedDeltaTime);
        transform.eulerAngles = rotationSetting;

    }
}

