using UnityEngine;

public class NewPlayer1Anim : MonoBehaviour
{
    Animator anim;
    NewPlayer1 player;
    NewPlayer1Grab grab;

    bool playanim;

    private void Awake()
    {
        player = GetComponent<NewPlayer1>();
        anim = GetComponent<Animator>();
        grab = GetComponentInChildren<NewPlayer1Grab>();
    }

    private void Update()
    {
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);

        if (player.Push && !playanim)
        {
            anim.Play("PushGlove");
            playanim = true;
        }

        if (stateinfo.IsName("PushGlove") && stateinfo.normalizedTime > 0.8f)
        {
            player.Push = false;
            playanim = false;
        }
    }
}
