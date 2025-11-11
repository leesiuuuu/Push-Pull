using UnityEngine;

public class NewPlayer2Anim : MonoBehaviour
{
    Animator anim;
    NewPlayer2 player;
    bool playanim;

    private void Awake()
    {
        player = GetComponent<NewPlayer2>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);

        if (player.Push && !playanim)
        {
            anim.Play("PushGlovePlayer2");
            playanim = true;
        }

        if (stateinfo.IsName("PushGlovePlayer2") && stateinfo.normalizedTime > 0.8f)
        {
            player.Push = false;
            playanim = false;
        }
    }
}
