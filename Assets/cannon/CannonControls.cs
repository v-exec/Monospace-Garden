using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonControls : MonoBehaviour {

	//elements
	private GameObject tip;
	private GameObject mesh;
	private LineRenderer line;
	private AudioController audio;
	private CameraRotation camControls;
	private GameObject bubble;
	private Scorekeeper scorer;
	private Vector3 camPosition;
	private Quaternion camRotation;

	//properties
	private float speed = 2;
	private float angleH = 0;
	private float angleV = 0;
	private float tipOffset = 1.25f;
	private bool timeStopped = false;
	public float strength = 0.0f;
	private float strengthLimit = 27.0f;
	private float strengthChange = 0.6f;
	public bool reloadReady = true;
	private bool reloaded = false;
	private float minShotSpeed = 5.0f;

	//initialization
	void Start() {
		//get cannon elements
		tip = GameObject.Find("Tip");
		mesh = GameObject.Find("Mesh");
		line = GameObject.Find("CannonLine").GetComponent<LineRenderer>();
		audio = GameObject.Find("AudioController").GetComponent<AudioController>();
		scorer = GameObject.Find("UI").GetComponent<Scorekeeper>();
		camControls = GameObject.Find("Main Camera").GetComponent<CameraRotation>();
		camPosition = GameObject.Find("Main Camera").transform.position;
		camRotation = GameObject.Find("Main Camera").transform.rotation;

		//position elements according to master gameobject
		tip.transform.position = new Vector3(0, tipOffset, 0);
	}

	//runtime
	void Update() {
		if (Input.GetKey("escape")) {
			Application.Quit();
		}

		//stop time..? why not?
		if (Input.GetKeyDown("f")) {
			if (timeStopped) {
				timeStopped = false;
				Time.timeScale = 1.0f;
				GameObject.Find("Main Camera").transform.position = camPosition;
				GameObject.Find("Main Camera").transform.rotation = camRotation;
				camControls.enabled = false;
			} else {
				timeStopped = true;
				Time.timeScale = 0.0f;
				camControls.enabled = true;
			}
		}

		rotate();
		reload();
		shoot();
	}

	//rotate on A/D and W/D (bias towards A/D on gimbal hierarchy) keyhold
	void rotate() {
		Vector3 a = mesh.transform.eulerAngles;

		if (Input.GetKey("a") && angleH > -80) {
			angleH -= speed;
		}
		if (Input.GetKey("d") && angleH < 80) {
			angleH += speed;
		}

		mesh.transform.rotation = Quaternion.Euler (angleH, a.y, angleV);
	}

	//reload if shot has been fired
	void reload() {
		if (reloadReady) {
			bubble = Instantiate(Resources.Load("Bubble", typeof(GameObject)) as GameObject, tip.transform.position, mesh.transform.rotation);
			reloadReady = false;
			reloaded = true;
		}
	}

	//shoot on SPACE keypress
	void shoot() {
		Vector3[] pos = {new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 2.0f - ((strength / strengthLimit) * 2), 0)};
		line.SetPositions(pos);

		if (Input.GetKey("space") && reloaded) {
			if (bubble.GetComponent<BubbleBehavior>().isFired == false) {
				if (strength < strengthLimit) strength += strengthChange;
			}
		}

		if (Input.GetKeyUp("space") && reloaded) {
			bubble.GetComponent<BubbleBehavior>().isFired = true;
			bubble.GetComponent<BubbleBehavior>().speed = strength * bubble.GetComponent<BubbleBehavior>().mass + minShotSpeed;
			audio.playPop(strength / strengthLimit);
			scorer.shots++;
			reloaded = false;

			GameObject[] dots = GameObject.FindGameObjectsWithTag("dot");
			foreach (GameObject dot in dots) {
				dot.GetComponent<StarDrift>().jitterAmount = strength / strengthLimit;
			}

			strength = 0.0f;

			StartCoroutine(reloadRoutine());
		}
	}

	//wait for reload
	IEnumerator reloadRoutine() {
		yield return new WaitForSeconds(1);
		reloadReady = true;
	}
}