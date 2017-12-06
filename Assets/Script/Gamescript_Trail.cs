using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//FireBase
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class Gamescript_Trail : MonoBehaviour {

	public int waypoint = 7;
	public GameObject[]　gameObjects;
	private GameObject[] selectObjects;
	public GameObject messagewindow;
	public GameObject Unknown;
	public GameObject Target;
	public GameObject match;
	public GameObject miss;
	public Text startTex;
	public Text playTex;
	public Text Timertext;
	public Text hitCount;
	public Text Cleartext;
	public Text Excellent;
	public Text clear;
	public Text time;
	public Text correct;
	public Text perfect;
	public Text score;

	public GameObject Judge_start_Button;
	public GameObject Retry_Button;
	public GameObject Restart_Button;
	public GameObject Back_Object;

	private float nsecond;
	private int second;
	private int minute;
	private float old_second;

	private float distance = 100f;
	private int num = 0;
	private int count = 0;
	private bool mouseOn = false;
	public float startWait = 1.0f;
	public float waittime = 1.0f;
	private bool starOnoff = false;
	private bool timerOnoff = false;
	private bool judgeOnoff = false;
	private bool clearOnoff = false;
	private bool scoreOnoff = false;
	public int timeLimit = 10;
	private bool timeOnoff = false;
	private float timeScore;
	private int timeBonus;
	private int totalTime;
	private bool correctOnoff = false;
	private int correctScore = 0;
	private bool perfectOnoff = false;
	private bool totalScoreOnoff = false;
	private int totalScore = 0;
	private bool restartOnoff = false;

	private float score_c = 0.0f;
	private float score_t = 0.0f;
	private float score_m = 0.0f;
	private float score_p = 0.0f;
	private Transform[] ponints;

	private float m_progress = 0f;      //  進捗 [0, 1)
	private int m_ix = 0;               //  現データインデックス
	private Vector3[] m_data;
	public bool moveOn;
	private Animation anim;
	public float speed = 2.0f;
	public float stopTime = 4.0f;
	private float distanceBetween;
	private float reltiveTime;
	private Button jbutton;
	private bool startBool = false;
	private AudioSource audioSource;
	private AudioClip clip;

	// Firebase
	private DatabaseReference _FirebaseDB;
	private Firebase.Auth.FirebaseUser _FirebaseUser;

	void Start () {


		Debug.Log("Hello World");
		// Firebase RealtimeDatabase接続初期設定
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://wherecats-18333.firebaseio.com");

		// Databaseの参照先設定
		_FirebaseDB = FirebaseDatabase.DefaultInstance.GetReference("Ranking");

		_FirebaseDB.OrderByChild("score").LimitToLast(1).GetValueAsync().ContinueWith(task => {
					if (task.IsFaulted) {
						// 取得エラー
						Debug.Log("[ERROR] get ranking sort");
					} else if (task.IsCompleted) {
						// 取得成功
						Debug.Log("[INFO] get ranking success.");
						DataSnapshot snapshot = task.Result;
						IEnumerator<DataSnapshot> result = snapshot.Children.GetEnumerator();
						int rank = 0;

						string json = snapshot.GetRawJsonValue();


					}
				});

/*
		FirebaseDatabase.DefaultInstance
			 .GetReference("ranking")
			 .ValueChanged += HandleValueChanged;
*/

		audioSource = Target.GetComponent<AudioSource> ();
		AudioClip clip = Target.GetComponent<AudioSource> ().clip;

		Target.SetActive (false);

		jbutton = Judge_start_Button.GetComponent<Button> ();
		jbutton.onClick.AddListener(onClick);

		foreach (var objects in gameObjects) {

			objects.SetActive (false);

		}

		enabled = true;
		startTex.enabled = true;
		messagewindow.SetActive (true);
		selectObjects = new GameObject[waypoint];
		int n = waypoint;
		correctScore = n;

		int place = gameObjects.Length;

		while (place > 1) {

			place--;
			int k = Random.Range (0,place + 1);
			GameObject tmp = gameObjects[k];
			gameObjects[k] = gameObjects[place];
			gameObjects[place] = tmp;

		}

		for (int i = 0; i < n; i++) {

			selectObjects[i] = gameObjects[i];

		}

		Target.transform.position = new Vector3 (gameObjects[0].transform.position.x, gameObjects[0].transform.position.y, gameObjects[0].transform.position.z);
		Target.SetActive (true);

		mouseOn = true;

		nsecond = 0.0f;
		second = 0;
		minute = 0;
		timeBonus = timeLimit;
		timeScore = 0.0f;

		//ランダムに選ばれた場所の順番
		foreach (var objects in selectObjects)
		{

			Debug.Log(objects);

		}

	}

	void Update () {

		if (mouseOn) {

			if (Input.GetMouseButtonDown (0)) {

				startTex.enabled = false;
				messagewindow.SetActive (false);
				Destroy (Unknown);

				foreach (var objects in selectObjects) {

					objects.SetActive (true);

				}

				StartCoroutine ("waitTime");

			}

		}

		MoveTarget_b ();
		Judge ();
		ClearGame ();
		Score ();

	}

	void MoveTarget_b () {

		if (startBool) {

			playTex.enabled = true;
			messagewindow.SetActive (true);
			Judge_start_Button.SetActive (true);
			StartCoroutine ("waitTime2");

		}

		if(starOnoff) {

			if (m_progress == 0) {


				audioSource.Play ();

			}

			if (moveOn) {

				anim = gameObjects [m_ix + 1].GetComponent<Animation> ();

			} else {

				anim = gameObjects [m_ix].GetComponent<Animation> ();

			}

			m_progress += speed * Time.deltaTime;

			if (m_progress > stopTime * 0.5f && m_progress < stopTime) {

				if (m_ix < waypoint - 1) {

					anim ["objectShake"].speed = 1.5f;
					anim.Play ();

				}

			}

			if( m_progress >= stopTime ) {

				if (m_ix < waypoint - 1) {

					anim.Stop();
					gameObjects[m_ix+1].transform.rotation = Quaternion.Euler (0, 0, 0);

				}

				m_progress = 0f;
				++m_ix;

				if (m_ix >= waypoint - 1) {

					Destroy (Target);

					StartCoroutine ("judgstartWait");

				}

			}

			if (Target && m_ix < waypoint - 1) {

				Vector3 Target_position;

				if (moveOn) {

					Target_position = Vector3.Lerp (gameObjects[m_ix].transform.position, gameObjects[m_ix + 1].transform.position, m_progress);


					if (gameObjects[m_ix +1].transform.position.x < gameObjects[m_ix].transform.position.x) {

						Target.transform.rotation = Quaternion.Euler (0, 0, 0);

					} else if (gameObjects[m_ix +1].transform.position.x > gameObjects[m_ix].transform.position.x) {

						Target.transform.rotation = Quaternion.Euler (0, 180, 0);

					}

					Target_position.z = 0.1f;

				} else {

					Target_position = new Vector3 (gameObjects[m_ix].transform.position.x, gameObjects[m_ix].transform.position.y, gameObjects [m_ix].transform.position.z);

					Target_position.z = -5.5f;

				}

				Target.transform.position = Target_position;

			}

		}

		if (judgeOnoff && timerOnoff) {



			nsecond += Time.deltaTime * 60;
			timeScore += Time.deltaTime * 60;
			//			timeBonus = (int)(timeScore);
			totalTime = (minute * 3600) + (second * 60) + (int)(nsecond);
			//点数計算
			timeBonus = timeLimit * 60 - totalTime;
			if (timeBonus < 0) {

				timeBonus = 0;

			}

			if (nsecond >= 60.0f) {

				second++;
				nsecond = nsecond - 60;

			}

			if (second >= 60.0f) {

				minute++;
				second = second - 60;

			}

			Timertext.text = minute.ToString ("00") + ":" + second.ToString ("00") + ":" + ((int)(nsecond)).ToString ("00");

		}

	}

	void Judge () {

		if(judgeOnoff) {

			starOnoff = false;
			moveOn = false;
			playTex.enabled = false;
			messagewindow.SetActive (false);
			Judge_start_Button.SetActive (false);
			timerOnoff = true;
			Timertext.enabled = true;
			hitCount.text = count.ToString () + "/" + waypoint;
			hitCount.enabled = true;

			if (Input.GetMouseButtonDown (0)) {

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit = new RaycastHit ();

				if (num < waypoint) {

					if (Physics.Raycast (ray, out hit, distance)) {

						string objectName = hit.collider.gameObject.name;
						string hitName = hit.collider.gameObject.name;
						string objectsName = gameObjects [num].name;

						Vector3 pos = hit.collider.gameObject.transform.position;

						pos.z = -5.5f;
						Debug.Log ("Hit!!!!");

						if (hitName == objectsName) {

							Debug.Log (hitName + " " + "Matdh!!!");

							GameObject instanceObject = Instantiate (match, pos, Quaternion.identity);
							instanceObject.SetActive (true);

							count++;

							Destroy (instanceObject, 0.3f);

							num++;

						} else if (hitName != objectsName) {

							if (correctScore > 0) {

								correctScore--;
							}

							Debug.Log (hitName + " " + "Miss!!!");

							GameObject instanceObject = Instantiate (miss, pos, Quaternion.identity);

							instanceObject.SetActive (true);

							Destroy (instanceObject, 0.3f);

						}

					}

				}

			}

			if (count == waypoint) {


				timerOnoff = false;


				//				Debug.Log (timeBonus);
				//				Debug.Log (totalTime);

				StartCoroutine("clearWaitTime");

			}

		}

	}

	void ClearGame() {

		if (clearOnoff) {

			StartCoroutine("waitTime2");

			Text[] CleartextComponent = Cleartext.GetComponents<Text>();
			Score_sound clear_Sound = CleartextComponent[0].GetComponent<Score_sound> ();
			clear_Sound.playSond ();

			CleartextComponent[0].enabled = true;

			Back_Object.SetActive (true);
			StartCoroutine("waitTime3");

		}

	}

	void Score() {

		if (scoreOnoff) {

			Text[] clearGroup = clear.GetComponentsInChildren<Text>();
			Score_sound clear_Sound = clearGroup[1].GetComponent<Score_sound> ();

			foreach (var objects in clearGroup)
			{
				objects.enabled = true;
			}

			if (score_c < 500) {

				score_c += Time.deltaTime * 600;

				clear_Sound.playSond ();


			} else if (score_c > 500) {

				score_c = 500;
				clear_Sound.stopSond ();
				clear_Sound.playSond2 ();

				StartCoroutine("waitTime4");
			}

			clearGroup[1].text = ((int)score_c).ToString ("000");

			if (timeOnoff) {

				Text[] timeGroup = time.GetComponentsInChildren<Text> ();
				Score_sound time_Sound = timeGroup [1].GetComponent<Score_sound> ();

				foreach (var objects in timeGroup) {
					objects.enabled = true;
				}

				if (score_t < 10 * timeBonus) {

					score_t += Time.deltaTime * 3000;

					time_Sound.playSond ();


				} else if (score_t >= 10 * timeBonus) {

					score_t = 10 * timeBonus;
					time_Sound.stopSond ();
					time_Sound.playSond2 ();

					StartCoroutine("waitTime5");
				}

				timeGroup[1].text = ((int)score_t).ToString ("000000");

			}

			if (correctOnoff) {

				Text[] correctGroup = correct.GetComponentsInChildren<Text> ();
				Score_sound correct_Sound = correctGroup [1].GetComponent<Score_sound> ();

				foreach (var objects in correctGroup) {
					objects.enabled = true;
				}

				if (correctScore == 0) {

					StartCoroutine ("waitTime7");
					correct_Sound.playSond2 ();

				}

				if (score_m < 500 * correctScore) {

					score_m += Time.deltaTime * 3000;

					correct_Sound.playSond ();


				} else if (score_m > 500 * correctScore) {

					score_m = 500 * correctScore;
					correct_Sound.stopSond ();
					correct_Sound.playSond2 ();

					if (correctScore == waypoint) {

						StartCoroutine ("waitTime6");

					} else {

						StartCoroutine ("waitTime7");

					}

				}

				correctGroup[1].text = ((int)score_m).ToString ("000000");

			}

			if (perfectOnoff) {

				Text[] parfectGroup = perfect.GetComponentsInChildren<Text> ();
				Score_sound perfect_Sound = parfectGroup[1].GetComponent<Score_sound> ();

				foreach (var objects in parfectGroup) {
					objects.enabled = true;
				}

				if (score_p  < 10000) {

					score_p  += Time.deltaTime * 3000;

					perfect_Sound.playSond ();


				} else if (score_p > 10000) {

					score_p  = 10000;
					perfect_Sound.stopSond ();
					perfect_Sound.playSond2 ();

					StartCoroutine ("waitTime7");

				}

				parfectGroup[1].text = ((int)score_p).ToString ("000000");

			}

			if (totalScoreOnoff) {

				Text[] scoreGroup = score.GetComponentsInChildren<Text> ();
				Score_sound perfect_Sound = scoreGroup[1].GetComponent<Score_sound> ();
				perfect_Sound.playSond ();
				foreach (var objects in scoreGroup) {
					objects.enabled = true;
				}

				totalScore = ((int)score_c) + ((int)score_t) + ((int)score_m) + ((int)score_p);
				scoreGroup[1].text = ((int)totalScore).ToString ("0000000");


			//ハイスコア保持を保持
			if (LoadScene.Instance) {

				if (LoadScene.Instance.highScore < (int)totalScore) {

				LoadScene.Instance.highScore = (int)totalScore;

				}

			}

				StartCoroutine ("restartWait");

			}


			if (restartOnoff) {

				Retry_Button.SetActive (true);
				Restart_Button.SetActive (true);

			}

		}

	}

	public void onClick() {

		judgeOnoff = true;

	}

	IEnumerator waitTime() {

		mouseOn = false;
		yield return new WaitForSeconds (startWait);
		starOnoff = true;

	}

	IEnumerator waitTime2() {

		yield return new WaitForSeconds (1);
		starOnoff = false;

	}



	IEnumerator judgstartWait() {

		starOnoff = false;
		yield return new WaitForSeconds (1);
		startBool = true;

	}

	IEnumerator clearWaitTime() {

		yield return new WaitForSeconds (1);
		clearOnoff = true;

	}

	IEnumerator waitTime3() {

		yield return new WaitForSeconds (1);
		scoreOnoff = true;

	}

	IEnumerator waitTime4() {

		yield return new WaitForSeconds (1);
		timeOnoff = true;

	}

	IEnumerator waitTime5() {

		timeOnoff = false;
		yield return new WaitForSeconds (1);
		correctOnoff = true;

	}

	IEnumerator waitTime6() {

		correctOnoff = false;
		yield return new WaitForSeconds (1);
		perfectOnoff = true;

	}

	IEnumerator waitTime7() {

		correctOnoff = false;

		yield return new WaitForSeconds (1);
		totalScoreOnoff = true;

	}

	IEnumerator restartWait() {

		yield return new WaitForSeconds (1);
		restartOnoff = true;

	}

}
