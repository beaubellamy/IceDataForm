using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsettings;

namespace TrainPerformance
{
    public class Processing
    {
        Tools tool = new Tools();
        //TrackGeometry track = new TrackGeometry();

        /* Mean radius of the Earth */
        private const double EarthRadius = 6371000.0;   // metres
        /* Constant time factors. */
        private const double secPerHour = 3600;
        private const double secPerDay = 86400;
        private const double hoursPerDay = 24;
        private const double minutesPerHour = 60;
        private const double secPerMinute = 60;


        /// <summary>
        /// Convert degrees in to radians
        /// </summary>
        /// <param name="degrees">Angle in degrees.</param>
        /// <returns>Angle in radians.</returns>
        private double degress2radians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        /// <summary>
        /// Calculates the distance between two points using the haversine formula.
        /// </summary>
        /// <param name="latitude1">Latitude of location 1.</param>
        /// <param name="longitude1">Longitude of location 1.</param>
        /// <param name="latitude2">Latitude of location 2.</param>
        /// <param name="longitude2">Longitude of location 2.</param>
        /// <returns>The Distance between the two points in metres.</returns>
        public double calculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {

            double arcsine = Math.Sin(degress2radians((latitude2 - latitude1) / 2)) * Math.Sin(degress2radians((latitude2 - latitude1) / 2)) +
                Math.Cos(degress2radians(latitude1)) * Math.Cos(degress2radians(latitude2)) *
                Math.Sin(degress2radians((longitude2 - longitude1) / 2)) * Math.Sin(degress2radians((longitude2 - longitude1) / 2));
            double arclength = 2 * Math.Atan2(Math.Sqrt(arcsine), Math.Sqrt(1 - arcsine));

            return EarthRadius * arclength;

        }

        /// <summary>
        /// Calculates the great circle distance between two points on a sphere 
        /// given there latitudes and longitudes.
        /// </summary>
        /// <param name="point1">Geographic location containinig the latitude and longitude of the reference location.</param>
        /// <param name="point2">Geographic location containinig the latitude and longitude of the destination location.</param>
        /// <returns>The Distance between the two points in metres.</returns>
        public double calculateDistance(GeoLocation point1, GeoLocation point2)
        {

            double arcsine = Math.Sin(degress2radians((point2.latitude - point1.latitude) / 2)) * Math.Sin(degress2radians((point2.latitude - point1.latitude) / 2)) +
                Math.Cos(degress2radians(point1.latitude)) * Math.Cos(degress2radians(point2.latitude)) *
                Math.Sin(degress2radians((point2.longitude - point1.longitude) / 2)) * Math.Sin(degress2radians((point2.longitude - point1.longitude) / 2));
            double arclength = 2 * Math.Atan2(Math.Sqrt(arcsine), Math.Sqrt(1 - arcsine));

            return EarthRadius * arclength;

        }

        /// <summary>
        /// Function determines the direction of the train using the first and last km posts.
        /// </summary>
        /// <param name="train">A train object containing kmPost information</param>
        /// <returns>Enumerated direction of the train km's.</returns>
        private direction determineTrainDirection(Train train)
        {
            string T = train.TrainJourney[0].TrainID;
            string L = train.TrainJourney[0].LocoID;
            DateTime D = train.TrainJourney[0].NotificationDateTime;

            /* Determine the distance and sign from the first point to the last point */
            //double journeyDistance = train.TrainJourney[train.TrainJourney.Count - 1].trackKmPost - train.TrainJourney[0].trackKmPost; 
            double journeyDistance = train.TrainJourney[train.TrainJourney.Count - 1].kmPost - train.TrainJourney[0].kmPost;
            double distance = 0;
            int increasingcount = 0;
            int decreasingCount = 0;
            int zeroCount = 0;

            if (journeyDistance > 0)
                return direction.increasing;
            else
                return direction.decreasing;


            //for (int journeyIdx = 1; journeyIdx < train.TrainJourney.Count(); journeyIdx++)
            //{
            //    distance = train.TrainJourney[journeyIdx].kmPost - train.TrainJourney[journeyIdx - 1].kmPost;
            //    if (distance > 0)
            //        increasingcount++;
            //    else if (distance < 0)
            //        decreasingCount++;
            //    else
            //        zeroCount++;
            //}

            //if (increasingcount > 0 && decreasingCount < 0.4*increasingcount)
            //    return direction.increasing;
            //else if (decreasingCount > 0 && increasingcount < 0.4*decreasingCount)
            //    return direction.decreasing;
            //else
            //    return direction.notSpecified;

        }

