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


namespace EnemyRandomizer
{
    

    public class RandomHandleEnemyInfo : MonoBehaviour
    {// this is the main class that handles the manipulation of all AIActor components
        public static GameObject HandleCompanions(GameObject enemyobject)
        {
            AIActor component = enemyobject.GetComponent<AIActor>();
            component.IsNormalEnemy = true;
            component.healthHaver.SetHealthMaximum(15f);

            UnityEngine.Object.Destroy(component.GetComponent<CompanionController>());            
            return enemyobject;

        }

        public static GameObject RemoveBossIntros(GameObject bossobject)
        {   //removes boss generic and specific boss intros
            GenericIntroDoer genericIntroDoer = bossobject.GetComponent<GenericIntroDoer>();
            CustomEngageDoer customengage = bossobject.GetComponent<CustomEngageDoer>();
            GatlingGullIntroDoer ggintro = bossobject.GetComponent<GatlingGullIntroDoer>();

            if (genericIntroDoer)
            {
                SpecificIntroDoer specificintrodoer = bossobject.GetComponent<SpecificIntroDoer>();
                //MetalGearRatDeathController deathcontroller2 = bossobject.GetComponent<MetalGearRatDeathController>();


                if (specificintrodoer)
                {

                    Destroy(specificintrodoer);
                    specificintrodoer.RegenerateCache();


                }

                genericIntroDoer.InvisibleBeforeIntroAnim = false;
                Destroy(genericIntroDoer);
                genericIntroDoer.RegenerateCache();
                Destroy(customengage);
            }

            if (ggintro) //gatling gull
            {
                Destroy(ggintro);
            }

            return bossobject;


        }

