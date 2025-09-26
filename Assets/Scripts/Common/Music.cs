using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioClip introAudioClip;
    [SerializeField] private AudioClip loopingAudioClip;
    [SerializeField] private bool playOnStart = true;

    private AudioSource introSource;
    private AudioSource loopingSource;

    private void Awake()
    {
        introSource = gameObject.AddComponent<AudioSource>();
        introSource.clip = introAudioClip;
        loopingSource = gameObject.AddComponent<AudioSource>();
        loopingSource.clip = loopingAudioClip;
        loopingSource.loop = true;
    }

    private void Start()
    {
        if (playOnStart) Play();
    }

    public void Play()
    {
        if (introAudioClip == null)
        {
            loopingSource.Play();
        }
        else
        {
            var introTime = AudioSettings.dspTime;
            var loopTime = introTime + introAudioClip.length;

            introSource.PlayScheduled(introTime);
            loopingSource.PlayScheduled(loopTime);
        }
    }
    
    public void Stop()
    {
        introSource.Stop();
        loopingSource.Stop();
    }
}