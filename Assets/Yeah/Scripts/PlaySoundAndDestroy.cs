using UnityEngine;

public class PlaySoundAndDestroy : MonoBehaviour
{
    [SerializeField] private AudioSource bulletAudioSource;

    [SerializeField] private float timeToDestroy;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}