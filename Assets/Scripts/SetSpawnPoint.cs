using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SetSpawnPoint : MonoBehaviour
{
    public List<GameObject> floorObjects = new List<GameObject>();
    public List<GameObject> edgeFloors = new List<GameObject>();
    public MazeLoader mazeLoader;

    public GameObject player;
    public GameObject agent;

 

    public void FindEdgeFloor()
    {
        int floorObjectCount = mazeLoader.mazeRows - 1;
        Debug.Log("Works" + floorObjectCount);

        for (int i = 0; i < floorObjects.Count; i++)
        {
            if (floorObjects[i].name.Contains("Floor 0"))
            {
                Debug.Log("Works");
                edgeFloors.Add(floorObjects[i]);
            }
            else if (floorObjects[i].name.Contains("Floor " + floorObjectCount))
            {
                edgeFloors.Add(floorObjects[i]);
            }
            else if (floorObjects[i].name.EndsWith("0"))
            {
                edgeFloors.Add(floorObjects[i]);
            }
            else if (floorObjects[i].name.EndsWith("," + floorObjectCount))
            {
                edgeFloors.Add(floorObjects[i]);
            }
        }
    }

    public void SelectSpawnPoint()
    {
        int randomSpawnPoint;

        randomSpawnPoint = Random.Range(0, edgeFloors.Count);

        Instantiate(player, new Vector3 (edgeFloors[randomSpawnPoint].transform.position.x,edgeFloors[randomSpawnPoint].transform.position.y, edgeFloors[randomSpawnPoint].transform.position.z), Quaternion.identity);
    }

    public void FillSpawnPoint()
    {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor"))
            {
                floorObjects.Add(obj);
            } 
    }
}
