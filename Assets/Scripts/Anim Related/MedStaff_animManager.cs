using UnityEngine;
using Sirenix.OdinInspector;

public class MedStaff_animManager : MonoBehaviour
{
    Animator anim;
    // TableBehavior tableBehavior;
    TableInteraction tableInteraction;
    public int LVL = 1; // <1> default value | values : < 1 - 5 >

    void Start()
    {
        tableInteraction = GetComponentInParent<TableInteraction>();
        anim = GetComponent<Animator>();

        anim.SetBool("facingFront", true);
    }

    // TODO : Change anim >> SetLVL(LVL) based on the table upgrade

    private void Update()
    {
        if (tableInteraction.isOccupied)
        {
            SetState("working");
        }
        else
        {
            SetState("idle");
        }
    }


    /*void ToggleFront()
    {
        anim.SetBool("facingFront", true);
    }

    void ToggleBack()
    {
        anim.SetBool("facingFront", false);
    }*/

    public void SetLevel(int level)
    {
        if (LVL == level) return;

        LVL = level;
        anim.SetInteger("Level", level);
    }

    void SetState(string state)
    {
        anim.SetBool("isIdle", state == "idle");
        anim.SetBool("isWorking", state == "working");
    }

}
