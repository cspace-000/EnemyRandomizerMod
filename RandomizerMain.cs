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
using System.Collections.ObjectModel;
using UnityEngine.SceneManagement;
//To Do
// Give enemy companions health
// Fix broken rooms
// Fix broken bosses

namespace EnemyRandomizer
{
    public class GRandomMain : ETGModule
    {
        public static bool GRDebug = true;

        public override void Init()
        {

        }

        public override void Start()
        {

            ETGModConsole.Log("[EnemyRandomizer] Running");
            try
            {     
                GRandomRoomDatabaseHelper.Start();
                GRandomEnemyDataBaseHelper.Start();

                Hook enemyhook = new Hook(typeof(DungeonPlaceableUtility).GetMethod("InstantiateDungeonPlaceable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static),
                    typeof(GRandomHook).GetMethod("MainHook", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static));

                Hook DraGunRoomStartHook = new Hook(typeof(DraGunRoomPlaceable).GetMethod("Start", BindingFlags.Instance | BindingFlags.Public),
                    typeof(GRandomHook).GetMethod("DraGunRoomStartHook", BindingFlags.Instance | BindingFlags.Public), typeof(DraGunRoomPlaceable));

                Hook DraGunRoomUpdateHook = new Hook(typeof(DraGunRoomPlaceable).GetMethod("Update", BindingFlags.Instance | BindingFlags.Public),
                    typeof(GRandomHook).GetMethod("DraGunRoomUpdateHook", BindingFlags.Instance | BindingFlags.Public), typeof(DraGunRoomPlaceable));

                Hook DemonWallMovementBehaviorHook = new Hook(typeof(DemonWallMovementBehavior).GetMethod("Update", BindingFlags.Instance | BindingFlags.Public),
                    typeof(GRandomHook).GetMethod("DemonWallMovementBehaviorHook", BindingFlags.Instance | BindingFlags.Public), typeof(DemonWallMovementBehavior));

                Hook HealthHaver_ApplyDamageHook = new Hook(typeof(HealthHaver).GetMethod("ApplyDamage", BindingFlags.Instance | BindingFlags.Public),
                    typeof(BraveHook).GetMethod("HealthHaver_ApplyDamageHook", BindingFlags.Instance | BindingFlags.Public), typeof(HealthHaver));

                Hook orig_StartHook = new Hook(typeof(PlayerController).GetMethod("orig_Start", BindingFlags.Instance | BindingFlags.Public),
                typeof(GRandomHook).GetMethod("orig_StartHook", BindingFlags.Instance | BindingFlags.Public), typeof(PlayerController));

                Hook LateUpdateHook = new Hook(typeof(ResourcefulRatMinesHiddenTrapdoor).GetMethod("LateUpdate", BindingFlags.Instance | BindingFlags.NonPublic),
                    typeof(GRandomHook).GetMethod("LateUpdateHook", BindingFlags.Instance | BindingFlags.Public), typeof(ResourcefulRatMinesHiddenTrapdoor));

                ToggableSettings.StartingSettings();
                ToggableSettings.DisplayStats();

                ETGModConsole.Commands.AddGroup("jank", new Action<string[]>(GRandomDebugRoom.kill2));

                ETGModConsole.Commands.AddGroup("randhelp", new Action<string[]>(ToggableSettings.Help));
                ETGModConsole.Commands.AddGroup("randmodes", new Action<string[]>(ToggableSettings.GetStats));
                ETGModConsole.Commands.AddGroup("rand", new Action<string[]>(ToggableSettings.ConsoleLineHandler));

            }
            catch (Exception exception)
            {
                Console.WriteLine("[EnemyRandomizer] Error occured while installing hooks");
                Debug.LogException(exception);
            }

            if (GRDebug)
            {

            GRandomDebugRoom.EnemyDatabaseChecker();
            ETGModConsole.Log("[Random] In DEBUG Mode");
            GRandomDebugRoom.TestingRooms();
            GRandomDebugRoom.TestingAssets();
            GRandomDebugRoom.gettingAssetTypes();
            ETGModConsole.Commands.AddGroup("randomremove", new Action<string[]>(GRandomEnemyDataBaseHelper.DropEnemy));
            ETGModConsole.Commands.AddGroup("room", new Action<string[]>(GRandomDebugRoom.RoomStats));
            ETGModConsole.Commands.AddGroup("kill", new Action<string[]>(GRandomDebugRoom.KillRoom));
            ETGModConsole.Commands.AddGroup("randen", new Action<string[]>(GRandomDebugRoom.EnemySpawner));
            ETGModConsole.Commands.AddGroup("on", new Action<string[]>(GRandomDebugRoom.On));
            ETGModConsole.Commands.AddGroup("off", new Action<string[]>(GRandomDebugRoom.Off));
            ETGModConsole.Commands.AddGroup("collide", new Action<string[]>(GRandomDebugRoom.CollidesWithProjectiles));
            ETGModConsole.Commands.AddGroup("components", new Action<string[]>(GRandomDebugRoom.ListEnemyComponents));

            ETGModConsole.Commands.AddGroup("dungeon", new Action<string[]>(GRandomDebugRoom.ListDungeonComponents));

            ETGModConsole.Commands.AddGroup("room2", new Action<string[]>(GRandomDebugRoom.RoomStats2));
            ETGModConsole.Commands.AddGroup("dragun", new Action<string[]>(GRandomDebugRoom.DragunNearDeath));


            ETGModConsole.Commands.AddGroup("tele", new Action<string[]>(GRandomTeleport.TeleportToSpecificRoom));


            ETGModConsole.Commands.AddGroup("get", new Action<string[]>(GRandomDebugRoom.Get)); //for getting all room names

            ETGModConsole.Commands.AddGroup("getrooms", new Action<string[]>(GRandomDebugRoom.GetRooms));
            ETGModConsole.Commands.AddGroup("specificrooms", new Action<string[]>(GRandomDebugRoom.GetSpecificRooms));

            ETGModConsole.Commands.AddGroup("vector", new Action<string[]>(GRandomDebugRoom.GetPlayerVector2));

            ETGModConsole.Commands.AddGroup("getscene", new Action<string[]>(GRandomDebugRoom.GetSceneNames));
            ETGModConsole.Commands.AddGroup("telescene", new Action<string[]>(GRandomSceneTele.TeleSceneHandler));



            ETGModConsole.Commands.AddGroup("compareprojectile", new Action<string[]>(GRandomDebugRoom.CompareProjectileVelocity_HealthHaverDirection));

            ETGModConsole.Commands.AddGroup("returntofoyer", new Action<string[]>(GRandomDebugRoom._ReturnToFoyer));
            ETGModConsole.Commands.AddGroup("bulletstoppingerror", new Action<string[]>(GRandomDebugRoom.DebugBulletStoppingError));
            ETGModConsole.Commands.AddGroup("bosshealth", new Action<string[]>(GRandomDebugRoom.getBossHealth));
            ETGModConsole.Commands.AddGroup("bossroom", new Action<string[]>(GRandomDebugRoom.getBossRoomDatabase));


            }

        }



        public override void Exit()
        {

        }        
    }



