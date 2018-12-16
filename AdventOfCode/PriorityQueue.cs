using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class PriorityQueue<T>
    {
        private readonly SortedDictionary<float, Queue<T>> _queues = new SortedDictionary<float, Queue<T>>();

        public int Count { get; private set; }

        public void Enqueue(T item, float priority)
        {
            if (!_queues.TryGetValue(priority, out var queue))
            {
                queue = new Queue<T>();
                _queues.Add(priority, queue);
            }

            queue.Enqueue(item);
            Count++;
        }

        public T Dequeue()
        {
            if (Count == 0 || _queues.Count == 0)
                throw new InvalidOperationException();

            var queue = _queues.First();
            var item = queue.Value.Dequeue();
            Count--;
            if (queue.Value.Count == 0)
                _queues.Remove(queue.Key);
            return item;
        }
    }
}
