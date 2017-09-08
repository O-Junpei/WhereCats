using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Extra_Button : MonoBehaviour {

	public AudioClip audioClip;
	private AudioSource audioSource;

	public void Click() {

		audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.clip = audioClip;
		audioSource.PlayOneShot(audioClip,audioClip.length);
		StartCoroutine(waitTime());

	}

	IEnumerator waitTime() {

		yield return new WaitForSeconds (1);
		SceneManager.LoadScene("Extra");

	}

}