        public static GameObject ReplaceSpecificBossDeathController(GameObject bossobject)
        {// method replaces original boss death controller with custom death controllers to remove triggering events
            
            AIActor component = bossobject.GetComponent<AIActor>();
            string enemyguid = component.EnemyGuid;

            if (enemyguid == "4d164ba3f62648809a4a82c90fc22cae") //metalgear ratboss
            {
                bossobject.AddComponent<GRandomMetalGearRatDeathController>();
                GRandomMetalGearRatDeathController new_deathcontroller = bossobject.GetComponent<GRandomMetalGearRatDeathController>();
                MetalGearRatDeathController deathcontroller = bossobject.GetComponent<MetalGearRatDeathController>();

                new_deathcontroller.explosionVfx = deathcontroller.explosionVfx;
                new_deathcontroller.explosionMidDelay = deathcontroller.explosionMidDelay;
                new_deathcontroller.explosionCount = deathcontroller.explosionCount;

                Destroy(deathcontroller);

            }
            if (enemyguid == "05b8afe0b6cc4fffa9dc6036fa24c8ec") //AdvancedDraGun
            {
                
                bossobject.AddComponent<GRandomAdvancedDraGunDeathController2>();
                GRandomAdvancedDraGunDeathController2 new_deathcontroller = bossobject.GetComponent<GRandomAdvancedDraGunDeathController2>();
                AdvancedDraGunDeathController deathcontroller = bossobject.GetComponent<AdvancedDraGunDeathController>();


                new_deathcontroller.fingerDebris = deathcontroller.fingerDebris;
                new_deathcontroller.neckDebris = deathcontroller.neckDebris;
                Destroy(deathcontroller);

            }
            if (enemyguid == "2ccaa1b7ae10457396a1796decda9cf6") //Agunim
            {
                AgunimDeathController deathcontroller = bossobject.GetComponent<AgunimDeathController>();
                Destroy(deathcontroller);
                bossobject.AddComponent<GRandomAgunimDeathController>();

            }
            if (enemyguid == "465da2bb086a4a88a803f79fe3a27677") //DraGun
            {
                bossobject.AddComponent<GRandomDraGunDeathController>();
                GRandomDraGunDeathController new_deathcontroller = bossobject.GetComponent<GRandomDraGunDeathController>();
                DraGunDeathController deathcontroller = bossobject.GetComponent<DraGunDeathController>();

                new_deathcontroller.explosionVfx = deathcontroller.explosionVfx;
                new_deathcontroller.explosionMidDelay = deathcontroller.explosionMidDelay;
                new_deathcontroller.explosionCount = deathcontroller.explosionCount;

                new_deathcontroller.skullDebris = deathcontroller.skullDebris;
                new_deathcontroller.fingerDebris = deathcontroller.fingerDebris;
                new_deathcontroller.neckDebris = deathcontroller.neckDebris;


                Destroy(deathcontroller);
            }
            if (enemyguid == "39dca963ae2b4688b016089d926308ab") //Gunon(BulletPast)
            {
                bossobject.AddComponent<GRandomGunonDeathController>();
                GRandomGunonDeathController new_deathcontroller = bossobject.GetComponent<GRandomGunonDeathController>();
                GunonDeathController deathcontroller = bossobject.GetComponent<GunonDeathController>();

                new_deathcontroller.explosionVfx = deathcontroller.explosionVfx;
                new_deathcontroller.explosionMidDelay = deathcontroller.explosionMidDelay;
                new_deathcontroller.explosionCount = deathcontroller.explosionCount;


                Destroy(deathcontroller);

            }
            if (enemyguid == "880bbe4ce1014740ba6b4e2ea521e49d") //Last Human
            {
                BossFinalRobotDeathController deathcontroller = bossobject.GetComponent<BossFinalRobotDeathController>();
                Destroy(deathcontroller);
                bossobject.AddComponent<GRandomBossFinalRobotDeathController>();
            }
            if (enemyguid == "b98b10fca77d469e80fb45f3c5badec5") //Hyperion (Rogue Past)
            {
                bossobject.AddComponent<GRandomBossFinalRogueDeathController>();
                BossFinalRogueDeathController deathcontroller = bossobject.GetComponent<BossFinalRogueDeathController>();                               
                GRandomBossFinalRogueDeathController new_deathcontroller = bossobject.GetComponent<GRandomBossFinalRogueDeathController>();

                new_deathcontroller.explosionVfx = deathcontroller.explosionVfx;
                new_deathcontroller.explosionMidDelay = deathcontroller.explosionMidDelay;
                new_deathcontroller.explosionCount = deathcontroller.explosionCount;
                new_deathcontroller.bigExplosionVfx = deathcontroller.bigExplosionVfx;
                new_deathcontroller.bigExplosionCount = deathcontroller.bigExplosionCount;
                new_deathcontroller.bigExplosionMidDelay = deathcontroller.bigExplosionMidDelay;
                new_deathcontroller.DeathStarExplosionVFX = deathcontroller.DeathStarExplosionVFX;

                Destroy(deathcontroller);
            }
            if (enemyguid == "8b913eea3d174184be1af362d441910d") //Convict Past Boss
            {
                bossobject.AddComponent<GRandomBossFinalConvictDeathController>();
                BossFinalConvictDeathController deathcontroller = bossobject.GetComponent<BossFinalConvictDeathController>();
                GRandomBossFinalConvictDeathController new_deathcontroller = bossobject.GetComponent<GRandomBossFinalConvictDeathController>();
                Destroy(deathcontroller);


            }
            if (enemyguid == "8d441ad4e9924d91b6070d5b3438d066") //Hunter Past
            {
                bossobject.AddComponent<GRandomBossFinalGuideDeathController>(); 
                BossFinalGuideDeathController deathcontroller = bossobject.GetComponent<BossFinalGuideDeathController>();
                GRandomBossFinalGuideDeathController new_deathcontroller = bossobject.GetComponent<GRandomBossFinalGuideDeathController>();

                Destroy(deathcontroller);
            }
            if (enemyguid == "dc3cd41623d447aeba77c77c99598426") //InterdimensionalHorror
            {
                bossobject.AddComponent<GRandomBossFinalMarineDeathController>();
                GRandomBossFinalMarineDeathController new_deathcontroller = bossobject.GetComponent<GRandomBossFinalMarineDeathController>();
                BossFinalMarineDeathController deathcontroller = bossobject.GetComponent<BossFinalMarineDeathController>();

                new_deathcontroller.explosionVfx = deathcontroller.explosionVfx;
                new_deathcontroller.explosionMidDelay = deathcontroller.explosionMidDelay;
                new_deathcontroller.explosionCount = deathcontroller.explosionCount;

                new_deathcontroller.bigExplosionVfx = deathcontroller.bigExplosionVfx;
                new_deathcontroller.bigExplosionMidDelay = deathcontroller.bigExplosionMidDelay;
                new_deathcontroller.bigExplosionCount = deathcontroller.bigExplosionCount;

                Destroy(deathcontroller);

            }
            if (enemyguid == "cd88c3ce60c442e9aa5b3904d31652bc") //Lich
            {
                bossobject.AddComponent<GRandomLichDeathController>();
                GRandomLichDeathController new_deathcontroller = bossobject.GetComponent<GRandomLichDeathController>();
                LichDeathController deathcontroller = bossobject.GetComponent<LichDeathController>();
                Destroy(deathcontroller);

            }
            if (enemyguid == "68a238ed6a82467ea85474c595c49c6e") //MegaLich
            {
                bossobject.AddComponent<GRandomMegalichDeathController>();
                GRandomMegalichDeathController new_deathcontroller = bossobject.GetComponent<GRandomMegalichDeathController>();
                MegalichDeathController deathcontroller = bossobject.GetComponent<MegalichDeathController>();

                new_deathcontroller.explosionVfx = deathcontroller.explosionVfx;
                new_deathcontroller.shellCasing = deathcontroller.shellCasing;
                Destroy(deathcontroller);

            }
            if (enemyguid == "7c5d5f09911e49b78ae644d2b50ff3bf") //InfiLich
            {

                bossobject.AddComponent<GRandomInfinilichDeathController>();
                GRandomInfinilichDeathController new_deathcontroller = bossobject.GetComponent<GRandomInfinilichDeathController>();
                InfinilichDeathController deathcontroller = bossobject.GetComponent<InfinilichDeathController>();
                if (deathcontroller)
                {
                    new_deathcontroller.bigExplosionVfx = deathcontroller.bigExplosionVfx;
                    new_deathcontroller.finalExplosionVfx = deathcontroller.finalExplosionVfx;
                    new_deathcontroller.eyePos = deathcontroller.eyePos;


                    Destroy(deathcontroller);
                }



            }
            if (enemyguid == "8817ab9de885424d9ba83455ead5ffef") //BunkerBoss
            {
                bossobject.AddComponent<GRandomBunkerBossDeathController>();
                GRandomBunkerBossDeathController new_deathcontroller = bossobject.GetComponent<GRandomBunkerBossDeathController>();
                BunkerBossDeathController deathcontroller = bossobject.GetComponent<BunkerBossDeathController>();

                new_deathcontroller.explosionVfx = deathcontroller.explosionVfx;
                new_deathcontroller.explosionMidDelay = deathcontroller.explosionMidDelay;
                new_deathcontroller.explosionCount = deathcontroller.explosionCount;
                new_deathcontroller.debrisObjects = deathcontroller.debrisObjects;
                new_deathcontroller.debrisMidDelay = deathcontroller.debrisMidDelay;
                new_deathcontroller.debrisCount = deathcontroller.debrisCount;
                new_deathcontroller.debrisMinForce = deathcontroller.debrisMinForce;
                new_deathcontroller.debrisMaxForce = deathcontroller.debrisMaxForce;
                new_deathcontroller.debrisAngleVariance = deathcontroller.debrisAngleVariance;
                new_deathcontroller.deathAnimation = deathcontroller.deathAnimation;

                new_deathcontroller.deathAnimationDelay = deathcontroller.deathAnimationDelay;
                new_deathcontroller.dustVfx = deathcontroller.dustVfx;

                new_deathcontroller.dustTime = deathcontroller.dustTime;
                new_deathcontroller.dustMidDelay = deathcontroller.dustMidDelay;
                new_deathcontroller.dustOffset = deathcontroller.dustOffset;
                new_deathcontroller.dustDimensions = deathcontroller.dustDimensions;

                new_deathcontroller.shakeMidDelay = deathcontroller.shakeMidDelay;
                new_deathcontroller.flagAnimation = deathcontroller.flagAnimation;


                Destroy(deathcontroller);

            }
            if (enemyguid == "6868795625bd46f3ae3e4377adce288b") //ResourcefulRat
            {
                bossobject.AddComponent<GRandomResourcefulRatDeathController>();
                GRandomResourcefulRatDeathController new_deathcontroller = bossobject.GetComponent<GRandomResourcefulRatDeathController>();
                ResourcefulRatDeathController deathcontroller = bossobject.GetComponent<ResourcefulRatDeathController>();
                Destroy(deathcontroller);

            }
            if (enemyguid == "9189f46c47564ed588b9108965f975c9") //BossDoorMicmicDeathController
            {
                bossobject.AddComponent<GRandomBossDoorMimicDeathController>();
                GRandomBossDoorMimicDeathController new_deathcontroller = bossobject.GetComponent<GRandomBossDoorMimicDeathController>();
                BossDoorMimicDeathController deathcontroller = bossobject.GetComponent<BossDoorMimicDeathController>();
                Destroy(deathcontroller);

            }


            if (enemyguid == "f3b04a067a65492f8b279130323b41f0") //wallmonger
            {
                bossobject.AddComponent<GRandomDemonWallDeathController>();
                GRandomDemonWallDeathController new_deathcontroller = bossobject.GetComponent<GRandomDemonWallDeathController>();
                DemonWallDeathController deathcontroller = bossobject.GetComponent<DemonWallDeathController>();

                new_deathcontroller.deathEyes = deathcontroller.deathEyes;
                new_deathcontroller.deathOil = deathcontroller.deathOil;

                Destroy(deathcontroller);

            }

            
            return bossobject;
        }

