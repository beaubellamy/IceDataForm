﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsettings;

namespace IceDataForm2
{

    /// <summary>
    /// Enumerated direction of the train km's.
    /// </summary>
    public enum direction { increasing, decreasing, notSpecified };


    /// <summary>
    /// A class to hold the train details of each record.
    /// </summary>
    public class TrainDetails
    {

        public string TrainID;
        public string LocoID;
        public double powerToWeight;
        public DateTime NotificationDateTime;
        public GeoLocation location = new GeoLocation();
        public double speed;
        public double kmPost;
        public double geometryKm;
        public direction trainDirection;
        public bool isLoopHere;
        public bool isTSRHere;
        public double TSRspeed;

        /// <summary>
        /// Default trainDetails constructor.
        /// </summary>
        public TrainDetails()
        {
            this.TrainID = "none";
            this.LocoID = "none";
            this.powerToWeight = 1;
            this.NotificationDateTime = new DateTime(2000, 1, 1, 0, 0, 0);
            this.location.latitude = -33.8519;   //Sydney Harbour Bridge
            this.location.longitude = 151.2108;
            this.speed = 0;
            this.kmPost = 0;
            this.geometryKm = 0;
            this.trainDirection = direction.notSpecified;
            this.isLoopHere = false;
            this.isTSRHere = false;
            this.TSRspeed = 0;

        }

        /// <summary>
        /// TrainDetails constructor
        /// </summary>
        /// <param name="TrainID">The Train ID.</param>
        /// <param name="locoID">The Locomotive ID.</param>
        /// <param name="NotificationDateTime">The date and time of the record.</param>
        /// <param name="latitude">The latitude of the train.</param>
        /// <param name="longitude">The longitude of the train.</param>
        /// <param name="speed">The instantaneous speed of the train.</param>
        /// <param name="kmPost">The closest km marker to the train at the time of recording</param>
        /// <param name="geometryKm">The calcualted distance from the km post of the first point.</param>
        /// <param name="trainDirection">The train bearing.</param>
        public TrainDetails(string TrainID, string locoID, double powerToWeight, DateTime NotificationDateTime, double latitude, double longitude,
                            double speed, double kmPost, double geometryKm, direction trainDirection, bool loop, bool TSR, double TSRspeed)
        {
            this.TrainID = TrainID;
            this.LocoID = locoID;
            this.powerToWeight = powerToWeight;
            this.NotificationDateTime = NotificationDateTime;
            this.location.latitude = latitude;
            this.location.longitude = longitude;
            this.speed = speed;
            this.kmPost = kmPost;
            this.geometryKm = geometryKm;
            this.trainDirection = trainDirection;
            this.isLoopHere = loop;
            this.isTSRHere = TSR;
            this.TSRspeed = TSRspeed;

        }

    }

    /// <summary>
    /// A Class to hold the train journey details for an individual train.
    /// </summary>
    public class Train
    {
        public List<TrainDetails> TrainJourney;
        public bool include;                        // Include the train in the data.

        /// <summary>
        /// Default constructor
        /// </summary>
        public Train()
        {
            this.TrainJourney = null;
            this.include = false;
        }

        /// <summary>
        /// Train Object constructor.
        /// </summary>
        /// <param name="trainDetails">The list of trainDetails objects containing the details of the train journey.</param>
        public Train(List<TrainDetails> trainDetails)
        {
            this.TrainJourney = trainDetails;
            this.include = true;
        }

        /// <summary>
        /// Train Object constructor.
        /// </summary>
        /// <param name="trainDetails">The list of trainDetails objects containing the details of the train journey.</param>
        /// <param name="include">A flag to determine if the train journey will be included in the data.</param>
        public Train(List<TrainDetails> trainDetails, bool include)
        {
            this.TrainJourney = trainDetails;
            this.include = include;
        }

