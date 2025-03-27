using UnityEngine;

public class temp_animManager : MonoBehaviour
{
    private Animator anim;
    private bool isLvl1 = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isLvl1 = !isLvl1; 
            anim.SetBool("isLvl1", isLvl1);
        }
    }
}
