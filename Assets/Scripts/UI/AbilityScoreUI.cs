using UnityEngine;
using TMPro;

public class AbilityScoreUI : MonoBehaviour
{
    public TextMeshProUGUI autoritarian, cunning, diplomatic, liberal, nature, religious;

    public void UpdateUI(int[] abilityScores)
    {
        autoritarian.text = abilityScores[(int)Player.AbilityScore.Authoritarian].ToString();
        cunning.text = abilityScores[(int)Player.AbilityScore.Cunning].ToString();
        diplomatic.text = abilityScores[(int)Player.AbilityScore.Diplomatic].ToString();
        liberal.text = abilityScores[(int)Player.AbilityScore.Liberal].ToString();
        nature.text = abilityScores[(int)Player.AbilityScore.Nature].ToString();
        religious.text = abilityScores[(int)Player.AbilityScore.Religious].ToString();
    }
}
