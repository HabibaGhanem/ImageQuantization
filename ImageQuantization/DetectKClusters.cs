using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class DetectKClusters
    {
        Edge[] edges; //θ(1)
        List<Edge> edge; //θ(1)
        double currentstd = 0; //θ(1)
        double prevstd = 0; //θ(1)
        public int K = 0; //θ(1)
        public DetectKClusters(Edge[] edges)
        {
            this.edges =edges; //θ(1)

        }
        public double getMean() //O(E)
        {
            double mean = 0; //θ(1)
            for (int i = 0; i < edge.Count; i++) //O(E)
            {
                mean += edge[i].weight; //θ(1)
            }
            mean = mean / (edge.Count); //θ(1)
            return mean;
        }

        //returns index of edge with the max weight  
        public int CalculateStd (double mean) //O(E) 
        {
            double std; //θ(1)
            double stdMax = 0; //θ(1)
            int Removedindex = 0; //θ(1)
            double sum = 0; //θ(1)
            for (int i = 0; i < edge.Count; i++) //O(E)
            {
                std = (edge[i].weight - mean) * (edge[i].weight - mean); //θ(1)
                if (std > stdMax) //θ(1)
                {
                    stdMax = std; //θ(1)
                    Removedindex = i; //θ(1)
                }
                sum += std; //θ(1)
            }
            currentstd = sum / (edge.Count - 1); //θ(1)
            currentstd = Math.Sqrt(currentstd); //θ(1)
            return Removedindex;
        }
        public void DetectK() //O(E^2)
        {
            edge = edges.ToList(); //θ(E)
            double mean = getMean(); //O(E)
            int removed = CalculateStd(mean); //O(E)
            while (Math.Abs(currentstd - prevstd) > 0.0001) //O(E^2)
            {
                edge.RemoveAt(removed); //O(E)
                prevstd = currentstd; //θ(1)
                mean = getMean(); //O(E)
                removed = CalculateStd(mean); //O(E)
                K++; //θ(1)
            } 
        }
    }
}
