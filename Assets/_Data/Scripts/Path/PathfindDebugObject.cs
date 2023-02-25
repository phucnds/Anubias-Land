using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindDebugObject : GridDebugObject
{

    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;


    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }

    protected override void Update()
    {
        base.Update();

        gCostText.text = pathNode.GetGCost() + "";
        hCostText.text = pathNode.GetHCost() + "";
        fCostText.text = pathNode.GetFCost() + "";

       // isWalkableSpriteRenderer.color = pathNode.IsWalkable() ? Color.green : Color.red;
    }

}
