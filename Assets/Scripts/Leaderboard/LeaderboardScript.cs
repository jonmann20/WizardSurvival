﻿using UnityEngine;
using System.Collections.Generic;

using InControl;

public class LeaderboardScript : MonoBehaviour {
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
	

	bool gameOver = true;

	// Reference to the dreamloLeaderboard prefab in the scene
	dreamloLeaderBoard dl;
	
	void Start () 
	{
		// get the reference here...
		this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
		
		// get the other reference hereß
		
		this.timeLeft = startTime;
		this.gs = gameState.waiting;
	}
	
	void Update () 
	{
		if (this.gs == gameState.running)
		{
			timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0, this.startTime);

			if (timeLeft == 0)
			{
				this.gs = gameState.enterscore;
			}
		}
		else if (this.gs == gameState.leaderboard){
			InputDevice device = InputManager.ActiveDevice;

			InputControl ctrl_X = device.GetControl(InputControlType.Action1);
			InputControl ctrl_T = device.GetControl(InputControlType.Action4);

			if(gameOver){
				InputControl ctrl_Start = device.GetControl(InputControlType.Start);

				if(ctrl_Start.WasPressed || ctrl_T.WasPressed || ctrl_X.WasPressed){
					Application.LoadLevel("Title");
				}
			}
			else {

				
				if(ctrl_X.WasPressed){
					Application.LoadLevel("Title");
				}
				else if(ctrl_T.WasPressed){ // triangle
					Application.Quit();
				}
			}
		}
	}


	void OnGUI()
	{
		if(this.gs != gameState.leaderboard)
		{
			return;
		}

		GUILayoutOption[] width200 = new GUILayoutOption[] {GUILayout.Width(200)};
		
		float width = 400;  // Make this wider to add more columns
		float height = 200;
		
		Rect r = new Rect(Screen.width/2 + (Screen.width * LEADERBOARD_WIDTH), Screen.height/2 + (Screen.width * LEADERBOARD_HEIGHT)/2, Screen.width * LEADERBOARD_WIDTH, Screen.height * LEADERBOARD_HEIGHT);
		GUILayout.BeginArea(r, new GUIStyle("box"));

		if(gameOver){
			GUILayout.Label("Press \"Start\" to continue");
		}
		else {
			GUILayout.Label("Press \"△\" to exit to start screen");
			GUILayout.Label("Press \"X\" to quit the game");
		}


		//GUILayout.BeginVertical();

//		if (this.gs == gameState.leaderboard)
//		{
//			GUILayout.Label("High Scores:");
//			List<dreamloLeaderBoard.Score> scoreList = dl.ToListHighToLow();
//			
//			if (scoreList == null) 
//			{
//				GUILayout.Label("(loading...)");
//			} 
//			else 
//			{
//				int maxToDisplay = 20;
//				int count = 0;
//
//				foreach (dreamloLeaderBoard.Score currentScore in scoreList)
//				{
//					count++;
//					GUILayout.BeginHorizontal();
//					GUILayout.Label(currentScore.playerName, width200);
//					GUILayout.Label(currentScore.score.ToString(), width200);
//					GUILayout.EndHorizontal();
//					
//					if (count >= maxToDisplay) break;
//				}
//			}
//		}
		//GUILayout.EndArea();

	}

	public void FlipGameState()
	{
		if( this.gs == gameState.leaderboard )
		{
			this.gs = gameState.running;
		}
		else
		{
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
