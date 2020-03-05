using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ScriptableObjectArchitecture;
[RequireComponent(typeof(GridElement))]
public class FOV : MonoBehaviour
{
    GridElement gridElement;
    Agent agent;
    GameFlowManager gfm;
    public int viewRange;
    public bool canSee = true;
    public List<GridElement> itemsISee;
    public List<Agent> agentsISee;
    public List<Vector2Int> placesISee;
    public FloatReference visibilityThreshold;
    [Button("Assign gridElement")]
    void Awake()
    {
        agentsISee = new List<Agent>();
        itemsISee = new List<GridElement>();
        placesISee = new List<Vector2Int>();
        agent = GetComponent<Agent>();
        gridElement = GetComponent<GridElement>();
        //the WithinSight behavior will run CheckSight before ... checking the sight...
        //gridElement.OnNewPositionAction += CheckSight;
    }
    void OnEnable()
    {
        //gfm.PostGridElementsUpdatedAction += CheckSight;
    }
    void OnDisable()
    {
        //gfm.PostGridElementsUpdatedAction -= CheckSight;
    }
    [Button("Illuminate")]
    public void CheckSight()
    {
        placesISee.Clear();
        agentsISee.Clear();
        itemsISee.Clear();
        if(!canSee){return;}
        Vector2Int[] c = GridUtility.Arc(gridElement.position,agent.facingDirection,viewRange,45);
        foreach(Vector2Int p in c)
        {
            TileNode tn = gridElement.tilemapManager.GetTileNode(p);
            if(tn!=null)
            {
                if(tn.brightness > visibilityThreshold.Value){
                    if(gridElement.tilemapManager.LineOfSight(tn.position,gridElement.position)){
                        foreach(GridElement item in tn.itemsHere)
                        {
                            placesISee.Add(tn.position);
                            if(!itemsISee.Contains(item))
                            {
                                itemsISee.Add(item);
                                if(item.GetComponent<Agent>() != null)
                                {
                                    agentsISee.Add(item.GetComponent<Agent>());
                                }        
                            }                        
                        }                  
                    }
                }
            }
        }
    }

}
