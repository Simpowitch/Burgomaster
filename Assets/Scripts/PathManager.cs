using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathManager : MonoBehaviour
{
    public GraphicRaycaster[] graphicRaycasters = null;

    public PathCreator pathCreatorBP;

    public Transform creatorParent;
    public GameObject buttonPanel;
    public Toggle editModeToggle;

    PathCreator selectedCreator;
    public PathCreator SelectedCreator
    {
        get => selectedCreator;
        set
        {
            if (selectedCreator)
                selectedCreator.SetSelected(false);

            selectedCreator = value;

            if (selectedCreator)
                selectedCreator.SetSelected(true);
        }
    }
    PathAnchorPoint selectedAnchor;

    public enum State { CreateNewRoad, AddAndRemoveSegments, EditSegments }
    private State state;

    private void OnDisable()
    {
        buttonPanel.SetActive(false);
    }

    private void Update()
    {
        if (Utility.IsPointerOverUI(graphicRaycasters))
            return;

        Vector2 mousePos = Utility.GetMouseWorldPosition();

        //Check if clicked on different road
        if (Utility.GetObjectUnderMouse2D(out PathVisualizer pathUnderMouse, SelectedCreator ? SelectedCreator.pathVisualizer : null))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) //Left Click
            {
                SelectedCreator = pathUnderMouse.transform.parent.GetComponent<PathCreator>();
                SetState(State.EditSegments);
                selectedAnchor = null;
                editModeToggle.SetIsOnWithoutNotify(true);
                return;
            }
        }

        switch (state)
        {
            case State.CreateNewRoad:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Vector2 point = mousePos;
                    if (Utility.GetObjectUnderMouse2D(out PathAnchorPoint anchor))
                    {
                        point = anchor.transform.position;
                    }
                    CreateRoad(point);
                }
                break;
            case State.AddAndRemoveSegments:
                if (SelectedCreator)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0)) //Left Click
                    {
                        CreateNewNodeAndSegment(mousePos);
                    }
                    if (Input.GetKey(KeyCode.Mouse0)) //Held Down
                    {
                        Vector2 point = mousePos;
                        if (Utility.GetObjectUnderMouse2D(out PathAnchorPoint anchor, selectedAnchor))
                        {
                            point = anchor.transform.position;
                        }

                        selectedAnchor.MovePoint(point);
                        buttonPanel.transform.position = mousePos;
                    }
                    else if (Input.GetKeyDown(KeyCode.Mouse1)) //Right Click
                    {
                        if (SelectedCreator.CanRemoveAnchorPoint)
                        {
                            Debug.Log("Removing Segment");
                            SelectedCreator.DeleteLastSegment();

                            //SelectedRoadCreator.path.DeleteLastSegment();
                            //selectedAnchor.Delete();
                            selectedAnchor = SelectedCreator.LastAnchorPoint;
                            buttonPanel.transform.position = SelectedCreator.LastAnchorPoint.transform.position;
                            //SelectedRoadCreator.UpdatePath();
                        }
                        else
                        {
                            Debug.Log("Can't remove segment - too few segments");
                        }
                    }
                }
                break;
            case State.EditSegments:
                if (Input.GetKeyDown(KeyCode.Mouse0)) //Left Click
                {
                    if (Utility.GetObjectUnderMouse2D(out PathAnchorPoint anchor))
                    {
                        this.selectedAnchor = anchor;
                    }
                    else
                    {
                        this.selectedAnchor = null;
                    }
                }
                if (Input.GetKey(KeyCode.Mouse0)) //Held Down
                {
                    if (selectedAnchor)
                    {
                        selectedAnchor.MovePoint(mousePos);

                        buttonPanel.transform.position = mousePos;
                        //SelectedRoadCreator.UpdatePath();
                    }
                }
                break;
        }
    }

    public void SetState(int newState) => SetState((State)newState);
    public void SetState(State newState)
    {
        state = newState;
        switch (newState)
        {
            case State.CreateNewRoad:
                buttonPanel.SetActive(false);
                break;
            case State.AddAndRemoveSegments:
                buttonPanel.SetActive(true);
                break;
            case State.EditSegments:
                buttonPanel.SetActive(true);
                break;
        }
    }


    private void CreateRoad(Vector2 pos)
    {
        SelectedCreator = Instantiate(pathCreatorBP, creatorParent);
        SelectedCreator.Setup(pos);
        selectedAnchor = SelectedCreator.LastAnchorPoint;
        SetState(State.AddAndRemoveSegments);
    }

    private void CreateNewNodeAndSegment(Vector2 pos)
    {
        SelectedCreator.AddSegment(pos);
        selectedAnchor = SelectedCreator.LastAnchorPoint;

        buttonPanel.transform.position = pos;
    }

    public void Confirm()
    {
        SelectedCreator = null;
        SetState(State.CreateNewRoad);
    }

    public void Cancel()
    {
        SelectedCreator.Destroy();
        SelectedCreator = null;
        selectedAnchor = null;
        SetState(State.CreateNewRoad);
    }

    public void ToggleEditMode(bool value)
    {
        if (value)
            SetState(State.EditSegments);
        else
            SetState(State.AddAndRemoveSegments);
    }
}
