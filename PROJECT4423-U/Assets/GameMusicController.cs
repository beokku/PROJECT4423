using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicController : MonoBehaviour {
    void Play() {
        GetComponent<AudioSource>().Play();
    }

    void Stop() {
        GetComponent<AudioSource>().Stop();
    }
}
