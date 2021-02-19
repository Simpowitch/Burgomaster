using UnityEngine;
using TMPro;

public class AbilityScoreBlockUI : MonoBehaviour
{
    public TextMeshProUGUI autoritarian, cunning, diplomatic, liberal, nature, religious;

    public void UpdateUI(AbilityScoreBlock scoreBlock)
    {
        autoritarian.text = scoreBlock.authoritarian.ToString();
        cunning.text = scoreBlock.cunning.ToString();
        diplomatic.text = scoreBlock.diplomatic.ToString();
        liberal.text = scoreBlock.liberal.ToString();
        nature.text = scoreBlock.nature.ToString();
        religious.text = scoreBlock.religious.ToString();
    }

    public void UpdateUI(int[] abilityScores)
    {
        autoritarian.text = abilityScores[(int)AbilityScore.Authoritarian].ToString();
        cunning.text = abilityScores[(int)AbilityScore.Cunning].ToString();
        diplomatic.text = abilityScores[(int)AbilityScore.Diplomatic].ToString();
        liberal.text = abilityScores[(int)AbilityScore.Liberal].ToString();
        nature.text = abilityScores[(int)AbilityScore.Nature].ToString();
        religious.text = abilityScores[(int)AbilityScore.Religious].ToString();
    }
}
