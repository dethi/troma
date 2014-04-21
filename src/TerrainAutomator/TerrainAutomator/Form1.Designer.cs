namespace TerrainAutomator
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileHeighmap = new System.Windows.Forms.OpenFileDialog();
            this.openFileTexture = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.loadHeightmap = new System.Windows.Forms.Button();
            this.loadTexture = new System.Windows.Forms.Button();
            this.pathHeightmap = new System.Windows.Forms.TextBox();
            this.pathTexture = new System.Windows.Forms.TextBox();
            this.nbTerrain = new System.Windows.Forms.NumericUpDown();
            this.labelNbTerrain = new System.Windows.Forms.Label();
            this.generate = new System.Windows.Forms.Button();
            this.backgroundWorkerHeightmap = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerTexture = new System.ComponentModel.BackgroundWorker();
            this.comboHeighmapSize = new System.Windows.Forms.ComboBox();
            this.labelHeighmapSize = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbTerrain)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileHeighmap
            // 
            this.openFileHeighmap.FileName = "heightmap";
            // 
            // openFileTexture
            // 
            this.openFileTexture.FileName = "texture";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 189);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(389, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // loadHeightmap
            // 
            this.loadHeightmap.Location = new System.Drawing.Point(15, 13);
            this.loadHeightmap.Name = "loadHeightmap";
            this.loadHeightmap.Size = new System.Drawing.Size(115, 23);
            this.loadHeightmap.TabIndex = 1;
            this.loadHeightmap.Text = "Load heighmap";
            this.loadHeightmap.UseVisualStyleBackColor = true;
            this.loadHeightmap.Click += new System.EventHandler(this.loadHeightmap_Click);
            // 
            // loadTexture
            // 
            this.loadTexture.Location = new System.Drawing.Point(15, 39);
            this.loadTexture.Name = "loadTexture";
            this.loadTexture.Size = new System.Drawing.Size(115, 23);
            this.loadTexture.TabIndex = 2;
            this.loadTexture.Text = "Load texture";
            this.loadTexture.UseVisualStyleBackColor = true;
            this.loadTexture.Click += new System.EventHandler(this.loadTexture_Click);
            // 
            // pathHeightmap
            // 
            this.pathHeightmap.AllowDrop = true;
            this.pathHeightmap.Location = new System.Drawing.Point(146, 15);
            this.pathHeightmap.Name = "pathHeightmap";
            this.pathHeightmap.Size = new System.Drawing.Size(235, 20);
            this.pathHeightmap.TabIndex = 3;
            // 
            // pathTexture
            // 
            this.pathTexture.AllowDrop = true;
            this.pathTexture.Location = new System.Drawing.Point(146, 41);
            this.pathTexture.Name = "pathTexture";
            this.pathTexture.Size = new System.Drawing.Size(235, 20);
            this.pathTexture.TabIndex = 4;
            // 
            // nbTerrain
            // 
            this.nbTerrain.Location = new System.Drawing.Point(146, 94);
            this.nbTerrain.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nbTerrain.Name = "nbTerrain";
            this.nbTerrain.Size = new System.Drawing.Size(57, 20);
            this.nbTerrain.TabIndex = 5;
            this.nbTerrain.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelNbTerrain
            // 
            this.labelNbTerrain.AutoSize = true;
            this.labelNbTerrain.Location = new System.Drawing.Point(18, 96);
            this.labelNbTerrain.Name = "labelNbTerrain";
            this.labelNbTerrain.Size = new System.Drawing.Size(122, 13);
            this.labelNbTerrain.TabIndex = 6;
            this.labelNbTerrain.Text = "Number of side (square):";
            // 
            // generate
            // 
            this.generate.Location = new System.Drawing.Point(139, 144);
            this.generate.Name = "generate";
            this.generate.Size = new System.Drawing.Size(115, 23);
            this.generate.TabIndex = 7;
            this.generate.Text = "Generate";
            this.generate.UseVisualStyleBackColor = true;
            this.generate.Click += new System.EventHandler(this.generate_Click);
            // 
            // backgroundWorkerHeightmap
            // 
            this.backgroundWorkerHeightmap.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerHeightmap_RunWorkerCompleted);
            // 
            // backgroundWorkerTexture
            // 
            this.backgroundWorkerTexture.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerTexture_RunWorkerCompleted);
            // 
            // comboHeighmapSize
            // 
            this.comboHeighmapSize.FormattingEnabled = true;
            this.comboHeighmapSize.Items.AddRange(new object[] {
            "129",
            "257",
            "513",
            "1025"});
            this.comboHeighmapSize.Location = new System.Drawing.Point(146, 67);
            this.comboHeighmapSize.Name = "comboHeighmapSize";
            this.comboHeighmapSize.Size = new System.Drawing.Size(57, 21);
            this.comboHeighmapSize.TabIndex = 8;
            this.comboHeighmapSize.Text = "513";
            this.comboHeighmapSize.TextChanged += new System.EventHandler(this.comboHeighmapSize_TextChanged);
            // 
            // labelHeighmapSize
            // 
            this.labelHeighmapSize.AutoSize = true;
            this.labelHeighmapSize.Location = new System.Drawing.Point(61, 70);
            this.labelHeighmapSize.Name = "labelHeighmapSize";
            this.labelHeighmapSize.Size = new System.Drawing.Size(79, 13);
            this.labelHeighmapSize.TabIndex = 9;
            this.labelHeighmapSize.Text = "Heighmap size:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 211);
            this.Controls.Add(this.labelHeighmapSize);
            this.Controls.Add(this.comboHeighmapSize);
            this.Controls.Add(this.generate);
            this.Controls.Add(this.labelNbTerrain);
            this.Controls.Add(this.nbTerrain);
            this.Controls.Add(this.pathTexture);
            this.Controls.Add(this.pathHeightmap);
            this.Controls.Add(this.loadTexture);
            this.Controls.Add(this.loadHeightmap);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Terrain Automator";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbTerrain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileHeighmap;
        private System.Windows.Forms.OpenFileDialog openFileTexture;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Windows.Forms.Button loadHeightmap;
        private System.Windows.Forms.Button loadTexture;
        private System.Windows.Forms.TextBox pathHeightmap;
        private System.Windows.Forms.TextBox pathTexture;
        private System.Windows.Forms.NumericUpDown nbTerrain;
        private System.Windows.Forms.Label labelNbTerrain;
        private System.Windows.Forms.Button generate;
        private System.ComponentModel.BackgroundWorker backgroundWorkerHeightmap;
        private System.ComponentModel.BackgroundWorker backgroundWorkerTexture;
        private System.Windows.Forms.ComboBox comboHeighmapSize;
        private System.Windows.Forms.Label labelHeighmapSize;
    }
}

