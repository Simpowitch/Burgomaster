using UnityEngine;

public class AdvisorManager : MonoBehaviour
{
    public static Advisor SelectedAdvisor { get; private set; }
    int selectedIndex = -1;

    [SerializeField] Advisor[] advisors = null;
    [SerializeField] GameObject[] advisorSelectionFrames = null;

    private void OnEnable()
    {
        TurnManager.OnUpdateAdvisors += TurnUpdate;
    }

    private void OnDisable()
    {
        TurnManager.OnUpdateAdvisors -= TurnUpdate;
    }

    public void TurnUpdate(object sender, TurnManager.OnTurnEventArgs e)
    {
        foreach (var advisor in advisors)
        {
            advisor.TurnUpdate();
        }
    }


    public void SelectAdvisor(int index)
    {
        if (index < advisors.Length)
        {
            Advisor newSelected = advisors[index];

            if (newSelected == SelectedAdvisor) //Same as previously selected
            {
                DeselectSelected();
            }
            else
            {
                DeselectSelected();
                //Select new
                SelectedAdvisor = advisors[index];
                selectedIndex = index;
                //Visually show the selected portrait
                advisorSelectionFrames[index].SetActive(true);
            }
        }
        else
            Debug.LogError("Out of range selection of advisor");
    }

    private void DeselectSelected()
    {
        //Visually hide the old selected portrait
        if (selectedIndex >= 0)
            advisorSelectionFrames[selectedIndex].SetActive(false);
        SelectedAdvisor = null;
        selectedIndex = -1;
    }
}
