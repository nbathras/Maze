using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private class Node
    {
        public Node parent;
        public MazeCell position;

        public float g;
        public float h;
        public float f;

        public Node(Node iParent, MazeCell iPosition) {
            parent = iParent;
            position = iPosition;

            g = 0;
            h = 0;
            f = 0;
        }

        public override bool Equals(object obj) {
            Node objNode = (Node)obj;
            if (position.Equals(objNode.position)) {
                return true;
            } else {
                return false;
            }
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }

    public static List<MazeCell> RunAStar(MazeCell start, MazeCell end) {
        Node startNode = new Node(null, start);
        startNode.g = startNode.h = startNode.f = 0;
        Node endNode = new Node(null, end);
        endNode.g = endNode.h = endNode.f = 0;

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        while(openList.Count > 0) {

            // Get the current node
            Node currentNode = openList[0];
            int currentIndex = 0;
            for (int i = 0; i < openList.Count; i++) {
                if (openList[i].f < currentNode.f) {
                    currentNode = openList[i];
                    currentIndex = i;
                }
            }

            closedList.Add(openList[currentIndex]);
            openList.RemoveAt(currentIndex);

            if (currentNode.Equals(endNode)) {
                List<MazeCell> path = new List<MazeCell>();
                Node current = currentNode;
                while (current != null) {
                    path.Add(current.position);
                    current = current.parent;
                }
                path.Reverse();
                return path;
            }

            List<MazeCell> childrenMazeCell = currentNode.position.getConnections();
            List<Node> children = new List<Node>();

            foreach (MazeCell mazeCell in childrenMazeCell) {
                children.Add(new Node(currentNode, mazeCell));
            }
            
            foreach(Node child in children) {
                if (closedList.Contains(child)) {
                    continue;
                }

                child.g = currentNode.g + 1;
                child.h = Mathf.Pow((child.position.GetX() - endNode.position.GetX()), 2) + Mathf.Pow((child.position.GetY() - endNode.position.GetY()), 2);
                child.f = child.g + child.h;

                bool shouldAddChild = true;
                foreach(Node openNode in openList) {
                    if (child.Equals(openNode) && child.g > openNode.g) {
                        shouldAddChild = false;
                        break;
                    }
                }

                if(shouldAddChild) {
                    openList.Add(child);
                }
            }
        }

        return null;
    }
}
