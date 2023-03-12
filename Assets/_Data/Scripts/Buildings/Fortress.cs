using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortress : Building
{
    [SerializeField] private Transform spawnerPoint;
    [SerializeField] private int amount = 20;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject vfxSpawn;

    [SerializeField] private float spawner_min_range = 8f;
    [SerializeField] private float spawner_max_range = 20f;
    [SerializeField] private float spawner_interval = 10f;

    private float state_timer = 0f;
    private Vector3 start_pos = Vector3.zero;

    protected override void Start()
    {
        base.Start();

        start_pos = Fortress.GetNearest(Vector3.zero).transform.position;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 p = Vector3.zero;
            yield return new WaitForSeconds(spawner_interval);
            SpawnVfx(out p);
            yield return new WaitForSeconds(3);
            SpawnEnemy(p);
        }
    }

    private void SpawnEnemy(Vector3 pos)
    {
        GameObject prefab = enemyPrefabs[1];
        GameObject enemy = Instantiate(prefab, pos, Quaternion.identity, spawnerPoint);
    }

    private void SpawnVfx(out Vector3 p)
    {
        Vector3 pos = NextPosition();
        GameObject vfx = Instantiate(vfxSpawn, pos, Quaternion.identity, spawnerPoint);
        Destroy(vfx, 3);
        p = pos;
    }

    private Vector3 NextPosition()
    {
        float range = Random.Range(spawner_min_range, spawner_max_range);
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 spos = start_pos;
        Vector3 pos = spos + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * range;
        return pos;
    }

    public static Fortress GetNearest(Vector3 pos, float range = 999f, Fortress ignore = null)
    {
        float min_dist = range;
        Fortress nearest = null;
        foreach (Fortress fortress in GameMgr.Instance.BuildingManager.GetListFortress())
        {
            float dist = (pos - fortress.transform.position).magnitude;
            if (dist < min_dist && !fortress.Interactable.IsInteractFull() && fortress != ignore)
            {
                min_dist = dist;
                nearest = fortress;
            }
        }
        return nearest;
    }
}