        public static GameObject ReinstateBossObjectInfo(GameObject bossobject)
        {   //After removing boss intro, need to reinstate behavior to have it active, awake and have specbody.
            //otherwise they stand there and don't take damage
            //this method also handles specific bosses

            bossobject = ReinstateAIActorObjectInfo(bossobject);
            AIActor bosscomponent = bossobject.GetComponent<AIActor>();
            if (bosscomponent)
            {
                if (bosscomponent.EnemyGuid == "05b8afe0b6cc4fffa9dc6036fa24c8ec" | bosscomponent.EnemyGuid == "465da2bb086a4a88a803f79fe3a27677" 
                    |bosscomponent.EnemyGuid == "880bbe4ce1014740ba6b4e2ea521e49d" | bosscomponent.EnemyGuid == "b98b10fca77d469e80fb45f3c5badec5"
                    |bosscomponent.EnemyGuid == "8b913eea3d174184be1af362d441910d" | bosscomponent.EnemyGuid == "8d441ad4e9924d91b6070d5b3438d066"
                    | bosscomponent.EnemyGuid == "dc3cd41623d447aeba77c77c99598426")
                    // advancedDragun | dragun | Last Human | Hyperion | Baldy | Frankenstein | InterdimensionalHorror
                {//need this to be able to shoot boss
                    ObjectVisibilityManager objectvisibilitymanager = bosscomponent.GetComponent<ObjectVisibilityManager>();
                    objectvisibilitymanager.SuppressPlayerEnteredRoom = false;
                }             
                
                if (bosscomponent.EnemyGuid == "2ccaa1b7ae10457396a1796decda9cf6") // agunim
                {
                    bosscomponent.healthHaver.OnlyAllowSpecialBossDamage = false;
                }

            }

            return bossobject;
        }

