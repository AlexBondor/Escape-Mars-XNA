using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Path
{
    public class GraphNode
    {
        // Flag to check whether the node should be drawn or not
        public bool Active { get; set; }

        // Boolean which says whether the graph should be
        // displayed or not
        public bool DisplayGraph { get; set; }

        // Position of node in graph
        public int Row { get; set; }
        public int Col { get; set; }

        // Node index
        public int Index { get; private set; }

        // Position of node
        public Vector2 Position { get; set; }

        // Sprite of node that should be displayed
        public Texture2D Texture { get; set; }

        // Sprite of node with dots
        public Texture2D Dot { get; set; }

        // List of neighbour nodes
        public List<GraphEdge> Edges = new List<GraphEdge>();
        
        //
        public int Width { get; set; }
        //
        public int Height { get; set; }

        public GraphNode(int index, Vector2 position)
        {
            Index = index;
            Position = position;
            Width = 32;
            Height = 32;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var centerX = (int) Position.X;// + Width/2;
            var centerY = (int) Position.Y;// + Height/2;
            spriteBatch.Draw(
                Texture,
                new Rectangle(centerX, centerY, Width, Height),
                Color.White
                );

            if (!DisplayGraph) return;

            if (!Active) return;

            spriteBatch.Draw(
                Dot,
                new Rectangle(centerX, centerY, Width, Height),
                Color.Black
                );
        }
    }
}
