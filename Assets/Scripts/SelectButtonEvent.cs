using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectButtonEvent : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	public Animator animator;
	public CinemachineVirtualCamera vc;
	public bool isSelected = false;

	GameObject n;
	public void OnSelect(BaseEventData eventData)
	{
		if (!isSelected)
		{
			animator.SetTrigger("Appear");
			n = new GameObject("ex");
			n.transform.position = new Vector3(transform.position.x, 0, 0);
			vc.Follow = n.transform;
		}
		isSelected = true;
	}

	public void OnDeselect(BaseEventData eventData)
	{
		Destroy(n);
		isSelected = false;
		animator.SetTrigger("Disappear");
	}
}
