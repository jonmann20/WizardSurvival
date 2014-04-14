using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MGSpawn.Spawn;

public class GameController : MonoBehaviour {
	
	public int wave;

	private GameObject[] spawners;

	public float[] SpawnWeights;

	private PlayerController masterClientController;

	public int numUnitstoSpawn = 5;
	private int SpawnedUnits = 0;
	private int numUnitsStillinScene = 0;

	private bool WaveOver = true;
	public float timeBetweenWaves = 7;
	private float waveTimer; 

	private float totalWeight;

	public float timeBetweenSpawns = 1;

	private bool spawning = false;
	private bool doneSpawning = false;

	private bool connected = false;
	

	// Use this for initialization
	void Start () {
		waveTimer = timeBetweenWaves;

	}
	
	// Update is called once per frame
	void Update () {

		if( connected == false )
		{
			if( PhotonNetwork.isNonMasterClientInRoom == true )
			{
				return;
			}
			else if( PhotonNetwork.room == null )
			{
				return;
			}
			else
			{

				print (PhotonNetwork.countOfPlayersInRooms);

				if( PhotonNetwork.isMasterClient )
				{
					if( Wizard.myWizard != null )
					{
						masterClientController = Wizard.myWizard.GetComponent<PlayerController>();
						wave = (int) PhotonNetwork.player.customProperties["Wave"];
						waveTimer = timeBetweenWaves;
						spawners = GameObject.FindGameObjectsWithTag("Spawner");
						if( spawners.Length == 0 )
						{
							Debug.LogError("Make sure you make spawners with tag 'Spawner'");
						}
						
						for(int i = 0; i < SpawnWeights.Length; i++ )
						{
							totalWeight += SpawnWeights[i];
						}
						connected = true;
					}
					else
					{
						return;
					}
				}
				else
				{
					//print ("This is not the master client");
				}
			}
		}
		else
		{
			if( PhotonNetwork.isMasterClient != true )
			{
				return;
			}
		}

		//In between waves
		if( WaveOver == true )
		{
			waveTimer -= Time.deltaTime;

			if( waveTimer <= 0 )
			{
				wave++;
				masterClientController.networkedProperties["Wave"] = wave;
				PhotonNetwork.player.SetCustomProperties(masterClientController.networkedProperties);
				waveTimer = timeBetweenWaves;
				WaveOver = false;
				doneSpawning = false;
				spawning = false;
				SpawnedUnits = 0;
				numUnitsStillinScene = 0;
				numUnitstoSpawn = numUnitstoSpawn + (int)((float)numUnitstoSpawn * 0.2f);
			}
			return;
		}
		else
		{
			if( spawning == false && doneSpawning == false )
			{
				spawning = true;
				StartCoroutine("WaveSpawn");
			}

		}


		//check to see if wave is over
		if( spawning == false && doneSpawning == true )
		{
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

			print ( enemies.Length );
			if ( enemies.Length <= 0 )
			{
				WaveOver = true;
				doneSpawning = false;
			}
			/*numUnitsStillinScene = 0;
			for( int i = 0; i < spawners.Length; i++ )
			{
				numUnitsStillinScene += spawners[i].GetComponent<MGSpawner>().numberOfUnits;
			}
			print (numUnitsStillinScene);
			if( numUnitsStillinScene <= 0 )
			{
				WaveOver = true;
				doneSpawning = false;
			}*/

		}

	
	}

	private IEnumerator WaveSpawn()
	{

		//still some units to spawn
		while( numUnitstoSpawn > SpawnedUnits )
		{
			yield return new WaitForSeconds(timeBetweenSpawns);
			int spawnerIdx = Random.Range(0,spawners.Length-1);
			float randomWeight = Random.Range(0,totalWeight);

			int index = -1;
			for(int i = 0; i < SpawnWeights.Length; i++ )
			{
				if( randomWeight < SpawnWeights[i] )
				{
					index = i;
					break;
				}
				randomWeight -= SpawnWeights[i];
			}
			if( index == -1 )
			{
				index = SpawnWeights.Length-2;
			}

			switch (index)
			{
			case 0:
				spawners[spawnerIdx].GetComponent<MGSpawner>().EnemyToSpawn = "Skeleton_Basic";
				break;
			case 1:
				spawners[spawnerIdx].GetComponent<MGSpawner>().EnemyToSpawn = "Skeleton_Spear";
				break;
			case 2:
				spawners[spawnerIdx].GetComponent<MGSpawner>().EnemyToSpawn = "Skeleton_Mage";
				break;
			case 3:
				spawners[spawnerIdx].GetComponent<MGSpawner>().EnemyToSpawn = "Ice_Golem";
				break;
			case 4:
				spawners[spawnerIdx].GetComponent<MGSpawner>().EnemyToSpawn = "Daemon";
				break;
			default:
				break;
			}


			spawners[spawnerIdx].GetComponent<MGSpawner>().Reset();
			spawners[spawnerIdx].GetComponent<MGSpawner>().StartSpawn();
			
			SpawnedUnits++;
			
		}

		spawning = false;
		doneSpawning = true;


	}
}
