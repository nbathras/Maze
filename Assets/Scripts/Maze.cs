using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    MazeGraph mazeGraph;

    public Maze() {
        mazeGraph = new MazeGraph();
    }

    private class MazeNode
    {
        private (int x, int y) cord;

        int weight;
        List<MazeNode> next;

        public override bool Equals(object obj) {
            MazeNode mazeNodeObj = (MazeNode) obj;
            if (this.cord.x == mazeNodeObj.cord.x && this.cord.y == mazeNodeObj.cord.y) {
                return true;
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }

    private class MazeGraph
    {
        int V;
        List<MazeNode> allNode;

        public MazeGraph() {
            int V = 0;
            allNode = new List<MazeNode>();
        }
    }

    private void AddEdge() {

    }
}
