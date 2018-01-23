using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VictoryManager : MonoBehaviour
{
    public UIVictoryPanelManager uiVictoryPanel;

    public void EnableVictoryPanel(bool p_enable = true)
    {
        uiVictoryPanel.EnableVictoryPanel();
    }
}