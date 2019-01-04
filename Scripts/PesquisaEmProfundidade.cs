using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesquisaEmProfundidade : SearchAlgorithm
{
	private Stack<SearchState> openStack; // a visitar

	protected override void Begin()
	{
		startNode = GridMap.instance.NodeFromWorldPoint(startPos);
		targetNode = GridMap.instance.NodeFromWorldPoint(targetPos);

		SearchState start = new SearchState(startNode, 0);
		openStack = new Stack<SearchState>();
		openStack.Push(start);
	}

	protected override void Step()
	{

		if (openStack.Count > 0)
		{
			SearchState currentState = openStack.Pop();
			VisitNode(currentState);

			if (currentState.node == targetNode) // teste objetivo
			{
				//devolve no
				solution = currentState;
				finished = true;
				running = false;
				foundPath = true;
			}
			else
			{
				foreach (Node suc in GetNodeSucessors(currentState.node))
				{
					if (!nodesVisited.Contains (suc)) { // ????
						// se o sucessor nao estiver na lista de visitados
						// insere pilha
						SearchState new_node = new SearchState (suc, suc.gCost + currentState.g, currentState);
						openStack.Push (new_node);
					}
				}
				// for energy
				if ((ulong)openStack.Count > maxListSize)
				{
					maxListSize = (ulong)openStack.Count;
				}
			}
		}
		else
		{
			// pilha vazia
			finished = true;
			running = false;
			foundPath = true;
		}

	}
}
