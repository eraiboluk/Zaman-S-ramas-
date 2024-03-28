using UnityEngine;

public class SoundManagement : MonoBehaviour
{
    public AudioClip[] sounds; // Array containing sound files
    private AudioSource audioSource; // AudioSource component

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        // Start playing the sound
        Play();
    }

    void Play()
    {
        // Create a random order for the sounds in the list
        ShuffleSounds();

        // Play the first sound
        audioSource.PlayOneShot(sounds[0]);

        // Invoke the method at the end of the sound
        Invoke("RepeatAtEndOfSound", sounds[0].length);
    }

    void RepeatAtEndOfSound()
    {
        // Index of the next sound
        int index = Random.Range(0, sounds.Length);

        // Play the selected sound
        audioSource.PlayOneShot(sounds[index]);

        // Repeat the invocation
        Invoke("RepeatAtEndOfSound", sounds[index].length);
    }

    void ShuffleSounds()
    {
        // Shuffle the sounds using the Fisher-Yates shuffle algorithm
        for (int i = 0; i < sounds.Length; i++)
        {
            AudioClip temp = sounds[i];
            int randomIndex = Random.Range(i, sounds.Length);
            sounds[i] = sounds[randomIndex];
            sounds[randomIndex] = temp;
        }
    }
}
