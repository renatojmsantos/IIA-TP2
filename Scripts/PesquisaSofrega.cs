using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesquisaSofrega : SearchAlgorithm {

	// lista de nos que funciona como lista ordenada
	private PriorityQueue p_queue = new PriorityQueue(); // sitios possiveis a ir

	public int NrHeuristica;

	protected override void Begin () {
		startNode = GridMap.instance.NodeFromWorldPoint (startPos);
		targetNode = GridMap.instance.NodeFromWorldPoint (targetPos);

		//inicializa a arvore de procura com o estado (no) inicial do problema
		SearchState start = new SearchState (startNode, 0);

		p_queue.Add (start, (int)start.f);
	}

	protected override void Step () {

		if (p_queue.Count > 0)
		{
			// retira da lista de nós o primeiro nó e insere-o na lista de visitados
			SearchState currentState = p_queue.PopFirst(); 
			VisitNode (currentState);

			if (currentState.node == targetNode) { //se o no contem o objetivo
				//devolve a solucao correspondente
				solution = currentState;
				finished = true;
				running = false;
				foundPath = true;
			} else {
				foreach (Node suc in GetNodeSucessors(currentState.node)) {
					if(!nodesVisited.Contains(suc)){

						// limita-se a manter a fronteira da arvore de procura ordenada pelos valores de h(n),
						// sendo sempre escolhido o nó de valor mais baixo, isto é, aquele que está mais proximo da solucao
						int Hn;

						SearchState new_node = new SearchState(suc, suc.gCost + currentState.g, currentState);

						if (NrHeuristica == 0)
							Hn = GetHeuristic_Euclidean (new_node.node);
						else
							Hn = GetHeuristic_Manhattan (new_node.node);

						p_queue.Add(new_node, Hn); // ordenada
					}
				}
				// for energy
				if ((ulong) p_queue.Count > maxListSize) {
					maxListSize = (ulong) p_queue.Count;
				} 
			}
		}
		else // se nao ha candidatos para expandir
		{
			finished = true;
			running = false;
			foundPath = true; 
		}

	}
}