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
        public string catagory;
        public double numberOfTrains;
        public double averageDistanceTravelled;
        public double averageSpeed;
        public double averagePowerToWeightRatio;
        public double standardDeviationP2W;

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
        public static Statistics generateStats(List<Train> trains, string catagory)
        {
            Statistics stats = new Statistics();
            
            stats.catagory = catagory;

            /* Extract the number of trains in the list */
            stats.numberOfTrains = trains.Count();

            List<double> distance = new List<double>();
            List<double> speed = new List<double>();
            List<double> power2Weight = new List<double>();

            /* Cycle through all the trains. */
            foreach (Train train in trains)
            {
                /* Calculate the distance travelled for each train */
                double distanceTravelled = (train.TrainJourney.Where(t => t.speed > 0).Max(t => t.geometryKm) - train.TrainJourney.Where(t => t.speed > 0).Min(t => t.geometryKm));
                distance.Add(distanceTravelled);

                /* Calcualte the average speed of the train journey. */
                speed.Add(train.TrainJourney.Where(t => t.speed > 0).Average(t => t.speed));

                /* Add he power to weight ratio to the list. */
                power2Weight.Add(train.TrainJourney[0].powerToWeight);

            }

            /* Calcaulte the averages. */
            if (speed.Count() > 0)
                stats.averageSpeed = speed.Average();
            else
                stats.averageSpeed = 0;

            if (distance.Count() > 0)
                stats.averageDistanceTravelled = distance.Average();
            else
                stats.averageDistanceTravelled = 0;

            if (power2Weight.Count() > 0)
            {
                stats.averagePowerToWeightRatio = power2Weight.Average();
                /* Calcualte the standard deviation of the power to weight ratios. */
                stats.standardDeviationP2W = Math.Sqrt(power2Weight.Average(v => Math.Pow(v - stats.averagePowerToWeightRatio, 2)));
            }
            else
            {
                stats.averagePowerToWeightRatio = 0;
                stats.standardDeviationP2W = 0;
            }

            return stats;
        }

        /// <summary>
        /// Determine if the statistics have been calculated
        /// </summary>
        /// <returns>True if the generateStats function has been called at least once.</returns>
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


    } // Statistics Class
}

