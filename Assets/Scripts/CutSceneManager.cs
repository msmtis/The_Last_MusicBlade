using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutSceneManager : MonoBehaviour
{
    public bool ignoreVideo = false;
    public RawImage videoCanvas;
    public RawImage blackscreen;
    public AudioSource MetalSoundTrack;
    public VideoPlayer video;
    public GeneralPlayerMovement playerMovement;


    private int fadeDuration = 1;

    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;                // Rende visibile il cursore
        Cursor.lockState = CursorLockMode.None;
        if (!ignoreVideo)
        {
            video.loopPointReached += onVideoEnd;

            playerMovement.cutsceneEnded = false;


            video.Play();
            MetalSoundTrack.Play();
            StartCoroutine(FadeOutRoutine(blackscreen));

        }
    }

    public void SkipVideo()
    {
        video.time = video.length - 0.1f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void onVideoEnd(VideoPlayer vp) {

        Cursor.visible = false;               
        Cursor.lockState = CursorLockMode.Locked;

        //StartCoroutine(FadeInRoutine(blackscreen));
        StartCoroutine(FadeOutRoutine(blackscreen));
        videoCanvas.gameObject.SetActive(false);
        playerMovement.cutsceneEnded = true;
        StartCoroutine(WaitToStart());
        





    }


    IEnumerator FadeOutRoutine(RawImage sprite)
    {
        float elapsedTime = 0f;
        Color originalColor = sprite.color;
        yield return new WaitForSeconds(0.5f);
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Assicura che l'opacità sia esattamente 0 alla fine
        sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    IEnumerator FadeInRoutine(RawImage sprite)
    {
        float elapsedTime = 0f;
        Color originalColor = sprite.color;
        
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Assicura che l'opacità sia esattamente 0 alla fine
        sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(2f);
        FightManager.Instance.cutSceneEnded = true;
        UIManager.Instance.StartFight();
    }
   




}
