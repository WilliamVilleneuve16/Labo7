using UnityEngine;

public static class Finder
{
    private static GameObject gameControllerObject;
    private static GameObject globalAudioSourceObject;
    private static AudioSource globalAudioSource;
    private static EventChannels eventChannels;

    public static AudioSource GlobalAudioSource
    {
        get
        {
            if (globalAudioSource == null)
                globalAudioSource = GlobalAudioSourceObject.AddComponent<AudioSource>();
            return globalAudioSource;
        }
    }

    public static EventChannels EventChannels
    {
        get
        {
            if (eventChannels == null)
                eventChannels = GameControllerObject.GetComponent<EventChannels>();
            return eventChannels;
        }
    }

    private static GameObject GameControllerObject
    {
        get
        {
            if (gameControllerObject == null)
                gameControllerObject = GameObject.FindWithTag("GameController");
            return gameControllerObject;
        }
    }

    private static GameObject GlobalAudioSourceObject
    {
        get
        {
            if (globalAudioSourceObject == null)
                globalAudioSourceObject = new GameObject { name = "GlobalAudioSource" };
            return globalAudioSourceObject;
        }
    }
}