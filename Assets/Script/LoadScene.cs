using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadScene: MonoBehaviour {

	public static LoadScene Instance;

	public int highScore;

//	public static LoadScene Instance{
//		
//		get; private set;
//
//	}

	void Awake () {
		
		if (Instance == null) {

			Instance = this;
			DontDestroyOnLoad (gameObject);

		}
		else {
			

			Destroy (gameObject);

		}

	}
		
}
