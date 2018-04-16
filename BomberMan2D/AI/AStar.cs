using BehaviourEngine;
using BehaviourEngine.Pathfinding;
using System.Collections.Generic;

namespace BomberMan2D
{
    public static class AStar
    {
        public static List<Node> GetPath<T>(T map, int startX, int startY, int endX, int endY) where T : IMap
        {
            Node start = (map as Map).GetNodeByIndex(startX, startY);
            Node end = (map as Map).GetNodeByIndex(endX, endY);
            if (start != null && end != null)
            {
                return GetPath(start, end);
            }
            return null;
        }

        public static List<Node> GetPath(Node start, Node end)
        {
            List<Node> path = new List<Node>();

            Frontier<Node> frontier = new Frontier<Node>();
            // hashing
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

            // Dijkstra
            Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();

            frontier.Enqueue(start, 0);
            cameFrom[start] = null;
            costSoFar[start] = 0;

            while (!frontier.IsEmpty)
            {
                Node currentNode = frontier.Dequeue();

                if (currentNode == end)
                    break;

                foreach (Node neighbour in currentNode.Neighbours)
                {
                    int newCost = costSoFar[currentNode] + neighbour.Cost;
                    if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
                    {
                        costSoFar[neighbour] = newCost;
                        int priority = newCost;
                        frontier.Enqueue(neighbour, priority);
                        // for each neighbour, store its parent
                        cameFrom[neighbour] = currentNode;
                    }
                }
            }

            // build the list of node to traverse
            Node currentStep = end;
            while (currentStep != start && currentStep != null)
            {
                if (!cameFrom.ContainsKey(currentStep)) return null;
                path.Add(currentStep);
                currentStep = cameFrom[currentStep];
            }
            //path.Add(start);

            path.Reverse();

            return path;
        }
    }
}
