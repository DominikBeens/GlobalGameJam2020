using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    public AudioClip LoopAudio;
    public AudioSource mySource;

    void Update() {
        if (mySource.isPlaying == false) {
            mySource.clip = LoopAudio;
            mySource.Play();
            mySource.loop = true;
            Destroy(this);
        }
    }
}