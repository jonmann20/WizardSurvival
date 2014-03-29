using UnityEngine;
using System.Collections;

public class GameAudio : MonoBehaviour {

	public static AudioSource[] jumpSrc;
	public static AudioSource jumplandSrc, introSrc, spell0Src;

	GameObject[] jumpHolder;
	GameObject audioHolder, jumplandHolder, introHolder, spell0Holder;

	void Awake(){
		audioHolder = new GameObject("_AudioHolder");

		jumpSrc = new AudioSource[4];
		jumpHolder = new GameObject[4];

		setSoundEffect(ref jumpHolder[0], ref jumpSrc[0], "jump0");
		setSoundEffect(ref jumpHolder[1], ref jumpSrc[1], "jump1");
		setSoundEffect(ref jumpHolder[2], ref jumpSrc[2], "jump2");
		setSoundEffect(ref jumpHolder[3], ref jumpSrc[3], "jump3");
		setSoundEffect(ref jumplandHolder, ref jumplandSrc, "jumpLand");
		setSoundEffect(ref introHolder, ref introSrc, "HeavenSings");
		setSoundEffect(ref spell0Holder, ref spell0Src, "warp3");
	}

	void setSoundEffect(ref GameObject holder, ref AudioSource src, string clip){
		holder = new GameObject(clip);
		holder.transform.parent = audioHolder.transform;

		src = holder.AddComponent<AudioSource>();
		src.playOnAwake = false;
		src.clip = Resources.Load<AudioClip>("Audio/" + clip);
	}

	public static void playJump(){
		int i = Random.Range(0, 4);
		jumpSrc[i].audio.Play();
	}

	public static void playJumpland(){
		jumplandSrc.audio.Play();
	}

	public static void playIntro(){
		introSrc.audio.Play();
	}

	public static void playSpell0(){
		spell0Src.audio.Play();
	}
}
