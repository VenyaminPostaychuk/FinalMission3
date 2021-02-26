using System;
using System.Collections.Generic;
using System.IO;
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
                        
                        break;

                    case "d":
                        
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
