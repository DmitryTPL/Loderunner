using System;
using System.Collections.Generic;

namespace Loderunner.Service
{
    public class PriorityQueue<T>
    {
        private readonly LinkedList<PriorityNode> _items = new();

        private readonly struct PriorityNode
        {
            public float Priority { get; }
            public T Item { get; }

            public PriorityNode(T item, float priority)
            {
                Item = item;
                Priority = priority;
            }
        }

        public int Count => _items.Count;

        public void Enqueue(T item, float priority)
        {
            if (_items.Count == 0)
            {
                _items.AddLast(new PriorityNode(item, priority));
                return;
            }

            PriorityNode lowPriorityNode = default;

            foreach (var node in _items)
            {
                if (node.Priority >= priority)
                {
                    lowPriorityNode = node;
                    break;
                }
            }

            var linkedListNode = _items.Find(lowPriorityNode);

            if (linkedListNode == null)
            {
                _items.AddLast(new PriorityNode(item, priority));
            }
            else
            {
                _items.AddBefore(linkedListNode, new PriorityNode(item, priority));
            }
        }

        public T Dequeue()
        {
            if (_items.Count == 0)
            {
                throw new IndexOutOfRangeException("No items in queue");
            }
            
            
            var node = _items.First;
            
            _items.RemoveFirst();

            return node.Value.Item;
        }
    }
}