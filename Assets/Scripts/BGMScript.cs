using UnityEngine;

public class BGMScript : MonoBehaviour
{
	[SerializeField]
	private AudioSource bgm;
	private void Start()
	{
		FadeIn();
	}
	public void FadeOut()
	{
		SoundManager.Instance.BgFadeIn(bgm);
	}
	public void FadeOutCustom(float volume, float time)
	{
		SoundManager.Instance.BgFadeInCustom(bgm, volume, time);
	}
	public void FadeIn()
	{
		SoundManager.Instance.BgFadeOut(bgm);
	}
	public void FadeInCustom(float time)
	{
		SoundManager.Instance.BgFadeOutCustom(bgm, time);
	}
}
