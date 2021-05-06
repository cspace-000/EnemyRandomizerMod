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
{//this class adds teleporting components during boss death in these specific rooms to allow for player progression.
	public class ReplacementDraGunDeathController : BraveBehaviour
	{
		// Token: 0x06005A4B RID: 23115 RVA: 0x0021D328 File Offset: 0x0021B528
		public ReplacementDraGunDeathController()
		{
			//this.explosionMidDelay = 0.3f;
			//this.explosionCount = 10;

		}

		// Token: 0x06005A4C RID: 23116 RVA: 0x0021D343 File Offset: 0x0021B543
		public void Awake()
		{
			//this.m_dragunController = base.GetComponent<DraGunController>();
			//this.m_deathDummy = base.transform.Find("DeathDummy").GetComponent<tk2dSpriteAnimator>();
			AssetBundle sharedAssets = ResourceManager.LoadAssetBundle("shared_auto_001");
			GameObject dragunroomplaceable = sharedAssets.LoadAsset<GameObject>("dragunroomplaceable");
			//MovingPlatform deathBridge = dragunroomplaceable.GetComponentInChildren<MovingPlatform>();

			this.m_deathBridge = dragunroomplaceable.GetComponentInChildren<MovingPlatform>();
			
		}

		// Token: 0x06005A4D RID: 23117 RVA: 0x0021D36C File Offset: 0x0021B56C
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			base.healthHaver.OverrideKillCamTime = new float?(1.25f);


			
		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x0021D3A6 File Offset: 0x0021B5A6
		private void OnBossDeath(Vector2 dir)
		{
			base.behaviorSpeculator.enabled = false;
			GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());
			AkSoundEngine.PostEvent("Play_BOSS_dragun_thunder_01", base.gameObject);

		}



		// Token: 0x06005A52 RID: 23122 RVA: 0x0021D434 File Offset: 0x0021B634
		private IEnumerator OnDeathExplosionsCR()
		{
            //for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
            //{
            //    GameManager.Instance.AllPlayers[i].SetInputOverride("DraGunDeathController");
            //}
            //GameManager.Instance.PreventPausing = true;
            //GameUIRoot.Instance.HideCoreUI("dragun");
            //GameUIRoot.Instance.ToggleLowerPanels(false, false, "dragun");
            //this.healthHaver.OverrideKillCamPos = new Vector2?(this.specRigidbody.UnitCenter + new Vector2(0f, 6f));
            //this.aiAnimator.PlayUntilCancelled("heart_burst", false, null, -1f, false);
            //while (this.aiAnimator.IsPlaying("heart_burst"))
            //{
            //	yield return null;
            //}
            //this.aiAnimator.EndAnimationIf("heart_burst");
            //this.aiAnimator.PlayVfx("heart_burst", null, null, null);
            //Pixelator.Instance.FadeToColor(0.75f, Color.white, true, 0f);
            yield return new WaitForSeconds(0.3f);
			//GameManager.Instance.PreventPausing = true;
			////this.aiActor.ToggleRenderers(true);
			////this.m_deathDummy.gameObject.SetActive(true);
			////this.m_deathDummy.GetComponent<Renderer>().enabled = true;
			//////this.m_dragunController.IsTransitioning = true;
			////this.m_deathDummy.Play("die");
			//////this.StartCoroutine(this.LerpCrackEmission(1f, 250f, 3f));
			//yield return new WaitForSeconds(3f);
			//GameManager.Instance.PreventPausing = true;
			//////this.StartCoroutine(this.LerpCrackColor(Color.white, 3f));
			//////this.StartCoroutine(this.LerpCrackEmission(250f, 50000f, 3f));
			//yield return new WaitForSeconds(1.5f);
			//Pixelator.Instance.FadeToColor(0.5f, Color.white, false, 0f);
			//yield return new WaitForSeconds(0.75f);
			//////this.m_dragunController.ModifyCamera(false);
			//////this.m_dragunController.BlockPitTiles(false);
			//yield return new WaitForSeconds(0.75f);
			//this.m_dragunController.IsTransitioning = false;
			//this.SpawnBones(this.fingerDebris, UnityEngine.Random.Range(3, 6), new Vector2(2f, 4f), new Vector3(-24f, -15f));
			//this.SpawnBones(this.fingerDebris, UnityEngine.Random.Range(3, 6), new Vector2(24f, 4f), new Vector3(-2f, -15f));
			//this.SpawnBones(this.neckDebris, UnityEngine.Random.Range(1, 3), new Vector2(2f, 4f), new Vector3(-24f, -15f));
			//this.SpawnBones(this.neckDebris, UnityEngine.Random.Range(1, 3), new Vector2(24f, 4f), new Vector3(-2f, -15f));
			//this.SpawnBones(this.skullDebris, 1, new Vector2(8f, 6f), new Vector2(-22f, -23f));
			//Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
			//DraGunRoomPlaceable dragunRoomController = this.aiActor.ParentRoom.GetComponentsAbsoluteInRoom<DraGunRoomPlaceable>()[0];



			//dragunRoomController.DraGunKilled = true;
			this.healthHaver.DeathAnimationComplete(null, null);
			
			yield return new WaitForSeconds(0.75f);
			//dragunRoomController.ExtendDeathBridge();

			//AssetBundle sharedAssets = ResourceManager.LoadAssetBundle("shared_auto_001");
			//GameObject dragunroomplaceable = sharedAssets.LoadAsset<GameObject>("dragunroomplaceable");
			//MovingPlatform m_deathBridge = dragunroomplaceable.GetComponentInChildren<MovingPlatform>();

			Debug.Log("Extending Bridge");
			//ExtendDeathBridge();
			
			Debug.Log("here");
            //for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
            //{
            //    GameManager.Instance.AllPlayers[j].ClearInputOverride("DraGunDeathController");
            //}
            //if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH)
            //{
            //    GameUIRoot.Instance.ShowCoreUI("dragun");
            //    GameUIRoot.Instance.ToggleLowerPanels(true, false, "dragun");
            //}
            yield return null;
		   // GameManager.Instance.PreventPausing = false;
            //for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
            //{
            //    PlayerController playerController = GameManager.Instance.AllPlayers[k];
            //    if (playerController && playerController.passiveItems != null)
            //    {
            //        for (int l = 0; l < playerController.passiveItems.Count; l++)
            //        {
            //            CompanionItem companionItem = playerController.passiveItems[l] as CompanionItem;
            //            if (companionItem && companionItem.ExtantCompanion)
            //            {
            //                GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_SUNLIGHT_SPEAR_UNLOCK, true);
            //                if (!GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_DRAGUN_WITH_TURTLE))
            //                {
            //                    CompanionController component = companionItem.ExtantCompanion.GetComponent<CompanionController>();
            //                    if (component && component.companionID == CompanionController.CompanionIdentifier.SUPER_SPACE_TURTLE)
            //                    {
            //                        GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_DRAGUN_WITH_TURTLE, true);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
            //{
            //    GameManager.Instance.MainCameraController.SetManualControl(true, true);
            //    GameStatsManager.Instance.SetFlag(GungeonFlags.SHERPA_BOSSRUSH_COMPLETE, true);
            //    GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_BOSSRUSH, true);
            //    GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
            //    GameUIRoot.Instance.HideCoreUI(string.Empty);
            //    for (int m = 0; m < GameManager.Instance.AllPlayers.Length; m++)
            //    {
            //        GameManager.Instance.AllPlayers[m].SetInputOverride("game complete");
            //    }
            //    Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
            //    AmmonomiconController.Instance.OpenAmmonomicon(true, true);
            //}
            //if (GameStatsManager.Instance.IsRainbowRun)
            //{
            //    GameStatsManager.Instance.SetFlag(GungeonFlags.BOWLER_RAINBOW_RUN_COMPLETE, true);
            //}
			//yield return new WaitForSeconds(5f);
			TeleportToEnd();
			Debug.Log("End");
			UnityEngine.Object.Destroy(this.gameObject);
			yield break;
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x0021D450 File Offset: 0x0021B650
		//private void SpawnBones(GameObject bonePrefab, int count, Vector2 min, Vector2 max)
		//{
		//	Vector2 min2 = base.aiActor.ParentRoom.area.basePosition.ToVector2() + min + new Vector2(0f, (float)DraGunRoomPlaceable.HallHeight);
		//	Vector2 max2 = base.aiActor.ParentRoom.area.basePosition.ToVector2() + base.aiActor.ParentRoom.area.dimensions.ToVector2() + max;
		//	for (int i = 0; i < count; i++)
		//	{
		//		Vector2 v = BraveUtility.RandomVector2(min2, max2);
		//		GameObject gameObject = SpawnManager.SpawnVFX(bonePrefab, v, Quaternion.identity);
		//		if (gameObject)
		//		{
		//			gameObject.transform.parent = SpawnManager.Instance.VFX;
		//			DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
		//			orAddComponent.decayOnBounce = 0.5f;
		//			orAddComponent.bounceCount = 1;
		//			orAddComponent.canRotate = true;
		//			float angle = UnityEngine.Random.Range(-80f, -100f);
		//			Vector2 vector = BraveMathCollege.DegreesToVector(angle, 1f) * UnityEngine.Random.Range(0.1f, 3f);
		//			Vector3 startingForce = new Vector3(vector.x, (vector.y >= 0f) ? 0f : vector.y, (vector.y <= 0f) ? 0f : vector.y);
		//			if (orAddComponent.minorBreakable)
		//			{
		//				orAddComponent.minorBreakable.enabled = true;
		//			}
		//			orAddComponent.Trigger(startingForce, UnityEngine.Random.Range(1f, 2f), 1f);
		//			if (orAddComponent.specRigidbody)
		//			{
		//				DebrisObject debrisObject = orAddComponent;
		//				debrisObject.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnGrounded, new Action<DebrisObject>(this.HandleComplexDebris));
		//			}
		//		}
		//	}
		//}

		//// Token: 0x06005A54 RID: 23124 RVA: 0x0021D63C File Offset: 0x0021B83C
		//private void HandleComplexDebris(DebrisObject debrisObject)
		//{
		//	GameManager.Instance.StartCoroutine(this.DelayedSpriteFixer(debrisObject.sprite));
		//	SpeculativeRigidbody specRigidbody = debrisObject.specRigidbody;
		//	PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(specRigidbody, null, false);
		//	UnityEngine.Object.Destroy(debrisObject);
		//	specRigidbody.RegenerateCache();
		//}

		//// Token: 0x06005A55 RID: 23125 RVA: 0x0021D688 File Offset: 0x0021B888
		//private IEnumerator DelayedSpriteFixer(tk2dBaseSprite sprite)
		//{
		//	yield return null;
		//	sprite.HeightOffGround = -1f;
		//	sprite.IsPerpendicular = true;
		//	sprite.UpdateZDepth();
		//	yield break;
		//}




		//// Token: 0x040053B6 RID: 21430
		//public List<GameObject> explosionVfx;

		//// Token: 0x040053B7 RID: 21431
		//public float explosionMidDelay;

		//// Token: 0x040053B8 RID: 21432
		//public int explosionCount;

		//// Token: 0x040053B9 RID: 21433
		//public GameObject skullDebris;

		//// Token: 0x040053BA RID: 21434
		//public GameObject fingerDebris;

		//// Token: 0x040053BB RID: 21435
		//public GameObject neckDebris;

		//// Token: 0x040053BC RID: 21436
		//private DraGunController m_dragunController;

		//// Token: 0x040053BD RID: 21437
		//private tk2dSpriteAnimator m_deathDummy;

		public void ExtendDeathBridge()
		{
			if (this.m_deathBridge == null)
			{
				Debug.Log("Cannot Find m_deathBridge");
			}
            else
            {
				Debug.Log("Found Find m_deathBridge");
			}
			this.m_deathBridge.enabled = true;
			this.m_deathBridge.specRigidbody.enabled = true;
			this.m_deathBridge.specRigidbody.Initialize();
			this.m_deathBridge.spriteAnimator.Play();
			this.m_deathBridge.MarkCells();


		}

		public void TeleportToEnd()
        {
			RoomHandler roomHandler;
			Dungeon d = GameManager.Instance.Dungeon;
			int count = d.data.rooms.Count;
			Debug.Log("Teleporting");
			for (int i = 0; i < count; i++)
			//foreach (RoomHandler roomHandler in d.data.rooms)
			{
				roomHandler = d.data.rooms[i];
				if (roomHandler.GetRoomName() == "Exit_Room_Forge")
				{
					//flag = true;
					ETGModConsole.Log("Teleporting to: " + roomHandler.GetRoomName());
					//Tele(roomHandler);

					//IntVector2 Epicenter = roomHandler.Epicenter;

					//Vector2 vEpicenter = Epicenter.ToVector2();

					//PlayerController primaryPlayer = Gungeon.Game.PrimaryPlayer;
					//primaryPlayer.TeleportToPoint(vEpicenter, true);

					roomHandler.AddProceduralTeleporterToRoom();

				}
			}
			//RoomHandler room;


		}


		public MovingPlatform m_deathBridge;

	}
	public class GenericDeathCreditsController : BraveBehaviour
	{
		// Token: 0x060058FD RID: 22781 RVA: 0x002148B9 File Offset: 0x00212AB9
		public GenericDeathCreditsController()
		{

		}

		// Token: 0x060058FE RID: 22782 RVA: 0x002148E7 File Offset: 0x00212AE7
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			
		}

		// Token: 0x060058FF RID: 22783 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005900 RID: 22784 RVA: 0x0021490C File Offset: 0x00212B0C
		private void OnBossDeath(Vector2 dir)
		{
			Debug.Log("Begin BossDeath");
			base.behaviorSpeculator.enabled = false;
			base.StartCoroutine(this.OnDeathExplosionsCR());
			Debug.Log("Here");
			


			PlayerController primaryPlayer = Gungeon.Game.PrimaryPlayer;
			RoomHandler room = primaryPlayer.CurrentRoom;
			List<AIActor> enemies = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);

			if (enemies.Count == 1)
			{
				base.StartCoroutine(this.HandleBossKilled());
			}



			//Dungeon d = GameManager.Instance.Dungeon;
			//Debug.Log("dungeon is active: " + d.isActiveAndEnabled);
			//Debug.Log(d.gameObject.name);


			//GameManager.Instance.DelayedReturnToFoyer(3f);

			//Debug.Log("End BossDeath");
		}

		// Token: 0x06005901 RID: 22785 RVA: 0x00214974 File Offset: 0x00212B74

		private IEnumerator HandleBossKilled()
		{

			//PlayableCharacters playablecharacter = GameManager.Instance.PrimaryPlayer.characterIdentity;
			//GameStatsManager.Instance.SetCharacterSpecificFlag(playablecharacter, CharacterSpecificGungeonFlags.KILLED_PAST, true);

			Debug.Log("Here2");
			//GameManager.Instance.ReturnToFoyer();
			//GameManager.Instance.DelayedReturnToFoyer(3f);
			GameManager.Instance.DelayedQuickRestart(3f);
			Debug.Log("Here3");
			UnityEngine.Object.Destroy(this.gameObject);
			Debug.Log("Here4");
			yield break;

		}

		//private PlayerController m_pilot = GameManager.Instance.PrimaryPlayer;

		private IEnumerator OnDeathExplosionsCR()
		{
			yield return null;
			BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
			if (extantCam)
			{
				extantCam.ForceCancelSequence();
			}
			GameManager.Instance.MainCameraController.DoContinuousScreenShake(new ScreenShakeSettings(2f, 20f, 1f, 0f, Vector2.right), this, false);
			//for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
			//{
			//	GameManager.Instance.AllPlayers[k].SetInputOverride("past");
			//}

			GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
			this.healthHaver.DeathAnimationComplete(null, null);
			

			yield break;
		}

	}
	public class LichRoom01DeathController : BraveBehaviour
	{
		// Token: 0x06005A4B RID: 23115 RVA: 0x0021D328 File Offset: 0x0021B528
		public LichRoom01DeathController()
		{
		}

		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;

			
		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			//base.OnDestroy();
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x0021D3A6 File Offset: 0x0021B5A6
		private void OnBossDeath(Vector2 dir)
		{
			base.behaviorSpeculator.enabled = false;
			GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());
			
			base.StartCoroutine(this.HandleBossKilled());

		}



		// Token: 0x06005A52 RID: 23122 RVA: 0x0021D434 File Offset: 0x0021B634
		private IEnumerator OnDeathExplosionsCR()
		{
			yield return new WaitForSeconds(0.3f);
	
			this.healthHaver.DeathAnimationComplete(null, null);
			//UnityEngine.Object.Destroy(this.gameObject);
			yield return new WaitForSeconds(0.75f);


			
			yield break;
		}
		private IEnumerator HandleBossKilled()
		{


			Debug.Log("Here2");

			//UnityEngine.Object.Destroy(this.gameObject);
			UniqueBossRoomDeathHandler.TeleportToEnd("LichRoom02");
			Debug.Log("Here3");
			
			Debug.Log("Here4");
			yield break;

		}

	}
	public class LichRoom02DeathController : BraveBehaviour
	{
		// Token: 0x06005A4B RID: 23115 RVA: 0x0021D328 File Offset: 0x0021B528
		public LichRoom02DeathController()
		{
		}

		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;


		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			//base.OnDestroy();
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x0021D3A6 File Offset: 0x0021B5A6
		private void OnBossDeath(Vector2 dir)
		{
			base.behaviorSpeculator.enabled = false;
			GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());

			base.StartCoroutine(this.HandleBossKilled());

		}



		// Token: 0x06005A52 RID: 23122 RVA: 0x0021D434 File Offset: 0x0021B634
		private IEnumerator OnDeathExplosionsCR()
		{
			yield return new WaitForSeconds(0.3f);

			this.healthHaver.DeathAnimationComplete(null, null);
			//UnityEngine.Object.Destroy(this.gameObject);
			yield return new WaitForSeconds(0.75f);



			yield break;
		}
		private IEnumerator HandleBossKilled()
		{


			Debug.Log("Here2");

			//UnityEngine.Object.Destroy(this.gameObject);
			UniqueBossRoomDeathHandler.TeleportToEnd("LichRoom03");
			Debug.Log("Here3");
			
			Debug.Log("Here4");
			yield break;

		}

	}
	public class ResourcefulRatRoom01DeathController : BraveBehaviour
	{
		// Token: 0x06005A4B RID: 23115 RVA: 0x0021D328 File Offset: 0x0021B528
		public ResourcefulRatRoom01DeathController()
		{
		}

		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;


		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x0021D3A6 File Offset: 0x0021B5A6
		private void OnBossDeath(Vector2 dir)
		{
			base.behaviorSpeculator.enabled = false;
			GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());

			PlayerController primaryPlayer = Gungeon.Game.PrimaryPlayer;
			RoomHandler room = primaryPlayer.CurrentRoom;
			List<AIActor> enemies = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);
			
			if (enemies.Count == 1)
            {
				base.StartCoroutine(this.HandleBossKilled());
			}
			

		}



		// Token: 0x06005A52 RID: 23122 RVA: 0x0021D434 File Offset: 0x0021B634
		private IEnumerator OnDeathExplosionsCR()
		{
			yield return new WaitForSeconds(0.3f);

			this.healthHaver.DeathAnimationComplete(null, null);
			//UnityEngine.Object.Destroy(this.gameObject);
			yield return new WaitForSeconds(0.75f);



			yield break;
		}
		private IEnumerator HandleBossKilled()
		{


			Debug.Log("Here2");

			UnityEngine.Object.Destroy(this.gameObject);
			UniqueBossRoomDeathHandler.TeleportToSpecificRoom("MetalGearRatRoom01");
			Debug.Log("Here3");
			
			Debug.Log("Here4");
			yield break;

		}

	}
	public class Bullet_End_Room_03DeathController : BraveBehaviour
	{
		// Token: 0x06005A4B RID: 23115 RVA: 0x0021D328 File Offset: 0x0021B528
		public Bullet_End_Room_03DeathController()
		{
		}

		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;


		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x0021D3A6 File Offset: 0x0021B5A6
		private void OnBossDeath(Vector2 dir)
		{
			base.behaviorSpeculator.enabled = false;
			GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());

			base.StartCoroutine(this.HandleBossKilled());

		}



		// Token: 0x06005A52 RID: 23122 RVA: 0x0021D434 File Offset: 0x0021B634
		private IEnumerator OnDeathExplosionsCR()
		{
			yield return new WaitForSeconds(0.3f);

			this.healthHaver.DeathAnimationComplete(null, null);
			//UnityEngine.Object.Destroy(this.gameObject);
			yield return new WaitForSeconds(0.75f);



			yield break;
		}
		private IEnumerator HandleBossKilled()
		{


			Debug.Log("Here2");

			UnityEngine.Object.Destroy(this.gameObject);
			UniqueBossRoomDeathHandler.TeleportToEnd("Bullet_End_Room_04");
			Debug.Log("Here3");
			
			Debug.Log("Here4");
			yield break;

		}

	}
	public class MetalGearRatRoom01DeathController : BraveBehaviour
	{
		// Token: 0x06005A4B RID: 23115 RVA: 0x0021D328 File Offset: 0x0021B528
		public MetalGearRatRoom01DeathController()
		{
		}

		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;


		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x0021D3A6 File Offset: 0x0021B5A6
		private void OnBossDeath(Vector2 dir)
		{
			base.behaviorSpeculator.enabled = false;
			GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());

			base.StartCoroutine(this.HandleBossKilled());

		}



		// Token: 0x06005A52 RID: 23122 RVA: 0x0021D434 File Offset: 0x0021B634
		private IEnumerator OnDeathExplosionsCR()
		{
			yield return new WaitForSeconds(0.3f);

			this.healthHaver.DeathAnimationComplete(null, null);
			//UnityEngine.Object.Destroy(this.gameObject);
			yield return new WaitForSeconds(0.75f);



			yield break;
		}
		private IEnumerator HandleBossKilled()
		{


			UnityEngine.Object.Destroy(this.gameObject);
			UniqueBossRoomDeathHandler.TeleportToSpecificRoom("ResourcefulRat_RewardRoom_01");

			
			for (int i = 0; i < 4; i++)
			{
				Game.PrimaryPlayer.GiveItem("rat_key");
			}
			
			yield break;

		}

	}
}

