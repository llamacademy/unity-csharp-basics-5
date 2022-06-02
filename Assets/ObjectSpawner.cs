using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private float AutoDestroyTime = 10f;
    [SerializeField]
    private bool SpawnInFixedUpdate = false;
    [SerializeField]
    private AutoDestroyText Prefab;
    private int TotalTextsDied = 0;

    private List<AutoDestroyText> AvailableObjects = new List<AutoDestroyText>();

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            AvailableObjects.Add(Instantiate(
                Prefab,
                new Vector3(
                    Random.Range(0, Screen.width),
                    Random.Range(0, Screen.height),
                    0
                ),
                Quaternion.identity,
                transform
            ));
            AvailableObjects[i].gameObject.SetActive(false);
        }
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(1);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 60, 200, 20), $"Texts Died: {TotalTextsDied}");
    }

    private void SpawnObject()
    {
        if (AvailableObjects.Count == 0)
        {
            AvailableObjects.Add(Instantiate(
                Prefab,
                new Vector3(
                    Random.Range(0, Screen.width),
                    Random.Range(0, Screen.height),
                    0
                ),
                Quaternion.identity,
                transform
            ));
        }

        AutoDestroyText text = AvailableObjects[0];

        text.OnDeath += HandleTextDeath;
        text.AutoDestroyTime = AutoDestroyTime;
        
        text.UnityEventOnDeath.AddListener(HandleTextDeath);

        text.gameObject.SetActive(true);
        AvailableObjects.RemoveAt(0);
    }

    public void HandleTextDeath(AutoDestroyText Instance, Vector2 Position)
    {
        TotalTextsDied++;
        AvailableObjects.Add(Instance);
        Instance.OnDeath -= HandleTextDeath;
    }
}
