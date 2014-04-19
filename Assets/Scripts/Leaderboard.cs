using UnityEngine;
using System.Collections.Generic;

using InControl;
using System;

public class Leaderboard : MonoBehaviour {

	float startTime = 10.0f;
	float timeLeft = 0.0f;
	
	int totalScore = 0;
	string playerName = "";
	//string code = "";

	Font arial;
	
	public enum gameState {
		waiting,
		running,
		enterscore,
		leaderboard,
		highscore
	};
	
	public static gameState gs;

	dreamloLeaderBoard dl;

	void Awake() {
		arial = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
	}

	void Start(){
		dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
		timeLeft = startTime;
		//gs = gameState.waiting;
		gs = gameState.highscore;
	}
	
	void Update(){
		if(GLOBAL.gameOver || gs == gameState.leaderboard){
			InputDevice device = InputManager.ActiveDevice;
			
			InputControl ctrl_O = device.GetControl(InputControlType.Action2);
			InputControl ctrl_T = device.GetControl(InputControlType.Action4);
			
			if(GLOBAL.gameOver){
				InputControl ctrl_X = device.GetControl(InputControlType.Action1);

				if(gs == gameState.leaderboard){
					if(ctrl_X.WasPressed || ctrl_T.WasPressed || ctrl_O.WasPressed) {
						gs = gameState.highscore;
					}
				}
				else {// highscore
					if(ctrl_X.WasPressed || ctrl_T.WasPressed || ctrl_O.WasPressed) {
						Application.LoadLevel("Title");
					}
				}
			}
			else {
				if(ctrl_T.WasPressed){
					Application.LoadLevel("Title");
				}
				else if(ctrl_O.WasPressed){
					Application.Quit();
				}
			}
		}
		else if(gs == gameState.running){
			timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0, startTime);

			if(timeLeft == 0){
				gs = gameState.enterscore;
			}
		}
	}

	void OnGUI(){
		if(gs != gameState.leaderboard && gs != gameState.highscore){
			return;
		}

        EZGUI.init();

        float startX = 222 + 30;
		float startY = 265;

        EZGUI.drawBox(startX - 30, 90, 1500, 900, new Color(0.09f, 0.09f, 0.09f, 0.44f));

        EZOpt e = new EZOpt();
        e.dropShadow = new Color(0.1f, 0.1f, 0.1f);
        e.leftJustify = false;

		e.font = GLOBAL.spookyMagic;

		if(gs == gameState.leaderboard){
			EZGUI.placeTxt("Leaderboard", 50, EZGUI.HALFW, 190, e);
			e.font = arial;
			e.leftJustify = true;

			if(PhotonNetwork.playerList.Length > 0) {
				printPlayers(startX, startY, e);
			}
			else {
				e.color = new Color(0.95f, 0.95f, 0.95f);
				EZGUI.placeTxt("-No Entries-", 35, startX, startY, e);
			}

			e.color = new Color(0.95f, 0.95f, 0.95f);
			if(GLOBAL.gameOver) {
				e.hoverColor = new Color(1, 1, 0);
				e.activeColor = new Color(0.8f, 0.8f, 0);
				e.leftJustify = false;
				EZGUI.pulseBtn("Press \"X\" to continue", 36, EZGUI.HALFW, 990 - 28, e);
			}
			else {
				EZGUI.placeTxt("Press \"△\" for start screen", 36, startX, 990 - 60, e);
				EZGUI.placeTxt("Press \"○\" to quit game", 36, startX, 990 - 20, e);
			}
		}
		else {//if(gs == gameState.highscore)
			EZGUI.placeTxt("High Scores", 50, EZGUI.HALFW, 190, e);
			e.font = arial;
			e.leftJustify = true;

			printHighScores(startX, startY, e);

			e.color = new Color(0.95f, 0.95f, 0.95f);
			e.hoverColor = new Color(1, 1, 0);
			e.activeColor = new Color(0.8f, 0.8f, 0);
			e.leftJustify = false;
			EZGUI.pulseBtn("Press \"X\" for start screen", 36, EZGUI.HALFW, 990 - 28, e);
		}
	}

	void printHighScores(float startX, float startY, EZOpt e){
		int lineHeight = 41;

		EZOpt titleOpt = e;
		titleOpt.color = Color.cyan;

		//--- best individual
		EZGUI.placeTxt("-Best Individual Score-", 35, startX, startY, titleOpt);
		
		string bestIndividualName = PlayerPrefs.GetString("BestIndividualName");
		if(String.IsNullOrEmpty(bestIndividualName)) {
			e.color = new Color(0.95f, 0.95f, 0.95f);
			EZGUI.placeTxt("-No Entry-", 35, startX + 100, startY + 240 + lineHeight*2, e);
		}
		else {
			EZGUI.placeTxt(bestIndividualName, 35, startX, startY + lineHeight, e);

			EZGUI.placeTxt(PlayerPrefs.GetInt("BestIndividualPoints") + " points", 35, startX + 50, startY + lineHeight*2, e);
			EZGUI.placeTxt(PlayerPrefs.GetInt("BestIndividualTeamPoints") + " team points", 35, startX + 50, startY + lineHeight*3, e);
			EZGUI.placeTxt(PlayerPrefs.GetInt("BestIndividualNum") + "th wave reached", 35, startX + 50, startY + lineHeight*4, e);
		}

		//--- best team
		EZGUI.placeTxt("-Best Team Score-", 35, startX, startY + 240, titleOpt);

		int bestTeam = PlayerPrefs.GetInt("BestTeam");
		//if(String.IsNullOrEmpty(bestTeam)){
		//	e.color = new Color(0.95f, 0.95f, 0.95f);
		//	EZGUI.placeTxt("-No Entry-", 35, startX + 50, startY + 240 + lineHeight, e);
		//}
		//else {
			EZGUI.placeTxt("Team Score: " + bestTeam + " points", 35, startX + 50, startY + 240 + lineHeight, e);	
		//}

		// player 0
		string t0 = PlayerPrefs.GetString("BestTeamName_0");
		int t0Num = PlayerPrefs.GetInt("BestTeamScore_0");
		if(String.IsNullOrEmpty(t0)) {
			e.color = new Color(0.95f, 0.95f, 0.95f);
			EZGUI.placeTxt("-No Entry-", 35, startX + 100, startY + 240 + lineHeight*2, e);
		}
		else {
			EZGUI.placeTxt(t0 + ": " + t0Num + " points", 35, startX + 100, startY + 240 + lineHeight*2, e);
		}
		
		// player 1
		string t1 = PlayerPrefs.GetString("BestTeamName_1");
		int t1Num = PlayerPrefs.GetInt("BestTeamScore_1");
		if(String.IsNullOrEmpty(t1)) {
			e.color = new Color(0.95f, 0.95f, 0.95f);
			EZGUI.placeTxt("-No Entry-", 35, startX + 100, startY + 240 + lineHeight*3, e);
		}
		else {
			EZGUI.placeTxt(t1 + ": " + t1Num + " points", 35, startX + 100, startY + 240 + lineHeight*3, e);
		}

		// player 2
		string t2 = PlayerPrefs.GetString("BestTeamName_2");
		int t2Num = PlayerPrefs.GetInt("BestTeamScore_2");
		if(String.IsNullOrEmpty(t2)) {
			e.color = new Color(0.95f, 0.95f, 0.95f);
			EZGUI.placeTxt("-No Entry-", 35, startX + 100, startY + 240 + lineHeight*4, e);
		}
		else {
			EZGUI.placeTxt(t2 + ": " + t2Num + " points", 35, startX + 100, startY + 240 + lineHeight*4, e);
		}

		// player 3
		string t3 = PlayerPrefs.GetString("BestTeamName_3");
		int t3Num = PlayerPrefs.GetInt("BestTeamScore_3");
		if(String.IsNullOrEmpty(t3)) {
			e.color = new Color(0.95f, 0.95f, 0.95f);
			EZGUI.placeTxt("-No Entry-", 35, startX + 100, startY + 240 + lineHeight*5, e);
		}
		else {
			EZGUI.placeTxt(t3 + ": " + t3Num + " points", 35, startX + 100, startY + 240 + lineHeight*5, e);
		}

		//--- longest wave
		EZGUI.placeTxt("-Longest Wave Reached-", 35, startX, startY + 540, titleOpt);

		string longestWaveName = PlayerPrefs.GetString("LongestWaveName");
		int longestWaveNum = PlayerPrefs.GetInt("LongestWaveNum");
		if(String.IsNullOrEmpty(longestWaveName)) {
			e.color = new Color(0.95f, 0.95f, 0.95f);
			EZGUI.placeTxt("-No Entry-", 35, startX + 50, startY + 540 + lineHeight, e);
		}
		else {
			EZGUI.placeTxt(longestWaveName + ": " + longestWaveNum + "th wave reached", 35, startX + 50, startY + 540 + lineHeight, e);
		}
	}

	void printPlayers(float startX, float startY, EZOpt e){
		float lineHeight = 41;
		float padBot = 172;

		int teamScore = 0;

		for(int i=0; i < PhotonNetwork.playerList.Length; ++i) {
			PhotonPlayer p = PhotonNetwork.playerList[i];

			bool isYourWizard = (GLOBAL.myWizard != null && p == GLOBAL.myWizard.GetComponent<PhotonView>().owner);

			// Name
			e.leftJustify = true;
			e.color = Color.white;
			if(isYourWizard){
				EZGUI.placeTxt(BETWEEN_SCENES.player_name, 35, startX, startY + (i * padBot), e);
			}
			else {
				EZGUI.placeTxt("Player " + p.ID, 35, startX, startY + (i * padBot), e);
			}

			// Health
			e.color = new Color(0.85f, 0.85f, 0.85f);
			if(p.customProperties.ContainsKey("Health")) {
				float tempHealth = (int)p.customProperties["Health"];
				EZGUI.placeTxt("health: ", 35, startX + 50, startY + (i*padBot) + lineHeight, e);
				EZGUI.drawBox(startX + 164, startY + (i * padBot) + 14, 1.7f * tempHealth, 20, Color.red);
			}

			// Score
			int yourScore = (int)p.customProperties["Score"];
			teamScore += yourScore;
			EZGUI.placeTxt("points: " + yourScore.ToString(), 35, startX + 50, startY + (i*padBot) + lineHeight*2, e);

			// Ability Name
			e.leftJustify = false;
			if(p.customProperties.ContainsKey("Ability")) {
				string abilityName = (string)p.customProperties["Ability"];

				if(abilityName == "none"){
					EZGUI.placeTxt("no base ability chosen", 35, EZGUI.HALFW, startY + lineHeight + (i * padBot), e);
				}
				else{
					e.color = Color.white;
					EZGUI.placeTxt(abilityName, 35, EZGUI.HALFW, startY + lineHeight + (i * padBot), e);
				}
			}
			else{
				EZGUI.placeTxt("no base ability chosen", 35, EZGUI.HALFW, startY + lineHeight/2 + (i * padBot), e);
			}

			// Active Player Indicator
			if(isYourWizard) {
				e.color = new Color(0, 0.9f, 0);
				EZGUI.placeTxt("(you)", 40, 1530, startY + lineHeight + (i * padBot), e);
			}
		}

		e.color = new Color(0.85f, 0.85f, 0.85f);
		EZGUI.placeTxt("Team Score: " + teamScore.ToString(), 35, 1530, 990 - 28, e);
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
