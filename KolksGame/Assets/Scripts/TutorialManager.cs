using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public static bool      onTutorial;
    
    public List<GameObject>         tutorialPrefabs;
    public GameObject               tutorialCanvas;
    public GameObject               tutorial;
    public TutorialPanelReferences  tutorialReference;

    [SerializeField]
    private float gotItButtonDelay;
    [SerializeField]
    private string tutorialName;

    public enum TutorialType
    {
        LEVEL_1,
        LEVEL_2,
        LEVEL_2B,
        LEVEL_3,
        LEVEL_3B,
        LEVEL_5,
        LEVEL_5B,
        LEVEL_9
    }
    public void SceneLoaded (int p_levelIndex, int p_coinCount)
    {
        if (p_levelIndex == 1)
            LoadTutorial(TutorialType.LEVEL_1);
        else if (p_levelIndex == 2)
            LoadTutorial(TutorialType.LEVEL_2);
        else if (p_levelIndex == 3)
            LoadTutorial(TutorialType.LEVEL_3);
        else if (p_levelIndex == 5 && p_coinCount == 2)
            LoadTutorial(TutorialType.LEVEL_5);
        else if (p_levelIndex == 9)
            LoadTutorial(TutorialType.LEVEL_9);
    }
    public void LevelEnded(int p_levelIndex, int p_coinCount)
    {
        if (p_levelIndex == 5 && p_coinCount == 1)
            LoadTutorial(TutorialType.LEVEL_5B);
    }

    public void PlayerMoveToTargetPosition(int p_levelIndex)
    {
        if (p_levelIndex == 2)
            LoadTutorial(TutorialType.LEVEL_2B);
        else if (p_levelIndex == 3)
            LoadTutorial(TutorialType.LEVEL_3B);
    }
	
	public void LoadTutorial(TutorialType p_tutorialType)
    {
        int __tutorialIndex = (int)p_tutorialType;
        tutorial = Instantiate(tutorialPrefabs[__tutorialIndex]);
        tutorial.transform.SetParent(tutorialCanvas.transform);
        tutorial.transform.localScale = Vector3.one;
        tutorial.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        tutorialReference = tutorial.GetComponent<TutorialPanelReferences>();

        if (tutorialReference.gotItButton != null)
        {
            tutorialReference.gotItButton.gameObject.SetActive(false);
            tutorialReference.gotItButton.onClick.AddListener(CloseTutorial);
            StartCoroutine(EnableGotItButton());
        }

        tutorialName = tutorialPrefabs[__tutorialIndex].name.Remove(0, 9);
        onTutorial = true;
    }
    public void OnSwipeLeft()
    {
        if (onTutorial && tutorialName == "Level2")
            CloseTutorial();
    }
    public void OnTap(Orientation p_ori)
    {
        if (!onTutorial)
            return;
        if (tutorialName == "Level2b" && (p_ori == Orientation.RIGHT || p_ori == Orientation.DOWN))
            CloseTutorial();
        else if (tutorialName == "Level3b" && (p_ori == Orientation.LEFT || p_ori == Orientation.UP))
            CloseTutorial();
    }
    public void CloseTutorial()
    {
        Destroy(tutorial.gameObject);
        onTutorial = false;
    }

    IEnumerator EnableGotItButton()
    {
        yield return new WaitForSeconds(gotItButtonDelay);
        tutorialReference.gotItButton.gameObject.SetActive(true);
        tutorialReference.gotItButton.GetComponent<Animator>().SetTrigger("Play");
    }
}
