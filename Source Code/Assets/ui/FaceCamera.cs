using UnityEngine;
using System.Collections;
 
public class FaceCamera : MonoBehaviour {

    public GameObject cam;

    void Start() {
    	cam = GameObject.Find("Main Camera");
    	transform.LookAt(2 * transform.position - cam.transform.position);
    }
 
    void Update() {
        transform.LookAt(2 * transform.position - cam.transform.position);
    }
}