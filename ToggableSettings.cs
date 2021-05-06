using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Dungeonator;
using Gungeon;
using MonoMod.RuntimeDetour;
using System.IO;
using System.Threading.Tasks;
using System.Collections;

namespace EnemyRandomizer
{
    class ToggableSettings : MonoBehaviour
    {//set in ETGModConsole by player during runtime
        public static void ConsoleLineHandler (string[] args)
        {
            if (args.Length != 2)
            {
                string[] x = null;
                ETGModConsole.Log("Not valid input, see randhelp");
                
            }

            else
            {
                string command = args[0].Trim();
                string toggle = args[1].Trim().ToLower();
                ETGModConsole.Log("");
                if (command == "1") // Normal Enemies as bosses
                {
                    if (toggle == "on")
                    {
                        ToggableSettings.one = "on";
                    }

                    else
                    {
                        ToggableSettings.one = "off";
                    }
                    
                    ETGModConsole.Log("Normal Enemies as Bosses: " + ToggableSettings.one);
                }

                else if (command == "2") // Bosses as Normal Enemies
                {
                    if (toggle == "on")
                    {
                        ToggableSettings.two = "on";
                    }

                    else
                    {
                        ToggableSettings.two = "off";
                    }

                    ETGModConsole.Log("Bosses as Normal Enemies: " + ToggableSettings.two);

                }

                else if (command == "3") // Start w/ Random Gun and Item
                {
                    if (toggle == "on")
                    {
                        ToggableSettings.three = "on";
                    }

                    else
                    {
                        ToggableSettings.three = "off";
                    }

                    ETGModConsole.Log("Start w/ Random Gun and Item: " + ToggableSettings.three);

                }

                else
                {
                    ETGModConsole.Log("Not valid input, see randhelp");
                    
                }

                ETGModConsole.Log("Start new dungeon or floor to initiate changes");
                GRandomEnemyDataBaseHelper.ToggleDatabases();
            }



        }

        public static void DisplayStats()
        {
            
            ETGModConsole.Log("-----------[RANDOMIZER Modes]------------");
            ETGModConsole.Log("Normal Enemies as Bosses: rand 1 " + ToggableSettings.one);
            ETGModConsole.Log("Bosses as Normal Enemies: rand 2 " + ToggableSettings.two);
            ETGModConsole.Log("Start w/ Random Gun and Item: rand 3 " + ToggableSettings.three);
            ETGModConsole.Log("-----------------------------------------");
        }

        public static void StartingSettings()
        {
            ToggableSettings.one = "on";
            ToggableSettings.two = "on";
            ToggableSettings.three = "on";
            GRandomEnemyDataBaseHelper.ToggleDatabases();

        }

        public static string one;
        public static string two;
        public static string three;


        public static void GetStats(string[] notused)
        {
            ToggableSettings.DisplayStats();
        }

        public static void Help(string[] notused)
        {
            ETGModConsole.Log("");
            ETGModConsole.Log("To Toggle Modes example: rand 1 off");
            ETGModConsole.Log("Possible Modes:");
            ToggableSettings.DisplayStats();

        }

    }


}
