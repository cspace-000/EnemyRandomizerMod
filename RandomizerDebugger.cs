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


namespace EnemyRandomizer
{
    public class GRandomDebugRoom : BraveBehaviour
    {//used for debugging 
        public static void RoomStats(string[] notused)
        {
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            List<AIActor> activeenemies_typeall = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            List<AIActor> activeenemies_typeroomclear = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);


            ETGModConsole.Log("");
            ETGModConsole.Log(string.Format("Current Room: {0}", currentRoom.GetRoomName()));
            ETGModConsole.Log(string.Format("There are {0} Type All enemies", activeenemies_typeall.Count));
            ETGModConsole.Log(string.Format("There are {0} Type Room Clear enemies", activeenemies_typeroomclear.Count));

            //ETGModConsole.Log("ActorName: {0}, IsVulnerable: {1}, CurrentHealth: {2}, IsHarmlessEnemy: {3}, activeenabled: {4}, PreventAllDamage: {5}, " +
            //    "CanBeKilled: {6}, FlashesOnDamage: {7}, IsNormalEnemy: {8}");

            ETGModConsole.Log(" {0}:Guid, {1}:IsVulnerable, {2}:playerentered, {3}:IsHarmlessEnemy, {4}:activeenabled, {5}:PreventAllDamage, " +
                    "{6}:awoken, {7}:engaged, {8}:IsNormalEnemy");

            for (int e = 0; e < activeenemies_typeall.Count; e++)
            {
                AIActor enemy = activeenemies_typeall[e];
                bool vulnerable = enemy.healthHaver.IsVulnerable;
                float health = enemy.healthHaver.GetCurrentHealth();
                bool isharmlessenemy = enemy.IsHarmlessEnemy;
                bool isbuffenemy = enemy.IsBuffEnemy;
                bool activeenabled = enemy.isActiveAndEnabled;
                bool preventalldamage = enemy.healthHaver.PreventAllDamage;
                bool flash = enemy.healthHaver.flashesOnDamage;
                bool awoken = enemy.HasBeenAwoken;
                bool engaged = enemy.HasBeenEngaged;
                bool playerentered = enemy.HasDonePlayerEnterCheck;


                //ETGModConsole.Log(string.Format("0{0}, 1{1}, 2{2}, 3{3}, 4{4}, 5{5}, 6{6}, 7{7}, 8{8} ", 
                //    enemy.GetActorName(), vulnerable.ToString(), health.ToString(), isharmlessenemy.ToString(), activeenabled.ToString(), preventalldamage.ToString(), 
                //    enemy.healthHaver.CanCurrentlyBeKilled.ToString(), flash.ToString(), enemy.IsNormalEnemy.ToString()));

                GenericIntroDoer genericIntroDoer = enemy.GetComponent<GenericIntroDoer>();


                ETGModConsole.Log(string.Format("0{0}, 1{1}, 2{2}, 3{3}, 4{4}, 5{5}, 6{6}, 7{7}, 8{8} ",
                    enemy.EnemyGuid, vulnerable.ToString(), playerentered.ToString(), isharmlessenemy.ToString(), activeenabled.ToString(), preventalldamage.ToString(),
                    awoken.ToString(), engaged.ToString(), enemy.IsNormalEnemy.ToString()));

                AIActor objectToInstantiate = activeenemies_typeall[e];
                SpeculativeRigidbody custom_component = objectToInstantiate.GetComponent<SpeculativeRigidbody>();

                //if (genericIntroDoer)
                //{
                //    ETGModConsole.Log("Has Generic Intro Doer");
                //}

                //else
                //{
                //    ETGModConsole.Log("has no Gneric Intro Doer");
                //}



                //custom_component.HitboxPixelCollider.IsTrigger = true;
                //custom_component.HitboxPixelCollider.CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.Projectile);

                //custom_component.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));
                //custom_component.RemoveCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));
                //enemy.IsNormalEnemy = true;


                //custom_component.HitboxPixelCollider.CanCollideWith(CollisionLayer.Projectile);
                ////enemy.CollisionDamage()

                //if (custom_component.HasFrameSpecificCollisionExceptions)
                //{
                //    ETGModConsole.Log("HAS Frame Specific Collision Exeptions");
                //}

                //if (custom_component.GhostCollisionExceptions != null)
                //{
                //    List<SpeculativeRigidbody> ghostcollisions = custom_component.GhostCollisionExceptions;
                //    ETGModConsole.Log(string.Format("Number Ghost Collisions: {0}", ghostcollisions.Count.ToString()));
                //}

                //else
                //{
                //    ETGModConsole.Log("No Ghosst Collisions");
                //}

                ////bool hashiteffecthandler = custom_component.hasHit

                //List<PixelCollider> custom_pixels = custom_component.PixelColliders;

                //ETGModConsole.Log(string.Format("Number Pixel Colliders: {0}", custom_pixels.Count.ToString()));
                //for (int g = 0; g < custom_pixels.Count; g++)
                //{
                //    PixelCollider custom_pixel_collider = custom_pixels[g];

                //    //foreach(var x in CollisionLayer)
                //    //{

                //    //}
                //        bool playercollider = custom_pixel_collider.CanCollideWith(CollisionLayer.PlayerHitBox);
                //    bool projectile = custom_pixel_collider.CanCollideWith(CollisionLayer.Projectile);
                //    bool tiles = custom_pixel_collider.CanCollideWith(CollisionLayer.LowObstacle);

                //    ETGModConsole.Log(string.Format("PlayerCollider: {0}, Projectile: {1}, LowObstacle: {2}", playercollider.ToString(), projectile.ToString(), tiles.ToString()));


                //}

                //base.specRigidbody.HitboxPixelCollider.IsTrigger = true;
                //base.specRigidbody.HitboxPixelCollider.CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.Projectile);

                //custom_pixel_collider.ClearFrameSpecificCollisionExceptions();

            }

        }