        /// <summary>
        /// Populate the Setting parameters from the form profived.
        /// </summary>
        /// <param name="form">The Form object containg the form parameters.</param>
        public void populateFormParameters(TrainPerformanceAnalysis form)
        {

            /* Extract the form parameters. */
            Settings.dateRange = form.getDateRange();
            Settings.topLeftLocation = form.getTopLeftLocation();
            Settings.bottomRightlocation = form.getBottomRightLocation();
            Settings.includeAListOfTrainsToExclude = form.getTrainListExcludeFlag();
            Settings.startKm = form.getStartKm();
            Settings.endKm = form.getEndKm();
            Settings.interval = form.getInterval();
            Settings.minimumJourneyDistance = form.getJourneydistance();
            Settings.loopSpeedThreshold = form.getLoopFactor();
            Settings.loopBoundaryThreshold = form.getLoopBoundary();
            Settings.TSRwindowBounday = form.getTSRWindow();
            Settings.timeThreshold = form.getTimeSeparation();
            Settings.distanceThreshold = form.getDistanceThreshold();
            Settings.underpoweredLowerBound = form.getUnderpoweredLowerBound();
            Settings.underpoweredUpperBound = form.getUnderpoweredUpperBound();
            Settings.overpoweredLowerBound = form.getOvderpoweredLowerBound();
            Settings.overpoweredUpperBound = form.getOvderpoweredUpperBound();


        }
        
        /// <summary>
        /// Validate the form parameters are not null
        /// </summary>
        /// <returns></returns>
        public bool areFormParametersValid()
        {
            if (Settings.dateRange == null ||
            Settings.topLeftLocation == null ||
            Settings.bottomRightlocation == null ||
            //Settings.includeAListOfTrainsToExclude == null ||
            Settings.startKm == null || 
            Settings.endKm == null ||
            Settings.interval == null ||
            Settings.minimumJourneyDistance == null ||
            Settings.loopSpeedThreshold == null ||
            Settings.loopBoundaryThreshold == null ||
            Settings.TSRwindowBounday == null ||
            Settings.timeThreshold == null ||
            Settings.distanceThreshold == null ||
            Settings.underpoweredLowerBound == null ||
            Settings.underpoweredUpperBound == null ||
            Settings.overpoweredLowerBound == null ||
            Settings.overpoweredUpperBound == null)
                return false;

            return true;


        }

        /// <summary>
        /// Function populates the direction parameter for the train.
        /// </summary>
        /// <param name="train">The train object</param>
        public void populateDirection(Train train, List<TrackGeometry> trackGeometry)
        {
            /* Match the track km post with the track geoemtry */
            //track.matchTrainLocationToTrackGeometry(train, trackGeometry);
            TrainPerformanceAnalysis.track.matchTrainLocationToTrackGeometry(train, trackGeometry);

            /* Determine the direction of the train */
            direction direction = determineTrainDirection(train);

            /* Populate the direction parameter. */
            foreach (TrainDetails trainPoint in train.TrainJourney)
            {
                trainPoint.trainDirection = direction;
            }

        }

        /// <summary>
        /// Populate the geometry km information based on the calculated distance from the first km post.
        /// </summary>
        /// <param name="train">A train object.</param>
        public void populateGeometryKm(Train train, List<TrackGeometry> trackGeometry)
        {
            /* Determine the direction of the km's the train is travelling. */
            direction direction = determineTrainDirection(train);
            double point2PointDistance = 0;
            double x = 0, y = 0;
            

            /* Thie first km point is populated by the parent function ICEData.CleanData(). */
            for (int journeyIdx = 1; journeyIdx < train.TrainJourney.Count(); journeyIdx++)
            {
                

                /* Calculate the distance between successive points. */
                GeoLocation point1 = new GeoLocation(train.TrainJourney[journeyIdx - 1]);
                GeoLocation point2 = new GeoLocation(train.TrainJourney[journeyIdx]);
                point2PointDistance = calculateDistance(point1, point2);

                //x = TrainPerformance.TrackGeometry.findClosestTrackGeometryPoint(trackGeometry, point1);
                //y = track.findClosestTrackGeometryPoint(trackGeometry, point2);

                /* Determine the cumulative actual geometry km based on the direction. */
                if (direction.Equals(direction.increasing))
                    train.TrainJourney[journeyIdx].geometryKm = train.TrainJourney[journeyIdx - 1].geometryKm + point2PointDistance / 1000;
                    // if y < km-offest, then the direction has changed.

                else if (direction.Equals(direction.decreasing))
                    train.TrainJourney[journeyIdx].geometryKm = train.TrainJourney[journeyIdx - 1].geometryKm - point2PointDistance / 1000;
                    // if y > km+offest, then the direction has changed.

                else
                    train.TrainJourney[journeyIdx].geometryKm = train.TrainJourney[journeyIdx].kmPost;
            }

        }

        /// <summary>
        /// Populate the loop location information for each point in the train journey.
        /// </summary>
        /// <param name="train">A train object containing the journey details.</param>
        /// <param name="trackGeometry">The track geometry object indicating the location of the loops.</param>
        public void populateLoopLocations(Train train, List<TrackGeometry> trackGeometry)
        {
            /* Create a track geometry object. */
            TrackGeometry track = new TrackGeometry();
            int index = 0;
            double trainPoint = 0;

            /* Cycle through the train journey. */
            foreach (TrainDetails journey in train.TrainJourney)
            {
                trainPoint = journey.geometryKm;
                /* Find the index of the closest point on the track to the train. */
                index = track.findClosestTrackGeometryPoint(trackGeometry, trainPoint);
                /* Populate the loop */
                journey.isLoopHere = trackGeometry[index].isLoopHere;

            }

        }

