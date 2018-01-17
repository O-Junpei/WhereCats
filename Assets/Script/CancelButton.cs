using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CancelButton : MonoBehaviour {

	public GameObject userform;
	public Button restartButton;
	public Button writeScoreButton;
	public void OnClick(){
		userform.SetActive (false);
		restartButton.enabled = true;
		writeScoreButton.enabled = true;
		Debug.Log("Cancel Button click!");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
