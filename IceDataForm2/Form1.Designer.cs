namespace IceDataForm2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.fileSelectionTab = new System.Windows.Forms.TabPage();
            this.TrainFile = new System.Windows.Forms.TextBox();
            this.DecreasingSimulationFile = new System.Windows.Forms.TextBox();
            this.IncreasingSimulationFile = new System.Windows.Forms.TextBox();
            this.GeometryFile = new System.Windows.Forms.TextBox();
            this.IceDataFile = new System.Windows.Forms.TextBox();
            this.includeAListOfTrainsToExclude = new System.Windows.Forms.CheckBox();
            this.selectTrainList = new System.Windows.Forms.Button();
            this.selectDecreasingSimulation = new System.Windows.Forms.Button();
            this.selectIncreasingSimulation = new System.Windows.Forms.Button();
            this.selectGeomtryFile = new System.Windows.Forms.Button();
            this.selectDataFile = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.simulationTab = new System.Windows.Forms.TabPage();
            this.overpoweredDecreasingP2W = new System.Windows.Forms.Label();
            this.overpoweredIncreasingP2W = new System.Windows.Forms.Label();
            this.underpoweredDecreasingP2W = new System.Windows.Forms.Label();
            this.SimOverpoweredLabel = new System.Windows.Forms.Label();
            this.SimUnderpoweredLabel = new System.Windows.Forms.Label();
            this.underpoweredIncreasingP2W = new System.Windows.Forms.Label();
            this.SimDecreasingLabel = new System.Windows.Forms.Label();
            this.SimIncreasingLabel = new System.Windows.Forms.Label();
            this.averagePowerToWeightRatios = new System.Windows.Forms.Button();
            this.overpoweredUpperBound = new System.Windows.Forms.TextBox();
            this.overpoweredLowerBound = new System.Windows.Forms.TextBox();
            this.underpoweredUpperBound = new System.Windows.Forms.TextBox();
            this.underpoweredLowerBound = new System.Windows.Forms.TextBox();
            this.UpperBoundLabel = new System.Windows.Forms.Label();
            this.LowerBoundLabel = new System.Windows.Forms.Label();
            this.PowerToWeightLabel = new System.Windows.Forms.Label();
            this.overpoweredLabel = new System.Windows.Forms.Label();
            this.underpoweredLabel = new System.Windows.Forms.Label();
            this.DataFileLabel = new System.Windows.Forms.Label();
            this.simIceDataFile = new System.Windows.Forms.TextBox();
            this.processingTab = new System.Windows.Forms.TabPage();
            this.Execute = new System.Windows.Forms.Button();
            this.TSRWindowBoundary = new System.Windows.Forms.TextBox();
            this.timeSeparation = new System.Windows.Forms.TextBox();
            this.loopSpeedFactor = new System.Windows.Forms.TextBox();
            this.distanceThreshold = new System.Windows.Forms.TextBox();
            this.loopBoundary = new System.Windows.Forms.TextBox();
            this.minimumJourneyDistance = new System.Windows.Forms.TextBox();
            this.interpolationInterval = new System.Windows.Forms.TextBox();
            this.endInterpolationKm = new System.Windows.Forms.TextBox();
            this.startInterpolationKm = new System.Windows.Forms.TextBox();
            this.tsrLabel = new System.Windows.Forms.Label();
            this.loopSpeedLabel = new System.Windows.Forms.Label();
            this.loopBoundaryLabel = new System.Windows.Forms.Label();
            this.endInterpolationLabel = new System.Windows.Forms.Label();
            this.timeSeparationLabel = new System.Windows.Forms.Label();
            this.tiemSeparationLabel = new System.Windows.Forms.Label();
            this.minDistanceLabel = new System.Windows.Forms.Label();
            this.IntervalLabel = new System.Windows.Forms.Label();
            this.startInterpolationLabel = new System.Windows.Forms.Label();
            this.toLongitude = new System.Windows.Forms.TextBox();
            this.toLatitude = new System.Windows.Forms.TextBox();
            this.toLongitudeLabel = new System.Windows.Forms.Label();
            this.toLatitudeLabel = new System.Windows.Forms.Label();
            this.bottomRightLabel = new System.Windows.Forms.Label();
            this.fromLongitude = new System.Windows.Forms.TextBox();
            this.fromLatitude = new System.Windows.Forms.TextBox();
            this.fromLongitudeLabel = new System.Windows.Forms.Label();
            this.fromLatitudeLabel = new System.Windows.Forms.Label();
            this.TopLeftLabel = new System.Windows.Forms.Label();
            this.ConfinementLabel = new System.Windows.Forms.Label();
            this.ToDateLabel = new System.Windows.Forms.Label();
            this.FromDateLabel = new System.Windows.Forms.Label();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.DateRangeLabel = new System.Windows.Forms.Label();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.label1 = new System.Windows.Forms.Label();
            this.combinedIncreasingP2W = new System.Windows.Forms.Label();
            this.combinedDecreasingP2W = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.fileSelectionTab.SuspendLayout();
            this.simulationTab.SuspendLayout();
            this.processingTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.fileSelectionTab);
            this.tabControl1.Controls.Add(this.simulationTab);
            this.tabControl1.Controls.Add(this.processingTab);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(869, 427);
            this.tabControl1.TabIndex = 0;
            // 
            // fileSelectionTab
            // 
            this.fileSelectionTab.Controls.Add(this.TrainFile);
            this.fileSelectionTab.Controls.Add(this.DecreasingSimulationFile);
            this.fileSelectionTab.Controls.Add(this.IncreasingSimulationFile);
            this.fileSelectionTab.Controls.Add(this.GeometryFile);
            this.fileSelectionTab.Controls.Add(this.IceDataFile);
            this.fileSelectionTab.Controls.Add(this.includeAListOfTrainsToExclude);
            this.fileSelectionTab.Controls.Add(this.selectTrainList);
            this.fileSelectionTab.Controls.Add(this.selectDecreasingSimulation);
            this.fileSelectionTab.Controls.Add(this.selectIncreasingSimulation);
            this.fileSelectionTab.Controls.Add(this.selectGeomtryFile);
            this.fileSelectionTab.Controls.Add(this.selectDataFile);
            this.fileSelectionTab.Controls.Add(this.menuStrip1);
            this.fileSelectionTab.Location = new System.Drawing.Point(4, 22);
            this.fileSelectionTab.Name = "fileSelectionTab";
            this.fileSelectionTab.Padding = new System.Windows.Forms.Padding(3);
            this.fileSelectionTab.Size = new System.Drawing.Size(861, 401);
            this.fileSelectionTab.TabIndex = 0;
            this.fileSelectionTab.Text = "File Selection";
            this.fileSelectionTab.UseVisualStyleBackColor = true;
            // 
            // TrainFile
            // 
            this.TrainFile.Location = new System.Drawing.Point(253, 286);
            this.TrainFile.Name = "TrainFile";
            this.TrainFile.Size = new System.Drawing.Size(550, 20);
            this.TrainFile.TabIndex = 11;
            // 
            // DecreasingSimulationFile
            // 
            this.DecreasingSimulationFile.Location = new System.Drawing.Point(253, 213);
            this.DecreasingSimulationFile.Name = "DecreasingSimulationFile";
            this.DecreasingSimulationFile.Size = new System.Drawing.Size(550, 20);
            this.DecreasingSimulationFile.TabIndex = 10;
            // 
            // IncreasingSimulationFile
            // 
            this.IncreasingSimulationFile.Location = new System.Drawing.Point(253, 153);
            this.IncreasingSimulationFile.Name = "IncreasingSimulationFile";
            this.IncreasingSimulationFile.Size = new System.Drawing.Size(550, 20);
            this.IncreasingSimulationFile.TabIndex = 9;
            // 
            // GeometryFile
            // 
            this.GeometryFile.Location = new System.Drawing.Point(253, 88);
            this.GeometryFile.Name = "GeometryFile";
            this.GeometryFile.Size = new System.Drawing.Size(550, 20);
            this.GeometryFile.TabIndex = 8;
            // 
            // IceDataFile
            // 
            this.IceDataFile.Location = new System.Drawing.Point(253, 33);
            this.IceDataFile.Name = "IceDataFile";
            this.IceDataFile.Size = new System.Drawing.Size(550, 20);
            this.IceDataFile.TabIndex = 7;
            // 
            // includeAListOfTrainsToExclude
            // 
            this.includeAListOfTrainsToExclude.AutoSize = true;
            this.includeAListOfTrainsToExclude.Location = new System.Drawing.Point(31, 315);
            this.includeAListOfTrainsToExclude.Name = "includeAListOfTrainsToExclude";
            this.includeAListOfTrainsToExclude.Size = new System.Drawing.Size(118, 17);
            this.includeAListOfTrainsToExclude.TabIndex = 5;
            this.includeAListOfTrainsToExclude.Text = "Exclude trains in list";
            this.includeAListOfTrainsToExclude.UseVisualStyleBackColor = true;
            // 
            // selectTrainList
            // 
            this.selectTrainList.Location = new System.Drawing.Point(31, 281);
            this.selectTrainList.Name = "selectTrainList";
            this.selectTrainList.Size = new System.Drawing.Size(163, 28);
            this.selectTrainList.TabIndex = 4;
            this.selectTrainList.Text = "Select Train List File";
            this.selectTrainList.UseVisualStyleBackColor = true;
            this.selectTrainList.Click += new System.EventHandler(this.selectTrainList_Click);
            // 
            // selectDecreasingSimulation
            // 
            this.selectDecreasingSimulation.Location = new System.Drawing.Point(31, 208);
            this.selectDecreasingSimulation.Name = "selectDecreasingSimulation";
            this.selectDecreasingSimulation.Size = new System.Drawing.Size(163, 28);
            this.selectDecreasingSimulation.TabIndex = 3;
            this.selectDecreasingSimulation.Text = "Select Decreasing Simulation";
            this.selectDecreasingSimulation.UseVisualStyleBackColor = true;
            this.selectDecreasingSimulation.Click += new System.EventHandler(this.selectDecreasingSimulationFile_Click);
            // 
            // selectIncreasingSimulation
            // 
            this.selectIncreasingSimulation.Location = new System.Drawing.Point(31, 148);
            this.selectIncreasingSimulation.Name = "selectIncreasingSimulation";
            this.selectIncreasingSimulation.Size = new System.Drawing.Size(163, 28);
            this.selectIncreasingSimulation.TabIndex = 2;
            this.selectIncreasingSimulation.Text = "Select Increasing Simulation File";
            this.selectIncreasingSimulation.UseVisualStyleBackColor = true;
            this.selectIncreasingSimulation.Click += new System.EventHandler(this.selectIncreasingSimulationFile_Click);
            // 
            // selectGeomtryFile
            // 
            this.selectGeomtryFile.Location = new System.Drawing.Point(31, 83);
            this.selectGeomtryFile.Name = "selectGeomtryFile";
            this.selectGeomtryFile.Size = new System.Drawing.Size(163, 28);
            this.selectGeomtryFile.TabIndex = 1;
            this.selectGeomtryFile.Text = "Select Geometry File";
            this.selectGeomtryFile.UseVisualStyleBackColor = true;
            this.selectGeomtryFile.Click += new System.EventHandler(this.selectGeometryFile_Click);
            // 
            // selectDataFile
            // 
            this.selectDataFile.Location = new System.Drawing.Point(31, 28);
            this.selectDataFile.Name = "selectDataFile";
            this.selectDataFile.Size = new System.Drawing.Size(163, 28);
            this.selectDataFile.TabIndex = 0;
            this.selectDataFile.Text = "Select Ice Data File";
            this.selectDataFile.UseVisualStyleBackColor = true;
            this.selectDataFile.Click += new System.EventHandler(this.selectDataFile_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(855, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // simulationTab
            // 
            this.simulationTab.Controls.Add(this.combinedDecreasingP2W);
            this.simulationTab.Controls.Add(this.combinedIncreasingP2W);
            this.simulationTab.Controls.Add(this.label1);
            this.simulationTab.Controls.Add(this.overpoweredDecreasingP2W);
            this.simulationTab.Controls.Add(this.overpoweredIncreasingP2W);
            this.simulationTab.Controls.Add(this.underpoweredDecreasingP2W);
            this.simulationTab.Controls.Add(this.SimOverpoweredLabel);
            this.simulationTab.Controls.Add(this.SimUnderpoweredLabel);
            this.simulationTab.Controls.Add(this.underpoweredIncreasingP2W);
            this.simulationTab.Controls.Add(this.SimDecreasingLabel);
            this.simulationTab.Controls.Add(this.SimIncreasingLabel);
            this.simulationTab.Controls.Add(this.averagePowerToWeightRatios);
            this.simulationTab.Controls.Add(this.overpoweredUpperBound);
            this.simulationTab.Controls.Add(this.overpoweredLowerBound);
            this.simulationTab.Controls.Add(this.underpoweredUpperBound);
            this.simulationTab.Controls.Add(this.underpoweredLowerBound);
            this.simulationTab.Controls.Add(this.UpperBoundLabel);
            this.simulationTab.Controls.Add(this.LowerBoundLabel);
            this.simulationTab.Controls.Add(this.PowerToWeightLabel);
            this.simulationTab.Controls.Add(this.overpoweredLabel);
            this.simulationTab.Controls.Add(this.underpoweredLabel);
            this.simulationTab.Controls.Add(this.DataFileLabel);
            this.simulationTab.Controls.Add(this.simIceDataFile);
            this.simulationTab.Location = new System.Drawing.Point(4, 22);
            this.simulationTab.Name = "simulationTab";
            this.simulationTab.Padding = new System.Windows.Forms.Padding(3);
            this.simulationTab.Size = new System.Drawing.Size(861, 401);
            this.simulationTab.TabIndex = 1;
            this.simulationTab.Text = "Simulation Parameters";
            this.simulationTab.UseVisualStyleBackColor = true;
            // 
            // overpoweredDecreasingP2W
            // 
            this.overpoweredDecreasingP2W.AutoSize = true;
            this.overpoweredDecreasingP2W.Location = new System.Drawing.Point(444, 314);
            this.overpoweredDecreasingP2W.Name = "overpoweredDecreasingP2W";
            this.overpoweredDecreasingP2W.Size = new System.Drawing.Size(13, 13);
            this.overpoweredDecreasingP2W.TabIndex = 21;
            this.overpoweredDecreasingP2W.Text = "0";
            // 
            // overpoweredIncreasingP2W
            // 
            this.overpoweredIncreasingP2W.AutoSize = true;
            this.overpoweredIncreasingP2W.Location = new System.Drawing.Point(216, 314);
            this.overpoweredIncreasingP2W.Name = "overpoweredIncreasingP2W";
            this.overpoweredIncreasingP2W.Size = new System.Drawing.Size(13, 13);
            this.overpoweredIncreasingP2W.TabIndex = 20;
            this.overpoweredIncreasingP2W.Text = "0";
            // 
            // underpoweredDecreasingP2W
            // 
            this.underpoweredDecreasingP2W.AutoSize = true;
            this.underpoweredDecreasingP2W.Location = new System.Drawing.Point(444, 283);
            this.underpoweredDecreasingP2W.Name = "underpoweredDecreasingP2W";
            this.underpoweredDecreasingP2W.Size = new System.Drawing.Size(13, 13);
            this.underpoweredDecreasingP2W.TabIndex = 19;
            this.underpoweredDecreasingP2W.Text = "0";
            // 
            // SimOverpoweredLabel
            // 
            this.SimOverpoweredLabel.AutoSize = true;
            this.SimOverpoweredLabel.Location = new System.Drawing.Point(49, 314);
            this.SimOverpoweredLabel.Name = "SimOverpoweredLabel";
            this.SimOverpoweredLabel.Size = new System.Drawing.Size(74, 13);
            this.SimOverpoweredLabel.TabIndex = 18;
            this.SimOverpoweredLabel.Text = "Overpowered:";
            // 
            // SimUnderpoweredLabel
            // 
            this.SimUnderpoweredLabel.AutoSize = true;
            this.SimUnderpoweredLabel.Location = new System.Drawing.Point(49, 283);
            this.SimUnderpoweredLabel.Name = "SimUnderpoweredLabel";
            this.SimUnderpoweredLabel.Size = new System.Drawing.Size(80, 13);
            this.SimUnderpoweredLabel.TabIndex = 17;
            this.SimUnderpoweredLabel.Text = "Underpowered:";
            // 
            // underpoweredIncreasingP2W
            // 
            this.underpoweredIncreasingP2W.AutoSize = true;
            this.underpoweredIncreasingP2W.Location = new System.Drawing.Point(216, 283);
            this.underpoweredIncreasingP2W.Name = "underpoweredIncreasingP2W";
            this.underpoweredIncreasingP2W.Size = new System.Drawing.Size(13, 13);
            this.underpoweredIncreasingP2W.TabIndex = 16;
            this.underpoweredIncreasingP2W.Text = "0";
            // 
            // SimDecreasingLabel
            // 
            this.SimDecreasingLabel.AutoSize = true;
            this.SimDecreasingLabel.Location = new System.Drawing.Point(444, 247);
            this.SimDecreasingLabel.Name = "SimDecreasingLabel";
            this.SimDecreasingLabel.Size = new System.Drawing.Size(106, 13);
            this.SimDecreasingLabel.TabIndex = 15;
            this.SimDecreasingLabel.Text = "Decreasing Direction";
            // 
            // SimIncreasingLabel
            // 
            this.SimIncreasingLabel.AutoSize = true;
            this.SimIncreasingLabel.Location = new System.Drawing.Point(216, 247);
            this.SimIncreasingLabel.Name = "SimIncreasingLabel";
            this.SimIncreasingLabel.Size = new System.Drawing.Size(104, 13);
            this.SimIncreasingLabel.TabIndex = 14;
            this.SimIncreasingLabel.Text = "Increasing Direction:";
            // 
            // averagePowerToWeightRatios
            // 
            this.averagePowerToWeightRatios.Location = new System.Drawing.Point(52, 202);
            this.averagePowerToWeightRatios.Name = "averagePowerToWeightRatios";
            this.averagePowerToWeightRatios.Size = new System.Drawing.Size(148, 46);
            this.averagePowerToWeightRatios.TabIndex = 11;
            this.averagePowerToWeightRatios.Text = "Simulation Power to Weight Ratios";
            this.averagePowerToWeightRatios.UseVisualStyleBackColor = true;
            this.averagePowerToWeightRatios.Click += new System.EventHandler(this.averagePowerToWeightRatios_Click);
            // 
            // overpoweredUpperBound
            // 
            this.overpoweredUpperBound.Location = new System.Drawing.Point(447, 141);
            this.overpoweredUpperBound.Name = "overpoweredUpperBound";
            this.overpoweredUpperBound.Size = new System.Drawing.Size(100, 20);
            this.overpoweredUpperBound.TabIndex = 10;
            this.overpoweredUpperBound.Text = "5";
            // 
            // overpoweredLowerBound
            // 
            this.overpoweredLowerBound.Location = new System.Drawing.Point(209, 141);
            this.overpoweredLowerBound.Name = "overpoweredLowerBound";
            this.overpoweredLowerBound.Size = new System.Drawing.Size(100, 20);
            this.overpoweredLowerBound.TabIndex = 9;
            this.overpoweredLowerBound.Text = "2";
            // 
            // underpoweredUpperBound
            // 
            this.underpoweredUpperBound.Location = new System.Drawing.Point(447, 115);
            this.underpoweredUpperBound.Name = "underpoweredUpperBound";
            this.underpoweredUpperBound.Size = new System.Drawing.Size(100, 20);
            this.underpoweredUpperBound.TabIndex = 8;
            this.underpoweredUpperBound.Text = "2";
            // 
            // underpoweredLowerBound
            // 
            this.underpoweredLowerBound.Location = new System.Drawing.Point(209, 115);
            this.underpoweredLowerBound.Name = "underpoweredLowerBound";
            this.underpoweredLowerBound.Size = new System.Drawing.Size(100, 20);
            this.underpoweredLowerBound.TabIndex = 7;
            this.underpoweredLowerBound.Text = "0";
            // 
            // UpperBoundLabel
            // 
            this.UpperBoundLabel.AutoSize = true;
            this.UpperBoundLabel.Location = new System.Drawing.Point(444, 92);
            this.UpperBoundLabel.Name = "UpperBoundLabel";
            this.UpperBoundLabel.Size = new System.Drawing.Size(70, 13);
            this.UpperBoundLabel.TabIndex = 6;
            this.UpperBoundLabel.Text = "Upper Bound";
            // 
            // LowerBoundLabel
            // 
            this.LowerBoundLabel.AutoSize = true;
            this.LowerBoundLabel.Location = new System.Drawing.Point(216, 92);
            this.LowerBoundLabel.Name = "LowerBoundLabel";
            this.LowerBoundLabel.Size = new System.Drawing.Size(70, 13);
            this.LowerBoundLabel.TabIndex = 5;
            this.LowerBoundLabel.Text = "Lower Bound";
            // 
            // PowerToWeightLabel
            // 
            this.PowerToWeightLabel.AutoSize = true;
            this.PowerToWeightLabel.Location = new System.Drawing.Point(49, 73);
            this.PowerToWeightLabel.Name = "PowerToWeightLabel";
            this.PowerToWeightLabel.Size = new System.Drawing.Size(126, 13);
            this.PowerToWeightLabel.TabIndex = 4;
            this.PowerToWeightLabel.Text = "Power To Weight Ratios:";
            // 
            // overpoweredLabel
            // 
            this.overpoweredLabel.AutoSize = true;
            this.overpoweredLabel.Location = new System.Drawing.Point(49, 144);
            this.overpoweredLabel.Name = "overpoweredLabel";
            this.overpoweredLabel.Size = new System.Drawing.Size(74, 13);
            this.overpoweredLabel.TabIndex = 3;
            this.overpoweredLabel.Text = "Overpowered:";
            // 
            // underpoweredLabel
            // 
            this.underpoweredLabel.AutoSize = true;
            this.underpoweredLabel.Location = new System.Drawing.Point(49, 118);
            this.underpoweredLabel.Name = "underpoweredLabel";
            this.underpoweredLabel.Size = new System.Drawing.Size(80, 13);
            this.underpoweredLabel.TabIndex = 2;
            this.underpoweredLabel.Text = "Underpowered:";
            // 
            // DataFileLabel
            // 
            this.DataFileLabel.AutoSize = true;
            this.DataFileLabel.Location = new System.Drawing.Point(49, 33);
            this.DataFileLabel.Name = "DataFileLabel";
            this.DataFileLabel.Size = new System.Drawing.Size(70, 13);
            this.DataFileLabel.TabIndex = 1;
            this.DataFileLabel.Text = "Ice Data File:";
            // 
            // simIceDataFile
            // 
            this.simIceDataFile.Location = new System.Drawing.Point(163, 30);
            this.simIceDataFile.Name = "simIceDataFile";
            this.simIceDataFile.Size = new System.Drawing.Size(671, 20);
            this.simIceDataFile.TabIndex = 0;
            // 
            // processingTab
            // 
            this.processingTab.Controls.Add(this.Execute);
            this.processingTab.Controls.Add(this.TSRWindowBoundary);
            this.processingTab.Controls.Add(this.timeSeparation);
            this.processingTab.Controls.Add(this.loopSpeedFactor);
            this.processingTab.Controls.Add(this.distanceThreshold);
            this.processingTab.Controls.Add(this.loopBoundary);
            this.processingTab.Controls.Add(this.minimumJourneyDistance);
            this.processingTab.Controls.Add(this.interpolationInterval);
            this.processingTab.Controls.Add(this.endInterpolationKm);
            this.processingTab.Controls.Add(this.startInterpolationKm);
            this.processingTab.Controls.Add(this.tsrLabel);
            this.processingTab.Controls.Add(this.loopSpeedLabel);
            this.processingTab.Controls.Add(this.loopBoundaryLabel);
            this.processingTab.Controls.Add(this.endInterpolationLabel);
            this.processingTab.Controls.Add(this.timeSeparationLabel);
            this.processingTab.Controls.Add(this.tiemSeparationLabel);
            this.processingTab.Controls.Add(this.minDistanceLabel);
            this.processingTab.Controls.Add(this.IntervalLabel);
            this.processingTab.Controls.Add(this.startInterpolationLabel);
            this.processingTab.Controls.Add(this.toLongitude);
            this.processingTab.Controls.Add(this.toLatitude);
            this.processingTab.Controls.Add(this.toLongitudeLabel);
            this.processingTab.Controls.Add(this.toLatitudeLabel);
            this.processingTab.Controls.Add(this.bottomRightLabel);
            this.processingTab.Controls.Add(this.fromLongitude);
            this.processingTab.Controls.Add(this.fromLatitude);
            this.processingTab.Controls.Add(this.fromLongitudeLabel);
            this.processingTab.Controls.Add(this.fromLatitudeLabel);
            this.processingTab.Controls.Add(this.TopLeftLabel);
            this.processingTab.Controls.Add(this.ConfinementLabel);
            this.processingTab.Controls.Add(this.ToDateLabel);
            this.processingTab.Controls.Add(this.FromDateLabel);
            this.processingTab.Controls.Add(this.toDate);
            this.processingTab.Controls.Add(this.DateRangeLabel);
            this.processingTab.Controls.Add(this.fromDate);
            this.processingTab.Location = new System.Drawing.Point(4, 22);
            this.processingTab.Name = "processingTab";
            this.processingTab.Padding = new System.Windows.Forms.Padding(3);
            this.processingTab.Size = new System.Drawing.Size(861, 401);
            this.processingTab.TabIndex = 2;
            this.processingTab.Text = "Processing Parameters";
            this.processingTab.UseVisualStyleBackColor = true;
            // 
            // Execute
            // 
            this.Execute.Location = new System.Drawing.Point(387, 349);
            this.Execute.Name = "Execute";
            this.Execute.Size = new System.Drawing.Size(126, 29);
            this.Execute.TabIndex = 34;
            this.Execute.Text = "Execute";
            this.Execute.UseVisualStyleBackColor = true;
            // 
            // TSRWindowBoundary
            // 
            this.TSRWindowBoundary.Location = new System.Drawing.Point(562, 303);
            this.TSRWindowBoundary.Name = "TSRWindowBoundary";
            this.TSRWindowBoundary.Size = new System.Drawing.Size(100, 20);
            this.TSRWindowBoundary.TabIndex = 33;
            this.TSRWindowBoundary.Text = "1";
            // 
            // timeSeparation
            // 
            this.timeSeparation.Location = new System.Drawing.Point(206, 303);
            this.timeSeparation.Name = "timeSeparation";
            this.timeSeparation.Size = new System.Drawing.Size(100, 20);
            this.timeSeparation.TabIndex = 32;
            this.timeSeparation.Text = "10";
            // 
            // loopSpeedFactor
            // 
            this.loopSpeedFactor.Location = new System.Drawing.Point(562, 277);
            this.loopSpeedFactor.Name = "loopSpeedFactor";
            this.loopSpeedFactor.Size = new System.Drawing.Size(100, 20);
            this.loopSpeedFactor.TabIndex = 31;
            this.loopSpeedFactor.Text = "50";
            // 
            // distanceThreshold
            // 
            this.distanceThreshold.Location = new System.Drawing.Point(206, 277);
            this.distanceThreshold.Name = "distanceThreshold";
            this.distanceThreshold.Size = new System.Drawing.Size(100, 20);
            this.distanceThreshold.TabIndex = 30;
            this.distanceThreshold.Text = "4";
            // 
            // loopBoundary
            // 
            this.loopBoundary.Location = new System.Drawing.Point(562, 251);
            this.loopBoundary.Name = "loopBoundary";
            this.loopBoundary.Size = new System.Drawing.Size(100, 20);
            this.loopBoundary.TabIndex = 29;
            this.loopBoundary.Text = "2";
            // 
            // minimumJourneyDistance
            // 
            this.minimumJourneyDistance.Location = new System.Drawing.Point(206, 251);
            this.minimumJourneyDistance.Name = "minimumJourneyDistance";
            this.minimumJourneyDistance.Size = new System.Drawing.Size(100, 20);
            this.minimumJourneyDistance.TabIndex = 28;
            this.minimumJourneyDistance.Text = "40";
            // 
            // interpolationInterval
            // 
            this.interpolationInterval.Location = new System.Drawing.Point(206, 225);
            this.interpolationInterval.Name = "interpolationInterval";
            this.interpolationInterval.Size = new System.Drawing.Size(100, 20);
            this.interpolationInterval.TabIndex = 27;
            this.interpolationInterval.Text = "50";
            // 
            // endInterpolationKm
            // 
            this.endInterpolationKm.Location = new System.Drawing.Point(562, 199);
            this.endInterpolationKm.Name = "endInterpolationKm";
            this.endInterpolationKm.Size = new System.Drawing.Size(100, 20);
            this.endInterpolationKm.TabIndex = 26;
            this.endInterpolationKm.Text = "70";
            // 
            // startInterpolationKm
            // 
            this.startInterpolationKm.Location = new System.Drawing.Point(206, 199);
            this.startInterpolationKm.Name = "startInterpolationKm";
            this.startInterpolationKm.Size = new System.Drawing.Size(100, 20);
            this.startInterpolationKm.TabIndex = 25;
            this.startInterpolationKm.Text = "5";
            // 
            // tsrLabel
            // 
            this.tsrLabel.AutoSize = true;
            this.tsrLabel.Location = new System.Drawing.Point(361, 306);
            this.tsrLabel.Name = "tsrLabel";
            this.tsrLabel.Size = new System.Drawing.Size(94, 13);
            this.tsrLabel.TabIndex = 24;
            this.tsrLabel.Text = "TSR Window (km)";
            // 
            // loopSpeedLabel
            // 
            this.loopSpeedLabel.AutoSize = true;
            this.loopSpeedLabel.Location = new System.Drawing.Point(361, 280);
            this.loopSpeedLabel.Name = "loopSpeedLabel";
            this.loopSpeedLabel.Size = new System.Drawing.Size(98, 13);
            this.loopSpeedLabel.TabIndex = 23;
            this.loopSpeedLabel.Text = "Loop Speed Factor";
            // 
            // loopBoundaryLabel
            // 
            this.loopBoundaryLabel.AutoSize = true;
            this.loopBoundaryLabel.Location = new System.Drawing.Point(361, 254);
            this.loopBoundaryLabel.Name = "loopBoundaryLabel";
            this.loopBoundaryLabel.Size = new System.Drawing.Size(129, 13);
            this.loopBoundaryLabel.TabIndex = 22;
            this.loopBoundaryLabel.Text = "Loop Boundary Threshold";
            // 
            // endInterpolationLabel
            // 
            this.endInterpolationLabel.AutoSize = true;
            this.endInterpolationLabel.Location = new System.Drawing.Point(361, 202);
            this.endInterpolationLabel.Name = "endInterpolationLabel";
            this.endInterpolationLabel.Size = new System.Drawing.Size(87, 13);
            this.endInterpolationLabel.TabIndex = 21;
            this.endInterpolationLabel.Text = "End Interpolation";
            // 
            // timeSeparationLabel
            // 
            this.timeSeparationLabel.AutoSize = true;
            this.timeSeparationLabel.Location = new System.Drawing.Point(35, 306);
            this.timeSeparationLabel.Name = "timeSeparationLabel";
            this.timeSeparationLabel.Size = new System.Drawing.Size(99, 13);
            this.timeSeparationLabel.TabIndex = 20;
            this.timeSeparationLabel.Text = "Time Separation (h)";
            // 
            // tiemSeparationLabel
            // 
            this.tiemSeparationLabel.AutoSize = true;
            this.tiemSeparationLabel.Location = new System.Drawing.Point(35, 280);
            this.tiemSeparationLabel.Name = "tiemSeparationLabel";
            this.tiemSeparationLabel.Size = new System.Drawing.Size(107, 13);
            this.tiemSeparationLabel.TabIndex = 19;
            this.tiemSeparationLabel.Text = "Data Separation (km)";
            // 
            // minDistanceLabel
            // 
            this.minDistanceLabel.AutoSize = true;
            this.minDistanceLabel.Location = new System.Drawing.Point(35, 255);
            this.minDistanceLabel.Name = "minDistanceLabel";
            this.minDistanceLabel.Size = new System.Drawing.Size(149, 13);
            this.minDistanceLabel.TabIndex = 18;
            this.minDistanceLabel.Text = "Minimum Travel Distance (km)";
            // 
            // IntervalLabel
            // 
            this.IntervalLabel.AutoSize = true;
            this.IntervalLabel.Location = new System.Drawing.Point(35, 228);
            this.IntervalLabel.Name = "IntervalLabel";
            this.IntervalLabel.Size = new System.Drawing.Size(103, 13);
            this.IntervalLabel.TabIndex = 17;
            this.IntervalLabel.Text = "Interpolation Interval";
            // 
            // startInterpolationLabel
            // 
            this.startInterpolationLabel.AutoSize = true;
            this.startInterpolationLabel.Location = new System.Drawing.Point(35, 202);
            this.startInterpolationLabel.Name = "startInterpolationLabel";
            this.startInterpolationLabel.Size = new System.Drawing.Size(90, 13);
            this.startInterpolationLabel.TabIndex = 16;
            this.startInterpolationLabel.Text = "Start Interpolation";
            // 
            // toLongitude
            // 
            this.toLongitude.Location = new System.Drawing.Point(562, 162);
            this.toLongitude.Name = "toLongitude";
            this.toLongitude.Size = new System.Drawing.Size(100, 20);
            this.toLongitude.TabIndex = 15;
            this.toLongitude.Text = "150";
            // 
            // toLatitude
            // 
            this.toLatitude.Location = new System.Drawing.Point(409, 162);
            this.toLatitude.Name = "toLatitude";
            this.toLatitude.Size = new System.Drawing.Size(100, 20);
            this.toLatitude.TabIndex = 14;
            this.toLatitude.Text = "-33";
            // 
            // toLongitudeLabel
            // 
            this.toLongitudeLabel.AutoSize = true;
            this.toLongitudeLabel.Location = new System.Drawing.Point(559, 146);
            this.toLongitudeLabel.Name = "toLongitudeLabel";
            this.toLongitudeLabel.Size = new System.Drawing.Size(57, 13);
            this.toLongitudeLabel.TabIndex = 13;
            this.toLongitudeLabel.Text = "Longitude:";
            // 
            // toLatitudeLabel
            // 
            this.toLatitudeLabel.AutoSize = true;
            this.toLatitudeLabel.Location = new System.Drawing.Point(406, 146);
            this.toLatitudeLabel.Name = "toLatitudeLabel";
            this.toLatitudeLabel.Size = new System.Drawing.Size(45, 13);
            this.toLatitudeLabel.TabIndex = 12;
            this.toLatitudeLabel.Text = "Latitude";
            // 
            // bottomRightLabel
            // 
            this.bottomRightLabel.AutoSize = true;
            this.bottomRightLabel.Location = new System.Drawing.Point(153, 165);
            this.bottomRightLabel.Name = "bottomRightLabel";
            this.bottomRightLabel.Size = new System.Drawing.Size(115, 13);
            this.bottomRightLabel.TabIndex = 11;
            this.bottomRightLabel.Text = "Bottom Right Location:";
            // 
            // fromLongitude
            // 
            this.fromLongitude.Location = new System.Drawing.Point(562, 107);
            this.fromLongitude.Name = "fromLongitude";
            this.fromLongitude.Size = new System.Drawing.Size(100, 20);
            this.fromLongitude.TabIndex = 10;
            this.fromLongitude.Text = "150";
            // 
            // fromLatitude
            // 
            this.fromLatitude.Location = new System.Drawing.Point(409, 107);
            this.fromLatitude.Name = "fromLatitude";
            this.fromLatitude.Size = new System.Drawing.Size(100, 20);
            this.fromLatitude.TabIndex = 9;
            this.fromLatitude.Text = "-33";
            // 
            // fromLongitudeLabel
            // 
            this.fromLongitudeLabel.AutoSize = true;
            this.fromLongitudeLabel.Location = new System.Drawing.Point(559, 91);
            this.fromLongitudeLabel.Name = "fromLongitudeLabel";
            this.fromLongitudeLabel.Size = new System.Drawing.Size(57, 13);
            this.fromLongitudeLabel.TabIndex = 8;
            this.fromLongitudeLabel.Text = "Longitude:";
            // 
            // fromLatitudeLabel
            // 
            this.fromLatitudeLabel.AutoSize = true;
            this.fromLatitudeLabel.Location = new System.Drawing.Point(406, 91);
            this.fromLatitudeLabel.Name = "fromLatitudeLabel";
            this.fromLatitudeLabel.Size = new System.Drawing.Size(45, 13);
            this.fromLatitudeLabel.TabIndex = 7;
            this.fromLatitudeLabel.Text = "Latitude";
            // 
            // TopLeftLabel
            // 
            this.TopLeftLabel.AutoSize = true;
            this.TopLeftLabel.Location = new System.Drawing.Point(153, 110);
            this.TopLeftLabel.Name = "TopLeftLabel";
            this.TopLeftLabel.Size = new System.Drawing.Size(94, 13);
            this.TopLeftLabel.TabIndex = 6;
            this.TopLeftLabel.Text = "Top Left Location:";
            // 
            // ConfinementLabel
            // 
            this.ConfinementLabel.AutoSize = true;
            this.ConfinementLabel.Location = new System.Drawing.Point(35, 93);
            this.ConfinementLabel.Name = "ConfinementLabel";
            this.ConfinementLabel.Size = new System.Drawing.Size(127, 13);
            this.ConfinementLabel.TabIndex = 5;
            this.ConfinementLabel.Text = "Geographic Confinement:";
            // 
            // ToDateLabel
            // 
            this.ToDateLabel.AutoSize = true;
            this.ToDateLabel.Location = new System.Drawing.Point(406, 22);
            this.ToDateLabel.Name = "ToDateLabel";
            this.ToDateLabel.Size = new System.Drawing.Size(23, 13);
            this.ToDateLabel.TabIndex = 4;
            this.ToDateLabel.Text = "To:";
            // 
            // FromDateLabel
            // 
            this.FromDateLabel.AutoSize = true;
            this.FromDateLabel.Location = new System.Drawing.Point(203, 22);
            this.FromDateLabel.Name = "FromDateLabel";
            this.FromDateLabel.Size = new System.Drawing.Size(33, 13);
            this.FromDateLabel.TabIndex = 3;
            this.FromDateLabel.Text = "From:";
            // 
            // toDate
            // 
            this.toDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.toDate.Location = new System.Drawing.Point(409, 49);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(104, 20);
            this.toDate.TabIndex = 2;
            // 
            // DateRangeLabel
            // 
            this.DateRangeLabel.AutoSize = true;
            this.DateRangeLabel.Location = new System.Drawing.Point(35, 56);
            this.DateRangeLabel.Name = "DateRangeLabel";
            this.DateRangeLabel.Size = new System.Drawing.Size(68, 13);
            this.DateRangeLabel.TabIndex = 1;
            this.DateRangeLabel.Text = "Date Range:";
            // 
            // fromDate
            // 
            this.fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fromDate.Location = new System.Drawing.Point(206, 50);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(100, 20);
            this.fromDate.TabIndex = 0;
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 351);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Combined P/W:";
            // 
            // combinedIncreasingP2W
            // 
            this.combinedIncreasingP2W.AutoSize = true;
            this.combinedIncreasingP2W.Location = new System.Drawing.Point(216, 351);
            this.combinedIncreasingP2W.Name = "combinedIncreasingP2W";
            this.combinedIncreasingP2W.Size = new System.Drawing.Size(13, 13);
            this.combinedIncreasingP2W.TabIndex = 23;
            this.combinedIncreasingP2W.Text = "0";
            // 
            // combinedDecreasingP2W
            // 
            this.combinedDecreasingP2W.AutoSize = true;
            this.combinedDecreasingP2W.Location = new System.Drawing.Point(444, 351);
            this.combinedDecreasingP2W.Name = "combinedDecreasingP2W";
            this.combinedDecreasingP2W.Size = new System.Drawing.Size(13, 13);
            this.combinedDecreasingP2W.TabIndex = 24;
            this.combinedDecreasingP2W.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 459);
            this.Controls.Add(this.tabControl1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.fileSelectionTab.ResumeLayout(false);
            this.fileSelectionTab.PerformLayout();
            this.simulationTab.ResumeLayout(false);
            this.simulationTab.PerformLayout();
            this.processingTab.ResumeLayout(false);
            this.processingTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage fileSelectionTab;
        private System.Windows.Forms.TabPage simulationTab;
        private System.Windows.Forms.TextBox TrainFile;
        private System.Windows.Forms.TextBox DecreasingSimulationFile;
        private System.Windows.Forms.TextBox IncreasingSimulationFile;
        private System.Windows.Forms.TextBox GeometryFile;
        private System.Windows.Forms.TextBox IceDataFile;
        private System.Windows.Forms.CheckBox includeAListOfTrainsToExclude;
        private System.Windows.Forms.Button selectTrainList;
        private System.Windows.Forms.Button selectDecreasingSimulation;
        private System.Windows.Forms.Button selectIncreasingSimulation;
        private System.Windows.Forms.Button selectGeomtryFile;
        private System.Windows.Forms.Button selectDataFile;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TabPage processingTab;
        private System.Windows.Forms.Label overpoweredDecreasingP2W;
        private System.Windows.Forms.Label overpoweredIncreasingP2W;
        private System.Windows.Forms.Label underpoweredDecreasingP2W;
        private System.Windows.Forms.Label SimOverpoweredLabel;
        private System.Windows.Forms.Label SimUnderpoweredLabel;
        private System.Windows.Forms.Label underpoweredIncreasingP2W;
        private System.Windows.Forms.Label SimDecreasingLabel;
        private System.Windows.Forms.Label SimIncreasingLabel;
        private System.Windows.Forms.Button averagePowerToWeightRatios;
        private System.Windows.Forms.TextBox overpoweredUpperBound;
        private System.Windows.Forms.TextBox overpoweredLowerBound;
        private System.Windows.Forms.TextBox underpoweredUpperBound;
        private System.Windows.Forms.TextBox underpoweredLowerBound;
        private System.Windows.Forms.Label UpperBoundLabel;
        private System.Windows.Forms.Label LowerBoundLabel;
        private System.Windows.Forms.Label PowerToWeightLabel;
        private System.Windows.Forms.Label overpoweredLabel;
        private System.Windows.Forms.Label underpoweredLabel;
        private System.Windows.Forms.Label DataFileLabel;
        private System.Windows.Forms.TextBox simIceDataFile;
        private System.Windows.Forms.Label tsrLabel;
        private System.Windows.Forms.Label loopSpeedLabel;
        private System.Windows.Forms.Label loopBoundaryLabel;
        private System.Windows.Forms.Label endInterpolationLabel;
        private System.Windows.Forms.Label timeSeparationLabel;
        private System.Windows.Forms.Label tiemSeparationLabel;
        private System.Windows.Forms.Label minDistanceLabel;
        private System.Windows.Forms.Label IntervalLabel;
        private System.Windows.Forms.Label startInterpolationLabel;
        private System.Windows.Forms.TextBox toLongitude;
        private System.Windows.Forms.TextBox toLatitude;
        private System.Windows.Forms.Label toLongitudeLabel;
        private System.Windows.Forms.Label toLatitudeLabel;
        private System.Windows.Forms.Label bottomRightLabel;
        private System.Windows.Forms.TextBox fromLongitude;
        private System.Windows.Forms.TextBox fromLatitude;
        private System.Windows.Forms.Label fromLongitudeLabel;
        private System.Windows.Forms.Label fromLatitudeLabel;
        private System.Windows.Forms.Label TopLeftLabel;
        private System.Windows.Forms.Label ConfinementLabel;
        private System.Windows.Forms.Label ToDateLabel;
        private System.Windows.Forms.Label FromDateLabel;
        private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.Label DateRangeLabel;
        private System.Windows.Forms.DateTimePicker fromDate;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private System.Windows.Forms.TextBox TSRWindowBoundary;
        private System.Windows.Forms.TextBox timeSeparation;
        private System.Windows.Forms.TextBox loopSpeedFactor;
        private System.Windows.Forms.TextBox distanceThreshold;
        private System.Windows.Forms.TextBox loopBoundary;
        private System.Windows.Forms.TextBox minimumJourneyDistance;
        private System.Windows.Forms.TextBox interpolationInterval;
        private System.Windows.Forms.TextBox endInterpolationKm;
        private System.Windows.Forms.TextBox startInterpolationKm;
        private System.Windows.Forms.Button Execute;
        private System.Windows.Forms.Label combinedDecreasingP2W;
        private System.Windows.Forms.Label combinedIncreasingP2W;
        private System.Windows.Forms.Label label1;
    }
}

