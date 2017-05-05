using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
                //FileSettings.dataFile = //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\raw data - sample.csv";
                //            @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\raw data - fulltest.csv";
                //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\raw data - interpolation test.csv";
                //@"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\Macarthur to Botany test data.csv";
                //tool.browseFile("Select the data file.");
                //IceDataFile.Text = Path.GetFileName(FileSettings.dataFile);
                //simIceDataFile.Text = Path.GetFileName(FileSettings.dataFile);
                batchFileName = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\raw data - fulltest.csv";
                IceDataFile.Text = Path.GetFileName(batchFileName);
                simIceDataFile.Text = Path.GetFileName(batchFileName);

                if (batchFileName != null || batchFileName != "")
                    FileSettings.batchFiles.Add(batchFileName);

            }
            else if (sender == selectDataFile1)
            {
                batchFileName = tool.browseFile("Select the data file.");
                IceDataFile1.Text = Path.GetFileName(batchFileName);
                simIceDataFile.Text = Path.GetFileName(batchFileName);

                if (batchFileName != null && batchFileName != "")
                    FileSettings.batchFiles.Add(batchFileName);
            }
            else if (sender == selectDataFile2)
            {
                batchFileName = tool.browseFile("Select the data file.");
                IceDataFile2.Text = Path.GetFileName(batchFileName);
                
                if (batchFileName != null || batchFileName != "")
                    FileSettings.batchFiles.Add(batchFileName);
            }
            else if (sender == selectDataFile3)
            {
                batchFileName = tool.browseFile("Select the data file.");
                IceDataFile3.Text = Path.GetFileName(batchFileName);
                
                if (batchFileName != null || batchFileName != "")
                    FileSettings.batchFiles.Add(batchFileName);
            }
            else if (sender == selectDataFile4)
            {
                batchFileName = tool.browseFile("Select the data file.");
                IceDataFile4.Text = Path.GetFileName(batchFileName);
                
                if (batchFileName != null || batchFileName != "")
                    FileSettings.batchFiles.Add(batchFileName);
            }
            else if (sender == selectDataFile5)
            {
                batchFileName = tool.browseFile("Select the data file.");
                IceDataFile5.Text = Path.GetFileName(batchFileName);
                
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
            FileSettings.geometryFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\Macarthur to Botany Geometry.csv";
            //tool.browseFile("Select the geometry file.");
            GeometryFile.Text = Path.GetFileName(FileSettings.geometryFile);
        }

        /// <summary>
        /// Select the simulation file with increasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectUnderpoweredIncreasingSimulationFile_Click(object sender, EventArgs e)
        {
            FileSettings.underpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Botany to Macarthur - All - 3.33_ThuW1.csv";
                //tool.browseFile("Select the increasing km simulation file.");
            underpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredIncreasingSimulationFile);
        }

        /// <summary>
        /// Selct the simulation file with decreasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectUnderpoweredDecreasingSimulationFile_Click(object sender, EventArgs e)
        {
            FileSettings.underpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Macarthur to Botany - All - 3.20_SatW1.csv";
            //tool.browseFile("Select the decreasing km simulation file.");
            underpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.underpoweredDecreasingSimulationFile);
        }
        /// <summary>
        /// Select the simulation file with increasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectOverpoweredIncreasingSimulationFile_Click(object sender, EventArgs e)
        {
            FileSettings.overpoweredIncreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Botany to Macarthur - All - 7.87_ThuW1.csv";
            //tool.browseFile("Select the increasing km simulation file.");
            overpoweredIncreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredIncreasingSimulationFile);
        }

        /// <summary>
        /// Selct the simulation file with decreasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectOverpoweredDecreasingSimulationFile_Click(object sender, EventArgs e)
        {
            FileSettings.overpoweredDecreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Macarthur to Botany - All - 6.97_SatW1.csv";
            //tool.browseFile("Select the decreasing km simulation file.");
            overpoweredDecreasingSimulationFile.Text = Path.GetFileName(FileSettings.overpoweredDecreasingSimulationFile);
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

            /* Run the train performance analysis. */
            List<Train> trains = new List<Train>();
            trains = Algorithm.trainPerformance();

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
            //if (FileSettings.dataFile == null || FileSettings.geometryFile == null)
            //    return;
            if (FileSettings.batchFiles.Count() == 0 || FileSettings.geometryFile == null)
                return;


            /* Ensure there is a empty list of trains to exclude to start. */
            List<string> excludeTrainList = new List<string> { };

            /* Populate the exluded train list. */
            if (Settings.includeAListOfTrainsToExclude)
                excludeTrainList = FileOperations.readTrainList(FileSettings.trainList);

            /* Read in the track gemoetry data. */
            List<TrackGeometry> trackGeometry = new List<TrackGeometry>();
            trackGeometry = track.readGeometryfile(FileSettings.geometryFile);


            /* Read the data. */
            List<TrainDetails> TrainRecords = new List<TrainDetails>();
            foreach (string file in FileSettings.batchFiles)
            {
                TrainRecords.AddRange(FileOperations.readICEData(file, excludeTrainList));
            }
            if (TrainRecords.Count() == 0)
            {
                tool.messageBox("There is no data within the specified boundaries.\nCheck the processing parameters.");
                return;
            }


            /* Sort the data by [trainID, locoID, Date & Time, kmPost]. */
            List<TrainDetails> OrderdTrainRecords = new List<TrainDetails>();
            OrderdTrainRecords = TrainRecords.OrderBy(t => t.TrainID).ThenBy(t => t.LocoID).ThenBy(t => t.NotificationDateTime).ThenBy(t => t.kmPost).ToList();

            /* Clean data - remove trains with insufficient data. */
            /******** Should only be required while we are waiting for the data in the prefered format ********/
            List<Train> CleanTrainRecords = new List<Train>();
            CleanTrainRecords = Algorithm.CleanData(trackGeometry, OrderdTrainRecords);
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
                return loopFactor;

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

        private void button3_Click(object sender, EventArgs e)
        {

        }

      
    } // Partial Class TrainPerformance

    
  
}