        /// <summary>
        /// Train Object constructor converting an list of interpolatedTrain objects into a list of trainDetail objects.
        /// </summary>
        /// <param name="trainDetails">A list of interpolatedTrain objects.</param>
        /// <param name="trainDirection">The direction of kilometreage of the train.</param>
        public Train(List<InterpolatedTrain> trainDetails, direction trainDirection)
        {
            double powerToWeight = 1;
            List<TrainDetails> journey = new List<TrainDetails>();

            for (int journeyIdx = 0; journeyIdx < trainDetails.Count(); journeyIdx++)
            {
                /* Convert each interpolatedTrain object to a trainDetail object. */
                TrainDetails newitem = new TrainDetails(trainDetails[journeyIdx].TrainID, trainDetails[journeyIdx].LocoID, powerToWeight, trainDetails[journeyIdx].NotificationDateTime, 0, 0,
                                                        trainDetails[journeyIdx].speed, 0, trainDetails[journeyIdx].geometryKm, trainDirection, trainDetails[journeyIdx].isLoopeHere,
                                                        trainDetails[journeyIdx].isTSRHere, trainDetails[journeyIdx].TSRspeed);


                journey.Add(newitem);

            }
            this.TrainJourney = journey;
            this.include = true;
        }

        /// <summary>
        /// Determine the index of the geomerty data for the supplied kilometreage.
        /// </summary>
        /// <param name="TrainJourney">List of train details objects containt the journey details of the train.</param>
        /// <param name="targetKm">The target location to find in the geomerty data.</param>
        /// <returns>The index of the target kilometreage in teh geomerty data, -1 if the target is not found.</returns>
        public int indexOfgeometryKm(List<TrainDetails> TrainJourney, double targetKm)
        {
            /* Loop through the train journey. */
            for (int journeyIdx = 0; journeyIdx < TrainJourney.Count(); journeyIdx++)
            {
                /* match the current location with the geometry information. */
                if (Math.Abs(TrainJourney[journeyIdx].geometryKm - targetKm) * 1e12 < 1)
                    return journeyIdx;
            }

            return -1;
        }

    }

    /// <summary>
    /// A class to hold the interpolated train details.
    /// </summary>
    public class InterpolatedTrain
    {
        public string TrainID;
        public string LocoID;
        public double powerToWeight;
        public DateTime NotificationDateTime;
        public double speed;
        public double geometryKm;
        public bool isLoopeHere;
        public bool isTSRHere;
        public double TSRspeed;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public InterpolatedTrain()
        {
            this.TrainID = null;
            this.LocoID = null;
            this.powerToWeight = 1;
            this.NotificationDateTime = new DateTime(2000, 1, 1, 0, 0, 0);
            this.speed = 0;
            this.geometryKm = 0;
            this.isLoopeHere = false;
            this.isTSRHere = false;
            this.TSRspeed = 0;
        }

        /// <summary>
        /// InterpolatedTrain object constructor.
        /// </summary>
        /// <param name="TrainID">The train ID.</param>
        /// <param name="locoID">The Loco ID</param>
        /// <param name="NotificationDateTime">The intiial notification time for the start of the data.</param>
        /// <param name="geometryKm">The calculated actual kilometerage of the train.</param>
        /// <param name="speed">The speed (kph) at each location.</param>
        public InterpolatedTrain(string TrainID, string locoID, double powerToWeight, DateTime NotificationDateTime, double geometryKm, double speed, bool loop, bool TSR, double TSRspeed)
        {
            this.TrainID = TrainID;
            this.LocoID = locoID;
            this.powerToWeight = powerToWeight;
            this.NotificationDateTime = NotificationDateTime;
            this.geometryKm = geometryKm;
            this.speed = speed;
            this.isLoopeHere = loop;
            this.isTSRHere = TSR;
            this.TSRspeed = TSRspeed;
        }

        /// <summary>
        /// InterpolatedTrain object constructor to convert a trainDetails object into an interpolatedTrain object.
        /// </summary>
        /// <param name="details">The trainDetails object containing the train journey parameters.</param>
        public InterpolatedTrain(TrainDetails details)
        {
            this.TrainID = details.TrainID;
            this.LocoID = details.LocoID;
            this.powerToWeight = details.powerToWeight;
            this.NotificationDateTime = details.NotificationDateTime;
            this.geometryKm = details.geometryKm;
            this.speed = details.speed;
            this.isLoopeHere = details.isLoopHere;
            this.isTSRHere = details.isTSRHere;
            this.TSRspeed = details.TSRspeed;
        }
    }

