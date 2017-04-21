using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsettings;

namespace TrainPerformance
{
    class FileOperations
    {


        public static Tools tool = new Tools();



        /// <summary>
        /// Read the ICE data file.
        /// The file is assumed to be in a specific format.
        /// 
        /// 1       Alarms
        /// 2       Exclude
        /// 3       TrainID - Exclude
        /// 4       Train Shortlist
        /// 5       TrainCount
        /// 6       Direction
        /// 7       Extract Date Time
        /// 8       Faults
        /// 9       Insert Date Time
        /// 10      Journey ID	
        /// 11      KM Post	
        /// 12      Latitude	
        /// 13      Loco ID	
        /// 14      Longitude	
        /// 15      Notification Date Time	
        /// 16      Notification Date	
        /// 17      Notification Time	
        /// 18      Notification Type	
        /// 19      Number of Records	
        /// 20      Source System	
        /// 21      Speed	
        /// 22      Track Number	
        /// 23      Train ID	
        /// 24      Update Date Time
        /// 25      Version ID
        /// </summary>
        /// <param name="filename">The filename of the ICE data</param>
        /// <returns>The list of trainDetails objects containnig each valid record.</returns>
        public static List<TrainDetails> readICEData(string filename, List<string> excludeTrainList)
        {
            /* Read all the lines of the data file. */
            isFileOpen(filename);
                        
            string[] lines = System.IO.File.ReadAllLines(filename);
            char[] delimeters = { ',', '\t' };

            /* Seperate the fields. */
            string[] fields = lines[0].Split(delimeters);

            /* Initialise the fields of interest. */
            string TrainID = "none";
            string locoID = "none";
            double powerToWeight = 1.0;
            double speed = 0.0;
            double kmPost = 0.0;
            double geometryKm = 0.0;
            double latitude = 0.0;
            double longitude = 0.0;
            DateTime NotificationDateTime = new DateTime(2000, 1, 1);

            bool header = true;
            bool includeTrain = true;

            /* List of all valid train data. */
            List<TrainDetails> IceRecord = new List<TrainDetails>();

            foreach (string line in lines)
            {
                if (header)
                    /* Ignore the header line. */
                    header = false;
                else
                {
                    /* Seperate each record into each field */
                    fields = line.Split(delimeters);

                    TrainID = fields[22];
                    locoID = fields[12];
                    /* needs to perform tests */
                    double.TryParse(fields[20], out speed);
                    double.TryParse(fields[10], out kmPost);
                    double.TryParse(fields[11], out latitude);
                    double.TryParse(fields[13], out longitude);
                    DateTime.TryParse(fields[14], out NotificationDateTime);
                    double.TryParse(fields[25], out powerToWeight);
                    /* possible TSR information as well*/

                    /* Check if the train is in the exclude list */
                    includeTrain = excludeTrainList.Contains(TrainID);

                    if (latitude < Settings.topLeftLocation.latitude && latitude > Settings.bottomRightlocation.latitude &&
                        longitude > Settings.topLeftLocation.longitude && longitude < Settings.bottomRightlocation.longitude &&
                        NotificationDateTime >= Settings.dateRange[0] && NotificationDateTime < Settings.dateRange[1] &&
                        !includeTrain)
                    {
                        TrainDetails record = new TrainDetails(TrainID, locoID, powerToWeight, NotificationDateTime, latitude, longitude, speed, kmPost, geometryKm, direction.notSpecified, false, false, 0);
                        IceRecord.Add(record);
                    }

                }
            }

            /* Return the list of records. */
            return IceRecord;
        }

        /// <summary>
        /// This function reads the file with the list of trains to exclude from the 
        /// data and stores the list in a managable list object.
        /// The file is assumed to have one train per line or have each train seperated 
        /// by a common delimiter [ , \ " \t \n]
        /// </summary>
        /// <param name="filename">The full path of the file containing the list of trains to exclude.</param>
        /// <returns>The populated list of all trains to exclude.</returns>
        public static List<string> readTrainList(string filename)
        {
            List<string> excludeTrainList = new List<string>();

            /* Read all the lines of the file. */
            isFileOpen(filename);

            string[] lines = System.IO.File.ReadAllLines(filename);
            char[] delimeters = { ',', '\'', '"', '\t', '\n' };     // not sure of the delimter ??

            /* Seperate the fields. */
            string[] fields = lines[0].Split(delimeters);

            /* Add the trains to the list. */
            foreach (string line in lines)
                excludeTrainList.Add(line);


            return excludeTrainList;
        }

