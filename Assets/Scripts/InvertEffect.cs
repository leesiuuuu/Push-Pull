using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InvertEffect : MonoBehaviour
{
	private Material material;

	private Camera _camera;

	private Vector3 scale;

	private Coroutine invertEffect;

	[SerializeField]
	private float pervSize;

	private void Start()
	{
		material = GetComponent<SpriteRenderer>().material;
		material.SetFloat("_WaveDistanceFromCenter", -0.1f);
		_camera = transform.parent.GetComponent<Camera>();

		scale = transform.localScale;
		pervSize = _camera.orthographicSize;

	}
	private void Update()
	{
		float size = _camera.orthographicSize;
		transform.localScale = scale * (size / pervSize);
	}
	public void EffectOn()
	{
		if (invertEffect == null)
		{
			invertEffect = StartCoroutine(effect());
		}
	}
	private IEnumerator effect()
	{
		float prevWaveDis = material.GetFloat("_WaveDistanceFromCenter");
		float ElapsedTime = 0f;
		float Duration = 0.1f;
		while(ElapsedTime < Duration)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / Duration;
			float wavedistance;
			if(prevWaveDis < 0) wavedistance = Mathf.Lerp(-0.1f, 1f, t);
			else wavedistance = Mathf.Lerp(1f, -0.1f, t);
			material.SetFloat("_WaveDistanceFromCenter", wavedistance);
			yield return null;
		}
		invertEffect = null;
		yield break;
	}
}