    /// <summary>
    /// A class to hold the simulated train data.
    /// </summary>
    public class simulatedTrain
    {
        public double kmPoint;
        public double singleLineKm;
        public GeoLocation location = new GeoLocation();
        public double elevation;
        public string TraximNode;
        public string TraximSection;
        public double time;
        public double velocity;
        public double previousDistance;
        public double maxSpeed;

        /// <summary>
        /// Default SimulatedTrain constructor.
        /// </summary>
        public simulatedTrain()
        {
            kmPoint = 0;
            singleLineKm = 0;
            location.latitude = -33.8519;   //Sydney Harbour Bridge
            location.longitude = 151.2108;
            elevation = 0;
            TraximNode = "none";
            TraximSection = "none";
            time = 0;
            velocity = 0;
            previousDistance = 0;
            maxSpeed = 0;
        }

        /// <summary>
        /// SimulatedTrain object constructor
        /// </summary>
        /// <param name="kmPoint">kilometreage of the simualted train.</param>
        /// <param name="singleLineKm">Cummulative kilometreage of the simulated train.</param>
        /// <param name="location">Geographoc location of the simualted train.</param>
        /// <param name="elevation">Elevation of the simulated train.</param>
        /// <param name="TraximNode">The Traxim node relevant to the current location</param>
        /// <param name="TraximSection">The Traxim section relevant to the current location</param>
        /// <param name="time">Cummulative time in seconds of the simulated train.</param>
        /// <param name="velocity">The instantaneous velocity of the simualted train at the current location.</param>
        /// <param name="previousDistance">The distance in metres traveled between the previous position and the current position.</param>
        /// <param name="maxSpeed">The maximum permissable speed at the current location.</param>
        public simulatedTrain(double kmPoint, double singleLineKm, GeoLocation location, double elevation, string TraximNode, string TraximSection,
                            double time, double velocity, double previousDistance, double maxSpeed)
        {
            this.kmPoint = kmPoint;
            this.singleLineKm = singleLineKm;
            this.location = location;
            this.elevation = elevation;
            this.TraximNode = TraximNode;
            this.TraximSection = TraximSection;
            this.time = time;
            this.velocity = velocity;
            this.previousDistance = previousDistance;
            this.maxSpeed = maxSpeed;
        }

        /// <summary>
        /// SimulatedTrain object constructor
        /// </summary>
        /// <param name="kmPoint">kilometreage of the simualted train.</param>
        /// <param name="singleLineKm">Cummulative kilometreage of the simulated train.</param>/// <param name="latitude"></param>
        /// <param name="latitude">The latitude of the current location.</param>
        /// <param name="longitude">The longitude of the current location.</param>
        /// <param name="elevation">Elevation of the simulated train.</param>
        /// <param name="TraximNode">The Traxim node relevant to the current location</param>
        /// <param name="TraximSection">The Traxim section relevant to the current location</param>
        /// <param name="time">Cummulative time in seconds of the simulated train.</param>
        /// <param name="velocity">The instantaneous velocity of the simualted train at the current location.</param>
        /// <param name="previousDistance">The distance in metres traveled between the previous position and the current position.</param>
        /// <param name="maxSpeed">The maximum permissable speed at the current location.</param>
        public simulatedTrain(double kmPoint, double singleLineKm, double latitude, double longitude, double elevation, string TraximNode, string TraximSection,
                            double time, double velocity, double previousDistance, double maxSpeed)
        {
            this.kmPoint = kmPoint;
            this.singleLineKm = singleLineKm;
            this.location.latitude = latitude;
            this.location.longitude = longitude;
            this.elevation = elevation;
            this.TraximNode = TraximNode;
            this.TraximSection = TraximSection;
            this.time = time;
            this.velocity = velocity;
            this.previousDistance = previousDistance;
            this.maxSpeed = maxSpeed;
        }


    }

    /// <summary>
    /// A class to hold all averaged train data.
    /// </summary>
    public class averagedTrainData
    {
        public double kilometerage;
        public double underpoweredIncreaseingAverage;
        public double underpoweredDecreaseingAverage;
        public double overpoweredIncreaseingAverage;
        public double overpoweredDecreaseingAverage;
        public bool isInLoopBoundary;

