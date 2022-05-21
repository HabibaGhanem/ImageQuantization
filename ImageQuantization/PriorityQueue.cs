using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class PriorityQueue
    {
        private HeapNode[] heap;
        private int maxSize;
        public int heapSize;
        private int[] indexes;
        public PriorityQueue(int maxSize) //θ(1)
        {
            this.maxSize = maxSize;
            this.heap = new HeapNode[maxSize];
            this.heapSize = 0;
            this.indexes = new int[maxSize];
        }

        private int getLeftChild(int index) //θ(1)
        {
            return 2 * index + 1; 
        }
        private int getRightChild(int index) //θ(1)
        {
            return 2 * index + 2; 
        }
        private int getParent(int index) //θ(1)
        {
            if (index == 0)
            {
                return -1;
            }
            return (index - 1) / 2;
        }

        private void minHeapify(int index) //O(log(V))
        {
            int smallest = index;
            int l = this.getLeftChild(index);
            int r = this.getRightChild(index);

            if (l < this.heapSize && this.heap[l].weight < this.heap[index].weight)
            {
                smallest = l;
            }
            if (r < this.heapSize && this.heap[r].weight < this.heap[smallest].weight)
            {
                smallest = r;
            }
            if (smallest != index)
            {
                this.indexes[this.heap[smallest].vertex] = index;
                this.indexes[this.heap[index].vertex] = smallest;
                Exchange(ref this.heap[index], ref this.heap[smallest]);
                this.minHeapify(smallest);
            }
        }
        public void Exchange(ref HeapNode A, ref HeapNode B) //θ(1)
        {
            HeapNode tmp = A;
            A = B;
            B = tmp;
        }

        
        /**
         * Get the min, and delete from heap
         * Runs in O(log n)
         */
        public HeapNode extractMin()  //O(log(V))
        {

            HeapNode ret = this.heap[0];
            this.heap[0] = this.heap[this.heapSize - 1];
            this.indexes[this.heap[0].vertex] = 0;
            this.heapSize --;
            this.minHeapify(0);

            return ret;
        }

    
        //Set the value at index specified to new value specified   
        public void decrement(HeapNode node) //O(log(V))
        {
            int index = indexes[node.vertex];
            this.heap[index] = node;
            while (index > 0 && this.heap[index].weight < this.heap[this.getParent(index)].weight)
            {
                this.indexes[this.heap[index].vertex] = this.getParent(index);
                this.indexes[this.heap[this.getParent(index)].vertex] = index;
                Exchange(ref this.heap[index], ref this.heap[this.getParent(index)]);
                index = this.getParent(index);
            }
        }
        public void insert(HeapNode node) //O(log(V))
        {
            if (this.heapSize >= this.maxSize)
            {
                throw new Exception("Overflow");
            }
            this.heapSize++;
            this.heap[this.heapSize - 1] = node;
            this.indexes[node.vertex] = this.heapSize - 1;
            this.decrement(node);
        }
    }
}