        /// <summary>
        /// populate the temporary speed rrestrcition information for each train journey.
        /// </summary>
        /// <param name="train">A train object containing teh journey details.</param>
        /// <param name="trackGeometry">teh track Geometry object indicating the TSR information at each location.</param>
        public void populateTemporarySpeedRestrictions(Train train, List<TrackGeometry> trackGeometry)
        {
            /* Create a track geometry object. */
            TrackGeometry track = new TrackGeometry();
            int index = 0;
            double trainPoint = 0;

            /* Cycle through the train journey. */
            foreach (TrainDetails journey in train.TrainJourney)
            {
                trainPoint = journey.geometryKm;
                /* Find the index of the closest point on the track to the train. */
                index = track.findClosestTrackGeometryPoint(trackGeometry, trainPoint);
                /* Populate the loop */
                journey.isTSRHere = trackGeometry[index].isTSRHere;
                journey.TSRspeed = trackGeometry[index].temporarySpeedRestriction;
            }
        }

        /// <summary>
        /// Linear interpolation to a target point.
        /// </summary>
        /// <param name="targetX">Target invariant location to be interpolated to.</param>
        /// <param name="X0">Lower invariant position to interpolate between.</param>
        /// <param name="X1">Upper invariant position to interpolate between.</param>
        /// <param name="Y0">Lower variant to interpolate between.</param>
        /// <param name="Y1">Upper variant to interpolate between.</param>
        /// <returns>The interpolate variant value at the target invariant location.</returns>
        private double linear(double targetX, double X0, double X1, double Y0, double Y1)
        {
            /* Take the average when the invariant location does not change. */
            if ((X1 - X0) == 0)
                return (Y0 + Y1) / 2;

            return Y0 + (targetX - X0) * (Y1 - Y0) / (X1 - X0);

        }

        /// <summary>
        ///  Interpolate the train speed to a specified interval using a linear interpolation.
        /// </summary>
        /// <param name="trains">List of train objects containing the parameters for each train journey.</param>
        /// <param name="trackGeometry">The list of Track geometry data to align the train location.</param>
        /// <returns>List of train objects with interpolated values at the specified interval.</returns>
        public List<Train> interpolateTrainData(List<Train> trains, List<TrackGeometry> trackGeometry)
        {
            /* Placeholders for the interpolated distance markers. */
            double previousKm = 0;
            double currentKm = 0;
            /* Place holder to calaculte the time for each interpolated value. */
            DateTime time = new DateTime();
            /* Flag to indicate when to collect the next time value. */
            bool timeChange = true;

            /* Additional loop and TSR details. */
            int geometryIdx = 0;
            bool loop = false;
            bool TSR = false;
            double TSRspeed = 0;

            /* Index values for the interpolation parameters */
            int index0 = -1;
            int index1 = -1;

            /* Interplation parameters. */
            double interpolatedSpeed = 0;
            double X0, X1, Y0, Y1;

            int a = 0;


            /* Create a new list of trains for the journies interpolated values. */
            List<Train> newTrainList = new List<Train>();
            /* Create a journey list to store the existing journey details. */
            List<TrainDetails> journey = new List<TrainDetails>();

            /* Cycle through each train to interpolate between points. */
            for (int trainIdx = 0; trainIdx < trains.Count(); trainIdx++)
            {
                
                /* Create a new journey list of interpolated values. */
                List<InterpolatedTrain> interpolatedTrainList = new List<InterpolatedTrain>();

                journey = trains[trainIdx].TrainJourney;

                string d = journey[0].trainDirection.ToString();

                /* Set the start of the interpolation. */
                currentKm = Settings.startKm;

                while (currentKm < Settings.endKm)
                {
                    
                    /* Find the closest kilometerage markers either side of the current interpolation point. */
                    index0 = findClosestLowerKm(currentKm, journey);
                    index1 = findClosestGreaterKm(currentKm, journey);

                    /* If a valid index is found, extract the existing journey parameters and interpolate. */
                    if (index0 >= 0 && index1 >= 0)
                    {
                        X0 = journey[index0].geometryKm;
                        X1 = journey[index1].geometryKm;
                        Y0 = journey[index0].speed;
                        Y1 = journey[index1].speed;
                        if (timeChange)
                        {
                            time = journey[index0].NotificationDateTime;
                            timeChange = false;
                        }

                        /* Perform linear interpolation. */
                        interpolatedSpeed = linear(currentKm, X0, X1, Y0, Y1);
                        /* Interpolate the time. */
                        time = time.AddHours(calculateTimeInterval(previousKm, currentKm, interpolatedSpeed));


                        if (X0 < 6)
                            a = 0;

                    }
                    else
                    {
                        /* Boundary conditions for interpolating the data prior to and beyond the existing journey points. */
                        time = new DateTime(2000, 1, 1);    // journey[0].NotificationDateTime
                        interpolatedSpeed = 0;

                    }

                    
<<<<<<< HEAD

=======
>>>>>>> dfe05a991504f14c2991e612a189adfb63ab1792
                    geometryIdx = trackGeometry[0].findClosestTrackGeometryPoint(trackGeometry, currentKm);

                    if (geometryIdx >= 0)
                    {
                        /* Check if there is a loop at this location. */
                        loop = trackGeometry[geometryIdx].isLoopHere;

                        /* Check if there is a TSR at this location. */
                        TSR = trackGeometry[geometryIdx].isTSRHere;
                        TSRspeed = trackGeometry[geometryIdx].temporarySpeedRestriction;
                    }

                    /* Create the interpolated data object and add it to the list. */
                    InterpolatedTrain item = new InterpolatedTrain(trains[trainIdx].TrainJourney[0].TrainID, trains[trainIdx].TrainJourney[0].LocoID,
                                                                    trains[trainIdx].TrainJourney[0].powerToWeight, time, currentKm, interpolatedSpeed, loop, TSR, TSRspeed);
                    interpolatedTrainList.Add(item);

                    /* Create a copy of the current km marker and increment. */
                    previousKm = currentKm;
                    currentKm = currentKm + Settings.interval / 1000;

                    /* Determine if we need to extract the time from the data or interpolate it. */
                    if (index1 >= 0)
                        if (currentKm >= journey[index1].geometryKm)
                            timeChange = true;

                }

                /* Add the interpolated list to the list of new train objects. */
                Train trainItem = new Train(interpolatedTrainList, journey[0].trainDirection);
                newTrainList.Add(trainItem);
                string T = trainItem.TrainJourney[0].TrainID;
                d = trainItem.TrainJourney[0].LocoID;
                
            }

            /* Return the completed interpolated train data. */
            return newTrainList;
        }

