using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Patient_animManager : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    Patient patient; // to access >> isWaiting, hasReachedTable
    //TableInteraction tableInteraction; // to access >> isOccupied
    Animator anim;
    SpriteRenderer sprite;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        patient = GetComponentInParent<Patient>();
        //tableInteraction = GetComponent<TableInteraction>();
        agent.angularSpeed = 720f;  // Increase rotation speed (default is 120)
        agent.acceleration = 4f;
    }


    void Update()
    {
        if (!patient.hasReachedTable)
        {
            Vector2 direction = GetDirection();
            SetAnim(direction);
        }
        else
        {
            SetState("sit");
            anim.SetBool("facingFront", false);
        }

        if (patient.hasBeenTreated)
        {
            SetState("walk");
            anim.SetBool("facingFront", true);
        }

        // TODO : Check if patient is done with treatment so it changes to healed state
        /*if (!tableInteraction.isOccupied)
        {
            SetState("healed");
        }*/
    }
    public void SetHealedState()
    {
        SetState("healed");
    }



    Vector2 GetDirection()
    {
        Vector3 velocity = agent.velocity;
        return new Vector2(velocity.x, velocity.z).normalized;
    }

    void SetAnim(Vector2 direction)
    {
        if (direction.magnitude < 0.1f)
        {
            if (patient.isWaiting && patient.hasReachedTable)
            {
                SetState("sit");
                anim.SetBool("facingFront", false);
            }
            else
            {
                SetState("idle");
            }

        }
        else
        {
            SetState("walk");
        }

        if (direction.y > 0)
        {
            anim.SetBool("facingFront", false);
            if (direction.x > 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }
        else
        {
            anim.SetBool("facingFront", true);
            if (direction.x > 0)
            {
                sprite.flipX = true;
            }
            else
            {

                sprite.flipX = false;
            }
        }
    }

    /*void SetLevel(int level)
    {
        if (LVL == level) return;

        LVL = level;
        anim.SetInteger("Level", level);
    }*/

    void SetState(string state)
    {
        anim.SetBool("isWalking", state == "walk");
        anim.SetBool("isSitting", state == "sit");
        anim.SetBool("isIdle", state == "idle");
        anim.SetBool("isHealed", state == "healed");
        //Debug.Log("State set to: " + state);
    }
}
