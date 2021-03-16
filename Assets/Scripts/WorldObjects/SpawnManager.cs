using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Player player = null;
    [SerializeField] Camera c = null;
    [SerializeField] GraphicRaycaster m_Raycaster = null;
    [SerializeField] Transform spawnParent = null;
    public LayerMask mask;
    Spawner spawner;
    [SerializeField] float keyRotationSpeed = 30;

    string latestTooltip;
    [SerializeField] MouseTooltip mouseTooltip = null;

    Blueprint selectedBlueprint;

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

        if (spawner == null)
            return;

        if (IsPointerOverUI())
        {
            spawner.SetActive(false);
            return;
        }

        spawner.SetActive(true);


        RaycastHit2D hit = Physics2D.Raycast(c.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, mask);
        if (hit.collider != null)
        {
            spawner.transform.position = hit.point;
        }

        string newTooltip = spawner.TooltipExplanation;

        if (latestTooltip != newTooltip)
        {
            latestTooltip = newTooltip;
            if (latestTooltip != "" && latestTooltip != null)
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
            spawner.transform.rotation *= Quaternion.Euler(Vector3.forward * keyRotationSpeed * Time.deltaTime);
        }
        //Rotate Right
        if (Input.GetKey(KeyCode.Period))
        {
            spawner.transform.rotation *= Quaternion.Euler(Vector3.forward * -keyRotationSpeed * Time.deltaTime);
        }
        //Rotate 90 degrees right
        if (Input.GetKeyDown(KeyCode.R))
            spawner.transform.rotation *= Quaternion.Euler(Vector3.forward * -90f);

        //Change to next available theme
        if (Input.GetKeyDown(KeyCode.B))
            spawner.ChangeTheme(true);
        //Change to previous available theme
        if (Input.GetKeyDown(KeyCode.V))
            spawner.ChangeTheme(false);
    }

    private void SpawnPreviewObject()
    {
        if (spawner)
            DestroyPreviewObject();

        spawner = Instantiate(selectedBlueprint.prefab, spawnParent);
    }

    public void CancelBuild()
    {
        DestroyPreviewObject();
    }

    private void ConfirmPlacement()
    {
        if (spawner.CanSpawn)
        {
            spawner.Spawn(selectedBlueprint, spawnParent, player);

            mouseTooltip.Hide();
            player.PayForProject(selectedBlueprint);
            if (!player.CanDoProject(selectedBlueprint))
            {
                CancelBuild();
                this.enabled = false;
            }
        }
    }

    private void DestroyPreviewObject()
    {
        if (!spawner)
            return;
        Destroy(spawner.gameObject);
        mouseTooltip.Hide();
    }

    public void SetCurrentBlueprint(Blueprint blueprint)
    {
        this.enabled = true;
        selectedBlueprint = blueprint;
        //Change object if already one clicked
        if (spawner)
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
