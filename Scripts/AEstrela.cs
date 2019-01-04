using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEstrela : SearchAlgorithm {

	private PriorityQueue p_queue = new PriorityQueue();

	public int NrHeuristica=0;

	protected override void Begin () {
		startNode = GridMap.instance.NodeFromWorldPoint (startPos);
		targetNode = GridMap.instance.NodeFromWorldPoint (targetPos);

		SearchState start = new SearchState (startNode, 0);

		p_queue.Add(start,(int)start.f); // 0 ???
	}

	protected override void Step () {

		if (p_queue.Count > 0)
		{
			// retira da lista de nos o primeiro e insere-o na lista de visitados
			SearchState currentState = p_queue.PopFirst();
			//Debug.Log ("Profundidade do no atual: " + currentState.depth);
			VisitNode (currentState);

			if (currentState.node == targetNode) {
				solution = currentState;
				finished = true;
				running = false;
				foundPath = true;

			} else {
				foreach (Node suc in GetNodeSucessors(currentState.node)) {
					if (!nodesVisited.Contains (suc)) {

						//define-se o g(n) que é o custo de transitar 
						float Gn = currentState.g + suc.gCost;

						// define-se a heuristica a utilizar
						float Hn;


						if (NrHeuristica == 0)
							Hn = GetHeuristic_Euclidean (suc);
						else
							Hn = GetHeuristic_Manhattan (suc);
						
						SearchState new_node = new SearchState(suc, Gn, Hn, currentState);

						int Fn = (int)(Gn + Hn); //f(n) = g(n) + h(n) 
						p_queue.Add(new_node, Fn); // lista ordenada

					}
				}
				// for energy
				if ((ulong) p_queue.Count > maxListSize) {
					maxListSize = (ulong) p_queue.Count;
				} 
			}
		}
		else
		{
			// devolve falha
			finished = true;
			running = false;
			foundPath = true;
		}

	}
}
