////////////////////////////////////////////
///                                      ///
///         RealSky Version 1.4          ///
///  Created by: Black Rain Interactive  ///
///                                      ///
//////////////////////////////////////////// 

using UnityEngine;
using System.Collections;

public enum TimeOfDay {Day, Night};

public class RealSky : MonoBehaviour {

    public Texture[] sunRise;
	public Texture[] dayTime;
	public Texture[] sunSet;
	public Texture[] nightTime;
    public TimeOfDay timeOfDay = TimeOfDay.Night;
    public float fadeDuration = 20.0f;
    public float dayDuration = 10.0f;
    public float nightDuration = 10.0f;
    public float sunRiseSetDuration = 10.0f;
	public float skySpeed = 0.0f;
	
	public GameObject lightSource;
	public Color sunRiseColour;
	public Color dayTimeColour;
	public Color sunSetColour;
	public Color nightTimeColour;
    public Camera mainCamera;
    public int skyBoxLayer = 8;
	
	Color colour01;
	Color colour02;
	
    int currentCycle = 0;
    float counter = 0.0f;
    bool isPaused = false;
    GameObject skyCamera;
	
	void Awake(){

        StartCoroutine("Counter");
		StartCoroutine("SkyBlend");
		StartCoroutine("TexSwap");
		//StartCoroutine("Lighting");

        if (mainCamera == null){
            return;
        }

        gameObject.layer = skyBoxLayer;

        skyCamera = new GameObject("SkyboxCamera");
        skyCamera.AddComponent<Camera>();
        skyCamera.camera.depth = -10;
        skyCamera.camera.clearFlags = CameraClearFlags.Color;
        skyCamera.camera.cullingMask = 1 << skyBoxLayer;
        skyCamera.transform.position = gameObject.transform.position;

        mainCamera.cullingMask = 1;
        mainCamera.clearFlags = CameraClearFlags.Depth;
	}
	
	void Start(){

        switch (timeOfDay){

            case TimeOfDay.Night:
                renderer.material.SetTexture("_Texture01", nightTime[Random.Range(0, nightTime.Length)]);
                renderer.material.SetTexture("_Texture02", sunRise[Random.Range(0, sunRise.Length)]);
                colour01 = nightTimeColour;
                colour02 = sunRiseColour;
                currentCycle = 0;
                break;

            case TimeOfDay.Day:
                renderer.material.SetTexture("_Texture01", dayTime[Random.Range(0, dayTime.Length)]);
                renderer.material.SetTexture("_Texture02", sunSet[Random.Range(0, sunSet.Length)]);
                colour01 = dayTimeColour;
                colour02 = sunSetColour;
                currentCycle = 2;
                break;
        }
	}

    void Update(){

        if (mainCamera != null){
            skyCamera.transform.rotation = mainCamera.transform.rotation;
        }
    }

    IEnumerator Counter(){

        while (true){

            if (isPaused == false){
                counter += Time.deltaTime;
            }

            yield return null;
        }
    }
	
	IEnumerator SkyBlend(){
		
		while (true){

            float lerp = Mathf.PingPong(counter, fadeDuration) / fadeDuration; 
            renderer.material.SetFloat( "_Blend", lerp );
			lightSource.light.color = Color.Lerp (colour01, colour02, lerp);
			
			transform.Rotate(Vector3.up * Time.deltaTime * skySpeed, Space.World);
			
			yield return null;
			
		}
	}
	
	IEnumerator TexSwap(){
		
		while (true){
			
			yield return new WaitForSeconds (fadeDuration);
            isPaused = true;

            switch (currentCycle){
                case 3:
                    yield return new WaitForSeconds(nightDuration);
                    break;
                case 2:
                    yield return new WaitForSeconds(sunRiseSetDuration);
                    break;
                case 1:
                    yield return new WaitForSeconds(dayDuration);
                    break;
                case 0:
                    yield return new WaitForSeconds(sunRiseSetDuration);
                    break;
            }

            isPaused = false;

			currentCycle += 1;

            if (currentCycle == 4){
                currentCycle = 0;
            }
			
			else if (currentCycle == 0){
				renderer.material.SetTexture("_Texture02", sunRise[Random.Range(0, sunRise.Length)]);
				colour02 = sunRiseColour;
			}
			
			else if (currentCycle == 1){
				renderer.material.SetTexture("_Texture01", dayTime[Random.Range(0, dayTime.Length)]);
				colour01 = dayTimeColour;
			}
			
			else if (currentCycle == 2){
				renderer.material.SetTexture("_Texture02", sunSet[Random.Range(0, sunSet.Length)]);
				colour02 = sunSetColour;
			}

            else if (currentCycle == 3){
                renderer.material.SetTexture("_Texture01", nightTime[Random.Range(0, nightTime.Length)]);
                colour01 = nightTimeColour;
            }
			
			yield return null;
			
		}
	}
	
	/*IEnumerator Lighting(){
		
		while (true){
			float time = fadeDuration *2;
            float lerp = Mathf.PingPong(counter, time) / time / 2 / 0.5f;
			lightSource.light.intensity = lerp * currentCycle;
			
			yield return null;
		}
	} */
}