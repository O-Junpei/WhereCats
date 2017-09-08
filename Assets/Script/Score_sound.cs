using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_sound : MonoBehaviour {

	public AudioClip[] audioClip;
//	public AudioClip audioClip2_b;
	public AudioSource audioSource;

	private bool isCalled = false;

	public void playSond () {

		if (!isCalled) {
			
			audioSource = gameObject.GetComponent<AudioSource> ();
			audioSource.clip = audioClip [0];
//			audioSource.PlayOneShot(audioClip[0],audioClip[0].length);
			audioSource.Play ();

			isCalled = true;
		}

	}

	public void stopSond () {

		audioSource.Stop ();

	}

	public void playSond2 () {

		audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.clip = audioClip[1];
		audioSource.PlayOneShot(audioClip[1],audioClip[1].length);
//		audioSource.Play ();

	}
		
}