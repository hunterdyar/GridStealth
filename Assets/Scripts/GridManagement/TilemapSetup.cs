using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
public class TilemapSetup : MonoBehaviour
{
    public TilemapManager manager;
    public Tilemap tilemap;
    [Button("Assign Tilemap")]
    public void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        manager.tilemap = tilemap;
        manager.InitiateNodes();
    }
}
