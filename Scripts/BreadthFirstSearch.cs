using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : SearchAlgorithm {

	private Queue<SearchState> openQueue;

	protected override void Begin () {
		startNode = GridMap.instance.NodeFromWorldPoint (startPos);
		targetNode = GridMap.instance.NodeFromWorldPoint (targetPos);

		// l_nos <- faz fila(Estado inicial(problema))
		SearchState start = new SearchState (startNode, 0);
		openQueue = new Queue<SearchState> ();
		openQueue.Enqueue(start);
	}
	
	protected override void Step () {
		
		if (openQueue.Count > 0)
		{
			SearchState currentState = openQueue.Dequeue();
			//Debug.Log ("Profundidade do no atual: " + currentState.depth);
			VisitNode (currentState);

			if (currentState.node == targetNode) {
				//devolve nó
				solution = currentState;
				finished = true;
				running = false;
				foundPath = true;
			} else {
				// insere fila (l_nos, Expansao(nó, Operadores(problema)))
				foreach (Node suc in GetNodeSucessors(currentState.node)) {
					SearchState new_node = new SearchState(suc, suc.gCost + currentState.g, currentState);
					openQueue.Enqueue (new_node);
				}
				// for energy
				if ((ulong) openQueue.Count > maxListSize) {
					maxListSize = (ulong) openQueue.Count;
				} 
			}
		}
		else // se fila vazia
		{
			// devolve falha
			finished = true;
			running = false;
			foundPath = true;
		}

	}
}
