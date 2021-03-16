using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropSelectionOverview : MonoBehaviour
{
    public PropBlueprintPanel projectPanel_BP;
    public Transform viewportContent;

    private List<PropBlueprintPanel> allTab = new List<PropBlueprintPanel>();
    private List<PropBlueprintPanel> treeTab = new List<PropBlueprintPanel>();
    private List<PropBlueprintPanel> stoneTab = new List<PropBlueprintPanel>();
    private List<PropBlueprintPanel> miscTab = new List<PropBlueprintPanel>();
    public Toggle allToggle;

    public PropBlueprintPanel blueprintInspector;

    private Player player;

    public void UpdateProjectList(List<PropBlueprint> availableBlueprints, Player player)
    {
        this.player = player;
        foreach (var projectPanel in allTab)
        {
            Destroy(projectPanel.gameObject);
        }
        allTab.Clear();
        treeTab.Clear();
        stoneTab.Clear();
        miscTab.Clear();


        for (int i = 0; i < availableBlueprints.Count; i++)
        {
            PropBlueprintPanel newPanel = Instantiate(projectPanel_BP, viewportContent);

            switch (availableBlueprints[i].category)
            {
                case PropBlueprint.Category.Tree:
                    treeTab.Add(newPanel);
                    break;
                case PropBlueprint.Category.Stone:
                    stoneTab.Add(newPanel);
                    break;
                case PropBlueprint.Category.Misc:
                    miscTab.Add(newPanel);
                    break;
            }
            allTab.Add(newPanel);
            newPanel.Setup(availableBlueprints[i], this);
        }
        allToggle.Select();
    }

    public void ShowUI(bool show)
    {
        this.gameObject.SetActive(show);

        if (!show)
            blueprintInspector.SetActive(false);
    }

    public void ShowAll(bool state) => ShowList(allTab, state);

    public void ShowTrees(bool state) => ShowList(treeTab, state);

    public void ShowStones(bool state) => ShowList(stoneTab, state);

    public void ShowMisc(bool state) => ShowList(miscTab, state);

    private void ShowList(List<PropBlueprintPanel> list, bool show)
    {
        foreach (var item in list)
        {
            item.gameObject.SetActive(show);
        }
    }

    public void SelectBlueprint(PropBlueprint selected)
    {
        blueprintInspector.SetActive(true);
        blueprintInspector.Setup(selected, this);
        player.SelectBlueprint(selected);
    }
}
