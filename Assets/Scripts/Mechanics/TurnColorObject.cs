using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ColorType
{
    White,
    Black
}

public class TurnColorObject : MonoBehaviour
{
    public List<GameObject> whiteTileList;
    public List<GameObject> blackTileList;

    private InvertEffect invertEffect;

    public Animator anim;

    [SerializeField] AudioClip clip;
   
    static ColorType colorType = ColorType.White;

    private void Start()
    {
        colorType = ColorType.White;

        invertEffect = FindObjectOfType<InvertEffect>();

        for (int i = 0; i < blackTileList.Count; i++)
        {
            blackTileList[i].SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<NewPlayer1>(out var _) || collision.TryGetComponent<NewPlayer2>(out var _) || collision.gameObject.layer == 8)
        {
            TurnColor();
        }
    }

    void TurnColor()
    {
        invertEffect.EffectOn();
        SoundManager.Instance.SFXPlay("TurnColor",clip);

        colorType = colorType == ColorType.White ? ColorType.Black : ColorType.White;

        if (colorType == ColorType.White)
        {
            for (int i = 0; i < whiteTileList.Count; i++)
            {
                whiteTileList[i].SetActive(true);
            }
            for (int i = 0; i < blackTileList.Count; i++)
            {
                blackTileList[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < whiteTileList.Count; i++)
            {
                whiteTileList[i].SetActive(false);
            }
            for (int i = 0; i < blackTileList.Count; i++)
            {
                blackTileList[i].SetActive(true);
            }
        }
    }
}
