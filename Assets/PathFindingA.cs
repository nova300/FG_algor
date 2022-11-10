using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingA : MonoBehaviour
{
    HashSet<Vector2Int> closedPos = new HashSet<Vector2Int>();
    List<Node> openNodes = new List<Node>();
    List<Node> closedNodes = new List<Node>();
    Node currentNode;

    void A_Star(Vector2Int start, Vector2Int goal)
    {
        openNodes.Clear();
        closedNodes.Clear();
        closedPos.Clear();

        openNodes.Add(new Node(start, 0, Vector2.Distance(start, goal)));


        while (openNodes.Count > 0)
        {
            currentNode = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].f <= currentNode.f && openNodes[i].h < currentNode.h)
                {
                    currentNode = openNodes[i];
                }
            }

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            closedPos.Add(currentNode.pos);

            Vector2Int[] nextStep = GetNextStep(currentNode.pos);

            for (int i = 0; i < nextStep.Length; i++)
            {
                if (closedPos.Contains(nextStep[i])){
                    continue;
                }
                else if (CheckCollision(nextStep[i]))
                {
                    closedPos.Add(nextStep[i]);
                    continue;
                }

                Node nextStepNode = PosToNode(nextStep[i]);
                float nextStepG = currentNode.g + Vector2Int.Distance(nextStep[i], currentNode.pos);

                if (nextStepNode != null)
                {
                    if (nextStepNode.g > nextStepG){
                        nextStepNode.Set(nextStepG, currentNode);
                    }
                } else {
                    openNodes.Add(new Node(nextStep[i], nextStepG, Vector2Int.Distance(nextStep[i], goal), currentNode));
                }

                
            }
        }
    }

    public bool CheckCollision(Vector2Int pos)
    {
        if (Physics2D.OverlapPoint(pos) != null)
        {
            return true;
        }
        return false;
    }

    public Vector2Int[] GetNextStep(Vector2Int pos)
    {
        return new[]{
            pos + Vector2Int.up,
            pos + Vector2Int.down,
            pos + Vector2Int.left,
            pos + Vector2Int.right
        };
    }

    Node PosToNode(Vector2Int pos)
    {
        return closedNodes.Find(Node => Node.pos == pos);
    }

    public class Node
    {
        public Vector2Int pos;
        public Node parent;
        public float g;
        public float h;
        public float f;

        public Node(Vector2Int pos, float g, float h, Node parent = null)
        {
            this.pos = pos;
            this.parent = parent;
            this.g = g;
            this.h = h;
            this.f = g + h;
        }

        public void Set(float g, Node parent){
            this.g = g;
            this.parent = parent;
        }

    }
}
