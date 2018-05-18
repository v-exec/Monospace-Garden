using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//include UI
using UnityEngine.UI;

public class Textmaker : MonoBehaviour {

	List<GameObject> text = new List<GameObject>();
	const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789";

	void Start () {
		//
	}
	
	void Update () {
		GameObject[] bubbles = GameObject.FindGameObjectsWithTag("bubble");

		foreach (GameObject t in text) {
			if (t.gameObject) {
				Destroy(t.gameObject);
			}
		}

		text.Clear();

		foreach (GameObject bubble in bubbles) {
			GameObject t = Instantiate(Resources.Load("BubbleText", typeof(GameObject)) as GameObject, transform.position, transform.rotation);
			t.transform.SetParent(this.transform);

			int charAmount = Random.Range(4, 7);
			string myString = "";
			for(int i = 0; i < charAmount; i++) {
			    myString += glyphs[Random.Range(0, glyphs.Length)];
			}

			t.GetComponent<Text>().text = myString;
			text.Add(t);

			t.transform.position = bubble.transform.position;
		}
	}
}