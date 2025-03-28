using UnityEngine;

public class temp_animManager : MonoBehaviour
{
    private Animator anim;
    private int currLVL = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isIdle", true);
        anim.SetBool("facingFront", true);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {

            anim.SetBool("facingFront", false);
            Debug.Log("Facing Back.");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("facingFront", true);
            Debug.Log("Facing Front.");
        }

        if (Input.GetKeyDown(KeyCode.F)) SetState("walk");
        else if (Input.GetKeyDown(KeyCode.G)) SetState("sit");
        else if (Input.GetKeyDown(KeyCode.H)) SetState("idle");

        if (Input.GetKeyDown(KeyCode.Alpha0)) SetLevel(0);
        else if (Input.GetKeyDown(KeyCode.Alpha1)) SetLevel(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SetLevel(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SetLevel(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SetLevel(4);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) SetLevel(5);

    }

    void SetLevel(int level)
    {
        if (currLVL == level) return;

        currLVL = level;
        anim.SetInteger("Level", level);
    }

    void SetState(string state)
    {
        anim.SetBool("isWalking", state == "walk");
        Debug.Log(state == "walk");
        anim.SetBool("isSitting", state == "sit");
        Debug.Log(state == "sit");
        anim.SetBool("isIdle", state == "idle");
        Debug.Log(state == "idle");
        Debug.Log("State set to: " + state);
    }
}