    public class GRandomHook : MonoBehaviour
    {
        public static GameObject MainHook(GameObject objectToInstantiate, RoomHandler targetRoom,
            IntVector2 location, bool deferConfiguration, AIActor.AwakenAnimationType awakenAnimType = AIActor.AwakenAnimationType.Default, bool autoEngage = false)
        {   //hooks into InstantiateDungeonPlaceable
            
            GameObject result;
            System.Random rnd = new System.Random();
            int nu = 0; 
            string enemyGuid;
            GRandomHook.wasBoss = false;
            bool isbossroom = false;


            if (targetRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS &&
                targetRoom.area.PrototypeRoomBossSubcategory == PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS)
            {
                isbossroom = true;
            }

            else if (GRandomRoomDatabaseHelper.AllSpecificDeathRooms.Contains(targetRoom.GetRoomName()))
            {
                isbossroom = true;
            }


                try
            {
                if (objectToInstantiate != null)
                {

                    Vector3 vector = location.ToVector3(0f) + targetRoom.area.basePosition.ToVector3();
                    vector.z = vector.y + vector.z;
                    AIActor component = objectToInstantiate.GetComponent<AIActor>(); //notused
                    AIActor ogcomponent = component;


                    if (component is AIActorDummy)
                    {
                        objectToInstantiate = (component as AIActorDummy).realPrefab;
                        component = objectToInstantiate.GetComponent<AIActor>(); 
                    }

                    SpeculativeRigidbody component2 = objectToInstantiate.GetComponent<SpeculativeRigidbody>(); //notused
                    if (component && component2)
                    {
                        
                        if (component.EnemyGuid != null)
                        {
                            //Here gets enemyGuid based on room. Pulls from EnemyDatabase   ///////////////////////
                            if (isbossroom)
                            {                              
                                if (component.healthHaver.IsBoss)
                                {
                                    GRandomHook.wasBoss = true;
                                    if (component.healthHaver.GetMaxHealth() !=60 ) //sometimes gets health as regular enemy health, 60
                                    {                                        
                                        GRandomHook.boss_health = component.healthHaver.GetMaxHealth();
                                        //getting boss health to set for replacement boss
                                    }

                                    //replacement for Boss
                                    nu = rnd.Next(0, GRandomEnemyDataBaseHelper.UsedBossRoomDatabase.Count);
                                    enemyGuid = GRandomEnemyDataBaseHelper.UsedBossRoomDatabase[nu];

                                }


                                else
                                { //normal enemies as bosses is off and the enemy is not a boss; pull from no bosses database for enemy spawnings
                                    nu = rnd.Next(0, GRandomEnemyDataBaseHelper.BossRoomRegularEnemiesOnly.Count);
                                    enemyGuid = GRandomEnemyDataBaseHelper.BossRoomRegularEnemiesOnly[nu];

                                }                             
                            }

                            else if(targetRoom.GetRoomName() == "ResourcefulRat_PitEntrance_01" | targetRoom.GetRoomName() == "ResourcefulRat_Entrance")
                            {
                                nu = rnd.Next(0, GRandomEnemyDataBaseHelper.HarmlessEnemyDatabase.Count);
                                enemyGuid = GRandomEnemyDataBaseHelper.HarmlessEnemyDatabase[nu];
                            }

                            else
                            {
                                nu = rnd.Next(0, GRandomEnemyDataBaseHelper.UsedRegularRoomDatabase.Count);
                                enemyGuid = GRandomEnemyDataBaseHelper.UsedRegularRoomDatabase[nu];
                            }


                            if (component.EnemyGuid == "479556d05c7c44f3b6abb3b2067fc778") //wallmimic
                            {
                                enemyGuid = "479556d05c7c44f3b6abb3b2067fc778";
                            }

                            //
                            //can add specific Guid here for debugging
                            //


                            if (enemyGuid == "465da2bb086a4a88a803f79fe3a27677") //replace DraGun, can't remove him from database or forge dragunroom breaks
                            {
                                enemyGuid = "05b8afe0b6cc4fffa9dc6036fa24c8ec";
                            }

                            // End getting guid //////////////////////////////

                            //initializing new AIActor, not sure why they do it again below 
                            AIActor prefabActor = EnemyDatabase.GetOrLoadByGuid(enemyGuid);


                            objectToInstantiate = prefabActor.gameObject;
                            component = objectToInstantiate.GetComponent<AIActor>();
                            component2 = objectToInstantiate.GetComponent<SpeculativeRigidbody>();
                            //bool specificdeathdoer = prefabActor.healthHaver.ManualDeathHandling;



                            GenericIntroDoer genericIntroDoer = component.GetComponent<GenericIntroDoer>();

                            //if (genericIntroDoer) // is boss
                            // handles initiated boss settings
                            if (component.healthHaver.IsBoss)
                            {                               
                                if (isbossroom)
                                    {
                                        prefabActor.healthHaver.SetHealthMaximum(GRandomHook.boss_health);
                                        ETGModConsole.Log("Newbosshealth " + prefabActor.healthHaver.GetMaxHealth());
                                }
                                else
                                {
                                    prefabActor.healthHaver.SetHealthMaximum(60f);
                                }

                                objectToInstantiate = RandomHandleEnemyInfo.RemoveBossIntros(objectToInstantiate);
                                objectToInstantiate = RandomHandleEnemyInfo.ReplaceSpecificBossDeathController(objectToInstantiate);
                                objectToInstantiate = RandomHandleEnemyInfo.AttackBehaviorManipulator(objectToInstantiate);

                                DemonWallController dwc = objectToInstantiate.GetComponent<DemonWallController>();
                                if (dwc)
                                {
                                    Destroy(dwc);
                                }


                            }

                            if (!component.IsNormalEnemy) 
                            {
                                objectToInstantiate = RandomHandleEnemyInfo.HandleCompanions(objectToInstantiate);
                            }



                        }


                        PixelCollider pixelCollider = component2.GetPixelCollider(ColliderType.Ground);
                        if (pixelCollider.ColliderGenerationMode != PixelCollider.PixelColliderGeneration.Manual)
                        {
                            Debug.LogErrorFormat("Trying to spawn an AIActor who doesn't have a manual ground collider... do we still do this? Name: {0}", new object[]
                            {
                    objectToInstantiate.name
                            });
                        }
                        Vector2 a = PhysicsEngine.PixelToUnit(new IntVector2(pixelCollider.ManualOffsetX, pixelCollider.ManualOffsetY));
                        Vector2 vector2 = PhysicsEngine.PixelToUnit(new IntVector2(pixelCollider.ManualWidth, pixelCollider.ManualHeight));
                        Vector2 vector3 = new Vector2((float)Mathf.CeilToInt(vector2.x), (float)Mathf.CeilToInt(vector2.y));
                        Vector2 b = new Vector2((vector3.x - vector2.x) / 2f, 0f).Quantize(0.0625f);



                        if (targetRoom.GetRoomName() == "DraGunRoom01" | targetRoom.GetRoomName() == "LichRoom02" | 
                            targetRoom.GetRoomName() == "LichRoom03" | targetRoom.GetRoomName() == "Bullet_End_Room_04" |
                            targetRoom.GetRoomName() == "ResourcefulRatRoom01")
                        {
                            b -= new Vector2(0.0f, 5.0f);
                        }

                        Vector3 v3 = a - b;
                        vector -= v3;
                        //vector -= a - b; //Vector3

                    }

                    if (component)
                    {
                        component.AwakenAnimType = awakenAnimType;
                    }


                    GameObject NewEnemyObject = UnityEngine.Object.Instantiate<GameObject>(objectToInstantiate, vector, Quaternion.identity);


                    if (!deferConfiguration)
                    {
                        Component[] componentsInChildren = NewEnemyObject.GetComponentsInChildren(typeof(IPlaceConfigurable));
                        for (int i = 0; i < componentsInChildren.Length; i++)
                        {
                            IPlaceConfigurable placeConfigurable = componentsInChildren[i] as IPlaceConfigurable;
                            if (placeConfigurable != null)
                            {
                                placeConfigurable.ConfigureOnPlacement(targetRoom);
                            }
                        }
                    }
                    ObjectVisibilityManager component3 = NewEnemyObject.GetComponent<ObjectVisibilityManager>();
                    if (component3 != null)
                    {
                       
                        component3.Initialize(targetRoom, autoEngage);
                    }
                    MinorBreakable componentInChildren = NewEnemyObject.GetComponentInChildren<MinorBreakable>();
                    if (componentInChildren != null)
                    {
                        IntVector2 key = location + targetRoom.area.basePosition;
                        CellData cellData = GameManager.Instance.Dungeon.data[key];
                        if (cellData != null)
                        {
                            cellData.cellVisualData.containsObjectSpaceStamp = true;
                        }
                    }
                    PlayerItem component4 = NewEnemyObject.GetComponent<PlayerItem>();
                    if (component4 != null)
                    {
                        component4.ForceAsExtant = true;
                    }


                    //[Randomizer] Add AIActor GameObjectInfo
                    AIActor enemy_component = NewEnemyObject.GetComponent<AIActor>();
                    if (enemy_component)
                    {
                        if (enemy_component.healthHaver.IsBoss) 
                        {
                            if (isbossroom)

                            { //Boss Room
                            enemy_component.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;
                            }
                            else
                            {
                                enemy_component.healthHaver.bossHealthBar = HealthHaver.BossBarType.None;
                                autoEngage = true;
                            }
                            NewEnemyObject = RandomHandleEnemyInfo.ReinstateBossObjectInfo(NewEnemyObject); //removes boss status if regular boss, needs hitbox stuff reinstated

                        }

         
                        if (GRandomEnemyDataBaseHelper.SpecificEnemyDatabase.Contains(enemy_component.EnemyGuid))
                        {
                            NewEnemyObject = RandomHandleEnemyInfo.SpecificEnemyHelper(NewEnemyObject);
                        }

                        NewEnemyObject = UniqueBossRoomDeathHandler.SpecificRoomHandler(targetRoom, NewEnemyObject);

                    }


                    result = NewEnemyObject;
                    
                }

                else
                {
                    result = null;
                    //return null;
                }

            }

            catch (Exception message)
            {
                Debug.Log("[RANDOMIZER ERROR] "  + message.ToString());
               

                result = null;                  

            }

            return result;
            

        }

