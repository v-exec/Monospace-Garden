using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBehavior : MonoBehaviour {

	//elements
	private GameObject tip;
	private GameObject mesh;
	private AudioController audio;
	private Scorekeeper scorer;

	private Rigidbody rb;

	//properties
	public float speed = 30f;
	public string color;
	public float size;
	public float falloff;
	public bool isHeavy;
	private float lifespan;
	private float life;

	public float mass = 0.0f;
	public bool isFired = false;
	public bool hasLaunched = false;

	public List<GameObject> neighbors = new List<GameObject>();

	private int score;

	private int speedCheckDelay = 0;

	private float massMin = 1.0f;
	private float massMax = 2.0f;
	private float massMinHeavy = 6.0f;
	private float massMaxHeavy = 9.0f;

	//initialization
	void Start() {
		//get elements
		tip = GameObject.Find("Tip");
		mesh = GameObject.Find("Mesh");
		audio = GameObject.Find("AudioController").GetComponent<AudioController>();
		scorer = GameObject.Find("UI").GetComponent<Scorekeeper>();
		rb = GetComponent<Rigidbody>();

		//assign random color
		int random = Random.Range(0, 3);
		switch(random) {
			case 0:
				color = "green";
				GetComponent<Renderer>().material = Resources.Load("Green Bubble", typeof(Material)) as Material;
				break;

			case 1:
				color = "blue";
				GetComponent<Renderer>().material = Resources.Load("Blue Bubble", typeof(Material)) as Material;
				break;

			case 2:
				color = "red";
				GetComponent<Renderer>().material = Resources.Load("Red Bubble", typeof(Material)) as Material;
				break;
		}

		//assign whether or not it's a heavy body
		random = Random.Range(0, 10);
		if (random > 7) isHeavy = true;
		else isHeavy = false;

		//give random mass
		float newMass = Random.Range(massMin, massMax);
		rb.mass = newMass;
		mass = newMass;

		//give appropriate size
		transform.localScale = new Vector3(newMass / 5, newMass / 5, newMass / 5);

		//set falloff according to mass
		falloff = newMass * 10;

		//give outline if isHeavy
		if (isHeavy) {
			GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 0.4f);
		} else {
			GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 0.0f);
			rb.drag = 0;
		}

		//set lifespan
		lifespan = 20000;
		life = lifespan;
	}
	
	//runtime
	void Update() {
		//check if no longer moving
		if (hasLaunched && rb.velocity.magnitude < 0.2f) {
			if (speedCheckDelay < 10) speedCheckDelay++;
			else {
				speedCheckDelay = 0;
				if (isHeavy) {
					float newMass = Random.Range(massMinHeavy, massMaxHeavy);
					rb.mass = newMass;
					rb.isKinematic = true;
				}
			}
		}

		//push forward if fired - keep on tip if not fired
		if (isFired) {
			if (!hasLaunched) {
				rb.AddRelativeForce(Vector3.up * speed, ForceMode.Impulse);
				hasLaunched = true;
				StartCoroutine(ageRoutine());
			}
		} else {
			transform.position = tip.transform.position;
			transform.rotation = mesh.transform.rotation;
		}

		//destroy if out of bounds
		if (Vector3.Distance(transform.position, new Vector3(0.0f, 0.0f, 0.0f)) > 100.0f) Destroy(this.gameObject);
	}

	void FixedUpdate() {
		gravitate();
	}

	//on collision
	void OnCollisionEnter (Collision collision) {
		//only collide if fired
		if (isFired && !isHeavy) {
			Destroy(this.gameObject);
		}
	}

	//gravitate towards neighbors
	void gravitate() {
		GameObject[] bubbles = GameObject.FindGameObjectsWithTag("bubble");
		List<Vector3> bubbleLocations = new List<Vector3>();

		//check distance from each
		foreach (GameObject bubble in bubbles) {
			float distance = Vector3.Distance(bubble.transform.position, transform.position);
			if (distance < falloff) {
				if (!isHeavy || !bubble.GetComponent<BubbleBehavior>().isHeavy) {
					if (bubble.GetComponent<BubbleBehavior>().isFired && isFired && bubble != this.gameObject) {
						rb.AddForce((bubble.transform.position - transform.position).normalized * ((bubble.GetComponent<Rigidbody>().mass * 7) / distance));
						bubbleLocations.Add(bubble.transform.position);

						score += 1;
						if (bubble.GetComponent<BubbleBehavior>().color == color) {
							switch (color) {
								case "green":
									audio.playArp1 = true;
									break;

								case "blue":
									audio.playArp2 = true;
									break;

								case "red":
									audio.playArp3 = true;
									break;
							}
							score += 1;
						}
						scorer.score += score;
						score = 0;
					}
				}
			}
		}

		List<Vector3> points = new List<Vector3>();
		LineRenderer line = this.gameObject.transform.GetChild(0).gameObject.GetComponent<LineRenderer>();

		//draw lines
		if (bubbleLocations.Count < 1) {
			Vector3[] def = {transform.position};
			line.positionCount = 1;
			line.SetPositions(def);
		} else {
			for (int i = 0; i < bubbleLocations.Count; i++) {
				points.Add(transform.position);
				points.Add(bubbleLocations[i]);
			}
			Vector3[] p = points.ToArray();
			line.positionCount = p.Length;
			line.SetPositions(p);
		}
	}

	//age
	IEnumerator ageRoutine() {
		yield return new WaitForSeconds(0.01f);
		life--;
		if (life <= 0) {
			Destroy(this.gameObject);
		}

		//dim outline color
		Material m = GetComponent<Renderer>().material;
		float col = life / lifespan;
		m.SetVector("_OutlineColor", new Vector4(col, col, col, 1.0f));

		StartCoroutine(ageRoutine());
	}
}