using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsettings;

namespace TrainPerformance
{

    /// <summary>
    /// Enumerated direction of the train km's.
    /// </summary>
    public enum direction { increasing, decreasing, invalid, notSpecified };
    public enum trainOperator { PacificNational, Aurizon, Simulated, unknown};

    /// <summary>
    /// A class to hold the train details of each record.
    /// </summary>
    public class TrainDetails
    {

        public string TrainID;
        public string LocoID;
        public trainOperator Operator;
        public double powerToWeight;
        public DateTime NotificationDateTime;
        public GeoLocation location = new GeoLocation();
        public double speed;
        public double kmPost;
        //public double trackKmPost;
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
            this.Operator = trainOperator.unknown;
            this.powerToWeight = 0;
            this.NotificationDateTime = new DateTime(2000, 1, 1, 0, 0, 0);
            this.location.latitude = -33.8519;   //Sydney Harbour Bridge
            this.location.longitude = 151.2108;
            this.speed = 0;
            this.kmPost = 0;
            //this.trackKmPost = 0;
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
        public TrainDetails(string TrainID, string locoID, trainOperator Operator, double powerToWeight, DateTime NotificationDateTime, double latitude, double longitude,
                            double speed, double kmPost, double geometryKm, direction trainDirection, bool loop, bool TSR, double TSRspeed)
        {
            this.TrainID = TrainID;
            this.LocoID = locoID;
            this.Operator = Operator;
            this.powerToWeight = powerToWeight;
            this.NotificationDateTime = NotificationDateTime;
            this.location.latitude = latitude;
            this.location.longitude = longitude;
            this.speed = speed;
            this.kmPost = kmPost;
            //this.trackKmPost = kmPost;
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
        //public string Operator;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Train()
        {
            this.TrainJourney = null;
            this.include = false;
            //this.Operator = "Not Specified";
        }

        /// <summary>
        /// Train Object constructor.
        /// </summary>
        /// <param name="trainDetails">The list of trainDetails objects containing the details of the train journey.</param>
        public Train(List<TrainDetails> trainDetails)
        {
            this.TrainJourney = trainDetails;
            this.include = true;
            //this.Operator = "Not Specified";
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
            //this.Operator = "Not Specified";
        }

        /// <summary>
        /// Train Object constructor converting an list of interpolatedTrain objects into a list of trainDetail objects.
        /// </summary>
        /// <param name="trainDetails">A list of interpolatedTrain objects.</param>
        /// <param name="trainDirection">The direction of kilometreage of the train.</param>
        public Train(List<InterpolatedTrain> trainDetails, direction trainDirection)
        {
            List<TrainDetails> journey = new List<TrainDetails>();

            for (int journeyIdx = 0; journeyIdx < trainDetails.Count(); journeyIdx++)
            {
                /* Convert each interpolatedTrain object to a trainDetail object. */
                TrainDetails newitem = new TrainDetails(trainDetails[journeyIdx].TrainID, trainDetails[journeyIdx].LocoID, trainDetails[journeyIdx].Operator ,trainDetails[journeyIdx].powerToWeight, trainDetails[journeyIdx].NotificationDateTime, 0, 0,
                                                        trainDetails[journeyIdx].speed, 0, trainDetails[journeyIdx].geometryKm, trainDirection, trainDetails[journeyIdx].isLoopeHere,
                                                        trainDetails[journeyIdx].isTSRHere, trainDetails[journeyIdx].TSRspeed);
                journey.Add(newitem);

            }
            this.TrainJourney = journey;
            this.include = true;
            //this.Operator = Operator;
        }

        /// <summary>
        /// Determine the index of the geomerty data for the supplied kilometreage.
        /// </summary>
        /// <param name="TrainJourney">List of train details objects containt the journey details of the train.</param>
        /// <param name="targetKm">The target location to find in the geomerty data.</param>
        /// <returns>The index of the target kilometreage in the geomerty data, -1 if the target is not found.</returns>
        public int indexOfgeometryKm(List<TrainDetails> TrainJourney, double targetKm)
        {
            /* Loop through the train journey. */
            for (int journeyIdx = 0; journeyIdx < TrainJourney.Count(); journeyIdx++)
            {
                /* Match the current location with the geometry information. */
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
        public trainOperator Operator;
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
            this.Operator = trainOperator.unknown;
            this.powerToWeight = 1;
            this.NotificationDateTime = DateTime.MinValue;
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
        public InterpolatedTrain(string TrainID, string locoID, trainOperator Operator, double powerToWeight, DateTime NotificationDateTime, double geometryKm, double speed, bool loop, bool TSR, double TSRspeed)
        {
            this.TrainID = TrainID;
            this.LocoID = locoID;
            this.Operator = Operator;
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
            this.Operator = details.Operator;
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
        /// <param name="singleLineKm">Cummulative kilometreage of the simulated train.</param>
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
        public double elevation;
        public double underpoweredIncreaseingAverage;
        public double underpoweredDecreaseingAverage;
        public double overpoweredIncreaseingAverage;
        public double overpoweredDecreaseingAverage;
        public double totalIncreasingAverage;
        public double totalDecreasingAverage;
        public bool isInLoopBoundary;
        public bool isTSRboundary;

        /// <summary>
        /// Default averageTrainData Constructor.
        /// </summary>
        public averagedTrainData()
        {
            this.kilometerage = 0;
            this.elevation = 0;
            this.underpoweredIncreaseingAverage = 0;
            this.underpoweredDecreaseingAverage = 0;
            this.overpoweredIncreaseingAverage = 0;
            this.overpoweredDecreaseingAverage = 0;
            this.totalIncreasingAverage = 0;
            this.totalDecreasingAverage = 0;
            this.isInLoopBoundary = false;
            this.isTSRboundary = false;
        }

        /// <summary>
        /// averageTrainData Constructor.
        /// </summary>
        /// <param name="km">Kilometerage of each location</param>
        /// <param name="elevation">The elevationof the track at each location</param>
        /// <param name="underIncreasing">Average Speed at kilometreage for the underpowered category in the increasing direction.</param>
        /// <param name="underDecreasing">Average Speed at kilometreage for the underpowered category in the decreasing direction.</param>
        /// <param name="overIncreasing">Average Speed at kilometreage for the overpowered category in the increasing direction.</param>
        /// <param name="overDecreasing">Average Speed at kilometreage for the overpowered category in the decreasing direction.</param>
        /// <param name="loop">Flag indicating if the location is within the boudanry of a loop.</param>
        public averagedTrainData(double km, double elevation, double underIncreasing, double underDecreasing, double overIncreasing, double overDecreasing, 
            double totalIncreasing, double totalDecreasing, bool loop, bool TSR)
        {
            this.kilometerage = km;
            this.elevation = elevation;
            this.underpoweredIncreaseingAverage = underIncreasing;
            this.underpoweredDecreaseingAverage = underDecreasing;
            this.overpoweredIncreaseingAverage = overIncreasing;
            this.overpoweredDecreaseingAverage = overDecreasing;
            this.totalIncreasingAverage = totalIncreasing;
            this.totalDecreasingAverage = totalDecreasing;
            this.isInLoopBoundary = loop;
            this.isTSRboundary = TSR;
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
        public string Region;
        public DateTime IssueDate;
        public DateTime LiftedDate;
        public double startKm;
        public double endKm;
        public double TSRSpeed;

        /// <summary>
        /// Default TSRObject constructor.
        /// </summary>
        public TSRObject()
        {
            this.Region = "Unknown";
            this.IssueDate = DateTime.MinValue;
            this.LiftedDate = DateTime.MinValue;
            this.startKm = 0;
            this.endKm = 0;
            this.TSRSpeed = 0;
        }

        /// <summary>
        /// TSRObject constructor
        /// </summary>
        /// <param name="region">Region the TSR is in.</param>
        /// <param name="issued">The date the TSR was applied.</param>
        /// <param name="lifted">The Date the TSR was lifted, if applicable.</param>
        /// <param name="start">The start km of the TSR.</param>
        /// <param name="finish">The end Km of the TSR.</param>
        /// <param name="speed">The speed restriction applied to the TSR.</param>
        public TSRObject(string region, DateTime issued, DateTime lifted, double start, double finish, double speed)
        {
            this.Region = region;
            this.IssueDate = issued;
            this.LiftedDate = lifted;
            this.startKm = start;
            this.endKm = finish;
            this.TSRSpeed = speed;
        }

    }



    class Algorithm
    {
        /* Create a tools object. */
        public static Tools tool = new Tools();
        /* Create a processing object. */
        public static Processing processing = new Processing();
        /* Create a trackGeometry object. */
        public static TrackGeometry track = new TrackGeometry();
        /* Create a statistics object. */
        public static Statistics stats = new Statistics();

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

            /* Populate the exluded train list. */
            if (Settings.includeAListOfTrainsToExclude)
                excludeTrainList = FileOperations.readTrainList(FileSettings.trainList);

            /* Read in the track geometry data. */
            List<TrackGeometry> trackGeometry = new List<TrackGeometry>();
            trackGeometry = FileOperations.readGeometryfile(FileSettings.geometryFile);

            /* Read in the TSR information */
            List<TSRObject> TSRs = new List<TSRObject>();
            TSRs = FileOperations.readTSRFile(FileSettings.temporarySpeedRestrictionFile);

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

            /* Read the data. */
            List<TrainDetails> TrainRecords = new List<TrainDetails>();
            foreach (string batchFile in FileSettings.batchFiles)
                TrainRecords.AddRange(FileOperations.readICEData(batchFile, excludeTrainList));

            if (TrainRecords.Count() == 0)
            {
                tool.messageBox("There are no records in the list to analyse.", "No trains available.");
                return new List<Train>();
            }

            //int a = TrainRecords.Where(t => t.powerToWeight == 0).Count();
            if (TrainRecords.Where(t => t.powerToWeight == 0).Count() == TrainRecords.Count())
                Settings.resetPowerToWeightBoundariesToZero();

            /* Sort the data by [trainID, locoID, Date & Time, kmPost]. */
            List<TrainDetails> OrderdTrainRecords = new List<TrainDetails>();
            OrderdTrainRecords = TrainRecords.OrderBy(t => t.TrainID).ThenBy(t => t.LocoID).ThenBy(t => t.NotificationDateTime).ThenBy(t => t.kmPost).ToList();




            /**************************************************************************************************/
            /* Clean data - remove trains with insufficient data. */
            /******** Should only be required while we are waiting for the data in the prefered format ********/

            //List<Train> testTrainRecords = new List<Train>();
            //testTrainRecords = MakeTrains(trackGeometry, OrderdTrainRecords, TSRs);

            List<Train> CleanTrainRecords = new List<Train>();
            CleanTrainRecords = CleanData(trackGeometry, OrderdTrainRecords, TSRs);
                        
            /* interpolate data */
            /******** Should only be required while we are waiting for the data in the prefered format ********/
            List<Train> interpolatedRecords = new List<Train>();
            interpolatedRecords = processing.interpolateTrainData(CleanTrainRecords, trackGeometry);
            //interpolatedRecords = processing.interpolateTrainData(testTrainRecords, trackGeometry);

            /**************************************************************************************************/




            /* Populate the trains TSR values after interpolation to gain more granularity with TSR boundary. */
            processing.populateAllTrainsTemporarySpeedRestrictions(interpolatedRecords, TSRs);

            //List<InterpolatedTrain> unpackedInterpolation = new List<InterpolatedTrain>();
            //unpackedInterpolation = unpackInterpolatedData(interpolatedRecords);
            //FileOperations.writeTrainData(unpackedInterpolation);
            FileOperations.writeTrainData(interpolatedRecords);

           
            /* Genearate the statistics lists. */
            List<Statistics> stats = new List<Statistics>();
            List<Train> increasing = interpolatedRecords.Where(t => t.TrainJourney[0].trainDirection == direction.increasing).ToList();
            List<Train> decreasing = interpolatedRecords.Where(t => t.TrainJourney[0].trainDirection == direction.decreasing).ToList();
                
            /* Average the train data for each direction with regard for TSR's and loop locations. */
            List<averagedTrainData> averageData = new List<averagedTrainData>();

            if (Settings.HunterValleyRegion)
            {
                averageData = processing.operatorAverageSpeed(interpolatedRecords, trackGeometry, simulationUnderpoweredIncreasing, simulationUnderpoweredDecreasing,
                    simulationOverpoweredIncreasing, simulationOverpoweredDecreasing);

                /* Generate some statistical information for the aggregated data. */
                List<Train> PacificNationalIncreasing = interpolatedRecords.Where(t => t.TrainJourney[0].Operator == trainOperator.PacificNational).Where(t => t.TrainJourney[0].trainDirection == direction.increasing).ToList();
                List<Train> PacificNationalDecreasing = interpolatedRecords.Where(t => t.TrainJourney[0].Operator == trainOperator.PacificNational).Where(t => t.TrainJourney[0].trainDirection == direction.decreasing).ToList();
                List<Train> AurizonIncreasing = interpolatedRecords.Where(t => t.TrainJourney[0].Operator == trainOperator.Aurizon).Where(t => t.TrainJourney[0].trainDirection == direction.increasing).ToList();
                List<Train> AurizonDecreasing = interpolatedRecords.Where(t => t.TrainJourney[0].Operator == trainOperator.Aurizon).Where(t => t.TrainJourney[0].trainDirection == direction.decreasing).ToList();
                
                /* Calcualte the statistics on each group. */
                stats.Add(Statistics.generateStats(PacificNationalIncreasing, "Pacific National Increasing"));
                stats.Add(Statistics.generateStats(PacificNationalDecreasing, "Pacific National Decreasing"));
                stats.Add(Statistics.generateStats(AurizonIncreasing, "Aurizon Increasing"));
                stats.Add(Statistics.generateStats(AurizonDecreasing, "Aurizon Decreasing"));
                

            }
            else
            {
                averageData = processing.powerToWeightAverageSpeed(interpolatedRecords, trackGeometry, simulationUnderpoweredIncreasing, simulationUnderpoweredDecreasing,
                    simulationOverpoweredIncreasing, simulationOverpoweredDecreasing);

                /* Generate some statistical information for the aggregated data. */
                List<Train> underpoweredTrains = interpolatedRecords.Where(t => t.TrainJourney[0].powerToWeight > Settings.underpoweredLowerBound &&
                    t.TrainJourney[0].powerToWeight > Settings.underpoweredLowerBound).ToList();
                List<Train> overpoweredTrains = interpolatedRecords.Where(t => t.TrainJourney[0].powerToWeight > Settings.overpoweredLowerBound &&
                    t.TrainJourney[0].powerToWeight > Settings.overpoweredLowerBound).ToList();

                /* Calcualte the statistics on each group. */
                stats.Add(Statistics.generateStats(underpoweredTrains, "Underpowered Trains"));
                stats.Add(Statistics.generateStats(overpoweredTrains, "Overpowered Trains"));
                stats.Add(Statistics.generateStats(interpolatedRecords, "Combined"));

            }

            stats.Add(Statistics.generateStats(increasing, "Combined Increasing"));
            stats.Add(Statistics.generateStats(decreasing, "Combined Decreasing"));
                
            /* Seperate averages for P/W ratio groups, commodity, Operator */
            /* AverageByPower2Weight    -> powerToWeightAverageSpeed
             * AverageByCommodity       -> not written
             * AverageByOperator        -> not written
             * 
             * Maybe use a generic function - pass in only the list of trains that conform to the desired boundaries.
             */

            /* Write the averaged Data to file for inspection. */
            FileOperations.writeAverageData(averageData, stats);

            ///* Unpack the records into a single trainDetails object list. */
            //List<TrainDetails> unpackedData = new List<TrainDetails>();
            //unpackedData = unpackCleanData(CleanTrainRecords);

            ///* Write data to an excel file. */
            //FileOperations.writeTrainData(unpackedData);

            return interpolatedRecords;
        }

        /// <summary>
        /// This function cleans the data from large gaps in the data and ensures the trains 
        /// are all travelling in a single direction with a minimum total distance.
        /// </summary>
        /// <param name="trackGeometry">A list of track Geometry objects</param>
        /// <param name="trainRecords">List of TrainDetail objects</param>
        /// <returns>List of Train objects containign the journey details of each train.</returns>
        public static List<Train> CleanData(List<TrackGeometry> trackGeometry, List<TrainDetails> trainRecords, List<TSRObject> TSRs)
        {
            /* Note: this function will not be needed when Enterprise Services delivers the interpolated 
             * date directly to the database. We can access this data directly, then analyse.
             */

            bool removeTrain = false;
            double distance = 0;
            double journeyDistance = 0;
            trainOperator trainOperator = trainOperator.unknown;

            GeoLocation point1 = null;
            GeoLocation point2 = null;

            /* Place holder for the train records that are acceptable. */
            List<TrainDetails> newTrainList = new List<TrainDetails>();
            /* List of each Train with its journey details that is acceptable. */
            List<Train> cleanTrainList = new List<Train>();

            /* Add the first record to the list. */
            newTrainList.Add(trainRecords[0]);
            GeoLocation trainPoint = new GeoLocation(trainRecords[0]);
            /* Populate the first actual kilometreage point. */
            newTrainList[0].geometryKm = track.findClosestTrackGeometryPoint(trackGeometry, trainPoint);

            for (int trainIndex = 1; trainIndex < trainRecords.Count(); trainIndex++)
            {
                /* Compare next train details with current train details to establish if its a new train. */
                if (trainRecords[trainIndex].TrainID.Equals(trainRecords[trainIndex - 1].TrainID) &&
                    trainRecords[trainIndex].LocoID.Equals(trainRecords[trainIndex - 1].LocoID) &&
                    (trainRecords[trainIndex].NotificationDateTime - trainRecords[trainIndex - 1].NotificationDateTime).TotalMinutes < Settings.timeThreshold)
                {
                    /* If the current and previous record represent the same train journey, add it to the list. */
                    newTrainList.Add(trainRecords[trainIndex]);

                    point1 = new GeoLocation(trainRecords[trainIndex - 1]);
                    point2 = new GeoLocation(trainRecords[trainIndex]);

                    distance = processing.calculateGreatCircleDistance(point1, point2);

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
                    /* Check uni directionality of the train */
                    newTrainList = processing.longestDistanceTravelledInOneDirection(newTrainList, trackGeometry);
                    /* Calculate the total length of the journey */
                    journeyDistance = processing.calculateTrainJourneyDistance(newTrainList);

                    /* Validate the direction of train */
                    Train item = new Train();
                    bool HunterValley = true;
                    
                    if (HunterValley)
                        trainOperator = whoIsOperator(newTrainList[0].LocoID);

                    item.TrainJourney = newTrainList.ToList();
                    processing.populateOperator(item, trainOperator);
                    processing.populateDirection(item, trackGeometry);

                    /* remove the train if the direction is not valid. */
                    if (item.TrainJourney[0].trainDirection == direction.invalid)
                        removeTrain = true;

                    /* The end of the train journey has been reached. */
                    if (!removeTrain && journeyDistance > Settings.minimumJourneyDistance)
                    {
                        /* If all points are acceptable and the train travels the minimum distance, 
                         * add the train journey to the cleaned list. 
                         */

                        /* Determine the actual km, and populate the loops and TSR information. */
                        processing.populateGeometryKm(item, trackGeometry);
                        processing.populateLoopLocations(item, trackGeometry);

                        /* Sort the journey in ascending order. */
                        item.TrainJourney = item.TrainJourney.OrderBy(t => t.geometryKm).ToList();

                        cleanTrainList.Add(item);

                    }

                    /* Reset the parameters for the next train. */
                    removeTrain = false;
                    journeyDistance = 0;
                    newTrainList.Clear();

                    /* Add the first record of the new train journey. */
                    newTrainList.Add(trainRecords[trainIndex]);
                    trainPoint = new GeoLocation(trainRecords[trainIndex]);

                }

                /* The end of the records have been reached. */
                if (trainIndex == trainRecords.Count() - 1 && !removeTrain)
                {
                    /* Check uni directionality of the last train */
                    newTrainList = processing.longestDistanceTravelledInOneDirection(newTrainList, trackGeometry);
                    /* Calculate the total length of the journey */
                    journeyDistance = processing.calculateTrainJourneyDistance(newTrainList);

                    /* Validate the direction of train */
                    Train lastItem = new Train();
                    bool HunterValley = true;
                    if (HunterValley)
                        trainOperator = whoIsOperator(newTrainList[0].LocoID);

                    lastItem.TrainJourney = newTrainList.ToList();
                    processing.populateOperator(lastItem, trainOperator);
                    processing.populateDirection(lastItem, trackGeometry);

                    /* remove the train if the direction is not valid. */
                    if (lastItem.TrainJourney[0].trainDirection == direction.invalid)
                        removeTrain = true;

                    if (!removeTrain && journeyDistance > Settings.minimumJourneyDistance)
                    {
                        /* If all points are aceptable, add the train journey to the cleaned list. */
                        processing.populateGeometryKm(lastItem, trackGeometry);
                        processing.populateLoopLocations(lastItem, trackGeometry);

                        /* Sort the journey in ascending order. */
                        lastItem.TrainJourney = lastItem.TrainJourney.OrderBy(t => t.geometryKm).ToList();

                        cleanTrainList.Add(lastItem);
                    }

                }

            }

            return cleanTrainList;

        }

        /// <summary>
        /// Construct a list of Trains with individual train journey and details
        /// </summary>
        /// <param name="trackGeometry">A list of track Geometry objects</param>
        /// <param name="trainRecords">List of TrainDetail objects</param>
        /// <returns>List of Train objects containign the journey details of each train.</returns>
        public static List<Train> MakeTrains(List<TrackGeometry> trackGeometry, List<TrainDetails> trainRecords, List<TSRObject> TSRs)
        {

            /* Place holder for the train records that are acceptable. */
            List<TrainDetails> newTrainList = new List<TrainDetails>();
            /* List of each Train with its journey details that is acceptable. */
            List<Train> cleanTrainList = new List<Train>();

            trainOperator trainOperator = trainOperator.unknown;

            /* Add the first record to the list. */
            newTrainList.Add(trainRecords[0]);
            GeoLocation trainPoint = new GeoLocation(trainRecords[0]);
            /* Populate the first actual kilometreage point. */
            newTrainList[0].geometryKm = track.findClosestTrackGeometryPoint(trackGeometry, trainPoint);

            for (int trainIndex = 1; trainIndex < trainRecords.Count(); trainIndex++)
            {
                /* Compare next train details with current train details to establish if its a new train. */
                if (trainRecords[trainIndex].TrainID.Equals(trainRecords[trainIndex - 1].TrainID) &&
                    trainRecords[trainIndex].LocoID.Equals(trainRecords[trainIndex - 1].LocoID) &&
                    (trainRecords[trainIndex].NotificationDateTime - trainRecords[trainIndex - 1].NotificationDateTime).TotalMinutes < Settings.timeThreshold)
                {
                    /* If the current and previous record represent the same train journey, add it to the list. */
                    newTrainList.Add(trainRecords[trainIndex]);

                }
                else
                {
                    /* Check uni directionality of the train */
                    newTrainList = processing.longestDistanceTravelledInOneDirection(newTrainList, trackGeometry);

                    /* Validate the direction of train */
                    Train item = new Train();
                    bool HunterValley = true;
                    if (HunterValley)
                        trainOperator = whoIsOperator(newTrainList[0].LocoID);

                    item.TrainJourney = newTrainList.ToList();
                    processing.populateOperator(item, trainOperator);
                    processing.populateDirection(item, trackGeometry);

                    /* Determine the actual km, and populate the loops and TSR information. */
                    processing.populateGeometryKm(item, trackGeometry);
                    processing.populateLoopLocations(item, trackGeometry);

                    /* Sort the journey in ascending order. */
                    item.TrainJourney = item.TrainJourney.OrderBy(t => t.geometryKm).ToList();

                    cleanTrainList.Add(item);



                    /* Reset the parameters for the next train. */
                    newTrainList.Clear();

                    /* Add the first record of the new train journey. */
                    newTrainList.Add(trainRecords[trainIndex]);

                }

                /* The end of the records have been reached. */
                if (trainIndex == trainRecords.Count() - 1)
                {
                    /* Check uni directionality of the last train */
                    newTrainList = processing.longestDistanceTravelledInOneDirection(newTrainList, trackGeometry);

                    /* Validate the direction of train */
                    Train lastItem = new Train();
                    bool HunterValley = true;
                    if (HunterValley)
                        trainOperator = whoIsOperator(newTrainList[0].LocoID);

                    lastItem.TrainJourney = newTrainList.ToList();
                    processing.populateOperator(lastItem, trainOperator);
                    processing.populateDirection(lastItem, trackGeometry);

                    /* If all points are aceptable, add the train journey to the cleaned list. */
                    processing.populateGeometryKm(lastItem, trackGeometry);
                    processing.populateLoopLocations(lastItem, trackGeometry);

                    /* Sort the journey in ascending order. */
                    lastItem.TrainJourney = lastItem.TrainJourney.OrderBy(t => t.geometryKm).ToList();

                    cleanTrainList.Add(lastItem);


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

        /// <summary>
        /// This function determines the train operator based on the loco ID, This is generally only 
        /// applicable for the Hunter Valley region.
        /// </summary>
        /// <param name="locoID">The Loco ID of the train</param>
        /// <returns>The name of the train operator.</returns>
        private static trainOperator whoIsOperator(string locoID)
        {
            double aurizon = 0;
            double.TryParse(locoID, out aurizon);

            if (locoID.Substring(0, 2).Equals("TT", StringComparison.OrdinalIgnoreCase))
                return trainOperator.PacificNational;  //"Pacific National";
            else if (aurizon >= 5000)
                return trainOperator.Aurizon; // "Aurizon";
            else
                return trainOperator.unknown; // "Unknown";
        }

        



    } // Class Algorithm

}