        public static GameObject AttackBehaviorManipulator(GameObject bossobject)
        {// for removing interdimensional horror teleporting move. Sometimes would teleport out of room and not teleport back in
            AIActor component = bossobject.GetComponent<AIActor>();
            string enemyguid = component.EnemyGuid;

            if (enemyguid == "dc3cd41623d447aeba77c77c99598426") //InterdimensionalHorror
                {//Removing Teleport out of room move            
                    BehaviorSpeculator behaviorspeculator = bossobject.GetComponent<BehaviorSpeculator>();
                List<AttackBehaviorBase> attackbehaviorbase = behaviorspeculator.AttackBehaviors;

                int count = attackbehaviorbase.Count();
                
                //foreach (AttackBehaviorBase attack in attackbehaviorbase)
                for (int i =0; i<count; i++)
                {
                    
                    if (attackbehaviorbase[i] is BossFinalMarinePortalBehavior)
                    {
                        manipulatedAttack = attackbehaviorbase[i];
                        break;
                    }

                    else
                    {
                        //do nothing
                    }

                }

                if (manipulatedAttack is BossFinalMarinePortalBehavior)
                {
                    attackbehaviorbase.Remove(manipulatedAttack);
                }

                manipulatedAttack = null;
            }

            return bossobject;
        }

