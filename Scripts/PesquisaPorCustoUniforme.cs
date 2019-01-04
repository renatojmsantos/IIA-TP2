using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesquisaPorCustoUniforme : SearchAlgorithm{
	
	private PriorityQueue openPriorityQueue = new PriorityQueue(); // a visitar

	protected override void Begin()
	{
		startNode = GridMap.instance.NodeFromWorldPoint(startPos);
		targetNode = GridMap.instance.NodeFromWorldPoint(targetPos);

		SearchState start = new SearchState(startNode, 0);

		openPriorityQueue.Add(start,0);

	}

	protected override void Step()
	{

		if (openPriorityQueue.Count > 0)
		{
			SearchState currentState = openPriorityQueue.PopFirst();

			VisitNode(currentState); 

			if (currentState.node == targetNode)
			{
				// teste objetivo
				solution = currentState;
				finished = true;
				running = false;
				foundPath = true;
			}

			else
			{
				foreach (Node suc in GetNodeSucessors(currentState.node))
				{
					SearchState new_node = new SearchState(suc, suc.gCost + currentState.g, currentState);

					//insercao é feita de forma ordenada, colocando os elementos de menor custo à cabeça da fila
					openPriorityQueue.Add(new_node,(int)new_node.g); //a priority queue ordena pelo 2ºelemento que está associado ao custo

				}
				if ((ulong)openPriorityQueue.Count > maxListSize)
				{
					maxListSize = (ulong)openPriorityQueue.Count;
				}
			}
		}
		else //fila vazia
		{
			//falha
			finished = true;
			running = false;
			foundPath = true;
		}

	}
}
