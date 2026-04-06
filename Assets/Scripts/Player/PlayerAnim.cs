using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Animator anim;
    InputPlayer player;
    bool playanim;

    private void Awake()
    {
        player = GetComponent<InputPlayer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);

        if (player.Push && !playanim)
        {
            player.PlayAnim("PushGlove");
            playanim = true;
        }

        if (stateinfo.IsName("PushGlove") && stateinfo.normalizedTime > 0.8f)
        {
            player.Push = false;
            playanim = false;
        }
    }
}