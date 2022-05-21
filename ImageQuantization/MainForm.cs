using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);//
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void btnQuantization_Click(object sender, EventArgs e)
        {          
            if (txtK.Text.ToString().Length > 0)
            {
                long StartTime = Environment.TickCount;
                long DistinctColorsCount = Graph.GetDistinctColors(ImageMatrix);
                //Graph.ConstructGraph();
                double MSTsum = MST.MinimumSpanningTree(Graph.DistinctColors);
                int K = int.Parse(txtK.Text);
                SinglelinkageClustering clustering = new SinglelinkageClustering(K, MST.MSTEdges);
                clustering.extract_cluster();
                clustering.representative_color();
                ImageMatrix = clustering.ReplaceColors(ImageMatrix, clustering.Palette);
                long EndTime = Environment.TickCount;
                MessageBox.Show("# Colors in palette = " + clustering.Palette.Count.ToString());
                txtDistinctColors.Text = DistinctColorsCount.ToString();
                txtMSTSum.Text = MSTsum.ToString();
                txtTimeInSec.Text = ((EndTime - StartTime) / 1000).ToString();
                txtTimeInMS.Text = (EndTime - StartTime).ToString();
                ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            }
            else
            {
                long StartTime = Environment.TickCount;
                long DistinctColorsCount = Graph.GetDistinctColors(ImageMatrix);
                //Graph.ConstructGraph();
                double MSTsum = MST.MinimumSpanningTree(Graph.DistinctColors);
                DetectKClusters c = new DetectKClusters(MST.MSTEdges);
                c.DetectK();
                int K= c.K;
                SinglelinkageClustering clustering = new SinglelinkageClustering(K, MST.MSTEdges);
                clustering.extract_cluster();
                clustering.representative_color();
                ImageMatrix = clustering.ReplaceColors(ImageMatrix, clustering.Palette);
                long EndTime = Environment.TickCount;
                MessageBox.Show("# Colors in palette = " + clustering.Palette.Count.ToString()+" ,Detected K = "+ K.ToString());
                txtDistinctColors.Text = DistinctColorsCount.ToString();
                txtMSTSum.Text = MSTsum.ToString();
                txtTimeInSec.Text = ((EndTime - StartTime) / 1000).ToString();
                txtTimeInMS.Text = (EndTime - StartTime).ToString();
                ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            }
           
        }
    }
}