        /// <summary>
        /// Read the Traxim simulation files for the simulated data.
        /// </summary>
        /// <param name="filename">The simulation filename.</param>
        /// <returns>The list of data for the simualted train.</returns>
        public static List<simulatedTrain> readSimulationData(string filename)
        {
            /* Read all the lines of the data file. */
            isFileOpen(filename);

            string[] lines = System.IO.File.ReadAllLines(filename);
            char[] delimeters = { ',', '\t' };

            /* Seperate the fields. */
            string[] fields = lines[0].Split(delimeters);

            /* Initialise the fields of interest. */
            double kmPoint = 0;
            double singleLineKm = 0;
            double lat = 0;
            double lon = 0;
            double elevation = 0;
            string TraximNode = "none";
            string TraximSection = "none";
            double time = 0;
            double velocity = 0;
            double previousDistance = 0;
            double maxSpeed = 0;

            bool header = true;

            /* List of all simulated train data. */
            List<simulatedTrain> simulatedTrain = new List<simulatedTrain>();

            foreach (string line in lines)
            {
                if (header)
                    /* Ignore the header line. */
                    header = false;
                else
                {
                    /* Seperate each record into each field */
                    fields = line.Split(delimeters);

                    double.TryParse(fields[0], out kmPoint);
                    double.TryParse(fields[1], out lat);
                    double.TryParse(fields[2], out lon);
                    double.TryParse(fields[3], out elevation);
                    TraximNode = fields[4];
                    TraximSection = fields[6];
                    double.TryParse(fields[8], out time);
                    double.TryParse(fields[9], out velocity);
                    double.TryParse(fields[10], out previousDistance);
                    double.TryParse(fields[11], out maxSpeed);
                    double.TryParse(fields[14], out singleLineKm);

                    simulatedTrain record = new simulatedTrain(kmPoint, singleLineKm, lat, lon, elevation, TraximNode, TraximSection, time, velocity, previousDistance, maxSpeed);
                    simulatedTrain.Add(record);

                }
            }

            /* Return the list of records. */
            return simulatedTrain;
        }

