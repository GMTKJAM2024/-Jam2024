using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace DynamicAudio
{
    [RequireComponent(typeof(AudioSource))]
    public class ObjectInfo : MonoBehaviour
    {
        public EventReference objectSoundBank;



       

        public Sound[] ownedSound;
        public ObjectParticle[] particle;
        
        public MaterialType materialType;
        public void Awake()
        {       

        }
        
    }
    
}