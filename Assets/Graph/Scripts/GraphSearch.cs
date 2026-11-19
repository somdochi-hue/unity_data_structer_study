using System.Collections.Generic;
using UnityEngine;

public class GraphSearch
{
    private Graph graph;
    public List<GraphNode> path = new List<GraphNode>();

    public void Init(Graph graph)
    {
        this.graph = graph;
    }

    public void DFS(GraphNode node)
    {
        path.Clear();
        graph.ResetNodePrevious();

        var stack = new Stack<GraphNode>();
        var visited = new HashSet<GraphNode>();

        stack.Push(node);
        visited.Add(node);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            path.Add(current);

            foreach (var adjacents in current.adjacents)
            {
                if (!adjacents.CanVisit || visited.Contains(adjacents))
                {
                    continue;
                }

                stack.Push(adjacents);
                visited.Add(adjacents);
            }
        }
    }

    public void BFS(GraphNode node)
    {
        path.Clear();
        graph.ResetNodePrevious();

        var queue = new Queue<GraphNode>();
        var visited = new HashSet<GraphNode>();

        queue.Enqueue(node);
        visited.Add(node);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            path.Add(current);

            foreach (var adjacents in current.adjacents)
            {
                if (!adjacents.CanVisit || visited.Contains(adjacents))
                {
                    continue;
                }

                queue.Enqueue(adjacents);
                visited.Add(adjacents);
            }
        }

    }

    public void DFSRecursive(GraphNode node)
    {
        path.Clear();
        graph.ResetNodePrevious();

        var visited = new HashSet<GraphNode>();

        RecursiveHelper(node, visited);
    }

    protected void RecursiveHelper(GraphNode node, HashSet<GraphNode> visited)
    {
        if (node == null) return;

        visited.Add(node);
        path.Add(node);

        foreach (GraphNode nextNode in node.adjacents)
        {
            if (!visited.Contains(nextNode) && nextNode.CanVisit)
            {
                RecursiveHelper(nextNode, visited);
            }
        }
    }

    public bool PathFindingBFS(GraphNode startNode, GraphNode endNode)
    {
        path.Clear();
        graph.ResetNodePrevious();

        var queue = new Queue<GraphNode>();
        var visited = new HashSet<GraphNode>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        bool success = false;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == endNode)
            {
                success = true;
                break;
            }

            foreach (var adjacents in current.adjacents)
            {
                if (!adjacents.CanVisit || visited.Contains(adjacents))
                {
                    continue;
                }

                adjacents.previous = current;

                visited.Add(adjacents);
                queue.Enqueue(adjacents);
            }
        }

        if (!success)
        {
            return false;
        }

        GraphNode step = endNode;
        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }

        path.Reverse();
        return true;
    }

    public bool Dijkstra(GraphNode startNode, GraphNode endNode)
    {
        path.Clear();
        graph.ResetNodePrevious();

        var visited = new HashSet<GraphNode>();
        var pQueue = new PriorityQueue<GraphNode, int>();
        var distances = new int[graph.nodes.Length];
        for (int i = 0; i < distances.Length; ++i)
        {
            distances[i] = int.MaxValue;
        }

        distances[startNode.id] = 0;
        pQueue.Enqueue(startNode, distances[startNode.id]);

        bool success = false;
        while (pQueue.Count > 0)
        {
            var currentNote = pQueue.Dequeue();
            if (visited.Contains(currentNote))
            {
                continue;
            }

            if (currentNote == endNode)
            {
                success = true;
                break;
            }

            visited.Add(currentNote);

            foreach (var adjacents in currentNote.adjacents)
            {
                if (!adjacents.CanVisit || visited.Contains(adjacents))
                {
                    continue;
                }

                var newDistance = distances[currentNote.id] + adjacents.weight;
                if (distances[adjacents.id] > newDistance)
                {
                    distances[adjacents.id] = newDistance;
                    adjacents.previous = currentNote;
                    pQueue.Enqueue(adjacents, newDistance);
                }
            }
        }

        if (!success)
        {
            return false;
        }

        GraphNode step = endNode;
        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }
        path.Reverse();
        return true;
    }

    private int Heuristic(GraphNode a, GraphNode b)
    {
        int ax = a.id % graph.cols;
        int ay = a.id / graph.cols;

        int bx = b.id % graph.cols;
        int by = b.id / graph.cols;

        return Mathf.Abs(ax - bx) + Mathf.Abs(ay - by);
    }

    public bool Astra(GraphNode startNode, GraphNode endNode)
    {
        path.Clear();
        graph.ResetNodePrevious();

        var visited = new HashSet<GraphNode>();
        var pQueue = new PriorityQueue<GraphNode, int>();
        var distances = new int[graph.nodes.Length];
        for (int i = 0; i < distances.Length; ++i)
        {
            distances[i] = int.MaxValue;
        }

        distances[startNode.id] = 0;
        pQueue.Enqueue(startNode, distances[startNode.id] + Heuristic(startNode, endNode));

        bool success = false;
        while (pQueue.Count > 0)
        {
            var currentNote = pQueue.Dequeue();
            if (visited.Contains(currentNote))
            {
                continue;
            }

            if (currentNote == endNode)
            {
                success = true;
                break;
            }

            visited.Add(currentNote);

            foreach (var adjacents in currentNote.adjacents)
            {
                if (!adjacents.CanVisit || visited.Contains(adjacents))
                {
                    continue;
                }

                var newDistance = distances[currentNote.id] + adjacents.weight;
                if (distances[adjacents.id] > newDistance)
                {
                    distances[adjacents.id] = newDistance;
                    adjacents.previous = currentNote;
                    pQueue.Enqueue(adjacents, newDistance + Heuristic(adjacents, endNode));
                }
            }
        }

        if (!success)
        {
            return false;
        }

        GraphNode step = endNode;
        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }
        path.Reverse();
        return true;
    }
}
