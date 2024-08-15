using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
public class FmodVolumeChange : MonoBehaviour
{
    private BoxCollider boxCollider;

    public EventReference reference;

    public Color boxColor;
    EventInstance eventInstance;

    [Range(0, 1f)]
    public float ReverbTime;
    [Range(0, 1f)]
    public float WetLevel;
    [Range(0, 1f)]
    public float DryLevel;

    // Start is called before the first frame update
    void Start()
    {
        // Get the BoxCollider component attached to this GameObject
        boxCollider = GetComponent<BoxCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Draw Gizmos to visualize the BoxCollider's volume
    void OnDrawGizmos()
    {
        // Get the BoxCollider component attached to this GameObject
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            // Set the Gizmo color
            Gizmos.color = boxColor;

            // Get the BoxCollider's center and size
            Vector3 colliderCenter = boxCollider.center;
            Vector3 colliderSize = boxCollider.size;

            // Transform the center position to world space
            Vector3 worldCenter = transform.TransformPoint(colliderCenter);

            // Draw a wireframe cube to represent the BoxCollider's volume
            Gizmos.DrawWireCube(worldCenter, colliderSize);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ISetFmodParameter>() != null)
        {
            other.GetComponent<ISetFmodParameter>().SetParameter("ReverbTime", ReverbTime);
            other.GetComponent<ISetFmodParameter>().SetParameter("WetLevel", WetLevel);
            other.GetComponent<ISetFmodParameter>().SetParameter("DryLevel", DryLevel);
            Debug.Log("Change");
        }

       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ISetFmodParameter>() != null)
        {
            other.GetComponent<ISetFmodParameter>().SetParameter("ReverbTime", 0.25f);
            other.GetComponent<ISetFmodParameter>().SetParameter("WetLevel", 0f);
            other.GetComponent<ISetFmodParameter>().SetParameter("DryLevel", 0.5f);

        }
    }
}
