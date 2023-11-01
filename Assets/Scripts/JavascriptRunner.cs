using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Jint;
using System;

public class JavascriptRunner : MonoBehaviour
{
    Engine engine;

    // Start is called before the first frame update
    void Start()
    {
        engine = new Engine();
        engine.SetValue("log", new Action<object>(msg => Debug.Log(msg)));

        engine.Execute(@"
            var myVariable = 108;
            log('Hello from Javascript! myVariable = '+myVariable);
        ");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
