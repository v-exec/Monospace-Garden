using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	private AudioSource audioSource;
	private AudioClip[] audioClips;
	private AudioClip[] audioArps;
	private AudioSource[] arps;

	public bool playArp1 = false;
	public bool playArp2 = false;
	public bool playArp3 = false;

	public float volumeTransition = 0.00001f;
	public float volumeLimit = 0.3f;

	//initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();

		arps = new AudioSource[3];
		for (int i = 0; i < arps.Length; i++) {
			arps[i] = GameObject.Find("Arp" + i).GetComponent<AudioSource>();
		}

		audioClips = new AudioClip[7];
		for (int i = 0; i < audioClips.Length; i++) {
			audioClips[i] = Resources.Load<AudioClip>("Monospace Garden Audio Note" + (i+1));
		}

		audioArps = new AudioClip[3];
		for (int i = 0; i < audioArps.Length; i++) {
			audioArps[i] = Resources.Load<AudioClip>("Monospace Garden Audio Arp" + (i+1));
		}

		for (int i = 0; i < arps.Length; i++) {
			arps[i].clip = audioArps[i];
			arps[i].Play();
			arps[i].volume = 0.0f;
		}

		StartCoroutine(volumeRoutine());
	}

	public void playPop(float vol) {
		audioSource.clip = audioClips[Random.Range(0, audioClips.Length - 1)];
		audioSource.volume = vol;
		audioSource.Play();
	}

	//changes volume of different arpegiators
	IEnumerator volumeRoutine() {
		yield return new WaitForSeconds(0.5f);
		
		if (playArp1 && arps[0].volume < volumeLimit) {
			arps[0].volume += volumeTransition;
			playArp1 = false;
		} else if (arps[0].volume > 0) arps[0].volume -= volumeTransition;

		if (playArp2 && arps[1].volume < volumeLimit) {
			arps[1].volume += volumeTransition;
			playArp2 = false;
		} else if (arps[1].volume > 0) arps[1].volume -= volumeTransition;

		if (playArp3 && arps[2].volume < volumeLimit) {
			arps[2].volume += volumeTransition;
			playArp3 = false;
		} else if (arps[2].volume > 0) arps[2].volume -= volumeTransition;

		StartCoroutine(volumeRoutine());
	}
}
