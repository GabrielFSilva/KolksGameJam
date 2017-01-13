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
    public void EnableEndLevelButtons(float p_value)
    {
        if (p_value >= 0.5f)
        {
            successImage.gameObject.SetActive(true);
            StartCoroutine(EndLevelFade());
        }
    }
   
    IEnumerator EndLevelFade()
    {
        yield return new WaitForSeconds(successMessageDuration);
        float __alpha = -Time.deltaTime;
        while(true)
        {
            __alpha += (Time.deltaTime / fadeDuration);
            fadeImage.color = new Color(1f, 1f, 1f, __alpha);
            if (__alpha >= 1f)
                break;
            yield return null;
        }
    }
}


