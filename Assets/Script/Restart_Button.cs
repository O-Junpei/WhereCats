using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Restart_Button : MonoBehaviour {

	public void Click() {
		　　// 「GameScene」シーンに遷移する
		　　SceneManager.LoadScene("Top");
		　}
	
}
