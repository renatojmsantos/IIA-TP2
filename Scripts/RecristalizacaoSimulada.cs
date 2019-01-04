using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecristalizacaoSimulada : SearchAlgorithm {
	
	private SearchState currentState;
	public float temperatura; //inicial
	public float Tdescrementa; 
	public float T;

	public int seed;

	public int NrHeuristica=0;

	protected override void Begin () {
		
		startNode = GridMap.instance.NodeFromWorldPoint (startPos);
		targetNode = GridMap.instance.NodeFromWorldPoint (targetPos);

		if (NrHeuristica == 0)
			currentState = new SearchState (startNode, 0, GetHeuristic_Euclidean(startNode));
		else
			currentState = new SearchState (startNode, 0, GetHeuristic_Manhattan(startNode));

		Random.InitState (seed);
		T = temperatura;

	}
	
	protected override void Step() {
		
		if (T > 0) {
			temperatura -= Tdescrementa; 
			// teoricamente, quanto + lentamente T for descrescendo, maior é a possibilidade de encontrar o maximo global
			VisitNode (currentState);

			if (T == 0) { 
				// devolve nó corrente
				solution = currentState;
				finished = true;
				running = false;
				foundPath = true;
				T = temperatura;
			} else {
				List<Node> sucessors = GetNodeSucessors (currentState.node);
				// no_seguinte <- aleatorio(expansao(no_corrente, operadores(problema)))
				Node randomNode = sucessors [Random.Range (0, sucessors.Count)]; 

				int h;
				if (NrHeuristica == 0) {
					h = GetHeuristic_Euclidean (randomNode);
				} else {
					h = GetHeuristic_Manhattan (randomNode);
				}

				if (h > currentState.h) { // h(no seguinte) > h (no corrente)
					// no corrente <- no seguinte
					currentState = new SearchState(randomNode, randomNode.gCost + currentState.g, h, currentState);
				} else {
					// no corrente <- prob(no_seguinte, e exp (deltaE/T)
					// https://docs.unity3d.com/ScriptReference/Random-value.html
					float num = Random.value; // entre 0.0 e 1.0
					float dif_qualidade; // diferença de qualidade entre o novo nó e o nó corrente
					dif_qualidade = currentState.h - h;

					float prob = Mathf.Exp (dif_qualidade / T);
					if (prob <= num) {
						currentState = new SearchState (randomNode, randomNode.gCost + currentState.g, h, currentState);
					}
				}
			}
		}
		else{
			finished = true;
			running = false;
			foundPath = true;
			//T = temperatura;
		}

	}
}
