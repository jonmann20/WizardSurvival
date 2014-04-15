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
		this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
		this.timeLeft = startTime;
		this.gs = gameState.waiting;
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
		else if(this.gs == gameState.running){
			timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0, this.startTime);

			if(timeLeft == 0){
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
        if(scoreList != null){
            int i = 0;
            foreach(dreamloLeaderBoard.Score s in scoreList){
                EZGUI.placeTxt(s.playerName, 35, startX, 230 + (i * 100), e);
                EZGUI.placeTxt(s.score.ToString(), 35, startX, 230 + (i*100) + 50, e);

                ++i;
            }
        }
        else{
            e.color = new Color(0.95f, 0.95f, 0.95f);
            EZGUI.placeTxt("-No Entries-", 35, startX, 230, e);
        }

        e.color = new Color(0.95f, 0.95f, 0.95f);
        if(GLOBAL.gameOver){
            EZGUI.pulseTxt("Press \"X\" for start screen", 35, startX, 990 - 20, e);
        }
        else{
            EZGUI.placeTxt("Press \"△\" for start screen", 35, startX, 990 - 60, e);
            EZGUI.placeTxt("Press \"○\" to quit game", 35, startX, 990 - 20, e);
        }
	}

	public void FlipGameState(){
		if(GLOBAL.gameOver) return;

		if(this.gs == gameState.leaderboard){
			this.gs = gameState.running;
		}
		else{
			this.gs = gameState.leaderboard;
		}
	}

	public void AddScore(){
		if(dl.publicCode == "") Debug.LogError("You forgot to set the publicCode variable");
		if(dl.privateCode == "") Debug.LogError("You forgot to set the privateCode variable");
		
		dl.AddScore(this.playerName, totalScore);
		
		//this.gs = gameState.leaderboard;
	}
	
	
}
