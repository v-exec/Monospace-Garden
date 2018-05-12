using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//include UI
using UnityEngine.UI;

public class Scorekeeper : MonoBehaviour {

	//elements
	Text info;
	GameObject controls;

	//properties
	public int score;
	public int shots;

	public int peak = 0;
	public int PSOPeak = 0;
	
	public int publicScore;
	public int PSO;

	public float ratio;

	private bool controlsVisible = true;

	//initialization
	void Start() {
		info = GameObject.Find("Info").GetComponent<Text>();
		controls = GameObject.Find("Controls");
		StartCoroutine(getScore());
	}
	
	//runtime
	void Update() {
		if (publicScore != 0 && shots != 0) {
			ratio = ((float) publicScore) / ((float)shots);
			ratio = Mathf.Round(ratio * 100f) / 100f;
			int objCount = GameObject.FindGameObjectsWithTag("bubble").Length;
			PSO = publicScore / objCount;
			info.text = "Points per second: " + publicScore + "\nPeak: " + peak + "\n\nP/S/O: " + PSO + "\nP/S/O Peak: " + PSOPeak + "\n\nShots: " + shots + "\nCelestial Entities: " + objCount;
		} else {
			info.text = "No emulator data.";
		}

		//toggle controls visibility
		if (Input.GetKeyDown("c")) {
			controlsVisible = !controlsVisible;
			controls.SetActive(controlsVisible);
		}
	}

	//get score
	IEnumerator getScore() {
        yield return new WaitForSeconds(1);
        publicScore = score;
        if (publicScore > peak) peak = publicScore;
        if (PSO > PSOPeak) PSOPeak = PSO;
        score = 0;
        StartCoroutine(getScore());
    }
}