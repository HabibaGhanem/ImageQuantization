using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    public class SinglelinkageClustering
    {
        int k;  //θ(1)
        bool[] visit;    //θ(1)  
        Edge [] edges;  //θ(1)
        public List<int>[] list;  //θ(1)
        public List<RGBPixel> Palette; //θ(1)
        public List<List<int>> cluster = new List<List<int>>(); //θ(1)
        public RGBPixel[,] Quantized_Image; //θ(1)
        public SinglelinkageClustering(int k, Edge [] edges) //θ(D)
        {
            this.k = k; //θ(1)
            this.edges = edges; //θ(1)
            list = new List<int>[Graph.DistinctColors.Count]; //θ(1)
            for (int i = 0; i < Graph.DistinctColors.Count; i++) //D * θ(1) -> θ(D)
            {
                list[i] = new List<int>(Graph.DistinctColors.Count); //θ(1)
            }
        }
        public void extract_cluster() //θ(K*D)
        {
            Palette = new List<RGBPixel>(); //θ(1)
            visit = new bool[Graph.DistinctColors.Count]; //θ(1)
            for (int i = 0; i < k - 1; i++) //K * θ(D) -> θ(K*D)
            {
                double max = 0; //θ(1)
                int index = 0; //θ(1)
                for (int j = 0; j < Graph.DistinctColors.Count; j++) //D * θ(1) -> θ(D)
                {
                    if (edges[j].weight > max) //θ(1)
                    {
                        max = edges[j].weight; //θ(1)
                        index = j; //θ(1)
                    }
                }
                edges[index].weight = edges[index].weight*( -1); //θ(1)
                edges[index].From = -1;  //θ(1)
            }
            int counter = 0;  //θ(1)
            while (counter < Graph.DistinctColors.Count) //D * θ(1) -> θ(D)
            {
                if (edges[counter].From != -1) //θ(1)
                {
                    list[counter].Add(edges[counter].From); //θ(1)
                    list[edges[counter].From].Add(counter); //θ(1)
                }
                counter++; //θ(1)
            }

            int counter1 = 0; //θ(1)
            while (counter1 < Graph.DistinctColors.Count) //θ(E) 
            {     
                List<int> merge = new List<int>(Graph.DistinctColors.Count); //θ(1)
                if (visit[counter1] == false) //θ(1)
                {
                    DFS(list, counter1, merge); // DFS called once per vertex ,touch each vertex takes θ(V)
                                                // DFS body (visiting adjacent vertices of u) takes θ(adj[u])
                                                // Total -> θ(E)
                    cluster.Add(merge);  //θ(1)
                }
                counter1++;  //θ(1)
            }

        }
        public void representative_color()  //θ(D)
        {
            Palette = new List<RGBPixel>();  //θ(1)
            RGBPixel color = new RGBPixel();  //θ(1)
            int cluster_size = cluster.Count; //θ(1)
            for (int k = 0; k < cluster_size; k++) //Total -> θ(D)
            {
                int g = 0, b = 0, r = 0; //θ(1)
                for (int n = 0; n < cluster[k].Count; n++)
                {
                    r = Graph.DistinctColors[cluster[k][n]].red + r; //θ(1)
                    b = Graph.DistinctColors[cluster[k][n]].blue + b; //θ(1)
                    g = Graph.DistinctColors[cluster[k][n]].green + g; //θ(1)
                }
                int size = cluster[k].Count; //θ(1)
                r = r / size; //θ(1)
                b = b / size; //θ(1)
                g = g / size; //θ(1)
                color.red = (byte)r; //θ(1)
                color.blue = (byte)b; //θ(1)
                color.green = (byte)g; //θ(1)
                Palette.Add(color); //θ(1)
            }
        }
        public void DFS(List<int>[] adj, int u, List<int> merge)
        {

            merge.Add(u);
            visit[u] = true;

            for (int i = 0; i < list[u].Count; i++) 
            {
                if (!visit[adj[u][i]])
                {
                    DFS(adj, adj[u][i], merge);
                }
            }
        }

        public RGBPixel[,] ReplaceColors (RGBPixel[,] ImageMatrix, List<RGBPixel> Palette) // θ(N*M)
        {
            RGBPixel [,,] DistinctColorsAfter = new RGBPixel[256, 256, 256];  //θ(1)
            for (int i = 0; i < cluster.Count; i++) //Total -> θ(D)
            {
                for (int j = 0; j < cluster[i].Count; j++)
                {
                    DistinctColorsAfter[Graph.DistinctColors[cluster[i][j]].red, 
                        Graph.DistinctColors[cluster[i][j]].blue,
                        Graph.DistinctColors[cluster[i][j]].green] = Palette[i]; //θ(1)
                }
            }
            for(int i = 0; i < ImageOperations.GetHeight(ImageMatrix); i++) //N * θ(M) -> θ(N*M)
            {
                for(int j = 0; j < ImageOperations.GetWidth(ImageMatrix); j++) //M * θ(1) -> θ(M)
                {
                    ImageMatrix[i,j]= DistinctColorsAfter[ImageMatrix[i, j].red, ImageMatrix[i, j].blue,
                        ImageMatrix[i, j].green]; //θ(1)
                }
            }
            return ImageMatrix;
        }

    }
}