        /// <summary>
        /// Interpolate the simultion data to a specified interval using a linear interpolation.
        /// </summary>
        /// <param name="simulatedTrain">The list of the simulated train details.</param>
        /// <param name="trackGeometry">The list of Track geometry data to align the train location.</param>
        /// <param name="startKm">Starting kilometreage for the interpolation.</param>
        /// <param name="endKm">End kilometerage for the interpolation.</param>
        /// <param name="interval">interpolation interval, specified in metres.</param>
        /// <returns>List of interpolated values for the simulation.</returns>
        public List<InterpolatedTrain> interpolateSimulationData(List<simulatedTrain> simulatedTrain, List<TrackGeometry> trackGeometry)
        {
            /* Placeholders for the interpolated distance markers. */
            double previousKm = 0;
            double currentKm = 0;
            /* Place holder to calaculte the time for each interpolated value. */
            DateTime simTime = new DateTime();
            double time = 0;
            double days, hours, minutes, seconds;
            /* Flag to indicate when to collect the next time value. */
            bool timeChange = true;

            /* Additional loop and TSR details. */
            int geometryIdx = 0;
            bool loop = false;
            bool TSR = false;
            double TSRspeed = 0;

            double powerToWeight = 0;   

            /* Index values for the interpolation parameters */
            int index0 = -1;
            int index1 = -1;

            /* Interplation parameters. */
            double interpolatedSpeed = 0;
            double X0, X1, Y0, Y1;

            List<InterpolatedTrain> simulatedInterpolation = new List<InterpolatedTrain>();


            /* Set the start of the interpolation. */
            currentKm = Settings.startKm;

            while (currentKm < Settings.endKm)
            {
                /* Find the closest kilometerage markers either side of the current interpoaltion point. */
                index0 = findClosestLowerKm(currentKm, simulatedTrain);
                index1 = findClosestGreaterKm(currentKm, simulatedTrain);

                /* If a valid index is found, extract the existing journey parameters and interpolate. */
                if (index0 >= 0 && index1 >= 0)
                {
                    X0 = simulatedTrain[index0].singleLineKm;
                    X1 = simulatedTrain[index1].singleLineKm;
                    Y0 = simulatedTrain[index0].velocity;
                    Y1 = simulatedTrain[index1].velocity;
                    if (timeChange)
                    {
                        time = simulatedTrain[index0].time;
                        days = (time / secPerDay);
                        hours = (days - Math.Truncate(days)) * hoursPerDay;
                        minutes = (hours - Math.Truncate(hours)) * minutesPerHour;
                        seconds = (minutes - Math.Truncate(minutes)) * secPerMinute;

                        simTime = new DateTime(2000, 1, (int)days + 1, (int)hours, (int)minutes, (int)seconds);
                        timeChange = false;
                    }

                    /* Perform linear interpolation. */
                    interpolatedSpeed = linear(currentKm, X0, X1, Y0, Y1);
                    /* Interpolate the time. */
                    simTime = simTime.AddHours(calculateTimeInterval(previousKm, currentKm, interpolatedSpeed));

                }
                else
                {
                    /* Boundary conditions for interpolating the data prior to and beyond the existing journey points. */
                    time = 0;
                    interpolatedSpeed = 0;

                }

                /* Determine if we need to extract the time from the data or interpolate it. */
                if (index1 >= 0)
                    if (currentKm >= simulatedTrain[index1].singleLineKm)
                        timeChange = true;

                geometryIdx = trackGeometry[0].findClosestTrackGeometryPoint(trackGeometry, currentKm);

                if (geometryIdx >= 0)
                {
                    /* Check if there is a loop at this location. */
                    loop = trackGeometry[geometryIdx].isLoopHere;

                    /* Check if there is a TSR at this location. */
                    TSR = trackGeometry[geometryIdx].isTSRHere;
                    TSRspeed = trackGeometry[geometryIdx].temporarySpeedRestriction;
                }

                /* Create the interpolated data object and add it to the list. */
                InterpolatedTrain item = new InterpolatedTrain("Simulated Train", "Simulated Loco", powerToWeight, simTime, currentKm, interpolatedSpeed, loop, TSR, TSRspeed);
                simulatedInterpolation.Add(item);

                /* Create a copy of the current km marker and increment. */
                previousKm = currentKm;
                currentKm = currentKm + Settings.interval / 1000;

            }

            
            

            return simulatedInterpolation;
        }