        /// <summary>
        /// Default averageTrainData Constructor.
        /// </summary>
        public averagedTrainData()
        {
            this.kilometerage = 0;
            this.underpoweredIncreaseingAverage = 0;
            this.underpoweredDecreaseingAverage = 0;
            this.overpoweredIncreaseingAverage = 0;
            this.overpoweredDecreaseingAverage = 0;
            this.isInLoopBoundary = false;
        }

        /// <summary>
        /// averageTrainData Constructor.
        /// </summary>
        /// <param name="km">Kilometerage of each location</param>
        /// <param name="underIncreasing">Average Speed at kilometreage for the underpowered category in the increasing direction.</param>
        /// <param name="underDecreasing">Average Speed at kilometreage for the underpowered category in the decreasing direction.</param>
        /// <param name="overIncreasing">Average Speed at kilometreage for the overpowered category in the increasing direction.</param>
        /// <param name="overDecreasing">Average Speed at kilometreage for the overpowered category in the decreasing direction.</param>
        /// <param name="loop">Flag indicating if the location is within the boudanry of a loop.</param>
        public averagedTrainData(double km, double underIncreasing, double underDecreasing, double overIncreasing, double overDecreasing, bool loop)
        {
            this.kilometerage = km;
            this.underpoweredIncreaseingAverage = underIncreasing;
            this.underpoweredDecreaseingAverage = underDecreasing;
            this.overpoweredIncreaseingAverage = overIncreasing;
            this.overpoweredDecreaseingAverage = overDecreasing;
            this.isInLoopBoundary = loop;
        }

    }
    
    /// <summary>
    /// A class describing a geographic location with latitude and longitude.
    /// </summary>
    public class GeoLocation
    {
        /* Latitude and longitude of the location */
        public double latitude;
        public double longitude;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GeoLocation()
        {
            // Default: Sydney Harbour Bridge
            this.latitude = -33.8519;
            this.longitude = 151.2108;
        }

        /// <summary>
        /// Geolocation constructor
        /// </summary>
        /// <param name="lat">latitude of the location.</param>
        /// <param name="lon">longitude of the location.</param>
        public GeoLocation(double lat, double lon)
        {
            this.latitude = lat;
            this.longitude = lon;
        }

        /// <summary>
        /// Geolocation constructor
        /// </summary>
        /// <param name="trainDetails">The trainDetails object containing the latitude and longitude of the location.</param>
        public GeoLocation(TrainDetails trainDetails)
        {
            this.latitude = trainDetails.location.latitude;
            this.longitude = trainDetails.location.longitude;
        }

        /// <summary>
        /// Geolocation constructor
        /// </summary>
        /// <param name="simulation">Simulated data object containing latitude and longitude of the location.</param>
        public GeoLocation(simulatedTrain simulation)
        {
            this.latitude = simulation.location.latitude;
            this.longitude = simulation.location.longitude;
        }


    }

    /// <summary>
    /// A class describing the parameters associated with a TSR.
    /// </summary>
    public class TSRObject
    {
        public bool isTSRHere;
        public double TSRSpeed;

        /// <summary>
        /// Default TSRObject constructor.
        /// </summary>
        public TSRObject()
        {
            this.isTSRHere = false;
            this.TSRSpeed = 0;
        }

    }



    class Algorithm
    {
        /* Create a tools Class. */
        public static Tools tool = new Tools();
        /* Create a processing Class. */
        public static Processing processing = new Processing();
        /* Create a trackGeometry Class. */
        public static TrackGeometry track = new TrackGeometry();


