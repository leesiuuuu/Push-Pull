using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundPlayer : MonoBehaviour, ISelectHandler, ISubmitHandler
{
	[SerializeField]
	private AudioClip ac;
	[SerializeField]
	private AudioClip Submitac;
	public void OnSelect(BaseEventData eventData)
	{
		SoundManager.Instance.SFXPlay("sfx", ac);
	}

	public void OnSubmit(BaseEventData eventData)
	{
		if(Submitac != null) SoundManager.Instance.SFXPlay("submit", Submitac);
	}
}
