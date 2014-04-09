////////////////////////////////////////////
///                                      ///
///         RealSky Version 1.4          ///
///  Created by: Black Rain Interactive  ///
///                                      ///
//////////////////////////////////////////// 

enum TimeOfDay {Day, Night};

var sunRise : Texture[];
var dayTime : Texture[];
var sunSet : Texture[];
var nightTime : Texture[];
var timeOfDay : TimeOfDay = TimeOfDay.Night;
var fadeDuration : float = 20.0f;
var dayDuration : float = 10.0f;
var nightDuration : float = 10.0f;
var sunRiseSetDuration : float = 10.0f;
var skySpeed : float = 0.0f;

var lightSource : GameObject;
var sunRiseColour : Color;
var dayTimeColour : Color;
var sunSetColour : Color;
var nightTimeColour : Color;
var mainCamera : Camera;
var skyBoxLayer : int = 8;

private var colour01 : Color;
private var colour02 : Color;

private var currentCycle : int = 0;
private var counter : float = 0.0f;
private var isPaused : boolean = false;
private var skyCamera : GameObject;

function Awake(){

    Counter();
    SkyBlend();
    TexSwap();
    
    if (mainCamera == null){
        return;
    }
    
    gameObject.layer = skyBoxLayer;
    
    skyCamera = new GameObject("SkyBoxCamera");
    skyCamera.AddComponent (Camera);
    skyCamera.camera.depth = -10;
    skyCamera.camera.clearFlags = CameraClearFlags.Color;
    skyCamera.camera.cullingMask = 1 << skyBoxLayer;
    skyCamera.transform.position = gameObject.transform.position;
    
    mainCamera.cullingMask = 1;
    mainCamera.clearFlags = CameraClearFlags.Depth;
}

function Start(){

    switch (timeOfDay){
    
        case (TimeOfDay.Night):
            renderer.material.SetTexture("_Texture01", nightTime[Random.Range(0, nightTime.Length)]);
            renderer.material.SetTexture("_Texture02", sunRise[Random.Range(0, sunRise.Length)]);
            colour01 = nightTimeColour;
            colour02 = sunRiseColour;
            currentCycle = 0; 
            break;
            
        case (TimeOfDay.Day):
            renderer.material.SetTexture("_Texture01", dayTime[Random.Range(0, dayTime.Length)]);
            renderer.material.SetTexture("_Texture02", sunSet[Random.Range(0, sunSet.Length)]);
            colour01 = dayTimeColour;
            colour02 = sunSetColour;
            currentCycle = 2;
            break;
    }
}

function Update(){
    
    if (mainCamera != null){
        skyCamera.transform.rotation = mainCamera.transform.rotation;
    }
}

function Counter(){

   while (true){
   
       if (isPaused == false){
           counter += Time.deltaTime;
       }
       
       yield;
   }
}

function SkyBlend(){

    while (true){
    
        var lerp : float = Mathf.PingPong (counter, fadeDuration) / fadeDuration;
        renderer.material.SetFloat ("_Blend", lerp);
        lightSource.light.color = Color.Lerp (colour01, colour02, lerp);
        
        transform.Rotate (Vector3.up * Time.deltaTime * skySpeed, Space.World);
        
        yield;
    }
}

function TexSwap(){
    
    while (true){
    
        yield WaitForSeconds (fadeDuration);
        isPaused = true;
        
        switch (currentCycle){
            case (3):
                yield WaitForSeconds (nightDuration);
                break;
                
            case (2):
                yield WaitForSeconds (sunRiseSetDuration);
                break;
                
            case (1):
                yield WaitForSeconds (dayDuration);
                break;
                
            case (0):
                yield WaitForSeconds (sunRiseSetDuration);
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
        
        yield;
    }
}