        /// <summary>
        /// Determine the average train performance in both directions based on the supplied 
        /// actual train data. The included data is restricted to values of the power to weight 
        /// ratios for the underpowered and overpowered catagories on the form. The loop 
        /// locations and TSR information is also used to extract the data that corresponds 
        /// to a train that enteres a loop and is bound by a TSR. If the train is within the 
        /// 'loop bounday threshold' and is deemed to be stopping in the loop, the data at this 
        /// location is not included. The train is deemed to be stopping in a loop if the train 
        /// speed drops below the simulated speed multiplied by the 'loop speed factor'. If the 
        /// train is within the 'TSR window', the data at this location is ignored as the train 
        /// is bound by the TSR at the location. The average train is then determined from the 
        /// included train data.
        /// 
        /// This function produces a number of ouput files containing the data through the 
        /// progresive steps.
        /// </summary>        
        [STAThread]
        public static List<Train> trainPerformance()
        {


            /* Ensure there is a empty list of trains to exclude to start. */
            List<string> excludeTrainList = new List<string> { };

            /* Use a browser to select the desired data file. */
            //string filename = null;
            //string geometryFile = null;
            //string trainList = null;
            //string increasingSimulationFile = null;
            //string decreasingSimulationFile = null;

            ///* Select the data file and the trainList file. */
            //filename = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\raw data - sample.csv";
            ////tool.browseFile("Select the data file.");
            //geometryFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis\Macarthur to Botany\Macarthur to Botany Geometry.csv";
            ////tool.browseFile("Select the geometry file.");
            //increasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Botany to Macarthur - All - 3.33_ThuW1.csv";
            ////tool.browseFile("Seelect the Simulation file with increasing km.");
            //decreasingSimulationFile = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Traxim\2017\Projects\Macarthur to Botany\Macarthur to Botany - All - 3.20_SatW1.csv";
            ////tool.browseFile("Seelect the Simulation file with decreasing km.");

            /* Populate the exluded train list. */
            if (Settings.includeAListOfTrainsToExclude)
                excludeTrainList = FileOperations.readTrainList(FileSettings.trainList);


            /* Read in the track gemoetry data. */
            List<TrackGeometry> trackGeometry = new List<TrackGeometry>();
            trackGeometry = track.readGeometryfile(FileSettings.geometryFile);

            /* Read in the simulation data and interpolate to the desired interval. */
            /* Underpowered Simualtions. */
            List<simulatedTrain> underpoweredIncreasingSimulation = new List<simulatedTrain>();
            underpoweredIncreasingSimulation = FileOperations.readSimulationData(FileSettings.underpoweredIncreasingSimulationFile);
            List<InterpolatedTrain> simulationUnderpoweredIncreasing = new List<InterpolatedTrain>();
            simulationUnderpoweredIncreasing = processing.interpolateSimulationData(underpoweredIncreasingSimulation, trackGeometry);

            List<simulatedTrain> underpoweredDecreasingSimulation = new List<simulatedTrain>();
            underpoweredDecreasingSimulation = FileOperations.readSimulationData(FileSettings.underpoweredDecreasingSimulationFile);
            underpoweredDecreasingSimulation = underpoweredDecreasingSimulation.OrderBy(t => t.singleLineKm).ToList();
            List<InterpolatedTrain> simulationUnderpoweredDecreasing = new List<InterpolatedTrain>();
            simulationUnderpoweredDecreasing = processing.interpolateSimulationData(underpoweredDecreasingSimulation, trackGeometry);
            
            /* Ovderpowered Simualtions. */
            List<simulatedTrain> overpoweredIncreasingSimulation = new List<simulatedTrain>();
            overpoweredIncreasingSimulation = FileOperations.readSimulationData(FileSettings.overpoweredIncreasingSimulationFile);
            List<InterpolatedTrain> simulationOverpoweredIncreasing = new List<InterpolatedTrain>();
            simulationOverpoweredIncreasing = processing.interpolateSimulationData(overpoweredIncreasingSimulation, trackGeometry);

            List<simulatedTrain> overpoweredDecreasingSimulation = new List<simulatedTrain>();
            overpoweredDecreasingSimulation = FileOperations.readSimulationData(FileSettings.overpoweredDecreasingSimulationFile);
            overpoweredDecreasingSimulation = overpoweredDecreasingSimulation.OrderBy(t => t.singleLineKm).ToList();
            List<InterpolatedTrain> simulationOverpoweredDecreasing = new List<InterpolatedTrain>();
            simulationOverpoweredDecreasing = processing.interpolateSimulationData(overpoweredDecreasingSimulation, trackGeometry);

            /* Sort the decreasing data to match the increasing data. */
            //simulationUnderpoweredDecreasing = simulationUnderpoweredDecreasing.OrderBy(t => t.geometryKm).ToList();
            //simulationOverpoweredDecreasing = simulationOverpoweredDecreasing.OrderBy(t => t.geometryKm).ToList();
                        
            
            /* Read the data. */
            List<TrainDetails> TrainRecords = new List<TrainDetails>();
            TrainRecords = FileOperations.readICEData(FileSettings.dataFile, excludeTrainList);

            /* Sort the data by [trainID, locoID, Date & Time, kmPost]. */
            List<TrainDetails> OrderdTrainRecords = new List<TrainDetails>();
            OrderdTrainRecords = TrainRecords.OrderBy(t => t.TrainID).ThenBy(t => t.LocoID).ThenBy(t => t.NotificationDateTime).ThenBy(t => t.kmPost).ToList();

            /* Clean data - remove trains with insufficient data. */
            /******** Should only be required while we are waiting for the data in the prefered format ********/
            List<Train> CleanTrainRecords = new List<Train>();
            CleanTrainRecords = CleanData(trackGeometry, OrderdTrainRecords);

            /* interpolate data */
            /******** Should only be required while we are waiting for the data in the prefered format ********/
            List<Train> interpolatedRecords = new List<Train>();
            interpolatedRecords = processing.interpolateTrainData(CleanTrainRecords, trackGeometry);
            List<InterpolatedTrain> unpackedInterpolation = new List<InterpolatedTrain>();
            unpackedInterpolation = unpackInterpolatedData(interpolatedRecords);
            FileOperations.writeTrainData(unpackedInterpolation);

            /* Average the train data for each direction with regard for TSR's and loop locations. */
            List<averagedTrainData> averageData = new List<averagedTrainData>();
            averageData = processing.powerToWeightAverageSpeed(interpolatedRecords, simulationUnderpoweredIncreasing, simulationUnderpoweredDecreasing, simulationOverpoweredIncreasing, simulationOverpoweredDecreasing);

            /* Write the averaged Data to file for inspection. */
            FileOperations.writeAverageData(averageData);
            
            /* seperate averages for P/W ratio groups, commodity, Operator */

            /* Unpack the records into a single trainDetails object list. */
            List<TrainDetails> unpackedData = new List<TrainDetails>();
            unpackedData = unpackCleanData(CleanTrainRecords);

            /* Write data to an excel file. */
            FileOperations.writeTrainData(unpackedData);

            return interpolatedRecords;
        }