        public static GameObject SpecificEnemyHelper(GameObject enemyobject)
        {//handles enemies that require specific conditions to work

            AIActor component = enemyobject.GetComponent<AIActor>();
            string enemyguid = component.EnemyGuid;
                
                if (enemyguid == "5d045744405d4438b371eb5ed3e2cdb2" | enemyguid == "ce2d2a0dced0444fb751b262ec6af08a" | enemyguid == "e456b66ed3664a4cb590eab3a8ff3814") 
                //bishop | Dr Wolf | baby good mimic
                {
                    enemyobject.AddComponent<KillOnRoomClear>();
                //component.healthHaver.persistsOnDeath = false;
                }

            component.IgnoreForRoomClear = true;

            if (enemyguid == "640238ba85dd4e94b3d6f68888e6ecb8") //|enemyguid == "e456b66ed3664a4cb590eab3a8ff3814")// robot cop | babygoodmimic doesn't work with baby good mimic, just removing from database
            {
                component.healthHaver.PreventAllDamage = false;
            }

            if (enemyguid == "5fa8c86a65234b538cd022f726af2aea")// bulletman
            {
                component.healthHaver.SetHealthMaximum(10f);
            }

            if (enemyguid == "6450d20137994881aff0ddd13e3d40c8" | enemyguid == "ac9d345575444c9a8d11b799e8719be0" |
                enemyguid == "abfb454340294a0992f4173d6e5898a8" | enemyguid == "d8fd592b184b4ac9a3be217bc70912a2" |
                enemyguid == "d8d651e3484f471ba8a2daa4bf535ce6" | enemyguid == "2ebf8ef6728648089babb507dec4edb7")
            {// mimics
                //nerfing health, they are ridiculous
                component.healthHaver.SetHealthMaximum(component.healthHaver.GetMaxHealth() * 0.7f);
            }

            return enemyobject;
        }


        public static GameObject CreateNewBossRoomAIActor(GameObject gameObject)
        {//turn regular enemy into boss
            AIActor bosscomponent = gameObject.GetComponent<AIActor>();

                if (bosscomponent.healthHaver.bossHealthBar == HealthHaver.BossBarType.None)
                {
                    bosscomponent.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;


                    gameObject.AddComponent<GenericIntroDoer>();

                    GenericIntroDoer genericIntroDoer = gameObject.GetComponent<GenericIntroDoer>();
                    genericIntroDoer.InvisibleBeforeIntroAnim = false;

                    genericIntroDoer.triggerType = GenericIntroDoer.TriggerType.PlayerEnteredRoom;

                    gameObject.AddComponent<SpecificIntroDoer>();
                    genericIntroDoer.RegenerateCache();

                    gameObject = ReinstateAIActorObjectInfo(gameObject);
                }
            

            return gameObject;

        }

