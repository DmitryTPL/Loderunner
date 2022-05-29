using System.Collections.Generic;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public readonly struct PathResult
    {
        public Stack<Vector2Int> Path { get; }
        public SearchPathResult SearchResult { get; }
        
        public PathResult(Stack<Vector2Int> path)
        {
            Path = path;
            SearchResult = SearchPathResult.Success;
        }
        
        public PathResult(SearchPathResult searchResult)
        {
            Path = null;
            SearchResult = searchResult;
        }

    }
}