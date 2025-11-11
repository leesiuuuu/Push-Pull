using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player1Anim : MonoBehaviour
{
    Animator anim;
    Player1 player1;
    Player1Grab grab;

    bool playanim;

    private void Awake()
    {
        player1  = GetComponent<Player1>();
        anim = GetComponent<Animator>();    
        grab = GetComponentInChildren<Player1Grab>();
    }

    private void Update()
    {
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);

        if (player1.Push && !playanim)
        {
            anim.Play("PushGlove");
            playanim = true;
        }

        if (stateinfo.IsName("PushGlove") && stateinfo.normalizedTime > 0.8f)
        {
            player1.Push = false;
            playanim = false;
        }
    }
}
