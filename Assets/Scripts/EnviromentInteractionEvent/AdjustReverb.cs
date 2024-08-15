using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
public class AdjustReverb : MonoBehaviour
{
    public StudioEventEmitter emitter;

    [Range(0, 1f)]
    public float ReverbTime;
    [Range(0, 1f)]
    public float WetLevel;
    [Range(0, 1f)]
    public float DryLevel;
    // Start is called before the first frame update
    void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        emitter.SetParameter("ReverbTime", ReverbTime);
        emitter.SetParameter("WetLevel", WetLevel);
        emitter.SetParameter("DryLevel", DryLevel);
    }
}
