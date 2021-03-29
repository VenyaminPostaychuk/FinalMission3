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


    public enum Command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        GETTEMPERATURE,
        DO360TURN,
        DONE
    }

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            
            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        static void WriteThemeData(ConsoleColor foreground, ConsoleColor background)
        {
            string dataPath = @"Data/Theme.txt";

            File.WriteAllText(dataPath, foreground.ToString() + "\n");
            File.WriteAllText(dataPath, background.ToString());

            
        }

        static ConsoleColor GetConsoleColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;

            do
            {
                Console.Write($"\tEnter a value for the {property}:");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\t***** It appears you did not provide a valid console color. Please try again.*****\n");
                }
                else
                {
                    validConsoleColor = true;
                }
            } while (!validConsoleColor);

            return consoleColor;

           
        }

        static void DataDisplaysetTheme()
        {
            //1
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;

            // set current theme from data
            
            themeColors = DataReadThemeData();
            Console.ForegroundColor = themeColors.foregroundColor;
            Console.BackgroundColor = themeColors.backgroundColor;
            Console.Clear();
            DisplayScreenHeader("Set Application Theme");

            Console.WriteLine($"\nCurrent foreground color: {Console.ForegroundColor}");
            Console.WriteLine($"\nCurrent background color: {Console.BackgroundColor}");
            Console.WriteLine();

            Console.Write("\tWould you like to change theme [ yes | no ]?");
            if (Console.ReadLine().ToLower() == "yes")
            {
                do
                {
                    themeColors.foregroundColor = GetConsoleColorFromUser("foreground");
                    themeColors.backgroundColor = GetConsoleColorFromUser("background");

                    //set new theme

                    Console.ForegroundColor = themeColors.foregroundColor;
                    Console.BackgroundColor = themeColors.foregroundColor;
                    Console.Clear();
                    DisplayScreenHeader("Set Application Theme");
                    Console.WriteLine($"\tNew foreground color: {Console.ForegroundColor}");
                    Console.WriteLine($"\tNew background color: {Console.BackgroundColor}");

                    Console.WriteLine();
                    Console.Write("\tIs this the theme you would like?");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        themeChosen = true;
                        WriteThemeData(themeColors.foregroundColor, themeColors.backgroundColor);
                    }

                } while (!themeChosen);
            }
            DisplayContinuePrompt();

        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static (ConsoleColor foregroundColor, ConsoleColor backgroundColor) DataReadThemeData()
        {
            string dataPath = @"Data/Theme.txt";
            string[] themeColors;

            ConsoleColor foregroundColor;
            ConsoleColor backgroundColor;

            themeColors = File.ReadAllLines(dataPath);

            Enum.TryParse(themeColors[0], true, out foregroundColor);
            Enum.TryParse(themeColors[1], true, out backgroundColor);

            return (foregroundColor, backgroundColor);
            //WriteThemeData();
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
                Console.WriteLine("\tg) Set Theme");
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
                        UserProgramingDisplayMenuScreen(finchRobot);
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "g":
                        DataDisplaysetTheme();
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

        

        #region USER PROGRAMING
        /// <summary>
        /// *****************************************************************
        /// *                     User Programin Menu                          *
        /// *****************************************************************
        /// </summary>
        static void UserProgramingDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            //List<(Command command, int duration)> command = new List<(Command command, int duration)>();

            List<Command> commands = new List<Command>();

            do
            {
                DisplayScreenHeader("User Programing Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Command");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Comands");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgramingDisplayGetCommandParameters();
                        break;

                    case "b":
                        commands = UserProgramingDisplayGetFinchCommands();
                        break;

                    case "c":
                        UserProgramingDisplayViewCommands(commands);
                        break;

                    case "d":
                        UserProgramingDisplayExecuteCommands(finchRobot, commands, commandParameters);
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
        /// execute
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="commands"></param>
        /// <param name="commandParameters"></param>
        static void UserProgramingDisplayExecuteCommands(Finch finchRobot, List<Command> commands, (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            double waitSeconds = commandParameters.waitSeconds;
            DisplayScreenHeader("Execute commands");

            Console.WriteLine("\tFinch Robot will now execute commands");
            DisplayContinuePrompt();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        Console.WriteLine();
                        Console.WriteLine("\tDefaut Value Error");
                        Console.WriteLine();
                        break;
                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);

                        break;
                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        break;
                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        break;
                    case Command.WAIT:
                        int waitMilliseconds = (int)(waitSeconds * 1000);
                        finchRobot.wait(waitMilliseconds);
                        break;
                    case Command.TURNRIGHT:
                        finchRobot.setMotors(0, 200);
                        finchRobot.wait(1000);
                        finchRobot.setMotors(0, 0);
                        break;
                    case Command.TURNLEFT:
                        finchRobot.setMotors(200, 0);
                        finchRobot.wait(1000);
                        finchRobot.setMotors(0, 0);
                        break;
                    case Command.LEDON:
                        finchRobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        break;
                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        break;
                    case Command.GETTEMPERATURE:
                        finchRobot.getTemperature();
                        break;
                    case Command.DO360TURN:
                        finchRobot.setMotors(0, 200);
                        finchRobot.wait(3000);
                        finchRobot.setMotors(0, 0);
                        break;
                    case Command.DONE:

                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tInvalid");
                        Console.WriteLine();
                        break;
                }

                Console.WriteLine($"\tCommand: {command}");
            }

            DisplayContinuePrompt();
        }


        /// <summary>
        /// view
        /// </summary>
        /// <param name="commands"></param>
        static void UserProgramingDisplayViewCommands(List<Command> commands)
        {
            DisplayScreenHeader("View Commands");

            Console.WriteLine("\tCommand List");
            Console.WriteLine("\t-------------");

            foreach (Command command in commands)
            {
                Console.WriteLine("\t"+ command);
            }

            DisplayContinuePrompt();
        }


        /// <summary>
        /// finch commands
        /// </summary>
        /// <returns></returns>
        static List<Command> UserProgramingDisplayGetFinchCommands()
        {
            List<Command> commands = new List<Command>();

            bool isDone = false;
            string userResponce;

            DisplayScreenHeader("User Commands");
            Console.WriteLine("moveforward----movebackward----stopmotors----wait----done");
            Console.WriteLine("turnright----turnleft----ledon----ledoff----gettemerature----do360turn");
            do
            {
                Console.Write("\tCommand:");
                userResponce = Console.ReadLine();

                if (userResponce != "done")
                {
                    if (Enum.TryParse(userResponce.ToUpper(), out Command command))
                    {
                        commands.Add(command);
                    }
                    else
                    {
                        Console.WriteLine("\tPlease enter a proper command:");
                    }
                }
                else
                {
                    isDone = true;
                }

            } while (!isDone);

            DisplayContinuePrompt();
            return commands;
        }


        /// <summary>
        /// get command
        /// </summary>
        /// <returns>comandParameters</returns>
        static (int motorSpeed, int ledBrightness, double waitSeconds) UserProgramingDisplayGetCommandParameters()
        {
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;

            DisplayScreenHeader("Command Parameters");
            bool validResponce;

            //Console.Write("Moter Speed: ");
            //commandParameters.motorSpeed = int.Parse(Console.ReadLine());//validate
            do
            {
                validResponce = true;
                Console.Write("\tMotor Speed:");
                string userResponce = Console.ReadLine().Trim();
                if (!int.TryParse(userResponce, out commandParameters.motorSpeed) || commandParameters.motorSpeed > 255)
                {
                    Console.WriteLine("Please enter between 0 and 255");
                    validResponce = false;
                }

            } while (!validResponce);
            //Console.Write("LED Brightness: ");
            //commandParameters.ledBrightness = int.Parse(Console.ReadLine());//validate
            do
            {
                validResponce = true;
                Console.Write("\tLED Brightness:");
                string userResponce = Console.ReadLine().Trim();
                if (!int.TryParse(userResponce, out commandParameters.ledBrightness) || commandParameters.ledBrightness > 255)
                {
                    Console.WriteLine("Please enter between 0 and 255");
                    validResponce = false;
                }

            } while (!validResponce);
            //Console.Write("Wait Time (seconds): ");
            //commandParameters.waitSeconds = double.Parse(Console.ReadLine());//validate
            do
            {
                validResponce = true;
                Console.Write("\tWait Time (seconds):");
                string userResponce = Console.ReadLine().Trim();
                if (!double.TryParse(userResponce, out commandParameters.waitSeconds) || commandParameters.waitSeconds < 0)
                {
                    Console.WriteLine("Please enter a number bigger than 0");
                    validResponce = false;
                }

            } while (!validResponce);
            DisplayContinuePrompt();

            return commandParameters;
        }

        #endregion

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
            rangeType = Console.ReadLine();
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
            sensorsToMonitor = Console.ReadLine();
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