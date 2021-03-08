using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Ven
    // Dated Created: 1/22/2020
    // Last Modified: 1/25/2020
    //
    // **************************************************

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        TalentShowDisplayMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderDisplayMenuScreen(finchRobot);
                        break;

                    case "d":
                        AlarmSystemDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region ALARM SYSTEM

        /// <summary>
        /// *****************************************************************
        /// *                     Alarm System Menu                          *
        /// *****************************************************************
        /// </summary>
        static void AlarmSystemDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            string sensorsToMonitor = "";
            string rangeType = "";
            int minMaxThresholdValue = 0;
            int timeToMonitor = 0;

            do
            {
                DisplayScreenHeader("Alarm System Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) SetMinimum/Maximum Threshold Value");
                Console.WriteLine("\td) Set Time To Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorsToMonitor = AlarmSystemDisplaySetSensors();
                        break;

                    case "b":
                        rangeType = AlarmSystemDisplayRangeType();
                        break;

                    case "c":
                        minMaxThresholdValue = AlarmSystemDisplayGetThresholdValue(sensorsToMonitor, finchRobot);
                        break;

                    case "d":
                        timeToMonitor = AlarmSystemDisplayTimeToMonitor();
                        break;

                    case "e":
                        AlarmSystemDisplaySetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }


        /// <summary>
        /// set alarm
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="sensorsToMonitor"></param>
        /// <param name="rangeType"></param>
        /// <param name="minMaxThresholdValue"></param>
        /// <param name="timeToMonitor"></param>
        static void AlarmSystemDisplaySetAlarm(Finch finchRobot, string sensorsToMonitor, string rangeType, int minMaxThresholdValue, int timeToMonitor)
        {

            bool thresholdExceeeded = false;
            int secondsElapsed = 1;
            //light levels are int
            int leftLightSensorValue;
            int rightLightSensorValue;

            DisplayScreenHeader("Set Alarm");

            // echo values to user

            // prompt user

            do
            {
                //
                //get and current light levels
                //
                leftLightSensorValue = finchRobot.getLeftLightSensor();
                rightLightSensorValue = finchRobot.getRightLightSensor();

                // dispaly
                switch (sensorsToMonitor)
                {
                    case "left":
                        Console.WriteLine($"\tCurrent left light Senser: {leftLightSensorValue}");
                        break;

                    case "right":
                        Console.WriteLine($"\tCurrent right light Senser: {rightLightSensorValue}");
                        break;

                    case "both":
                        Console.WriteLine($"\tCurrent left light Senser: {leftLightSensorValue}");
                        Console.WriteLine($"\tCurrent right light Senser: {rightLightSensorValue}");
                        break;

                    default:
                        Console.WriteLine("\tUnknown Sensor Reference");
                        break;
                }
                //
                // wait 1 second and incrament
                //
                finchRobot.wait(1000);
                secondsElapsed++;

                //
                //test for threashold exceeded

                switch (sensorsToMonitor)
                {
                    case "left":
                        if (rangeType == "minimum")
                        {
                            thresholdExceeeded = (leftLightSensorValue < minMaxThresholdValue);
                        }
                        else // max
                        {
                            thresholdExceeeded = (leftLightSensorValue > minMaxThresholdValue);
                        }
                        break;

                    case "right":
                        if (rangeType == "minimum")
                        {
                            if (rightLightSensorValue < minMaxThresholdValue)
                            {
                                thresholdExceeeded = true;
                            }
                        }
                        else // max
                        {
                            if (rightLightSensorValue > minMaxThresholdValue)
                            {
                                thresholdExceeeded = true;
                            }
                        }
                        break;

                    case "both":
                        if (rangeType == "minimum")
                        {
                            if ((leftLightSensorValue < minMaxThresholdValue) || (rightLightSensorValue < minMaxThresholdValue))
                            {
                                thresholdExceeeded = true;
                            }
                        }
                        else
                        {
                            if ((leftLightSensorValue > minMaxThresholdValue) || (rightLightSensorValue > minMaxThresholdValue))
                            {
                                thresholdExceeeded = true;
                            }
                        }
                        break;

                    default:
                        Console.WriteLine("\tUnknown Sensor Reference");
                        break;
                }

            } while (!thresholdExceeeded && (secondsElapsed <= timeToMonitor));

            //display result
            if (thresholdExceeeded)
            {
                Console.WriteLine("Threshold exceeded");
                finchRobot.setLED(255, 0, 0);
                finchRobot.noteOn(330);
                finchRobot.wait(2000);
                finchRobot.setLED(0, 0, 0);
                finchRobot.noteOff();
            }
            else
            {
                Console.WriteLine("Threshold not exceeded: Time limit met");
            }

            DisplayMenuPrompt("Alarm System");
        }


        /// <summary>
        /// time to monitor
        /// </summary>
        /// <returns>time to monitor</returns>
        static int AlarmSystemDisplayTimeToMonitor()
        {
            int timeToMonitor = 0;
            bool validResponce;

            DisplayScreenHeader("Time To Monitor");

            do
            {

                validResponce = true;
                Console.Write("\tTime to Monitor");
                string userResponce = Console.ReadLine().Trim();
                if (!int.TryParse(userResponce, out timeToMonitor) || timeToMonitor < 0)
                {
                    Console.WriteLine("Please enter positive number example[1, 2, 3]");
                    validResponce = false;
                }

            } while (!validResponce);

            Console.WriteLine($"\tYou chose {timeToMonitor} as Time to Monitor");

            DisplayMenuPrompt("Alarm System");

            return timeToMonitor;
        }


        /// <summary>
        /// threshhold value
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <returns>threshold value</returns>
        static int AlarmSystemDisplayGetThresholdValue(string sensorsToMonitor, Finch finchRobot)
        {
            int thresholdValue = 0;

            int currentLeftSensorValue = finchRobot.getLeftLightSensor();
            int currentRightSensorValue = finchRobot.getRightLightSensor();
            bool validResponce;

            DisplayScreenHeader("Threshold Value");

            //
            //display ambiant light value
            //
            switch (sensorsToMonitor)
            {
                case "left":
                    Console.WriteLine($"Current {sensorsToMonitor} Sensor Value {currentLeftSensorValue}");
                    break;

                case "right":
                    Console.WriteLine($"Current {sensorsToMonitor} Sensor Value {currentRightSensorValue}");
                    break;

                case "both":
                    Console.WriteLine($"Current {sensorsToMonitor} Sensor Value {currentLeftSensorValue}");
                    Console.WriteLine($"Current {sensorsToMonitor} Sensor Value {currentRightSensorValue}");
                    break;

                default:
                    Console.WriteLine("\tUnknown Sensor Reference");
                    break;
            }

            //
            //get threshold from user
            //

            do
            {

                validResponce = true;
                Console.Write("\tEnter Threshold Value");
                string userResponce = Console.ReadLine().Trim();
                if (!int.TryParse(userResponce, out thresholdValue) || thresholdValue < 0)
                {
                    Console.WriteLine("Please enter positive number example[1, 2, 3]");
                    validResponce = false;
                }

            } while (!validResponce);

            Console.WriteLine($"\tYou chose {thresholdValue} as your threshold value");


            DisplayMenuPrompt("Alarm System");

            return thresholdValue;

        }


        /// <summary>
        /// range type
        /// </summary>
        /// <returns>range type</returns>
        static string AlarmSystemDisplayRangeType()
        {
            string rangeType = "";
            

            DisplayScreenHeader("Range Type");

            Console.WriteLine("What range type [min, max]");
            Console.ReadLine();
            if (rangeType.Length > 3)
            {
                Console.WriteLine("Invalid responce");
            }
            Console.WriteLine($"\tYou chose {rangeType} as your range type");

            DisplayMenuPrompt("Alarm System");

            return rangeType;

        }


        /// <summary>
        /// sensors to monitor
        /// </summary>
        /// <returns>sensors to monitor</returns>
        static string AlarmSystemDisplaySetSensors()
        {
            string sensorsToMonitor = "";

            DisplayScreenHeader("Sensors to Monitor");

            Console.WriteLine("Enter Sensor to Monitor [left, right, both");
            Console.ReadLine();
            if (sensorsToMonitor.Length > 4)
            {
                Console.WriteLine("Invalid responce");
            }
            Console.WriteLine($"\tYou chose {sensorsToMonitor} as your sensors");

            DisplayMenuPrompt("Alarm System");

            return sensorsToMonitor;
        }


        #endregion

        #region DATA RECORDER
        /// <summary>
        /// *****************************************************************
        /// *                     Data Recorder Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DataRecorderDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] temperatures = null;

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number Of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\te) Show Temperature in F");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        temperatures = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayData(temperatures);
                        break;

                    case "e":
                        DataRecorderDisplayCelsius(temperatures);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static double DataRecorderDisplayCelsius(double[] temperatures)
        {
            DisplayScreenHeader("Temperature in Farhenheit");

            double fahrenheitTemp;




            fahrenheitTemp = (temperatures[0] * (9 / 5)) + 32;

            DisplayContinuePrompt();
            return fahrenheitTemp;
        }


        /// <summary>
        /// data table
        /// </summary>
        /// <param name="temperatures"></param>
        static void DataRecorderDisplayDataTable(double[] temperatures)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Reading #".PadLeft(20) +
                "Temperature".PadLeft(15)
                );
            Console.WriteLine();
            Console.WriteLine(
                "-------".PadLeft(20) +
                "-----------".PadLeft(15)
                );
            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                (index + 1).ToString().PadLeft(20) +
                (temperatures[index]).ToString("n1").PadLeft(15)
                );
            }
        }

        /// <summary>
        /// Display data
        /// </summary>
        /// <param name="temperatures"></param>
        static void DataRecorderDisplayData(double[] temperatures)
        {
            DisplayScreenHeader("Temperature");

            DataRecorderDisplayDataTable(temperatures);

            DisplayContinuePrompt();
        }


        /// <summary>
        /// get temperatures from robot
        /// </summary>
        /// <param name="numberOfDataPoints">number of data points</param>
        /// <param name="dataPointFrequency">data point frequency</param>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>temperatures</returns>
        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch finchRobot)
        {
            double[] temperatures = new double[numberOfDataPoints];
            int dataPointFrequencyMs;

            //convert the frequency in seconds to ms

            dataPointFrequencyMs = (int)(dataPointFrequency * 1000);

            DisplayScreenHeader("Temperatures");

            //echo the values
            Console.WriteLine($"\tThe finch robot will now record {numberOfDataPoints} temperatures {dataPointFrequency} seconds apart:");//Validate
            Console.WriteLine("\tPress any key to begin");
            Console.ReadKey();

            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperatures[index] = finchRobot.getTemperature();

                // echo new temperature

                Console.WriteLine($"\tTemperature {index + 1}: {temperatures[index]:n1}");

                finchRobot.wait(dataPointFrequencyMs);
            }

            DataRecorderDisplayDataTable(temperatures);

            //Sum and Average

            double sumTemps;
            sumTemps = temperatures.Sum();

            double averageTemp;
            averageTemp = temperatures.Average();

            Console.WriteLine("\tSum of temp is: {0:n1} F", sumTemps);
            Console.WriteLine();
            Console.WriteLine("\tAverage temp: {0:n1} F", averageTemp);

            DisplayMenuPrompt("Data Recorder");

            return temperatures;
        }


        /// <summary>
        /// get data point frequency from user
        /// </summary>
        /// <returns>data point frequency</returns>
        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double dataPointFrequency;
            bool validResponce;


            DisplayScreenHeader("Data Point Frequency");


            do
            {
                validResponce = true;

                Console.Write("Data Points Frequency:");//Validate
                string userResponce = Console.ReadLine().Trim();
                //dataPointFrequency = double.Parse(Console.ReadLine());

                if (!double.TryParse(userResponce, out dataPointFrequency) || dataPointFrequency < 0)
                {
                    Console.WriteLine("please enter positive number [1, 2, 3 ...]");

                    validResponce = false;
                }


            } while (!validResponce);

            Console.WriteLine();
            Console.WriteLine($"\tYou chose {dataPointFrequency} as the number of data points frequency.");


            DisplayMenuPrompt("Data Recorder");

            return dataPointFrequency;
        }


        /// <summary>
        /// get number of Data Point from user
        /// </summary>
        /// <returns>number of data points</returns>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            double numberOfDataPoints;
            bool validResponce;

            DisplayScreenHeader("Number Of Data Points");

            do
            {
                validResponce = true;
                Console.Write("Number of Data Points:");//Validate
                string userResponce = Console.ReadLine().Trim();

                if (!double.TryParse(userResponce, out numberOfDataPoints) || numberOfDataPoints < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("please enter positive number [1, 2, 3 ...]");

                    validResponce = false;
                }

            } while (!validResponce);



            Console.WriteLine();
            Console.WriteLine($"\tYou chose {numberOfDataPoints} as the number of data points.");

            DisplayMenuPrompt("Data Recorder");

            return (int)numberOfDataPoints;
        }
        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void TalentShowDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance");
                Console.WriteLine("\tc) Mixing It Up");
                Console.WriteLine("\td) Song");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        TalentShowDisplayLightAndSound(finchRobot);
                        break;

                    case "b":
                        TalentShowDisplayDance(finchRobot);
                        break;

                    case "c":
                        TalentShowDisplayMixingItUp(finchRobot);
                        break;

                    case "d":
                        TalentShowDisplaySong(finchRobot);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void TalentShowDisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will not show off its glowing talent!");
            DisplayContinuePrompt();

            for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
            {
                finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                finchRobot.noteOn(lightSoundLevel * 100);
            }
            for (int numberOfFlashes = 0; numberOfFlashes < 3; numberOfFlashes++)
            {
                finchRobot.setLED(255, 0, 0);
                finchRobot.setLED(0, 255, 0);
                finchRobot.setLED(0, 0, 255);
                finchRobot.wait(500);
            }
            finchRobot.noteOn(261);
            finchRobot.wait(500);
            finchRobot.noteOff();

            finchRobot.wait(500);

            finchRobot.noteOn(261);
            finchRobot.wait(500);
            finchRobot.noteOff();

            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Song                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void TalentShowDisplaySong(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Song");

            Console.WriteLine("\tThe Finch robot will not show off its singing talent!");
            DisplayContinuePrompt();

            finchRobot.noteOn(330);
            finchRobot.wait(300);
            finchRobot.noteOff();
            finchRobot.wait(300);
            finchRobot.noteOn(330);
            finchRobot.wait(300);
            finchRobot.noteOff();
            finchRobot.wait(300);
            finchRobot.noteOn(330);
            finchRobot.wait(300);
            finchRobot.noteOff();
            finchRobot.wait(500);
            finchRobot.noteOn(330);
            finchRobot.wait(300);
            finchRobot.noteOff();
            finchRobot.wait(300);
            finchRobot.noteOn(330);
            finchRobot.wait(300);
            finchRobot.noteOff();
            finchRobot.wait(300);
            finchRobot.noteOn(330);
            finchRobot.wait(300);
            finchRobot.noteOff();
            finchRobot.wait(500);
            finchRobot.noteOn(330);
            finchRobot.wait(500);
            finchRobot.noteOn(392);
            finchRobot.wait(500);
            finchRobot.noteOn(261);
            finchRobot.wait(500);
            finchRobot.noteOn(294);
            finchRobot.wait(500);
            finchRobot.noteOn(330);
            finchRobot.wait(500);
            finchRobot.noteOff();

            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Dance                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void TalentShowDisplayDance(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Dance");

            Console.WriteLine("\tThe Finch robot will not show off its Dancing talent!");
            DisplayContinuePrompt();

            finchRobot.setMotors(0, 255);
            finchRobot.wait(1000);
            finchRobot.setMotors(255, 125);
            finchRobot.wait(1000);
            finchRobot.setMotors(0, 0);
            finchRobot.setMotors(125, 255);
            finchRobot.wait(1000);
            finchRobot.setMotors(255, 0);
            finchRobot.wait(3000);
            finchRobot.setMotors(0, 0);

            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Mixing It Up                  *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void TalentShowDisplayMixingItUp(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Mixing It Up");

            Console.WriteLine("\tThe Finch robot will not show off its glowing, dancing, and singing talent!");
            DisplayContinuePrompt();

            for (int lightSoundLevel = 0; lightSoundLevel < 200; lightSoundLevel++)
            {
                finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                finchRobot.noteOn(lightSoundLevel * 100);
            }

            finchRobot.setMotors(-255, 255);
            finchRobot.wait(1000);
            finchRobot.setMotors(255, -255);
            finchRobot.wait(1000);
            finchRobot.setMotors(0, 0);
            finchRobot.setMotors(-255, 255);
            finchRobot.wait(1000);
            finchRobot.setMotors(255, -255);
            finchRobot.wait(3000);
            finchRobot.setMotors(-255, 255);
            finchRobot.wait(1000);
            finchRobot.setMotors(255, -255);
            finchRobot.wait(1000);
            finchRobot.setMotors(0, 0);
            finchRobot.setMotors(-255, 255);
            finchRobot.wait(1000);
            finchRobot.setMotors(255, -255);
            finchRobot.wait(3000);
            finchRobot.setMotors(0, 0);


            DisplayMenuPrompt("Talent Show Menu");
        }

        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            // TODO test connection and provide user feedback - text, lights, sounds

            if (robotConnected)
            {
                Console.WriteLine();
                Console.WriteLine("\tRobot now connected");
                finchRobot.setLED(0, 255, 0);
                finchRobot.noteOn(12000);
                finchRobot.wait(1000);
                finchRobot.setLED(0, 0, 0);
                finchRobot.noteOff();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\t\tUnable to connect.");
            }

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
        
    }
}