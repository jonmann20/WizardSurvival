using UnityEngine;
using System.Collections;

public class TestDontDestroyOnLoad : MonoBehaviour 
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
