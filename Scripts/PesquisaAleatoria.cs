using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesquisaAleatoria : SearchAlgorithm {

	private List<SearchState> openList; //lista com sitios possiveis onde se pode mexer
	public int seed;

	protected override void Begin () {
		startNode = GridMap.instance.NodeFromWorldPoint (startPos);
		targetNode = GridMap.instance.NodeFromWorldPoint (targetPos);

		//inicializa a arvore de procura com o estado (no) inicial do problema
		SearchState start = new SearchState (startNode, 0); 
		openList = new List<SearchState> ();
		openList.Add(start);

		Random.InitState (seed);
	}

	protected override void Step () {

		if (openList.Count > 0)
		{
			int random = Random.Range (0, openList.Count);

			SearchState currentState = openList[random]; //escolhe aleatoriamente um no da arvore de procura
			VisitNode (currentState); // visita no

			if (currentState.node == targetNode) { //se o no contem o objetivo
				//devolve a solucao correspondente
				solution = currentState;
				finished = true;
				running = false;
				foundPath = true;
			} else {
				// expande o nó e acrescenta à àrvore de procura os seus sucessores
				foreach (Node suc in GetNodeSucessors(currentState.node)) {
					SearchState new_node = new SearchState(suc, suc.gCost + currentState.g, currentState);
					openList.Add(new_node);
				}
				// for energy
				if ((ulong) openList.Count > maxListSize) {
					maxListSize = (ulong) openList.Count;
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