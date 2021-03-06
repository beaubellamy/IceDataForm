﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainPerformance;

namespace Globalsettings
{
    public static class FileSettings
    {
        /* Filenames for each required file. */
        public static List<string> batchFiles = new List<string>();
        public static string geometryFile = null;
        public static string temporarySpeedRestrictionFile = null;
        public static string trainList = null;                  /* File only required if includeAListOfTrainsToExclude is TRUE. */
        public static string underpoweredIncreasingSimulationFile = null;
        public static string underpoweredDecreasingSimulationFile = null;
        public static string overpoweredIncreasingSimulationFile = null;
        public static string overpoweredDecreasingSimulationFile = null;
        public static string alternativeIncreasingSimulationFile = null;
        public static string alternativeDecreasingSimulationFile = null;

        public static string aggregatedDestination = null;

    }

    public static class Settings
    {

        /* Data boundaries */
        public static DateTime[] dateRange;                 /* Date range of data to include. */
        public static GeoLocation topLeftLocation;          /* Top left corner of the geographic box describing the included data. */
        public static GeoLocation bottomRightLocation;      /* Bottom right corner of the geographic box describing the included data. */
        public static bool includeAListOfTrainsToExclude;   /* Is a list of trains that are to be excluded available. */

        /* Corridor dependant / Analysis parameters */
        public static double startKm;                       /* Start km for interpoaltion data. */
        public static double endKm;                         /* End km for interpolation data. */
        public static double interval;                      /* Interpolation interval (metres). */
        public static double minimumJourneyDistance;        /* Minimum distance of a train journey to be considered valid. */

        /* Processing parameters */
        public static double loopSpeedThreshold;            /* Cuttoff for the simulation speed, when comparing the train to the simualted train. */
        public static double loopBoundaryThreshold;         /* Distance either side of the loop to be considered within the loop boundary (km). */
        public static double TSRwindowBounday;              /* Distance either side of the TSR location to be considered within the TSR boundary (km). */
        public static double timeThreshold;                 /* Minimum time between data points to be considered a seperate train. */
        public static double distanceThreshold;             /* Minimum ditance between successive data points. */

        /* Simulation Parameters */
        public static double underpoweredLowerBound;        /* The lower bound cuttoff for the underpowered trains. */
        public static double underpoweredUpperBound;        /* The upper bound cuttoff for the underpowered trains. */
        public static double overpoweredLowerBound;         /* The lower bound cuttoff for the overpowered trains. */
        public static double overpoweredUpperBound;         /* The upper bound cuttoff for the overpowered trains. */
        public static double combinedLowerBound;            /* The lower bound cuttoff for the combined trains. */
        public static double combinedUpperBound;            /* The upper bound cuttoff for the combined trains. */

        public static bool HunterValleyRegion;

        /// <summary>
        /// This function resets the power to weight upper and lower boundaries to 
        /// default values when there are no power to weight ratio values available 
        /// for the trains.
        /// This means that all trains included will be classified as underpowered
        /// </summary>
        public static void resetPowerToWeightBoundariesToZero()
        {
            /* When the data has no opower to weight ratio for the train, the power to weight ratio will default to 0. 
             * The lower bound needs to allow this value to be include in the analysis.
             */
            underpoweredLowerBound = -1;
            underpoweredUpperBound = double.MaxValue / 2;

            overpoweredLowerBound = double.MaxValue / 2;
            overpoweredUpperBound = double.MaxValue;

            combinedLowerBound = -1;
            combinedUpperBound = double.MaxValue;
        }
        
    }



}
