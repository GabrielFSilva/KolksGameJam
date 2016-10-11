using UnityEngine;
using UnityEngine.UI;

public class UITopBarReferences : MonoBehaviour
{
    public Text     starCountLabel;
    public Text     coinCountLabel;
    public Button   optionsButton;

    public void UpdateStarCountLabel(int p_starCount)
    {
        starCountLabel.text = p_starCount.ToString();
    }
    public void UpdateCoinCountLabel(int p_coinCount)
    {
        coinCountLabel.text = p_coinCount.ToString();
    }
}
