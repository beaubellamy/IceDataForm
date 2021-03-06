﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsettings;

namespace TrainPerformance
{
    /* A class to control all file operations, such as read and 
     * write to file for the required objects. 
     */
    class FileOperations
    {


        public static Tools tool = new Tools();



        /// <summary>
        /// This function reads the data file that should be created using Tableau to extract 
        /// the Data into a csv/txt file. 
        /// The function produces a list of train records which can then be cleaned and 
        /// seperated into individual train journies. The data can then be analysed as 
        /// individual trains.
        /// The assumed format is described below:
        /// 
        /// 1       Latitude
        /// 2       Locomotive Code (Loco ID)
        /// 3       Longitude
        /// 4       Extract Date Time [Poll Movement] (Notification Date Time)
        /// 5       Operator Name
        /// 6       RAMS Commodity Description
        /// 7       Train ID [Poll Movement]
        /// 8       Horse Power (Power to Weight Ratio)
        /// 9       KM Post
        /// 10      Speed
        /// </summary>
        /// <param name="filename">The filename of the ICE data</param>
        /// <param name="excludeTrainList">A list of trains to exclude from teh analysis</param>
        /// <returns>The list of trainDetails objects containnig each valid record.</returns>
        public static List<TrainDetails> readICEData(string filename, List<string> excludeTrainList)
        {
            int a = 0;
            /* Read all the lines of the data file. */
            isFileOpen(filename);
                        
            string[] lines = System.IO.File.ReadAllLines(filename);
            char[] delimeters = { '\t' };

            /* Seperate the fields. */
            string[] fields = lines[0].Split(delimeters);

            /* Initialise the fields of interest. */
            string TrainID = "none";
            string locoID = "none";
            trainOperator Operator = trainOperator.unknown;
            string subOperator = null;
            string commodity = "none";
            double powerToWeight = 0.0;
            double speed = 0.0;
            double kmPost = 0.0;
            double geometryKm = 0.0;
            double latitude = 0.0;
            double longitude = 0.0;
            DateTime NotificationDateTime = DateTime.MinValue;

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

                    TrainID = fields[6];
                    locoID = fields[1];
                    if (fields[4].Count() >= 3)
                        subOperator = fields[4].Substring(0,3);
                    
                    switch (subOperator)
                    { 
                        case "Aur": // Aurizon
                            Operator = trainOperator.Aurizon;
                            break;  
                        case "Pac": // Pac Nat - [Coal / - Intermodal / -Rural & Bulk], Pacific Nat - ADHOC GRAIN
                            Operator = trainOperator.PacificNational;
                            break;
                        case "Fre": // Freightliner
                            Operator = trainOperator.Freightliner;
                            break;
                        default:    // Australian Rail Track Corporation, RailCorp
                            Operator = trainOperator.unknown;
                            break;
                    }


                    commodity = fields[5];

                    /* Ensure values are valid while reading them out. */
                    double.TryParse(fields[9], out speed);
                    double.TryParse(fields[8], out kmPost);
                    double.TryParse(fields[0], out latitude);
                    double.TryParse(fields[2], out longitude);
                    DateTime.TryParse(fields[3], out NotificationDateTime);
                    double.TryParse(fields[7], out powerToWeight);

                    // Assign appropriate value for operator, and comodity

                    /* possible TSR information as well*/
                    /* TSR region
                     * Start km
                     * end km
                     * TSR issue Data
                     * TSR lift date
                     */

                    /* Check if the train is in the exclude list */
                    includeTrain = excludeTrainList.Contains(TrainID);

                    if (latitude < Settings.topLeftLocation.latitude && latitude > Settings.bottomRightLocation.latitude &&
                        longitude > Settings.topLeftLocation.longitude && longitude < Settings.bottomRightLocation.longitude &&
                        NotificationDateTime >= Settings.dateRange[0] && NotificationDateTime < Settings.dateRange[1] &&
                        !includeTrain)
                    {
                        TrainDetails record = new TrainDetails(TrainID, locoID, Operator, powerToWeight, NotificationDateTime, latitude, longitude, 
                            speed, kmPost, geometryKm, direction.notSpecified, false, false, 0);
                        IceRecord.Add(record);
                                                
                    }

                }
            }

