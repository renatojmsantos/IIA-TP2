using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AprofundamentoProgressivo : SearchAlgorithm {

	private Stack<SearchState> openStack; //nos a visitar

	public int limiteMaximo; // a profundidade limitada tem um limite maximo

	protected override void Begin() { 
		startNode = GridMap.instance.NodeFromWorldPoint(startPos);
		targetNode = GridMap.instance.NodeFromWorldPoint(targetPos);

		SearchState start = new SearchState(startNode, 0);
		openStack = new Stack<SearchState>();
		openStack.Push(start);
	}
	
	protected override void Step() {
		
		if (openStack.Count > 0)
		{
			SearchState currentState = openStack.Pop();
			VisitNode(currentState);

			//visitados.Add (currentState);

			if (currentState.node == targetNode && currentState.depth <= limiteMaximo)
			{
				solution = currentState;
				finished = true;
				running = false;
				foundPath = true;
			}
			else
			{
				if (currentState.depth < limiteMaximo) // para os casos em q nao seja solucao, mas q nao ultrapasse o limite
				{
					foreach (Node suc in GetNodeSucessors(currentState.node))
					{
						if(!nodesVisited.Contains(suc)){
							SearchState new_node = new SearchState(suc, suc.gCost + currentState.g, currentState);
							openStack.Push(new_node);
						}
					}
					// for energy
					if ((ulong)openStack.Count > maxListSize)
					{
						maxListSize = (ulong)openStack.Count;
					}
				}
			}

		}
		else
		{
			limiteMaximo++; //incrementa o limite para fazer mais um chamada recursiva
			// limpa as listas
			openStack.Clear ();
			nodesVisited.Clear ();
			Begin(); // começa de novo. so termina quando encontrar a solucao

			//finished = true;
			//running = false;
			//foundPath = true;
		}



	}
}
