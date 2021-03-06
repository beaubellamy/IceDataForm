﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Globalsettings;

namespace TrainPerformance
{

    public partial class TrainPerformanceAnalysis : Form
    {
        
        public static Tools tool = new Tools();
        public static Processing processing = new Processing();
        public static TrackGeometry track = new TrackGeometry();

        public TrainPerformanceAnalysis()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Select the ICE data file
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectDataFile_Click(object sender, EventArgs e)
        {
            string batchFileName = null;

            if (sender == selectDataFile)
            {
                /* Select the data file. */
                batchFileName = tool.browseFile("Select the data file."); 
                IceDataFile.Text = Path.GetFileName(batchFileName);
                simIceDataFile.Text = Path.GetFileName(batchFileName);

                IceDataFile.ForeColor = System.Drawing.Color.Black;
                simIceDataFile.ForeColor = System.Drawing.Color.Black;

                if (batchFileName != null || batchFileName != "")
                    FileSettings.batchFiles.Add(batchFileName);

            }           
            else if (sender == selectDataFile1)
            {
                /* First additional file for batch reading. */
                batchFileName = tool.browseFile("Select the data file.");
                IceDataFile1.Text = Path.GetFileName(batchFileName);                
                IceDataFile1.ForeColor = System.Drawing.Color.Black;
                
                if (batchFileName != null && batchFileName != "")
                    FileSettings.batchFiles.Add(batchFileName);
            }
            else if (sender == selectDataFile2)
            {
                /* Second additional file for batch reading. */
                batchFileName = tool.browseFile("Select the data file.");
                IceDataFile2.Text = Path.GetFileName(batchFileName);
                IceDataFile2.ForeColor = System.Drawing.Color.Black;
                
                if (batchFileName != null || batchFileName != "")
                    FileSettings.batchFiles.Add(batchFileName);
            }
            else if (sender == selectDataFile3)
            {
                /* Third additional file for batch reading. */
                batchFileName = tool.browseFile("Select the data file.");
                IceDataFile3.Text = Path.GetFileName(batchFileName);
                IceDataFile3.ForeColor = System.Drawing.Color.Black;
                
                if (batchFileName != null || batchFileName != "")
                    FileSettings.batchFiles.Add(batchFileName);
            }
            else if (sender == selectDataFile4)
            {
                /* Fourth additional file for batch reading. */
                batchFileName = tool.browseFile("Select the data file.");
                IceDataFile4.Text = Path.GetFileName(batchFileName);
                IceDataFile4.ForeColor = System.Drawing.Color.Black;
                
                if (batchFileName != null || batchFileName != "")
                    FileSettings.batchFiles.Add(batchFileName);
            }
            else if (sender == selectDataFile5)
            {
                /* Fifth additional file for batch reading. */
                batchFileName = tool.browseFile("Select the data file.");
                IceDataFile5.Text = Path.GetFileName(batchFileName);
                IceDataFile5.ForeColor = System.Drawing.Color.Black;
                
                if (batchFileName != null || batchFileName != "")
                    FileSettings.batchFiles.Add(batchFileName);
            }
            else
            {
                Console.WriteLine("unkown behaviour");
            }


            

        }

        /// <summary>
        /// Select the geometry file.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectGeometryFile_Click(object sender, EventArgs e)
        {
            FileSettings.geometryFile = //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\Macarthur to Botany Geometry.csv";
            tool.browseFile("Select the geometry file.");
            GeometryFile.Text = Path.GetFileName(FileSettings.geometryFile);
            GeometryFile.ForeColor = System.Drawing.Color.Black;
        }

        /// <summary>
        /// Select the temporary speed restriction file.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>        
        private void selectTSRFile_Click(object sender, EventArgs e)
        {
            FileSettings.temporarySpeedRestrictionFile = //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\Macarthur to Botany TSR.csv";
            tool.browseFile("Select the TSR file.");
            temporarySpeedRestrictionFile.Text = Path.GetFileName(FileSettings.temporarySpeedRestrictionFile);
            temporarySpeedRestrictionFile.ForeColor = System.Drawing.Color.Black;
        }

        /// <summary>
        /// Select the simulation file with increasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectUnderpoweredIncreasingSimulationFile_Click(object sender, EventArgs e)
        {
            string browseFile = "Select the underpowered increasing km simulation file.";
            if (getHunterValleyRegion())
                browseFile = "Select the Pacific National increasing km simulation file.";

            FileSettings.underpoweredIncreasingSimulationFile = //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Botany to Macarthur - All - 3.33_ThuW1.csv";
                tool.browseFile(browseFile);
            underpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredIncreasingSimulationFile);
            underpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;
        }

