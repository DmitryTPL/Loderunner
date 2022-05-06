using System;
using System.Collections.Generic;
using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class AStarPathFinder : IPathFinder
    {
        private readonly Vector2Int[] _directions;

        public AStarPathFinder()
        {
            _directions = new Vector2Int[]
            {
                new(1, 0),
                new(0, -1),
                new(-1, 0),
                new(0, 1)
            };
        }

        public Stack<Vector2Int> GetPath(Matrix<int> map, Vector2Int startPoint, Vector2Int goal)
        {
            var frontier = new PriorityQueue<Vector2Int>();

            frontier.Enqueue(startPoint, 0);

            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

            var pathCost = new Dictionary<Vector2, float>
            {
                [startPoint] = 0
            };

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current == goal)
                {
                    break;
                }

                var neighbours = GetNeighbours(map, current);

                foreach (var neighbour in neighbours)
                {
                    if (map[neighbour.y, neighbour.x] <= 0)
                    {
                        continue;
                    }

                    var newPathCost = pathCost[current] + map[neighbour.y, neighbour.x];

                    if (!pathCost.ContainsKey(neighbour) || newPathCost < pathCost[neighbour])
                    {
                        pathCost[neighbour] = newPathCost;

                        var priority = newPathCost + GetHeuristicsPathDistance(neighbour, goal);

                        frontier.Enqueue(neighbour, priority);

                        cameFrom[neighbour] = current;
                    }
                }
            }

            var path = new Stack<Vector2Int>();

            if (!cameFrom.ContainsKey(goal))
            {
                path.Push(startPoint);
                
                return path;
            }

            path.Push(goal);

            var pathPoint = goal;

            while (true)
            {
                pathPoint = cameFrom[pathPoint];

                if (pathPoint == startPoint)
                {
                    break;
                }

                path.Push(pathPoint);
            }

            return path;
        }

        private float GetHeuristicsPathDistance(Vector2Int current, Vector2Int goal)
        {
            return Math.Abs(current.x - goal.x) + Math.Abs(current.y - goal.y);
        }

        private IEnumerable<Vector2Int> GetNeighbours(Matrix<int> map, Vector2Int point)
        {
            var neighbours = new List<Vector2Int>();

            foreach (var direction in _directions)
            {
                var neighbour = point + direction;

                if (neighbour.x >= 0 && neighbour.x < map.Columns &&
                    neighbour.y >= 0 && neighbour.y < map.Rows)
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }
    }
}