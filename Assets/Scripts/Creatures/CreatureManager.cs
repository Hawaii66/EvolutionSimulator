using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public enum CreatureType { none, ant, mouse}
    public Creature[] Creatures;

    public void CreateCreature(Creature creature, GameObject obj)
    {
        GameObject temp = Instantiate(creature.gameObject);
        temp.transform.parent = transform;
        temp.transform.position = obj.transform.position;

        if(creature.type == CreatureType.ant)
        {
            CreatureStats mutations = new CreatureStats(obj.GetComponent<CreatureBrain>().creatureMutations.speed, obj.GetComponent<CreatureBrain>().creatureMutations.senseRadious, obj.GetComponent<CreatureBrain>().creatureMutations.mutationRate);
            temp.GetComponent<CreatureBrain>().creatureMutations = mutations;
        }
    }

    public Creature FindCreatureWithType(CreatureType type)
    {
        for (int i = 0; i < Creatures.Length; i++)
        {
            if(Creatures[i].type == type)
            {
                return Creatures[i];
            }
        }

        return Creatures[0];
    }
}

[System.Serializable]
public struct Creature
{
    public string Name;
    public GameObject gameObject;
    public CreatureManager.CreatureType type;
}
