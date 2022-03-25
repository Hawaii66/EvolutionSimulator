using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBrain : MonoBehaviour
{
    public ThingsOnMap thingsOnMap;
    public Vector2Int gridPos;
    [Header("Mutation")]
    public FoodStats mutation;

    public float nextTime;

    void Start()
    {
        nextTime = transform.parent.GetComponent<FoodSpawner>().elapsedTime + mutation.spreadTime;
    }

    public void Die()
    {
        thingsOnMap.DeleteFromMap(gridPos.x, gridPos.y);
        Destroy(gameObject);
    }
}