            /* Return the list of records. */
            return IceRecord;
        }

        /// <summary>
        /// Function reads in the track geometry data from file.
        /// </summary>
        /// <param name="filename">Full filename of the geometry file.</param>
        /// <returns>A list of track Geometry objects describing the track geometry.</returns>
        public static List<TrackGeometry> readGeometryfile(string filename)
        {
            Processing processing = new Processing();

            /* Create the list of track geometry objects. */
            List<TrackGeometry> trackGeometry = new List<TrackGeometry>();

            bool header = true;

            /* Read all the lines of the file. */
            string[] lines = System.IO.File.ReadAllLines(filename);
            char[] delimeters = { ',', '\'', '"', '\t', '\n' };     // not sure of the delimter ??

            /* Seperate the fields. */
            string[] fields = lines[0].Split(delimeters);

            bool firstPoint = true;

            /* Define the track geomerty parameters. */
            string geometryName = null;
            double latitude = 0.0;
            double longitude = 0.0;
            double elevation = 0.0;
            double kilometreage = 0.0;
            double virtualKilometreage = 0.0;
            bool isLoopHere = false;

            /* Define some additional helper parameters. */
            double distance = 0;
            direction direction = direction.notSpecified;
            double previousLat = 0;
            double previousLong = 0;
            double previouskm = 0;
            string loop;

            /* Add the trains to the list. */
            foreach (string line in lines)
            {
                if (header)
                    /* Ignore the header line. */
                    header = false;
                else
                {
                    /* Seperate each record into each field */
                    fields = line.Split(delimeters);
                    geometryName = fields[0];
                    double.TryParse(fields[1], out latitude);
                    double.TryParse(fields[2], out longitude);
                    double.TryParse(fields[3], out elevation);
                    double.TryParse(fields[4], out kilometreage);
                    loop = fields[6];

                    if (loop.Equals("loop", StringComparison.OrdinalIgnoreCase) || loop.Equals("true", StringComparison.OrdinalIgnoreCase))
                        isLoopHere = true;
                    else
                        isLoopHere = false;

                    /* The virtual kilometreage starts at the first kilometreage of the track. */
                    if (firstPoint)
                    {
                        virtualKilometreage = kilometreage;
                        /* Set the 'pervious' parameters. */
                        previousLat = latitude;
                        previousLong = longitude;
                        previouskm = kilometreage;
                        firstPoint = false;
                    }
                    else
                    {
                        /* Determine the direction the track kilometreage. */
                        if (direction == direction.notSpecified)
                        {
                            if (kilometreage - previouskm > 0)
                                direction = direction.increasing;
                            else
                                direction = direction.decreasing;
                        }

                        /* Calcualte the distance between succesive points and increment the virtual kilometreage. */
                        distance = processing.calculateGreatCircleDistance(previousLat, previousLong, latitude, longitude);
                        
                        if (direction == direction.increasing)
                            virtualKilometreage = virtualKilometreage + distance / 1000;

                        else
                            virtualKilometreage = virtualKilometreage - distance / 1000;

                        /* Set the 'previous' parameters. */
                        previousLat = latitude;
                        previousLong = longitude;

                    }

                    /* Add the geometry point to the list. */
                    TrackGeometry geometry = new TrackGeometry(0, geometryName, latitude, longitude, elevation, kilometreage, virtualKilometreage, isLoopHere);
                    trackGeometry.Add(geometry);

                }
            }


            return trackGeometry;
        }

        /// <summary>
        /// Read the file containing the temporary speed restriction information and 
        /// store in a manalgable list of TSR objects, which contain all neccessary 
        /// information for each TSR.
        /// </summary>
        /// <param name="filename">TSR file</param>
        /// <returns>List of TSR objects contianting the parameters for each TSR.</returns>
        public static List<TSRObject> readTSRFile(string filename)
        {
            /* Read all the lines of the data file. */
            isFileOpen(filename);

            string[] lines = System.IO.File.ReadAllLines(filename);
            char[] delimeters = { ',', '\t' };

            /* Seperate the fields. */
            string[] fields = lines[0].Split(delimeters);

            /* Initialise the fields of interest. */
            string region = "none";
            DateTime issueDate = DateTime.MinValue;
            DateTime liftedDate = DateTime.MinValue;
            double startKm = 0.0;
            double endKm = 0.0;
            double speed = 0.0;

            bool header = true;

            /* List of all TSR details. */
            List<TSRObject> TSRList = new List<TSRObject>();

            foreach (string line in lines)
            {
                if (header)
                    /* Ignore the header line. */
                    header = false;
                else
                {
                    /* Seperate each record into each field */
                    fields = line.Split(delimeters);

                    region = fields[0];
                    /* needs to perform tests */
                    DateTime.TryParse(fields[1], out issueDate);
                    DateTime.TryParse(fields[2], out liftedDate);
                    double.TryParse(fields[10], out startKm);
                    double.TryParse(fields[11], out endKm);
                    double.TryParse(fields[5], out speed);

                    /* Set the lift date if the TSR applies the full time period. */
                    if (liftedDate == DateTime.MinValue)
                        liftedDate = Settings.dateRange[1];

                    /* Add the TSR properties that are within the period of analysis. */
                    if (issueDate < Settings.dateRange[1] && liftedDate >= Settings.dateRange[0])                        
                    {
                        TSRObject record = new TSRObject(region, issueDate, liftedDate, startKm, endKm, speed);
                        TSRList.Add(record);
                    }
                    
                }
            }

            /* Return the list of TSR records. */
            return TSRList;
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
            char[] delimeters = { ',', '\'', '"', '\t', '\n' };

            /* Seperate the fields. */
            string[] fields = lines[0].Split(delimeters);

            /* Add the trains to the list. */
            foreach (string line in lines)
                excludeTrainList.Add(line);

            return excludeTrainList;
        }

        /// <summary>
        /// This function reads the Traxim simulation files and populates the simualtedTrain 
        /// data for comparison to the averaged ICE data.
        /// </summary>
        /// <param name="filename">The simulation filename.</param>
        /// <returns>The list of data for the simualted train.</returns>
        public static List<simulatedTrain> readSimulationData(string filename)
        {
            /* Read all the lines of the data file. */
            isFileOpen(filename);

            string[] lines = System.IO.File.ReadAllLines(filename);
            char[] delimeters = { ',', '\'', '"', '\t', '\n' };

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
        /// This function writes the train records to an excel file for inspection.
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
            string savePath = FileSettings.aggregatedDestination;
            string saveFilename = savePath + @"\ICEData_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            /* Check the file does not exist yet. */
            if (File.Exists(saveFilename))             
            {
                isFileOpen(saveFilename);
                File.Delete(saveFilename);
            }

            /* Save the excel file. */
            excel.UserControl = false;
            workbook.SaveAs(saveFilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            workbook.Close();

            return;
        }

        /// <summary>
        ///This function writes the interpolated train records to an excel file for inspection.
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
            string savePath = FileSettings.aggregatedDestination;
            string saveFilename = savePath + @"\ICEData_Interpolated" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            /* Check the file does not exist yet. */
            if (File.Exists(saveFilename))
            {
                isFileOpen(saveFilename);
                File.Delete(saveFilename);
            }

            /* Save the excel file. */
            excel.UserControl = false;
            workbook.SaveAs(saveFilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            workbook.Close();

            return;
        }
        
        /// <summary>
        /// This function writes each interpolated train journey to an individual column in excel.
        /// This can be used to compare against previously completed corridor analysis for validation.
        /// </summary>
        /// <param name="trainRecords">List of trains containing the interpolated data.</param>
        public static void writeTrainData(List<Train> trainRecords)
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
            string[] headerString2 = { "", "Train ID:" };
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
                worksheet.get_Range("A2", "B2").Value2 = headerString2;
                worksheet.get_Range("A3", "B3").Value2 = headerString3;
                worksheet.get_Range("A4", "B4").Value2 = headerString4;
                worksheet.get_Range("A5", "B5").Value2 = headerString5;
                worksheet.get_Range("A6", "B6").Value2 = headerString6;

                /* Loop through the data for each excel page. */
                for (int trainIdx = 0; trainIdx < trainRecords.Count(); trainIdx++)
                {
                    TrainID[0, trainIdx] = trainRecords[trainIdx].TrainJourney[0].TrainID;
                    LocoID[0, trainIdx] = trainRecords[trainIdx].TrainJourney[0].LocoID;
                    NotificationTime[0, trainIdx] = findMinDate(trainRecords[trainIdx].TrainJourney);                    

                    powerToWeight[0, trainIdx] = trainRecords[trainIdx].TrainJourney[0].powerToWeight;

                    if (trainRecords[trainIdx].TrainJourney[0].trainDirection == TrainPerformance.direction.increasing)
                        direction[0, trainIdx] = "Increasing";
                    else if (trainRecords[trainIdx].TrainJourney[0].trainDirection == TrainPerformance.direction.decreasing)
                        direction[0, trainIdx] = "Decreasing";
                    else if (trainRecords[trainIdx].TrainJourney[0].trainDirection == TrainPerformance.direction.invalid)
                        direction[0, trainIdx] = "Invalid";
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
            string savePath = FileSettings.aggregatedDestination;
            string saveFilename = savePath + @"\ICEData_InterpolatedTrains" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            /* Check the file does not exist yet. */
            if (File.Exists(saveFilename))
            {
                isFileOpen(saveFilename);
                File.Delete(saveFilename);
            }

            /* Save the excel file. */
            excel.UserControl = false;
            workbook.SaveAs(saveFilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            workbook.Close();

            return;
        }

        /// <summary>
        /// This function writes all catagories of the averaged Ice data to a file, information includes the loop boundary flags.
        /// </summary>
        /// <param name="averageData">A list of train data containing the average speed and loop boundary locations for each catagory.</param>
        public static void writeAverageData(List<averagedTrainData> averageData, List<Statistics> stats)
        {
            /* Create the microsfot excel references. */
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel._Workbook workbook;
            Microsoft.Office.Interop.Excel._Worksheet worksheet;

            /* Start Excel and get Application object. */
            excel = new Microsoft.Office.Interop.Excel.Application();

            /* Get the reference to the new workbook. */
            workbook = (Microsoft.Office.Interop.Excel._Workbook)(excel.Workbooks.Add(""));

            /* Extract the statistics */
            /* Note: there is no check to confimr the order in which the statistics values are listed. */
            string[,] statisticsHeader = { { "Statistics:" }, { "Number Of Trains" }, { "Average Distance Travelled" }, { "Average Speed" }, { "Average P/W Ratio" }, { "P/W standard Deviation" } };
            string[,] totalStatistics = new string[statisticsHeader.GetLength(0), stats.Count()];


            for (int index = 0; index < stats.Count(); index++)
            {
                totalStatistics[0, index] = stats[index].catagory;
                totalStatistics[1, index] = stats[index].numberOfTrains.ToString();
                totalStatistics[2, index] = stats[index].averageDistanceTravelled.ToString();
                totalStatistics[3, index] = stats[index].averageSpeed.ToString();
                totalStatistics[4, index] = stats[index].averagePowerToWeightRatio.ToString();
                totalStatistics[5, index] = stats[index].standardDeviationP2W.ToString();

            }

            int catagories = 2;

            if (averageData.Sum(t => t.alternativeIncreaseingAverage) > 0 && averageData.Sum(t => t.alternativeDecreaseingAverage) > 0)
                catagories = 3;
            

            /* Create the header details. */
            string[] headerString = new string[] {};
            if (Settings.HunterValleyRegion)
            {
                if (catagories == 2)
                    headerString = new string[] { "Kilometreage", "Elevation", "Pacific National Increasing Speed", "Pacific National Decreasing Speed", 
                        "Aurizon Increasing Speed", "Aurizon Decreasing Speed", "Weighted Average Increasing Speed", "Weighted Average Decreasing Speed", "Loop", "TSRs" };
                
                if (catagories == 3)
                    headerString = new string[] { "Kilometreage", "Elevation", "Pacific National Increasing Speed", "Pacific National Decreasing Speed", 
                        "Aurizon Increasing Speed", "Aurizon Decreasing Speed", "Freightliner Increasing Speed", "Freightliner Decreasing Speed", 
                        "Weighted Average Increasing Speed", "Weighted Average Decreasing Speed", "Loop", "TSRs" };
            }
            else
                headerString = new string[] { "Kilometreage", "Elevation", "Underpowered Increasing Speed", "Underpowered Decreasing Speed", "Overpowered Increasing Speed", 
                                        "Overpowered Decreasing Speed", "Weighted Average Increasing Speed", "Weighted Average Decreasing Speed", "Loop", "TSRs" };

            /* Pagenate the data for writing to excel. */
            int excelPageSize = 1000000;        /* Page size of the excel worksheet. */
            int excelPages = 1;                 /* Number of Excel pages to write. */
            int headerOffset = statisticsHeader.GetLength(0) + 4;

            /* Adjust the excel page size or the number of pages to write. */
            if (averageData.Count() < excelPageSize)
                excelPageSize = averageData.Count();
            else
                excelPages = (int)Math.Round((double)averageData.Count() / excelPageSize + 0.5);
            

            /* Deconstruct the train details into excel columns. */
            double[,] kilometerage = new double[excelPageSize, 1];
            double[,] elevation = new double[excelPageSize, 1];
            double[,] underpoweredIncreasingSpeed = new double[excelPageSize, 1];   // Pacific National
            double[,] underpoweredDecreasingSpeed = new double[excelPageSize, 1];   // Pacific National
            double[,] overpoweredIncreasingSpeed = new double[excelPageSize, 1];    // Aurizon
            double[,] overpoweredDecreasingSpeed = new double[excelPageSize, 1];    // Aurizon
            double[,] alternativeIncreasingSpeed = new double[excelPageSize, 1];    // Freightliner
            double[,] alternativeDecreasingSpeed = new double[excelPageSize, 1];    // Freightliner

            double[,] totalIncreasingSpeed = new double[excelPageSize, 1];
            double[,] totalDecreasingSpeed = new double[excelPageSize, 1];
            string[,] isLoophere = new string[excelPageSize, 1];
            string[,] isTSRhere = new string[excelPageSize, 1];

            
            
            /* Loop through the excel pages. */
            for (int excelPage = 0; excelPage < excelPages; excelPage++)
            {
                /* Set the active worksheet. */
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets[excelPage + 1];
                workbook.Sheets[excelPage + 1].Activate();
                
                /* Loop through the data for each excel page. */
                for (int j = 0; j < excelPageSize; j++)
                {
                    /* Check we dont try to read more data than there really is. */
                    int checkIdx = j + excelPage * excelPageSize;

                    kilometerage[j, 0] = Settings.startKm + Settings.interval / 1000 * checkIdx;
                    elevation[j, 0] = 0;
                   
                    underpoweredIncreasingSpeed[j, 0] = 0;
                    underpoweredDecreasingSpeed[j, 0] = 0;
                    overpoweredIncreasingSpeed[j, 0] = 0;
                    overpoweredDecreasingSpeed[j, 0] = 0;
                    alternativeIncreasingSpeed[j, 0] = 0;
                    alternativeDecreasingSpeed[j, 0] = 0;

                    totalIncreasingSpeed[j, 0] = 0;
                    totalDecreasingSpeed[j, 0] = 0;
                    isLoophere[j, 0] = "";
                    isTSRhere[j, 0] = "";

                    /* Populate the average speed data. */
                    if (checkIdx < averageData.Count())
                    {
                        elevation[j, 0] = averageData[checkIdx].elevation;
                        underpoweredIncreasingSpeed[j, 0] = averageData[checkIdx].underpoweredIncreaseingAverage;   // Pacific National
                        underpoweredDecreasingSpeed[j, 0] = averageData[checkIdx].underpoweredDecreaseingAverage;   // Pacific National
                        overpoweredIncreasingSpeed[j, 0] = averageData[checkIdx].overpoweredIncreaseingAverage;     // Aurizon
                        overpoweredDecreasingSpeed[j, 0] = averageData[checkIdx].overpoweredDecreaseingAverage;     // Aurizon
                        if (catagories == 3)
                        {
                            alternativeIncreasingSpeed[j, 0] = averageData[checkIdx].alternativeIncreaseingAverage;     // Freightliner
                            alternativeDecreasingSpeed[j, 0] = averageData[checkIdx].alternativeDecreaseingAverage;     // Freightliner
                        }
                        totalIncreasingSpeed[j, 0] = averageData[checkIdx].totalIncreasingAverage;
                        totalDecreasingSpeed[j, 0] = averageData[checkIdx].totalDecreasingAverage;

                        if (averageData[checkIdx].isInLoopBoundary)
                            isLoophere[j, 0] = "Loop Boundary";

                        if (averageData[checkIdx].isTSRboundary)
                            isTSRhere[j, 0] = "TSR Boundary";

                    }

                }

                /* Display the statistics for each catagory. */
                int column = 3;
                Microsoft.Office.Interop.Excel.Range topLeft = 
                    (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[statisticsHeader.GetLength(1), column];
                Microsoft.Office.Interop.Excel.Range bottomRight =
                    (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[statisticsHeader.GetLength(0), column + totalStatistics.GetLength(1) - 1];

                /* Set statistics. */
                worksheet.get_Range("A1", "A6").Value2 = statisticsHeader;
                worksheet.get_Range(topLeft, bottomRight).Value2 = totalStatistics;


                /* Generalise the row and columns */
                /* Set the data header. */
                worksheet.get_Range("A9", "L9").Value2 = headerString;

                /* Write the data to the active excel workseet. */
                worksheet.get_Range("A" + headerOffset, "A" + (headerOffset + excelPageSize - 1)).Value2 = kilometerage;
                worksheet.get_Range("B" + headerOffset, "B" + (headerOffset + excelPageSize - 1)).Value2 = elevation;
                worksheet.get_Range("C" + headerOffset, "C" + (headerOffset + excelPageSize - 1)).Value2 = underpoweredIncreasingSpeed;     // Pacific National
                worksheet.get_Range("D" + headerOffset, "D" + (headerOffset + excelPageSize - 1)).Value2 = underpoweredDecreasingSpeed;     // Pacific National
                worksheet.get_Range("E" + headerOffset, "E" + (headerOffset + excelPageSize - 1)).Value2 = overpoweredIncreasingSpeed;      // Aurizon
                worksheet.get_Range("F" + headerOffset, "F" + (headerOffset + excelPageSize - 1)).Value2 = overpoweredDecreasingSpeed;      // Aurizon
                if (catagories == 3)
                {
                    worksheet.get_Range("G" + headerOffset, "G" + (headerOffset + excelPageSize - 1)).Value2 = alternativeIncreasingSpeed;      // Freightliner
                    worksheet.get_Range("H" + headerOffset, "H" + (headerOffset + excelPageSize - 1)).Value2 = alternativeDecreasingSpeed;      // Freightliner
                    worksheet.get_Range("I" + headerOffset, "I" + (headerOffset + excelPageSize - 1)).Value2 = totalIncreasingSpeed;
                    worksheet.get_Range("J" + headerOffset, "J" + (headerOffset + excelPageSize - 1)).Value2 = totalDecreasingSpeed;
                    worksheet.get_Range("K" + headerOffset, "K" + (headerOffset + excelPageSize - 1)).Value2 = isLoophere;
                    worksheet.get_Range("L" + headerOffset, "L" + (headerOffset + excelPageSize - 1)).Value2 = isTSRhere;
                }
                else
                {
                    worksheet.get_Range("G" + headerOffset, "G" + (headerOffset + excelPageSize - 1)).Value2 = totalIncreasingSpeed;
                    worksheet.get_Range("H" + headerOffset, "H" + (headerOffset + excelPageSize - 1)).Value2 = totalDecreasingSpeed;
                    worksheet.get_Range("I" + headerOffset, "I" + (headerOffset + excelPageSize - 1)).Value2 = isLoophere;
                    worksheet.get_Range("J" + headerOffset, "J" + (headerOffset + excelPageSize - 1)).Value2 = isTSRhere;
                }
                

            }

            
            /* Generate the resulting file name and location to save to. */
            string savePath = FileSettings.aggregatedDestination;
            string saveFilename = savePath + @"\AverageSpeed_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

            /* Check the file does not exist yet. */
            if (File.Exists(saveFilename))
            {
                isFileOpen(saveFilename);
                File.Delete(saveFilename);
            }

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
            FileStream stream = null;

            /* Can the file be opened and read. */
            try
            {
                stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read);
            }
            catch (IOException e)
            {
                /* File is already opended and locked for reading. */
                tool.messageBox(e.Message + ":\n\nClose the file and Start again.");
                Environment.Exit(0);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

        }

        /// <summary>
        /// Find the minimum notification date in the train journey.
        /// </summary>
        /// <param name="journey">The train details for each point of the journey.</param>
        /// <returns>The earliest date of the train journey.</returns>
        private static DateTime findMinDate(List<TrainDetails> journey)
        {

            /* Set the minimum value */
            DateTime minDate = DateTime.MaxValue;

            foreach (TrainDetails point in journey)
            {
                if (point.NotificationDateTime > DateTime.MinValue && point.NotificationDateTime < minDate)
                    minDate = point.NotificationDateTime;
            }

            return minDate;
        }

        

    }   // Class FileOperations
}
