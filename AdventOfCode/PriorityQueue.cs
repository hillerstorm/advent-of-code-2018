namespace AdventOfCode
{
    public class PriorityQueue<T>
    {
        private readonly MinHeap<T> _minHeap;
        private long _insertionOrder;

        public PriorityQueue()
        {
            _minHeap = new MinHeap<T>();
        }

        public int Count =>
            _minHeap.Size;

        public void Enqueue(T item, float priority) =>
            _minHeap.Insert(new HeapNode<T>(item, priority, _insertionOrder++));

        public T Dequeue() =>
            _minHeap.RemoveMin();
    }
}
