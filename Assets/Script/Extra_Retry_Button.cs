using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Extra_Retry_Button : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void Click() {
		// 「GameScene」シーンに遷移する
		SceneManager.LoadScene("Extra");
		}

}
