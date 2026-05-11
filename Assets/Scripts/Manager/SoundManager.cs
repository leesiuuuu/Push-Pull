using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public enum PlayerSounds
{
    Jump = 0,
    Push = 1,
    Pull = 2,	
	Die = 3,
    SoundsCount = 4
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
	{
		get
		{
			if(instance == null) instance = new SoundManager();
			return instance;
		}
	}
	public AudioMixer audioMixer;

	private static SoundManager instance;
	private ObjectPool<AudioSource> sfxPool;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);

			sfxPool = new ObjectPool<AudioSource>(
				createFunc: () =>
				{
					GameObject go = new GameObject("PooledSFX");
					AudioSource source = go.AddComponent<AudioSource>();
					go.transform.SetParent(transform);
					source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
					return source;
				},
				actionOnGet: (source) => source.gameObject.SetActive(true),
				actionOnRelease: source =>
				{
					source.Stop();
					source.clip = null;
					source.gameObject.SetActive(false);
				},
				actionOnDestroy: source => Destroy(source.gameObject),
				defaultCapacity: 7,
				maxSize: 15
			);
        }
		else
		{
			Destroy(gameObject);
		}
	}
	private void Start()
	{
		if (PlayerPrefs.HasKey("MusicVolume"))
		{
			BGSoundVolume(PlayerPrefs.GetFloat("MusicVolume"));
		}
		else
		{
			BGSoundVolume(1);
		}

		if (PlayerPrefs.HasKey("SFXVolume"))
		{
			SFXSoundVolume(PlayerPrefs.GetFloat("SFXVolume"));
		}
		else
		{
			SFXSoundVolume(1);
		}
	}

    #region Sounds
	public void SFXPlay(string sfxName, AudioClip clip)
	{
		AudioSource source = sfxPool.Get();
		source.gameObject.name = sfxName + "SFX";		// 추후 문자열 최적화 진행(필요 시)
		source.clip = clip;
		source.Play();

		StartCoroutine(ReleaseAfterRealtime(source, clip.length));
	}
    public void BgFadeIn(AudioSource BgPlayer)
	{
		StartCoroutine(FadeIn(BgPlayer));
	}
	public void BgFadeInCustom(AudioSource BgPlayer, float volume, float time)
	{
		StartCoroutine(FadeIn(BgPlayer, volume, time));
	}
	public void BgFadeOut(AudioSource BgPlayer)
	{
		StartCoroutine(FadeOut(BgPlayer));
	}
	public void BgFadeOutCustom(AudioSource BgPlayer, float time)
	{
		StartCoroutine(FadeOut(BgPlayer, time));
	}
	public void BGSoundVolume(float val)
	{
        float n = Mathf.Log10(Mathf.Max(val, 0.0001f)) * 20;
        audioMixer.SetFloat("MusicVolume", n);
		PlayerPrefs.SetFloat("MusicVolume", val);
		PlayerPrefs.Save();
	}
	public void SFXSoundVolume(float val)
	{
        float n = Mathf.Log10(Mathf.Max(val, 0.0001f)) * 20;
        audioMixer.SetFloat("SFXVolume", n);
		PlayerPrefs.SetFloat("SFXVolume", val);
		PlayerPrefs.Save();
	}
    #endregion
    
	#region Coroutines
    private IEnumerator ReleaseAfterRealtime(AudioSource source, float delay)
	{
		yield return new WaitForSecondsRealtime(delay); // TimeScale 0이어도 기다림
		sfxPool.Release(source);
	}
	private IEnumerator FadeIn(AudioSource BgPlayer)
	{
		float ElapsedTime = 0f;
		float Duration = 0.8f;
		float volume = BgPlayer.volume;
		while(ElapsedTime < Duration)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / Duration;
			BgPlayer.volume = Mathf.Lerp(volume, 0f, t);
			yield return null;
		}
	}
	private IEnumerator FadeIn(AudioSource BgPlayer, float v, float Time)
	{
		float ElapsedTime = 0f;
		float volume = BgPlayer.volume;
		while (ElapsedTime < Time)
		{
			ElapsedTime += UnityEngine.Time.unscaledDeltaTime;
			float t = ElapsedTime / Time;
			BgPlayer.volume = Mathf.Lerp(volume, v, t);
			yield return null;
		}
	}
	private IEnumerator FadeOut(AudioSource BgPlayer)
	{
		float ElapsedTime = 0f;
		float Duration = 0.8f;
		float volume = BgPlayer.volume;
		while (ElapsedTime < Duration)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / Duration;
			BgPlayer.volume = Mathf.Lerp(volume, 1f, t);
			yield return null;
		}
	}
	private IEnumerator FadeOut(AudioSource BgPlayer, float ti)
	{
		float ElapsedTime = 0f;
		while (ElapsedTime < ti)
		{
			ElapsedTime += Time.deltaTime;
			float v = BgPlayer.volume;
			float t = ElapsedTime / ti;
			BgPlayer.volume = Mathf.Lerp(v, 1f, t);
			yield return null;
		}
	}
    #endregion
}
