using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicAudio;

[CreateAssetMenu(menuName = "Environment Interaction/Event")]
public class EventScriptableObject : ScriptableObject
{
    public EventEventProperty eventProperty = new EventEventProperty(); 
}