        /// <summary>
        /// Calculate the time interval between two locations based on the speed.
        /// </summary>
        /// <param name="startPositon">Starting kilometreage.</param>
        /// <param name="endPosition">Final kilometreage.</param>
        /// <param name="speed">Average speed between locations.</param>
        /// <returns>The time taken to traverse the distance in hours.</returns>
        private double calculateTimeInterval(double startPositon, double endPosition, double speed)
        {
<<<<<<< HEAD
            if (speed == 0)
                return 0;
            return Math.Abs(endPosition - startPositon) / speed;    // hours.
=======
            if (speed > 0)
                return Math.Abs(endPosition - startPositon) / speed;    // hours.
            else
                return 0;
>>>>>>> dfe05a991504f14c2991e612a189adfb63ab1792
        }

        /// <summary>
        /// Find the index of the closest kilometerage that is less than the target point.
        /// </summary>
        /// <param name="target">The target kilometerage.</param>
        /// <param name="journey">The list of train details containig the journey parameters.</param>
        /// <returns>The index of the closest point that is less than the target point. 
        /// Returns -1 if a point does not exist.</returns>
        private int findClosestLowerKm(double target, List<TrainDetails> journey)
        {
            /* set the initial values. */
            double minimum = double.MaxValue;
            double difference = double.MaxValue;
            int index = 0;

            /* Cycle through the journey parameters. */
            for (int journeyIdx = 0; journeyIdx < journey.Count(); journeyIdx++)
            {
                /* Find the difference if the value is lower. */
                if (journey[journeyIdx].geometryKm < target)
                    difference = Math.Abs(journey[journeyIdx].geometryKm - target);

                /* Find the minimum difference. */
                if (difference < minimum)
                {
                    minimum = difference;
                    index = journeyIdx;
                }

            }

            if (difference == double.MaxValue)
                return -1;

            return index;
        }

        /// <summary>
        /// Find the index of the closest kilometerage that is less than the target point.
        /// </summary>
        /// <param name="target">The target kilometerage.</param>
        /// <param name="journey">The list of train details containig the journey parameters.</param>
        /// <returns>The index of the closest point that is less than the target point. 
        /// Returns -1 if a point does not exist.</returns>
        private int findClosestLowerKm(double target, List<simulatedTrain> journey)
        {
            /* set the initial values. */
            double minimum = double.MaxValue;
            double difference = double.MaxValue;
            int index = 0;

            /* Cycle through the journey parameters. */
            for (int journeyIdx = 0; journeyIdx < journey.Count(); journeyIdx++)
            {
                /* Find the difference if the value is lower. */
                if (journey[journeyIdx].singleLineKm < target)
                    difference = Math.Abs(journey[journeyIdx].singleLineKm - target);

                /* Find the minimum difference. */
                if (difference < minimum)
                {
                    minimum = difference;
                    index = journeyIdx;
                }

            }

            if (difference == double.MaxValue)
                return -1;

            return index;
        }

        /// <summary>
        /// Find the index of the closest kilometerage that is larger than the target point.
        /// </summary>
        /// <param name="target">The target kilometerage.</param>
        /// <param name="journey">The list of train details containig the journey parameters.</param>
        /// <returns>The index of the closest point that is larger than the target point. 
        /// Returns -1 if a point does not exist.</returns>
        private int findClosestGreaterKm(double target, List<TrainDetails> journey)
        {
            /* set the initial values. */
            double minimum = double.MaxValue;
            double difference = double.MaxValue;
            int index = 0;

            /* Cycle through the journey parameters. */
            for (int journeyIdx = 0; journeyIdx < journey.Count(); journeyIdx++)
            {
                /* Find the difference if the value is lower. */
                if (journey[journeyIdx].geometryKm > target)
                    difference = Math.Abs(journey[journeyIdx].geometryKm - target);

                /* Find the minimum difference. */
                if (difference < minimum)
                {
                    minimum = difference;
                    index = journeyIdx;
                }
            }

            if (difference == double.MaxValue)
                return -1;

            return index;
        }

