using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingA : MonoBehaviour
{
    [SerializeField] bool manhattanDist, manhattanPath;
    [SerializeField] public Vector2[] path;
    [SerializeField] Transform startT, goalT;
    [SerializeField] float time;
    [SerializeField] int poisions, nodes;
    HashSet<Vector2Int> closedPos = new HashSet<Vector2Int>();
    List<Node> openNodes = new List<Node>();
    HashSet<Node> closedNodes = new HashSet<Node>();
    Node currentNode;
    Vector2Int goalPos;
    Vector2Int startPos;
    float timeS;
    public int pi = 1;
    Vector2 currentTarget;

    [ContextMenu("MakePath")] public void MakePath()
    {
        startPos = new Vector2Int(Mathf.RoundToInt(startT.position.x) ,Mathf.RoundToInt(startT.position.y));
        goalPos = new Vector2Int(Mathf.RoundToInt(goalT.position.x) ,Mathf.RoundToInt(goalT.position.y));
        path = null;
        path = A_Star(startPos, goalPos);
        time = (Time.realtimeSinceStartup - timeS);
        poisions = closedPos.Count;
        nodes = closedNodes.Count;

    }

    void OnDrawGizmos()
    {

        Vector2Int deltaStartPos = new Vector2Int(Mathf.RoundToInt(startT.position.x) ,Mathf.RoundToInt(startT.position.y));
        Vector2Int deltaGoalPos = new Vector2Int(Mathf.RoundToInt(goalT.position.x) ,Mathf.RoundToInt(goalT.position.y));
        if (deltaStartPos != startPos || deltaGoalPos != goalPos)
        {
            //MakePath();
        }
        if (path.Length < 1)
        {
            return;
        }
        Gizmos.color = Color.red;
        for (int i = 1; i < path.Length; i++)
        {
            Gizmos.DrawLine(path[i], path[i - 1]);
        }
    }
    
    void OnValidate()
    {
        //MakePath();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            goalT.gameObject.SetActive(true);
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            goalT.localPosition = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0);
            startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x) ,Mathf.RoundToInt(transform.position.y));
            goalPos = new Vector2Int(Mathf.RoundToInt(pos.x) ,Mathf.RoundToInt(pos.y));
            path = null;
            path = A_Star(startPos, goalPos);
            pi = 1;
            return;
        }
        

        if (path == null)
        {
            pi = 1;
            return;
        }

        if (pi >= path.Length)
        {
            path = null;
            goalT.gameObject.SetActive(false);
            return;
        }

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), path[pi]) > 1)
        {
            transform.Translate((path[pi] - new Vector2(transform.position.x, transform.position.y)) * 10f * Time.deltaTime);
        }
        else
        {
            pi++;
        }

        

        
    }


    

    public Vector2[] A_Star(Vector2Int start, Vector2Int goal)
    {
        timeS = Time.realtimeSinceStartup;
        openNodes.Clear();
        closedNodes.Clear();
        closedPos.Clear();

        Node startNode = new Node(start, 0, Dist(start, goal));
        openNodes.Add(startNode);


        while (openNodes.Count > 0)
        {    
            currentNode = openNodes[0];
            for (int i = 0; i < openNodes.Count; i++)
            {
                if (openNodes[i].f <= currentNode.f && openNodes[i].h < currentNode.h)
                {
                    currentNode = openNodes[i];
                }
            }

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            closedPos.Add(currentNode.pos);

            if (currentNode.pos == goal)
            {
                return recordPath(startNode, currentNode);
            }

            if (currentNode.h > Dist(start, goal) * 2)
            {
                return null;
            }
            

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

                Node nextStepNode = openNodes.Find(Node => Node.pos == nextStep[i]);
                float nextStepG = currentNode.g + Dist(nextStep[i], currentNode.pos);

                if (nextStepNode != null)
                {
                    if (nextStepNode.g > nextStepG){
                        nextStepNode.Set(nextStepG, currentNode);
                    }
                } else {
                    openNodes.Add(new Node(nextStep[i], nextStepG, Dist(nextStep[i], goal), currentNode));
                }

                
            }
        }
        return null;
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
        if (manhattanPath)
        {
            return new[]{
            pos + Vector2Int.up,
            pos + Vector2Int.down,
            pos + Vector2Int.left,
            pos + Vector2Int.right
            };
        }
        return new[]{
            pos + Vector2Int.up,
            pos + Vector2Int.down,
            pos + Vector2Int.left,
            pos + Vector2Int.right,
            pos + Vector2Int.up + Vector2Int.left,
            pos + Vector2Int.up + Vector2Int.right,
            pos + Vector2Int.down + Vector2Int.left,
            pos + Vector2Int.down + Vector2Int.right
        };
    }

    public Vector2[] recordPath(Node firstNode, Node lastNode)
    {
        List<Vector2> path = new List<Vector2>();
        Node current = lastNode;

        while (current != firstNode)
        {
            path.Add(current.pos);
            current = current.parent;
        }
        path.Add(current.pos);
        path.Reverse();
        Vector2[] result = path.ToArray();
        return result;
    }

    public float Dist(Vector2Int a, Vector2Int b)
    {
        if (manhattanDist)
        {
            return Mathf.Abs(a.x-b.x) + Mathf.Abs(a.y-b.y);
        }
        return Vector2Int.Distance(a, b);
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
