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

//bullet_that_can_kill_the_past
namespace EnemyRandomizer
{
    public class UniqueBossRoomDeathHandler : MonoBehaviour
    {
        public static GameObject SpecificRoomHandler(RoomHandler room, GameObject gameObject)
        {//attaches specificroomdeathcontroller to new AIActor
            string roomname = room.GetRoomName();

            bool isbossroom = false;


            if (room.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS &&
                room.area.PrototypeRoomBossSubcategory == PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS)
            {
                isbossroom = true;
            }

            else if (GRandomRoomDatabaseHelper.AllSpecificDeathRooms.Contains(room.GetRoomName()))
            {
                isbossroom = true;
            }

            if (isbossroom)
            { //Boss Room

                if (ToggableSettings.one == "off" && !GRandomHook.wasBoss)// Normal Enemies as bosses
                {
                }
                else
                {
                    gameObject = RandomHandleEnemyInfo.CreateNewBossRoomAIActor(gameObject);
                }


                    
            }


            if (GRandomRoomDatabaseHelper.PastRooms.Contains(roomname))
            {// Is past Room and Last Lich Room
                //handles player beating game
                gameObject = RandomHandleEnemyInfo.CreateNewBossRoomAIActor(gameObject);
                gameObject.AddComponent<GenericDeathCreditsController>();
            }

            if (roomname== "Bullet_End_Room_03")
            {
                gameObject.AddComponent<Bullet_End_Room_03DeathController>();
            }

            if (roomname == "DraGunRoom01")
            {
                gameObject.AddComponent<ReplacementDraGunDeathController>();
            }

            if (roomname == "ResourcefulRatRoom01")
            {
                gameObject.AddComponent<ResourcefulRatRoom01DeathController>();
            }


            if (roomname == "MetalGearRatRoom01")
            {
                gameObject.AddComponent<MetalGearRatRoom01DeathController>();
            }



            if (roomname == "LichRoom01")
            {
                gameObject.AddComponent<LichRoom01DeathController>();
            }

            if (roomname == "LichRoom02")
            {
                gameObject.AddComponent<LichRoom02DeathController>();
            }



            return gameObject;

        }

        public static void Tele(RoomHandler room)
        {//used for resourceful rat fight
            IntVector2 Epicenter = room.Epicenter;

            Vector2 vEpicenter = Epicenter.ToVector2();

            vEpicenter -= new Vector2(0.0f, 10.0f);
            PlayerController primaryPlayer = Gungeon.Game.PrimaryPlayer;
            primaryPlayer.TeleportToPoint(vEpicenter, true);
            //"MetalGearRatRoom01"
        }

        public static void TeleportToSpecificRoom(string roomname)
        {
            //(this.dungeonFloors[i].dungeonSceneName == ss_ratscene)
            bool flag = false;

            RoomHandler roomHandler;
            Dungeon d = GameManager.Instance.Dungeon;
            int count = d.data.rooms.Count;
            for (int i = 0; i < count; i++)
            {
                roomHandler = d.data.rooms[i];
                if (roomHandler.GetRoomName() == roomname)
                {
                    flag = true;
                    //ETGModConsole.Log("Teleporting to: " + roomname);
                    Tele(roomHandler);
                }
            }

            if (!flag)
            {
                ETGModConsole.Log(roomname + " Not Found");
            }


        }

        public static void TeleportToEnd(string roomname)
        {//creates procedural teleporter on boss death
            RoomHandler roomHandler;
            Dungeon d = GameManager.Instance.Dungeon;
            int count = d.data.rooms.Count;
            for (int i = 0; i < count; i++)

            {
                roomHandler = d.data.rooms[i];
                if (roomHandler.GetRoomName() == roomname)
                {
                    roomHandler.OverrideVisibility = RoomHandler.VisibilityStatus.VISITED;
                    roomHandler.AddProceduralTeleporterToRoom();

                }
            }

        }



    }
}



//public enum ValidTilesets
//{
//	// Token: 0x040046A2 RID: 18082
//	GUNGEON = 1,
//	// Token: 0x040046A3 RID: 18083
//	CASTLEGEON = 2,
//	// Token: 0x040046A4 RID: 18084
//	SEWERGEON = 4,
//	// Token: 0x040046A5 RID: 18085
//	CATHEDRALGEON = 8,
//	// Token: 0x040046A6 RID: 18086
//	MINEGEON = 16,
//	// Token: 0x040046A7 RID: 18087
//	CATACOMBGEON = 32,
//	// Token: 0x040046A8 RID: 18088
//	FORGEGEON = 64,
//	// Token: 0x040046A9 RID: 18089
//	HELLGEON = 128,
//	// Token: 0x040046AA RID: 18090
//	SPACEGEON = 256,
//	// Token: 0x040046AB RID: 18091
//	PHOBOSGEON = 512,
//	// Token: 0x040046AC RID: 18092
//	WESTGEON = 1024,
//	// Token: 0x040046AD RID: 18093
//	OFFICEGEON = 2048,
//	// Token: 0x040046AE RID: 18094
//	BELLYGEON = 4096,
//	// Token: 0x040046AF RID: 18095
//	JUNGLEGEON = 8192,
//	// Token: 0x040046B0 RID: 18096
//	FINALGEON = 16384,
//	// Token: 0x040046B1 RID: 18097
//	RATGEON = 32768
//}