        /// <summary>
        /// Find the index of the closest kilometerage that is larger than the target point.
        /// </summary>
        /// <param name="target">The target kilometerage.</param>
        /// <param name="journey">The list of simulated train data containig the journey parameters.</param>
        /// <returns>The index of the closest point that is larger than the target point. 
        /// Returns -1 if a point does not exist.</returns>        
        private int findClosestGreaterKm(double target, List<simulatedTrain> journey)
        {
            /* set the initial values. */
            double minimum = double.MaxValue;
            double difference = double.MaxValue;
            int index = 0;

            /* Cycle through the journey parameters. */
            for (int journeyIdx = 0; journeyIdx < journey.Count(); journeyIdx++)
            {
                /* Find the difference if the value is lower. */
                if (journey[journeyIdx].singleLineKm > target)
                    difference = Math.Abs(journey[journeyIdx].singleLineKm - target);

                /* Find the minimum difference. */
                if (difference < minimum)
                {
                    minimum = difference;
                    index = journeyIdx;
                }
            }

            if (difference == double.MaxValue)
                return -1;

            return index;
        }

        /// <summary>
        /// Determine the average speed of all trains in a specified direction given a power to weight range.
        /// </summary>
        /// <param name="trains">All train data containig each train journey details.</param>
        /// <param name="simulation">The journey of the simulated train.</param>
        /// <param name="lowerBound">The lower bound of the acceptable power to weight ratio.</param>
        /// <param name="upperBound">The upper bound of the acceptable power to weight ratio.</param>
        /// <param name="direction">The direction of the km of the train journey.</param>
        /// <returns>A list for the average speed of trains within the power to weight range.</returns>
        public List<double> powerToWeightAverageSpeed(List<Train> trains, List<InterpolatedTrain> simulation, double lowerBound, double upperBound, direction direction)
        {

            int size = (int)((Settings.endKm - Settings.startKm) / (Settings.interval / 1000));
            double sum = 0;

            /* Place holders for the included speeds and the resulting average speed at each location. */
            List<double> speed = new List<double>();
            List<double> averageSpeed = new List<double>();

            TrainDetails journey = new TrainDetails();

            /* Loop through each interpoalted location. */
            for (int journeyIdx = 0; journeyIdx <= size; journeyIdx++)
            {
                sum = 0;
                speed.Clear();

                /* Loop through each train. */
                foreach (Train train in trains)
                {
                    journey = train.TrainJourney[journeyIdx];

                    if (journey.trainDirection == direction)
                    {

                        /* Is there a TSR that applies */
                        if (!temporarySpeedRestrictionParameters(train, journey.geometryKm).isTSRHere)
                        {
                            /* Check train is not within the loop boundaries */
                            if (!isTrainInLoopBoundary(train, journey.geometryKm))
                            {
                                if (journey.powerToWeight > lowerBound && journey.powerToWeight <= upperBound)
                                {
                                    speed.Add(journey.speed);
                                    sum = sum + journey.speed;
                                }
                            }
                            else
                            {
                                /* Train is within the loop boundaries */
                                if (journey.speed > (simulation[journeyIdx].speed * Settings.loopSpeedThreshold))
                                {
                                    speed.Add(journey.speed);
                                    sum = sum + journey.speed;
                                }
                            }
                        }
                        else
                        {
                            /* A TSR applies to the current position of the train. */
                        }
                    }
                }

                if (speed.Count == 0)
                    averageSpeed.Add(simulation[journeyIdx].speed); // This might have to be the simulated speed
                else
                {

                    if (sum == 0)
                    {
                        averageSpeed.Add(0);
                    }
                    else
                    {
                        averageSpeed.Add(speed.Where(x => x > 0.0).Average(x => x));
                    }
                }

            }

            return averageSpeed;
        }

