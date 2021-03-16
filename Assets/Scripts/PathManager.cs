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

        //Check if clicked on new path
        if (Input.GetKeyDown(KeyCode.Mouse0)) //Left Click
        {
            if (Utility.GetObjectUnderMouse2D(out PathVisualizer pathUnderMouse, SelectedCreator ? SelectedCreator.pathVisualizer : null))
            {
                SelectedCreator = pathUnderMouse.transform.parent.GetComponent<PathCreator>();
                SetState(State.EditSegments);
                selectedAnchor = null;
                PathAnchorPoint.Selection = selectedAnchor;
                editModeToggle.SetIsOnWithoutNotify(true);
                return;
            }
            if (Utility.GetObjectUnderMouse2D(out PathAnchorPoint anchorUnderMouse, selectedAnchor))
            {
                SelectedCreator = anchorUnderMouse.transform.parent.GetComponent<PathCreator>();
                SetState(State.EditSegments);
                selectedAnchor = anchorUnderMouse;
                PathAnchorPoint.Selection = selectedAnchor;
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
                            PathAnchorPoint.Selection = selectedAnchor;
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

                            selectedAnchor = SelectedCreator.LastAnchorPoint;
                            PathAnchorPoint.Selection = selectedAnchor;

                            buttonPanel.transform.position = SelectedCreator.LastAnchorPoint.transform.position;
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
                            PathAnchorPoint.Selection = selectedAnchor;
                    }
                    else
                    {
                        this.selectedAnchor = null;
                            PathAnchorPoint.Selection = selectedAnchor;
                    }
                }
                if (Input.GetKey(KeyCode.Mouse0)) //Held Down
                {
                    if (selectedAnchor)
                    {
                        Vector2 point = mousePos;
                        if (Utility.GetObjectUnderMouse2D(out PathAnchorPoint anchor, selectedAnchor))
                        {
                            point = anchor.transform.position;
                        }

                        selectedAnchor.MovePoint(point);

                        buttonPanel.transform.position = mousePos;
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
        PathAnchorPoint.Selection = selectedAnchor;

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
        selectedAnchor = null;
        PathAnchorPoint.Selection = selectedAnchor;
        SetState(State.CreateNewRoad);
    }

    public void Cancel()
    {
        SelectedCreator.Destroy();
        SelectedCreator = null;
        selectedAnchor = null;
        PathAnchorPoint.Selection = selectedAnchor;
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
