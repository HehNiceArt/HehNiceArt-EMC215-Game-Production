using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Patient_animManager : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    
    Patient patient; // to access >> isWaiting, hasReachedTable
    Animator anim;
    SpriteRenderer sprite;
    [SerializeField] private int LVL = 0; // <0> default value | values : < 0 - 5 >

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        patient = GetComponentInParent<Patient>();
    }


    void Update()
    {
        SetLevel(LVL);
        Vector2 direction = GetDirection();
        SetAnim(direction);
    }

    

    Vector2 GetDirection()
    {
        Vector3 velocity = agent.velocity;
        return new Vector2 (velocity.x, velocity.z).normalized;
    }

    void SetAnim(Vector2 direction)
    {
        if (direction.magnitude < 0.1f)
        {
            if (patient.isWaiting || patient.hasReachedTable)
            {
                SetState("sit");
            } else
            {
                SetState("idle");
            }
            
        } else
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

    void SetLevel(int level)
    {
        if (LVL == level) return;

        LVL = level;
        anim.SetInteger("Level", level);
    }

    void SetState(string state)
    {
        anim.SetBool("isWalking", state == "walk");
        anim.SetBool("isSitting", state == "sit");
        anim.SetBool("isIdle", state == "idle");
        //anim.SetBool("isWorking", state == "working");
        //Debug.Log("State set to: " + state);
    }
}
