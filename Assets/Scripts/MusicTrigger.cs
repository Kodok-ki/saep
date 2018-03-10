using UnityEngine;
using System.Collections;

public class MusicTrigger : MonoBehaviour {

    AudioSource audio;
        
    void Start() {
        audio = GetComponent<AudioSource>();
    }

	void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            audio.Play();
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            audio.Stop();
        }
    }

}