        /// <summary>
        /// Determine the average speed of all trains in a each direction and each power to weight category.
        /// </summary>
        /// <param name="trains">List of trains, each with a trainJourney object.</param>
        /// <param name="underpoweredIncreasing">The simulated train journey for the underpowered category in the increasing direction.</param>
        /// <param name="underpoweredDecreasing">The simulated train journey for the underpowered category in the decreasing direction.</param>
        /// <param name="overpoweredIncreasing">The simulated train journey for the overpowered category in the increasing direction.</param>
        /// <param name="overpoweredDecreasing">The simulated train journey for the overpowered category in the decreasing direction.</param>
        /// <returns>A list of averaged train objects containing the average speed at each location for all four power to weight catagories.</returns>
        public List<averagedTrainData> powerToWeightAverageSpeed(List<Train> trains, 
                                                                List<InterpolatedTrain> underpoweredIncreasing, List<InterpolatedTrain> underpoweredDecreasing, 
                                                                List<InterpolatedTrain> overpoweredIncreasing, List<InterpolatedTrain> overpoweredDecreasing)
        {
            /* Declare the local variables to store the sum and averages. */
            double underpoweredIncreasingSum, underpoweredDecreasingSum, overpoweredIncreasingSum, overpoweredDecreasingSum;
            double underIncreasingAverage, underDecreasingAverage, overIncreasingAverage, overDecreasingAverage;

            bool isInLoopBoundary = false;
            
            /* Calcualte the number of elements in the arary/list. */
            int size = (int)((Settings.endKm - Settings.startKm) / (Settings.interval / 1000));
            
            /* Place holders for the included speeds and the resulting average speed at each location. */
            List<double> underIncreasingSpeed = new List<double>();
            List<double> underDecreasingSpeed = new List<double>();
            List<double> overIncreasingSpeed = new List<double>();
            List<double> overDecreasingSpeed = new List<double>();
            List<averagedTrainData> averageSpeed = new List<averagedTrainData>();

            TrainDetails journey = new TrainDetails();

            /* Loop through each interpolated location. */
            for (int journeyIdx = 0; journeyIdx <= size; journeyIdx++)
            {
                /* Initialise the sums and the speed lists. */
                underpoweredIncreasingSum = 0;
                underpoweredDecreasingSum = 0;
                overpoweredIncreasingSum = 0;
                overpoweredDecreasingSum = 0;

                underIncreasingSpeed.Clear();
                underDecreasingSpeed.Clear();
                overIncreasingSpeed.Clear();
                overDecreasingSpeed.Clear();

                
                /* Loop through each train. */
                foreach (Train train in trains)
                {
                    journey = train.TrainJourney[journeyIdx];

                    /* Seperate the averages for each direction. */
                    if (journey.trainDirection == direction.increasing)
                    {                        
                        /* Is there a TSR that applies */
                        if (!temporarySpeedRestrictionParameters(train, journey.geometryKm).isTSRHere)
                        {
                            /* Check train is not within the loop boundaries */
                            if (!isTrainInLoopBoundary(train, journey.geometryKm))
                            {
                                isInLoopBoundary = false;

                                if (journey.powerToWeight > Settings.underpoweredLowerBound && journey.powerToWeight <= Settings.underpoweredUpperBound)
                                {
                                    /* Underpowered increasing trains. */
                                    underIncreasingSpeed.Add(journey.speed);
                                    underpoweredIncreasingSum = underpoweredIncreasingSum + journey.speed;
                                }
                                if (journey.powerToWeight > Settings.overpoweredLowerBound && journey.powerToWeight <= Settings.overpoweredUpperBound)
                                {
                                    /* Overpowered incerasing trains. */
                                    overIncreasingSpeed.Add(journey.speed);
                                    overpoweredIncreasingSum = overpoweredIncreasingSum + journey.speed;
                                }

                            }
                            else
                            {
                                isInLoopBoundary = true;
   
                                /* Train is within the loop boundaries */
                                if (journey.powerToWeight > Settings.underpoweredLowerBound && journey.powerToWeight <= Settings.underpoweredUpperBound)
                                {
                                    /* Underpowered increasing trains. */
                                    if (journey.speed > (underpoweredIncreasing[journeyIdx].speed * Settings.loopSpeedThreshold))
                                    {
                                        underIncreasingSpeed.Add(journey.speed);
                                        underpoweredIncreasingSum = underpoweredIncreasingSum + journey.speed;
                                    }
                                }

                                if (journey.powerToWeight > Settings.overpoweredLowerBound && journey.powerToWeight <= Settings.overpoweredUpperBound)
                                {
                                    /* Overpowered incerasing trains. */
                                    if (journey.speed > (overpoweredIncreasing[journeyIdx].speed * Settings.loopSpeedThreshold))
                                    {
                                        overIncreasingSpeed.Add(journey.speed);
                                        overpoweredIncreasingSum = overpoweredIncreasingSum + journey.speed;
                                    }
                                }


                            }
                        }
                        else
                        {
                            /* A TSR applies to the current position of the train. */
                        }
                    }
                    else if (journey.trainDirection == direction.decreasing)
                    {
                        /* Is there a TSR that applies */
                        if (!temporarySpeedRestrictionParameters(train, journey.geometryKm).isTSRHere)
                        {
                            /* Check train is not within the loop boundaries */
                            if (!isTrainInLoopBoundary(train, journey.geometryKm))
                            {
                                isInLoopBoundary = false;
   
                                if (journey.powerToWeight > Settings.underpoweredLowerBound && journey.powerToWeight <= Settings.underpoweredUpperBound)
                                {
                                    /* Underpowered decreasing trains. */
                                    underDecreasingSpeed.Add(journey.speed);
                                    underpoweredDecreasingSum = underpoweredDecreasingSum + journey.speed;
                                }
                                if (journey.powerToWeight > Settings.overpoweredLowerBound && journey.powerToWeight <= Settings.overpoweredUpperBound)
                                {
                                    /* Overpowered decerasing trains. */
                                    overDecreasingSpeed.Add(journey.speed);
                                    overpoweredDecreasingSum = overpoweredDecreasingSum + journey.speed;
                                }

                            }
                            else
                            {
                                isInLoopBoundary = true;
                                /* Train is within the loop boundaries */
                                if (journey.powerToWeight > Settings.underpoweredLowerBound && journey.powerToWeight <= Settings.underpoweredUpperBound)
                                {
                                    /* Underpowered decreasing trains. */
                                    if (journey.speed > (underpoweredIncreasing[journeyIdx].speed * Settings.loopSpeedThreshold))
                                    {
                                        underIncreasingSpeed.Add(journey.speed);
                                        underpoweredDecreasingSum = underpoweredDecreasingSum + journey.speed;
                                    }
                                }

                                if (journey.powerToWeight > Settings.overpoweredLowerBound && journey.powerToWeight <= Settings.overpoweredUpperBound)
                                {
                                    /* overpowered decreasing trains. */
                                    if (journey.speed > (overpoweredIncreasing[journeyIdx].speed * Settings.loopSpeedThreshold))
                                    {
                                        overIncreasingSpeed.Add(journey.speed);
                                        overpoweredDecreasingSum = overpoweredDecreasingSum + journey.speed;
                                    }
                                }


                            }
                        }
                    }
                    else
                    {
                        /* No direction specified. */
                    }

                }

                /* Calcualte the average speed for each category at each location. */
                if (underIncreasingSpeed.Count() == 0 || underpoweredIncreasingSum == 0)
                    underIncreasingAverage = underpoweredIncreasing[journeyIdx].speed;
                else
                    underIncreasingAverage = underIncreasingSpeed.Where(x => x > 0.0).Average(x => x);

                if (underDecreasingSpeed.Count() == 0 || underpoweredDecreasingSum == 0)
                    underDecreasingAverage = underpoweredDecreasing[journeyIdx].speed;
                else
                    underDecreasingAverage = underDecreasingSpeed.Where(x => x > 0.0).Average(x => x);

                if (overIncreasingSpeed.Count() == 0 || overpoweredIncreasingSum == 0)
                    overIncreasingAverage = overpoweredIncreasing[journeyIdx].speed;
                else
                    overIncreasingAverage = overIncreasingSpeed.Where(x => x > 0.0).Average(x => x);                
                
                if (overDecreasingSpeed.Count() == 0 || overpoweredDecreasingSum == 0)
                    overDecreasingAverage = overpoweredDecreasing[journeyIdx].speed;
                else
                    overDecreasingAverage = overDecreasingSpeed.Where(x => x > 0.0).Average(x => x);

                double kilometerage = Settings.startKm + Settings.interval/1000 * journeyIdx;

                /* Add the averages to the list. */
                averagedTrainData item = new averagedTrainData(kilometerage, underIncreasingAverage, underDecreasingAverage, overIncreasingAverage, overDecreasingAverage, isInLoopBoundary);
                averageSpeed.Add(item);
                
            }

            return averageSpeed;
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
        /// Determine if the train is approaching, leaving or within a loop.
        /// </summary>
        /// <param name="train">The train object containing the journey details.</param>
        /// <param name="targetLocation">The specific location being considered.</param>
        /// <returns>True, if the train is within the boundaries of teh loop window.</returns>
        public bool isTrainInLoopBoundary(Train train, double targetLocation)
        {

            /* Find the indecies of the boundaries of the loop. */
            double lookBack = targetLocation - Settings.loopBoundaryThreshold;
            double lookForward = targetLocation + Settings.loopBoundaryThreshold;
            int lookBackIdx = train.indexOfgeometryKm(train.TrainJourney, lookBack);
            int lookForwardIdx = train.indexOfgeometryKm(train.TrainJourney, lookForward);

            /* Check the indecies are valid */
            if (lookBack < Settings.startKm && lookBackIdx == -1)
                lookBackIdx = 0;
            if (lookForward > Settings.endKm && lookForwardIdx == -1)
            {
                if (train.TrainJourney[0].trainDirection == direction.increasing)
                    lookForwardIdx = train.TrainJourney.Count() - 1;
                else
                    lookForwardIdx = 0;
            }

           

            /* Determine if a loop is within the loop window of the current position. */
            if (lookBackIdx >= 0 && lookForwardIdx >= 0)
            {
                for (int journeyIdx = lookBackIdx; journeyIdx < lookForwardIdx; journeyIdx++)
                {
                    TrainDetails journey = train.TrainJourney[journeyIdx];

                    if (journey.isLoopHere)
                        return true;

                }
            }
            return false;
        }

        /// <summary>
        /// Determine the properties of the TSR if one applies.
        /// </summary>
        /// <param name="train">The train object containing the journey details.</param>
        /// <param name="targetLocation">The specific location being considered.</param>
        /// <returns>TSR object containting the TSR flag and the associated speed. </returns>
        public TSRObject temporarySpeedRestrictionParameters(Train train, double targetLocation)
        {
            TSRObject TSR = new TSRObject();

            /* Find the indecies of the boundaries of the loop. */
            double lookBack = targetLocation - Settings.TSRwindowBounday;
            double lookForward = targetLocation + Settings.TSRwindowBounday;
            int lookBackIdx = train.indexOfgeometryKm(train.TrainJourney, lookBack);
            int lookForwardIdx = train.indexOfgeometryKm(train.TrainJourney, lookForward);

            /* Check the indecies are valid */
            if (lookBack < Settings.startKm && lookBackIdx == -1)
                lookBackIdx = 0;
            if (lookForward > Settings.endKm && lookForwardIdx == -1)
                lookForwardIdx = train.TrainJourney.Count() - 1;

            /* Determine if a loop is within the loop window of the current position. */
            if (lookBackIdx >= 0 && lookForwardIdx >= 0)
            {
                for (int journeyIdx = lookBackIdx; journeyIdx < lookForwardIdx; journeyIdx++)
                {
                    TrainDetails journey = train.TrainJourney[journeyIdx];

                    if (journey.isTSRHere)
                    {
                        TSR.isTSRHere = true;
                        TSR.TSRSpeed = journey.TSRspeed;
                    }

                }
            }
            return TSR;
        }



    } // Class processing

}
