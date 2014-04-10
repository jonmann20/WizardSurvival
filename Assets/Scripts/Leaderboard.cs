using UnityEngine;
using System.Collections.Generic;

using InControl;

public class Leaderboard : MonoBehaviour {
	float startTime = 10.0f;
	float timeLeft = 0.0f;
	
	int totalScore = 0;
	string playerName = "";
	string code = "";
	
	public enum gameState {
		waiting,
		running,
		enterscore,
		leaderboard
	};
	
	public gameState gs;

	const float LEADERBOARD_WIDTH = .25f;
	const float LEADERBOARD_HEIGHT = .70f;

	// Reference to the dreamloLeaderboard prefab in the scene
	dreamloLeaderBoard dl;

	void Start(){
		// get the reference here...
		this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
		
		// get the other reference hereß
		
		this.timeLeft = startTime;
		this.gs = gameState.waiting;
	}
	
	void Update () 
	{
		if(GLOBAL.gameOver || this.gs == gameState.leaderboard){
			InputDevice device = InputManager.ActiveDevice;
			
			InputControl ctrl_O = device.GetControl(InputControlType.Action2);
			InputControl ctrl_T = device.GetControl(InputControlType.Action4);
			
			if(GLOBAL.gameOver){
				InputControl ctrl_Start = device.GetControl(InputControlType.Start);
				
				if(ctrl_Start.WasPressed || ctrl_T.WasPressed || ctrl_O.WasPressed){
					Application.LoadLevel("Title");
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
		else if(this.gs == gameState.running)
		{
			timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0, this.startTime);

			if (timeLeft == 0)
			{
				this.gs = gameState.enterscore;
			}
		}
	}

	void OnGUI(){
		if(!GLOBAL.gameOver && this.gs != gameState.leaderboard){
			return;
		}

        EZGUI.init();

        float startX = 210 + 30;
        EZGUI.drawBox(startX - 30, 90, 1500, 900, new Color(0.09f, 0.09f, 0.09f, 0.44f));

        EZOpt e = new EZOpt();
        e.dropShadow = new Color(0.1f, 0.1f, 0.1f);
        e.leftJustify = false;

        EZGUI.placeTxt("Leaderboard", 50, EZGUI.HALFW, 170, e);
        e.leftJustify = true;

        List<dreamloLeaderBoard.Score> scoreList = null;//dl.ToListHighToLow();
        if(scoreList != null) {
            int i = 0;
            foreach(dreamloLeaderBoard.Score s in scoreList) {
                EZGUI.placeTxt(s.playerName, 35, startX, 230 + (i * 100), e);
                EZGUI.placeTxt(s.score.ToString(), 35, startX, 230 + (i*100) + 50, e);

                ++i;
            }
        }
        else {
            e.color = new Color(0.95f, 0.95f, 0.95f);
            EZGUI.placeTxt("-No Entries-", 35, startX, 230, e);
        }

        e.color = new Color(0.95f, 0.95f, 0.95f);
        if(GLOBAL.gameOver) {
            EZGUI.placeTxt("Press \"Start\" to return to start screen", 33, startX, 990 - 20, e);
        }
        else {
            EZGUI.placeTxt("Press \"△\" to return to start screen", 33, startX, 990 - 60, e);
            EZGUI.placeTxt("Press \"○\" to quit game", 33, startX, 990 - 20, e);
        }
	}

	public void FlipGameState(){
		if(GLOBAL.gameOver) return;

		if(this.gs == gameState.leaderboard){
			this.gs = gameState.running;
		}
		else {
			this.gs = gameState.leaderboard;
		}
	}

	public void AddScore()
	{
		// add the score...
		if (dl.publicCode == "") Debug.LogError("You forgot to set the publicCode variable");
		if (dl.privateCode == "") Debug.LogError("You forgot to set the privateCode variable");
		
		dl.AddScore(this.playerName, totalScore);
		
		//this.gs = gameState.leaderboard;
	}
	
	
}
