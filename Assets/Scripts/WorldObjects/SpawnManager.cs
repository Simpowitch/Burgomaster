using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Player player = null;
    [SerializeField] Camera c = null;
    [SerializeField] GraphicRaycaster m_Raycaster = null;
    [SerializeField] Transform spawnParent = null;
    public ThemeSelector themeSelector;

    public LayerMask mask;
    Spawner spawner;
    [SerializeField] float keyRotationSpeed = 30;

    string latestTooltip;
    [SerializeField] MouseTooltip mouseTooltip = null;

    Blueprint selectedBlueprint;

    [Header("Key commands")]
    public KeyCode cancelSpawn = KeyCode.C;
    public KeyCode rotateClockwise = KeyCode.Period;
    public KeyCode rotateCounterClockwise = KeyCode.Comma;
    public KeyCode rotate90 = KeyCode.R;
    public KeyCode nextTheme = KeyCode.B;
    public KeyCode previousTheme = KeyCode.V;

    private void OnDisable()
    {
        DestroyPreviewObject();
    }

    private void Update()
    {
        if (Input.GetKeyDown(cancelSpawn))
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
        if (Input.GetKey(rotateCounterClockwise))
        {
            spawner.transform.rotation *= Quaternion.Euler(Vector3.forward * keyRotationSpeed * Time.deltaTime);
        }
        //Rotate Right
        if (Input.GetKey(rotateClockwise))
        {
            spawner.transform.rotation *= Quaternion.Euler(Vector3.forward * -keyRotationSpeed * Time.deltaTime);
        }
        //Rotate 90 degrees right
        if (Input.GetKeyDown(rotate90))
            spawner.transform.rotation *= Quaternion.Euler(Vector3.forward * -90f);

        //Change to next available theme
        if (Input.GetKeyDown(nextTheme))
            spawner.ChangeTheme(true);
        //Change to previous available theme
        if (Input.GetKeyDown(previousTheme))
            spawner.ChangeTheme(false);
    }

    private void SpawnPreviewObject()
    {
        if (spawner)
            DestroyPreviewObject();

        spawner = Instantiate(selectedBlueprint.prefab, spawnParent);

        //Theme setup
        WorldObject objectToSpawn = spawner.GetComponent<WorldObject>();
        if (objectToSpawn.HasThemes)
        {
            Sprite[] themeSprites = objectToSpawn.themes;
            Action[] themeChoiceActions = new Action[themeSprites.Length];

            for (int i = 0; i < themeChoiceActions.Length; i++)
            {
                int index = i;
                themeChoiceActions[i] = () => spawner.ChangeTheme(index);
            }
            themeSelector.Setup(themeSprites, themeChoiceActions);
        }
        else
        {
            themeSelector.SetActive(false);
        }
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
        themeSelector.SetActive(false);
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

    public void SetThemeIndex(int index)
    {
        if (spawner)
            spawner.ChangeTheme(index);
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
