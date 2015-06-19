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
            _startPosition = from;
            _endPos = to;

            var startNode = _navGraph.GetNodeByPosition(from);
            var endNode = _navGraph.GetNodeByPosition(to);

            if (startNode == null || endNode == null)
            {
                return false;
            }

            if (startNode.Index == endNode.Index)
            {
                return false;
            }

            if (!_aStar.Search(startNode, endNode)) return false;

            _path = _aStar.GetPath();
            _intermediatePos = Vector2Helper.ScalarAdd(_path[0].To.Position, 16);

            return true;
        }

        public void Update()
        {
            if (_path != null && _path.Count == 0)
            {
                return;
            }
            _currentPos = _owner.Position;  

            if (Vector2Helper.Distance(_currentPos, _intermediatePos) < 5 && _path != null)
            {
                _intermediatePos = Vector2Helper.ScalarAdd(_path[0].To.Position, 16);
                _path.Remove(_path[0]);
            }
            _owner.SteeringPosition = _intermediatePos;
        }

        public AStar GetAStar()
        {
            return _aStar;
        }

        public double GetCostToClosestItem(EntityFeature.Itm itemType)
        {
            var itemTypePositions = _owner.World.GetItemTypePositions(itemType);

            var closest = double.MaxValue;
            var closestPos = new Vector2(float.MaxValue, float.MaxValue);

            foreach (var itemPosition in itemTypePositions)
            {
                var dist = Vector2Helper.DistanceSq(_owner.Position, itemPosition);
                if (dist < closest)
                {
                    closest = dist;
                    closestPos = itemPosition;
                }
            }

            if (Math.Abs(closestPos.X - float.MaxValue) < float.Epsilon)
            {
                return -1;
            }

            var startNode = _navGraph.GetNodeByPosition(_owner.Position);
            var endNode = _navGraph.GetNodeByPosition(closestPos);

            if (_aStar.Search(startNode, endNode))
            {
                return _aStar.GetCost();
            }
            return -1;
        }
    }
}