        public static float boss_health;
        public static bool wasBoss;

        public IEnumerator DraGunRoomStartHook(DraGunRoomPlaceable self)
        {
            //AssetBundle sharedAssets = ResourceManager.LoadAssetBundle("shared_auto_001");
            //GameObject dragunroomplaceable = sharedAssets.LoadAsset<GameObject>("dragunroomplaceable");
            //MovingPlatform deathBridge = dragunroomplaceable.GetComponentInChildren<MovingPlatform>();

            
            ////Debug.Log("DraGunRoomStartHook");
            //FieldInfo field = typeof(DraGunRoomPlaceable).GetField("m_deathBridge", BindingFlags.Instance | BindingFlags.NonPublic);
            //field.SetValue(self, deathBridge);

            yield return null;
            yield break;
        }
        public void DraGunRoomUpdateHook(DraGunRoomPlaceable self)
        {
            //Debug.Log("DraGunRoomUpdateHook");
        }
        public BehaviorResult DemonWallMovementBehaviorHook(Action<DemonWallMovementBehavior> orig, DemonWallMovementBehavior self)
        {
            //Debug.Log("DemonWallMovementBehavior Hook");
            return BehaviorResult.Continue;
        }//used
        public void orig_StartHook(Action<PlayerController>orig, PlayerController self)
        {   //give player random gun and weapon when randomizer settings are active
            orig(self);
            if (ToggableSettings.three == "on")
            {
                RandomHandlePlayerInfo.PlayerGunsnItem();
            }
            
        }
        public void LateUpdateHook(Action<ResourcefulRatMinesHiddenTrapdoor>orig, ResourcefulRatMinesHiddenTrapdoor self)
        {// so you can actually find the trap door
            orig(self);
            if (self.RevealPercentage<1f)
            {
                self.RevealPercentage = 1f;
            }
        }
        
        //endclass
    }

    public class BraveHook : BraveBehaviour
    {

        public void HealthHaver_ApplyDamageHook(
            HealthHaver self, float damage, Vector2 direction, string sourceName, CoreDamageTypes damageTypes = CoreDamageTypes.None, DamageCategory damageCategory = DamageCategory.Normal,
            bool ignoreInvulnerabilityFrames = false, PixelCollider hitPixelCollider = null, bool ignoreDamageCaps = false)
        {//Killithid's ghosts spawn without correct hitboxes and crash game on hit, this method is a bandaid for that. Kills all enemies and projectiles in room

            try
            {
                self.orig_ApplyDamage(damage, direction, sourceName, damageTypes, damageCategory, ignoreInvulnerabilityFrames, hitPixelCollider, ignoreDamageCaps);
                if (0f < self.GetCurrentHealth() && 0f < damage && ETGModGUI.UseDamageIndicators)
                {
                    ETGDamageIndicatorGUI.HealthHaverTookDamage(self, damage);
                }

            }

            catch (Exception e)
            {
                Debug.Log("[Randomizer] HealthHaverHook Error" + e);
                RandomHandleEnemyInfo.DestroyEverything();
                throw;
                
            }
            

            



        }



    }

}
