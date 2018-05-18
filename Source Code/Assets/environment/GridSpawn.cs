using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawn : MonoBehaviour {

	int size = 15;
	float spacing = 5.0f;

	void Start () {
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				for (int k = 0; k < size; k++) {
					float randomX = Random.Range(0f, 3f);
					float randomY = Random.Range(0f, 3f);
					float randomZ = Random.Range(0f, 3f);
					GameObject d = Instantiate(Resources.Load("Dot", typeof(GameObject)) as GameObject, new Vector3((i + randomX) * spacing, (j + randomY) * spacing, (k + randomZ) * spacing), transform.rotation);
					d.transform.SetParent(this.transform);
				}
			}
		}

		transform.position = new Vector3(-42.5f, -42.5f, -42.5f);
	}
}