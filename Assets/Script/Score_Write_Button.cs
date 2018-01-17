using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Score_Write_Button : MonoBehaviour {
	public GameObject userform;
	public Button restartButton;
	public Button writeScoreButton;
	public void OnClick(){
		userform.SetActive (true);
		restartButton.enabled = false;
		writeScoreButton.enabled = false;
		Debug.Log("Button click!");
	}
}
