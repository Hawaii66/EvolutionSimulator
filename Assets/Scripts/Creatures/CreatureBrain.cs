using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class CreatureBrain : MonoBehaviour
{
    public bool drawStats;

    [Header("Mutations")]
    public CreatureStats creatureMutations;
    public CreatureManager.CreatureType type;

    [Header("Animal Stats")]
    [Range(0, 100)]
    public float drinkUrge;
    [Range(0, 100)]
    public float eatUrge;
    public bool isIdle;
    public string currentAction;

    [Header("Creature Info")]
    public MapSpot targetSpot;
    public bool isFollowingPath;

    CreatureHelpFunctions helpFunctions;
    Unit MoveManager;

    [Header("Reproduce Stats")]
    public int eatTimes;
    public int drinkTimes;
    public int eatMax;
    public int drinkMax;

    [Header("Death Numbers")]
    public float drinkUrgeMax;
    public float eatUrgeMax;

    int internalTick;

    void Start()
    {
        helpFunctions = FindObjectOfType<CreatureHelpFunctions>();
        MoveManager = GetComponent<Unit>();

        creatureMutations = new CreatureStats(2.5f, 50, 6);

        MoveManager.speed = creatureMutations.speed;

        eatUrge = 0;
        drinkUrge = 0;
        isIdle = true;
        currentAction = "";
        isFollowingPath = false;
    }

    void Update()
    {
        internalTick += 1;

        eatUrge += Time.deltaTime;
        drinkUrge += Time.deltaTime;

        if(eatUrge >= eatUrgeMax || drinkUrge >= drinkUrgeMax)
        {
            Die();
            return;
        }


        bool hasFound = false;
        if (currentAction == "")
        {
            if (eatUrge >= drinkUrge)
            {
                hasFound = FindFood();
            }
            else
            {
                hasFound = FindWater();
            }
        } else
        {
            if(currentAction == "Eat")
            {
                if (internalTick >= 10)
                {
                    internalTick = 0;
                    hasFound = FindFood();
                } else
                {
                    hasFound = true;
                }
            } 
            if(currentAction == "Drink")
            {
                //hasFound = FindWater();
                hasFound = true;
            }
        }

        if (hasFound)
        {
            if (targetSpot != null)
            {
                if (targetSpot.type == ThingsOnMap.TypeOnMap.apple)
                {
                    if (targetSpot.transform != null)
                    {
                        isIdle = false;
                        currentAction = "Eat";
                        MoveManager.hasPath = false;
                        if (!isFollowingPath)
                        {
                            isFollowingPath = true;
                            MoveManager.SetDestination(targetSpot.transform.position);
                        }
                    }
                    else
                    {
                        currentAction = "";
                        isIdle = true;
                        targetSpot = null;
                    }
                }

                if(targetSpot != null && targetSpot.type == ThingsOnMap.TypeOnMap.water)
                {
                    isIdle = false;
                    currentAction = "Drink";
                    MoveManager.hasPath = false;
                    if (!isFollowingPath)
                    {
                        isFollowingPath = true;
                        MoveManager.SetDestination(new Vector3(targetSpot.gridPos.x, transform.position.y, targetSpot.gridPos.y));
                    }
                }
            }
        } else
        {
            Vector2Int randomPos = FindRandomPositionInCircle();
            if(randomPos == new Vector2Int(-1, -1))
            {
                return;
            }
            currentAction = "";
            MoveManager.SetDestination(new Vector3(randomPos.x, transform.position.y, randomPos.y));
        }
    }

    void Die()
    {
        if(eatUrge >= eatUrgeMax)
        {
            FindObjectOfType<CauseOfDeath>().AddToList(CauseOfDeath.Causes.hunger);
        } else if(drinkUrge >= drinkUrgeMax)
        {
            FindObjectOfType<CauseOfDeath>().AddToList(CauseOfDeath.Causes.thirst);
        }

        Destroy(gameObject);
    }

    Vector2Int FindRandomPositionInCircle()
    {
        return helpFunctions.FindRandomPosition(transform, creatureMutations.senseRadious);
    }
    /*
    void OnDrawGizmos()
    {
        if (!drawStats) { return; }
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, creatureMutations.senseRadious);
    }
    */
    bool FindWater()
    {
        Vector2Int closestWaterPos = helpFunctions.FindClosestWater(transform, creatureMutations.senseRadious);
        if (closestWaterPos == new Vector2Int(-1, -1))
        {
            return false;
        }

        targetSpot = new MapSpot(closestWaterPos, null, ThingsOnMap.TypeOnMap.water);
        return true;
    }

    bool FindFood()
    {
        Vector2Int closestFoodPos = helpFunctions.FindClosestFood(transform, creatureMutations.senseRadious);
        if(closestFoodPos == new Vector2Int(-1, -1))
        {
            return false;
        }

        targetSpot = helpFunctions.FindMapSpot(closestFoodPos);
        return true;
    }

    public void ErrorCreatingPath()
    {
        isFollowingPath = false;
        targetSpot = null;
        isIdle = true;
        currentAction = "";
    }

    public void ReachedGoal(bool success)
    {
        isFollowingPath = false;

        if (currentAction == "Eat")
        {
            if (helpFunctions.isOnMap(targetSpot.gridPos, ThingsOnMap.TypeOnMap.apple))
            {
                Debug.Log("Has eat");
                eatTimes += 1;
                eatUrge = 0;
                targetSpot.transform.GetComponent<FoodBrain>().Die();
            }
        }
        if (currentAction == "Drink")
        {
            Debug.Log("Has Drink");
            drinkTimes += 1;
            drinkUrge = 0;
        }

        Reporocude();

        targetSpot = null;
        isIdle = true;
        currentAction = "";
    }

    void Reporocude()
    {
        if(eatTimes >= eatMax && drinkTimes >= drinkMax)
        {
            Debug.Log("Spawning New");

            CreatureManager man = FindObjectOfType<CreatureManager>();
            man.CreateCreature(man.FindCreatureWithType(type), gameObject);

            eatTimes = 0;
            drinkTimes = 0;
        }
    }
}

[System.Serializable]
public class CreatureStats
{
    public float speed;
    public float senseRadious;
    public float mutationRate;

    public CreatureStats(float _speed, float _sense, float _muteRate)
    {
        speed = _speed;
        senseRadious = _sense;
        mutationRate = _muteRate;

        Mutate();
    }
    
    public void Mutate()
    {
        speed += Random.Range(-mutationRate, mutationRate);
        senseRadious += Random.Range(-mutationRate, mutationRate);

        speed = Mathf.Max(0.1f, speed);
        senseRadious = Mathf.Max(0.1f, senseRadious);
    }
}