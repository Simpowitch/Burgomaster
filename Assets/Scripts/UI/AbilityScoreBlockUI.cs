using UnityEngine;
using TMPro;

public class AbilityScoreBlockUI : MonoBehaviour
{
    public TextMeshProUGUI autoritarian, cunning, diplomatic, liberal, nature, religious;

    public void UpdateUI(AbilityScoreBlock scoreBlock)
    {
        autoritarian.text = scoreBlock.authority.ToString();
        cunning.text = scoreBlock.cunning.ToString();
        diplomatic.text = scoreBlock.diplomacy.ToString();
        liberal.text = scoreBlock.knowledge.ToString();
        nature.text = scoreBlock.nature.ToString();
        religious.text = scoreBlock.piety.ToString();
    }

    public void UpdateUI(int[] abilityScores)
    {
        autoritarian.text = abilityScores[(int)AbilityScore.Authority].ToString();
        cunning.text = abilityScores[(int)AbilityScore.Cunning].ToString();
        diplomatic.text = abilityScores[(int)AbilityScore.Diplomacy].ToString();
        liberal.text = abilityScores[(int)AbilityScore.Knowledge].ToString();
        nature.text = abilityScores[(int)AbilityScore.Nature].ToString();
        religious.text = abilityScores[(int)AbilityScore.Piety].ToString();
    }
}
