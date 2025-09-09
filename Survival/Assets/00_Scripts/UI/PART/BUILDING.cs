using System.Collections.Generic;
using UnityEngine;

public class BUILDING : UIPART
{
    public GameObject BuildingPanel;
    public Transform Content;
    List<Building_Panel> building_list = new List<Building_Panel>();

    // Awake -> OnEnable -> Start
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        SetBuilding();
    }

    private void Init()
    {
        var buildings = Asset_Mng.Buildings;
        for (int i = 0; i < buildings.Length; i++)
        {
            var go = Instantiate(BuildingPanel, Content);
            var panel = go.GetComponent<Building_Panel>();
            panel.Init(buildings[i]);
            building_list.Add(panel);
        }
    }

    private void SetBuilding()
    {
        for (int i = 0; i < building_list.Count; i++)
        {
            building_list[i].SetData();
        }
    }
}
