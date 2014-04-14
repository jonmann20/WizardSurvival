using UnityEngine;
using System.Collections;

public class FireExplosion : MonoBehaviour {

    public ParticleEmitter p0;

    void Start() {
        p0.Simulate(0.6f);

        GameAudio.playFlame();

        Invoke("destroy", 2);
    }

    void destroy() {
        Destroy(gameObject);
    }
}