        /// <summary>
        /// Remove the whole train journey that does not contain successive points that conform to 
        /// the minimum distance threshold.
        /// </summary>
        /// <param name="trackGeometry">A list of track Geometry objects</param>
        /// <param name="OrderdTrainRecords">List of TrainDetail objects</param>
        /// <returns>List of Train objects containign the journey details of each train.</returns>
        public static List<Train> CleanData(List<TrackGeometry> trackGeometry, List<TrainDetails> OrderdTrainRecords)
        {
            bool removeTrain = false;
            double distance = 0;
            double journeyDistance = 0;

            GeoLocation point1 = null;
            GeoLocation point2 = null;

            /* Place holder for the train records that are acceptable. */
            List<TrainDetails> newTrainList = new List<TrainDetails>();
            /* List of each Train with its journey details that is acceptable. */
            List<Train> cleanTrainList = new List<Train>();

            /* Add the first record to the list. */
            newTrainList.Add(OrderdTrainRecords[0]);
            GeoLocation trainPoint = new GeoLocation(OrderdTrainRecords[0]);
            /* Populate the first actual kilometreage point. */
            newTrainList[0].geometryKm = track.findClosestTrackGeometryPoint(trackGeometry, trainPoint);


            for (int trainIndex = 1; trainIndex < OrderdTrainRecords.Count(); trainIndex++)
            {

                if (OrderdTrainRecords[trainIndex].TrainID.Equals(OrderdTrainRecords[trainIndex - 1].TrainID) &&
                    OrderdTrainRecords[trainIndex].LocoID.Equals(OrderdTrainRecords[trainIndex - 1].LocoID) &&
                    (OrderdTrainRecords[trainIndex].NotificationDateTime - OrderdTrainRecords[trainIndex - 1].NotificationDateTime).TotalMinutes < Settings.timeThreshold)
                {
                    /* If the current and previous record represent the same train journey, add it to the list. */
                    newTrainList.Add(OrderdTrainRecords[trainIndex]);

                    point1 = new GeoLocation(OrderdTrainRecords[trainIndex - 1]);
                    point2 = new GeoLocation(OrderdTrainRecords[trainIndex]);

                    distance = processing.calculateDistance(point1, point2);
                    journeyDistance = journeyDistance + distance;

                    if (distance > Settings.distanceThreshold)
                    {
                        /* If the distance between successive km points is greater than the
                         * threshold then we want to remove this train from the data. 
                         */
                        removeTrain = true;
                    }

                }
                else
                {
                    /* The end of the train journey had been reached. */
                    if (!removeTrain && journeyDistance > Settings.minimumJourneyDistance)
                    {
                        /* If all points are acceptable and the train travels the minimum distance, 
                         * add the train journey to the cleaned list. 
                         */
                        Train item = new Train();
                        item.TrainJourney = newTrainList.ToList();

                        /* Determine direction and actual km. */
                        processing.populateDirection(item);
                        processing.populateGeometryKm(item);
                        processing.populateLoopLocations(item, trackGeometry);
                        processing.populateTemporarySpeedRestrictions(item, trackGeometry);

                        /* Sort the journey in ascending order. */
                        item.TrainJourney = item.TrainJourney.OrderBy(t => t.geometryKm).ToList();

                        cleanTrainList.Add(item);

                    }

                    /* Reset the parameters for the next train. */
                    removeTrain = false;
                    journeyDistance = 0;
                    newTrainList.Clear();

                    /* Add the first record of the new train journey. */
                    newTrainList.Add(OrderdTrainRecords[trainIndex]);
                    trainPoint = new GeoLocation(OrderdTrainRecords[trainIndex]);
                    newTrainList[0].geometryKm = track.findClosestTrackGeometryPoint(trackGeometry, trainPoint);
                }

                /* The end of the records have been reached. */
                if (trainIndex == OrderdTrainRecords.Count() - 1 && !removeTrain)
                {
                    /* If all points are aceptable, add the train journey to the cleaned list. */
                    Train item = new Train();
                    item.TrainJourney = newTrainList.ToList();
                    processing.populateDirection(item);
                    processing.populateGeometryKm(item);
                    processing.populateLoopLocations(item, trackGeometry);
                    processing.populateTemporarySpeedRestrictions(item, trackGeometry);
                    
                    /* Sort the journey in ascending order. */
                    item.TrainJourney = item.TrainJourney.OrderBy(t => t.geometryKm).ToList();

                    cleanTrainList.Add(item);

                }

            }

            return cleanTrainList;

        }