        /// <summary>
        /// Selct the simulation file with decreasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectUnderpoweredDecreasingSimulationFile_Click(object sender, EventArgs e)
        {
            string browseFile = "Select the underpowered decreasing km simulation file.";
            if (getHunterValleyRegion())
                browseFile = "Select the Pacific National decreasing km simulation file.";

            FileSettings.underpoweredDecreasingSimulationFile = //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Macarthur to Botany - All - 3.20_SatW1.csv";
            tool.browseFile(browseFile);
            underpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredDecreasingSimulationFile);
            underpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;
        }

        /// <summary>
        /// Select the simulation file with increasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectOverpoweredIncreasingSimulationFile_Click(object sender, EventArgs e)
        {
            string browseFile = "Select the overpowered increasing km simulation file.";
            if (getHunterValleyRegion())
                browseFile = "Select the Aurizon increasing km simulation file.";

            FileSettings.overpoweredIncreasingSimulationFile = //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Botany to Macarthur - All - 7.87_ThuW1.csv";
            tool.browseFile(browseFile);
            overpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredIncreasingSimulationFile);
            overpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;
        }

        /// <summary>
        /// Selct the simulation file with decreasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectOverpoweredDecreasingSimulationFile_Click(object sender, EventArgs e)
        {
            string browseFile = "Select the overpowered decreasing km simulation file.";
            if (getHunterValleyRegion())
                browseFile = "Select the Aurizon decreasing km simulation file.";

            FileSettings.overpoweredDecreasingSimulationFile = //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Macarthur to Botany - All - 6.97_SatW1.csv";
            tool.browseFile(browseFile);
            overpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredDecreasingSimulationFile);
            overpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;
        }

        /// <summary>
        /// Select the simulation file with increasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectAlternativeIncreasingSimulationFile_Click(object sender, EventArgs e)
        {
            string browseFile = "Select the underpowered increasing km simulation file.";
            if (getHunterValleyRegion())
                browseFile = "Select the alternative operator increasing km simulation file.";

            FileSettings.alternativeIncreasingSimulationFile = //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Botany to Macarthur - All - 3.33_ThuW1.csv";
                tool.browseFile(browseFile);
            alternativeIncreasingFile.Text = Path.GetFileName(FileSettings.underpoweredIncreasingSimulationFile);
            alternativeIncreasingFile.ForeColor = System.Drawing.Color.Black;
        }

        /// <summary>
        /// Select the simulation file with decreasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectAlternativeDecreasingSimulationFile_Click(object sender, EventArgs e)
        {
            string browseFile = "Select the alternative operator decreasing km simulation file.";
            if (getHunterValleyRegion())
                browseFile = "Select the alternative operator decreasing km simulation file.";

            FileSettings.alternativeDecreasingSimulationFile = //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Macarthur to Botany - All - 6.97_SatW1.csv";
            tool.browseFile(browseFile);
            alternativeDecreasingFile.Text = Path.GetFileName(FileSettings.overpoweredDecreasingSimulationFile);
            alternativeDecreasingFile.ForeColor = System.Drawing.Color.Black;
        }

        /// <summary>
        /// Select the train list file.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectTrainList_Click(object sender, EventArgs e)
        {
            FileSettings.trainList = tool.browseFile("Select the train list file.");
            TrainFile.Text = Path.GetFileName(FileSettings.trainList);
            TrainFile.ForeColor = System.Drawing.Color.Black;
        }

        /// <summary>
        /// Select the destination folder.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void DestinationFolder_Click(object sender, EventArgs e)
        {
            FileSettings.aggregatedDestination = //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis";
            tool.selectFolder();
            ResultsDestination.Text = FileSettings.aggregatedDestination;
            ResultsDestination.ForeColor = System.Drawing.Color.Black;
        }

        /// <summary>
        /// Execute the analysis.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void Execute_Click(object sender, EventArgs e)
        {
            /* Populate the parameters. */
            processing.populateFormParameters(this);
            /* Validate the form parameters. */
            if (!processing.areFormParametersValid())
            {
                tool.messageBox("One or more parameters are invalid.");
                return;
            }

            Stopwatch timer = new Stopwatch();
            timer.Start();

            /* Run the train performance analysis. */
            List<Train> trains = new List<Train>();
            trains = Algorithm.trainPerformance();
            timer.Stop();
            TimeSpan ts = timer.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",ts.Hours, ts.Minutes, ts.Seconds,ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);


            /* Populate the counts for each train catagory. */
            underpoweredIncreasingCount.Text = trains.Where(t => t.TrainJourney[0].trainDirection == direction.increasing).
                                            Where(t => t.TrainJourney[0].powerToWeight > Settings.underpoweredLowerBound).
                                            Where(t => t.TrainJourney[0].powerToWeight <= Settings.underpoweredUpperBound).Count().ToString();
            underpoweredDecreasingCount.Text = trains.Where(t => t.TrainJourney[0].trainDirection == direction.decreasing).
                                            Where(t => t.TrainJourney[0].powerToWeight > Settings.underpoweredLowerBound).
                                            Where(t => t.TrainJourney[0].powerToWeight <= Settings.underpoweredUpperBound).Count().ToString();
            overpoweredIncreasingCount.Text = trains.Where(t => t.TrainJourney[0].trainDirection == direction.increasing).
                                            Where(t => t.TrainJourney[0].powerToWeight > Settings.overpoweredLowerBound).
                                            Where(t => t.TrainJourney[0].powerToWeight <= Settings.overpoweredUpperBound).Count().ToString();
            overpoweredDecreasingCount.Text = trains.Where(t => t.TrainJourney[0].trainDirection == direction.decreasing).
                                            Where(t => t.TrainJourney[0].powerToWeight > Settings.overpoweredLowerBound).
                                            Where(t => t.TrainJourney[0].powerToWeight <= Settings.overpoweredUpperBound).Count().ToString();

            totalIncreasingCount.Text = trains.Where(t => t.TrainJourney[0].trainDirection == direction.increasing).
                                            Where(t => t.TrainJourney[0].powerToWeight > Settings.underpoweredLowerBound).
                                            Where(t => t.TrainJourney[0].powerToWeight <= Settings.overpoweredUpperBound).Count().ToString();
            totalDecreasingCount.Text = trains.Where(t => t.TrainJourney[0].trainDirection == direction.decreasing).
                                            Where(t => t.TrainJourney[0].powerToWeight > Settings.underpoweredLowerBound).
                                            Where(t => t.TrainJourney[0].powerToWeight <= Settings.overpoweredUpperBound).Count().ToString();

            ExecutionTime.Text = elapsedTime;

            tool.messageBox("Program Complete.");

        }

        /// <summary>
        /// Caalcaulte the average power to weight ratios for the simualted trains
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void averagePowerToWeightRatios_Click(object sender, EventArgs e)
        {
            
            processing.populateFormParameters(this);
            /* Validate the form parameters. */
            if (!processing.areFormParametersValid())
            {
                tool.messageBox("One or more parameters are invalid.");
                return;
            }
            
            if (FileSettings.batchFiles.Count() == 0 || FileSettings.geometryFile == null)
                return;
            
            /* Ensure there is a empty list of trains to exclude to start. */
            List<string> excludeTrainList = new List<string> { };

            /* Populate the exluded train list. */
            if (Settings.includeAListOfTrainsToExclude)
                excludeTrainList = FileOperations.readTrainList(FileSettings.trainList);

            /* Read in the track gemoetry data. */
            List<TrackGeometry> trackGeometry = new List<TrackGeometry>();
            trackGeometry = FileOperations.readGeometryfile(FileSettings.geometryFile);
            

            /* Read in the TSR information */
            List<TSRObject> TSRs = new List<TSRObject>();
            TSRs = FileOperations.readTSRFile(FileSettings.temporarySpeedRestrictionFile);

            /* Read the data. */
            List<TrainDetails> TrainRecords = new List<TrainDetails>();
            foreach (string file in FileSettings.batchFiles)
                TrainRecords.AddRange(FileOperations.readICEData(file, excludeTrainList));

            if (TrainRecords.Count() == 0)
            {
                tool.messageBox("There is no data within the specified boundaries.\nCheck the processing parameters.");
                return;
            }
            
            if (TrainRecords.Where(t => t.powerToWeight == 0).Count() == TrainRecords.Count())
                Settings.resetPowerToWeightBoundariesToZero();

            

            /* Sort the data by [trainID, locoID, Date & Time, kmPost]. */
            List<TrainDetails> OrderdTrainRecords = new List<TrainDetails>();
            OrderdTrainRecords = TrainRecords.OrderBy(t => t.TrainID).ThenBy(t => t.LocoID).ThenBy(t => t.NotificationDateTime).ThenBy(t => t.kmPost).ToList();

            /* Clean data - remove trains with insufficient data. */
            /******** Should only be required while we are waiting for the data in the prefered format ********/
            List<Train> CleanTrainRecords = new List<Train>();
            CleanTrainRecords = Algorithm.CleanData(trackGeometry, OrderdTrainRecords, TSRs);
            /**************************************************************************************************/

            
            /* Calculate the avareage power to weight ratio for a given band and train direction. */
            underpoweredIncreasingP2W.Text = string.Format("{0:#.000}", averagePowerToWeightRatio(CleanTrainRecords, Settings.underpoweredLowerBound, Settings.underpoweredUpperBound, direction.increasing));
            underpoweredDecreasingP2W.Text = string.Format("{0:#.000}", averagePowerToWeightRatio(CleanTrainRecords, Settings.underpoweredLowerBound, Settings.underpoweredUpperBound, direction.decreasing));

            overpoweredIncreasingP2W.Text = string.Format("{0:#.000}", averagePowerToWeightRatio(CleanTrainRecords, Settings.overpoweredLowerBound, Settings.overpoweredUpperBound, direction.increasing));
            overpoweredDecreasingP2W.Text = string.Format("{0:#.000}", averagePowerToWeightRatio(CleanTrainRecords, Settings.overpoweredLowerBound, Settings.overpoweredUpperBound, direction.decreasing));

            combinedIncreasingP2W.Text = string.Format("{0:#.000}", averagePowerToWeightRatio(CleanTrainRecords, Settings.underpoweredLowerBound, Settings.overpoweredUpperBound, direction.increasing));
            combinedDecreasingP2W.Text = string.Format("{0:#.000}", averagePowerToWeightRatio(CleanTrainRecords, Settings.underpoweredLowerBound, Settings.overpoweredUpperBound, direction.decreasing));

            /* Need to run the simulaions bsed on the average power to weight ratios before continueing with the analysis. */
            SimulationP2WRatioLabel.Text = "Run Simualtions based on these power to weight ratios";
        }
        
        /// <summary>
        /// Calculate the average power to weight ratio of all trains within a band for a given direction of travel.
        /// </summary>
        /// <param name="trains">List of trains in the data.</param>
        /// <param name="lowerBound">The lower bound of the acceptable power to weight ratio.</param>
        /// <param name="upperBound">The upper bound of the acceptable power to weight ratio.</param>
        /// <param name="direction">The direction of the km of the train journey.</param>
        /// <returns>The average power to weight ratio for the trains in the specified direction.</returns>
        public double averagePowerToWeightRatio(List<Train> trains, double lowerBound, double upperBound, direction direction)
        {

            List<double> power2Weight = new List<double>();
            double power = 0;

            /* Clycle through each train. */
            foreach (Train train in trains)
            {
                if (train.TrainJourney[0].trainDirection == direction)
                {
                    /* Extract the power to weight ratio for each train. */
                    power = train.TrainJourney[0].powerToWeight;
                    if (power > lowerBound && power <= upperBound)
                        power2Weight.Add(power);
                }
            }

            /* Return the average power to weight ratio. */
            if (power2Weight.Count() == 0)
                return 0;
            else
                return power2Weight.Average();

        }

        /// <summary>
        /// Extract the date range for the data.
        /// </summary>
        /// <returns>A 2-element array containig the start and end date to consider.</returns>
        public DateTime[] getDateRange() { return new DateTime[2] { fromDate.Value, toDate.Value }; }

        /// <summary>
        /// Extract the top left corner of the geographic box.
        /// </summary>
        /// <returns>A geographic location describing the top left corner of the box.</returns>
        public GeoLocation getTopLeftLocation()
        {
            double latitude, longitude;
            if (double.TryParse(fromLatitude.Text, out latitude) && (double.TryParse(fromLongitude.Text, out longitude)))
                return new GeoLocation(latitude, longitude);

            return new GeoLocation(0, 0);
        }

        /// <summary>
        /// Extract the bottom right corner of the geographic box.
        /// </summary>
        /// <returns>A geographic location describing the bottom right corner of the box.</returns>
        public GeoLocation getBottomRightLocation()
        {
            double latitude, longitude;
            if (double.TryParse(toLatitude.Text, out latitude) && (double.TryParse(toLongitude.Text, out longitude)))
                return new GeoLocation(latitude, longitude);

            return new GeoLocation(0, 0);
        }

        /// <summary>
        /// Extract the value of the includeAListOfTrainsToExclude flag.
        /// </summary>
        /// <returns>The value of the boolean flag.</returns>
        public bool getTrainListExcludeFlag() { return includeAListOfTrainsToExclude.Checked; }

        /// <summary>
        /// Extract the value of the start km for interpolation.
        /// </summary>
        /// <returns>The start km.</returns>
        public double getStartKm()
        {
            double startKm;
            if (double.TryParse(startInterpolationKm.Text, out startKm))
                return startKm;

            return 0;
        }

        /// <summary>
        /// Extract the value of the end km for interpolation.
        /// </summary>
        /// <returns>The end km.</returns>
        public double getEndKm()
        {
            double endKm;
            if (double.TryParse(endInterpolationKm.Text, out endKm))
                return endKm;

            return 0;
        }

        /// <summary>
        /// Extract the value of the interpolation interval.
        /// </summary>
        /// <returns>The interpolation interval.</returns>
        public double getInterval()
        {
            double interval;
            if (double.TryParse(interpolationInterval.Text, out interval))
                return interval;

            return 0;
        }

        /// <summary>
        /// Extract the value of the minimum journey distance threshold.
        /// </summary>
        /// <returns>The minimum distance of the train journey.</returns>
        public double getJourneydistance()
        {
            double journeyDistance;
            if (double.TryParse(minimumJourneyDistance.Text, out journeyDistance))
                return journeyDistance * 1000;

            return 0;
        }

        /// <summary>
        /// Extract the value of the loop speed factor for comparison to the simulation data.
        /// </summary>
        /// <returns>The comparison factor.</returns>
        public double getLoopFactor()
        {
            double loopFactor;
            if (double.TryParse(loopSpeedFactor.Text, out loopFactor))
                return loopFactor/100.0;

            return 0;
        }

        /// <summary>
        /// Extract the value of the distance before and after a loop to be considered within the loop boundary
        /// </summary>
        /// <returns>The loop boundary threshold.</returns>
        public double getLoopBoundary()
        {
            double loopWindow;
            if (double.TryParse(loopBoundary.Text, out loopWindow))
                return loopWindow;

            return 0;
        }

        /// <summary>
        /// Extract the value of the distance before and after a TSR location to be considered within the TSR boundary
        /// </summary>
        /// <returns>The TSR boundary window</returns>
        public double getTSRWindow()
        {
            double TSR;
            if (double.TryParse(TSRWindowBoundary.Text, out TSR))
                return TSR;

            return 0;
        }

        /// <summary>
        /// Extract the value of the time difference between succesive data points to be considered as seperate trains.
        /// </summary>
        /// <returns>The time difference in minutes.</returns>
        public double getTimeSeparation()
        {
            double timeDifference;
            if (double.TryParse(timeSeparation.Text, out timeDifference))
                return timeDifference * 60;

            return 0;
        }

        /// <summary>
        /// Extract the value of the minimum distance between successive data points.
        /// </summary>
        /// <returns>The minimum distance threshold.</returns>
        public double getDistanceThreshold()
        {
            double distance;
            if (double.TryParse(distanceThreshold.Text, out distance))
                return distance * 1000;

            return 0;
        }

        /// <summary>
        /// Extract the lower bound value of the power to weight ratio for the underpowered trains.
        /// </summary>
        /// <returns>The lower bound power to weight ratio.</returns>
        public double getUnderpoweredLowerBound()
        {
            double p2W;
            if (double.TryParse(underpoweredLowerBound.Text, out p2W))
                return p2W;

            return 0;
        }

        /// <summary>
        /// Extract the upper bound value of the power to weight ratio for the underpowered trains.
        /// </summary>
        /// <returns>The upper bound power to weight ratio.</returns>
        public double getUnderpoweredUpperBound()
        {
            double p2W;
            if (double.TryParse(underpoweredUpperBound.Text, out p2W))
                return p2W;

            return 0;
        }

        /// <summary>
        /// Extract the lower bound value of the power to weight ratio for the overpowered trains.
        /// </summary>
        /// <returns>The lower bound power to weight ratio.</returns>
        public double getOvderpoweredLowerBound()
        {
            double p2W;
            if (double.TryParse(overpoweredLowerBound.Text, out p2W))
                return p2W;

            return 0;
        }

        /// <summary>
        /// Extract the upper bound value of the power to weight ratio for the overpowered trains.
        /// </summary>
        /// <returns>The upper bound power to weight ratio.</returns>
        public double getOvderpoweredUpperBound()
        {
            double p2W;
            if (double.TryParse(overpoweredUpperBound.Text, out p2W))
                return p2W;

            return 0;
        }

        /// <summary>
        /// Extract the value of the getHunterValleyRegion flag.
        /// </summary>
        /// <returns>The value of the boolean flag.</returns>
        public bool getHunterValleyRegion() { return HunterValley.Checked; }

        /// <summary>
        /// This function sets all the testing parameters for the Cullerin Ranges data
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void CulleranRanges_Click(object sender, EventArgs e)
        {
            /* Data File */
            string batchFileName = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Cullerin Ranges\Cullerin Ranges test data.csv";

            IceDataFile.Text = Path.GetFileName(batchFileName);
            simIceDataFile.Text = Path.GetFileName(batchFileName);

            IceDataFile.ForeColor = System.Drawing.Color.Black;
            simIceDataFile.ForeColor = System.Drawing.Color.Black;

            if (batchFileName != null || batchFileName != "")
                FileSettings.batchFiles.Add(batchFileName);

            /* Geometry File */
            FileSettings.geometryFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Cullerin Ranges\Cullerin Ranges Geometry.csv";
            GeometryFile.Text = Path.GetFileName(FileSettings.geometryFile);
            GeometryFile.ForeColor = System.Drawing.Color.Black;

            /* TSR File */
            FileSettings.temporarySpeedRestrictionFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Cullerin Ranges\Cullerin Ranges TSR.csv";
            temporarySpeedRestrictionFile.Text = Path.GetFileName(FileSettings.temporarySpeedRestrictionFile);
            temporarySpeedRestrictionFile.ForeColor = System.Drawing.Color.Black;

            /* Simulation files */
            FileSettings.underpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Cullerin Ranges\Increasing 3.31_ThuW1.csv";
            underpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredIncreasingSimulationFile);
            underpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.underpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Cullerin Ranges\Decreasing 3.33_TueW1.csv";
            underpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredDecreasingSimulationFile);
            underpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Cullerin Ranges\Increasing 4.8_FriW1.csv";
            overpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredIncreasingSimulationFile);
            overpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Cullerin Ranges\Decreasing 4.68_WedW1.csv";
            overpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredDecreasingSimulationFile);
            overpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            /* Destination Folder */
            FileSettings.aggregatedDestination = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Cullerin Ranges";
            ResultsDestination.Text = FileSettings.aggregatedDestination;
            ResultsDestination.ForeColor = System.Drawing.Color.Black;
        
            /* Settings */           
            fromDate.Value = new DateTime(2016,1,1);
            toDate.Value = new DateTime(2016,4,1);

            fromLatitude.Text = "-10";
            toLatitude.Text = "-40";
            fromLongitude.Text = "110";
            toLongitude.Text = "152";

            includeAListOfTrainsToExclude.Checked = false;

            startInterpolationKm.Text = "220";
            endInterpolationKm.Text = "320";
            interpolationInterval.Text = "50";
            minimumJourneyDistance.Text = "80";
            distanceThreshold.Text = "4";
            timeSeparation.Text = "10";

            loopBoundary.Text = "1";
            loopSpeedFactor.Text = "50";
            TSRWindowBoundary.Text = "1";

            underpoweredLowerBound.Text = "2";
            underpoweredUpperBound.Text = "4";
            overpoweredLowerBound.Text = "4";
            overpoweredUpperBound.Text = "6";

            HunterValley.Checked = false;

           
        }

        /// <summary>
        /// This function sets all the testing parameters for the Gunnedah Basin data
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void GunnedahBasin_Click(object sender, EventArgs e)
        {
            /* Data File */
            string batchFileName = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Gunnedah Basin\Gunnedah Basin Data 2016-2017.txt";
                //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Gunnedah Basin\Gunnedah Basin test data.csv";

            IceDataFile.Text = Path.GetFileName(batchFileName);
            simIceDataFile.Text = Path.GetFileName(batchFileName);

            IceDataFile.ForeColor = System.Drawing.Color.Black;
            simIceDataFile.ForeColor = System.Drawing.Color.Black;

            if (batchFileName != null || batchFileName != "")
                FileSettings.batchFiles.Add(batchFileName);

            /* Geometry File */
            FileSettings.geometryFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Gunnedah Basin\Gunnedah Basin Geometry.csv";
            GeometryFile.Text = Path.GetFileName(FileSettings.geometryFile);
            GeometryFile.ForeColor = System.Drawing.Color.Black;

            /* TSR File */
            FileSettings.temporarySpeedRestrictionFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Gunnedah Basin\Gunnedah Basin TSR 2016 - 2017.csv";
            temporarySpeedRestrictionFile.Text = Path.GetFileName(FileSettings.temporarySpeedRestrictionFile);
            temporarySpeedRestrictionFile.ForeColor = System.Drawing.Color.Black;

            /* Simulation files */
            FileSettings.underpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Gunnedah Basin\PacificNational-Increasing.csv"; //PN - Increasing.csv";
            underpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredIncreasingSimulationFile);
            underpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.underpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Gunnedah Basin\PacificNational-Decreasing.csv"; //PN - Decreasing.csv";
            underpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredDecreasingSimulationFile);
            underpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Gunnedah Basin\Aurizon-Increasing-60.csv"; //QR - Increasing.csv";
            overpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredIncreasingSimulationFile);
            overpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Gunnedah Basin\Aurizon-Decreasing.csv"; //QR - Decreasing.csv";
            overpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredDecreasingSimulationFile);
            overpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            /* Destination Folder */
            FileSettings.aggregatedDestination = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Gunnedah Basin";
            ResultsDestination.Text = FileSettings.aggregatedDestination;
            ResultsDestination.ForeColor = System.Drawing.Color.Black;

            /* Settings */

            fromDate.Value = new DateTime(2016,1,1);
            toDate.Value = new DateTime(2016,10,1);

            fromLatitude.Text = "-10";
            toLatitude.Text = "-40";
            fromLongitude.Text = "110";
            toLongitude.Text = "152";

            includeAListOfTrainsToExclude.Checked = false;

            startInterpolationKm.Text = "264" ;
            endInterpolationKm.Text = "541";
            interpolationInterval.Text = "50";
            minimumJourneyDistance.Text = "250";
            distanceThreshold.Text = "4";
            timeSeparation.Text = "10";

            loopBoundary.Text = "1";
            loopSpeedFactor.Text = "50";
            TSRWindowBoundary.Text = "1";

            /* Power to weight ratios done make sense for Gunnedah basin analysis, as this is between operators. */
            underpoweredLowerBound.Text = "0";
            underpoweredUpperBound.Text = "100";
            overpoweredLowerBound.Text = "100";
            overpoweredUpperBound.Text = "200";

            HunterValley.Checked = true;

        }

        /// <summary>
        /// This function sets all the testing parameters for the Macarthur to Botany data
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void Macarthur2Botany_Click(object sender, EventArgs e)
        { 
        
            /* Data File */
            string batchFileName = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\Macarthur to Botany test data.csv";

            IceDataFile.Text = Path.GetFileName(batchFileName);
            simIceDataFile.Text = Path.GetFileName(batchFileName);

            IceDataFile.ForeColor = System.Drawing.Color.Black;
            simIceDataFile.ForeColor = System.Drawing.Color.Black;

            if (batchFileName != null || batchFileName != "")
                FileSettings.batchFiles.Add(batchFileName);

            /* Geometry File */
            FileSettings.geometryFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\Macarthur to Botany Geometry.csv";
            GeometryFile.Text = Path.GetFileName(FileSettings.geometryFile);
            GeometryFile.ForeColor = System.Drawing.Color.Black;

            /* TSR File */
            FileSettings.temporarySpeedRestrictionFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Gunnedah Basin\Gunnedah Basin TSR.csv";
            temporarySpeedRestrictionFile.Text = Path.GetFileName(FileSettings.temporarySpeedRestrictionFile);
            temporarySpeedRestrictionFile.ForeColor = System.Drawing.Color.Black;

            /* Simulation files */
            FileSettings.underpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\Botany to Macarthur - increasing - 3.33_ThuW1.csv";
            underpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredIncreasingSimulationFile);
            underpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.underpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\Macarthur to Botany - decreasing - 3.20_SatW1.csv";
            underpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredDecreasingSimulationFile);
            underpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\Botany to Macarthur - increasing - 7.87_ThuW1.csv";
            overpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredIncreasingSimulationFile);
            overpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\Macarthur to Botany - decreasing - 6.97_SatW1.csv";
            overpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredDecreasingSimulationFile);
            overpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            /* Destination Folder */
            FileSettings.aggregatedDestination = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany";
            ResultsDestination.Text = FileSettings.aggregatedDestination;
            ResultsDestination.ForeColor = System.Drawing.Color.Black;

            /* Settings */

            fromDate.Value = new DateTime(2016,1,1);
            toDate.Value = new DateTime(2016,2,1);

            fromLatitude.Text = "-10";
            toLatitude.Text = "-40";
            fromLongitude.Text = "110";
            toLongitude.Text = "152";

            includeAListOfTrainsToExclude.Checked = false;

            startInterpolationKm.Text = "5";
            endInterpolationKm.Text = "70";
            interpolationInterval.Text = "50";
            minimumJourneyDistance.Text = "40";
            distanceThreshold.Text = "4";
            timeSeparation.Text = "10";

            loopBoundary.Text = "1";
            loopSpeedFactor.Text = "50";
            TSRWindowBoundary.Text = "1";

            underpoweredLowerBound.Text = "1.5";
            underpoweredUpperBound.Text = "4.5";
            overpoweredLowerBound.Text = "4.5";
            overpoweredUpperBound.Text = "11.5";

            HunterValley.Checked = false;

        }

        /// <summary>
        /// This function sets all the testing parameters for the Melbourne to Cootamundra data
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void Melbourne2Cootamundra_Click(object sender, EventArgs e)
        { 
        
            /* Data File */
            string batchFileName = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Melbourne to Cootamundra\Melbourne to Cootamundra test data.csv";

            IceDataFile.Text = Path.GetFileName(batchFileName);
            simIceDataFile.Text = Path.GetFileName(batchFileName);

            IceDataFile.ForeColor = System.Drawing.Color.Black;
            simIceDataFile.ForeColor = System.Drawing.Color.Black;

            if (batchFileName != null || batchFileName != "")
                FileSettings.batchFiles.Add(batchFileName);

            /* Geometry File */
            FileSettings.geometryFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Melbourne to Cootamundra\Melbourne to Cootamundra Geometry.csv";
            GeometryFile.Text = Path.GetFileName(FileSettings.geometryFile);
            GeometryFile.ForeColor = System.Drawing.Color.Black;

            /* TSR File */
            FileSettings.temporarySpeedRestrictionFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Melbourne to Cootamundra\Melbourne to Cootamundra TSR.csv";
            temporarySpeedRestrictionFile.Text = Path.GetFileName(FileSettings.temporarySpeedRestrictionFile);
            temporarySpeedRestrictionFile.ForeColor = System.Drawing.Color.Black;

            /* Simulation files */
            FileSettings.underpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Melbourne to Cootamundra\Increasing sim 3.5.csv";
            underpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredIncreasingSimulationFile);
            underpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.underpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Melbourne to Cootamundra\decreasing sim 3.5.csv";
            underpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredDecreasingSimulationFile);
            underpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Melbourne to Cootamundra\Increasing sim 4.6.csv";
            overpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredIncreasingSimulationFile);
            overpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Melbourne to Cootamundra\decreasing sim 4.6.csv";
            overpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredDecreasingSimulationFile);
            overpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            /* Destination Folder */
            FileSettings.aggregatedDestination = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Melbourne to Cootamundra";
            ResultsDestination.Text = FileSettings.aggregatedDestination;
            ResultsDestination.ForeColor = System.Drawing.Color.Black;

            /* Settings */

            fromDate.Value = new DateTime(2016,1,1);
            toDate.Value = new DateTime(2016,4,1);

            fromLatitude.Text = "-10";
            toLatitude.Text = "-40";
            fromLongitude.Text = "110";
            toLongitude.Text = "152";

            includeAListOfTrainsToExclude.Checked = false;

            startInterpolationKm.Text = "5";
            endInterpolationKm.Text = "505";
            interpolationInterval.Text = "50";
            minimumJourneyDistance.Text = "400";
            distanceThreshold.Text = "5";
            timeSeparation.Text = "10";

            loopBoundary.Text = "1";
            loopSpeedFactor.Text = "50";
            TSRWindowBoundary.Text = "1";
            
            underpoweredLowerBound.Text = "2.5";
            underpoweredUpperBound.Text = "4";
            overpoweredLowerBound.Text = "4";
            overpoweredUpperBound.Text = "5.5";

            HunterValley.Checked = false;

        }

        /// <summary>
        /// This function sets all the testing parameters for the Tarcoola To Kalgoorlie data
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void Tarcoola2Kalgoorlie_Click(object sender, EventArgs e)
        { 
        
            /* Data File */
            string batchFileName = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Tarcoola to Kalgoorlie\Tarcoola to Kalgoorlie test data.csv";

            IceDataFile.Text = Path.GetFileName(batchFileName);
            simIceDataFile.Text = Path.GetFileName(batchFileName);

            IceDataFile.ForeColor = System.Drawing.Color.Black;
            simIceDataFile.ForeColor = System.Drawing.Color.Black;

            if (batchFileName != null || batchFileName != "")
                FileSettings.batchFiles.Add(batchFileName);

            /* Geometry File */
            FileSettings.geometryFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Tarcoola to Kalgoorlie\Tarcoola to Kalgoorlie Geometry.csv";
            GeometryFile.Text = Path.GetFileName(FileSettings.geometryFile);
            GeometryFile.ForeColor = System.Drawing.Color.Black;

            /* TSR File */
            FileSettings.temporarySpeedRestrictionFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Tarcoola to Kalgoorlie\Tarcoola to Kalgoorlie TSR.csv";
            temporarySpeedRestrictionFile.Text = Path.GetFileName(FileSettings.temporarySpeedRestrictionFile);
            temporarySpeedRestrictionFile.ForeColor = System.Drawing.Color.Black;

            /* Simulation files */
            FileSettings.underpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Tarcoola to Kalgoorlie\increasing 2.2_ThuW1.csv";
            underpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredIncreasingSimulationFile);
            underpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.underpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Tarcoola to Kalgoorlie\decreasing 2.6_SunW1.csv";
            underpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredDecreasingSimulationFile);
            underpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Tarcoola to Kalgoorlie\increasing 3.4_FriW1.csv";
            overpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredIncreasingSimulationFile);
            overpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Tarcoola to Kalgoorlie\decreasing 3.5_MonW1.csv";
            overpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredDecreasingSimulationFile);
            overpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            /* Destination Folder */
            FileSettings.aggregatedDestination = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Tarcoola to Kalgoorlie";
            ResultsDestination.Text = FileSettings.aggregatedDestination;
            ResultsDestination.ForeColor = System.Drawing.Color.Black;

            /* Settings */
            fromDate.Value = new DateTime(2016,1,1);
            toDate.Value = new DateTime(2016,2,1);

            fromLatitude.Text = "-10";
            toLatitude.Text = "-40";
            fromLongitude.Text = "110";
            toLongitude.Text = "152";

            includeAListOfTrainsToExclude.Checked = false;

            startInterpolationKm.Text = "950";
            endInterpolationKm.Text = "1400";
            interpolationInterval.Text = "50";
            minimumJourneyDistance.Text = "350";
            distanceThreshold.Text = "4";
            timeSeparation.Text = "10";

            loopBoundary.Text = "1";
            loopSpeedFactor.Text = "50";
            TSRWindowBoundary.Text = "1";

            underpoweredLowerBound.Text = "1.5";
            underpoweredUpperBound.Text = "3";
            overpoweredLowerBound.Text = "3";
            overpoweredUpperBound.Text = "4";

            HunterValley.Checked = false;

        }

        /// <summary>
        /// This function sets all the testing parameters for the the Ulan line
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void Ulan_Click(object sender, EventArgs e)
        {
            /* Data File */
            string batchFileName = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Ulan\Ulan Data 2016-2017.txt";

            IceDataFile.Text = Path.GetFileName(batchFileName);
            simIceDataFile.Text = Path.GetFileName(batchFileName);

            IceDataFile.ForeColor = System.Drawing.Color.Black;
            simIceDataFile.ForeColor = System.Drawing.Color.Black;

            if (batchFileName != null || batchFileName != "")
                FileSettings.batchFiles.Add(batchFileName);

            /* Geometry File */
            FileSettings.geometryFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Ulan\Ulan Geometry.csv";
            GeometryFile.Text = Path.GetFileName(FileSettings.geometryFile);
            GeometryFile.ForeColor = System.Drawing.Color.Black;

            /* TSR File */
            FileSettings.temporarySpeedRestrictionFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Ulan\Ulan TSR.csv";
            temporarySpeedRestrictionFile.Text = Path.GetFileName(FileSettings.temporarySpeedRestrictionFile);
            temporarySpeedRestrictionFile.ForeColor = System.Drawing.Color.Black;

            /* Simulation files */
            FileSettings.underpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Ulan\Pacific National - Increasing.csv"; //PN - Increasing.csv";
            underpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredIncreasingSimulationFile);
            underpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.underpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Ulan\Pacific National - Decreasing.csv"; //PN - Decreasing.csv";
            underpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredDecreasingSimulationFile);
            underpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Ulan\Aurizon - Increasing.csv"; //QR - Increasing.csv";
            overpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredIncreasingSimulationFile);
            overpoweredIncreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.overpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Ulan\Aurizon - Decreasing.csv"; //QR - Decreasing.csv";
            overpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredDecreasingSimulationFile);
            overpoweredDecreasingSimulationFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.alternativeIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Ulan\Freightliner - Increasing.csv"; //Freightliner - Increasing.csv";
            alternativeIncreasingFile.Text = Path.GetFileName(FileSettings.alternativeIncreasingSimulationFile);
            alternativeIncreasingFile.ForeColor = System.Drawing.Color.Black;

            FileSettings.alternativeDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Ulan\Freightliner - Decreasing.csv"; //Freightliner - Decreasing.csv";
            alternativeDecreasingFile.Text = Path.GetFileName(FileSettings.alternativeDecreasingSimulationFile);
            alternativeDecreasingFile.ForeColor = System.Drawing.Color.Black;

            /* Destination Folder */
            FileSettings.aggregatedDestination = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Ulan";
            ResultsDestination.Text = FileSettings.aggregatedDestination;
            ResultsDestination.ForeColor = System.Drawing.Color.Black;

            /* Settings */

            fromDate.Value = new DateTime(2016, 1, 1);
            toDate.Value = new DateTime(2016, 2, 1);

            fromLatitude.Text = "-10";
            toLatitude.Text = "-40";
            fromLongitude.Text = "110";
            toLongitude.Text = "152";

            includeAListOfTrainsToExclude.Checked = false;

            startInterpolationKm.Text = "280";
            endInterpolationKm.Text = "460";
            interpolationInterval.Text = "50";
            minimumJourneyDistance.Text = "100";
            distanceThreshold.Text = "4";
            timeSeparation.Text = "10";

            loopBoundary.Text = "1";
            loopSpeedFactor.Text = "50";
            TSRWindowBoundary.Text = "1";

            /* Power to weight ratios done make sense for Ulan analysis, as this is between operators. */
            underpoweredLowerBound.Text = "0";
            underpoweredUpperBound.Text = "100";
            overpoweredLowerBound.Text = "100";
            overpoweredUpperBound.Text = "200";

            HunterValley.Checked = true;

        }

        /// <summary>
        /// This function resets the required lables on the fomr when changing between the 
        /// Hunter Valley region and the Interstate Network.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void hunterValley_CheckedChanged(object sender, EventArgs e)
        {
            if (HunterValley.Checked)
            {
                underpoweredFileLabel.Text = "Pacific National";
                underpoweredLabel.Text = "Pacific National:";
                SimUnderpoweredLabel.Text = "Pacific National:";

                overpoweredFileLabel.Text = "Aurizon";
                overpoweredLabel.Text = "Aurizon:";
                SimOverpoweredLabel.Text = "Aurizon:";

                alternativeFileLabel.Text = "Freightliner";

            }
            else 
            {
                underpoweredFileLabel.Text = "Underpowered";
                underpoweredLabel.Text = "Underpowered:";
                SimUnderpoweredLabel.Text = "Underpowered:";

                overpoweredFileLabel.Text = "Overpowered";
                overpoweredLabel.Text = "Overpowered:";
                SimOverpoweredLabel.Text = "Overpowered:";

                alternativeFileLabel.Text = "Alternative";
            }
        }

        /// <summary>
        /// Function determines if the testing parameters for Culleran Ranges need 
        /// to be set or resets to default settings.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void CulleranRanges_CheckedChanged(object sender, EventArgs e)
        {
            /* If Cullerin Ranges tesging flag is checked, set the appropriate parameters. */
            if (CulleranRanges.Checked)
                CulleranRanges_Click(sender, e);
            else
                resetDefaultParameters();
        }

        /// <summary>
        /// Function determines if the testing parameters for Gunnedah Basin need 
        /// to be set or resets to default settings.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void GunnedahBasin_CheckedChanged(object sender, EventArgs e)
        {
            /* If Gunnedah Basin testing flag is checked, set the appropriate parameters. */
            if (GunnedahBasin.Checked)
                GunnedahBasin_Click(sender, e);
            else
                resetDefaultParameters();
        }

        // <summary>
        /// Function determines if the testing parameters for the Ulan line need 
        /// to be set or resets to default settings.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void Ulan_CheckedChanged(object sender, EventArgs e)
        {
            /* If Gunnedah Basin testing flag is checked, set the appropriate parameters. */
            if (Ulan.Checked)
                Ulan_Click(sender, e);
            else
                resetDefaultParameters();
        }

        /// <summary>
        /// Function determines if the testing parameters for Macarthur to Botany need 
        /// to be set or resets to default settings.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void Macarthur2Botany_CheckedChanged(object sender, EventArgs e)
        {
            /* If Macarthur to Botany testing flag is checked, set the appropriate parameters. */
            if (Macarthur2Botany.Checked)
                Macarthur2Botany_Click(sender, e);
            else
                resetDefaultParameters();
        }

        /// <summary>
        /// Function determines if the testing parameters for Melbourne to Cootamundra need 
        /// to be set or resets to default settings.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void Melbourne2Cootamundra_CheckedChanged(object sender, EventArgs e)
        {
            /* If Melbourne to Cootamundra testing flag is checked, set the appropriate parameters. */
            if (Melbourne2Cootamundra.Checked)
                Melbourne2Cootamundra_Click(sender, e);
            else
                resetDefaultParameters();
        }

        /// <summary>
        /// Function determines if the testing parameters for Tarcoola to Kalgoorlie need 
        /// to be set or resets to default settings.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void Tarcoola2Kalgoorlie_CheckedChanged(object sender, EventArgs e)
        {
            /* If Tarcoola to Kalgoorlie testing flag is checked, set the appropriate parameters. */
            if (Tarcoola2Kalgoorlie.Checked)
                Tarcoola2Kalgoorlie_Click(sender, e);
            else
                resetDefaultParameters();
        }

        /// <summary>
        /// function resets the train performance analysis form to default settings.
        /// </summary>
        private void resetDefaultParameters()
        {

            /* Data File */
            IceDataFile.Text = "<Required>";
            IceDataFile.ForeColor = SystemColors.InactiveCaptionText;
            simIceDataFile.Text = "Data File Loaded from FileSelection tab";
            simIceDataFile.ForeColor = SystemColors.InactiveCaptionText;

            FileSettings.batchFiles.Clear(); 

            /* Geometry File */
            FileSettings.geometryFile = null;
            GeometryFile.Text = "<Required>";
            GeometryFile.ForeColor = SystemColors.InactiveCaptionText;

            /* TSR File */
            FileSettings.temporarySpeedRestrictionFile = null;
            temporarySpeedRestrictionFile.Text = "<Required>";
            temporarySpeedRestrictionFile.ForeColor = SystemColors.InactiveCaptionText;

            /* Simulation files */
            FileSettings.underpoweredIncreasingSimulationFile = null;
            underpoweredIncreasingSimulationFile.Text = "<Optional>";
            underpoweredIncreasingSimulationFile.ForeColor = SystemColors.InactiveCaptionText;

            FileSettings.underpoweredDecreasingSimulationFile = null;
            underpoweredDecreasingSimulationFile.Text = "<Optional>";
            underpoweredDecreasingSimulationFile.ForeColor = SystemColors.InactiveCaptionText;

            FileSettings.overpoweredIncreasingSimulationFile = null;
            overpoweredIncreasingSimulationFile.Text = "<Optional>";
            overpoweredIncreasingSimulationFile.ForeColor = SystemColors.InactiveCaptionText;

            FileSettings.overpoweredDecreasingSimulationFile = null;
            overpoweredDecreasingSimulationFile.Text = "<Optional>";
            overpoweredDecreasingSimulationFile.ForeColor = SystemColors.InactiveCaptionText;

            FileSettings.alternativeIncreasingSimulationFile = null;
            alternativeIncreasingFile.Text = "<Optional>";
            alternativeIncreasingFile.ForeColor = SystemColors.InactiveCaptionText;

            FileSettings.alternativeDecreasingSimulationFile = null;
            alternativeDecreasingFile.Text = "<Optional>";
            alternativeDecreasingFile.ForeColor = SystemColors.InactiveCaptionText;



            /* Destination Folder */
            FileSettings.aggregatedDestination = null;
            ResultsDestination.Text = "<Required>";
            ResultsDestination.ForeColor = SystemColors.InactiveCaptionText;

            /* Settings */
            fromDate.Value = new DateTime(2016, 1, 1);
            toDate.Value = new DateTime(2016, 2, 1);

            /* Geographic box for Australia */
            fromLatitude.Text = "-10";
            toLatitude.Text = "-40";
            fromLongitude.Text = "110";
            toLongitude.Text = "152";

            includeAListOfTrainsToExclude.Checked = false;

            startInterpolationKm.Text = "0";
            endInterpolationKm.Text = "100";
            interpolationInterval.Text = "50";
            minimumJourneyDistance.Text = "80";
            distanceThreshold.Text = "4";
            timeSeparation.Text = "10";

            loopBoundary.Text = "1";
            loopSpeedFactor.Text = "50";
            TSRWindowBoundary.Text = "1";

            underpoweredLowerBound.Text = "0";
            underpoweredUpperBound.Text = "0";
            overpoweredLowerBound.Text = "0";
            overpoweredUpperBound.Text = "0";

            HunterValley.Checked = false;
            
        
        }



    } // Partial Class TrainPerformance

    
  
}
