using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class MinHeap<T>
    {
        private readonly List<HeapNode<T>> _values;

        public MinHeap()
        {
            Size = 0;
            _values = new List<HeapNode<T>>();
        }

        public IEnumerable<T> Values => _values.Take(Size).Select(x => x.Value);
        public int Size { get; private set; }

        public void Insert(HeapNode<T> value)
        {
            Size++;
            var i = Size - 1;
            if (i >= _values.Count)
                _values.Add(value);
            else
                _values[i] = value;
            while (i != 0 && _values[Parent(i)].CompareTo(_values[i]) == 1)
            {
                Swap(i, Parent(i));
                i = Parent(i);
            }
        }

        public T RemoveMin()
        {
            if (Size == 0)
                throw new IndexOutOfRangeException();

            if (Size == 1)
            {
                Size--;
                return _values[0].Value;
            }

            var min = _values[0];
            _values[0] = _values[Size - 1];
            Size--;
            MinHeapify(0);

            return min.Value;
        }

        private void Swap(int i, int parentIdx)
        {
            var tmp = _values[i];
            _values[i] = _values[parentIdx];
            _values[parentIdx] = tmp;
        }

        private void MinHeapify(int i)
        {
            while (true)
            {
                var left = Left(i);
                var right = Right(i);
                var min = i;

                if (left < Size && _values[left].CompareTo(_values[i]) == -1)
                    min = left;
                if (right < Size && _values[right].CompareTo(_values[min]) == -1)
                    min = right;

                if (min != i)
                {
                    Swap(i, min);
                    i = min;
                    continue;
                }

                break;
            }
        }

        private static int Parent(int i) => (i - 1) / 2;
        private static int Left(int i) => 2 * i + 1;
        private static int Right(int i) => 2 * i + 2;
    }
}
