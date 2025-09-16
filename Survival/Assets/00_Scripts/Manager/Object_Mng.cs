using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Mng : MonoBehaviour
{
    private CullingGroup CullingGroup;
    private BoundingSphere[] BoundingSpheres;
    private List<GameObject> SetObjects = new List<GameObject>();

    public float CullingGroupRadius = 10.0f;
    public float SpawnAngleValue = 80.0f;
    public float CenterLimitValue = 5.0f;
    public int Maximum = 50;

    Object_Scriptable[] m_Datas;
    private void Start()
    {
        m_Datas = Resources.LoadAll<Object_Scriptable>("Object");

        GetSpawnObject();
    }
    private void MakeCulling()
    {
        BoundingSpheres = new BoundingSphere[SetObjects.Count];

        CullingGroup = new CullingGroup();
        CullingGroup.targetCamera = Camera.main;
        CullingGroup.SetBoundingSpheres(BoundingSpheres);
        CullingGroup.SetBoundingSphereCount(SetObjects.Count);

        for (int i = 0; i < SetObjects.Count; i++)
        {
            BoundingSpheres[i] = new BoundingSphere(SetObjects[i].transform.position, CullingGroupRadius);
        }
        CullingGroup.onStateChanged += OnStateChanged;
    }
    public void RemoveObject(GameObject obj)
    {
        int index = SetObjects.IndexOf(obj);
        SetObjects.RemoveAt(index);

        List<BoundingSphere> newSphere = new List<BoundingSphere>(BoundingSpheres);
        newSphere.RemoveAt(index);
        BoundingSpheres = newSphere.ToArray();

        CullingGroup.SetBoundingSpheres(BoundingSpheres);
        CullingGroup.SetBoundingSphereCount(BoundingSpheres.Length);
    }

    public void OnDestroy()
    {
        if (CullingGroup != null)
        {
            CullingGroup.Dispose();
            CullingGroup = null;
        }
    }

    private void OnStateChanged(CullingGroupEvent evt)
    {
        if (evt.isVisible)
        {
            SetObjects[evt.index].SetActive(true);
        }
        else
        {
            SetObjects[evt.index].SetActive(false);
        }
    }

    public void GetSpawnObject()
    {
        StartCoroutine(CreateObjectStart());
    }

    IEnumerator CreateObjectStart()
    {
        for (int i = 0; i < Maximum; i++)
        {
            Vector3 pos;
            MakePos(out pos);

            while (Vector3.Distance(pos, Vector3.zero) <= CenterLimitValue)
            {
                MakePos(out pos);
            }

            var objIndex = Random.Range(0, m_Datas.Length - 1);
            var getObject = m_Datas[objIndex].obj;

            Vector3 targetPos = new Vector3(pos.x, getObject.transform.position.y, pos.z);

            var go = Instantiate(getObject, targetPos,
                Quaternion.Euler(0.0f, Random.Range(0, 360),  // 회전 값을 주어서 다르게 표현
                0.0f));

            go.gameObject.SetActive(false);
            SetObjects.Add(go);

            yield return null;
        }

        MakeCulling();
    }

    private void MakePos(out Vector3 pos)
    {
        pos = Vector3.zero + Random.insideUnitSphere * SpawnAngleValue;
        pos.y = 0.0f;
    }
}
