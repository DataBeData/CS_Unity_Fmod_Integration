using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}// Singleton

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play one-shot (fire and forget)
    public void PlayOneShot(EventReference eventReference, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(eventReference, worldPosition);
    }

    // Create and return an EventInstance for more control
    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventReference);
        return instance;
    }

     // Play a looping or controllable event and return the instance
    public EventInstance PlayEvent(EventReference sound)
    {
        EventInstance instance = RuntimeManager.CreateInstance(sound);
        instance.start();
        return instance;
    }

    // Play a one-shot sound attached to a GameObject (follows the GameObject while playing)
    public void PlayOneShotAttached(EventReference sound, GameObject attachTo)
    {
        EventInstance instance = RuntimeManager.CreateInstance(sound);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(attachTo.transform.position));
        instance.start();

        // Automatically release the instance when finished playing
        RuntimeManager.AttachInstanceToGameObject(instance, attachTo.transform, attachTo.GetComponent<Rigidbody>());
        instance.release();
    }

    // Stop an event instance with fade out
    public void StopEvent(EventInstance instance)
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }

    // Set a parameter
    public void SetParameter(EventInstance instance, string paramName, float value)
    {
        instance.setParameterByName(paramName, value);
    }
}
