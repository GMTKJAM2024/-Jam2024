    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicAudio;
using UnityEngine.Events;
using FMODUnity;
using FMOD.Studio;

public class EnviromentInteractionEvent : MonoBehaviour
{
    public static void PlayParticleAndSound(ObjectInfo objectInfo, EnviromentEventProperty propertyHolder)
    {
        PlayPaticle(objectInfo, propertyHolder);
        PlaySound(objectInfo, propertyHolder);
    }
    public static void PlayEvent(ObjectInfo objectInfo, EnviromentEventProperty propertyHolder)
    {
        int value = ReturnObjectNumber(objectInfo);
        MaterialType type = (MaterialType)value;
        UnityEvent events = ReturnObjectEvent(propertyHolder, value);
        events?.Invoke();
    }
    public static void PlayPaticleAtPosition(ObjectInfo objectInfo, EnviromentEventProperty propertyHolder,Vector3 transformPar)
    {
        int value = ReturnObjectNumber(objectInfo);
        MaterialType type = (MaterialType)value;
        if (type == MaterialType.None)
        {

            int ranNum = Random.Range(0, objectInfo.particle.Length);
            GameObject particleObject = objectInfo.particle[ranNum].ParticleObject;
            if (!particleObject.GetComponent<ParticleSystem>()) return;
            Debug.LogWarning("PlaySound");
            // Reference ParticleSystem component (already retrieved)

            // Store MainModule in a variable
            var mainModule = particleObject.GetComponent<ParticleSystem>().main;

            // Modify startDelay and stopAction using the variable
            mainModule.startDelay = objectInfo.particle[ranNum].startDelay;
            mainModule.stopAction = objectInfo.particle[ranNum].action;

            // Instantiate at desired position
            Instantiate(particleObject, transformPar,Quaternion.identity);
            return;
        }

        
    }
    public static void PlayPaticle(ObjectInfo objectInfo, EnviromentEventProperty propertyHolder)
    {
        int value = ReturnObjectNumber(objectInfo);
        MaterialType type = (MaterialType)value;
        if (type == MaterialType.None)
        {
            
            int ranNum = Random.Range(0, objectInfo.particle.Length);
            GameObject particleObject = objectInfo.particle[ranNum].ParticleObject;
            if (!particleObject.GetComponent<ParticleSystem>()) return;
            Debug.LogWarning("PlaySound");
            // Reference ParticleSystem component (already retrieved)

            // Store MainModule in a variable
            var mainModule = particleObject.GetComponent<ParticleSystem>().main;

            // Modify startDelay and stopAction using the variable
            mainModule.startDelay = objectInfo.particle[ranNum].startDelay;
            mainModule.stopAction = objectInfo.particle[ranNum].action;

            // Instantiate at desired position
            Instantiate(particleObject, objectInfo.particle[ranNum].particleSpawnTransform);
            return;
        }

        ObjectParticle[] objectParticles = ReturnObjectParticle(propertyHolder, value);
        int ran = Random.Range(0, objectParticles.Length);
        GameObject particleObj = objectParticles[ran].ParticleObject;
        if (!particleObj.GetComponent<ParticleSystem>()) return;
   
        // Reference ParticleSystem component (already retrieved)

        // Store MainModule in a variable
        var main = particleObj.GetComponent<ParticleSystem>().main;

        // Modify startDelay and stopAction using the variable
        main.startDelay = objectParticles[ran].startDelay;
        main.stopAction = objectParticles[ran].action;

        // Instantiate at desired position
        Instantiate(particleObj, objectParticles[ran].particleSpawnTransform);
    }

    public static void PlaySound(ObjectInfo objectInfo, EnviromentEventProperty soundHolder)
    {
        
    }
    public static void PlaySound(ObjectInfo objectInfo)
    {
        
    }

    public static void PlaySound(MaterialType materialType,ref EnviromentEventProperty soundHolder, GameObject pos)
    {
      
        EventInstance eventInstance = soundHolder.soundEvent.soundInstance;

        if (eventInstance.isValid())
        {
            
            eventInstance.setParameterByName("MaterialType", (int)materialType);
            RuntimeManager.AttachInstanceToGameObject(soundHolder.soundEvent.soundInstance, pos.transform);
            eventInstance.start();

        }
        
    }
    static ObjectParticle[] ReturnObjectParticle(EnviromentEventProperty holder, int value)
    {
        return holder.ParicleEvent.particleEvent.particleProperties[value].particleSystems;
    }
    static UnityEvent ReturnObjectEvent(EnviromentEventProperty holder, int value)
    {
        return holder.eventScriptable.eventProperty.eventProperties[value].objectEvent;
    }
    static int ReturnObjectNumber(ObjectInfo objectInfo)
    {
        return (int)System.Enum.Parse(typeof(MaterialType), objectInfo.materialType.ToString()); // return the number of element in Enum
    }    

    public static void InitialSound(ref EnviromentEventProperty holder,GameObject gameOB)
    {
        
        holder.soundEvent.soundInstance = RuntimeManager.CreateInstance(holder.soundEvent.soundProperty);
        holder.soundEvent.soundInstance.setVolume(holder.soundEvent.VolumneRange);
        RuntimeManager.AttachInstanceToGameObject(holder.soundEvent.soundInstance, gameOB.transform);

    }
}
