using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int instancesCount = 10;

    private void Awake()
    {
        for (var i = 0; i < instancesCount; i++)
        {
            Create(isActive: false);
        }
    }

    public GameObject Get()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var instance = transform.GetChild(i).gameObject;
            if (instance.activeSelf) continue;
            instance.SetActive(true);
            return instance;
        }

        return Create(isActive: true);
    }

    public GameObject Place(Vector3 position, Quaternion rotation)
    {
        var instance = Get();
        instance.transform.position = position;
        instance.transform.rotation = rotation;
        return instance;
    }

    public GameObject Place(Transform transform)
    {
        return Place(transform.position, transform.rotation);
    }

    public T Get<T>() where T : Component
    {
        var instance = Get().GetComponent<T>();
        Debug.Assert(instance != null, $"Object from pool {name} doesn't have a component of type {typeof(T)}.");
        return instance;
    }

    public T Place<T>(Vector3 position, Quaternion rotation) where T : Component
    {
        var instance = Get<T>();
        instance.transform.position = position;
        instance.transform.rotation = rotation;
        return instance;
    }

    public T Place<T>(Transform transform) where T : Component
    {
        return Place<T>(transform.position, transform.rotation);
    }

    public void Release(GameObject instance)
    {
        instance.SetActive(false);
        instance.transform.parent = transform; // In case the object was moved.
    }

    private GameObject Create(bool isActive)
    {
        var instance = Instantiate(prefab, transform);
        instance.SetActive(isActive);
        return instance;
    }
}