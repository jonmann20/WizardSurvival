using UnityEngine;
using System.Collections;

public class ForcefieldAbility : AbilityBase {

	static bool canFire = true;
	const int COOLDOWN_TIME = 10;
	static int cooldownTimeLeft = COOLDOWN_TIME;

	const int SPELL_DURATION = 15;
	int spellDurationLeft = 0;

	public static ForcefieldAbility that;

	void Start(){
		that = this;
	}

	public override void fire(){
		if(canFire){
			canFire = false;
			GameAudio.playSpell0();
			
			Transform w = GLOBAL.myWizard.transform;
			
			GameObject ff = PhotonNetwork.Instantiate(
				"ForcefieldPrefab",
				w.position, //+ w.forward.normalized,
				Quaternion.identity,//gameObject.transform.rotation,
				0
			) as GameObject;

			ff.transform.parent = w.parent;

			spellDurationLeft = SPELL_DURATION;
			StartCoroutine(canFireChargedown());
		}
		else {
			GameAudio.playMagicFail();
		}
	}
	
	public override string getAbilityName(){
		return "Forcefield";
	}
	
	public override string getAbilityDescription(){
		return "Hold! Hold!!!";
	}

	public void startCooldown(){
		cooldownTimeLeft = COOLDOWN_TIME;
		StartCoroutine(canFireChargeup());
	}

	IEnumerator canFireChargedown(){
		while(--spellDurationLeft > 0){
			yield return new WaitForSeconds(1);
		}
	}

	IEnumerator canFireChargeup(){
		while(--cooldownTimeLeft >= 0){
			if(cooldownTimeLeft <= 0){
				canFire = true;
			}
			else {
				yield return new WaitForSeconds(1);
			}
		}
	}

	void OnGUI(){
		EZGUI.init();
		EZOpt e = new EZOpt(Color.cyan, new Color(0.1f, 0.1f, 0.1f));
		e.font = GLOBAL.spookyMagic;
		e.dropShadowX = 1;

		// forcefield is active
		if(spellDurationLeft > 0){
			EZGUI.drawBox(0, 0, EZGUI.FULLW, EZGUI.FULLH, new Color(0, 0.9f + Mathf.PingPong(Time.time/2, 0.1f), 0, 0.175f));
			EZGUI.placeTxt("forcefiled time remaining: " + spellDurationLeft + "s", 25, EZGUI.FULLW - 230, 130, e);
		}
		else if(!canFire && cooldownTimeLeft != COOLDOWN_TIME){
			e.color = Color.red;
			EZGUI.placeTxt("cooldown: " + cooldownTimeLeft + "s", 25, EZGUI.FULLW - 170, 130, e);
		}
	}
}
