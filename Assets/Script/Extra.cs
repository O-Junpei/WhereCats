using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Extra : MonoBehaviour {

	public GameObject[] gameObjects;
	public GameObject[] neko;
	private GameObject selectObjects;
	public GameObject messagewindow;
	public GameObject Yes_No_BUtton;
	public GameObject yes_button;
	public GameObject no_button;
	public Text messageTex;
	public GameObject smoke;
	private AudioClip audioClip;
	private AudioSource audioSource;
	private GameObject doko_tex;
	private Button ybutton;
	private Button nbutton;
	private GameObject hitObject;
	private GameObject hitNeko;
	public GameObject restart_Button;
	public GameObject retry_Button;

	private bool extra_onoff = false;

	IEnumerator Start () {

		if (neko.Length < 2) {

			hitNeko = neko [0];

		} else {
			
			//Random.Range intが含まれている？
			hitNeko = neko[Random.Range (0, neko.Length)];

		}

		selectObjects = gameObjects[Random.Range (0, gameObjects.Length)];

//		Debug.Log (hitNeko);
//		Debug.Log (selectObjects);

		ybutton = yes_button.GetComponent<Button> ();
		ybutton.onClick.AddListener(yClick);
		nbutton = no_button.GetComponent<Button> ();
		nbutton.onClick.AddListener(nClick);

//		Text[] messageTex = messagewindow.GetComponentsInChildren<Text> ();

//			yield return new WaitForSeconds (0.5f);

		messagewindow.SetActive (true);

		yield return new WaitForSeconds (1.0f);

		messagewindow.SetActive (false);

	}
		
	void Update () {
		
		StartCoroutine("waitTime");

		if (extra_onoff) {

			ExtraStart ();

		}

	}
		
	void ExtraStart () {

		if(Input.GetMouseButtonDown(0)) {

		　　　Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		　　　RaycastHit hit = new RaycastHit();

		　　　if (Physics.Raycast(ray, out hit)){
				
				hitObject = hit.collider.gameObject;
				Animation anim =  hit.collider.gameObject.GetComponent<Animation> ();

				anim ["objectShake"].speed = 1.0f;
				anim.Play ();

				audioSource = hit.collider.gameObject.GetComponent<AudioSource>();
				//						audioSource.clip = audioClip;
				audioSource.Play();

					for (int i = 0; i < gameObjects.Length; i++) {

						if (gameObjects [i] != hit.collider.gameObject) {

						gameObjects [i].SetActive (false);

						}


					}

				messagewindow.SetActive (true);
				messageTex.text = "これにしますか？";

				Yes_No_BUtton.SetActive (true);

			}

		}

	}
		
	public void yClick() {

		messagewindow.SetActive (false);
		Yes_No_BUtton.SetActive (false);
		hitObject.SetActive (false);
		GameObject smoke_Instance =  Instantiate (smoke, hitObject.transform.position, Quaternion.identity);
		smoke_Instance.SetActive (true);

		StartCoroutine("yes_no_waitTime");

	}

	public void nClick() {

		messagewindow.SetActive (false);
		Yes_No_BUtton.SetActive (false);

		foreach (var objects in gameObjects)
		{

			objects.SetActive (true);

		}

	}


	IEnumerator waitTime() {

		yield return new WaitForSeconds (1.0f);
		extra_onoff = true;

	}

	IEnumerator yes_no_waitTime() {

		yield return new WaitForSeconds (1.0f);

		if (hitObject == selectObjects) {

			ParticleSystem particle = smoke.GetComponent<ParticleSystem> ();

			if (!particle.IsAlive ()) {

				yield return new WaitForSeconds (0.1f);

				hitNeko.transform.position = hitObject.transform.position;

			}
				
			if (hitNeko.name == "Nyanko") {

				messageTex.text = "にゃん子をゲット！";

			} else if (hitNeko.name == "Nyantarou") {

				messageTex.text = "にゃん太郎をゲット！";

			}

		} else {

			messageTex.text = "　はずれでした。";

		}
			
		messagewindow.SetActive (true);

		restart_Button.SetActive (true);
		retry_Button.SetActive (true);
	
	}



}
