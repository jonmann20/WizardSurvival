using UnityEngine;
using System.Collections.Generic;

using InControl;

public class Leaderboard : MonoBehaviour {
	float startTime = 10.0f;
	float timeLeft = 0.0f;
	
	int totalScore = 0;
	string playerName = "";
	//string code = "";
	
	public enum gameState {
		waiting,
		running,
		enterscore,
		leaderboard
	};
	
	public gameState gs;

	dreamloLeaderBoard dl;

	void Start(){
		dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
		timeLeft = startTime;
		gs = gameState.waiting;
	}
	
	void Update(){
		if(GLOBAL.gameOver || this.gs == gameState.leaderboard){
			InputDevice device = InputManager.ActiveDevice;
			
			InputControl ctrl_O = device.GetControl(InputControlType.Action2);
			InputControl ctrl_T = device.GetControl(InputControlType.Action4);
			
			if(GLOBAL.gameOver){
				InputControl ctrl_X = device.GetControl(InputControlType.Action1);
				
				if(ctrl_X.WasPressed || ctrl_T.WasPressed || ctrl_O.WasPressed){
					StartCoroutine(GLOBAL.ChangeSceneWithDelay("Title", 1));			// NOTE: why delaying??? also deleted Logger, it should not be obtrusive
				}
			}
			else {
				if(ctrl_T.WasPressed){
					StartCoroutine(GLOBAL.ChangeSceneWithDelay("Title", 1));
				}
				else if(ctrl_O.WasPressed){
					StartCoroutine(GLOBAL.QuitWithDelay(1));
				}
			}
		}
		else if(gs == gameState.running){
			timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0, this.startTime);

			if(timeLeft == 0){
				gs = gameState.enterscore;
			}
		}
	}

	void OnGUI(){
		if(gs != gameState.leaderboard){
			return;
		}

        EZGUI.init();

        float startX = 220 + 30;
        EZGUI.drawBox(startX - 30, 90, 1500, 900, new Color(0.09f, 0.09f, 0.09f, 0.44f));

        EZOpt e = new EZOpt();
        e.dropShadow = new Color(0.1f, 0.1f, 0.1f);
        e.leftJustify = false;

        EZGUI.placeTxt("Leaderboard", 50, EZGUI.HALFW, 170, e);
        e.leftJustify = true;

		if(PhotonNetwork.playerList.Length > 0){
			printPlayers(startX, e);
        }
        else{
            e.color = new Color(0.95f, 0.95f, 0.95f);
            EZGUI.placeTxt("-No Entries-", 35, startX, 230, e);
        }

        e.color = new Color(0.95f, 0.95f, 0.95f);
        if(GLOBAL.gameOver){
			e.hoverColor = new Color(1, 1, 0);
			e.activeColor = new Color(0.8f, 0.8f, 0);
			e.leftJustify = false;
            EZGUI.pulseBtn("Press \"X\" for start screen", 36, EZGUI.HALFW, 990 - 23, e);
        }
        else{
            EZGUI.placeTxt("Press \"△\" for start screen", 36, startX, 990 - 60, e);
            EZGUI.placeTxt("Press \"○\" to quit game", 36, startX, 990 - 20, e);
        }
	}

	void printPlayers(float startX, EZOpt e){
		float startY = 250;
		float lineHeight = 45;
		float padBot = 110;

		int teamScore = 0;

		for(int i=0; i < PhotonNetwork.playerList.Length; ++i) {
			PhotonPlayer p = PhotonNetwork.playerList[i];

			// Name
			EZGUI.placeTxt("Player " + p.ID, 35, startX, startY + (i * padBot), e);

			// Health
			if(p.customProperties.ContainsKey("Health")) {
				float tempHealth = (int)p.customProperties["Health"];
				EZGUI.drawBox(startX + 150, startY + (i * padBot) - 30, 1.7f * tempHealth, 20, Color.red);
			}

			// Score
			e.color = new Color(0.85f, 0.85f, 0.85f);
			int tempScore = (int)p.customProperties["Score"];
			teamScore += tempScore;
			EZGUI.placeTxt(tempScore.ToString() + " points", 35, startX, startY + (i*padBot) + lineHeight, e);
			e.color = Color.white;

			// Ability Name
			if(p.customProperties.ContainsKey("Ability")) {
				string abilityName = (string)p.customProperties["Ability"];

				if(abilityName == "none"){
					e.color = new Color(0.75f, 0.75f, 0.75f);
					EZGUI.placeTxt("no ability chosen", 40, startX + 500, startY + lineHeight/2 + (i * padBot), e);
				}
				else{
					EZGUI.placeTxt(abilityName, 40, startX + 500, startY + lineHeight/2 + (i * padBot), e);
				}
			}
			else{
				e.color = new Color(0.75f, 0.75f, 0.75f);
				EZGUI.placeTxt("no ability chosen", 40, startX + 500, startY + lineHeight/2 + (i * padBot), e);
			}

			// Active Player Indicator
			if(Wizard.myWizard != null && p == Wizard.myWizard.GetComponent<PhotonView>().owner){
				e.color = Color.green;
				EZGUI.placeTxt("(you)", 40, 1400, startY + lineHeight/2 + (i * padBot), e);
			}
		}

		e.color = new Color(0, 0.8f, 0.9f);
		EZGUI.placeTxt("Team Score: " + teamScore.ToString(), 35, 1400, 990 - 40, e);
	}

	public void FlipGameState(){
		if(GLOBAL.gameOver){
			return;
		}

		if(gs == gameState.leaderboard){
			gs = gameState.running;
		}
		else{
			gs = gameState.leaderboard;
		}
	}

	public void AddScore(){
		if(dl.publicCode == ""){
			Debug.LogError("You forgot to set the publicCode variable");
		}

		if(dl.privateCode == ""){
			Debug.LogError("You forgot to set the privateCode variable");
		}
		
		dl.AddScore(playerName, totalScore);
	}
	
	
}
