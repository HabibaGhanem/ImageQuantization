using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    public struct edge
    {
        public RGBPixel C1;
        public RGBPixel C2;
        public double weight;
      
    }
    class Graph
    {
        public static edge[] edges; //θ(1)
        public static List<RGBPixel> DistinctColors; //θ(1)
       
        /// <summary>
        /// Find the distinct colors from the input image and adding them to a list
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>number of distinct colors</returns>
        public static int GetDistinctColors(RGBPixel[,] ImageMatrix) // θ(N*M)
        {
            DistinctColors = new List<RGBPixel>(); //θ(1)
            bool[,,] isExist = new bool[256, 256, 256]; //θ(1)              
            int Height = ImageOperations.GetHeight(ImageMatrix); //θ(1)
            int Width = ImageOperations.GetWidth(ImageMatrix);   //θ(1)
            for (int i = 0; i < Height; i++) // N * θ(M) -> θ(N*M)
            {
                for (int j = 0; j < Width; j++) // M * θ(1) -> θ(M)
                {
                    if (!isExist[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue]) //θ(1)
                    {
                        DistinctColors.Add(ImageMatrix[i, j]); //θ(1)
                        isExist[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue] = true;//θ(1)
                    }                   
                }
            }
            return DistinctColors.Count; //θ(1)
        }
        /// <summary>
        /// Calculate the Euclidean Distance between the RGB values of the 2 vertices
        /// </summary>
        /// <param name="C1">first vertex(color)</param>
        /// <param name="C2">second vertex(color) </param>
        /// <returns>the Distance</returns>
        public static double EuclideanDistance(RGBPixel C1, RGBPixel C2) //θ(1)
        {
            int r = (C1.red - C2.red) * (C1.red - C2.red); //θ(1)
            int g = (C1.green - C2.green) * (C1.green - C2.green); //θ(1)
            int b = (C1.blue - C2.blue) * (C1.blue - C2.blue); //θ(1)
            double distance = Math.Sqrt(r + g + b); //θ(1) 
            return distance;
        }
        public static void ConstructGraph() //O(V^2)
        {
            int V = DistinctColors.Count; //VerticesCount  //θ(1) 
            
            int E = (V*(V - 1)) / 2;       //EdgesCount     //θ(1)
            edges = new edge[E];  //θ(1)
            int EdgesCounter = 0;  //θ(1)      
            
            for (int i = 0; i < V; i++)    // V * O(V) -> O(V^2)
            {
                for(int j = i + 1; j < V; j++)  //O(V) 
                {
                    edges[EdgesCounter].C1 = DistinctColors[i];  //θ(1)
                    edges[EdgesCounter].C2 = DistinctColors[j];   //θ(1)
                    edges[EdgesCounter].weight = EuclideanDistance(edges[EdgesCounter].C1, edges[EdgesCounter].C2);//θ(1)

                    EdgesCounter++;  //θ(1)
                }
            }
            
        }
    }
}
