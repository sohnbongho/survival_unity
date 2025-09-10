using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BUILDING : UIPART
{
    public GameObject BuildingPanel;
    public Transform Content;
    List<Building_Panel> building_list = new List<Building_Panel>();

    public GameObject ItemClickTap;

    Animator animator;

    // Awake -> OnEnable -> Start
    private void Awake()
    {
        Init();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SetBuilding();
    }

    public void AnimationChange(string temp)
    {
        animator.SetTrigger(temp);
    }

    private void Init()
    {
        var buildings = Asset_Mng.Buildings;
        for (int i = 0; i < buildings.Length; i++)
        {
            var go = Instantiate(BuildingPanel, Content);
            var panel = go.GetComponent<Building_Panel>();
            panel.Init(buildings[i], this);
            building_list.Add(panel);
        }
    }

    private void SetBuilding()
    {
        StartCoroutine(GetOpenCoroutine());
    }

    IEnumerator GetOpenCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < building_list.Count; i++)
        {
            building_list[i].SetData();
            yield return new WaitForSeconds(0.02f);
        }

    }

    private void OnDisable()
    {
        for (int i = 0; i < building_list.Count; i++)
        {
            building_list[i].gameObject.SetActive(false);
        }
    }

    public void SetItemClickAnimation(Building_Panel panel)
    {
        ItemClickTap.gameObject.SetActive(true);
        ItemClickTap.transform.SetParent(panel.transform);
        ItemClickTap.transform.localPosition = Vector2.zero;
    }
}
