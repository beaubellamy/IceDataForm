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

namespace IceDataForm2
{

    public partial class Form1 : Form
    {
        
        public static Tools tool = new Tools();
        public static Processing processing = new Processing();
        public static TrackGeometry track = new TrackGeometry();

        public Form1()
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
            
            FileSettings.dataFile = tool.browseFile("Select the data file.");
            IceDataFile.Text = Path.GetFileName(FileSettings.dataFile);
            simIceDataFile.Text = Path.GetFileName(FileSettings.dataFile);
        }

        /// <summary>
        /// Select the geometry file.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectGeometryFile_Click(object sender, EventArgs e)
        {
            FileSettings.geometryFile = tool.browseFile("Select the geometry file.");
            GeometryFile.Text = Path.GetFileName(FileSettings.geometryFile);
        }

        /// <summary>
        /// Select the simulation file with increasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectIncreasingSimulationFile_Click(object sender, EventArgs e)
        {
            FileSettings.increasingSimulationFile = tool.browseFile("Select the increasing km simulation file.");
            IncreasingSimulationFile.Text = Path.GetFileName(FileSettings.increasingSimulationFile);
        }

        /// <summary>
        /// Selct the simulation file with decreasing km.
        /// </summary>
        /// <param name="sender">The object container.</param>
        /// <param name="e">The event arguments.</param>
        private void selectDecreasingSimulationFile_Click(object sender, EventArgs e)
        {
            FileSettings.decreasingSimulationFile = tool.browseFile("Select the decreasing km simulation file.");
            DecreasingSimulationFile.Text = Path.GetFileName(FileSettings.decreasingSimulationFile);
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

            /* Run the train performance analysis. */
            Algorithm.trainPerformance();

        }

        private void averagePowerToWeightRatios_Click(object sender, EventArgs e)
        {
            
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
            TrainRecords = FileOperations.readICEData(FileSettings.dataFile, excludeTrainList);

            /* Sort the data by [trainID, locoID, Date & Time, kmPost]. */
            List<TrainDetails> OrderdTrainRecords = new List<TrainDetails>();
            OrderdTrainRecords = TrainRecords.OrderBy(t => t.TrainID).ThenBy(t => t.LocoID).ThenBy(t => t.NotificationDateTime).ThenBy(t => t.kmPost).ToList();

            /* Clean data - remove trains with insufficient data. */
            /******** Should only be required while we are waiting for the data in the prefered format ********/
            List<Train> CleanTrainRecords = new List<Train>();
            CleanTrainRecords = Algorithm.CleanData(trackGeometry, OrderdTrainRecords);
            
            
            underpoweredIncreasingP2W.Text = averagePowerToWeightRatio(CleanTrainRecords, Settings.underpoweredLowerBound, Settings.underpoweredUpperBound, direction.increasing).ToString();
            underpoweredDecreasingP2W.Text = averagePowerToWeightRatio(CleanTrainRecords, Settings.underpoweredLowerBound, Settings.underpoweredUpperBound, direction.decreasing).ToString();

            overpoweredIncreasingP2W.Text = averagePowerToWeightRatio(CleanTrainRecords, Settings.overpoweredLowerBound, Settings.overpoweredUpperBound, direction.increasing).ToString();
            overpoweredDecreasingP2W.Text = averagePowerToWeightRatio(CleanTrainRecords, Settings.overpoweredLowerBound, Settings.overpoweredUpperBound, direction.decreasing).ToString();

            combinedIncreasingP2W.Text = averagePowerToWeightRatio(CleanTrainRecords, Settings.underpoweredLowerBound, Settings.overpoweredUpperBound, direction.increasing).ToString();
            combinedDecreasingP2W.Text = averagePowerToWeightRatio(CleanTrainRecords, Settings.underpoweredLowerBound, Settings.overpoweredUpperBound, direction.decreasing).ToString();

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
        


    }

    
  
}
