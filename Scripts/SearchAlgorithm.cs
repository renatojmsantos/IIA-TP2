using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SearchAlgorithm : MonoBehaviour {

	public int stepsPerFrame = 100;
	public int pathCost;
	public int numberOfExpandedNodes = 0;
	public int numberOfVisited = 0;
	public ulong maxListSize = 0;
	public ulong listSizeLimit = 100;

	// for debug purposes this should hold all  nodes
	public List<Node> nodesVisited;

	public int maxNumberOfExpanded = 1000;
	[HideInInspector] public int maxNumberOfVisited = 1000;
	[HideInInspector] public Vector3 startPos;
	[HideInInspector] public Vector3 targetPos;


	protected Node startNode;
	protected Node targetNode;
	protected int numberOfSteps = 0;
	protected List<Node> path;
	protected int pathSize = 0;
	protected SearchState solution = null;
	protected bool foundPath = false;
	protected bool running = false;
	protected bool finished = false;



	public void StartRunning() {
		running = true;
		finished = false;
		numberOfSteps = 0;
		pathSize = 0;
		foundPath = false;
		numberOfExpandedNodes = 0;

		numberOfVisited = 0;
		pathCost = 0;

		// for debug purposes add all expanded nodes to this list
		nodesVisited = new List<Node>();
		maxListSize = 0;
		solution = null;
		Begin ();
	}

	// Update is called once per frame
	void Update () {
		if (running && !finished) {
			for (int i = 0; i < stepsPerFrame; i++) {
				if (!finished) {
					Step ();
					numberOfSteps++;
					if (numberOfExpandedNodes > maxNumberOfExpanded || maxListSize > listSizeLimit) {
						break;
					}
				} else {
					break;
				}
			}
		}
	}

	public bool FoundPath() {
		return foundPath;
	}

	public int GetNumberOfSteps() {
		return numberOfSteps;
	}

	public  List<Node> GetVisitedNodes() {
		return nodesVisited;
	}

	public  int GetNumberOfVisitedNodes() {
		return numberOfVisited;
	}

	public int GetNumberOfNodesExpanded() {
		return numberOfExpandedNodes;
	}

	public ulong GetMaxListSize(){
		return maxListSize;
	}

	public List<Node> RetracePath() {
		path = null;
		if (finished && foundPath) {
			path = new List<Node> ();
			// debug
			List<SearchState> lstates = new List<SearchState> ();

			SearchState state = solution;
			pathCost = (int)solution.f;
			print ("Path cost " + pathCost);
			while (state.parent != null) {
				path.Insert (0, state.node);
				lstates.Insert (0, state);
				state = state.parent;
			}
		}
			
		return path;
	}



	public int GetPathCost() {
		return pathCost;
	}

	public bool Finished() {
		return finished;
	}
		

	public int GetPathSize() {
		return pathSize;
	}

	public int GetMaxNumberOfExpandedNodes() {
		return maxNumberOfExpanded;
	}

	public int GetMaxNumberOfVisitedNoded() {
		return maxNumberOfVisited;
	}

	public ulong GetListSizeLimit() {
		return listSizeLimit;
	}

	public bool GetRunning(){
		return running;
	}

	public void setRunning(bool state){
		running = state;
	}

	public void setFinished(bool state){
		finished = state;
	}


	public void VisitNode(SearchState state) {
		nodesVisited.Add (state.node);
		numberOfVisited++;

	}

	protected List<Node> GetNodeSucessors(Node currentNode) {
		List<Node> neighbours = GridMap.instance.GetNeighbours (currentNode);
		numberOfExpandedNodes += neighbours.Count;
		return neighbours;
	}




	// These methods should be overriden on each specific search algorithm.
	protected abstract void Begin ();
	protected abstract void Step ();


	// ----------------------------------



	//https://en.wikipedia.org/wiki/Admissible_heuristic
	// https://fenix.tecnico.ulisboa.pt/downloadFile/3779572005298/cap4-proc-informada.pdf
	// https://math.stackexchange.com/questions/139600/how-to-calculate-the-euclidean-and-manhattan-distance
	// http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html


	//NOTE: You have to implement this method if your algorithm requires an heuristic

	protected virtual int GetHeuristic_Euclidean(Node p) {
		int dx = Mathf.Abs (p.gridX - targetNode.gridX);
		int dy = Mathf.Abs (p.gridY - targetNode.gridY);
		return (int)Mathf.Sqrt ((dx * dx) + (dy * dy));
	}

	protected virtual int GetHeuristic_Manhattan(Node p){
		int dx = Mathf.Abs (p.gridX - targetNode.gridX);
		int dy = Mathf.Abs (p.gridY - targetNode.gridY);
		return (int)(dx + dy);
	}
}
