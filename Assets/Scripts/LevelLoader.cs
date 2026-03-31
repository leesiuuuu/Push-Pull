using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private float LoadingTime = 1f;

	public void LoadNextLevel(int SceneIndex)
	{
		FindObjectOfType<EventSystem>().gameObject.SetActive(false);
		StartCoroutine(LoadLevel(
			SceneManager.GetActiveScene().buildIndex + 1));
	}
	public void LoadScene(string name)
	{
		FindObjectOfType<EventSystem>().gameObject.SetActive(false);
		StartCoroutine(LoadLevel(name));
	}
	IEnumerator LoadLevel(int levelIndex)
	{
		animator.SetTrigger("Start");

		yield return new WaitForSeconds(LoadingTime);

		SceneManager.LoadScene(levelIndex);
	}

	IEnumerator LoadLevel(string name)
	{
		animator.SetTrigger("Start");

		yield return new WaitForSeconds(LoadingTime);

		SceneManager.LoadScene(name);
	}
}
