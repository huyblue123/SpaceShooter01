using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip energyClip;
    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource bossAudioSource;
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private AudioClip dashClip;
    public void PlayShootSound()
    {
        audioSource.PlayOneShot(shootClip);
    }
    public void PlayEnergyPickUp()
    {
        audioSource.PlayOneShot(energyClip);
    }
    public void PlayDefaultAudio()
    {
        bossAudioSource.Stop();
        defaultAudioSource.Play();
    }
    public void PlayBossAudio()
    {
        defaultAudioSource.Stop();
        bossAudioSource.Play();
    }
    public void StopPlayAudio()
    {
        bossAudioSource.Stop();
        defaultAudioSource.Stop();
    }
    public void PlayEnemyHit()
    {
        audioSource.PlayOneShot(enemyHitClip);
    }
    public void PlayDash()
    {
        audioSource.PlayOneShot(dashClip);
    }
}
