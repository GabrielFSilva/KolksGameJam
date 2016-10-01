using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static bool      onTutorial;

    public List<GameObject> tutorialPrefabs;
    public GameObject       tutorialCanvas;
    public GameObject       tutorial;

    public void SceneLoaded (int p_levelIndex, int p_coinCount)
    {
        if (p_levelIndex == 1)
            LoadTutorial(0);
        else if (p_levelIndex == 2)
            LoadTutorial(1);
        else if (p_levelIndex == 5 && p_coinCount == 2)
            LoadTutorial(2);
        else if (p_levelIndex == 9)
            LoadTutorial(4);
    }
    public void LevelEnded(int p_levelIndex, int p_coinCount)
    {
        if (p_levelIndex == 5 && p_coinCount == 1)
            LoadTutorial(3);
    }
	
	public void LoadTutorial(int p_tutorialIndex)
    {
        tutorial = Instantiate(tutorialPrefabs[p_tutorialIndex]);
        tutorial.transform.SetParent(tutorialCanvas.transform);
        tutorial.transform.localScale = Vector3.one;
        tutorial.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        TutorialPanelReferences __ref = tutorial.GetComponent<TutorialPanelReferences>();

        if (__ref.gotItButton != null)
            __ref.gotItButton.onClick.AddListener(CloseTutorial);

        onTutorial = true;
    }
    public void OnLeftSwipeDone(int p_tutorialIndex)
    {
        if (onTutorial && p_tutorialIndex == 2)
            CloseTutorial();
    }
    public void CloseTutorial()
    {
        Destroy(tutorial.gameObject);
        onTutorial = false;
    }
}
