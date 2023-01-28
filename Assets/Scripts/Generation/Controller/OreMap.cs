using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreMap : MonoBehaviour
{
    // Components
    // Each oreSetting represents a type of ore that can spawn
    [SerializeField] private List<OreSettings> oreSettings = new List<OreSettings>();

    [Header("Preview")]
    [SerializeField] private bool togglePreview = false;

    private Map map;

    private void Start() {
        map = Map.instance;
    }
}