        public static void RoomStats2(string[] notused)
        {
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            List<AIActor> activeenemies_typeall = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            List<AIActor> activeenemies_typeroomclear = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);


            ETGModConsole.Log("");
            ETGModConsole.Log(string.Format("Current Room: {0}", currentRoom.GetRoomName()));


            //ETGModConsole.Log("ActorName: {0}, IsVulnerable: {1}, CurrentHealth: {2}, IsHarmlessEnemy: {3}, activeenabled: {4}, PreventAllDamage: {5}, " +
            //    "CanBeKilled: {6}, FlashesOnDamage: {7}, IsNormalEnemy: {8}");

            ETGModConsole.Log(" {0}:Guid, {1}:IsVulnerable, {2}:health, {3}:IsHarmlessEnemy, {4}:specialbossdamage, {5}:PreventAllDamage, " +
                    "{6}:awoken, {7}:engaged, {8}:IsNormalEnemy, {9} cancurrentlybekilled");

            for (int e = 0; e < activeenemies_typeall.Count; e++)
            {
                AIActor enemy = activeenemies_typeall[e];
                bool vulnerable = enemy.healthHaver.IsVulnerable;
                float health = enemy.healthHaver.GetCurrentHealth();
                bool isharmlessenemy = enemy.IsHarmlessEnemy;
                bool isbuffenemy = enemy.IsBuffEnemy;
                bool activeenabled = enemy.isActiveAndEnabled;
                bool preventalldamage = enemy.healthHaver.PreventAllDamage;
                bool flash = enemy.healthHaver.flashesOnDamage;
                bool awoken = enemy.HasBeenAwoken;
                bool engaged = enemy.HasBeenEngaged;
                bool playerentered = enemy.HasDonePlayerEnterCheck;

                bool specialbossdamage = enemy.healthHaver.OnlyAllowSpecialBossDamage;

                bool cancurrentlybekilled = enemy.healthHaver.CanCurrentlyBeKilled;
                //ETGModConsole.Log(string.Format("0{0}, 1{1}, 2{2}, 3{3}, 4{4}, 5{5}, 6{6}, 7{7}, 8{8} ", 
                //    enemy.GetActorName(), vulnerable.ToString(), health.ToString(), isharmlessenemy.ToString(), activeenabled.ToString(), preventalldamage.ToString(), 
                //    enemy.healthHaver.CanCurrentlyBeKilled.ToString(), flash.ToString(), enemy.IsNormalEnemy.ToString()));

                GenericIntroDoer genericIntroDoer = enemy.GetComponent<GenericIntroDoer>();


                ETGModConsole.Log(string.Format("0{0}, 1{1}, 2{2}, 3{3}, 4{4}, 5{5}, 6{6}, 7{7}, 8{8}, 9{9} ",
                    enemy.EnemyGuid, vulnerable.ToString(), health.ToString(), isharmlessenemy.ToString(), specialbossdamage.ToString(), preventalldamage.ToString(),
                    awoken.ToString(), engaged.ToString(), enemy.IsNormalEnemy.ToString(), cancurrentlybekilled));


            }

        }



        public static void Off(string[] notused)
        {
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            List<AIActor> activeenemies_typeall = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            for (int e = 0; e < activeenemies_typeall.Count; e++)
            {
                AIActor enemy = activeenemies_typeall[e];

                AIActor objectToInstantiate = activeenemies_typeall[e];
                SpeculativeRigidbody custom_component = objectToInstantiate.GetComponent<SpeculativeRigidbody>();

                if (custom_component != null)
                {
                    ETGModConsole.Log("Turing off SpeculativeRigidBody");
                    custom_component.HitboxPixelCollider.IsTrigger = true;
                    custom_component.HitboxPixelCollider.CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.Projectile);
                    enemy.IsNormalEnemy = false;
                }
            }
        }

        public static void On(string[] notused)
        {
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            List<AIActor> activeenemies_typeall = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            for (int e = 0; e < activeenemies_typeall.Count; e++)
            {
                AIActor enemy = activeenemies_typeall[e];

                AIActor objectToInstantiate = activeenemies_typeall[e];
                SpeculativeRigidbody custom_component = objectToInstantiate.GetComponent<SpeculativeRigidbody>();

                enemy.State = AIActor.ActorState.Normal;
                enemy.HasBeenEngaged = true;

                //if (custom_component != null)
                //{
                //    //ETGModConsole.Log("Turing On SpeculativeRigidBody");
                //    //custom_component.HitboxPixelCollider.IsTrigger = false;
                //    //custom_component.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));
                //    //enemy.IsNormalEnemy = true;


                //    //custom_component.RemoveCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));
                //}
            }
        }


        public static void CollidesWithProjectiles(string[] notused)
        {
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            List<AIActor> activeenemies_typeall = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            for (int e = 0; e < activeenemies_typeall.Count; e++)
            {
                AIActor enemy = activeenemies_typeall[e];

                AIActor objectToInstantiate = activeenemies_typeall[e];
                SpeculativeRigidbody custom_component = objectToInstantiate.GetComponent<SpeculativeRigidbody>();
                ETGModConsole.Log(string.Format("Enemy: {0}, IsValid {1}: ", enemy.ActorName, enemy.IsValid.ToString()));




                //if (custom_component != null)
                //{
                //    ETGModConsole.Log("Has SpeculativeRigidBody");
                //    ETGModConsole.Log(string.Format("Enemy: {0}, Can Collide with others {1}: ", enemy.ActorName, custom_component.CollideWithOthers));

                //    List<PixelCollider> pixelColliders = custom_component.PixelColliders;
                //    for (int i = 0; i < pixelColliders.Count; i++)
                //    {
                //        PixelCollider pixelCollider = pixelColliders[i];
                //        ETGModConsole.Log(string.Format("Enemy: {0}, pixelColliderEnabled {1}: ", enemy.ActorName, pixelCollider.Enabled.ToString()));
                //        if ((CollisionLayerMatrix.GetMask(pixelCollider.CollisionLayer) & CollisionMask.LayerToMask(CollisionLayer.Projectile)) == CollisionMask.LayerToMask(CollisionLayer.Projectile))
                //        {
                //            ETGModConsole.Log("Can be hit by projectiles");
                //        }
                //        else if ((pixelCollider.CollisionLayerCollidableOverride & CollisionMask.LayerToMask(CollisionLayer.Projectile)) == CollisionMask.LayerToMask(CollisionLayer.Projectile))
                //        {
                //            ETGModConsole.Log("Can be hit by projectiles2");
                //        }

                //        else
                //        {
                //            ETGModConsole.Log("Can Not be hit by projectiles");
                //        }
                //    }


                //    //custom_component.RemoveCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile));
                //}
            }
        }





        public static void KillRoom(string[] notused)
        {
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            List<AIActor> activeenemies_typeall = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            for (int e = 0; e < activeenemies_typeall.Count; e++)
            {
                AIActor enemy = activeenemies_typeall[e];
                enemy.EraseFromExistence(false);
            }

        }

        public static void EnemySpawner(string[] enemyguid)
        {
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            currentRoom.AddSpecificEnemyToRoomProcedurally(enemyguid[0]);

        }

        public static void EnemyDatabaseChecker()
        {
            int count = EnemyDatabase.Instance.Entries.Count;
            ETGModConsole.Log(string.Format("There are : {0} in Database", count.ToString()));
            //bool flag = false;
            //bool flag_prefabActor = false;

            Debug.Log("[RANDOMIZER] Start EnemyDatabase");

            for (int i = 0; i < count; i++)
            {
                EnemyDatabaseEntry enemyDatabaseEntry = EnemyDatabase.Instance.Entries[i];

                if (enemyDatabaseEntry != null)
                //if (enemyDatabaseEntry != null && enemyDatabaseEntry.name.Equals(_name[0], StringComparison.OrdinalIgnoreCase))
                {
                    string enemyGuid = enemyDatabaseEntry.myGuid;
                    if (EnemyDatabase.GetOrLoadByGuid(enemyGuid) != null)
                    {
                        AIActor prefabActor = EnemyDatabase.GetOrLoadByGuid(enemyGuid);
                        if (prefabActor.healthHaver != null)
                        {
                            HealthHaver health = prefabActor.healthHaver;
                            Debug.Log(string.Format("{0}: {1} NormalEnemy: {2}, Boss: {3}, SubBoss{4} ", enemyGuid, prefabActor.ActorName, prefabActor.IsNormalEnemy, health.IsBoss, health.IsSubboss));
                        }
                        else
                        {

                            Debug.Log(string.Format("{0}: No HealthHaver Found!!!, {1} NormalEnemy: {2}", enemyGuid, prefabActor.ActorName, prefabActor.IsNormalEnemy));
                        }


                    }
                    else
                    {
                        Debug.Log(string.Format("{0}: No PrefabActorFound!!!", enemyGuid));
                        //flag_prefabActor = true;
                    }

                }
            }
            Debug.Log("[RANDOMIZER] End EnemyDatabase");


        }


        public static void ListEnemyComponents(string[] notused)
        {

            string enemyGuid = "dc3cd41623d447aeba77c77c99598426"; //MarinePast

            enemyGuid = "5d045744405d4438b371eb5ed3e2cdb2"; //Bullet Bishop
            AIActor prefabActor = EnemyDatabase.GetOrLoadByGuid(enemyGuid);


            GameObject objectToInstantiate = prefabActor.gameObject;

            // int flag = 0;

            Debug.Log("Listing Components");




            Component[] components = objectToInstantiate.GetComponents(typeof(Component));
            //for (int i = 0; i < components.Length; i++)
            foreach (Component component in components)
            {
                //Debug.Log(components[i].name);
                Debug.Log(component.ToString());
                //(UnityEngine.Transform)
                //(SpeculativeRigidbody)
                //(AIActor)
                //(AIBulletBank)
                //(HitEffectHandler)
                //(HealthHaver)
                //(KnockbackDoer)
                //(UnityEngine.MeshFilter)
                //(UnityEngine.MeshRenderer)
                //(tk2dSprite)
                //(tk2dSpriteAnimator)
                //(AIAnimator)
                //(ObjectVisibilityManager)
                //(BehaviorSpeculator)
                //(AgunimDeathController)
                //(EncounterTrackable)
                //(AkGameObj)


            }

            Debug.Log("End Listing Components");



        }

        public static void ListDungeonComponents(string[] notused)
        {
            MetalGearRatRoomController[] rooms = UnityEngine.Resources.FindObjectsOfTypeAll<MetalGearRatRoomController>();
            MetalGearRatRoomController metalgearratroom = UnityEngine.Object.FindObjectOfType<MetalGearRatRoomController>();
            GameObject gameobject = metalgearratroom.gameObject;

            if (gameobject)
            {
                string name = gameobject.name;
                ETGModConsole.Log("MetalGearRatRoomControllerAttachedTo: " + name);

            }

            else
            {
                ETGModConsole.Log("No RatRoomObject Found");

            }

            
            //MetalGearRatRoomPlaceable
        }

        public static void ListBullets()
        {
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            List<AIActor> activeenemies_typeall = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            // int flag = 0;

            Debug.Log("Listing Bullets");

            for (int e = 0; e < activeenemies_typeall.Count; e++)
            {
                AIActor enemy = activeenemies_typeall[e];
                GameObject objectToInstantiate = enemy.gameObject;

                //AIShooter aishooter = objectToInstantiate.GetComponent<AIShooter>();

                //if (aishooter)
                //{
                //    Debug.Log("Has aishooter");
                //}

                //else
                //{
                //    Debug.Log("No ai shooter");

                //}
                AIBulletBank bulletbank = objectToInstantiate.GetComponent<AIBulletBank>();


                List<AIBulletBank.Entry> bullets = bulletbank.Bullets;

                foreach (AIBulletBank.Entry bullet in bullets)
                {
                    //Debug.Log(bullet.Name);
                    //Debug.Log(bullet.ToString());

                    if (bullet != null)
                    {
                        if (bullet.Name == "reflect")
                        {
                            Debug.Log("Reflecting!");
                            GameObject gameObject = bullet.BulletObject;
                            Projectile component = gameObject.GetComponent<Projectile>();
                            SpeculativeRigidbody specRigidBody = component.specRigidbody;

                            //specRigidBody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)System.Delegate.Combine(specRigidBody.OnPreRigidbodyCollision,
                            //    new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(GRandomAgunimProjectile.Instance.OnPreRigidbodyCollision));

                            Debug.Log("Reflected Fuck you");
                        }

                    }

                    else
                    {
                        Debug.Log("not reflected!");
                    }
                }

                // AIBulletBank.Entry bullet = enemy.aiShooter.GetBulletEntry("reflect");


            }
            Debug.Log("End Listing Bullets");



        }

        public static void BraveBehaviorDebug()
        {
            string enemyGuid = "dc3cd41623d447aeba77c77c99598426"; //MarinePast
            AIActor prefabActor = EnemyDatabase.GetOrLoadByGuid(enemyGuid);


            GameObject objectToInstantiate = prefabActor.gameObject;

            // int flag = 0;

            Debug.Log("Begin BraveBehavior Debug");


            BehaviorSpeculator behaviorspeculator = objectToInstantiate.GetComponent<BehaviorSpeculator>();
            AIBulletBank aibulletbank = objectToInstantiate.GetComponent<AIBulletBank>();

            List<AttackBehaviorBase> attackbehaviorbase = behaviorspeculator.AttackBehaviors;

            foreach (AttackBehaviorBase attack in attackbehaviorbase)
            {
                Debug.Log("ATTACK Behaviors: " + attack.ToString());
            }

            AttackBehaviorGroup attackbehaviorgroup = behaviorspeculator.AttackBehaviorGroup;
            List<AttackBehaviorGroup.AttackGroupItem> attackbehaviors = attackbehaviorgroup.AttackBehaviors;

            foreach (AttackBehaviorGroup.AttackGroupItem attackgroupitem in attackbehaviors)
            {
                Debug.Log("AttackGroup Item :" + attackgroupitem.Behavior.ToString());
            }
            //AttackBehaviorGroup
            //AttackGruop //ShootBehavior --> has bullet scripts

            Debug.Log("End BraveBehavior Debug");

        }

        public static void DragunNearDeath(string[] notused)
        {

            //string enemyGuid = "dc3cd41623d447aeba77c77c99598426"; //MarinePast

            //AIActor prefabActor = EnemyDatabase.GetOrLoadByGuid(enemyGuid);


            //GameObject objectToInstantiate = prefabActor.gameObject;


            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            List<AIActor> activeenemies_typeall = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            for (int e = 0; e < activeenemies_typeall.Count; e++)
            {
                AIActor enemy = activeenemies_typeall[e];

                AIActor objectToInstantiate = activeenemies_typeall[e];
                //SpeculativeRigidbody custom_component = objectToInstantiate.GetComponent<SpeculativeRigidbody>();

                DraGunController draguncontroller = objectToInstantiate.GetComponent<DraGunController>();
                ETGModConsole.Log("DraGunController!");
                if (draguncontroller)
                {
                    ETGModConsole.Log("");
                    ETGModConsole.Log("Is near death: " + draguncontroller.IsNearDeath);
                    ETGModConsole.Log("Is near transitioning: " + draguncontroller.IsTransitioning);
                    ETGModConsole.Log("Is near activeandenabled: " + draguncontroller.isActiveAndEnabled);
                    ETGModConsole.Log("Is near HasdoneIntro: " + draguncontroller.HasDoneIntro);
                    ETGModConsole.Log("Is near HasConvertedBaby: " + draguncontroller.HasConvertedBaby);
                    
                }

                else
                {
                    ETGModConsole.Log("No DraGunController");
                }


                //BehaviorSpeculator behaviorspeculator = objectToInstantiate.GetComponent<BehaviorSpeculator>();
                //List<AttackBehaviorBase> attackbehaviorbase = behaviorspeculator.AttackBehaviors;

                //foreach (AttackBehaviorBase attack in attackbehaviorbase)
                //{
                //    if (attack is DraGunNearDeathBehavior)
                //    {
                //        ETGModConsole.Log("DraGunNearDeathBehavior");

                //        //attackbehaviorbase.Remove(attack);
                //        //Debug.Log("Removed BossFinalMarinePortalBehavior");
                //    }

                //    else
                //    {
                //        //Debug.Log("NO BossFinalMarinePortalBehavior");
                //    }
                //    //Debug.Log("ATTACK Behaviors: " + attack.ToString());
                //}







                Debug.Log("End Listing Components");



            }
        }

        public static void Get(string[] notused)
        {
            //Debug.Log("Platforms!");
            //MovingPlatform[] movingplatforms = UnityEngine.GameObject.FindObjectsOfType<MovingPlatform>();


            //foreach(MovingPlatform platform in movingplatforms)
            //{
            //    string name = platform.name;
            //    Debug.Log("platform.name: " + name);
            //}


            //DungeonPlaceableBehaviour[] mylist = UnityEngine.GameObject.FindObjectsOfType<DungeonPlaceableBehaviour>();
            //foreach (DungeonPlaceableBehaviour d in mylist)
            //{
            //    Debug.Log("DUNGEON " + d.name);
            //    ETGModConsole.Log("Dungeon " + d.name);

            //    if (d is DraGunRoomPlaceable)
            //    {
            //        GameObject g = d.gameObject;

            //        ETGModConsole.Log("GameObject " + g.name);

            //    }


            //}

         PrototypeDungeonRoom[] prototypedungeonrooms= UnityEngine.GameObject.FindObjectsOfType<PrototypeDungeonRoom>();

            Dungeon d = GameManager.Instance.Dungeon;
            int count = d.data.rooms.Count;

            ETGModConsole.Log("There are rooms:" + count);
            ETGModConsole.Log("There are prototyperooms: " + prototypedungeonrooms.Length.ToString());

            for (int i = 0; i<prototypedungeonrooms.Length; i++)
            {
                PrototypeDungeonRoom room = prototypedungeonrooms[i];
                if (room.name == "DraGunRoom01")
                {
                    ETGModConsole.Log("Found DraGunRoom01");
                    
                }
            }
            //foreach (RoomHandler roomHandler in d.data.rooms)
            for (int i = 0; i < count; i++)
            {
                RoomHandler roomHandler = d.data.rooms[i];
                string roomname = roomHandler.GetRoomName();
                ETGModConsole.Log("roomname: " + roomname);
                //if (roomname == "DraGunRoom01")
                //{
                //    ETGModConsole.Log("Found DraGunRoom01");
                //    foreach (Component component in roomHandler.GameObject.GetComponents(typeof(Component)))
                //    {

                //    }


                //    //DraGunRoomPlaceable emberDoer = roomHandler.GetComponentsAbsoluteInRoom<DraGunRoomPlaceable>()[0];




                //    //if (emberDoer)
                //    //{
                //    //    ETGModConsole.Log("Found DraGunRoomPlaceable");
                //    //}
                //    //else
                //    //{
                //    //    ETGModConsole.Log("No DraGunRoomPlaceable");
                //    //}


                //}
                //ETGModConsole.Log(roomname);
                //Debug.Log(roomname);
            }

        }

        public static void TestingRooms()
        {
            AssetBundle sharedAssets = ResourceManager.LoadAssetBundle("shared_auto_001");
            PrototypeDungeonRoom DraGunRoom01 = sharedAssets.LoadAsset<PrototypeDungeonRoom>("dragunroom01");

            if (DraGunRoom01!=null)
            {
                ETGModConsole.Log("Loading DragunRoom asset");
                
                List<PrototypePlacedObjectData> placedObjects = DraGunRoom01.placedObjects;
                ETGModConsole.Log("Counting objects " + placedObjects.Count);

                foreach (PrototypePlacedObjectData placedObject in placedObjects)
                {
                    ETGModConsole.Log("Placed asset " + placedObject.enemyBehaviourGuid);
                    if (placedObject.nonenemyBehaviour != null)
                    {
                        Debug.Log(placedObject.nonenemyBehaviour.name);
                        ETGModConsole.Log(placedObject.nonenemyBehaviour.name);
                    }



                }

            }


        }
            
        public static void TestingAssets()
        {
            AssetBundle sharedAssets = ResourceManager.LoadAssetBundle("shared_auto_001");
            string[] sharedAssetNames = sharedAssets.GetAllAssetNames();


            AssetBundle braveResources = ResourceManager.LoadAssetBundle("brave_resources_001");
            string[] braveResourcesNames = sharedAssets.GetAllAssetNames();

            AssetBundle base_forge = ResourceManager.LoadAssetBundle("base_forge");
            string[] base_forge_names = sharedAssets.GetAllAssetNames();

            Debug.Log("Asset Reader here");

            foreach (string name in sharedAssetNames)
            {
                Debug.Log("SharedAssets: " + name);

            }

            foreach (string name in braveResourcesNames)
            {
                Debug.Log("braveResourcesNames: " + name);
            }

            foreach (string name in base_forge_names)
            {
                Debug.Log("base_forge_names: " + name);
            }

            Debug.Log("End Asset Reader here");
        }

        public static void gettingAssetTypes()
        {
            AssetBundle sharedAssets = ResourceManager.LoadAssetBundle("shared_auto_001");
            //Object[] fuckle = sharedAssets.LoadAllAssets();

            GameObject fuckle = sharedAssets.LoadAsset<GameObject>("dragunroomplaceable");


            //foreach (Component component in fuckle.GetComponents(typeof (Component)))
            //    {
            //    Debug.Log("Fuckle: " + component.name);
            //    MovingPlatform movingplatform = component.GetComponentInChildren<MovingPlatform>();



            //}
            //ETGModConsole.Log("fuckle" + fuckle.GetType().ToString());

            GameObject dragunroomplaceable = sharedAssets.LoadAsset<GameObject>("dragunroomplaceable");
            if (dragunroomplaceable)
            {
                MovingPlatform movingplatform = dragunroomplaceable.GetComponentInChildren<MovingPlatform>();
                if (movingplatform)
                {
                    Debug.Log("Fuckle Moving Platform");
                }
                else
                {
                    Debug.Log("Fuckle No Moving Platform");
                }

            }

            else
            {
                Debug.Log("no dragunroomplaceable");
            }


            //dragunroomplaceable.prefab
        }

        public static void GetRooms(string[] notused)
        {
            try
            {
                RoomHandler roomHandler;
                Dungeon dungeon;


                //Debug.Log("hooked into Loadnextlevel hook");
                if (GameManager.Instance.CurrentlyGeneratingDungeonPrefab != null)
                {
                    dungeon = GameManager.Instance.BestGenerationDungeonPrefab;
                }
                else
                {
                    dungeon = GameManager.Instance.Dungeon;
                }

                if (dungeon != null)
                {
                    int count = dungeon.data.rooms.Count;
                    Debug.Log("found + Rooms: " + count);
                    for (int i = 0; i < count; i++)
                    {
                        roomHandler = dungeon.data.rooms[i];
                        if (roomHandler != null)
                        {
                            string roomname = roomHandler.GetRoomName();
                            ETGModConsole.Log(roomname);
                            Debug.Log(roomname);

                        }

                        else
                        {
                            Debug.Log("No Room?");
                        }

                    }

                }

                else
                {
                    Debug.Log("No Dungeon");
                }

            }

            catch (Exception exception)
            {
                Debug.Log("GetRooms Exception " + exception);
            }

        }

        public static void GetSpecificRooms(string[] notused)
        {
            ETGModConsole.Log("ss_resourcefulrat");
            ETGModConsole.Log("MetalGearRatRoom01");
            ETGModConsole.Log("DraGunRoom01");
        }


        public static void GetPlayerVector2(string[] notused)
        {
            PlayerController p = GameManager.Instance.PrimaryPlayer;
            ETGModConsole.Log(p.CenterPosition.ToString());

        }


        public static void GetSceneNames(string[] notused)
        {
            GameLevelDefinition gameLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();


            //GameLevelDefinition gameLevelDefinition = new GameLevelDefinition();
            //gameLevelDefinition.dungeonSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            ETGModConsole.Log("SceneName " + gameLevelDefinition.dungeonSceneName);

        }

        public static void GetAiAnimator(string[] notused)
        {

        }


        public static void CompareProjectileVelocity_HealthHaverDirection(string[] notused)
        {
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            List<AIActor> activeenemies_typeall = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            // int flag = 0;

            Debug.Log("CompareProjectileVelocity_HealthHaverDirection");
            ETGModConsole.Log("");
            for (int e = 0; e < activeenemies_typeall.Count; e++)
            {
                AIActor enemy = activeenemies_typeall[e];
                GameObject objectToInstantiate = enemy.gameObject;
                //AIBulletBank bulletbank = objectToInstantiate.GetComponent<AIBulletBank>();

                Vector2 lastIncurredDamageDirection = enemy.specRigidbody.healthHaver.lastIncurredDamageDirection;
                Vector2 lastIncurredDamageDirection2 = enemy.healthHaver.lastIncurredDamageDirection;
                //Vector2 projectiledirection =
                //List<AIBulletBank.Entry> bullets = bulletbank.Bullets;



                HealthHaver healthhaver = enemy.healthHaver;
                bool isdead = enemy.healthHaver.IsDead;

                ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
                ETGModConsole.Log("Active Projectiles: " + allProjectiles.Count);
                for (int i = allProjectiles.Count - 1; i >= 0; i--)
                {

                    Projectile projectile = allProjectiles[i];
                    if (projectile)
                    {
                        if (projectile.Owner is PlayerController)
                        {
                            Vector2 velocity = projectile.specRigidbody.Velocity;
                            ETGModConsole.Log("Velocity" + velocity.ToString());

                        }
                    }
                }


                ETGModConsole.Log(string.Format("{3}: {4}: IsDead:{0},  LIDD1:{1},  LIDD2:{2}", isdead, lastIncurredDamageDirection.ToString(), lastIncurredDamageDirection2.ToString(), e, enemy.EnemyGuid));





            }
            Debug.Log("End Listing Bullets");


        }


        public static void _ReturnToFoyer(string[] notused)
        {
            GameManager.Instance.DelayedReturnToFoyer(3f);
        }

        public static void DebugBulletStoppingError(string[] notused)
        {
            RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
            List<AIActor> activeenemies_typeall = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            // int flag = 0;

            Debug.Log("DebugBulletStoppingError");
            ETGModConsole.Log("");
            for (int e = 0; e < activeenemies_typeall.Count; e++)
            {
                AIActor enemy = activeenemies_typeall[e];
                GameObject objectToInstantiate = enemy.gameObject;
                //AIBulletBank bulletbank = objectToInstantiate.GetComponent<AIBulletBank>();

                Vector2 lastIncurredDamageDirection = enemy.specRigidbody.healthHaver.lastIncurredDamageDirection;
                Vector2 lastIncurredDamageDirection2 = enemy.healthHaver.lastIncurredDamageDirection;
                //Vector2 projectiledirection =
                //List<AIBulletBank.Entry> bullets = bulletbank.Bullets;



                HealthHaver healthhaver = enemy.healthHaver;

                AIAnimator animator = objectToInstantiate.GetComponent<AIAnimator>();
                bool animatorbool = false;

                tk2dSpriteAnimator sprite_animator = objectToInstantiate.GetComponent<tk2dSpriteAnimator>();
                bool sprite_animatorbool = false;


                DisplacedImageController displacedimagecontroller = objectToInstantiate.GetComponent<DisplacedImageController>();
                bool displacedimagecontrollerbool = false;

                tk2dSprite tk2dsprite = objectToInstantiate.GetComponent<tk2dSprite>();
                bool tk2dspritebool = false;


                if (animator != null)
                {
                    animatorbool = true;
                }


                if (sprite_animator!=null)
                {
                    sprite_animatorbool = true;
                }

                if (displacedimagecontroller != null)
                {
                    displacedimagecontrollerbool = true;
                }

                if(tk2dsprite!=null)
                {
                    tk2dspritebool = true;
                }


                bool isdead = enemy.healthHaver.IsDead;


                ETGModConsole.Log(string.Format("{0}, {1}, IsDead:{2},  LIDD1:{3}, animator:{4}, spriteanimator:{5}, DisplacedImageController{6}, tk2dsprite{7}",
                   e, enemy.EnemyGuid, isdead, lastIncurredDamageDirection.ToString(), animatorbool, sprite_animatorbool, displacedimagecontrollerbool, tk2dspritebool));




            }

        }

        public static void kill2(string[] notused)
        {
            RandomHandleEnemyInfo.DestroyEverything();
        }

       public static void getBossHealth(string[] notused)
        {
            List<string> bosses = GRandomEnemyDataBaseHelper.BossOnlyDatabase;

            for (int i =0; i<bosses.Count; i++)
            {
                string enemyGuid = bosses[i];
                AIActor prefabActor = EnemyDatabase.GetOrLoadByGuid(enemyGuid);

                ETGModConsole.Log(String.Format("name:{0} health:{1}" ,prefabActor.ActorName, prefabActor.healthHaver.GetMaxHealth()));
                ETGModConsole.Log(GRandomHook.boss_health.ToString());



            }


        }

       public static void getBossRoomDatabase(string[] notused)
       {
            List<string> database = GRandomEnemyDataBaseHelper.UsedBossRoomDatabase;
            ETGModConsole.Log("Boss database: " + database.Count);
            for (int i=0; i< database.Count; i++)
            {
                string enemyGuid = database[i];
                AIActor prefabActor = EnemyDatabase.GetOrLoadByGuid(enemyGuid);
                ETGModConsole.Log(enemyGuid + ": " + prefabActor.ActorName);
            }


       }
        //endclass
    }


    public class GRandomTeleport
    {
        public static void TeleportToSpecificRoom(string[] args)
        {
            //(this.dungeonFloors[i].dungeonSceneName == ss_ratscene)
            string roomname = args[0];
            
            //ETGModConsole.Log("ARGS 0 == " + roomname);
            bool flag = false;

            RoomHandler roomHandler;
            Dungeon d = GameManager.Instance.Dungeon;
            int count = d.data.rooms.Count;
            for (int i = 0; i<count; i++)
            //foreach (RoomHandler roomHandler in d.data.rooms)
            {
                roomHandler = d.data.rooms[i];
                if (roomname == "boss")
                {

                    if (roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS &&
                            roomHandler.area.PrototypeRoomBossSubcategory == PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS |
                            GRandomRoomDatabaseHelper.AllSpecificDeathRooms.Contains(roomHandler.GetRoomName()))
                    {

                        {
                            roomname = roomHandler.GetRoomName();
                            flag = true;
                            ETGModConsole.Log("Teleporting to: " + roomname);
                            Tele(roomHandler);
                            break;
                        }


                    }
                }
                else
                {
                    if (roomHandler.GetRoomName() == roomname)
                    {
                        flag = true;
                        ETGModConsole.Log("Teleporting to: " + roomname);
                        Tele(roomHandler);

                    }
                }

                
            }

            if (!flag)
            {
                ETGModConsole.Log(roomname + " Not Found");
            }

            


        }


        public static void Tele(RoomHandler room)
        {
            IntVector2 Epicenter = room.Epicenter;

            Vector2 vEpicenter = Epicenter.ToVector2();
            if (room.GetRoomName() == "DraGunRoom01")
            {
                vEpicenter -= new Vector2(0.0f, 5.0f);
            }



            PlayerController primaryPlayer = Gungeon.Game.PrimaryPlayer;
            primaryPlayer.TeleportToPoint(vEpicenter, true);
            //"MetalGearRatRoom01"

        }



    }


    public class GRandomSceneTele
    {
        public static void TeleSceneHandler(string[] args)
        {
            //(this.dungeonFloors[i].dungeonSceneName == ss_ratscene)
            string scenename = args[0];

            ETGModConsole.Log("ARGS 0 == " + scenename);
            bool flag = false;



            TeleportToSpecificScene(scenename);
        }


        public static void TeleportToSpecificScene(string scenename)
        {
            //(this.dungeonFloors[i].dungeonSceneName == ss_ratscene)
            
            GameManager.Instance.LoadCustomLevel(scenename);


        }



    }
}
