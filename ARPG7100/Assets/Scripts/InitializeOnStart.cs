using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeOnStart : MonoBehaviour
{
    void Awake()
    {
        PlayerStats.LoadBaseStats();
        ItemGeneration.LoadAffixes();
        ItemGeneration.LoadBaseItems();
    }
}
