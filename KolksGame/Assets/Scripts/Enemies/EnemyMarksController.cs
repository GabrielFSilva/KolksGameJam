using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyMarksController : MonoBehaviour
{
    public SpriteRenderer   yawnSucceedIcon;
    public Animator         yawnSucceedController;
    public SpriteRenderer   yawnFailIcon;
    public Animator         yawnFailController;

    public SpriteRenderer   actionDeniedIcon;
    public Animator         actionDeniedController;
    public CustomAnimEvent  actionDeniedAnimEvent;
    // Use this for initialization
    void Start ()
    {
        actionDeniedAnimEvent.OnAnimationCalled += ActionDeniedAnimEnded;
	}
	
    public void PlayActionDenied()
    {
        actionDeniedIcon.gameObject.SetActive(true);
        actionDeniedController.SetTrigger("Play");
    }
    public void ActionDeniedAnimEnded()
    {
        actionDeniedIcon.gameObject.SetActive(false);
    }
    public void ShowYawnIcon(bool p_yawned, float p_delay, int p_sortingOrder)
    {
        StartCoroutine(IEnableYawnIcon(p_yawned, p_delay, p_sortingOrder));
    }
    IEnumerator IEnableYawnIcon(bool p_yawned, float p_delay, int p_sortingOrder)
    {
        yield return new WaitForSeconds(p_delay);
        yawnSucceedIcon.gameObject.SetActive(p_yawned);
        yawnFailIcon.gameObject.SetActive(!p_yawned);
        yawnSucceedIcon.sortingOrder = p_sortingOrder + 10;
        yawnFailIcon.sortingOrder = p_sortingOrder + 10;
        //if (p_yawned)
        //	SoundManager.GetInstance ().PlayEndOfLevelSFX ();
    }
}
