using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    public struct Edge
    {
        public int From;
        public int To;
        public double weight;
        public Edge(int From, int To, double weight)
        {
            this.From = From;
            this.To = To;
            this.weight = weight;
        }

    }

    class MST
    {
        public static int[] parent; //θ(1)
        public static Edge[] MSTEdges; //θ(1)

        public static double MinimumSpanningTree (List<RGBPixel> DistinctColors) //O(E log(V))
        {
            
            double MST_Sum = 0; //θ(1)
            int V = DistinctColors.Count; //Vertices Count
            MSTEdges = new Edge[V]; //θ(1)

            PriorityQueue MinPQ = new PriorityQueue(V); //θ(1)
            bool[] visited = new bool[V]; //θ(1)  visited == true-> the node is in MST,
                                                //else-> the node is still in the priority queue 
            parent = new int[V]; //θ(1) holds the parent of each vertex  
            HeapNode [] Node = new HeapNode[V]; //θ(1)

            Node[0] = new HeapNode(); //θ(1)
            Node[0].vertex = 0;  //θ(1)
            Node[0].weight = 0;  //θ(1)
            MinPQ.insert(Node[0]);  //O(log(V))
            visited[0] = false;  //θ(1)
            parent[0] = 0;   //θ(1)
            MSTEdges[0] = new Edge(parent[0], 0, Node[0].weight); //θ(1)

            for (int i = 1; i < V; i++) // V* O(log(V)) -> O(V log(V))
            {
                Node[i] = new HeapNode();  //θ(1)
                Node[i].vertex = i;  //θ(1)
                Node[i].weight = Double.MaxValue;  //θ(1)
                MinPQ.insert(Node[i]);  //O(log(V))
                visited[i] = false;  //θ(1)
                parent[i] = -1;  //θ(1)
            }
            
            HeapNode MinNode = new HeapNode(); //θ(1)

            while (MinPQ.heapSize!=0) // V*O(body)-> V*(O(log(V))+ O(V log(V)))-> V*O(V log(V))
            {                         // ->O(V^2 log(V)) -> O(E log(V)) (In dense graph E = O(V^2))
            
                MinNode = MinPQ.extractMin(); //O(log(V))
                visited[MinNode.vertex] = true; //θ(1)
                MST_Sum += MinNode.weight; //θ(1)

                for (int j = 0; j < V; j++) // V*O(log(V)) -> O(V log(V))
                {
                    if (visited[j] == false) //θ(1)
                    {
                        double dist = Graph.EuclideanDistance(DistinctColors[MinNode.vertex],DistinctColors[j]);//θ(1)
                        if (dist < Node[j].weight) //θ(1)
                        {
                            parent[j] = MinNode.vertex; //θ(1)
                            Node[j].weight = dist;  //θ(1)
                            MSTEdges[j] = new Edge(parent[j], j, Node[j].weight); //θ(1)
                            MinPQ.decrement(Node[j]); //O(log(V))
                        }
                    }
                }
            }
            
            return MST_Sum;
        }
    }
}
