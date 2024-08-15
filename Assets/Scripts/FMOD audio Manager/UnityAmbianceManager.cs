using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityAmbianceManager : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioClip playClipOnStart;
    private AudioClip chosenClip;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource1 == audioSource2)
            Debug.LogWarning("Heads Up! You've referenced the exact same Audio Source component twice on your 'Unity Ambiance Manager' component attached to the game object called " +
                gameObject.name + ". Make sure the fields 'Audio Source 1' and 'Audio Source 2' " +
                "are referencing two separate Audio Source components in order for the 'Unity Ambiance Manager' script to work properly.");
        audioSource1.loop = true;
        audioSource2.loop = true;
        audioSource1.playOnAwake = false;
        audioSource2.playOnAwake = false;
        if (playClipOnStart != null)
        {
            audioSource1.clip = playClipOnStart;
            audioSource1.Play();
        }
        else
            audioSource1.volume = 0f;
        audioSource2.volume = 0f;
    }

    public void PlayAmbainceClip(AudioClip clip)
    {
        if (audioSource1.clip == clip || audioSource2.clip == clip)
            Debug.LogWarning("Warning! A 'Trigger Box Audio Blender' component just tried to play '" + clip.name + "', but it's already playing that clip. " +
                "I'd recommend you check where you've put your audio blenders. Make sure the target object can't enter one from the same side (A for example) " +
                "twice in a row.");
        if (!audioSource1.isPlaying)
        {
            audioSource1.clip = clip;
            audioSource1.Play();
        }
        else
        {
            audioSource2.clip = clip;
            audioSource2.Play();
        }
    }

    public void SelectAmbianceClip(AudioClip clip)
    {
        chosenClip = clip;
    }

    public void FadeVolumeInViaAtoB(TriggerBoxAudioBlender triggerBoxAudioBlender)
    {
        if (audioSource1.clip == chosenClip)
            audioSource1.volume = triggerBoxAudioBlender.ProgressThroughTrigger / 100f;
        else if (audioSource2.clip == chosenClip)
            audioSource2.volume = triggerBoxAudioBlender.ProgressThroughTrigger / 100f;
        else
            Debug.LogWarning("Warning! The ambiance clip '" + chosenClip.name + "' that you're trying to set the volume for is not " +
                "currently being played and therefore it's volume could not be changed.");
    }

    public void FadeVolumeOutViaAtoB(TriggerBoxAudioBlender triggerBoxAudioBlender)
    {
        if (audioSource1.clip == chosenClip)
            audioSource1.volume = (triggerBoxAudioBlender.ProgressThroughTrigger - 100f) * -1f / 100f;
        else if (audioSource2.clip == chosenClip)
            audioSource2.volume = (triggerBoxAudioBlender.ProgressThroughTrigger - 100f) * -1f / 100f;
        else
            Debug.LogWarning("Warning! The ambiance clip '" + chosenClip.name + "' that you're trying to set the volume for is not " +
                "currently being played and therefore its volume could not be changed.");
    }

    public void StopAmbianceClip(AudioClip clip)
    {
        if (audioSource1.clip == clip)
        {
            audioSource1.Stop();
            audioSource1.clip = null;
        }
        else if (audioSource2.clip == clip)
        {
            audioSource2.Stop();
            audioSource2.clip = null;
        }
        else
        {
            Debug.LogWarning("Warning! The ambiance clip '" + clip.name + "' that you've told to stop is not currently being played and therefore could not be told stop.");
        }
    }
}