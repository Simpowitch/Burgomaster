using System.Collections;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    public AudioSource audioPlayer;
    public float minPauseTime = 30f, maxPauseTime = 180f;
    public AudioClip[] scores;
    int scoreIndex;
    bool replayingAfterDuration = false;

    private void Start()
    {
        audioPlayer.Stop();

        scoreIndex = Random.Range(0, scores.Length);
        audioPlayer.clip = scores[scoreIndex];
        audioPlayer.Play();
        audioPlayer.loop = false;
    }

    private void Update()
    {
        if (!audioPlayer.isPlaying && !replayingAfterDuration)
            StartCoroutine(ReplayMusicAfterDuration(Random.Range(minPauseTime, maxPauseTime)));
        if (Input.GetKeyDown(KeyCode.M))
        {
            StopAllCoroutines();
            PlayNextSong();
            replayingAfterDuration = false;
        }
    }

    IEnumerator ReplayMusicAfterDuration(float duration)
    {
        replayingAfterDuration = true;
        Debug.Log($"Pausing music and replaying in: { duration} seconds");
        yield return new WaitForSeconds(duration);
        PlayNextSong();
        replayingAfterDuration = false;
    }

    void PlayNextSong()
    {
        scoreIndex++;
        scoreIndex = scoreIndex % scores.Length;
        Debug.Log("Score index:" + scoreIndex);
        audioPlayer.clip = scores[scoreIndex];
        audioPlayer.Play();
    }
}
