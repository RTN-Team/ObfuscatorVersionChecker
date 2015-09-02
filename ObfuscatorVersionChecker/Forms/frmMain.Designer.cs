namespace ObfuscatorVersionChecker
{
    partial class frmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonStartAll = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderThread = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timerGraph = new System.Windows.Forms.Timer(this.components);
            this.lineGraphCheck = new LineGraph();
            this.itemProgress = new System.Windows.Forms.ProgressBar();
            this.lineGraphSpeed = new LineGraph();
            this.SuspendLayout();
            // 
            // buttonStartAll
            // 
            this.buttonStartAll.Location = new System.Drawing.Point(12, 201);
            this.buttonStartAll.Name = "buttonStartAll";
            this.buttonStartAll.Size = new System.Drawing.Size(496, 23);
            this.buttonStartAll.TabIndex = 1;
            this.buttonStartAll.Text = "Start";
            this.buttonStartAll.UseVisualStyleBackColor = true;
            this.buttonStartAll.Click += new System.EventHandler(this.buttonStartAll_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTime,
            this.columnHeaderThread,
            this.columnHeaderMessage});
            this.listView1.Location = new System.Drawing.Point(12, 230);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(496, 132);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderTime
            // 
            this.columnHeaderTime.Text = "Time";
            this.columnHeaderTime.Width = 87;
            // 
            // columnHeaderThread
            // 
            this.columnHeaderThread.Text = "Thread";
            this.columnHeaderThread.Width = 157;
            // 
            // columnHeaderMessage
            // 
            this.columnHeaderMessage.Text = "Message";
            this.columnHeaderMessage.Width = 222;
            // 
            // timerGraph
            // 
            this.timerGraph.Interval = 1000;
            this.timerGraph.Tick += new System.EventHandler(this.timerGraph_Tick);
            // 
            // lineGraphCheck
            // 
            this.lineGraphCheck.DataPointDensity = 5;
            this.lineGraphCheck.GraphColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(121)))), ((int)(((byte)(192)))));
            this.lineGraphCheck.GridLineDensity = 2;
            this.lineGraphCheck.HighlightDataPoint = true;
            this.lineGraphCheck.Location = new System.Drawing.Point(12, 12);
            this.lineGraphCheck.Name = "lineGraphCheck";
            this.lineGraphCheck.ShowAverageBar = false;
            this.lineGraphCheck.Size = new System.Drawing.Size(496, 54);
            this.lineGraphCheck.TabIndex = 0;
            this.lineGraphCheck.Text = "lineGraphCheck";
            // 
            // itemProgress
            // 
            this.itemProgress.Location = new System.Drawing.Point(13, 172);
            this.itemProgress.Name = "itemProgress";
            this.itemProgress.Size = new System.Drawing.Size(495, 23);
            this.itemProgress.TabIndex = 3;
            // 
            // lineGraphSpeed
            // 
            this.lineGraphSpeed.DataPointDensity = 5;
            this.lineGraphSpeed.GraphColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(121)))), ((int)(((byte)(192)))));
            this.lineGraphSpeed.GridLineDensity = 2;
            this.lineGraphSpeed.HighlightDataPoint = true;
            this.lineGraphSpeed.Location = new System.Drawing.Point(13, 126);
            this.lineGraphSpeed.Name = "lineGraphSpeed";
            this.lineGraphSpeed.ShowAverageBar = true;
            this.lineGraphSpeed.Size = new System.Drawing.Size(495, 40);
            this.lineGraphSpeed.TabIndex = 4;
            this.lineGraphSpeed.Text = "lineGraph1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 374);
            this.Controls.Add(this.lineGraphSpeed);
            this.Controls.Add(this.itemProgress);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonStartAll);
            this.Controls.Add(this.lineGraphCheck);
            this.Name = "frmMain";
            this.Text = "ObfuscatorVersionChecker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private LineGraph lineGraphCheck;
        private System.Windows.Forms.Button buttonStartAll;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.ColumnHeader columnHeaderThread;
        private System.Windows.Forms.ColumnHeader columnHeaderMessage;
        private System.Windows.Forms.Timer timerGraph;
        private System.Windows.Forms.ProgressBar itemProgress;
        private LineGraph lineGraphSpeed;
    }
}

