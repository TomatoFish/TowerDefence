using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Pool : MonoBehaviour
{
    private List<GameObject> objects;

    public void Initialize(int count, GameObject obj)
    {
        objects = new List<GameObject>();
        foreach (Transform tr in transform)
            Destroy(tr.gameObject);

        for (int i = 0; i < count; i++)
            AddToPool(obj, transform);
    }

    public void ReturnToPool(GameObject obj)
    {
        if (!objects.Contains(obj))
            objects.Add(obj);
        obj.SetActive(false);
        obj.transform.parent = transform;
    }

    public GameObject GetFromPool(Transform targetContainer)
    {
        var obj = objects.FirstOrDefault(o => !o.activeSelf);
        if (obj != null)
            obj.transform.parent = targetContainer;
        else
            obj = AddToPool(objects.First(), transform);
        obj.SetActive(true);
        return obj;
    }

    private GameObject AddToPool(GameObject obj, Transform targetContainer)
    {
        var newObj = Instantiate(obj, transform);
        objects.Add(newObj);
        newObj.SetActive(false);
        return newObj;
    }
}
