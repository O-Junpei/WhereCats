using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HighScore : MonoBehaviour {

	private int High_Score;

	void Start () {

		High_Score = LoadScene.Instance.highScore;

		this.GetComponent<Text>().text = "HIGH SCORE" + " : "  + ((int)High_Score).ToString ("0000000");
		
	}

	void Update () {
		
		
	}

}
