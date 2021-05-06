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

    public class GRandomRoomDatabaseHelper: MonoBehaviour
    {

        public static void Start()
        {
            CreatePastRooms();
            CreateAllSpecificDeathRooms();
        }

        private static void CreatePastRooms()
        {
            PastRooms.Add("Guide_End_Room");
            PastRooms.Add("Pilot_End_Room");
            PastRooms.Add("Convict_End_Room");
            PastRooms.Add("Robot_End_Room");
            PastRooms.Add("Soldier_End_Room");             
            PastRooms.Add("Bullet_End_Room_04");
            PastRooms.Add("LichRoom03");
        }
        private static void CreateAllSpecificDeathRooms()
        {
            AllSpecificDeathRooms.Add("Guide_End_Room");
            AllSpecificDeathRooms.Add("Pilot_End_Room");
            AllSpecificDeathRooms.Add("Convict_End_Room");
            AllSpecificDeathRooms.Add("Robot_End_Room");
            AllSpecificDeathRooms.Add("Soldier_End_Room");

            AllSpecificDeathRooms.Add("Bullet_End_Room_02");
            AllSpecificDeathRooms.Add("Bullet_End_Room_04");
            AllSpecificDeathRooms.Add("Bullet_End_Room_03");
            AllSpecificDeathRooms.Add("DraGunRoom01");
            AllSpecificDeathRooms.Add("ResourcefulRatRoom01");
            AllSpecificDeathRooms.Add("MetalGearRatRoom01");

            AllSpecificDeathRooms.Add("LichRoom01");
            AllSpecificDeathRooms.Add("LichRoom02");
            AllSpecificDeathRooms.Add("LichRoom03");




        }
        public static List<string> AllSpecificDeathRooms = new List<string>();
        public static List<string> PastRooms = new List<string>();


    }


    public class GRandomEnemyDataBaseHelper : MonoBehaviour
    {

        public static void Start()
        {
            RemoveEnemyDatabase.Add("05b8afe0b6cc4fffa9dc6036fa24c8ec"); //DraGunGold
            RemoveEnemyDatabase.Add("98e52539e1964749a8bbce0fe6a85d6b"); //unused_muzzle_flare
            RemoveEnemyDatabase.Add("3f11bbbc439c4086a180eb0fb9990cb4"); //killpillars
            RemoveEnemyDatabase.Add("e667fdd01f1e43349c03a18e5b79e579"); //tutorial turrets
            RemoveEnemyDatabase.Add("41ba74c517534f02a62f2e2028395c58"); //tutorial turrets x2
            RemoveEnemyDatabase.Add("e456b66ed3664a4cb590eab3a8ff3814"); //baby good mimic
            

            Create_Databases();

        }

        public static void DropEnemy(string[] _enemyguid)
        {   //method used for debugging, not used in regular settings
            int count = EnemyDatabase.Instance.Entries.Count;
            ETGModConsole.Log(string.Format("There are : {0} in Database", count.ToString()));
            bool re = false;

            ETGModConsole.Log("Removing: " + _enemyguid[0] + " from database");

            for (int i = 0; i < count; i++)
            {
                EnemyDatabaseEntry enemyDatabaseEntry = EnemyDatabase.Instance.Entries[i];

                if (enemyDatabaseEntry != null && enemyDatabaseEntry.myGuid == _enemyguid[0])

                {
                    ETGModConsole.Log("Found: " + _enemyguid[0] + " in database");
                    EnemyDatabase.Instance.Entries.Remove(enemyDatabaseEntry);
                    ETGModConsole.Log("Removed: " + _enemyguid[0] + " from database");
                    re = true;
                    break;
                }
            }

            if (re == false)
            {
                ETGModConsole.Log(_enemyguid[0] + " Not found in database");
            }

        }

        public static void Create_Databases()
        {

            for (int i =0; i < EnemyDatabase.Instance.Entries.Count(); i++)
            {
                EnemyDatabaseEntry EnemyDatabaseEntry = EnemyDatabase.Instance.Entries[i];
                string enemyGuid = EnemyDatabaseEntry.myGuid;

                if (!RemoveEnemyDatabase.Contains(enemyGuid))
                {
                    try
                    {

                        AIActor prefabActor = EnemyDatabase.GetOrLoadByGuid(enemyGuid);


                        if (prefabActor.healthHaver.IsBoss | prefabActor.healthHaver.IsSubboss)
                        {
                            BossOnlyDatabase.Add(enemyGuid);

                        }


                        if (prefabActor.IsNormalEnemy && prefabActor.IsWorthShootingAt && prefabActor.CanTargetPlayers && prefabActor.healthHaver.CanCurrentlyBeKilled && !prefabActor.IsMimicEnemy
                            && !prefabActor.IgnoreForRoomClear && !prefabActor.IsHarmlessEnemy)
                        {
                            BossRoomAllEnemies.Add(enemyGuid);
                        }

                        if (prefabActor.IsNormalEnemy && !prefabActor.healthHaver.IsBoss && !prefabActor.healthHaver.IsSubboss)
                        {
                            ClearOnlyDatabase_NoBosses.Add(enemyGuid);
                        }

                        if(prefabActor.IsNormalEnemy && prefabActor.IsWorthShootingAt && prefabActor.CanTargetPlayers && prefabActor.healthHaver.CanCurrentlyBeKilled && !prefabActor.IsMimicEnemy
                            && !prefabActor.IgnoreForRoomClear && !prefabActor.IsHarmlessEnemy && !prefabActor.healthHaver.IsBoss && !prefabActor.healthHaver.IsSubboss)
                        {
                            BossRoomRegularEnemiesOnly.Add(enemyGuid);
                        }

                        if(prefabActor.IsHarmlessEnemy)
                        {
                            HarmlessEnemyDatabase.Add(enemyGuid);
                        }


                        All_Database.Add(enemyGuid);

                    }

                    catch
                    {
                        Debug.Log("Error in loading to database: " + enemyGuid);
                        throw;
                    }
                }

                
            }

            SpecificEnemyDatabase.Add("0d3f7c641557426fbac8596b61c9fb45");//lord_of_the_jammed
            SpecificEnemyDatabase.Add("5d045744405d4438b371eb5ed3e2cdb2");//bishop
            SpecificEnemyDatabase.Add("ce2d2a0dced0444fb751b262ec6af08a");//DrWolf
            SpecificEnemyDatabase.Add("640238ba85dd4e94b3d6f68888e6ecb8");//robocop
            SpecificEnemyDatabase.Add("e456b66ed3664a4cb590eab3a8ff3814");//babygood mimic
            SpecificEnemyDatabase.Add("5fa8c86a65234b538cd022f726af2aea");//bulletman
            SpecificEnemyDatabase.Add("998807b57e454f00a63d67883fcf90d6");//turret

            SpecificEnemyDatabase.Add("6450d20137994881aff0ddd13e3d40c8");//mimic
            SpecificEnemyDatabase.Add("abfb454340294a0992f4173d6e5898a8");//mimic
            SpecificEnemyDatabase.Add("ac9d345575444c9a8d11b799e8719be0");//mimic
            SpecificEnemyDatabase.Add("d8fd592b184b4ac9a3be217bc70912a2");//mimic
            SpecificEnemyDatabase.Add("d8d651e3484f471ba8a2daa4bf535ce6");//mimic
            SpecificEnemyDatabase.Add("2ebf8ef6728648089babb507dec4edb7");//mimic
            SpecificEnemyDatabase.Add("796a7ed4ad804984859088fc91672c7f");//mimic
            

            for (int i=0; i<SpecificEnemyDatabase.Count; i++)
            {
                RemovefromBossRoomDatabase.Add(SpecificEnemyDatabase[i]);
            }

            RemovefromBossRoomDatabase.Add("3e98ccecf7334ff2800188c417e67c15"); //killithid
            RemovefromBossRoomDatabase.Add("45192ff6d6cb43ed8f1a874ab6bef316"); //misfirebeast
            RemovefromBossRoomDatabase.Add("2ccaa1b7ae10457396a1796decda9cf6"); //agunim
            RemovefromBossRoomDatabase.Add("39dca963ae2b4688b016089d926308ab"); //cannon
            RemovefromBossRoomDatabase.Add("0ff278534abb4fbaaa65d3f638003648"); //popcorn
            RemovefromBossRoomDatabase.Add("0d3f7c641557426fbac8596b61c9fb45"); //hollowpoint
            RemovefromBossRoomDatabase.Add("dc3cd41623d447aeba77c77c99598426"); //other worldy terror (marine past)
            

        }

        public static void ToggleDatabases()
        {

            if(ToggableSettings.one == "on") //Normal Enemies as bosses
            {
                UsedBossRoomDatabase = BossRoomAllEnemies;
            }

            else
            {
                UsedBossRoomDatabase = BossOnlyDatabase;

            }

            //remove from boss database
            for (int i=0; i<GRandomEnemyDataBaseHelper.RemovefromBossRoomDatabase.Count; i++)
            {
                UsedBossRoomDatabase.Remove(GRandomEnemyDataBaseHelper.RemovefromBossRoomDatabase[i]);
                BossRoomRegularEnemiesOnly.Remove(GRandomEnemyDataBaseHelper.RemovefromBossRoomDatabase[i]);
            }
            ////////////////////////////////
            

            if (ToggableSettings.two =="on") // Bosses as Normal Enemies
            {
                UsedRegularRoomDatabase = All_Database;

            }
            else
            {
                UsedRegularRoomDatabase = ClearOnlyDatabase_NoBosses;
            }

        }


        public static List<string> All_Database = new List<string>();
        public static List<string> BossOnlyDatabase = new List<string>();
        public static List<string> BossRoomAllEnemies = new List<string>(); //includes bosses
        public static List<string> ClearOnlyDatabase_NoBosses = new List<string>(); // does NOT include bosses
        public static List<string> SpecificEnemyDatabase = new List<string>();

        public static List<string> RemoveEnemyDatabase = new List<string>(); //removes guid from all lists

        public static List<string> UsedBossRoomDatabase = new List<string>();
        public static List<string> UsedRegularRoomDatabase = new List<string>(); 

        public static List<string> RemovefromBossRoomDatabase = new List<string>(); //breaks boss room rewards
        public static List<string> BossRoomRegularEnemiesOnly = new List<string>(); //for enemies spawning in boss room

        public static List<string> HarmlessEnemyDatabase = new List<string>(); // for rat entrance

    }

}


