using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIVictoryPanelManager : MonoBehaviour
{
    public Canvas victoryCanvas;

    public void EnableVictoryPanel(bool p_enable = true)
    {
        victoryCanvas.gameObject.SetActive(p_enable);
    }
}

