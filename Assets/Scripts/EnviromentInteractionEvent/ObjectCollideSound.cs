using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ObjectCollideSound : MonoBehaviour,ISetFmodParameter
{
    public EventReference eventReference;
    private FMOD.Studio.EventInstance soundEvent;

    Rigidbody objectRigidBody;


    [Range(0, 1f)]
    public float ReverbTime = 0.5f;
    [Range(0, 1f)]
    public float WetLevel=0.7f;
    [Range(0, 1f)]
    public float DryLevel=0.2f;
    [Range(0, 10000f)]
    public float Distance = 10f;
    [Range(0, 10000f)]
    public float MaxDistance = 30f;
    void Start()
    {
        objectRigidBody = GetComponent<Rigidbody>();
        soundEvent = RuntimeManager.CreateInstance(eventReference);
         soundEvent.setParameterByName("ReverbTime", ReverbTime);
         soundEvent.setParameterByName("WetLevel", WetLevel);
         soundEvent.setParameterByName("DryLevel", DryLevel);
        soundEvent.setParameterByName("Distance", WetLevel);
        soundEvent.setParameterByName("MaxDistance", DryLevel);
        //soundEvent.setParameterByName("Distance", DryLevel);


        soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(this.transform.position));
        RuntimeManager.AttachInstanceToGameObject(soundEvent, this.gameObject.transform);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            float velocityV1 = objectRigidBody.velocity.magnitude;
            float massM1 = objectRigidBody.mass;
            float kinematicForce1 = 0.5f * massM1 * velocityV1 * velocityV1;

            Rigidbody objectRigidBody2 = collision.gameObject.GetComponent<Rigidbody>();
            float velocityV2 = objectRigidBody2.velocity.magnitude;
            float massM2 = objectRigidBody2.mass;
            float kinematicForce2 = 0.5f * massM2 * velocityV2 * velocityV2;

            float kinematicForce = kinematicForce1 + kinematicForce2;

            float soundIntensity = kinematicForce / 1.2f;

            float absorptionNumber = 0.45f; // hệ số hấp thụ âm

            float distance = (1 / absorptionNumber) * Mathf.Log(0.01f / soundIntensity);

            // Set the volume of the FMOD event instance based on distance or other criteria
            //soundEvent.setVolume(1.0f); // Adjust volume as needed
            RuntimeManager.AttachInstanceToGameObject(soundEvent, this.gameObject.transform);
            // Start playing the FMOD event instance
            soundEvent.start();
        }
        else
        {
            float velocityV1 = objectRigidBody.velocity.magnitude;
            float massM1 = objectRigidBody.mass;
            float kinematicForce = 0.5f * massM1 * velocityV1 * velocityV1;

            float soundIntensity = kinematicForce / 1.2f;

            float absorptionNumber = 0.45f; // hệ số hấp thụ âm

            float distance = (1 / absorptionNumber) * Mathf.Log(0.01f / soundIntensity);

            // Set the volume of the FMOD event instance based on distance or other criteria
            //soundEvent.setVolume(1.0f); // Adjust volume as needed
            RuntimeManager.AttachInstanceToGameObject(soundEvent, this.gameObject.transform);
            // Start playing the FMOD event instance
            soundEvent.start();
        }
    }

    public void SetParameter(string parameter, float value)
    {
        Debug.Log("Change Box");
        soundEvent.setParameterByName(parameter, value);
    }
}
