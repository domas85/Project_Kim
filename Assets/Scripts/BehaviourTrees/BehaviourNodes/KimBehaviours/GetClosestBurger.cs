using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GetClosestBurger : BehaviourNode
{
    public GetClosestBurger(List<BehaviourNode> someChildren) : base(someChildren)
    {

    }

    Burger[] allburgers;
    public override ReturnState Evaluate()
    {
        Kim kim = myBlackBoard.data["Kim"] as Kim;
        allburgers = myBlackBoard.data["Burgers"] as Burger[];
  
        List<List<Node>> burgerPath = new List<List<Node>>();

        List<List<Node>> SoretedBurgerList = new List<List<Node>>();
        if (allburgers != null && allburgers.Length >= 2)
        {
            for (int i = 0; i < allburgers.Length; i++)
            {
                kim.FindShortestPathToTarget(kim.transform.position, allburgers[i].transform.position);
                burgerPath.Add(kim.grid.path);
            }
        }
        if (allburgers.Length >= 2)
        {
            SoretedBurgerList = burgerPath.OrderBy(go => go.Count).ToList();
        }


        if (SoretedBurgerList[0].Count < SoretedBurgerList[1].Count && SoretedBurgerList.Count > 1)
        {
            kim.grid.path = SoretedBurgerList[0];
            allburgers.ToList().RemoveAt(0);
            myBlackBoard.data["Burgers"] = allburgers;
            SoretedBurgerList.RemoveAt(0);
            return ReturnState.Success;
        }
        else if (SoretedBurgerList.Count == 1)
        {
            kim.grid.path = SoretedBurgerList[0];
            allburgers.ToList().RemoveAt(0);
            return ReturnState.Success;
        }

        return ReturnState.Failure;
    }

}
