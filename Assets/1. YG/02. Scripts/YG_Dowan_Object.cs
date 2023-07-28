using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YG_Dowan_Object : MonoBehaviour
{
    public GameObject audio_Sand1, audio_Sand2, audio_Sea;

    // 모래사장에 떨어지는 소리
    public void SandSound()
    {
        // 1/2 확률
        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            audio_Sand1.SetActive(true);
            StartCoroutine("TurnOFf", audio_Sand1);
        }
        else
        {
            audio_Sand2.SetActive(true);
            StartCoroutine("TurnOFf", audio_Sand2);
        }
    }

    public void SeaSound()
    {
        audio_Sea.SetActive(true);

        StartCoroutine("TurnOFf", audio_Sea);
    }

    public IEnumerator TurnOFf(GameObject audioFile)
    {
        yield return new WaitForSeconds(audioFile.GetComponent<AudioSource>().clip.length);
        audioFile.SetActive(false);
    }
}
