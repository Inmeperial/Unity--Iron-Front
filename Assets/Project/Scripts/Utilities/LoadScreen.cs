using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class LoadScreen : MonoBehaviour
{
    public static LoadScreen Instance;
    public string sceneToLoad;
    public float progress;
    public AsyncOperation op;
    public float time;
    public bool called = false;
    
    
}
