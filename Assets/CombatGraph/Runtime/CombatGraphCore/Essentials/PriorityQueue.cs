using System;
using System.Collections.Generic;

namespace CombatGraph
{
    public class PriorityQueue<T>
    {
        private List<(float Priority, T Item)> _heap = new List<(float, T)>();

        private void Swap(int i, int j)
        {
            var temp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = temp;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_heap[index].Priority < _heap[parent].Priority)
                {
                    Swap(index, parent);
                    index = parent;
                }
                else
                    break;
            }
        }

        private void HeapifyDown(int index)
        {
            int lastIndex = _heap.Count - 1;

            while (index < _heap.Count)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int smallest = index;

                if (left <= lastIndex && _heap[left].Priority < _heap[smallest].Priority)
                    smallest = left;
                if (right <= lastIndex && _heap[right].Priority < _heap[smallest].Priority)
                    smallest = right;

                if (smallest != index)
                {
                    Swap(index, smallest);
                    index = smallest;
                }
                else
                    break;
            }
        }

        public void Enqueue(T item, float priority)
        {
            _heap.Add((priority, item));
            HeapifyUp(_heap.Count - 1);
        }

        public T Dequeue()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("The queue is empty.");

            T result = _heap[0].Item;
            _heap[0] = _heap[^1];
            _heap.RemoveAt(_heap.Count - 1);
            HeapifyDown(0);
            return result;
        }

        public int Count => _heap.Count;

        public bool IsEmpty => _heap.Count == 0;

        public void Clear()
        {
            _heap.Clear();
        }
    }
}
