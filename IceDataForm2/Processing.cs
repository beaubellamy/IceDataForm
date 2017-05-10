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
        /// Calculates the great circle distance between two points on a sphere
        /// given there latitudes and longitudes.
        /// </summary>
        /// <param name="latitude1">Latitude of location 1.</param>
        /// <param name="longitude1">Longitude of location 1.</param>
        /// <param name="latitude2">Latitude of location 2.</param>
        /// <param name="longitude2">Longitude of location 2.</param>
        /// <returns>The Distance between the two points in metres.</returns>
        public double calculateGreatCircleDistance(double latitude1, double longitude1, double latitude2, double longitude2)
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
        public double calculateGreatCircleDistance(GeoLocation point1, GeoLocation point2)
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
            /* NOTE: This function does not take into account any train journey data that have 
             * multiple changes of direction. This should not be seen when the 'Cleaned Data' 
             * is deleviered by Enetrprise services.
             */
            
            /* Determine the distance and sign from the first point to the last point */
            double journeyDistance = train.TrainJourney[train.TrainJourney.Count - 1].kmPost - train.TrainJourney[0].kmPost;
            
            /* The invalid direction captures those train journeys that change in direction and return to a similar location. */
            //if (journeyDistance > -10 && journeyDistance < 10)
            //    return direction.invalid;
            //else 
            if (journeyDistance > 0)
                return direction.increasing;
            else
                return direction.decreasing;

            
        }

        /// <summary>
        /// Calcualte the single train journey length.
        /// </summary>
        /// <param name="journey">The journey points for the train.</param>
        /// <returns>The total distance travelled in metres.</returns>
        public double calculateTrainJourneyDistance(List<TrainDetails> journey)
        {
            double distance = 0;

            for (int pointIdx = 1; pointIdx < journey.Count; pointIdx++)
            {
                /* Create the conequtive points */
                GeoLocation point1 = journey[pointIdx - 1].location;
                GeoLocation point2 = journey[pointIdx].location;

                /* Calcualte the great circle distance. */
                distance = distance + calculateGreatCircleDistance(point1, point2);
            }

            return distance;

        }

        /// <summary>
        /// Determine if the single train journey has a single direction. When the journey has 
        /// multilpe directions, the part of the journey that has the largest total length in
        /// a single direction is returned.
        /// </summary>
        /// <param name="journey">The complete train journey.</param>
        /// <param name="trackGeometry">The geometry of the track.</param>
        /// <returns>A list of train details objects describing the longest distance the train has 
        /// travelled in a single direction.</returns>
        public List<TrainDetails> longestDistanceTravelledInOneDirection(List<TrainDetails> journey, List<TrackGeometry> trackGeometry)
        {
            /* Set up intial conditions */
            double movingAverage = 0;
            double previousAverage = 0;
            double distance = 0;
            double increasingDistance = 0;
            double decreasingDistance = 0;
            
            int start, end;
            int newStart, count;
            double maxValue = 0;

            /* Create lists to add each journey for each change in direction. */
            List<double> distances = new List<double>();
            List<int> startIdx = new List<int>();
            List<int> endIdx = new List<int>();

            /* Set the number of points to average over. */
            int numPoints = 10;

            if (journey.Count <= numPoints)
                return journey;

            /* Set the kmPosts to the closest points on the geometry alignment. */
            TrainPerformanceAnalysis.track.matchTrainLocationToTrackGeometry(journey, trackGeometry);
            
            start = 0;
            
            for (int journeyIdx = 0; journeyIdx < journey.Count() - numPoints; journeyIdx++)
            {
                /* Calculate the moving average of the kmposts ahead of current position. */
                distance = journey[journeyIdx + numPoints].kmPost - journey[journeyIdx].kmPost;
                movingAverage =  distance / numPoints;

                /* Check the direction has not changed. */
                if (Math.Sign(movingAverage) == Math.Sign(previousAverage) || Math.Sign(movingAverage) == 0 || Math.Sign(previousAverage) == 0)
                {
                    /* Increment the assumed distance travelled in current direction. */
                    if (movingAverage > 0)
                        increasingDistance = increasingDistance + movingAverage;
                    
                    else if (movingAverage < 0)
                        decreasingDistance = decreasingDistance - movingAverage;
                 
                }
                else 
                {
                    /* There has been a change in direction. */
                    end = journeyIdx;

                    /* Add the total distance achieved from the previous km posts to the list. */
                    if (previousAverage > 0)
                    {
                        distances.Add(increasingDistance);
                        startIdx.Add(start);
                        endIdx.Add(end);
                        increasingDistance = 0;
                    }
                    else if (previousAverage < 0)
                    {
                        distances.Add(decreasingDistance);
                        startIdx.Add(start);
                        endIdx.Add(end);
                        decreasingDistance = 0;
                    }

                    /* Reset the new start postion. */
                    start = journeyIdx++;
                }

                previousAverage = movingAverage;
                
            }

            /* Add the last total distance achieved to the list. */
            end = journey.Count()-1;
            if (previousAverage > 0)
            {
                distances.Add(increasingDistance);
                startIdx.Add(start);
                endIdx.Add(end);
            }
            else if (previousAverage < 0)
            {
                distances.Add(decreasingDistance);
                startIdx.Add(start);
                endIdx.Add(end);
            }
            else 
            {
                /* Condition when last average is 0, determine which total to add to the list. */
                if (increasingDistance > decreasingDistance)
                {
                    distances.Add(increasingDistance);
                    startIdx.Add(start);
                    endIdx.Add(end);
                }
                else 
                {
                    distances.Add(decreasingDistance);
                    startIdx.Add(start);
                    endIdx.Add(end);
                }
            }

            if (distances.Count() == 1)
                return journey;

            /* Determine the largest distance to return that section of the journey */
            maxValue = distances.Max();
            int index = distances.ToList().IndexOf(maxValue);
            newStart = startIdx[index];
            count = endIdx[index] - newStart+1;

            /* Return the part of the journey that has the largest total length in a single direction. */
            return journey.GetRange(newStart, count);           
                        
        }
        
        /// <summary>
        /// Populate the Setting parameters from the form provided.
        /// </summary>
        /// <param name="form">The Form object containg the form parameters.</param>
        public void populateFormParameters(TrainPerformanceAnalysis form)
        {

            /* Extract the form parameters. */
            Settings.dateRange = form.getDateRange();
            Settings.topLeftLocation = form.getTopLeftLocation();
            Settings.bottomRightLocation = form.getBottomRightLocation();
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
            Settings.combinedLowerBound = form.getUnderpoweredLowerBound();
            Settings.combinedUpperBound = form.getOvderpoweredUpperBound();


        }
        
        /// <summary>
        /// Validate the form parameters are within logical boundaries.
        /// </summary>
        /// <returns></returns>
        public bool areFormParametersValid()
        {

            if (Settings.dateRange == null ||
                Settings.dateRange[0] > DateTime.Today || Settings.dateRange[1] > DateTime.Today ||
                Settings.dateRange[0] > Settings.dateRange[1])
                return false;

            if (Settings.topLeftLocation == null ||
                Settings.topLeftLocation.latitude > -10 ||      /* Australian top left bounday */
                Settings.topLeftLocation.longitude < 110 ||
                Settings.topLeftLocation.latitude > -10 ||      /* Australian top right bounday */
                Settings.topLeftLocation.longitude > 155)
                return false;

            if (Settings.bottomRightLocation == null ||
                Settings.bottomRightLocation.latitude < -40 ||      /* Australian bottom left bounday */
                Settings.bottomRightLocation.longitude < 110 ||
                Settings.bottomRightLocation.latitude < -40 ||      /* Australian bottom right bounday */
                Settings.bottomRightLocation.longitude > 155)
                return false;

            if (Settings.startKm < 0)
                return false;

            if (Settings.endKm < 0)
                return false;

            if ( Settings.interval < 0)
                return false;

            if (Settings.minimumJourneyDistance < 0)
                return false;

            if (Settings.loopSpeedThreshold < 0 || Settings.loopSpeedThreshold > 100)
                return false;

            if (Settings.loopBoundaryThreshold < 0)
                return false;

            if (Settings.TSRwindowBounday < 0)
                return false;

            if (Settings.timeThreshold < 0)
                return false;

            if (Settings.distanceThreshold < 0)
                return false;

            if (Settings.underpoweredLowerBound < 0)
                return false;
            
            if (Settings.underpoweredUpperBound < 0)
                return false;
            
            if (Settings.overpoweredLowerBound < 0)
                return false;
            
            if (Settings.overpoweredUpperBound < 0)
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
            GeoLocation trainPoint = new GeoLocation();
            
            /* The first km point is populated by the parent function ICEData.CleanData(). */
            for (int journeyIdx = 0; journeyIdx < train.TrainJourney.Count(); journeyIdx++)
            {
                /* Find the kilometerage of the closest point on the track and associate it with the current train location.*/
                trainPoint = new GeoLocation(train.TrainJourney[journeyIdx]);
                train.TrainJourney[journeyIdx].geometryKm = TrainPerformanceAnalysis.track.findClosestTrackGeometryPoint(trackGeometry, trainPoint);
                /* Note: This is not ideal, as the actual distances travelled will end up being out by a few km.
                 * This approach reduces the effect of the train journey's appearing to change direction several times.
                 */


                /* The following approach can be used when the data being read does not contain changing directions. */

                /* Calculate the distance between successive points. */
                //GeoLocation point1 = new GeoLocation(train.TrainJourney[journeyIdx - 1]);
                //GeoLocation point2 = new GeoLocation(train.TrainJourney[journeyIdx]);
                //point2PointDistance = calculateDistance(point1, point2);

                ///* Determine the cumulative actual geometry km based on the direction. */
                //if (train.TrainJourney[0].trainDirection == direction.increasing)
                //    train.TrainJourney[journeyIdx].geometryKm = train.TrainJourney[journeyIdx - 1].geometryKm + point2PointDistance / 1000;
                
                //else if (train.TrainJourney[0].trainDirection == direction.decreasing)
                //    train.TrainJourney[journeyIdx].geometryKm = train.TrainJourney[journeyIdx - 1].geometryKm - point2PointDistance / 1000;
                
                //else
                //    train.TrainJourney[journeyIdx].geometryKm = train.TrainJourney[journeyIdx].kmPost;
                
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
        /// populate the temporary speed restriction information for each train journey.
        /// </summary>
        /// <param name="train">A train object containing the journey details.</param>
        /// <param name="trackGeometry">the track Geometry object indicating the TSR information at each location.</param>
        public void populateTemporarySpeedRestrictions(Train train, List<TrackGeometry> trackGeometry, List<TSRObject> TSRs)
        {
            /* Create a track geometry object. */
            TrackGeometry track = new TrackGeometry();
            //int index = 0;
            double trainPoint = 0;

            /* Cycle through the train journey. */
            foreach (TrainDetails journey in train.TrainJourney)
            {
                /* Extract the current point in the journey */
                trainPoint = journey.geometryKm;

                /* Cycle through each TSR. */
                foreach (TSRObject TSR in TSRs)
                {
                    if (trainPoint >= TSR.startKm && trainPoint <= TSR.endKm)
                    {
                        if (journey.NotificationDateTime >= TSR.IssueDate && journey.NotificationDateTime <= TSR.LiftedDate)
                        {
                            /* When the train is within the applicable TSR, add it to the journey. */
                            journey.isTSRHere = true;
                            journey.TSRspeed = TSR.TSRSpeed;
                        }
                    }
                    
                    /* When a TSR is applicable, break out of the current loop and continue with the rest of the journey. */
                    if (journey.isTSRHere)
                        break;
                }
            }
        }

        /// <summary>
        /// This function cycles through each train and determines if a TSR had applied to any part of the journey.
        /// </summary>
        /// <param name="trains">A list of trains containing the journey for each.</param>
        /// <param name="TSRs">A list of TSR objects.</param>
        public void populateAllTrainsTemporarySpeedRestrictions(List<Train> trains, List<TSRObject> TSRs)
        {
            
            foreach (Train train in trains)
            {
                int tsrIndex = 0;
                
                foreach (TrainDetails journey in train.TrainJourney)
                {
                    /* Establish the TSR that applies to the train position. */
                    if (journey.geometryKm > TSRs[tsrIndex].endKm && tsrIndex < TSRs.Count()-1)
                        tsrIndex++;

                    /* Determine if the TSR is applicable to the train by location and date. */
                    if (journey.geometryKm >= TSRs[tsrIndex].startKm && journey.geometryKm <= TSRs[tsrIndex].endKm &&
                        journey.NotificationDateTime >= TSRs[tsrIndex].IssueDate && journey.NotificationDateTime <= TSRs[tsrIndex].LiftedDate)
                    {
                        journey.isTSRHere = true;
                        journey.TSRspeed = TSRs[tsrIndex].TSRSpeed;
                    }
                }
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
            /* Place holder to calculate the time for each interpolated value. */
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
                
                /* Set the start of the interpolation. */
                currentKm = Settings.startKm;
                previousKm = currentKm;

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
                        
                    }
                    else
                    {
                        /* Boundary conditions for interpolating the data prior to and beyond the existing journey points. */
                        time = journey.Where(t => t.NotificationDateTime > DateTime.MinValue).Min(t => t.NotificationDateTime); // DateTime.MinValue;
                        interpolatedSpeed = 0;
                    }
                                       
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
            
            if (speed > 0)
                return Math.Abs(endPosition - startPositon) / speed;    // hours.
            else
                return 0;
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
            /* Set the initial values. */
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
            /* Set the initial values. */
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
            /* Set the initial values. */
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
            /* Set the initial values. */
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
                        if (!withinTemporarySpeedRestrictionBoundaries(train, journey.geometryKm))
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
        /// <param name="underpoweredIncreasingSimulation">The simulated train journey for the underpowered category in the increasing direction.</param>
        /// <param name="underpoweredDecreasingSimulation">The simulated train journey for the underpowered category in the decreasing direction.</param>
        /// <param name="overpoweredIncreasingSimulation">The simulated train journey for the overpowered category in the increasing direction.</param>
        /// <param name="overpoweredDecreasingSimulation">The simulated train journey for the overpowered category in the decreasing direction.</param>
        /// <returns>A list of averaged train objects containing the average speed at each location for all four power to weight catagories.</returns>
        public List<averagedTrainData> powerToWeightAverageSpeed(List<Train> trains, 
                                                                List<InterpolatedTrain> underpoweredIncreasingSimulation, List<InterpolatedTrain> underpoweredDecreasingSimulation, 
                                                                List<InterpolatedTrain> overpoweredIncreasingSimulation, List<InterpolatedTrain> overpoweredDecreasingSimulation)
        {
            /* Declare the local variables to store the sum and averages. */
            double underpoweredIncreasingSum, underpoweredDecreasingSum, overpoweredIncreasingSum, overpoweredDecreasingSum, totalIncreasingSum, totalDecreasingSum;
            double underIncreasingAverage, underDecreasingAverage, overIncreasingAverage, overDecreasingAverage, totalIncreasingAverage, totalDecreasingAverage;
            double underpoweredIncreasingWeight, underpoweredDecreasingWeight, overpoweredIncreasingWeight, overpoweredDecreasingWeight;

            bool isInLoopBoundary = false;
            bool isInTSRBoundary = false;
            List<bool> TSRList = new List<bool>();
            
            /* Calculate the number of elements in the array/list. */
            int size = (int)((Settings.endKm - Settings.startKm) / (Settings.interval / 1000));
            //int underpoweredIncreasingTrainCount, underpoweredDecerasingTrainCount, overpoweredIncreasingTrainCount, overpoweredDecerasingTrainCount;
            
            /* Place holders for the included speeds and the resulting average speed at each location. */
            List<double> underIncreasingSpeed = new List<double>();
            List<double> underDecreasingSpeed = new List<double>();
            List<double> overIncreasingSpeed = new List<double>();
            List<double> overDecreasingSpeed = new List<double>();
            List<double> totalIncreasingSpeed = new List<double>();
            List<double> totalDecreasingSpeed = new List<double>();

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
                totalIncreasingSum = 0;
                totalDecreasingSum = 0;

                underpoweredIncreasingWeight = 0;
                underpoweredDecreasingWeight = 0;
                overpoweredIncreasingWeight = 0;
                overpoweredDecreasingWeight = 0;

                underIncreasingSpeed.Clear();
                underDecreasingSpeed.Clear();
                overIncreasingSpeed.Clear();
                overDecreasingSpeed.Clear();
                totalDecreasingSpeed.Clear();
                totalIncreasingSpeed.Clear();

                TSRList.Clear();
                
                /* Loop through each train. */
                foreach (Train train in trains)
                {
                    
                    journey = train.TrainJourney[journeyIdx];
                    
                    /* Seperate the averages for each direction. */
                    if (journey.trainDirection == direction.increasing)
                    {                        

                        /* Is there a TSR that applies */                       
                        if (!withinTemporarySpeedRestrictionBoundaries(train, journey.geometryKm))
                        {
                            TSRList.Add(false);

                            /* Check train is not within the loop boundaries */
                            if (!isTrainInLoopBoundary(train, journey.geometryKm))
                            {
                                //isInLoopBoundary = false;

                                if (journey.powerToWeight > Settings.underpoweredLowerBound && journey.powerToWeight <= Settings.underpoweredUpperBound)
                                {
                                    /* Underpowered increasing trains. */
                                    underIncreasingSpeed.Add(journey.speed);
                                    underpoweredIncreasingSum = underpoweredIncreasingSum + journey.speed;

                                    totalIncreasingSpeed.Add(journey.speed);
                                    totalIncreasingSum = totalIncreasingSum + journey.speed;

                                    underpoweredIncreasingWeight++;
                                }

                                if (journey.powerToWeight > Settings.overpoweredLowerBound && journey.powerToWeight <= Settings.overpoweredUpperBound)
                                {
                                    /* Overpowered incerasing trains. */
                                    overIncreasingSpeed.Add(journey.speed);
                                    overpoweredIncreasingSum = overpoweredIncreasingSum + journey.speed;

                                    totalIncreasingSpeed.Add(journey.speed);
                                    totalIncreasingSum = totalIncreasingSum + journey.speed;

                                    overpoweredIncreasingWeight++;
                                }

                            }
                            else
                            {
                                isInLoopBoundary = true;
   
                                /* Train is within the loop boundaries */
                                if (journey.powerToWeight > Settings.underpoweredLowerBound && journey.powerToWeight <= Settings.underpoweredUpperBound)
                                {
                                    /* Underpowered increasing trains. */
                                    if (journey.speed > (underpoweredIncreasingSimulation[journeyIdx].speed * Settings.loopSpeedThreshold))
                                    {
                                        underIncreasingSpeed.Add(journey.speed);
                                        underpoweredIncreasingSum = underpoweredIncreasingSum + journey.speed;

                                        totalIncreasingSpeed.Add(journey.speed);
                                        totalIncreasingSum = totalIncreasingSum + journey.speed;

                                        underpoweredIncreasingWeight++;
                                    }
                                }

                                if (journey.powerToWeight > Settings.overpoweredLowerBound && journey.powerToWeight <= Settings.overpoweredUpperBound)
                                {
                                    /* Overpowered incerasing trains. */
                                    if (journey.speed > (overpoweredIncreasingSimulation[journeyIdx].speed * Settings.loopSpeedThreshold))
                                    {
                                        overIncreasingSpeed.Add(journey.speed);
                                        overpoweredIncreasingSum = overpoweredIncreasingSum + journey.speed;

                                        totalIncreasingSpeed.Add(journey.speed);
                                        totalIncreasingSum = totalIncreasingSum + journey.speed;

                                        overpoweredIncreasingWeight++;
                                    }
                                }

                            }
                        }
                        else
                        {
                            /* A TSR applies to the current position of the train. */
                            TSRList.Add(true);

                            /* We dont want to include the speed in the aggregation if the train is within the
                             * bundaries of a TSR and is forced to slow down.  
                             */
                            if (journey.powerToWeight > Settings.underpoweredLowerBound && journey.powerToWeight <= Settings.underpoweredUpperBound)
                                underpoweredIncreasingWeight++;
                            
                            if (journey.powerToWeight > Settings.overpoweredLowerBound && journey.powerToWeight <= Settings.overpoweredUpperBound)
                                overpoweredIncreasingWeight++;
                            


                        }
                    }
                    else if (journey.trainDirection == direction.decreasing)
                    {

                         /* Is there a TSR that applies */
                        if (!withinTemporarySpeedRestrictionBoundaries(train, journey.geometryKm))
                        {
                            TSRList.Add(false);

                            /* Check train is not within the loop boundaries */
                            if (!isTrainInLoopBoundary(train, journey.geometryKm))
                            {
                                isInLoopBoundary = false;
   
                                if (journey.powerToWeight > Settings.underpoweredLowerBound && journey.powerToWeight <= Settings.underpoweredUpperBound)
                                {
                                    /* Underpowered decreasing trains. */
                                    underDecreasingSpeed.Add(journey.speed);
                                    underpoweredDecreasingSum = underpoweredDecreasingSum + journey.speed;

                                    totalDecreasingSpeed.Add(journey.speed);
                                    totalDecreasingSum = totalDecreasingSum + journey.speed;

                                    underpoweredDecreasingWeight++;

                                }
                                if (journey.powerToWeight > Settings.overpoweredLowerBound && journey.powerToWeight <= Settings.overpoweredUpperBound)
                                {
                                    /* Overpowered decerasing trains. */
                                    overDecreasingSpeed.Add(journey.speed);
                                    overpoweredDecreasingSum = overpoweredDecreasingSum + journey.speed;

                                    totalDecreasingSpeed.Add(journey.speed);
                                    totalDecreasingSum = totalDecreasingSum + journey.speed;

                                    overpoweredDecreasingWeight++;

                                }

                            }
                            else
                            {
                                isInLoopBoundary = true;
                                /* Train is within the loop boundaries */
                                if (journey.powerToWeight > Settings.underpoweredLowerBound && journey.powerToWeight <= Settings.underpoweredUpperBound)
                                {
                                    /* Underpowered decreasing trains. */
                                    if (journey.speed > (underpoweredDecreasingSimulation[journeyIdx].speed * Settings.loopSpeedThreshold))
                                    {
                                        underDecreasingSpeed.Add(journey.speed);
                                        underpoweredDecreasingSum = underpoweredDecreasingSum + journey.speed;

                                        totalDecreasingSpeed.Add(journey.speed);
                                        totalDecreasingSum = totalDecreasingSum + journey.speed;

                                        underpoweredDecreasingWeight++;
                                    }
                                }

                                if (journey.powerToWeight > Settings.overpoweredLowerBound && journey.powerToWeight <= Settings.overpoweredUpperBound)
                                {
                                    /* overpowered decreasing trains. */
                                    if (journey.speed > (overpoweredDecreasingSimulation[journeyIdx].speed * Settings.loopSpeedThreshold))
                                    {
                                        overDecreasingSpeed.Add(journey.speed);
                                        overpoweredDecreasingSum = overpoweredDecreasingSum + journey.speed;

                                        totalDecreasingSpeed.Add(journey.speed);
                                        totalDecreasingSum = totalDecreasingSum + journey.speed;

                                        overpoweredDecreasingWeight++;
                                    }
                                }


                            }
                        }
                        else
                        {
                            /* A TSR applies to the current position of the train. */
                            TSRList.Add(true);

                            /* We dont want to include the speed in the aggregation if the train is within the
                             * bundaries of a TSR and is forced to slow down.  
                             */
                            if (journey.powerToWeight > Settings.underpoweredLowerBound && journey.powerToWeight <= Settings.underpoweredUpperBound)
                                underpoweredDecreasingWeight++;

                            if (journey.powerToWeight > Settings.overpoweredLowerBound && journey.powerToWeight <= Settings.overpoweredUpperBound)
                                overpoweredDecreasingWeight++;
                            
                        }
                    }
                    else
                    {
                        /* No direction specified. */
                    }

                }

                int TSRTrueCount = TSRList.Where(t => t == true).Count();

                if (TSRTrueCount > 0)
                    isInTSRBoundary = true;
                else
                    isInTSRBoundary = false;

                /* If the TSR applied the whole analysis period, the simualtion speed is used. */
                if (TSRTrueCount == TSRList.Count())
                {
                    underIncreasingAverage = underpoweredIncreasingSimulation[journeyIdx].speed;
                    underDecreasingAverage = underpoweredDecreasingSimulation[journeyIdx].speed;
                    overIncreasingAverage = overpoweredIncreasingSimulation[journeyIdx].speed;
                    overDecreasingAverage = overpoweredDecreasingSimulation[journeyIdx].speed;

                    /* Calcualte the weighted average of the simualtions for the total power catagory. */
                    double totalIncreasing = (underIncreasingAverage * underpoweredIncreasingWeight + overIncreasingAverage * overpoweredIncreasingWeight) / 
                        (underpoweredIncreasingWeight + overpoweredIncreasingWeight);
                    double totalDecreasing = (underDecreasingAverage * underpoweredDecreasingWeight + overDecreasingAverage * overpoweredDecreasingWeight) /
                        (underpoweredDecreasingWeight + overpoweredDecreasingWeight);

                    totalIncreasingAverage = totalIncreasing;
                    totalDecreasingAverage = totalDecreasing;
                }
                else
                {
                    /* Calcualte the average speed for each category at each location. */
                    if (underIncreasingSpeed.Count() == 0 || underpoweredIncreasingSum == 0)
                        underIncreasingAverage = 0.0; //underpoweredIncreasingSimulation[journeyIdx].speed;
                    else
                        underIncreasingAverage = underIncreasingSpeed.Where(x => x > 0.0).Average(x => x);

                    if (underDecreasingSpeed.Count() == 0 || underpoweredDecreasingSum == 0)
                        underDecreasingAverage = 0.0; //underpoweredDecreasingSimulation[journeyIdx].speed;
                    else
                        underDecreasingAverage = underDecreasingSpeed.Where(x => x > 0.0).Average(x => x);

                    if (overIncreasingSpeed.Count() == 0 || overpoweredIncreasingSum == 0)
                        overIncreasingAverage = 0.0; //overpoweredIncreasingSimulation[journeyIdx].speed;
                    else
                        overIncreasingAverage = overIncreasingSpeed.Where(x => x > 0.0).Average(x => x);

                    if (overDecreasingSpeed.Count() == 0 || overpoweredDecreasingSum == 0)
                        overDecreasingAverage = 0.0; //overpoweredDecreasingSimulation[journeyIdx].speed;
                    else
                        overDecreasingAverage = overDecreasingSpeed.Where(x => x > 0.0).Average(x => x);


                    if (totalIncreasingSpeed.Count() == 0 || totalIncreasingSum == 0)
                        totalIncreasingAverage = 0.0; //journey.speed;
                    else
                        totalIncreasingAverage = totalIncreasingSpeed.Where(x => x > 0.0).Average(x => x);

                    if (totalDecreasingSpeed.Count() == 0 || totalDecreasingSum == 0)
                        totalDecreasingAverage = 0.0; //journey.speed;
                    else
                        totalDecreasingAverage = totalDecreasingSpeed.Where(x => x > 0.0).Average(x => x);
                }

                double kilometerage = Settings.startKm + Settings.interval/1000 * journeyIdx;

                /* Add the averages to the list. */
                averagedTrainData item = new averagedTrainData(kilometerage, underIncreasingAverage, underDecreasingAverage,
                    overIncreasingAverage, overDecreasingAverage, totalIncreasingAverage, totalDecreasingAverage, isInLoopBoundary, isInTSRBoundary);
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
        /// <returns>True, if the train is within the boundaries of the loop window.</returns>
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
        public bool withinTemporarySpeedRestrictionBoundaries(Train train, double targetLocation)
        {
            
            bool isTSRHere = false;
            
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
                        isTSRHere = true;
                }
            }
            return isTSRHere;
        }




        

    } // Class processing

}
