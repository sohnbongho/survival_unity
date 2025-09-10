using UnityEngine;

public class Building_Mng : MonoBehaviour
{
    Camera cam;
    [SerializeField] private float rayDistance = 100.0f;
    [SerializeField] private LayerMask layer;

    [HideInInspector] public GameObject BuildingObject;

    public void SetBuild(Building_Scriptable m_Data)
    {
        BuildingObject = Instantiate(m_Data.obj.gameObject);
    }

    private void Start()
    {
        cam = Camera.main;

    }

    private void Update()
    {
        if (BuildingObject == null)
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance, layer))
        {
            BuildingObject.transform.position = hitInfo.point;
        }

    }
}
