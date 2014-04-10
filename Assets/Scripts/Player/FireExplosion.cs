using UnityEngine;
using System.Collections;

public class FireExplosion : MonoBehaviour {

    public ParticleEmitter p0, p1;

    void Start() {
        p0.Simulate(0.6f);
        p1.Simulate(0.8f);

        GameAudio.playFlame();

        Invoke("destroy", 2);
    }


    void destroy() {
        PhotonNetwork.Destroy(gameObject);
    }
}
