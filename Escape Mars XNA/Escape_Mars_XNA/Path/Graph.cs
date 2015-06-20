using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Path
{
    public class Graph
    {
        // List of graph nodes
        public List<GraphNode> Nodes { get; set; }

        // List of graph edges
        public List<GraphEdge> Edges { get; set; }

        // List of graph drawable edges
        public List<GraphEdge> DrawableEdges { get; set; }

        // Graph dimensions
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        // List of neighbour nodes
        public Dictionary<GraphNode, List<GraphEdge>> NodeEdgeList { get; set; }

        public Graph(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;

            Nodes = new List<GraphNode>();
            Edges = new List<GraphEdge>();
            NodeEdgeList = new Dictionary<GraphNode, List<GraphEdge>>();
            DrawableEdges = new List<GraphEdge>();

            InitializeGraph(rows, cols);
        }

        // Create nodes and edges between them
        private void InitializeGraph(int rows, int cols)
        {
            var index = 0;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    // Compute position of node
                    var position = new Vector2(j * 32, i * 32);

                    // Create new node
                    var node = new GraphNode(index, position)
                    {
                        Row = i,
                        Col = j
                    };

                    // Add it to Nodes list
                    Nodes.Add(node);
                    NodeEdgeList.Add(node, new List<GraphEdge>());

                    var indexTop = index - cols;
                    var indexTopLeft = indexTop - 1;
                    var indexTopRight = indexTop + 1;
                    var indexLeft = index - 1;

                    // If there is a neighbour on top side
                    if (indexTop >= 0)
                    {
                        var topNode = GetNodeByIndex(indexTop);
                        CreateEdges(node, topNode);
                    }
                    
                    // If there is a neighbour on top left side
                    if (indexTopLeft >= 0 && indexTop % cols != 0)
                    {
                        var topLeftNode = GetNodeByIndex(indexTopLeft);
                        CreateEdges(node, topLeftNode);
                    }

                    // If there is a neighbour on top right side
                    if (indexTopRight > 0 && (indexTop + 1) % cols != 0)
                    {
                        var topRightNode = GetNodeByIndex(indexTopRight);
                        CreateEdges(node, topRightNode);
                    }

                    // If there is a neighbour on left side
                    if (indexLeft >= 0 && index % cols != 0)
                    {
                        var leftNode = GetNodeByIndex(indexLeft);
                        CreateEdges(node, leftNode);
                    }

                    index++;
                }
            }
        }

        // Creates the links from source to destination by adding edges
        // to both the NodeEdgeList of the graph and the Edges list of each
        // of the source and destination
        private void CreateEdges(GraphNode source, GraphNode destination)
        {
            // Values determined by tests
            var cost = 0.76;
            if (source.Col != destination.Col && source.Row != destination.Row) cost = 1.42;

            // Add edge from source to target in both the Graph
            // NodeEdgeList and the Edges list of source 
            var edge = new GraphEdge(source, destination, cost);
            NodeEdgeList[source].Add(edge);
            source.Edges.Add(edge);

            Edges.Add(edge);

            // Do the same for destination
            edge = new GraphEdge(destination, source, cost);
            NodeEdgeList[destination].Add(edge);
            destination.Edges.Add(edge);

            Edges.Add(edge);
            DrawableEdges.Add(edge);
        }

        // Get node by index
        public GraphNode GetNodeByIndex(int index)
        {
            try
            {
                return Nodes.First(graphNode => graphNode.Index == index);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Return the node for which the position intersects its zone
        public GraphNode GetNodeByPosition(Vector2 position)
        {
            try
            {
                var node =  Nodes.First(graphNode =>
                    graphNode.Position.X <= position.X &&
                    graphNode.Position.X + graphNode.Width > position.X &&
                    graphNode.Position.Y <= position.Y &&
                    graphNode.Position.Y + graphNode.Height > position.Y);
                return node;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        // Get node by Row and Col
        public GraphNode GetNodeByRowCol(int row, int col)
        {
            try
            {
                return Nodes.First(graphNode => graphNode.Row == row && graphNode.Col == col);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Get edges by node
        public List<GraphEdge> GetEdgesByNode(GraphNode node)
        {
            try
            {
                return NodeEdgeList[node];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public GraphEdge GetEdge(GraphNode start, GraphNode end)
        {
            return Edges.Find(e => (e.From.Index == start.Index && e.To.Index == end.Index));
        }

        public GraphEdge GetDrawableEdge(GraphNode start, GraphNode end)
        {
            return DrawableEdges.Find(e => (e.From.Index == start.Index && e.To.Index == end.Index) ||
                                            (e.From.Index == end.Index && e.To.Index == start.Index));
        }

        // Return the number of nodes of the graph
        public int NumNodes()
        {
            return Rows * Cols;
        }

        public GraphNode[] GetObstacles()
        {
            return Nodes.Where(n => !n.Active).Where(node => node.Col != 0 && node.Col != Cols - 1 && node.Row != 0 && node.Row != Rows - 1).ToArray();
        }

        public Vector2 RandomValidNode(int rowLeft, int rowRight, int colLeft, int colRight)
        {
            // So that consecutive calls won't return the same node
            Thread.Sleep(10);

            var nodes = Nodes.Where(n => n.Active).ToArray();

            var rdn = new Random();

            while (true)
            {
                var row = rdn.Next(rowLeft, rowRight);
                var col = rdn.Next(colLeft, colRight);
                try
                {
                    var temp = nodes.First(n => n.Row == row && n.Col == col);
                    return temp.Position;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
