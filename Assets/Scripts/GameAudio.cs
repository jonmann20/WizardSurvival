using UnityEngine;
using System.Collections;

public class GameAudio : MonoBehaviour {

	static int NUM_FLAME = 1;

	public static AudioSource[] jumpSrc, flameSrc;
	public static AudioSource chimesSrc, jumplandSrc, introSrc, spell0Src, lowHealthSrc, 
		invSelectSrc, windSrc, magicFailSrc, invMoveSrc, chainSrc, invNoMoveSrc, painSrc, flameShootSrc,
		gameOverSrc, freezeSpellSrc, freezeSpellCollisionSrc, battleMusicSrc;

	GameObject[] jumpHolder, flameHolder;
	GameObject audioHolder, chimesHolder, jumplandHolder, introHolder, spell0Holder, lowHealthHolder, 
		invSelectHolder, windHolder, magicFailHolder, invMoveHolder, invNoMoveHolder, flameShootHolder,
		chainHolder, painHolder, gameOverHolder, freezeSpellHolder, freezeSpellCollisionHolder, battleMusicHolder;

	void Awake(){
		audioHolder = new GameObject("_AudioHolder");
		audioHolder.transform.parent = GameObject.Find("MainCamera").transform;

		jumpSrc = new AudioSource[4];
		jumpHolder = new GameObject[4];

		flameSrc = new AudioSource[NUM_FLAME];
		flameHolder = new GameObject[NUM_FLAME];

		for(int i=0; i < NUM_FLAME; ++i){
			setSoundEffect(ref flameHolder[i], ref flameSrc[i], "Fire");
		}

        setSoundEffect(ref flameShootHolder, ref flameShootSrc, "TailWhip");

		setSoundEffect(ref jumpHolder[0], ref jumpSrc[0], "jump0");
		setSoundEffect(ref jumpHolder[1], ref jumpSrc[1], "jump1");
		setSoundEffect(ref jumpHolder[2], ref jumpSrc[2], "jump2");
		setSoundEffect(ref jumpHolder[3], ref jumpSrc[3], "jump3");
		setSoundEffect(ref jumplandHolder, ref jumplandSrc, "jumpLand");
		setSoundEffect(ref introHolder, ref introSrc, "HeavenSings");
		//setSoundEffect(ref spell0Holder, ref spell0Src, "warp3");
		setSoundEffect(ref lowHealthHolder, ref lowHealthSrc, "lowhealth");
		setSoundEffect(ref invSelectHolder, ref invSelectSrc, "enchant");
		setSoundEffect(ref windHolder, ref windSrc, "wind");
		setSoundEffect(ref magicFailHolder, ref magicFailSrc, "magicfail");
		setSoundEffect(ref invMoveHolder, ref invMoveSrc, "forcepulse");
		setSoundEffect(ref invNoMoveHolder, ref invNoMoveSrc, "zap13");
		setSoundEffect(ref chainHolder, ref chainSrc, "teleport");
		setSoundEffect(ref painHolder, ref painSrc, "pain1");
		setSoundEffect(ref chimesHolder, ref chimesSrc, "heal");
		setSoundEffect(ref gameOverHolder, ref gameOverSrc, "WinterSnow");
		setSoundEffect(ref freezeSpellHolder, ref freezeSpellSrc, "loudstream");
		//setSoundEffect(ref freezeSpellCollisionHolder, ref freezeSpellCollisionSrc, "freeze");
		setSoundEffect(ref battleMusicHolder, ref battleMusicSrc, "ColdHeartedv1");

		freezeSpellSrc.playOnAwake = true;
		freezeSpellSrc.audio.enabled = false;

		introSrc.loop = true;
		battleMusicSrc.loop = true;
		gameOverSrc.loop = true;
	}

	void Start(){
		audioHolder.transform.localPosition = Vector3.zero;
	}

	// NOTE: loads all sounds objects into the scene
	void setSoundEffect(ref GameObject holder, ref AudioSource src, string clip){
		holder = new GameObject(clip);
		holder.transform.parent = audioHolder.transform;

		src = holder.AddComponent<AudioSource>();
		src.playOnAwake = false;
		src.clip = Resources.Load<AudioClip>("Audio/" + clip);
	}

	public static void playGameOver(){
		if(!gameOverSrc.isPlaying){
			gameOverSrc.audio.time = 2.3f;
			gameOverSrc.audio.Play();
		}
	}

	public static void playBattleMusic(){
		battleMusicSrc.audio.Play();
	}

	public static void stopBattleMusic(){
		battleMusicSrc.audio.Stop();
	}

	public static void playFreezeSpell() {
		freezeSpellSrc.audio.enabled = true;
		freezeSpellSrc.audio.loop = true;
	}

	public static void stopFreezeSpell() {
		freezeSpellSrc.audio.enabled = false;
		freezeSpellSrc.audio.loop = false;
	}

	public static void playFreezeSpellCollision(){
		if(!freezeSpellCollisionSrc.isPlaying){
			freezeSpellCollisionSrc.audio.Play();
		}
	}

    public static void playFlameShoot(){
        flameShootSrc.audio.Play();
    }

	public static void playIntro(){
		introSrc.audio.Play();
	}

	public static void stopIntro(){
		introSrc.audio.Stop();
	}

	public static void playJump(){
		int i = Random.Range(0, 4);
		jumpSrc[i].audio.Play();
	}

	public static void playJumpland(){
		jumplandSrc.audio.Play();
	}

	public static void playPain(){
		painSrc.audio.Play();
	}

	public static void playSpell0(){
		if(!spell0Src.isPlaying){
			spell0Src.audio.Play();
		}
	}
	public static void playWind(){
		if(!windSrc.isPlaying){
			windSrc.audio.Play();
		}
	}

	public static void playLowHP(){
		if(!lowHealthSrc.isPlaying) {
			lowHealthSrc.volume = 2;
			lowHealthSrc.audio.Play();
		}
	}

	public static void playInvSelect(){
		invSelectSrc.audio.Play();
	}

	public static void playFlame(){

		for(int i=0; i < NUM_FLAME; ++i){
			if(flameSrc[i].isPlaying){
				flameSrc[i].volume -= 1 * Time.deltaTime;
				
				if((i != (NUM_FLAME-1)) && !flameSrc[i+1].isPlaying){
					flameSrc[i+1].volume = 1;
					flameSrc[i+1].audio.Play();
					break;
				}
			}
			else {
				flameSrc[i].volume = 1;
				flameSrc[i].audio.Play();
				break;
			}
		}
	}

	public static void playMagicFail(){
		magicFailSrc.audio.Play();
	}

	public static void playInvMove(){
		invMoveSrc.audio.Play();
	}

	public static void playInvNoMove(){
		invNoMoveSrc.audio.Play();
	}

	public static void playChimes(){
		chimesSrc.audio.Play();
	}

	public static void playChain(){
		if(!chainSrc.isPlaying){
			chainSrc.audio.Play();
		}
	}
}
