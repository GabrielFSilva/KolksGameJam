using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIEndLevelManager : MonoBehaviour
{
    public static float FadeDuration { get; private set;}
    public static float SuccessMessageDuration { get; private set; }
    [SerializeField]
    private float fadeDuration;
    [SerializeField]
    private float successMessageDuration;

    //NEW END LEVEL
    public Image fadeImage;

    public GameObject buttonsPanel;
    public GameObject endLevelPanel;

    public Image successImage;

    public Animator nextButtonAnimator;
    public Button nextButton;
    public Animator replayButtonAnimator;
    public Button replayButton;
    public RectTransform replayButtonCenterRef;

    public Animation fadeAnimation;

    public SoundManager soundManager;
    public int activeStarsCount = 0;

    void Start()
    {
        SuccessMessageDuration = successMessageDuration;
        FadeDuration = fadeDuration;
        soundManager = SoundManager.GetInstance();
    }
    public void EnableEndLevelPanel(bool p_enable)
    {
        endLevelPanel.gameObject.SetActive(p_enable);
        buttonsPanel.gameObject.SetActive(!p_enable);
    }
    public void PlayVictorySequence()
    {
        StartCoroutine(EndLevelSequence());
    }
   
    IEnumerator EndLevelSequence()
    {
        successImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        fadeAnimation.playAutomatically = true;
        fadeAnimation.Play();
        yield break;
    }
}


