using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] float minSpawnTimer;
    [SerializeField] float maxSpawnTimer;
    [SerializeField] float probabilidadDeNoSpawnear;
    [SerializeField] List<Obstacle> obstacles = new List<Obstacle>();
    [SerializeField] List<int> probabilidadesObstaculo = new List<int>();

    List<int> probabilidades = new List<int>();
    void Awake()
    {
        for (int i = 0; i < probabilidadesObstaculo.Count; i++)
        {
            for (int n = 0; n < probabilidadesObstaculo[i]; n++)
            {
                probabilidades.Add(i);
            }
        }
        for(int i = 0; i < probabilidadDeNoSpawnear; i++)
        {
            probabilidades.Add(-1);
        }
        randomizeList(probabilidades);
    }

    private void Start()
    {
        StartCoroutine(obstacleSpawner());
    }

    List<int> randomizeList(List<int> _list)
    {
        List<int> list = _list;
        for (int i = 0; i < list.Count; i++)
        {
            int aux = list[i];
            int r = Random.Range(0, list.Count);
            int randomSelected = list[r];
            list[i] = randomSelected;
            list[r] = aux;
        }
        return list;
    }

    IEnumerator obstacleSpawner()
    {
        while (true)
        {
            float r = Random.Range(minSpawnTimer, maxSpawnTimer);
            yield return new WaitForSeconds(r);
            spawnObstacle();
        }
    }

    void spawnObstacle()
    {
        int r = Random.Range(0, probabilidades.Count);
        int index = probabilidades[r];
        if(index == -1)
        {
            return;
        }
        Obstacle obstacle = obstacles[index];
        Instantiate(obstacle, transform.position, Quaternion.identity);
    }
}
