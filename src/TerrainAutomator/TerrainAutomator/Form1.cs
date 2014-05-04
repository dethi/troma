using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TerrainAutomator
{
    public partial class Form1 : Form
    {
        Image heightmap;
        Image texture;
        int heightmapSize;

        public Form1()
        {
            InitializeComponent();
            heightmapSize = 513;
            backgroundWorkerHeightmap.DoWork += new DoWorkEventHandler(backgroundWorkerHeightmap_DoWork);
            backgroundWorkerTexture.DoWork += new DoWorkEventHandler(backgroundWorkerTexture_DoWork);
        }

        private void loadHeightmap_Click(object sender, EventArgs e)
        {
            if (openFileHeighmap.ShowDialog() == DialogResult.OK)
            {
                heightmap = Image.FromFile(openFileHeighmap.FileName);
                pathHeightmap.Text = openFileHeighmap.FileName;
            }
        }

        private void loadTexture_Click(object sender, EventArgs e)
        {
            if (openFileTexture.ShowDialog() == DialogResult.OK)
            {
                texture = Image.FromFile(openFileTexture.FileName);
                pathTexture.Text = openFileTexture.FileName;
            }
        }

        private void generate_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "";

            if (heightmap != null && !backgroundWorkerHeightmap.IsBusy)
            {
                loadHeightmap.Enabled = false;
                backgroundWorkerHeightmap.RunWorkerAsync();
            }

            if (texture != null && !backgroundWorkerTexture.IsBusy)
            {
                loadTexture.Enabled = false;
                backgroundWorkerTexture.RunWorkerAsync();
            }
        }

        void backgroundWorkerHeightmap_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Image> croppedImage = new List<Image>();
            Image new_img;
            int _size = heightmap.Width / (int)nbTerrain.Value;

            for (int i = 0; i < nbTerrain.Value; i++)
            {
                for (int j = 0; j < nbTerrain.Value; j++)
                {
                    croppedImage.Add(ProcessImage.CropImage(heightmap,
                        new Rectangle(_size * i, _size * j, _size, _size)));
                }
            }

            int k = 0;

            foreach (Image img in croppedImage)
            {
                new_img = ProcessImage.resizeImage(img, new Size(heightmapSize, heightmapSize));
                new_img.Save(String.Format("{0}({1},{2}).png", openFileHeighmap.FileName.Split('.')[0],
                    k % nbTerrain.Value, (int)(k / nbTerrain.Value)));
                k++;
            }
        }

        void backgroundWorkerTexture_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Image> croppedImage = new List<Image>();
            int _size = texture.Width / (int)nbTerrain.Value;

            for (int i = 0; i < nbTerrain.Value; i++)
            {
                for (int j = 0; j < nbTerrain.Value; j++)
                {
                    croppedImage.Add(ProcessImage.CropImage(texture,
                        new Rectangle(_size * i, _size * j, _size, _size)));
                }
            }

            int k = 0;

            foreach (Image img in croppedImage)
            {
                img.Save(String.Format("{0}({1},{2}).png", openFileTexture.FileName.Split('.')[0],
                    k % nbTerrain.Value, (int)(k / nbTerrain.Value)));
                k++;
            }
        }

        private void backgroundWorkerHeightmap_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadHeightmap.Enabled = true;
            labelStatus.Text += "Heightmap: completed    ";
        }

        private void backgroundWorkerTexture_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadTexture.Enabled = true;
            labelStatus.Text += "Texture: completed    ";
        }

        private void comboHeighmapSize_TextChanged(object sender, EventArgs e)
        {
            try
            {
                heightmapSize = Convert.ToInt32(comboHeighmapSize.Text);
            }
            catch
            {
                MessageBox.Show("Invalid value!");
            }
        }
    }
}
