using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesquisaEmProfundidadeLimitada : SearchAlgorithm
{

	private Stack<SearchState> openStack; // a visitar

	public int limiteMaximo; // a profundidade limitada, tem um limite maximo, tal como o nome indica


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
			// retira da lista o primeiro nó e insere-o na lista de visitados
			SearchState currentState = openStack.Pop();
			VisitNode(currentState);

			// limita-se o nr de nos a percorrer
			if (currentState.node == targetNode && currentState.depth <= limiteMaximo)
			{
				solution = currentState;
				finished = true;
				running = false;
				foundPath = true;
			}
			else
			{
				// para os casos em q nao seja solucao, mas q nao ultrapasse o limite
				if (currentState.depth < limiteMaximo) 
				{
					foreach (Node suc in GetNodeSucessors(currentState.node))
					{
						if (!nodesVisited.Contains (suc)) {
							// se o sucessor nao estiver na lista de visitados
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

		}
		else
		{ //pilha vazia
			// falha
			finished = true;
			running = false;
			foundPath = true;
		}

	}
}

