using UnityEngine;
using System.Collections;

public class FireExplosion : ProjectileBase {

    public ParticleEmitter p0;

    void Start() {
        p0.Simulate(0.6f);

        GameAudio.playFlame();

		Destroy(gameObject, 2);
        //Invoke("destroy", 2);
    }

	//void destroy() {
	//	Destroy(gameObject);
	//}
}
