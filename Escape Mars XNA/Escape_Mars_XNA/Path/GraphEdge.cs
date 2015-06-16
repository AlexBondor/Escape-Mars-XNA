using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Path
{
    public class GraphEdge
    { 
        // Starting node of edge
        public GraphNode From { get; set; }

        // Other end of edge
        public GraphNode To { get; set; }

        // Cost of traversing the edge
        public double Cost { get; set; }

        public Color Color { get; set; }

        // Texture used for drawing lines
        public Texture2D Texture { get; set; }

        public bool DisplayGraph { get; set; }
        
        public GraphEdge(GraphNode from, GraphNode to, double cost)
        {
            From = from;
            To = to;
            Cost = cost;
            Color = Color.LightGray;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!DisplayGraph) return;

            if (!From.Active || !To.Active) return;

            var edge = To.Position - From.Position;

            // Clculate angle to rotate line
            var angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(Texture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)From.Position.X + To.Width / 2,
                    (int)From.Position.Y + To.Height / 2,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    2), //width of line, change this to make thicker line
                null,
                Color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }
    }
}
