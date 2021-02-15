using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] Player player = null;
    [SerializeField] Camera c = null;
    [SerializeField] GraphicRaycaster m_Raycaster = null;
    [SerializeField] Transform structureParent = null;
    public LayerMask mask;
    ConstructionPlacer previewObject;
    [SerializeField] float keyRotationSpeed = 30;

    string latestTooltip;
    [SerializeField] MouseTooltip mouseTooltip = null;

    Project selectedProject;

    private void OnDisable()
    {
        DestroyPreviewObject();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            mouseTooltip.Hide();
            DestroyPreviewObject();
            this.enabled = false;
        }

        if (IsPointerOverUI() || previewObject == null)
            return;

        RaycastHit2D hit = Physics2D.Raycast(c.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, mask);
        if (hit.collider != null)
        {
            previewObject.transform.position = hit.point;
        }

        string newTooltip = previewObject.TooltipExplanation;

        if (latestTooltip != newTooltip)
        {
            latestTooltip = newTooltip;
            if (latestTooltip != "")
            {
                Debug.Log($"Setup tooltip: {latestTooltip}");
                mouseTooltip.SetUp(MouseTooltip.ColorText.Default, latestTooltip);
            }
            else
            {
                Debug.Log($"Hiding tooltip");
                mouseTooltip.Hide();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
            ConfirmPlacement();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            DestroyPreviewObject();

        //Rotate Left
        if (Input.GetKey(KeyCode.Comma))
        {
            previewObject.transform.rotation *= Quaternion.Euler(Vector3.forward * keyRotationSpeed * Time.deltaTime);
        }
        //Rotate Right
        if (Input.GetKey(KeyCode.Period))
        {
            previewObject.transform.rotation *= Quaternion.Euler(Vector3.forward * -keyRotationSpeed * Time.deltaTime);
        }
        //Rotate 90 degrees right
        if (Input.GetKeyDown(KeyCode.R))
            previewObject.transform.rotation *= Quaternion.Euler(Vector3.forward * -90f);
    }

    private void SpawnPreviewObject()
    {
        if (previewObject)
            DestroyPreviewObject();

        previewObject = Instantiate(selectedProject.blueprint, structureParent);
        previewObject.Player = player;
    }

    public void CancelBuild()
    {
        DestroyPreviewObject();
    }

    private void ConfirmPlacement()
    {
        if (previewObject.ConfirmSpawn())
        {
            mouseTooltip.Hide();
            player.PayForProject(selectedProject);
            if (!player.CanPayForProject(selectedProject))
            {
                CancelBuild();
                this.enabled = false;
            }
        }
    }

    private void DestroyPreviewObject()
    {
        if (!previewObject)
            return;
        Destroy(previewObject.gameObject);
        mouseTooltip.Hide();
    }

    public void SetCurrentProject(Project project)
    {
        this.enabled = true;
        //Change object if already one clicked
            if (previewObject)
        {
            DestroyPreviewObject();
        }
        SpawnPreviewObject();
    }

    private bool IsPointerOverUI()
    {
        //Set up the new Pointer Event
        PointerEventData m_PointerEventData = new PointerEventData(EventSystem.current);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        ////For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        //foreach (RaycastResult result in results)
        //{
        //    Debug.Log("Hit " + result.gameObject.name);
        //}

        //Return true if results are more than 0
        return results.Count > 0;
    }
}
