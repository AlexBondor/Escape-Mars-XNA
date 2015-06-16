using System;
using System.Collections.Generic;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Path
{
    class PathPlanning
    {
        private readonly MovingEntity _owner;

        private readonly Graph _navGraph;

        private Vector2 _startPosition;
        private Vector2 _endPos;

        private Vector2 _intermediatePos;
        private Vector2 _currentPos;

        private readonly AStar _aStar;
        private List<GraphEdge> _path;

        public PathPlanning(MovingEntity owner, Graph navGraph)
        {
            _owner = owner;
            _navGraph = navGraph;
            _aStar = new AStar(navGraph);
            _intermediatePos = _owner.Position;
        }

        public bool CreatePath(Vector2 from, Vector2 to)
        {
            _owner.Arrived = false;
            _owner.Arriving = false;

            _startPosition = from;
            _endPos = to;

            var startNode = _navGraph.GetNodeByPosition(from);
            var endNode = _navGraph.GetNodeByPosition(to);

            if (startNode.Index == endNode.Index)
            {
                _owner.Arrived = true;
                return false;
            }

            _aStar.Search(startNode, endNode);

            try
            {
                _path = _aStar.GetPath();
                if (_path.Count == 0) return false;
                _intermediatePos = Vector2Helper.ScalarAdd(_path[0].To.Position, 16);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void Update()
        {
            if (_path != null && _path.Count == 0) return;
            _currentPos = _owner.Position;  

            if (Vector2Helper.Distance(_currentPos, _intermediatePos) < 5 && _path != null)
            {
                if (_path.Count == 1)
                {
                    _owner.Arriving = true;
                }
                _intermediatePos = Vector2Helper.ScalarAdd(_path[0].To.Position, 16);
                _path.Remove(_path[0]);
            }
            _owner.SeekablePosition = _intermediatePos;
        }

        public AStar GetAStar()
        {
            return _aStar;
        }
    }
}
