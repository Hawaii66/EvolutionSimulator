using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauseOfDeath : MonoBehaviour
{
    public enum Causes { hunger, thirst, eaten }

    public int Hunger, thirst, eaten;
    public List<Causes> CausesOfDeath;

    public void AddToList(Causes cause)
    {
        CausesOfDeath.Add(cause);

        if(cause == Causes.eaten)
        {
            eaten += 1;
        }
        if(cause == Causes.hunger)
        {
            Hunger += 1;
        }
        if(cause == Causes.thirst)
        {
            thirst += 1;
        }
    }
}