        /// <summary>
        /// Write the train records to an excel file for inspection.
        /// </summary>
        /// <param name="trainRecords">The list of trainDetails object containing all the train records.</param>
        public static void writeTrainData(List<TrainDetails> trainRecords)
        {

            /* Create the microsfot excel references. */
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel._Workbook workbook;
            Microsoft.Office.Interop.Excel._Worksheet worksheet;

            /* Start Excel and get Application object. */
            excel = new Microsoft.Office.Interop.Excel.Application();

            /* Get the reference to the new workbook. */
            workbook = (Microsoft.Office.Interop.Excel._Workbook)(excel.Workbooks.Add(""));

            /* Create the header details. */
            string[] headerString = { "Train ID", "loco ID", " Notification Date Time", "Latitude", "Longitude", "Speed", 
                                        "km Post", "Actual Km", "Train Direction", "Loop", "TSR", "TSR Speed" };

            /* Pagenate the data for writing to excel. */
            int excelPageSize = 1000000;        /* Page size of the excel worksheet. */
            int excelPages = 1;                 /* Number of Excel pages to write. */
            int headerOffset = 2;

            /* Adjust the excel page size or the number of pages to write. */
            if (trainRecords.Count() < excelPageSize)
                excelPageSize = trainRecords.Count();
            else
                excelPages = (int)Math.Round((double)trainRecords.Count() / excelPageSize + 0.5);


            /* Deconstruct the train details into excel columns. */
            string[,] TrainID = new string[excelPageSize + 10, 1];
            string[,] LocoID = new string[excelPageSize + 10, 1];
            DateTime[,] NotificationTime = new DateTime[excelPageSize + 10, 1];
            double[,] latitude = new double[excelPageSize + 10, 1];
            double[,] longitude = new double[excelPageSize + 10, 1];
            double[,] speed = new double[excelPageSize, 1];
            double[,] kmPost = new double[excelPageSize, 1];
            double[,] geometryKm = new double[excelPageSize, 1];
            string[,] trainDirection = new string[excelPageSize, 1];
            string[,] loopLocation = new string[excelPageSize, 1];
            string[,] TSRLocation = new string[excelPageSize, 1];
            double[,] TSRspeed = new double[excelPageSize, 1];


            /* Loop through the excel pages. */
            for (int excelPage = 0; excelPage < excelPages; excelPage++)
            {
                /* Set the active worksheet. */
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets[excelPage + 1];
                workbook.Sheets[excelPage + 1].Activate();
                worksheet.get_Range("A1", "L1").Value2 = headerString;

                /* Loop through the data for each excel page. */
                for (int j = 0; j < excelPageSize; j++)
                {
                    /* Set the default loop and TSR parameters. */
                    loopLocation[j, 0] = "";
                    TSRLocation[j, 0] = "";
                    TSRspeed[j, 0] = 0;


                    /* Check we dont try to read more data than there really is. */
                    int checkIdx = j + excelPage * excelPageSize;
                    if (checkIdx < trainRecords.Count())
                    {
                        TrainID[j, 0] = trainRecords[checkIdx].TrainID;
                        LocoID[j, 0] = trainRecords[checkIdx].LocoID;
                        NotificationTime[j, 0] = trainRecords[checkIdx].NotificationDateTime;
                        latitude[j, 0] = trainRecords[checkIdx].location.latitude;
                        longitude[j, 0] = trainRecords[checkIdx].location.longitude;
                        speed[j, 0] = trainRecords[checkIdx].speed;
                        kmPost[j, 0] = trainRecords[checkIdx].kmPost;
                        geometryKm[j, 0] = trainRecords[checkIdx].geometryKm;
                        trainDirection[j, 0] = trainRecords[checkIdx].trainDirection.ToString();
                        if (trainRecords[checkIdx].isLoopHere)
                            loopLocation[j, 0] = "Loop";
                        if (trainRecords[checkIdx].isTSRHere)
                        {
                            TSRLocation[j, 0] = "TSR";
                            TSRspeed[j, 0] = trainRecords[checkIdx].TSRspeed;
                        }

                    }
                    else
                    {
                        /* The end of the data has been reached. Populate the remaining elements. */
                        TrainID[j, 0] = "";
                        LocoID[j, 0] = "";
                        NotificationTime[j, 0] = DateTime.MinValue;
                        latitude[j, 0] = 0.0;
                        longitude[j, 0] = 0.0;
                        speed[j, 0] = 0.0;
                        kmPost[j, 0] = 0;
                        geometryKm[j, 0] = 0.0;
                        trainDirection[j, 0] = direction.notSpecified.ToString();
                    }
                }

                /* Write the data to the active excel workseet. */
                worksheet.get_Range("A" + headerOffset, "A" + (headerOffset + excelPageSize - 1)).Value2 = TrainID;
                worksheet.get_Range("B" + headerOffset, "B" + (headerOffset + excelPageSize - 1)).Value2 = LocoID;
                worksheet.get_Range("C" + headerOffset, "C" + (headerOffset + excelPageSize - 1)).Value2 = NotificationTime;
                worksheet.get_Range("D" + headerOffset, "D" + (headerOffset + excelPageSize - 1)).Value2 = latitude;
                worksheet.get_Range("E" + headerOffset, "E" + (headerOffset + excelPageSize - 1)).Value2 = longitude;
                worksheet.get_Range("F" + headerOffset, "F" + (headerOffset + excelPageSize - 1)).Value2 = speed;
                worksheet.get_Range("G" + headerOffset, "G" + (headerOffset + excelPageSize - 1)).Value2 = kmPost;
                worksheet.get_Range("H" + headerOffset, "H" + (headerOffset + excelPageSize - 1)).Value2 = geometryKm;
                worksheet.get_Range("I" + headerOffset, "I" + (headerOffset + excelPageSize - 1)).Value2 = trainDirection;
                worksheet.get_Range("J" + headerOffset, "J" + (headerOffset + excelPageSize - 1)).Value2 = loopLocation;
                worksheet.get_Range("K" + headerOffset, "K" + (headerOffset + excelPageSize - 1)).Value2 = TSRLocation;
                worksheet.get_Range("L" + headerOffset, "L" + (headerOffset + excelPageSize - 1)).Value2 = TSRspeed;


            }

            /* Generate the resulting file name and location to save to. */
            string savePath = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis";
            string saveFilename = savePath + @"\ICEData_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            /* Check the file does not exist yet. */
            if (File.Exists(saveFilename))
                File.Delete(saveFilename);

            /* Save the excel file. */
            excel.UserControl = false;
            workbook.SaveAs(saveFilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            workbook.Close();

            return;
        }

        /// <summary>
        /// Write the train records to an excel file for inspection.
        /// </summary>
        /// <param name="trainRecords">The list of interpolatedTrain object containing all the train records.</param>
        public static void writeTrainData(List<InterpolatedTrain> trainRecords)
        {

            /* Create the microsfot excel references. */
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel._Workbook workbook;
            Microsoft.Office.Interop.Excel._Worksheet worksheet;

            /* Start Excel and get Application object. */
            excel = new Microsoft.Office.Interop.Excel.Application();

            /* Get the reference to the new workbook. */
            workbook = (Microsoft.Office.Interop.Excel._Workbook)(excel.Workbooks.Add(""));

            /* Create the header details. */
            string[] headerString = { "Train ID", "loco ID", " Notification Date Time", "Actual Km", "Speed", "Loop", "TSR", "TSR speed" };

            /* Pagenate the data for writing to excel. */
            int excelPageSize = 1000000;        /* Page size of the excel worksheet. */
            int excelPages = 1;                 /* Number of Excel pages to write. */
            int headerOffset = 2;

            /* Adjust the excel page size or the number of pages to write. */
            if (trainRecords.Count() < excelPageSize)
                excelPageSize = trainRecords.Count();
            else
                excelPages = (int)Math.Round((double)trainRecords.Count() / excelPageSize + 0.5);


            /* Deconstruct the train details into excel columns. */
            string[,] TrainID = new string[excelPageSize + 10, 1];
            string[,] LocoID = new string[excelPageSize + 10, 1];
            DateTime[,] NotificationTime = new DateTime[excelPageSize + 10, 1];
            double[,] speed = new double[excelPageSize, 1];
            double[,] geometryKm = new double[excelPageSize, 1];
            string[,] loopLocation = new string[excelPageSize, 1];
            string[,] TSRLocation = new string[excelPageSize, 1];
            double[,] TSRspeed = new double[excelPageSize, 1];

            /* Loop through the excel pages. */
            for (int excelPage = 0; excelPage < excelPages; excelPage++)
            {
                /* Set the active worksheet. */
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets[excelPage + 1];
                workbook.Sheets[excelPage + 1].Activate();
                worksheet.get_Range("A1", "H1").Value2 = headerString;

                /* Loop through the data for each excel page. */
                for (int j = 0; j < excelPageSize; j++)
                {
                    /* Set default loop and TSR parameters. */
                    loopLocation[j, 0] = "";
                    TSRLocation[j, 0] = "";
                    TSRspeed[j, 0] = 0;

                    /* Check we dont try to read more data than there really is. */
                    int checkIdx = j + excelPage * excelPageSize;
                    if (checkIdx < trainRecords.Count())
                    {
                        TrainID[j, 0] = trainRecords[checkIdx].TrainID;
                        LocoID[j, 0] = trainRecords[checkIdx].LocoID;
                        NotificationTime[j, 0] = trainRecords[checkIdx].NotificationDateTime;
                        speed[j, 0] = trainRecords[checkIdx].speed;
                        geometryKm[j, 0] = trainRecords[checkIdx].geometryKm;
                        if (trainRecords[checkIdx].isLoopeHere)
                            loopLocation[j, 0] = "Loop";
                        if (trainRecords[checkIdx].isTSRHere)
                        {
                            TSRLocation[j, 0] = "TSR";
                            TSRspeed[j, 0] = trainRecords[checkIdx].TSRspeed;
                        }
                    }
                    else
                    {
                        /* The end of the data has been reached. Populate the remaining elements. */
                        TrainID[j, 0] = "";
                        LocoID[j, 0] = "";
                        NotificationTime[j, 0] = DateTime.MinValue;
                        speed[j, 0] = 0.0;
                        geometryKm[j, 0] = 0.0;

                    }
                }

                /* Write the data to the active excel workseet. */
                worksheet.get_Range("A" + headerOffset, "A" + (headerOffset + excelPageSize - 1)).Value2 = TrainID;
                worksheet.get_Range("B" + headerOffset, "B" + (headerOffset + excelPageSize - 1)).Value2 = LocoID;
                worksheet.get_Range("C" + headerOffset, "C" + (headerOffset + excelPageSize - 1)).Value2 = NotificationTime;
                worksheet.get_Range("D" + headerOffset, "D" + (headerOffset + excelPageSize - 1)).Value2 = geometryKm;
                worksheet.get_Range("E" + headerOffset, "E" + (headerOffset + excelPageSize - 1)).Value2 = speed;
                worksheet.get_Range("F" + headerOffset, "F" + (headerOffset + excelPageSize - 1)).Value2 = loopLocation;
                worksheet.get_Range("G" + headerOffset, "G" + (headerOffset + excelPageSize - 1)).Value2 = TSRLocation;
                worksheet.get_Range("H" + headerOffset, "H" + (headerOffset + excelPageSize - 1)).Value2 = TSRspeed;

            }

            /* Generate the resulting file name and location to save to. */
            string savePath = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis";
            string saveFilename = savePath + @"\ICEData_Interpolated" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            /* Check the file does not exist yet. */
            if (File.Exists(saveFilename))
                File.Delete(saveFilename);

            /* Save the excel file. */
            excel.UserControl = false;
            workbook.SaveAs(saveFilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            workbook.Close();

            return;
        }

        public static void writeTrainDataForComparison(List<Train> trainRecords)
        {

            /* Create the microsfot excel references. */
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel._Workbook workbook;
            Microsoft.Office.Interop.Excel._Worksheet worksheet;

            /* Start Excel and get Application object. */
            excel = new Microsoft.Office.Interop.Excel.Application();

            /* Get the reference to the new workbook. */
            workbook = (Microsoft.Office.Interop.Excel._Workbook)(excel.Workbooks.Add(""));

            /* Create the header details. */
            string[] headerString1 = { "km", "Trains:" };
            string[] headerString2 = { "Train ID:" };
            string[] headerString3 = { "Loco ID:" };
            string[] headerString4 = { "Date:" };
            string[] headerString5 = { "Power to Weight Ratio:" };
            string[] headerString6 = { "Direction:" };


            /* Pagenate the data for writing to excel. */
            int excelPageSize = 1000000;        /* Page size of the excel worksheet. */
            int excelPages = 1;                 /* Number of Excel pages to write. */
            int headerOffset = 8;

            /* Adjust the excel page size or the number of pages to write. */
            if (trainRecords.Count() < excelPageSize)
                excelPageSize = trainRecords.Count();
            else
                excelPages = (int)Math.Round((double)trainRecords.Count() / excelPageSize + 0.5);

            int middle = (int) trainRecords[0].TrainJourney.Count() / 2;
            /* Deconstruct the train details into excel columns. */
            string[,] TrainID = new string[1, trainRecords.Count()];
            string[,] LocoID = new string[1, trainRecords.Count()];
            DateTime[,] NotificationTime = new DateTime[1, trainRecords.Count()];
            double[,] powerToWeight = new double[1, trainRecords.Count()];
            string[,] direction = new string[1, trainRecords.Count()];
            
            double[,] kilometerage = new double[trainRecords[0].TrainJourney.Count(), 1];
            double[,] speed = new double[trainRecords[0].TrainJourney.Count(), trainRecords.Count() ];
            

            /* Loop through the excel pages. */
            for (int excelPage = 0; excelPage < excelPages; excelPage++)
            {
                /* Set the active worksheet. */
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets[excelPage + 1];
                workbook.Sheets[excelPage + 1].Activate();
                worksheet.get_Range("A1", "B1").Value2 = headerString1;
                worksheet.get_Range("A2", "A2").Value2 = headerString2;
                worksheet.get_Range("A3", "A3").Value2 = headerString3;
                worksheet.get_Range("A4", "A4").Value2 = headerString4;
                worksheet.get_Range("A5", "A5").Value2 = headerString5;
                worksheet.get_Range("A6", "A6").Value2 = headerString6;

                /* Loop through the data for each excel page. */
                for (int trainIdx = 0; trainIdx < trainRecords.Count(); trainIdx++)
                {
                    TrainID[0, trainIdx] = trainRecords[trainIdx].TrainJourney[0].TrainID;
                    LocoID[0, trainIdx] = trainRecords[trainIdx].TrainJourney[0].LocoID;
                    NotificationTime[0, trainIdx] = trainRecords[trainIdx].TrainJourney[middle].NotificationDateTime;
                    powerToWeight[0, trainIdx] = trainRecords[trainIdx].TrainJourney[0].powerToWeight;
                    
                    if (trainRecords[trainIdx].TrainJourney[0].trainDirection == IceDataForm2.direction.increasing)
                        direction[0, trainIdx] = "Increasing";
                    else if (trainRecords[trainIdx].TrainJourney[0].trainDirection == IceDataForm2.direction.decreasing)
                        direction[0, trainIdx] = "Decreasing";
                    else
                        direction[0, trainIdx] = "Not Defined";

                    for (int journeyIdx = 0; journeyIdx < trainRecords[trainIdx].TrainJourney.Count(); journeyIdx++)
                    {
                        kilometerage[journeyIdx, 0] = Settings.startKm + Settings.interval / 1000 * journeyIdx;

                        speed[journeyIdx, trainIdx] = trainRecords[trainIdx].TrainJourney[journeyIdx].speed;

                    }
                }

                /* Write the data to the active excel workseet. */
                worksheet.Range[worksheet.Cells[2, 2], worksheet.Cells[2, trainRecords.Count() + 1]].Value2 = TrainID;
                worksheet.Range[worksheet.Cells[3, 2], worksheet.Cells[3, trainRecords.Count() + 1]].Value2 = LocoID;
                worksheet.Range[worksheet.Cells[4, 2], worksheet.Cells[4, trainRecords.Count() + 1]].Value2 = NotificationTime;
                worksheet.Range[worksheet.Cells[5, 2], worksheet.Cells[5, trainRecords.Count() + 1]].Value2 = powerToWeight;
                worksheet.Range[worksheet.Cells[6, 2], worksheet.Cells[6, trainRecords.Count() + 1]].Value2 = direction;

                worksheet.Range[worksheet.Cells[headerOffset, 1], worksheet.Cells[headerOffset + trainRecords[0].TrainJourney.Count()-1, 1]].Value2 = kilometerage;
                worksheet.Range[worksheet.Cells[headerOffset, 2], worksheet.Cells[headerOffset + trainRecords[0].TrainJourney.Count()-1, 2 + trainRecords.Count()-1]].Value2 = speed;
                
            }

            /* Generate the resulting file name and location to save to. */
            string savePath = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis";
            string saveFilename = savePath + @"\ICEData_InterpolatedCompare" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            /* Check the file does not exist yet. */
            if (File.Exists(saveFilename))
                File.Delete(saveFilename);

            /* Save the excel file. */
            excel.UserControl = false;
            workbook.SaveAs(saveFilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            workbook.Close();

            return;
        }

        /// <summary>
        /// Write the averaged Ice data to file.
        /// </summary>
        /// <param name="averageData">The average speed data for a train category in a single direction</param>
        /// <param name="averagecategory">A string describing the category for the average speed data; Suggested values: 
        /// underpoweredIncreasing
        /// underpoweredDecreasing
        /// overpoweredIncreasing
        /// overpoweredDecreasing
        /// </param>
        public static void writeAverageData(List<double> averageData, string averagecategory)
        {
            /* Create the microsfot excel references. */
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel._Workbook workbook;
            Microsoft.Office.Interop.Excel._Worksheet worksheet;

            /* Start Excel and get Application object. */
            excel = new Microsoft.Office.Interop.Excel.Application();

            /* Get the reference to the new workbook. */
            workbook = (Microsoft.Office.Interop.Excel._Workbook)(excel.Workbooks.Add(""));

            /* Create the header details. */
            string[] headerString = { "Kilometreage", "Average Speed" };

            /* Pagenate the data for writing to excel. */
            int excelPageSize = 1000000;        /* Page size of the excel worksheet. */
            int excelPages = 1;                 /* Number of Excel pages to write. */
            int headerOffset = 2;

            /* Adjust the excel page size or the number of pages to write. */
            if (averageData.Count() < excelPageSize)
                excelPageSize = averageData.Count();
            else
                excelPages = (int)Math.Round((double)averageData.Count() / excelPageSize + 0.5);


            /* Deconstruct the train details into excel columns. */
            double[,] kilometerage = new double[excelPageSize, 1];
            double[,] speed = new double[excelPageSize, 1];
            
            /* Loop through the excel pages. */
            for (int excelPage = 0; excelPage < excelPages; excelPage++)
            {
                /* Set the active worksheet. */
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets[excelPage + 1];
                workbook.Sheets[excelPage + 1].Activate();
                worksheet.get_Range("A1", "B1").Value2 = headerString;

                /* Loop through the data for each excel page. */
                for (int j = 0; j < excelPageSize; j++)
                {
                    /* Check we dont try to read more data than there really is. */
                    int checkIdx = j + excelPage * excelPageSize;
                    
                    kilometerage[j, 0] = Settings.startKm + Settings.interval/1000 * checkIdx;

                    /* Populate the average speed data. */                        
                    if (checkIdx < averageData.Count())
                        speed[j, 0] = averageData[checkIdx];
                    else
                        speed[j, 0] = 0.0;
                        

                    
                }

                /* Write the data to the active excel workseet. */
                worksheet.get_Range("A" + headerOffset, "A" + (headerOffset + excelPageSize - 1)).Value2 = kilometerage;
                worksheet.get_Range("B" + headerOffset, "B" + (headerOffset + excelPageSize - 1)).Value2 = speed;
                
            }

            /* Generate the resulting file name and location to save to. */
            string savePath = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis";
            string saveFilename = savePath + @"\"+averagecategory+"AverageSpeed_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            /* Check the file does not exist yet. */
            if (File.Exists(saveFilename))
                File.Delete(saveFilename);

            /* Save the excel file. */
            excel.UserControl = false;
            workbook.SaveAs(saveFilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            workbook.Close();

            return;
        
        
        }

        public static void writeTrainDataForComparison(List<Train> trainRecords)
        {

            /* Create the microsfot excel references. */
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel._Workbook workbook;
            Microsoft.Office.Interop.Excel._Worksheet worksheet;

            /* Start Excel and get Application object. */
            excel = new Microsoft.Office.Interop.Excel.Application();

            /* Get the reference to the new workbook. */
            workbook = (Microsoft.Office.Interop.Excel._Workbook)(excel.Workbooks.Add(""));

            /* Create the header details. */
            string[] headerString1 = { "km", "", "Trains:" };
            string[] headerString2 = { "","Train ID:" };
            string[] headerString3 = { "", "Loco ID:" };
            string[] headerString4 = { "", "Date:" };
            string[] headerString5 = { "", "Power to Weight Ratio:" };
            string[] headerString6 = { "", "Direction:" };


            /* Pagenate the data for writing to excel. */
            int excelPageSize = 1000000;        /* Page size of the excel worksheet. */
            int excelPages = 1;                 /* Number of Excel pages to write. */
            int headerOffset = 8;

            /* Adjust the excel page size or the number of pages to write. */
            if (trainRecords.Count() < excelPageSize)
                excelPageSize = trainRecords.Count();
            else
                excelPages = (int)Math.Round((double)trainRecords.Count() / excelPageSize + 0.5);

            int middle = (int)trainRecords[0].TrainJourney.Count() / 2;
            /* Deconstruct the train details into excel columns. */
            string[,] TrainID = new string[1, trainRecords.Count()];
            string[,] LocoID = new string[1, trainRecords.Count()];
            DateTime[,] NotificationTime = new DateTime[1, trainRecords.Count()];
            double[,] powerToWeight = new double[1, trainRecords.Count()];
            string[,] direction = new string[1, trainRecords.Count()];

            double[,] kilometerage = new double[trainRecords[0].TrainJourney.Count(), 1];
            double[,] speed = new double[trainRecords[0].TrainJourney.Count(), trainRecords.Count()];


            /* Loop through the excel pages. */
            for (int excelPage = 0; excelPage < excelPages; excelPage++)
            {
                /* Set the active worksheet. */
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets[excelPage + 1];
                workbook.Sheets[excelPage + 1].Activate();
                worksheet.get_Range("A1", "C1").Value2 = headerString1;
                worksheet.get_Range("B2", "C2").Value2 = headerString2;
                worksheet.get_Range("B3", "C3").Value2 = headerString3;
                worksheet.get_Range("B4", "C4").Value2 = headerString4;
                worksheet.get_Range("B5", "C5").Value2 = headerString5;
                worksheet.get_Range("B6", "C6").Value2 = headerString6;

                /* Loop through the data for each excel page. */
                for (int trainIdx = 0; trainIdx < trainRecords.Count(); trainIdx++)
                {
                    TrainID[0, trainIdx] = trainRecords[trainIdx].TrainJourney[0].TrainID;
                    LocoID[0, trainIdx] = trainRecords[trainIdx].TrainJourney[0].LocoID;
                    NotificationTime[0, trainIdx] = trainRecords[trainIdx].TrainJourney[middle].NotificationDateTime;
                    powerToWeight[0, trainIdx] = trainRecords[trainIdx].TrainJourney[0].powerToWeight;

                    if (trainRecords[trainIdx].TrainJourney[0].trainDirection == TrainPerformance.direction.increasing)
                        direction[0, trainIdx] = "Increasing";
                    else if (trainRecords[trainIdx].TrainJourney[0].trainDirection == TrainPerformance.direction.decreasing)
                        direction[0, trainIdx] = "Decreasing";
                    else
                        direction[0, trainIdx] = "Not Defined";

                    for (int journeyIdx = 0; journeyIdx < trainRecords[trainIdx].TrainJourney.Count(); journeyIdx++)
                    {
                        kilometerage[journeyIdx, 0] = Settings.startKm + Settings.interval / 1000 * journeyIdx;

                        speed[journeyIdx, trainIdx] = trainRecords[trainIdx].TrainJourney[journeyIdx].speed;

                    }
                }

                /* Write the data to the active excel workseet. */
                worksheet.Range[worksheet.Cells[2, 3], worksheet.Cells[2, trainRecords.Count() + 2]].Value2 = TrainID;
                worksheet.Range[worksheet.Cells[3, 3], worksheet.Cells[3, trainRecords.Count() + 2]].Value2 = LocoID;
                worksheet.Range[worksheet.Cells[4, 3], worksheet.Cells[4, trainRecords.Count() + 2]].Value2 = NotificationTime;
                worksheet.Range[worksheet.Cells[5, 3], worksheet.Cells[5, trainRecords.Count() + 2]].Value2 = powerToWeight;
                worksheet.Range[worksheet.Cells[6, 3], worksheet.Cells[6, trainRecords.Count() + 2]].Value2 = direction;

                worksheet.Range[worksheet.Cells[headerOffset, 1], worksheet.Cells[headerOffset + trainRecords[0].TrainJourney.Count() - 1, 1]].Value2 = kilometerage;
                worksheet.Range[worksheet.Cells[headerOffset, 3], worksheet.Cells[headerOffset + trainRecords[0].TrainJourney.Count() - 1, 3 + trainRecords.Count() - 1]].Value2 = speed;

            }

            /* Generate the resulting file name and location to save to. */
            string savePath = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis";
            string saveFilename = savePath + @"\ICEData_InterpolatedCompare" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            /* Check the file does not exist yet. */
            if (File.Exists(saveFilename))
                File.Delete(saveFilename);

            /* Save the excel file. */
            excel.UserControl = false;
            workbook.SaveAs(saveFilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            workbook.Close();

            return;
        } 

        /// <summary>
        /// Write all catagories of the averaged Ice data to a file.
        /// </summary>
        /// <param name="underpoweredIncreasing">The underpwoered increasing average data.</param>
        /// <param name="underpoweredDecreasing">The underpwoered decreasing average data.</param>
        /// <param name="overpoweredIncreasing">The overpwoered increasing average data.</param>
        /// <param name="overpoweredDecreasing">The overpwoered decreasing average data.</param>
        public static void writeAverageData(List<double> underpoweredIncreasing, List<double> underpoweredDecreasing,List<double> overpoweredIncreasing, List<double> overpoweredDecreasing)
        {
            /* Create the microsfot excel references. */
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel._Workbook workbook;
            Microsoft.Office.Interop.Excel._Worksheet worksheet;

            /* Start Excel and get Application object. */
            excel = new Microsoft.Office.Interop.Excel.Application();

            /* Get the reference to the new workbook. */
            workbook = (Microsoft.Office.Interop.Excel._Workbook)(excel.Workbooks.Add(""));

            /* Create the header details. */
            string[] headerString = { "Kilometreage", "Underpowered Increasing Speed", "Underpowered Decreasing Speed", "Overpowered Increasing Speed", "Overpowered Decreasing Speed" };

            /* Pagenate the data for writing to excel. */
            int excelPageSize = 1000000;        /* Page size of the excel worksheet. */
            int excelPages = 1;                 /* Number of Excel pages to write. */
            int headerOffset = 2;

            /* Adjust the excel page size or the number of pages to write. */
            if (underpoweredIncreasing.Count() == underpoweredDecreasing.Count() &&
                underpoweredDecreasing.Count() == overpoweredIncreasing.Count() &&
                overpoweredIncreasing.Count() == overpoweredDecreasing.Count())
            {
                if (underpoweredIncreasing.Count() < excelPageSize)
                    excelPageSize = underpoweredIncreasing.Count();
                else
                    excelPages = (int)Math.Round((double)underpoweredIncreasing.Count() / excelPageSize + 0.5);
            }
            else
            {
                tool.messageBox("The averaged data is not the same size.","Average data error.");
                return;
            }

            /* Deconstruct the train details into excel columns. */
            double[,] kilometerage = new double[excelPageSize, 1];
            double[,] underpoweredIncreasingSpeed = new double[excelPageSize, 1];
            double[,] underpoweredDecreasingSpeed = new double[excelPageSize, 1];
            double[,] overpoweredIncreasingSpeed = new double[excelPageSize, 1];
            double[,] overpoweredDecreasingSpeed = new double[excelPageSize, 1];

            /* Loop through the excel pages. */
            for (int excelPage = 0; excelPage < excelPages; excelPage++)
            {
                /* Set the active worksheet. */
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets[excelPage + 1];
                workbook.Sheets[excelPage + 1].Activate();
                worksheet.get_Range("A1", "E1").Value2 = headerString;

                /* Loop through the data for each excel page. */
                for (int j = 0; j < excelPageSize; j++)
                {
                    /* Check we dont try to read more data than there really is. */
                    int checkIdx = j + excelPage * excelPageSize;

                    kilometerage[j, 0] = Settings.startKm + Settings.interval/1000 * checkIdx;
                    underpoweredIncreasingSpeed[j, 0] = 0;
                    underpoweredDecreasingSpeed[j, 0] = 0;
                    overpoweredIncreasingSpeed[j, 0] = 0;
                    overpoweredDecreasingSpeed[j, 0] = 0;

                    /* Populate the average speed data. */
                    if (checkIdx < underpoweredIncreasing.Count())
                    {
                        underpoweredIncreasingSpeed[j, 0] = underpoweredIncreasing[checkIdx];
                        underpoweredDecreasingSpeed[j, 0] = underpoweredDecreasing[checkIdx];
                        overpoweredIncreasingSpeed[j, 0] = overpoweredIncreasing[checkIdx];
                        overpoweredDecreasingSpeed[j, 0] = overpoweredDecreasing[checkIdx];
                    }
                    
                }

                /* Write the data to the active excel workseet. */
                worksheet.get_Range("A" + headerOffset, "A" + (headerOffset + excelPageSize - 1)).Value2 = kilometerage;
                worksheet.get_Range("B" + headerOffset, "B" + (headerOffset + excelPageSize - 1)).Value2 = underpoweredIncreasingSpeed;
                worksheet.get_Range("C" + headerOffset, "C" + (headerOffset + excelPageSize - 1)).Value2 = underpoweredDecreasingSpeed;
                worksheet.get_Range("D" + headerOffset, "D" + (headerOffset + excelPageSize - 1)).Value2 = overpoweredIncreasingSpeed;
                worksheet.get_Range("E" + headerOffset, "E" + (headerOffset + excelPageSize - 1)).Value2 = overpoweredDecreasingSpeed;

            }

            /* Generate the resulting file name and location to save to. */
            string savePath = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis";
            string saveFilename = savePath + @"\AverageSpeed_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            /* Check the file does not exist yet. */
            if (File.Exists(saveFilename))
                File.Delete(saveFilename);

            /* Save the excel file. */
            excel.UserControl = false;
            workbook.SaveAs(saveFilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            workbook.Close();

            return;


        }

        /// <summary>
        /// Write all catagories of the averaged Ice data to a file, information includes the loop boundary flags.
        /// </summary>
        /// <param name="averageData">A list of train data containing the average speed and loop boundary locations for each catagory.</param>
        public static void writeAverageData(List<averagedTrainData> averageData)
        {
            /* Create the microsfot excel references. */
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel._Workbook workbook;
            Microsoft.Office.Interop.Excel._Worksheet worksheet;

            /* Start Excel and get Application object. */
            excel = new Microsoft.Office.Interop.Excel.Application();

            /* Get the reference to the new workbook. */
            workbook = (Microsoft.Office.Interop.Excel._Workbook)(excel.Workbooks.Add(""));

            /* Create the header details. */
            string[] headerString = { "Kilometreage", "Underpowered Increasing Speed", "Underpowered Decreasing Speed", "Overpowered Increasing Speed", "Overpowered Decreasing Speed" , "Loop"};
            /* Pagenate the data for writing to excel. */
            int excelPageSize = 1000000;        /* Page size of the excel worksheet. */
            int excelPages = 1;                 /* Number of Excel pages to write. */
            int headerOffset = 2;

            /* Adjust the excel page size or the number of pages to write. */
            if (averageData.Count() < excelPageSize)
                excelPageSize = averageData.Count();
            else
                excelPages = (int)Math.Round((double)averageData.Count() / excelPageSize + 0.5);
            

            /* Deconstruct the train details into excel columns. */
            double[,] kilometerage = new double[excelPageSize, 1];
            double[,] underpoweredIncreasingSpeed = new double[excelPageSize, 1];
            double[,] underpoweredDecreasingSpeed = new double[excelPageSize, 1];
            double[,] overpoweredIncreasingSpeed = new double[excelPageSize, 1];
            double[,] overpoweredDecreasingSpeed = new double[excelPageSize, 1];
            string[,] isLoophere = new string[excelPageSize, 1];

            /* Loop through the excel pages. */
            for (int excelPage = 0; excelPage < excelPages; excelPage++)
            {
                /* Set the active worksheet. */
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets[excelPage + 1];
                workbook.Sheets[excelPage + 1].Activate();
                worksheet.get_Range("A1", "F1").Value2 = headerString;

                /* Loop through the data for each excel page. */
                for (int j = 0; j < excelPageSize; j++)
                {
                    /* Check we dont try to read more data than there really is. */
                    int checkIdx = j + excelPage * excelPageSize;

                    kilometerage[j, 0] = Settings.startKm + Settings.interval / 1000 * checkIdx;
                    underpoweredIncreasingSpeed[j, 0] = 0;
                    underpoweredDecreasingSpeed[j, 0] = 0;
                    overpoweredIncreasingSpeed[j, 0] = 0;
                    overpoweredDecreasingSpeed[j, 0] = 0;
                    isLoophere[j,0] = "";

                    /* Populate the average speed data. */
                    if (checkIdx < averageData.Count())
                    {
                        underpoweredIncreasingSpeed[j, 0] = averageData[checkIdx].underpoweredIncreaseingAverage;
                        underpoweredDecreasingSpeed[j, 0] = averageData[checkIdx].underpoweredDecreaseingAverage;
                        overpoweredIncreasingSpeed[j, 0] = averageData[checkIdx].overpoweredIncreaseingAverage;
                        overpoweredDecreasingSpeed[j, 0] = averageData[checkIdx].overpoweredDecreaseingAverage;
                        if (averageData[checkIdx].isInLoopBoundary)
                            isLoophere[j, 0] = "Loop Boundary";

                    }

                }

                /* Write the data to the active excel workseet. */
                worksheet.get_Range("A" + headerOffset, "A" + (headerOffset + excelPageSize - 1)).Value2 = kilometerage;
                worksheet.get_Range("B" + headerOffset, "B" + (headerOffset + excelPageSize - 1)).Value2 = underpoweredIncreasingSpeed;
                worksheet.get_Range("C" + headerOffset, "C" + (headerOffset + excelPageSize - 1)).Value2 = underpoweredDecreasingSpeed;
                worksheet.get_Range("D" + headerOffset, "D" + (headerOffset + excelPageSize - 1)).Value2 = overpoweredIncreasingSpeed;
                worksheet.get_Range("E" + headerOffset, "E" + (headerOffset + excelPageSize - 1)).Value2 = overpoweredDecreasingSpeed;
                worksheet.get_Range("F" + headerOffset, "F" + (headerOffset + excelPageSize - 1)).Value2 = isLoophere;

            }

            /* Generate the resulting file name and location to save to. */
            string savePath = @"S:\Corporate Strategy\Infrastructure Strategies\Simulations\Train Performance Analysis";
            string saveFilename = savePath + @"\AverageSpeed_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            /* Check the file does not exist yet. */
            if (File.Exists(saveFilename))
                File.Delete(saveFilename);

            /* Save the excel file. */
            excel.UserControl = false;
            workbook.SaveAs(saveFilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            workbook.Close();

            return;


        }

        /// <summary>
        /// Determine if a file is already open before trying to read the file.
        /// </summary>
        /// <param name="filename">Filename of the file to be opened</param>
        /// <returns>True if the file is already open.</returns>
        public static void isFileOpen(string filename)
        {
            /* Can the file be opened and read. */
            try
            {
                string[] l = System.IO.File.ReadAllLines(filename);
            }
            catch (IOException e)
            {
                /* File is already opended and locked for reading. */
                tool.messageBox(e.Message + ":\n\nClose the file and start again");
                Environment.Exit(0);
            }

        }

    }   // Class FileOperations
}
