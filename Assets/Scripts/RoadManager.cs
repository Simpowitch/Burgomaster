using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadManager : MonoBehaviour
{
    public GraphicRaycaster[] graphicRaycasters = null;

    public PathCreator roadCreatorBP;

    public Transform roadParent;
    public GameObject buttonPanel;
    public Toggle editModeToggle;

    Road selectedRoad;
    public Road SelectedRoad
    {
        get => selectedRoad;
        set
        {
            if (selectedRoad)
                selectedRoad.SetSelected(false);

            selectedRoad = value;

            if (selectedRoad)
                selectedRoad.SetSelected(true);
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
        if (Utility.GetObjectUnderMouse2D(out Road roadUnderMouse, selectedRoad))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) //Left Click
            {
                SelectedRoad = roadUnderMouse;
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
                if (SelectedRoad)
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
                        if (selectedRoad.CanRemoveAnchorPoint)
                        {
                            Debug.Log("Removing Segment");
                            selectedRoad.DeleteLastSegment();

                            //SelectedRoadCreator.path.DeleteLastSegment();
                            //selectedAnchor.Delete();
                            selectedAnchor = selectedRoad.LastAnchorPoint;
                            buttonPanel.transform.position = selectedRoad.LastAnchorPoint.transform.position;
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
        SelectedRoad = Instantiate(roadCreatorBP, roadParent).GetComponentInChildren<Road>();
        selectedRoad.Setup(pos);
        selectedAnchor = SelectedRoad.LastAnchorPoint;
        SetState(State.AddAndRemoveSegments);

        ////Create point 0
        //PathAnchorPoint newAnchor = Instantiate(roadAnchorPointBP, SelectedRoadCreator.transform);
        //newAnchor.Setup(SelectedRoadCreator, SelectedRoadCreator.path, 0, pos);

        //selectedRoad.pathAnchorPoints.Add(newAnchor);

        ////Create point 1
        //newAnchor = Instantiate(roadAnchorPointBP, SelectedRoadCreator.transform);
        //newAnchor.Setup(SelectedRoadCreator, SelectedRoadCreator.path, SelectedRoadCreator.path.LastAnchor, pos);

        //selectedRoad.pathAnchorPoints.Add(newAnchor);

        //selectedAnchor = newAnchor;
    }

    private void CreateNewNodeAndSegment(Vector2 pos)
    {
        SelectedRoad.AddSegment(pos);
        selectedAnchor = SelectedRoad.LastAnchorPoint;

        //SelectedRoadCreator.path.AddSegment(pos);

        //PathAnchorPoint newAnchor = Instantiate(roadAnchorPointBP, SelectedRoadCreator.transform);
        //newAnchor.Setup(SelectedRoadCreator, SelectedRoadCreator.path, SelectedRoadCreator.path.LastAnchor, pos);
        //selectedAnchor = newAnchor;

        buttonPanel.transform.position = pos;
    }

    public void Confirm()
    {
        SelectedRoad = null;
        SetState(State.CreateNewRoad);
    }

    public void Cancel()
    {
        SelectedRoad.DestroyRoad();
        //Destroy(SelectedRoadCreator.gameObject);
        SelectedRoad = null;
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
