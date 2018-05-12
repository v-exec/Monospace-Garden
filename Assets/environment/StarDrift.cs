using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDrift : MonoBehaviour {

	private float range = 0.003f;
	private float sizeRange = 0.1f;
	private float size = 0.4f;
	private Vector3 direction;
	private float sinOffset = 0.0f;
	private float sinSpeed = 0.05f;

	private float jitterRange = 0.05f;
	public float jitterAmount = 0.0f;
	private const float jitterFalloff = 0.02f;

	void Start() {
		sinOffset = Random.Range(0, 50);

		float randomX = Random.Range(-range, range);
		float randomY = Random.Range(-range, range);
		float randomZ = Random.Range(-range, range);
		direction = new Vector3(randomX, randomY, randomZ);
	}

	void Update () {
		if (jitterAmount > 0) {
			float randomX = Random.Range(-jitterRange * jitterAmount, jitterRange * jitterAmount);
			float randomY = Random.Range(-jitterRange * jitterAmount, jitterRange * jitterAmount);
			float randomZ = Random.Range(-jitterRange * jitterAmount, jitterRange * jitterAmount);
			transform.position = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z + randomZ);

			jitterAmount -= jitterFalloff;
		}

		transform.position += direction;

		sinOffset += sinSpeed;
		float off = Mathf.Sin(sinOffset);
		off = map(off, -1, 1, -0.002f, 0.002f);

		transform.localScale += new Vector3(off, off, off);
	}

	float map (float val, float from1, float to1, float from2, float to2) {
		return (val - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}