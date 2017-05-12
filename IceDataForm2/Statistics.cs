using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsettings;

namespace TrainPerformance
{
    /* A class to perform statistics operations to attach to the aggregated data. */
    class Statistics
    {
                
        public static double numberOfTrains;
        public static double averageDistanceTravelled;
        public static double averageSpeed;
        public static double averagePowerToWeightRatio;
        public static double standardDeviationP2W;

        /// <summary>
        /// Default Statisitcs Constructor
        /// </summary>
        public Statistics()
        { 
        }

        /// <summary>
        /// Calculates the statistics of the list of trains passed in.
        /// </summary>
        /// <param name="trains">A list of train objects.</param>
        public void generateStats(List<Train> trains)
        {
            /* Extract the number of trains in the list */
            numberOfTrains = trains.Count();
            // This should be generic, should pass in only the lists that conforms to desired boundaries.

            List<double> distance = new List<double>();
            List<double> speed = new List<double>();
            List<double> power2Weight = new List<double>();

            double a1, a2;

            /* Cycle through all the trains. */
            foreach (Train train in trains)
            {
                a1 = train.TrainJourney.Where(t => t.speed > 0).Max(t => t.geometryKm);
                a2 = train.TrainJourney.Where(t => t.speed > 0).Min(t => t.geometryKm);

                /* Calculate the distance travelled for each train */
                double distanceTravelled = (train.TrainJourney.Where(t => t.speed > 0).Max(t => t.geometryKm) - train.TrainJourney.Where(t => t.speed > 0).Min(t => t.geometryKm));
                distance.Add(distanceTravelled);

                /* Calcualte the average speed of the train journey. */
                speed.Add(train.TrainJourney.Where(t => t.speed > 0).Average(t => t.speed));

                /* Add he power to weight ratio to the list. */
                power2Weight.Add(train.TrainJourney[0].powerToWeight);

            }

            /* Calcaulte the averages. */
            averageSpeed = speed.Average();
            averageDistanceTravelled = distance.Average();
            averagePowerToWeightRatio = power2Weight.Average();

            /* Calcualte the standard deviation of the power to weight ratios. */
            standardDeviationP2W = Math.Sqrt(power2Weight.Average(v => Math.Pow(v - averagePowerToWeightRatio, 2)));

        }

        ///* Stats for each direction. */
        //public void generateStats(List<Train> trains, direction direction)
        //{
        //}

        ///* Stats for each power band */
        //public void generateStats(List<Train> trains, double lower, double upper)
        //{
        //}

        public bool isStatisticsAvailable()
        {
            if (numberOfTrains > 0 &&
                averageDistanceTravelled > 0 &&
                averageSpeed > 0 &&
                averagePowerToWeightRatio > 0 &&
                standardDeviationP2W > 0)
                return true;
            
            return false;
        }
    }
}