        public static GameObject ReinstateAIActorObjectInfo(GameObject gameObject)
        {//reinstate boss info, otherwise bosses stand and can't take damage
            AIActor bosscomponent = gameObject.GetComponent<AIActor>();
            
            if (bosscomponent)
            {
                bosscomponent.behaviorSpeculator.enabled = true;
                bosscomponent.specRigidbody.enabled = true;

                bosscomponent.ToggleRenderers(true);

                //component.invisibleUntilAwaken = false;
                bosscomponent.State = AIActor.ActorState.Normal;
                bosscomponent.specRigidbody.CollideWithOthers = true;
 
                bosscomponent.IsGone = false;

                if (bosscomponent.aiShooter)
                {
                    bosscomponent.aiShooter.ToggleGunAndHandRenderers(true, "genericIntro");
                }

                bosscomponent.invisibleUntilAwaken = false;
                bosscomponent.CanTargetPlayers = true;
                bosscomponent.IsNormalEnemy = true;
            }

            else
            {
                Debug.Log("No AiActor component found: " + gameObject.name);
            }
           

            return gameObject;
        }

        public static void DestroyEverything()
        {

            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            List<AIActor> activeenemies_typeall = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);

            //ETGModConsole.Log("");
            for (int e = 0; e < activeenemies_typeall.Count; e++)
            {
                AIActor enemy = activeenemies_typeall[e];
                GameObject objectToInstantiate = enemy.gameObject;
                UnityEngine.Object.Destroy(objectToInstantiate);
                //ETGModConsole.Log(string.Format("{3}: {4}: IsDead:{0},  LIDD1:{1},  LIDD2:{2}", isdead, lastIncurredDamageDirection.ToString(), lastIncurredDamageDirection2.ToString(), e, enemy.EnemyGuid));

            }

            ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
            //ETGModConsole.Log("Active Projectiles: " + allProjectiles.Count);
            for (int i = allProjectiles.Count - 1; i >= 0; i--)
            {
                Projectile projectile = allProjectiles[i];
                if (projectile)
                {
                    GameObject projectileObject = projectile.gameObject;
                    UnityEngine.Object.Destroy(projectileObject);
                }
            }

        }

        public static AttackBehaviorBase manipulatedAttack;

    }

    public class RandomHandlePlayerInfo
    {
        public static void PlayerGunsnItem()
        {//used for giving player random gun and item. Initialized in hook
            PlayerController player = GameManager.Instance.PrimaryPlayer;
            bool flag = player == null;
            if (flag)
            {
                ETGModConsole.Log("<color=#FF0000FF>Please select a character and enter the gungeon.</color>", false);
            }
            else
            {
                Gun[] guns = UnityEngine.Resources.FindObjectsOfTypeAll<Gun>();

                PassiveItem[] passiveitems = UnityEngine.Resources.FindObjectsOfTypeAll<PassiveItem>();
                PlayerItem[] playeritems = UnityEngine.Resources.FindObjectsOfTypeAll<PlayerItem>();

                System.Random rnd = new System.Random();
                //ETGModConsole.Log(String.Format("Length guns {0}, passive {1}, active{2}", guns.Length, passiveitems.Length, playeritems.Length));

                int guns_count = rnd.Next(0, guns.Length);
                int passiveitems_count = rnd.Next(0, passiveitems.Length);

                Gun gun = guns[guns_count];
                PassiveItem passiveitem = passiveitems[passiveitems_count];

                //ETGModConsole.Log(gun.gunName);

                PickupObject pickupgun = PickupObjectDatabase.Instance.InternalGetById(gun.PickupObjectId);
                PickupObject pickuppassive = PickupObjectDatabase.Instance.InternalGetById(passiveitem.PickupObjectId);

                LootEngine.TryGivePrefabToPlayer(pickupgun.gameObject, Game.PrimaryPlayer, false);
                LootEngine.TryGivePrefabToPlayer(pickuppassive.gameObject, Game.PrimaryPlayer, false);

            }

        }
    }


}