        /// <summary>
        /// Unpack the Train data structure into a single list of TrainDetails objects.
        /// </summary>
        /// <param name="OrderdTrainRecords">The Train object containing a list of trains with their journey details.</param>
        /// <returns>A single list of TrainDetail objects.</returns>
        public static List<TrainDetails> unpackCleanData(List<Train> OrderdTrainRecords)
        {
            /* Place holder to store all train records in one list. */
            List<TrainDetails> unpackedData = new List<TrainDetails>();

            /* Cycle through each train. */
            foreach (Train train in OrderdTrainRecords)
            {
                /* Cycle through each record in the train journey. */
                for (int journeyIdx = 0; journeyIdx < train.TrainJourney.Count(); journeyIdx++)
                {
                    /* Add it to the list. */
                    unpackedData.Add(train.TrainJourney[journeyIdx]);
                }
            }
            return unpackedData;
        }

        /// <summary>
        /// Unpack the Train data structure into a single list of interpolatedTrain objects.
        /// </summary>
        /// <param name="OrderdTrainRecords">The Train object containing a list of trains with their journey details.</param>
        /// <returns>A single list of interpolatedTrain objects.</returns>
        public static List<InterpolatedTrain> unpackInterpolatedData(List<Train> OrderdTrainRecords)
        {
            /* Place holder to store all train records in one list. */
            List<InterpolatedTrain> unpackedData = new List<InterpolatedTrain>();

            /* Cycle through each train. */
            foreach (Train train in OrderdTrainRecords)
            {
                /* Cycle through each record in the train journey. */
                for (int journeyIdx = 0; journeyIdx < train.TrainJourney.Count(); journeyIdx++)
                {
                    /* Add it to the list. */
                    unpackedData.Add(new InterpolatedTrain(train.TrainJourney[journeyIdx]));
                }
            }
            return unpackedData;
        }
    

    }
}
