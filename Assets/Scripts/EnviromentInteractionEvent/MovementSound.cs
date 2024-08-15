using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicAudio;

public class MovementSound : MonoBehaviour,ISetFmodParameter
{
    public DynamicAudio.EnviromentEventProperty EnviromentEvent;

    public LayerMask layer;
    // Start is called before the first frame update

    [Range(0, 1f)]
    public float ReverbTime;
    [Range(0, 1f)]
    public float WetLevel;
    [Range(0, 1f)]
    public float DryLevel;
    void Start()
    {
        EnviromentInteractionEvent.InitialSound(ref EnviromentEvent,this.gameObject);
        EnviromentEvent.soundEvent.soundInstance.setParameterByName("ReverbTime", ReverbTime);
        EnviromentEvent.soundEvent.soundInstance.setParameterByName("WetLevel",  WetLevel);
        EnviromentEvent.soundEvent.soundInstance.setParameterByName("DryLevel",  DryLevel);

        EnviromentEvent.soundEvent.soundInstance.getParameterByName("ReverbTime", out ReverbTime);
        EnviromentEvent.soundEvent.soundInstance.getParameterByName("WetLevel", out WetLevel);
        EnviromentEvent.soundEvent.soundInstance.getParameterByName("DryLevel", out DryLevel);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position+ Vector3.up * 0.01f, Vector3.down * 0.1f, Color.red);
        EnviromentEvent.soundEvent.soundInstance.getParameterByName("ReverbTime", out ReverbTime);
        EnviromentEvent.soundEvent.soundInstance.getParameterByName("WetLevel", out WetLevel);
        EnviromentEvent.soundEvent.soundInstance.getParameterByName("DryLevel", out DryLevel);

    }

    public void PlaySound()
    {
        RaycastHit ray;
        if (Physics.Raycast(this.transform.position + Vector3.up * 0.01f, Vector3.down, out ray, 5f, layer))
        {
            if (ray.collider != null)
            {
                if (ray.collider.GetComponent<ObjectInfo>())
                {
                    var map = ray.collider.GetComponent<ObjectInfo>();
                        
                    
                    EnviromentInteractionEvent.PlaySound(map.materialType, ref EnviromentEvent,this.gameObject);
                    // Create a new GameObject and set its position



                    //EnviromentInteractionEvent.PlayPaticleAtPosition(map, EnviromentEvent, ray.point);

                }
            }
        }
    }
    public void PlaySoundAt(Vector3 position)
    {
        RaycastHit ray;
        if (Physics.Raycast(position + Vector3.up * 0.01f, Vector3.down, out ray, 0.05f, layer))
        {
            if (ray.collider != null)
            {
                if (ray.collider.GetComponent<ObjectInfo>())
                {
                    var map = ray.collider.GetComponent<ObjectInfo>();

                    Debug.Log("Playing sound at specified position: " + position);
                    EnviromentInteractionEvent.PlaySound(map.materialType,ref EnviromentEvent, this.gameObject);

                }
            }
        }
    }

    public  void SetParameter(string parameter, float value)
    {
        EnviromentEvent.soundEvent.soundInstance.setParameterByName(parameter, value);
    }
}
