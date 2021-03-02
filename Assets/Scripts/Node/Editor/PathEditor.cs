using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path Path => creator.path;
    
    const float SEGMENTSELECTDISTANCETHRESHOLD = 0.1f;
    int selectedSegmentIndex = -1;

    private void OnEnable()
    {
        creator = (PathCreator)target;
        if (creator.path == null)
        {
            creator.CreatePath();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("Create New"))
        {
            Undo.RecordObject(creator, "Create new");
            creator.CreatePath();
        }

        bool isClosed = GUILayout.Toggle(Path.IsClosed, "Closed");
        if (isClosed != Path.IsClosed)
        {
            Undo.RecordObject(creator, "Toggle closed");
            Path.IsClosed = isClosed;
        }

        bool autoSetControlPoints = GUILayout.Toggle(Path.AutoSetControlPoints, "Auto Set Control Points");
        if (autoSetControlPoints != Path.AutoSetControlPoints)
        {
            Undo.RecordObject(creator, "Toggle auto set controls");
            Path.AutoSetControlPoints = autoSetControlPoints;
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    private void OnSceneGUI()
    {
        Input();
        Draw();

        if (creator.autoUpdate && Event.current.type == EventType.Repaint)
        {
            creator.UpdateRoad();
        }
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseMove)
        {
            float minDstToSegment = SEGMENTSELECTDISTANCETHRESHOLD;
            int newSelectedSegmentIndex = -1;

            //Find closest segment
            for (int i = 0; i < Path.NumberOfSegments; i++)
            {
                Vector2[] points = Path.GetPointsInSegment(i);
                float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);

                if (dst < minDstToSegment)
                {
                    minDstToSegment = dst;
                    newSelectedSegmentIndex = i;
                }
            }

            if (newSelectedSegmentIndex != selectedSegmentIndex)
            {
                selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }
        }

        //Leftclick + Shift
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            if (selectedSegmentIndex != -1)
            {
                Undo.RecordObject(creator, "Split Segment");
                Path.SplitSegment(mousePos, selectedSegmentIndex);
            }
            else if (!Path.IsClosed)
            {
                Undo.RecordObject(creator, "Add Segment");
                Path.AddSegment(mousePos);
            }
        }

        //Rightclick
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
        {
            float minDstToAnchor = creator.anchorDiameter * 0.5f;
            int closestAnchorIndex = -1;

            //Find closest point to the mouse
            for (int i = 0; i < Path.NumberOfPoints; i += 3)
            {
                float dst = Vector2.Distance(mousePos, Path[i]);

                if (dst < minDstToAnchor)
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }
            }

            if (closestAnchorIndex != -1)
            {
                Undo.RecordObject(creator, "Remove Segment");
                Path.DeleteSegment(closestAnchorIndex);
            }
        }

        HandleUtility.AddDefaultControl(0);
    }

    void Draw()
    {
        Handles.color = Color.black;
        for (int i = 0; i < Path.NumberOfSegments; i++)
        {
            Vector2[] points = Path.GetPointsInSegment(i);

            if (creator.displayControlPoints)
            {
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
            }

            Color segmentColor = i == selectedSegmentIndex && Event.current.shift ? creator.selectedSegmentColor : creator.segmentColor;

            Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentColor, null, 2f);
        }

        for (int i = 0; i < Path.NumberOfPoints; i++)
        {
            bool isAnchorPoint = i % 3 == 0;

            if (isAnchorPoint || creator.displayControlPoints)
            {
                Handles.color = isAnchorPoint ? creator.anchorColor : creator.controlColor;
                float handleSize = isAnchorPoint ? creator.anchorDiameter : creator.controlDiameter;

                Vector2 newPos = Handles.FreeMoveHandle(Path[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
                if (Path[i] != newPos)
                {
                    Undo.RecordObject(creator, "Move point");
                    Path.MovePoint(i, newPos);
                }
            }
        }
    }

    
}
