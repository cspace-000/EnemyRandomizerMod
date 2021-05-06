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
{//replacement boss death controller for turning bosses into regular enemies


	public class GRandomAgunimDeathController : BraveBehaviour
	{
		// Token: 0x060057A9 RID: 22441 RVA: 0x0020CC58 File Offset: 0x0020AE58
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			base.healthHaver.OverrideKillCamTime = new float?(5f);
		}

		// Token: 0x060057AA RID: 22442 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x060057AB RID: 22443 RVA: 0x0020CC94 File Offset: 0x0020AE94
		private void OnBossDeath(Vector2 dir)
		{
			base.aiAnimator.ChildAnimator.gameObject.SetActive(false);
			base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
			base.StartCoroutine(this.HandlePostDeathExplosionCR());
			base.healthHaver.OnPreDeath -= this.OnBossDeath;
			base.StartCoroutine(this.HandlePostDeathExplosionCR());
		}

		// Token: 0x060057AC RID: 22444 RVA: 0x0020CD04 File Offset: 0x0020AF04
		private IEnumerator HandlePostDeathExplosionCR()
		{
			yield return null;
			//BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
			//if (extantCam)
			//{
			//	extantCam.ForceCancelSequence();
			//}
			this.aiActor.StealthDeath = true;
			this.healthHaver.persistsOnDeath = true;
			this.healthHaver.DeathAnimationComplete(null, null);
			UnityEngine.Object.Destroy(this.GetComponent<AgunimIntroDoer>());
			this.aiActor.ToggleRenderers(true);
			if (this.specRigidbody)
			{
				this.specRigidbody.enabled = false;
			}
			if (this.aiActor)
			{
				UnityEngine.Object.Destroy(this.aiActor);
			}
			if (this.healthHaver)
			{
				UnityEngine.Object.Destroy(this.healthHaver);
			}
			if (this.behaviorSpeculator)
			{
				UnityEngine.Object.Destroy(this.behaviorSpeculator);
			}
			this.RegenerateCache();



			//while (base.aiAnimator.IsPlaying("die"))
			//{
			//	yield return null;
			//}
			//UnityEngine.Object.Destroy(this.AgunimPostDeathTalker.gameObject);

			//}


			yield return new WaitForSeconds(1f);

			UnityEngine.Object.Destroy(this.gameObject);
		}

		//public IEnumerator GRandomHandleAgunimDeath(Transform bossTransform)
		//{

		//	while (this.AgunimPostDeathTalker.aiAnimator.IsPlaying("die"))
		//	{
		//		yield return null;
		//	}
		//	yield break;
		//}
		//public TalkDoerLite AgunimPostDeathTalker;




	}
	public class GRandomMetalGearRatDeathController : BraveBehaviour
	{
		// Token: 0x06005C66 RID: 23654 RVA: 0x0022AFF1 File Offset: 0x002291F1
		public GRandomMetalGearRatDeathController()
		{
			this.explosionMidDelay = 0.3f;
			this.explosionCount = 10;
		}

		// Token: 0x06005C67 RID: 23655 RVA: 0x0022B00C File Offset: 0x0022920C
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			base.healthHaver.OverrideKillCamTime = new float?(3.5f);
		}

		// Token: 0x06005C68 RID: 23656 RVA: 0x0022B046 File Offset: 0x00229246
		protected override void OnDestroy()
		{
			if (ChallengeManager.CHALLENGE_MODE_ACTIVE && this.m_challengesSuppressed)
			{
				ChallengeManager.Instance.SuppressChallengeStart = false;
				this.m_challengesSuppressed = false;
			}
			base.OnDestroy();
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x0022B078 File Offset: 0x00229278
		private void OnBossDeath(Vector2 dir)
		{
			base.aiAnimator.PlayUntilCancelled("death", false, null, -1f, false);
			base.aiAnimator.PlayVfx("death", null, null, null);
			GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());
			GameManager.Instance.StartCoroutine(this.OnDeathCR());
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x0022B0EC File Offset: 0x002292EC
		private IEnumerator OnDeathExplosionsCR()
		{
			PixelCollider collider = this.specRigidbody.HitboxPixelCollider;
			for (int i = 0; i < this.explosionCount; i++)
			{
				Vector2 minPos = collider.UnitBottomLeft;
				Vector2 maxPos = collider.UnitTopRight;
				GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
				Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.2f, 0.2f));
				GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
				tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
				vfxSprite.HeightOffGround = 0.8f;
				this.sprite.AttachRenderer(vfxSprite);
				this.sprite.UpdateZDepth();
				yield return new WaitForSeconds(this.explosionMidDelay);
			}
			yield break;
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x0022B108 File Offset: 0x00229308
		private IEnumerator OnDeathCR()
		{
			//SuperReaperController.PreventShooting = true;
			//foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
			//{
			//	playerController.SetInputOverride("metal gear death");
			//}
			//yield return new WaitForSeconds(2f);
			//Pixelator.Instance.FadeToColor(0.75f, Color.white, false, 0f);
			//Minimap.Instance.TemporarilyPreventMinimap = true;
			//GameUIRoot.Instance.HideCoreUI(string.Empty);
			//GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
			//yield return new WaitForSeconds(3f);
			//MetalGearRatIntroDoer introDoer = this.GetComponent<MetalGearRatIntroDoer>();
			//introDoer.ModifyCamera(false);
			//yield return new WaitForSeconds(0.75f);
			this.aiActor.StealthDeath = true;
			this.healthHaver.persistsOnDeath = true;
			this.healthHaver.DeathAnimationComplete(null, null);
			if (this.aiActor)
			{
				UnityEngine.Object.Destroy(this.aiActor);
			}
			if (this.healthHaver)
			{
				UnityEngine.Object.Destroy(this.healthHaver);
			}
			if (this.behaviorSpeculator)
			{
				UnityEngine.Object.Destroy(this.behaviorSpeculator);
			}
			if (this.aiAnimator.ChildAnimator)
			{
				UnityEngine.Object.Destroy(this.aiAnimator.ChildAnimator.gameObject);
			}
			if (this.aiAnimator)
			{
				UnityEngine.Object.Destroy(this.aiAnimator);
			}
			if (this.specRigidbody)
			{
				UnityEngine.Object.Destroy(this.specRigidbody);
			}
			this.RegenerateCache();
			//MetalGearRatRoomController room = UnityEngine.Object.FindObjectOfType<MetalGearRatRoomController>();
			//room.TransformToDestroyedRoom();
			//GameManager.Instance.PrimaryPlayer.WarpToPoint(room.transform.position + new Vector3(17.25f, 14.5f), false, false);
			//if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer)
			//{
			//	GameManager.Instance.SecondaryPlayer.WarpToPoint(room.transform.position + new Vector3(27.5f, 14.5f), false, false);
			//}
			//this.aiActor.ToggleRenderers(false);
			//GameObject punchoutMinigame = UnityEngine.Object.Instantiate<GameObject>(this.PunchoutMinigamePrefab);
			//PunchoutController punchoutController = punchoutMinigame.GetComponent<PunchoutController>();
			//punchoutController.Init();
			yield return null;
			//foreach (PlayerController playerController2 in GameManager.Instance.AllPlayers)
			//{
			//	playerController2.ClearInputOverride("metal gear death");
			//}
			//Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
			yield return new WaitForSeconds(1f);
			//Minimap.Instance.TemporarilyPreventMinimap = false;
			UnityEngine.Object.Destroy(this.gameObject);
			yield break;
		}

		// Token: 0x04005601 RID: 22017
		public GameObject PunchoutMinigamePrefab;

		// Token: 0x04005602 RID: 22018
		public List<GameObject> explosionVfx;

		// Token: 0x04005603 RID: 22019
		public float explosionMidDelay;

		// Token: 0x04005604 RID: 22020
		public int explosionCount;

		// Token: 0x04005605 RID: 22021
		private bool m_challengesSuppressed;
	}
	public class GRandomDraGunDeathController : BraveBehaviour
	{
		// Token: 0x06005A4B RID: 23115 RVA: 0x0021D328 File Offset: 0x0021B528
		public GRandomDraGunDeathController()
		{
			this.explosionMidDelay = 0.3f;
			this.explosionCount = 10;

		}

		// Token: 0x06005A4C RID: 23116 RVA: 0x0021D343 File Offset: 0x0021B543
		public void Awake()
		{
			this.m_dragunController = base.GetComponent<DraGunController>();
			this.m_deathDummy = base.transform.Find("DeathDummy").GetComponent<tk2dSpriteAnimator>();
		}

		// Token: 0x06005A4D RID: 23117 RVA: 0x0021D36C File Offset: 0x0021B56C
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			base.healthHaver.OverrideKillCamTime = new float?(6.25f);
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

		// Token: 0x06005A50 RID: 23120 RVA: 0x0021D3D8 File Offset: 0x0021B5D8
		private IEnumerator LerpCrackEmission(float startVal, float targetVal, float duration)
		{
			Material targetMaterial = this.m_deathDummy.GetComponent<Renderer>().material;
			float ela = 0f;
			while (ela < duration)
			{
				ela += GameManager.INVARIANT_DELTA_TIME;
				float t = ela / duration;
				t *= t;
				targetMaterial.SetFloat("_CrackAmount", Mathf.Lerp(startVal, targetVal, t));
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005A51 RID: 23121 RVA: 0x0021D408 File Offset: 0x0021B608
		private IEnumerator LerpCrackColor(Color targetVal, float duration)
		{
			Material targetMaterial = this.m_deathDummy.GetComponent<Renderer>().material;
			Color startVal = targetMaterial.GetColor("_CrackBaseColor");
			float ela = 0f;
			while (ela < duration)
			{
				ela += GameManager.INVARIANT_DELTA_TIME;
				float t = ela / duration;
				t *= t;
				targetMaterial.SetColor("_CrackBaseColor", Color.Lerp(startVal, targetVal, t));
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x0021D434 File Offset: 0x0021B634
		private IEnumerator OnDeathExplosionsCR()
		{
			//for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			//{
			//	GameManager.Instance.AllPlayers[i].SetInputOverride("DraGunDeathController");
			//}
			//GameManager.Instance.PreventPausing = true;
			//GameUIRoot.Instance.HideCoreUI("dragun");
			//GameUIRoot.Instance.ToggleLowerPanels(false, false, "dragun");
			//this.healthHaver.OverrideKillCamPos = new Vector2?(this.specRigidbody.UnitCenter + new Vector2(0f, 6f));
			this.aiAnimator.PlayUntilCancelled("heart_burst", false, null, -1f, false);
			while (this.aiAnimator.IsPlaying("heart_burst"))
			{
				yield return null;
			}
			this.aiAnimator.EndAnimationIf("heart_burst");
			this.aiAnimator.PlayVfx("heart_burst", null, null, null);
			//Pixelator.Instance.FadeToColor(0.75f, Color.white, true, 0f);
			yield return new WaitForSeconds(0.3f);
			//GameManager.Instance.PreventPausing = true;
			this.aiActor.ToggleRenderers(true);
			this.m_deathDummy.gameObject.SetActive(true);
			this.m_deathDummy.GetComponent<Renderer>().enabled = true;
			this.m_dragunController.IsTransitioning = true;
			this.m_deathDummy.Play("die");
			this.StartCoroutine(this.LerpCrackEmission(1f, 250f, 3f));
			yield return new WaitForSeconds(3f);
			//GameManager.Instance.PreventPausing = true;
			this.StartCoroutine(this.LerpCrackColor(Color.white, 3f));
			this.StartCoroutine(this.LerpCrackEmission(250f, 50000f, 3f));
			yield return new WaitForSeconds(1.5f);
			//Pixelator.Instance.FadeToColor(0.5f, Color.white, false, 0f);
			yield return new WaitForSeconds(0.75f);
			//this.m_dragunController.ModifyCamera(false);
			//this.m_dragunController.BlockPitTiles(false);
			yield return new WaitForSeconds(0.75f);
			this.m_dragunController.IsTransitioning = false;
			//this.SpawnBones(this.fingerDebris, UnityEngine.Random.Range(3, 6), new Vector2(2f, 4f), new Vector3(-24f, -15f));
			//this.SpawnBones(this.fingerDebris, UnityEngine.Random.Range(3, 6), new Vector2(24f, 4f), new Vector3(-2f, -15f));
			//this.SpawnBones(this.neckDebris, UnityEngine.Random.Range(1, 3), new Vector2(2f, 4f), new Vector3(-24f, -15f));
			//this.SpawnBones(this.neckDebris, UnityEngine.Random.Range(1, 3), new Vector2(24f, 4f), new Vector3(-2f, -15f));
			//this.SpawnBones(this.skullDebris, 1, new Vector2(8f, 6f), new Vector2(-22f, -23f));
			//Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
			//DraGunRoomPlaceable dragunRoomController = this.aiActor.ParentRoom.GetComponentsAbsoluteInRoom<DraGunRoomPlaceable>()[0];
			//dragunRoomController.DraGunKilled = true;
			this.healthHaver.DeathAnimationComplete(null, null);
			UnityEngine.Object.Destroy(this.gameObject);
			yield return new WaitForSeconds(0.75f);
			//dragunRoomController.ExtendDeathBridge();
			//for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			//{
			//	GameManager.Instance.AllPlayers[j].ClearInputOverride("DraGunDeathController");
			//}
			//if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH)
			//{
			//	GameUIRoot.Instance.ShowCoreUI("dragun");
			//	GameUIRoot.Instance.ToggleLowerPanels(true, false, "dragun");
			//}
			yield return null;
			//GameManager.Instance.PreventPausing = false;
			//for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
			//{
			//	PlayerController playerController = GameManager.Instance.AllPlayers[k];
			//	if (playerController && playerController.passiveItems != null)
			//	{
			//		for (int l = 0; l < playerController.passiveItems.Count; l++)
			//		{
			//			CompanionItem companionItem = playerController.passiveItems[l] as CompanionItem;
			//			if (companionItem && companionItem.ExtantCompanion)
			//			{
			//				GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_SUNLIGHT_SPEAR_UNLOCK, true);
			//				if (!GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_DRAGUN_WITH_TURTLE))
			//				{
			//					CompanionController component = companionItem.ExtantCompanion.GetComponent<CompanionController>();
			//					if (component && component.companionID == CompanionController.CompanionIdentifier.SUPER_SPACE_TURTLE)
			//					{
			//						GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_DRAGUN_WITH_TURTLE, true);
			//					}
			//				}
			//			}
			//		}
			//	}
			//}
			//if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
			//{
			//	GameManager.Instance.MainCameraController.SetManualControl(true, true);
			//	GameStatsManager.Instance.SetFlag(GungeonFlags.SHERPA_BOSSRUSH_COMPLETE, true);
			//	GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_BOSSRUSH, true);
			//	GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
			//	GameUIRoot.Instance.HideCoreUI(string.Empty);
			//	for (int m = 0; m < GameManager.Instance.AllPlayers.Length; m++)
			//	{
			//		GameManager.Instance.AllPlayers[m].SetInputOverride("game complete");
			//	}
			//	//Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
			//	//AmmonomiconController.Instance.OpenAmmonomicon(true, true);
			//}
			//if (GameStatsManager.Instance.IsRainbowRun)
			//{
			//	GameStatsManager.Instance.SetFlag(GungeonFlags.BOWLER_RAINBOW_RUN_COMPLETE, true);
			//}
			yield break;
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x0021D450 File Offset: 0x0021B650
		private void SpawnBones(GameObject bonePrefab, int count, Vector2 min, Vector2 max)
		{
			Vector2 min2 = base.aiActor.ParentRoom.area.basePosition.ToVector2() + min + new Vector2(0f, (float)DraGunRoomPlaceable.HallHeight);
			Vector2 max2 = base.aiActor.ParentRoom.area.basePosition.ToVector2() + base.aiActor.ParentRoom.area.dimensions.ToVector2() + max;
			for (int i = 0; i < count; i++)
			{
				Vector2 v = BraveUtility.RandomVector2(min2, max2);
				GameObject gameObject = SpawnManager.SpawnVFX(bonePrefab, v, Quaternion.identity);
				if (gameObject)
				{
					gameObject.transform.parent = SpawnManager.Instance.VFX;
					DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
					orAddComponent.decayOnBounce = 0.5f;
					orAddComponent.bounceCount = 1;
					orAddComponent.canRotate = true;
					float angle = UnityEngine.Random.Range(-80f, -100f);
					Vector2 vector = BraveMathCollege.DegreesToVector(angle, 1f) * UnityEngine.Random.Range(0.1f, 3f);
					Vector3 startingForce = new Vector3(vector.x, (vector.y >= 0f) ? 0f : vector.y, (vector.y <= 0f) ? 0f : vector.y);
					if (orAddComponent.minorBreakable)
					{
						orAddComponent.minorBreakable.enabled = true;
					}
					orAddComponent.Trigger(startingForce, UnityEngine.Random.Range(1f, 2f), 1f);
					if (orAddComponent.specRigidbody)
					{
						DebrisObject debrisObject = orAddComponent;
						debrisObject.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnGrounded, new Action<DebrisObject>(this.HandleComplexDebris));
					}
				}
			}
		}

		// Token: 0x06005A54 RID: 23124 RVA: 0x0021D63C File Offset: 0x0021B83C
		private void HandleComplexDebris(DebrisObject debrisObject)
		{
			GameManager.Instance.StartCoroutine(this.DelayedSpriteFixer(debrisObject.sprite));
			SpeculativeRigidbody specRigidbody = debrisObject.specRigidbody;
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(specRigidbody, null, false);
			UnityEngine.Object.Destroy(debrisObject);
			specRigidbody.RegenerateCache();
		}

		// Token: 0x06005A55 RID: 23125 RVA: 0x0021D688 File Offset: 0x0021B888
		private IEnumerator DelayedSpriteFixer(tk2dBaseSprite sprite)
		{
			yield return null;
			sprite.HeightOffGround = -1f;
			sprite.IsPerpendicular = true;
			sprite.UpdateZDepth();
			yield break;
		}

		// Token: 0x040053B6 RID: 21430
		public List<GameObject> explosionVfx;

		// Token: 0x040053B7 RID: 21431
		public float explosionMidDelay;

		// Token: 0x040053B8 RID: 21432
		public int explosionCount;

		// Token: 0x040053B9 RID: 21433
		public GameObject skullDebris;

		// Token: 0x040053BA RID: 21434
		public GameObject fingerDebris;

		// Token: 0x040053BB RID: 21435
		public GameObject neckDebris;

		// Token: 0x040053BC RID: 21436
		private DraGunController m_dragunController;

		// Token: 0x040053BD RID: 21437
		private tk2dSpriteAnimator m_deathDummy;
	}
	public class GRandomAdvancedDraGunDeathController2 : BraveBehaviour
	{
		// Token: 0x06005762 RID: 22370 RVA: 0x0020AD7D File Offset: 0x00208F7D
		public void Awake()
		{
			this.m_dragunController = base.GetComponent<DraGunController>();
			this.m_roarDummy = base.aiActor.transform.Find("RoarDummy").GetComponent<tk2dSpriteAnimator>();
		}

		// Token: 0x06005763 RID: 22371 RVA: 0x0020ADAB File Offset: 0x00208FAB
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			base.healthHaver.OverrideKillCamTime = new float?(16.5f);
		}

		// Token: 0x06005764 RID: 22372 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x0020ADE5 File Offset: 0x00208FE5
		private void OnBossDeath(Vector2 dir)
		{
			base.behaviorSpeculator.enabled = false;
			GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x0020AE04 File Offset: 0x00209004
		private IEnumerator OnDeathExplosionsCR()
		{
			//for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			//{
			//	GameManager.Instance.AllPlayers[i].SetInputOverride("DraGunDeathController");
			//}
			//GameManager.Instance.PreventPausing = true;
			//GameUIRoot.Instance.HideCoreUI("dragun");
			//GameUIRoot.Instance.ToggleLowerPanels(false, false, "dragun");
			//Pixelator.Instance.FadeToColor(0.5f, Color.white, true, 0f);
			//this.healthHaver.OverrideKillCamPos = new Vector2?(this.specRigidbody.UnitCenter + new Vector2(0f, 6f));
			this.aiActor.ToggleRenderers(true);
			this.m_dragunController.head.OverrideDesiredPosition = new Vector2?(this.aiActor.transform.position + new Vector3(3.63f, 11.8f));
			this.m_roarDummy.gameObject.SetActive(true);
			this.m_roarDummy.GetComponent<Renderer>().enabled = true;
			this.m_roarDummy.sprite.usesOverrideMaterial = false;
			this.m_roarDummy.Play("death");
			this.aiAnimator.PlayVfx("roar_shake", null, null, null);
			while (this.m_roarDummy.IsPlaying("death"))
			{
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			//GameManager.Instance.PreventPausing = true;
			Animation leftArm = this.m_dragunController.leftArm.GetComponent<Animation>();
			AIAnimator leftHand = this.m_dragunController.leftArm.transform.Find("LeftHand").GetComponent<AIAnimator>();
			Animation rightArm = this.m_dragunController.rightArm.GetComponent<Animation>();
			AIAnimator rightHand = this.m_dragunController.rightArm.transform.Find("RightHand").GetComponent<AIAnimator>();
			AIAnimator head = this.m_dragunController.head.aiAnimator;
			foreach (Renderer renderer in this.m_dragunController.leftArm.GetComponentsInChildren<Renderer>())
			{
				renderer.enabled = true;
			}
			leftArm.Play("DraGunLeftAdvancedDeath");
			leftHand.spriteAnimator.enabled = true;
			leftHand.PlayUntilCancelled("predeath", false, null, -1f, false);
			foreach (Renderer renderer2 in this.m_dragunController.rightArm.GetComponentsInChildren<Renderer>())
			{
				renderer2.enabled = true;
			}
			rightArm.Play("DraGunRightAdvancedDeath");
			rightHand.spriteAnimator.enabled = true;
			rightHand.PlayUntilCancelled("predeath", false, null, -1f, false);
			this.m_dragunController.head.renderer.enabled = true;
			head.spriteAnimator.enabled = true;
			head.PlayUntilCancelled("predeath", false, null, -1f, false);
			head.LockFacingDirection = true;
			this.m_roarDummy.sprite.SetSprite("dragun_gold_death_body_001");
			leftArm.transform.Find("LeftArm (5)").GetComponentInChildren<Renderer>().enabled = false;
			leftArm.transform.Find("LeftArm (6)").GetComponentInChildren<Renderer>().enabled = false;
			rightArm.transform.Find("RightArm (5)").GetComponentInChildren<Renderer>().enabled = false;
			rightArm.transform.Find("RightArm (6)").GetComponentInChildren<Renderer>().enabled = false;
			this.StartCoroutine(this.ExplodeHand(leftHand, 180f));
			yield return new WaitForSeconds(2f);
			this.StartCoroutine(this.ExplodeHand(rightHand, 0f));
			yield return new WaitForSeconds(1.25f);
			this.StartCoroutine(this.ExplodeBall(leftArm, "LeftArm (4)", 180f, 0.5f));
			yield return new WaitForSeconds(0.5f);
			this.StartCoroutine(this.ExplodeBall(rightArm, "RightArm (4)", 0f, 0.5f));
			yield return new WaitForSeconds(0.5f);
			this.StartCoroutine(this.ExplodeBall(leftArm, "LeftArm (3)", 180f, 0.4f));
			yield return new WaitForSeconds(0.4f);
			this.StartCoroutine(this.ExplodeBall(rightArm, "RightArm (3)", 0f, 0.4f));
			yield return new WaitForSeconds(0.4f);
			this.StartCoroutine(this.ExplodeBall(leftArm, "LeftArm (2)", 180f, 0.3f));
			yield return new WaitForSeconds(0.3f);
			this.StartCoroutine(this.ExplodeBall(rightArm, "RightArm (2)", 0f, 0.3f));
			yield return new WaitForSeconds(0.3f);
			this.StartCoroutine(this.ExplodeBall(leftArm, "LeftArm (1)", 180f, 0.2f));
			yield return new WaitForSeconds(0.2f);
			this.StartCoroutine(this.ExplodeBall(rightArm, "RightArm (1)", 0f, 0.9f));
			yield return new WaitForSeconds(0.9f);
			this.StartCoroutine(this.ExplodeShoulder(leftArm, "LeftShoulder", 180f, 0.9f));
			yield return new WaitForSeconds(0.9f);
			this.StartCoroutine(this.ExplodeShoulder(rightArm, "RightShoulder", -90f, 1f));
			yield return new WaitForSeconds(1f);
			head.sprite.usesOverrideMaterial = false;
			head.PlayUntilCancelled("death", false, null, -1f, false);
			Vector2 shardPos = head.sprite.WorldCenter;
			yield return new WaitForSeconds(0.7f);
			this.m_roarDummy.Play("death_body_head_explode");
			this.aiAnimator.PlayVfx("death_big_shake", null, null, null);
			foreach (SpawnShardsOnDeath spawnShardsOnDeath in this.m_dragunController.head.GetComponentsInChildren<SpawnShardsOnDeath>())
			{
				spawnShardsOnDeath.HandleShardSpawns(Vector2.zero, new Vector2?(shardPos));
			}
			yield return new WaitForSeconds(1.67f);
			this.aiAnimator.PlayVfx("death_slow_shake", null, null, null);
			yield return new WaitForSeconds(0.66f);
			this.StartCoroutine(this.FadeBodyCR(1.33f));
			yield return new WaitForSeconds(0.67f);
			head.renderer.enabled = false;
			//Pixelator.Instance.FadeToColor(0.5f, Color.white, false, 0f);
			yield return new WaitForSeconds(0.75f);
			this.m_dragunController.ModifyCamera(false);
			this.m_dragunController.BlockPitTiles(false);
			yield return new WaitForSeconds(0.75f);
			this.m_dragunController.IsTransitioning = false;
			//this.SpawnBones(this.fingerDebris, UnityEngine.Random.Range(3, 6), new Vector2(2f, 4f), new Vector3(-24f, -15f));
			//this.SpawnBones(this.fingerDebris, UnityEngine.Random.Range(3, 6), new Vector2(24f, 4f), new Vector3(-2f, -15f));
			//this.SpawnBones(this.neckDebris, UnityEngine.Random.Range(1, 3), new Vector2(2f, 4f), new Vector3(-24f, -15f));
			//this.SpawnBones(this.neckDebris, UnityEngine.Random.Range(1, 3), new Vector2(24f, 4f), new Vector3(-2f, -15f));
			//Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
			//DraGunRoomPlaceable dragunRoomController = this.aiActor.ParentRoom.GetComponentsAbsoluteInRoom<DraGunRoomPlaceable>()[0];
			//dragunRoomController.DraGunKilled = true;
			this.healthHaver.DeathAnimationComplete(null, null);
			UnityEngine.Object.Destroy(this.gameObject);
			yield return new WaitForSeconds(0.75f);
			//dragunRoomController.ExtendDeathBridge();
			for (int m = 0; m < GameManager.Instance.AllPlayers.Length; m++)
			{
				GameManager.Instance.AllPlayers[m].ClearInputOverride("DraGunDeathController");
			}
			if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH)
			{
				GameUIRoot.Instance.ShowCoreUI("dragun");
				GameUIRoot.Instance.ToggleLowerPanels(true, false, "dragun");
			}
			yield return null;
			GameManager.Instance.PreventPausing = false;
			for (int n = 0; n < GameManager.Instance.AllPlayers.Length; n++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[n];
				if (playerController && playerController.passiveItems != null)
				{
					for (int num = 0; num < playerController.passiveItems.Count; num++)
					{
						CompanionItem companionItem = playerController.passiveItems[num] as CompanionItem;
						if (companionItem && companionItem.ExtantCompanion)
						{
							GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_SUNLIGHT_SPEAR_UNLOCK, true);
							if (!GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_DRAGUN_WITH_TURTLE))
							{
								CompanionController component = companionItem.ExtantCompanion.GetComponent<CompanionController>();
								if (component && component.companionID == CompanionController.CompanionIdentifier.SUPER_SPACE_TURTLE)
								{
									GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_DRAGUN_WITH_TURTLE, true);
								}
							}
						}
					}
				}
			}
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
			{
				GameManager.Instance.MainCameraController.SetManualControl(true, true);
				GameStatsManager.Instance.SetFlag(GungeonFlags.SHERPA_BOSSRUSH_COMPLETE, true);
				GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_BOSSRUSH, true);
				GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
				GameUIRoot.Instance.HideCoreUI(string.Empty);
				for (int num2 = 0; num2 < GameManager.Instance.AllPlayers.Length; num2++)
				{
					GameManager.Instance.AllPlayers[num2].SetInputOverride("game complete");
				}
				Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
				AmmonomiconController.Instance.OpenAmmonomicon(true, true);
			}
			if (GameStatsManager.Instance.IsRainbowRun)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.BOWLER_RAINBOW_RUN_COMPLETE, true);
			}
			yield break;
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x0020AE20 File Offset: 0x00209020
		private IEnumerator ExplodeHand(AIAnimator hand, float headDirection)
		{
			AIAnimator headAnimator = this.m_dragunController.head.aiAnimator;
			headAnimator.FacingDirection = headDirection;
			headAnimator.EndAnimation();
			headAnimator.PlayUntilCancelled("predeath", false, null, -1f, false);
			hand.sprite.usesOverrideMaterial = false;
			hand.PlayUntilCancelled("death", false, null, -1f, false);
			yield return new WaitForSeconds(0.75f);
			this.aiAnimator.PlayVfx("death_small_shake", null, null, null);
			foreach (SpawnShardsOnDeath spawnShardsOnDeath in hand.GetComponentsInChildren<SpawnShardsOnDeath>())
			{
				spawnShardsOnDeath.HandleShardSpawns(Vector2.zero, new Vector2?(hand.sprite.WorldCenter));
			}
			yield break;
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x0020AE4C File Offset: 0x0020904C
		private IEnumerator ExplodeBall(Animation arm, string ballName, float headDirection, float postDelay)
		{
			tk2dSprite ballSprite = arm.transform.Find(ballName).GetComponentInChildren<tk2dSprite>();
			ballSprite.spriteAnimator.enabled = true;
			ballSprite.usesOverrideMaterial = false;
			ballSprite.spriteAnimator.Play("arm_death_explode");
			yield return new WaitForSeconds(0.47f);
			this.aiAnimator.PlayVfx("death_small_shake", null, null, null);
			AIAnimator headAnimator = this.m_dragunController.head.aiAnimator;
			headAnimator.FacingDirection = headDirection;
			headAnimator.EndAnimation();
			if (postDelay < 0.3f)
			{
				AIAnimator aianimator = headAnimator;
				string name = "predeath";
				float warpClipDuration = postDelay - 0.05f;
				aianimator.PlayUntilCancelled(name, false, null, warpClipDuration, false);
			}
			else
			{
				headAnimator.PlayUntilCancelled("predeath", false, null, -1f, false);
			}
			yield break;
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x0020AE84 File Offset: 0x00209084
		private IEnumerator ExplodeShoulder(Animation arm, string ballName, float headDirection, float postDelay)
		{
			tk2dSprite ballSprite = arm.transform.Find(ballName).GetComponentInChildren<tk2dSprite>();
			ballSprite.spriteAnimator.enabled = true;
			ballSprite.usesOverrideMaterial = false;
			ballSprite.spriteAnimator.Play((headDirection <= 0f) ? "death_shoulder_left_explode" : "death_shoulder_right_explode");
			yield return new WaitForSeconds(0.42f);
			this.aiAnimator.PlayVfx("death_small_shake", null, null, null);
			AIAnimator headAnimator = this.m_dragunController.head.aiAnimator;
			headAnimator.FacingDirection = headDirection;
			headAnimator.EndAnimation();
			if (postDelay < 0.3f)
			{
				AIAnimator aianimator = headAnimator;
				string name = "predeath";
				float warpClipDuration = postDelay - 0.05f;
				aianimator.PlayUntilCancelled(name, false, null, warpClipDuration, false);
			}
			else
			{
				headAnimator.PlayUntilCancelled("predeath", false, null, -1f, false);
			}
			yield break;
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x0020AEBC File Offset: 0x002090BC
		private IEnumerator FadeBodyCR(float duration)
		{
			float timer = 0f;
			while (timer < duration)
			{
				yield return null;
				timer += BraveTime.DeltaTime;
				this.m_roarDummy.sprite.renderer.material.SetColor("_OverrideColor", new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, Mathf.Clamp01(timer / duration))));
			}
			yield break;
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x0020AEE0 File Offset: 0x002090E0
		private void SpawnBones(GameObject bonePrefab, int count, Vector2 min, Vector2 max)
		{
			Vector2 min2 = base.aiActor.ParentRoom.area.basePosition.ToVector2() + min + new Vector2(0f, (float)DraGunRoomPlaceable.HallHeight);
			Vector2 max2 = base.aiActor.ParentRoom.area.basePosition.ToVector2() + base.aiActor.ParentRoom.area.dimensions.ToVector2() + max;
			for (int i = 0; i < count; i++)
			{
				Vector2 v = BraveUtility.RandomVector2(min2, max2);
				GameObject gameObject = SpawnManager.SpawnVFX(bonePrefab, v, Quaternion.identity);
				if (gameObject)
				{
					gameObject.transform.parent = SpawnManager.Instance.VFX;
					DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
					orAddComponent.decayOnBounce = 0.5f;
					orAddComponent.bounceCount = 1;
					orAddComponent.canRotate = true;
					float angle = UnityEngine.Random.Range(-80f, -100f);
					Vector2 vector = BraveMathCollege.DegreesToVector(angle, 1f) * UnityEngine.Random.Range(0.1f, 3f);
					Vector3 startingForce = new Vector3(vector.x, (vector.y >= 0f) ? 0f : vector.y, (vector.y <= 0f) ? 0f : vector.y);
					if (orAddComponent.minorBreakable)
					{
						orAddComponent.minorBreakable.enabled = true;
					}
					orAddComponent.Trigger(startingForce, UnityEngine.Random.Range(1f, 2f), 1f);
					if (orAddComponent.specRigidbody)
					{
						DebrisObject debrisObject = orAddComponent;
						debrisObject.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnGrounded, new Action<DebrisObject>(this.HandleComplexDebris));
					}
				}
			}
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x0020B0CC File Offset: 0x002092CC
		private void HandleComplexDebris(DebrisObject debrisObject)
		{
			GameManager.Instance.StartCoroutine(this.DelayedSpriteFixer(debrisObject.sprite));
			SpeculativeRigidbody specRigidbody = debrisObject.specRigidbody;
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(specRigidbody, null, false);
			UnityEngine.Object.Destroy(debrisObject);
			specRigidbody.RegenerateCache();
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x0020B118 File Offset: 0x00209318
		private IEnumerator DelayedSpriteFixer(tk2dBaseSprite sprite)
		{
			yield return null;
			sprite.HeightOffGround = -1f;
			sprite.IsPerpendicular = true;
			sprite.UpdateZDepth();
			yield break;
		}

		// Token: 0x04005063 RID: 20579
		public GameObject fingerDebris;

		// Token: 0x04005064 RID: 20580
		public GameObject neckDebris;

		// Token: 0x04005065 RID: 20581
		private DraGunController m_dragunController;

		// Token: 0x04005066 RID: 20582
		private tk2dSpriteAnimator m_roarDummy;
	}
	public class GRandomGunonDeathController : BraveBehaviour
	{
		// Token: 0x06005B4A RID: 23370 RVA: 0x002245A8 File Offset: 0x002227A8
		public GRandomGunonDeathController()
		{
			this.explosionMidDelay = 0.3f;
			this.explosionCount = 10;

		}

		// Token: 0x06005B4B RID: 23371 RVA: 0x002245C3 File Offset: 0x002227C3
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			base.healthHaver.OverrideKillCamTime = new float?(5f);
		}

		// Token: 0x06005B4C RID: 23372 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005B4D RID: 23373 RVA: 0x00224600 File Offset: 0x00222800
		private void OnBossDeath(Vector2 dir)
		{
			base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
			base.StartCoroutine(this.HandleBossDeath());
			base.healthHaver.OnPreDeath -= this.OnBossDeath;
			AkSoundEngine.PostEvent("Play_BOSS_lichB_explode_01", base.gameObject);
		}

		// Token: 0x06005B4E RID: 23374 RVA: 0x0022465C File Offset: 0x0022285C
		private IEnumerator HandleBossDeath()
		{
			PixelCollider collider = this.specRigidbody.HitboxPixelCollider;
			//GameManager.Instance.MainCameraController.DoContinuousScreenShake(new ScreenShakeSettings(2f, 20f, 1f, 0f, Vector2.right), this, false);
			bool faded = false;
			for (int i = 0; i < this.explosionCount; i++)
			{
				Vector2 minPos = collider.UnitBottomLeft;
				Vector2 maxPos = collider.UnitTopRight;
				GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
				Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.5f, 0.5f));
				GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
				tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
				vfxSprite.HeightOffGround = 3f;
				this.sprite.AttachRenderer(vfxSprite);
				this.sprite.UpdateZDepth();
				if (!faded && (float)i * this.explosionMidDelay < 2f)
				{
					//Pixelator.Instance.FadeToColor(2f, Color.white, false, 0f);
					faded = true;
				}
				yield return new WaitForSeconds(this.explosionMidDelay);
			}
			//GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
			//BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
			//if (extantCam)
			//{
			//	extantCam.ForceCancelSequence();
			//}
			//PlayerController[] allPlayers = GameManager.Instance.AllPlayers;

			//for (int j = 0; j < allPlayers.Length; j++)
			//{
			//	allPlayers[j].CurrentInputState = PlayerInputState.NoInput;
			//}
			//GameManager.Instance.PrimaryPlayer.IsVisible = false;
			//GameManager.Instance.MainCameraController.SetManualControl(true, false);
			//GameManager.Instance.MainCameraController.OverridePosition = this.sprite.WorldCenter;
			//Pixelator.Instance.FadeToColor(0.5f, Color.white, true, 0f);
			this.aiAnimator.PlayUntilCancelled("postdeath", false, null, -1f, false);
			this.aiActor.ShadowObject.transform.localPosition += new Vector3(0f, 0.625f, 0f);
			this.aiActor.ToggleRenderers(true);
			if (this.specRigidbody)
			{
				this.specRigidbody.enabled = false;
			}
			if (this.aiActor)
			{
				UnityEngine.Object.Destroy(this.aiActor);
			}
			if (this.healthHaver)
			{
				UnityEngine.Object.Destroy(this.healthHaver);
			}
			if (this.behaviorSpeculator)
			{
				UnityEngine.Object.Destroy(this.behaviorSpeculator);
			}
			this.RegenerateCache();

			yield return new WaitForSeconds(7.2f);

			UnityEngine.Object.Destroy(this.gameObject);

			yield return null;
			////Pixelator.Instance.FadeToColor(1f, new Color(0.8f, 0.8f, 0.8f), false, 0f);
			yield return null;
			////Pixelator.Instance.FadeToColor(0.6f, new Color(0.8f, 0.8f, 0.8f), true, 0f);
			yield return null;
			////Pixelator.Instance.FadeToBlack(2f, false, 0f);
			yield return null;
			//GameManager.Instance.PrimaryPlayer.IsVisible = true;
			//BulletPastRoomController[] pastRooms = UnityEngine.Object.FindObjectsOfType<BulletPastRoomController>();
			//for (int k = 0; k < pastRooms.Length; k++)
			//{
			//	pastRooms[k].TriggerBulletmanEnding();
			//}
			this.healthHaver.DeathAnimationComplete(null, null);
			yield break;
		}

		// Token: 0x040054DB RID: 21723
		public List<GameObject> explosionVfx;

		// Token: 0x040054DC RID: 21724
		public float explosionMidDelay;

		// Token: 0x040054DD RID: 21725
		public int explosionCount;
	}
	public class GRandomBossFinalRobotDeathController : BraveBehaviour
	{
		// Token: 0x060058A9 RID: 22697 RVA: 0x002137B6 File Offset: 0x002119B6
		public void Start()
		{
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			base.healthHaver.OverrideKillCamTime = new float?(1f);
		}

		// Token: 0x060058AA RID: 22698 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x060058AB RID: 22699 RVA: 0x002137E4 File Offset: 0x002119E4
		private void OnBossDeath(Vector2 dir)
		{
			//UnityEngine.Object.FindObjectOfType<RobotPastController>().OnBossKilled(base.transform);
			base.StartCoroutine(this.OnBossKilled_CR());
		}


		// Token: 0x060066B6 RID: 26294 RVA: 0x002728C0 File Offset: 0x00270AC0
		private IEnumerator OnBossKilled_CR()
		{
			yield return new WaitForSeconds(2f);
			//BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
			//if (extantCam)
			//{
			//	extantCam.ForceCancelSequence();
			//}
			//GameStatsManager.Instance.SetCharacterSpecificFlag(PlayableCharacters.Robot, CharacterSpecificGungeonFlags.KILLED_PAST, true);
			PlayerController m_robot = GameManager.Instance.PrimaryPlayer;
			//PastCameraUtility.LockConversation(m_robot.CenterPosition);
			yield return null;
			//this.EmperorBot.Interact(m_robot);
			//GameManager.Instance.MainCameraController.OverridePosition = this.EmperorSprite.WorldCenter + new Vector2(0f, -3f);
			//m_robot.ForceIdleFacePoint(Vector2.down, true);
			//while (this.EmperorBot.IsTalking)
			//{
			//	yield return null;
			//}
			//this.TurnRobotsOff();
			yield return new WaitForSeconds(2f);
			//Vector2 idealPlayerPos = this.EmperorBot.transform.position + new Vector3(3.5f, -22f, 0f);
			//if (m_robot.transform.position.y < this.EmperorBot.transform.position.y - 6f)
			//{
			//	m_robot.transform.position = idealPlayerPos;
			//	m_robot.specRigidbody.Reinitialize();
			//	DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(m_robot.specRigidbody.UnitCenter, 1f);
			//}
			////PastCameraUtility.UnlockConversation();
			//GameManager.Instance.MainCameraController.SetManualControl(false, false);
			//GameManager.Instance.MainCameraController.SetManualControl(true, true);
			//GameManager.Instance.MainCameraController.OverridePosition = m_robot.specRigidbody.HitboxPixelCollider.UnitCenter;
			//PlayerController[] players = GameManager.Instance.AllPlayers;
			//for (int i = 0; i < players.Length; i++)
			//{
			//	players[i].CurrentInputState = PlayerInputState.NoInput;
			//}
			//for (int j = 0; j < 10; j++)
			//{
			//	AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CritterIds[UnityEngine.Random.Range(0, this.CritterIds.Length)]);
			//	AIActor.Spawn(orLoadByGuid, m_robot.CenterPosition.ToIntVector2(VectorConversions.Floor) + new IntVector2(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5)), m_robot.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
			//}
			yield return new WaitForSeconds(3f);
			m_robot.QueueSpecificAnimation("select_choose_long");
			yield return new WaitForSeconds(2f);
			//Pixelator.Instance.FreezeFrame();
			BraveTime.RegisterTimeScaleMultiplier(0f, this.gameObject);
			float ela = 0f;
			while (ela < ConvictPastController.FREEZE_FRAME_DURATION)
			{
				ela += GameManager.INVARIANT_DELTA_TIME;
				yield return null;
			}
			//BraveTime.ClearMultiplier(this.gameObject);
			//PastCameraUtility.LockConversation(GameManager.Instance.PrimaryPlayer.CenterPosition);
			//TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
			//Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
			//ttcc.ClearDebris();
			//yield return this.StartCoroutine(ttcc.HandleTimeTubeCredits(GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, false, null, -1, false));
			//AmmonomiconController.Instance.OpenAmmonomicon(true, true);

			this.aiActor.ToggleRenderers(true);
			if (this.specRigidbody)
			{
				this.specRigidbody.enabled = false;
			}
			if (this.aiActor)
			{
				UnityEngine.Object.Destroy(this.aiActor);
			}
			if (this.healthHaver)
			{
				UnityEngine.Object.Destroy(this.healthHaver);
			}
			if (this.behaviorSpeculator)
			{
				UnityEngine.Object.Destroy(this.behaviorSpeculator);
			}
			this.RegenerateCache();

			yield return new WaitForSeconds(2f);

			UnityEngine.Object.Destroy(this.gameObject);

			yield break;
		}
	}
	public class GRandomBossFinalRogueDeathController : BraveBehaviour
	{
		// Token: 0x060058FD RID: 22781 RVA: 0x002148B9 File Offset: 0x00212AB9
		public GRandomBossFinalRogueDeathController()
		{
			this.explosionMidDelay = 0.3f;
			this.explosionCount = 10;
			this.bigExplosionMidDelay = 0.3f;
			this.bigExplosionCount = 10;

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
			base.behaviorSpeculator.enabled = false;
			base.aiActor.BehaviorOverridesVelocity = true;
			base.aiActor.BehaviorVelocity = Vector2.zero;
			base.aiAnimator.PlayUntilCancelled("die", false, null, -1f, false);
			//base.StartCoroutine(this.Drift());
			base.StartCoroutine(this.OnDeathExplosionsCR());
		}

		// Token: 0x06005901 RID: 22785 RVA: 0x00214974 File Offset: 0x00212B74
		private IEnumerator Drift()
		{
			BossFinalRogueController bossController = this.GetComponent<BossFinalRogueController>();
			Vector2 initialLockPos = bossController.CameraPos;
			bossController.EndCameraLock();
			while (this.gameObject)
			{
				//GameManager.Instance.MainCameraController.OverridePosition = initialLockPos;
				this.transform.position = this.transform.position + new Vector3(1f, -1f, 0f) * BraveTime.DeltaTime;
				this.specRigidbody.Reinitialize();
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005902 RID: 22786 RVA: 0x00214990 File Offset: 0x00212B90
		private IEnumerator OnDeathExplosionsCR()
		{
			yield return null;
			//BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
			//if (extantCam)
			//{
			//	extantCam.ForceCancelSequence();
			//}
			//GameManager.Instance.MainCameraController.DoContinuousScreenShake(new ScreenShakeSettings(2f, 20f, 1f, 0f, Vector2.right), this, false);
			//for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
			//{
			//	GameManager.Instance.AllPlayers[k].SetInputOverride("past");
			//}
			PixelCollider collider = this.specRigidbody.HitboxPixelCollider;

			if (this.explosionCount != 0)
			{
				for (int i = 0; i < this.explosionCount; i++)
				{
					Vector2 minPos = collider.UnitBottomLeft;
					Vector2 maxPos = collider.UnitTopRight;
					GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
					Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.5f, 0.5f));
					GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
					tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
					vfxSprite.HeightOffGround = 3f;
					this.sprite.AttachRenderer(vfxSprite);
					this.sprite.UpdateZDepth();
					if (i < this.explosionCount - 1)
					{
						yield return new WaitForSeconds(this.explosionMidDelay);
					}
				}

			}

			if (this.bigExplosionCount != 0)
			{
				for (int j = 0; j < this.bigExplosionCount; j++)
				{
					Vector2 minPos2 = collider.UnitBottomLeft;
					Vector2 maxPos2 = collider.UnitTopRight;
					GameObject vfxPrefab2 = BraveUtility.RandomElement<GameObject>(this.bigExplosionVfx);
					Vector2 pos2 = BraveUtility.RandomVector2(minPos2, maxPos2, new Vector2(1f, 1f));
					GameObject vfxObj2 = SpawnManager.SpawnVFX(vfxPrefab2, pos2, Quaternion.identity);
					tk2dBaseSprite vfxSprite2 = vfxObj2.GetComponent<tk2dBaseSprite>();
					vfxSprite2.HeightOffGround = 3f;
					this.sprite.AttachRenderer(vfxSprite2);
					this.sprite.UpdateZDepth();
					if (j < this.bigExplosionCount - 1)
					{
						yield return new WaitForSeconds(this.bigExplosionMidDelay);
					}
					else if (this.DeathStarExplosionVFX != null)
					{
						GameObject deathStarObj = SpawnManager.SpawnVFX(this.DeathStarExplosionVFX, collider.UnitCenter, Quaternion.identity);
						tk2dBaseSprite deathStarSprite = deathStarObj.GetComponent<tk2dBaseSprite>();
						deathStarSprite.HeightOffGround = 3f;
						this.sprite.AttachRenderer(deathStarSprite);
						this.sprite.UpdateZDepth();
						AkSoundEngine.PostEvent("Play_BOSS_queenship_explode_01", this.gameObject);
						this.sprite.renderer.enabled = false;
						for (int l = 0; l < this.healthHaver.bodySprites.Count; l++)
						{
							if (this.healthHaver.bodySprites[l])
							{
								this.healthHaver.bodySprites[l].renderer.enabled = false;
							}
						}
						yield return new WaitForSeconds(1f);
						//Pixelator.Instance.FadeToColor(2f, Color.white, false, 0f);
						yield return new WaitForSeconds(2f);
						//Pixelator.Instance.FadeToColor(2f, Color.white, true, 1f);
					}
					else
					{
						//Pixelator.Instance.FadeToColor(3f, Color.white, true, 1f);
					}
				}
			}

			//GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
			this.healthHaver.DeathAnimationComplete(null, null);
			UnityEngine.Object.Destroy(this.gameObject);
			//PilotPastController ppc = UnityEngine.Object.FindObjectOfType<PilotPastController>();
			//GameManager.Instance.MainCameraController.SetManualControl(false, true);
			//ppc.OnBossKilled();
			yield break;
		}

		// Token: 0x04005216 RID: 21014
		public List<GameObject> explosionVfx;

		// Token: 0x04005217 RID: 21015
		public float explosionMidDelay;

		// Token: 0x04005218 RID: 21016
		public int explosionCount;

		// Token: 0x04005219 RID: 21017
		[Space(12f)]
		public List<GameObject> bigExplosionVfx;

		// Token: 0x0400521A RID: 21018
		public float bigExplosionMidDelay;

		// Token: 0x0400521B RID: 21019
		public int bigExplosionCount;

		// Token: 0x0400521C RID: 21020
		public GameObject DeathStarExplosionVFX;
	}
	public class GRandomBossFinalConvictDeathController : BraveBehaviour
	{
		// Token: 0x0600587E RID: 22654 RVA: 0x00212A91 File Offset: 0x00210C91
		public void Start()
		{
			base.healthHaver.OnPreDeath += this.OnBossDeath;
		}

		// Token: 0x0600587F RID: 22655 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005880 RID: 22656 RVA: 0x00212AAC File Offset: 0x00210CAC
		private void OnBossDeath(Vector2 dir)
		{
			GameManager.Instance.StartCoroutine(this.OnDeathCR());
			//ConvictPastController convictPastController = UnityEngine.Object.FindObjectOfType<ConvictPastController>();
			//convictPastController.OnBossKilled(base.transform);
		}

		private IEnumerator OnDeathCR()
		{
			//SuperReaperController.PreventShooting = true;
			//foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
			//{
			//	playerController.SetInputOverride("metal gear death");
			//}
			//yield return new WaitForSeconds(2f);
			//Pixelator.Instance.FadeToColor(0.75f, Color.white, false, 0f);
			//Minimap.Instance.TemporarilyPreventMinimap = true;
			//GameUIRoot.Instance.HideCoreUI(string.Empty);
			//GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
			//yield return new WaitForSeconds(3f);
			//MetalGearRatIntroDoer introDoer = this.GetComponent<MetalGearRatIntroDoer>();
			//introDoer.ModifyCamera(false);
			//yield return new WaitForSeconds(0.75f);
			this.aiActor.StealthDeath = true;
			this.healthHaver.persistsOnDeath = true;
			this.healthHaver.DeathAnimationComplete(null, null);
			if (this.aiActor)
			{
				UnityEngine.Object.Destroy(this.aiActor);
			}
			if (this.healthHaver)
			{
				UnityEngine.Object.Destroy(this.healthHaver);
			}
			if (this.behaviorSpeculator)
			{
				UnityEngine.Object.Destroy(this.behaviorSpeculator);
			}
			if (this.aiAnimator.ChildAnimator)
			{
				UnityEngine.Object.Destroy(this.aiAnimator.ChildAnimator.gameObject);
			}
			if (this.aiAnimator)
			{
				UnityEngine.Object.Destroy(this.aiAnimator);
			}
			if (this.specRigidbody)
			{
				UnityEngine.Object.Destroy(this.specRigidbody);
			}
			this.RegenerateCache();
			//MetalGearRatRoomController room = UnityEngine.Object.FindObjectOfType<MetalGearRatRoomController>();
			//room.TransformToDestroyedRoom();
			//GameManager.Instance.PrimaryPlayer.WarpToPoint(room.transform.position + new Vector3(17.25f, 14.5f), false, false);
			//if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer)
			//{
			//	GameManager.Instance.SecondaryPlayer.WarpToPoint(room.transform.position + new Vector3(27.5f, 14.5f), false, false);
			//}
			//this.aiActor.ToggleRenderers(false);
			//GameObject punchoutMinigame = UnityEngine.Object.Instantiate<GameObject>(this.PunchoutMinigamePrefab);
			//PunchoutController punchoutController = punchoutMinigame.GetComponent<PunchoutController>();
			//punchoutController.Init();
			yield return null;
			//foreach (PlayerController playerController2 in GameManager.Instance.AllPlayers)
			//{
			//	playerController2.ClearInputOverride("metal gear death");
			//}
			//Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
			yield return new WaitForSeconds(1f);
			//Minimap.Instance.TemporarilyPreventMinimap = false;
			UnityEngine.Object.Destroy(this.gameObject);
			yield break;
		}

	}
	public class GRandomBossFinalGuideDeathController : BraveBehaviour
	{
		// Token: 0x06005882 RID: 22658 RVA: 0x00212ACB File Offset: 0x00210CCB
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			base.healthHaver.OverrideKillCamTime = new float?(5f);
		}

		// Token: 0x06005883 RID: 22659 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005884 RID: 22660 RVA: 0x00212B08 File Offset: 0x00210D08
		private void OnBossDeath(Vector2 dir)
		{
			base.aiAnimator.ChildAnimator.gameObject.SetActive(false);
			base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
			base.StartCoroutine(this.HandlePostDeathExplosionCR());
			base.healthHaver.OnPreDeath -= this.OnBossDeath;
			GameObject gameObject = GameObject.Find("BossFinalGuide_DrWolf(Clone)");
			if (gameObject)
			{
				HealthHaver component = gameObject.GetComponent<HealthHaver>();
				component.healthIsNumberOfHits = false;
				component.ApplyDamage(10000f, Vector2.zero, "Boss Death", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
			}
		}

		// Token: 0x06005885 RID: 22661 RVA: 0x00212BA8 File Offset: 0x00210DA8
		private IEnumerator HandlePostDeathExplosionCR()
		{
			while (this.aiAnimator.IsPlaying("death"))
			{
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			//Pixelator.Instance.FadeToColor(2f, Color.white, false, 0f);
			this.aiActor.StealthDeath = true;
			this.healthHaver.persistsOnDeath = false;
			this.healthHaver.DeathAnimationComplete(null, null);
			if (this.aiActor)
			{
				UnityEngine.Object.Destroy(this.aiActor);
			}
			if (this.healthHaver)
			{
				UnityEngine.Object.Destroy(this.healthHaver);
			}
			if (this.behaviorSpeculator)
			{
				UnityEngine.Object.Destroy(this.behaviorSpeculator);
			}
			this.RegenerateCache();
			this.specRigidbody.PixelColliders[1].ManualHeight = 32;
			this.specRigidbody.RegenerateColliders = true;
			this.specRigidbody.CollideWithOthers = true;
			//GuidePastController gpc = UnityEngine.Object.FindObjectOfType<GuidePastController>();
			//gpc.OnBossKilled();
			yield break;
		}
	}
	public class GRandomBossFinalMarineDeathController : BraveBehaviour
	{
		// Token: 0x0600589A RID: 22682 RVA: 0x00213298 File Offset: 0x00211498
		public GRandomBossFinalMarineDeathController()
		{
			this.explosionMidDelay = 0.3f;
			this.explosionCount = 10;
			this.bigExplosionMidDelay = 0.3f;
			this.bigExplosionCount = 10;

		}

		// Token: 0x0600589B RID: 22683 RVA: 0x002132C6 File Offset: 0x002114C6
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			base.healthHaver.OverrideKillCamTime = new float?(5f);
		}

		// Token: 0x0600589C RID: 22684 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x0600589D RID: 22685 RVA: 0x00213300 File Offset: 0x00211500
		private void OnBossDeath(Vector2 dir)
		{
			base.behaviorSpeculator.enabled = false;
			base.aiActor.BehaviorOverridesVelocity = true;
			base.aiActor.BehaviorVelocity = Vector2.zero;
			base.aiAnimator.PlayUntilCancelled("die", false, null, -1f, false);
			GameManager.Instance.Dungeon.StartCoroutine(this.OnDeathExplosionsCR());
		}

		// Token: 0x0600589E RID: 22686 RVA: 0x00213364 File Offset: 0x00211564
		private IEnumerator OnDeathExplosionsCR()
		{
			PastLabMarineController plmc = UnityEngine.Object.FindObjectOfType<PastLabMarineController>();
			PixelCollider collider = this.specRigidbody.HitboxPixelCollider;
			for (int i = 0; i < this.explosionCount; i++)
			{
				Vector2 minPos = collider.UnitBottomLeft;
				Vector2 maxPos = collider.UnitTopRight;
				GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
				Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.5f, 0.5f));
				GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
				tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
				vfxSprite.HeightOffGround = 3f;
				this.sprite.AttachRenderer(vfxSprite);
				this.sprite.UpdateZDepth();
				if (i < this.explosionCount - 1)
				{
					yield return new WaitForSeconds(this.explosionMidDelay);
				}
			}
			for (int j = 0; j < this.bigExplosionCount; j++)
			{
				Vector2 minPos2 = collider.UnitBottomLeft;
				Vector2 maxPos2 = collider.UnitTopRight;
				GameObject vfxPrefab2 = BraveUtility.RandomElement<GameObject>(this.bigExplosionVfx);
				Vector2 pos2 = BraveUtility.RandomVector2(minPos2, maxPos2, new Vector2(1f, 1f));
				GameObject vfxObj2 = SpawnManager.SpawnVFX(vfxPrefab2, pos2, Quaternion.identity);
				tk2dBaseSprite vfxSprite2 = vfxObj2.GetComponent<tk2dBaseSprite>();
				vfxSprite2.HeightOffGround = 3f;
				this.sprite.AttachRenderer(vfxSprite2);
				this.sprite.UpdateZDepth();
				if (j < this.bigExplosionCount - 1)
				{
					yield return new WaitForSeconds(this.bigExplosionMidDelay);
				}
			}
			this.healthHaver.DeathAnimationComplete(null, null);
			UnityEngine.Object.Destroy(this.gameObject);
			yield return new WaitForSeconds(2f);
			//Pixelator.Instance.FadeToColor(2f, Color.white, false, 0f);
			//plmc.OnBossKilled();
			yield break;
		}

		// Token: 0x040051B6 RID: 20918
		public List<GameObject> explosionVfx;

		// Token: 0x040051B7 RID: 20919
		public float explosionMidDelay;

		// Token: 0x040051B8 RID: 20920
		public int explosionCount;

		// Token: 0x040051B9 RID: 20921
		[Space(12f)]
		public List<GameObject> bigExplosionVfx;

		// Token: 0x040051BA RID: 20922
		public float bigExplosionMidDelay;

		// Token: 0x040051BB RID: 20923
		public int bigExplosionCount;
	}
	public class GRandomLichDeathController : BraveBehaviour
	{
		// Token: 0x06005BD5 RID: 23509 RVA: 0x00227C27 File Offset: 0x00225E27
		public GRandomLichDeathController()
		{
			this.explosionMidDelay = 0.1f;
			this.explosionCount = 55;
			this.bigExplosionMidDelay = 0.2f;
			this.bigExplosionCount = 15;

		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06005BD6 RID: 23510 RVA: 0x00227C55 File Offset: 0x00225E55
		// (set) Token: 0x06005BD7 RID: 23511 RVA: 0x00227C5D File Offset: 0x00225E5D
		public bool IsDoubleFinalDeath { get; set; }

		// Token: 0x06005BD8 RID: 23512 RVA: 0x00227C68 File Offset: 0x00225E68
		public IEnumerator Start()
		{
			while (Dungeon.IsGenerating)
			{
				yield return null;
			}
			GameManager.Instance.Dungeon.StartCoroutine(this.LateStart());
			yield break;
		}

		// Token: 0x06005BD9 RID: 23513 RVA: 0x00227C84 File Offset: 0x00225E84
		public IEnumerator LateStart()
		{
			yield return null;
			//List<AIActor> allActors = StaticReferenceManager.AllEnemies;
			//for (int i = 0; i < allActors.Count; i++)
			//{
			//	if (allActors[i])
			//	{
			//		MegalichDeathController component = allActors[i].GetComponent<MegalichDeathController>();
			//		if (component)
			//		{
			//			this.m_megalich = component;
			//			break;
			//		}
			//	}
			//}
			//RoomHandler lichRoom = this.aiActor.ParentRoom;
			//RoomHandler megalichRoom = this.m_megalich.aiActor.ParentRoom;
			//megalichRoom.AddDarkSoulsRoomResetDependency(lichRoom);
			this.healthHaver.ManualDeathHandling = true;
			this.healthHaver.OnPreDeath += this.OnBossDeath;
			yield break;
		}

		// Token: 0x06005BDA RID: 23514 RVA: 0x00227C9F File Offset: 0x00225E9F
		protected override void OnDestroy()
		{
			if (ChallengeManager.CHALLENGE_MODE_ACTIVE && this.m_challengesSuppressed)
			{
				ChallengeManager.Instance.SuppressChallengeStart = false;
				this.m_challengesSuppressed = false;
			}
			base.OnDestroy();
		}

		// Token: 0x06005BDB RID: 23515 RVA: 0x00227CD0 File Offset: 0x00225ED0
		private void OnBossDeath(Vector2 dir)
		{
			if (LichIntroDoer.DoubleLich)
			{
				this.IsDoubleFinalDeath = true;
				foreach (AIActor aiactor in base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
				{
					if (aiactor.healthHaver.IsBoss && !aiactor.healthHaver.IsSubboss && aiactor != base.aiActor && aiactor.healthHaver.IsAlive)
					{
						this.IsDoubleFinalDeath = false;
						break;
					}
				}
			}
			if (!LichIntroDoer.DoubleLich || this.IsDoubleFinalDeath)
			{
				AkSoundEngine.PostEvent("Play_MUS_Lich_Transition_01", GameManager.Instance.gameObject);
			}
			if (this.IsDoubleFinalDeath)
			{
				base.aiAnimator.PlayUntilCancelled("death_real", true, null, -1f, false);
				//base.healthHaver.OverrideKillCamTime = new float?(11.5f);
				GameManager.Instance.StartCoroutine(this.HandleDoubleLichPostDeathCR());
				GameManager.Instance.StartCoroutine(this.HandleDoubleLichExtraExplosionsCR());
			}
			else
			{
				base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
				GameManager.Instance.StartCoroutine(this.HandlePostDeathCR());
			}
		}

		// Token: 0x06005BDC RID: 23516 RVA: 0x00227E44 File Offset: 0x00226044
		private IEnumerator HandlePostDeathCR()
		{
			//if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
			//{
			//	ChallengeManager.Instance.ForceStop();
			//}
			tk2dBaseSprite shadowSprite = this.aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
			while (this.aiAnimator.IsPlaying("death"))
			{
				shadowSprite.color = shadowSprite.color.WithAlpha(1f - this.aiAnimator.CurrentClipProgress);
				float progress = this.aiAnimator.CurrentClipProgress;
				if (progress < 0.4f)
				{
					GlobalSparksDoer.DoRandomParticleBurst((int)(200f * Time.deltaTime), this.transform.position + new Vector3(4.5f, 4.5f), this.transform.position + new Vector3(5f, 5.5f), new Vector3(0f, 1f, 0f), 90f, 0.75f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
				}
				yield return null;
			}
			this.renderer.enabled = false;
			shadowSprite.color = shadowSprite.color.WithAlpha(0f);
			this.aiActor.StealthDeath = true;
			this.healthHaver.persistsOnDeath = false;
			this.healthHaver.DeathAnimationComplete(null, null);
			if (this.aiActor)
			{
				UnityEngine.Object.Destroy(this.aiActor);
			}
			if (this.healthHaver)
			{
				UnityEngine.Object.Destroy(this.healthHaver);
			}
			if (this.behaviorSpeculator)
			{
				UnityEngine.Object.Destroy(this.behaviorSpeculator);
			}
			if (this.aiAnimator.ChildAnimator)
			{
				UnityEngine.Object.Destroy(this.aiAnimator.ChildAnimator.gameObject);
			}
			if (this.aiAnimator)
			{
				UnityEngine.Object.Destroy(this.aiAnimator);
			}
			if (this.specRigidbody)
			{
				UnityEngine.Object.Destroy(this.specRigidbody);
			}
			this.RegenerateCache();
			if (LichIntroDoer.DoubleLich)
			{
				yield break;
			}
			//yield return new WaitForSeconds(5f);
			//GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.hellDragScreenShake, this, false);
			//yield return new WaitForSeconds(3f);
			//SuperReaperController.PreventShooting = true;
			//yield return new WaitForSeconds(1f);
			//List<HellDraggerArbitrary> arbitraryGrabbers = new List<HellDraggerArbitrary>();
			//for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			//{
			//	PlayerController playerController = GameManager.Instance.AllPlayers[i];
			//	if (playerController && playerController.healthHaver.IsAlive)
			//	{
			//		playerController.CurrentInputState = PlayerInputState.NoInput;
			//		HellDraggerArbitrary component = UnityEngine.Object.Instantiate<GameObject>(this.HellDragVFX).GetComponent<HellDraggerArbitrary>();
			//		component.Do(playerController);
			//		arbitraryGrabbers.Add(component);
			//	}
			//}
			yield return new WaitForSeconds(1f);
			//GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
			//yield return new WaitForSeconds(1.5f);
			//if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
			//{
			//	ChallengeManager.Instance.SuppressChallengeStart = true;
			//	this.m_challengesSuppressed = true;
			//}
			//AIActor megalich = this.m_megalich.aiActor;
			//RoomHandler megalichRoom = GameManager.Instance.Dungeon.data.rooms.Find((RoomHandler r) => r.GetRoomName() == "LichRoom02");
			//int numPlayers = GameManager.Instance.AllPlayers.Length;
			//megalich.visibilityManager.SuppressPlayerEnteredRoom = true;
			//Pixelator.Instance.FadeToBlack(0.1f, false, 0f);
			//yield return new WaitForSeconds(0.1f);
			//for (int j = 0; j < arbitraryGrabbers.Count; j++)
			//{
			//	UnityEngine.Object.Destroy(arbitraryGrabbers[j].gameObject);
			//}
			//CameraController camera = GameManager.Instance.MainCameraController;
			//camera.SetZoomScaleImmediate(0.75f);
			//camera.LockToRoom = true;
			//for (int k = 0; k < numPlayers; k++)
			//{
			//	GameManager.Instance.AllPlayers[k].SetInputOverride("lich transition");
			//}
			//PlayerController player = GameManager.Instance.PrimaryPlayer;
			//Vector2 targetPoint = megalichRoom.area.basePosition.ToVector2() + new Vector2(19.5f, 5f);
			//if (player)
			//{
			//	player.WarpToPoint(targetPoint, false, false);
			//	player.DoSpinfallSpawn(3f);
			//}
			//if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			//{
			//	PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
			//	if (otherPlayer)
			//	{
			//		otherPlayer.ReuniteWithOtherPlayer(player, false);
			//		otherPlayer.DoSpinfallSpawn(3f);
			//	}
			//}
			//Vector2 idealCameraPosition = megalich.GetComponent<GenericIntroDoer>().BossCenter;
			//camera.SetManualControl(true, false);
			//camera.OverridePosition = idealCameraPosition + new Vector2(0f, 4f);
			//Pixelator.Instance.FadeToBlack(0.5f, true, 0f);
			//float timer = 0f;
			//float duration = 3f;
			//while (timer < duration)
			//{
			//	yield return null;
			//	timer += Time.deltaTime;
			//	camera.OverridePosition = idealCameraPosition + new Vector2(0f, Mathf.SmoothStep(4f, 0f, timer / duration));
			//}
			//yield return new WaitForSeconds(2.5f);
			//for (int l = 0; l < numPlayers; l++)
			//{
			//	GameManager.Instance.AllPlayers[l].ClearInputOverride("lich transition");
			//}
			//if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
			//{
			//	ChallengeManager.Instance.SuppressChallengeStart = false;
			//	this.m_challengesSuppressed = false;
			//	ChallengeManager.Instance.EnteredCombat();
			//}
			//megalich.visibilityManager.ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
			//megalich.GetComponent<GenericIntroDoer>().TriggerSequence(player);
			UnityEngine.Object.Destroy(this.gameObject);
			yield break;
		}

		// Token: 0x06005BDD RID: 23517 RVA: 0x00227E60 File Offset: 0x00226060
		private IEnumerator HandleDoubleLichExtraExplosionsCR()
		{
			PixelCollider collider = this.specRigidbody.HitboxPixelCollider;
			for (int i = 0; i < this.explosionCount; i++)
			{
				Vector2 minPos = collider.UnitBottomLeft;
				Vector2 maxPos = collider.UnitTopRight;
				Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.2f, 0.2f));
				GameObject vfxObj = SpawnManager.SpawnVFX(this.explosionVfx, pos, Quaternion.identity);
				tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
				vfxSprite.HeightOffGround = 0.8f;
				this.sprite.AttachRenderer(vfxSprite);
				this.sprite.UpdateZDepth();
				yield return new WaitForSeconds(this.explosionMidDelay);
			}
			yield break;
		}

		// Token: 0x06005BDE RID: 23518 RVA: 0x00227E7C File Offset: 0x0022607C
		private IEnumerator HandleDoubleLichPostDeathCR()
		{
			//SuperReaperController.PreventShooting = true;
			//GameStatsManager.Instance.SetFlag(GungeonFlags.GUNSLINGER_PAST_KILLED, true);
			//GameStatsManager.Instance.SetFlag(GungeonFlags.GUNSLINGER_UNLOCKED, true);
			//GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.dualLichShake1, this, false);
			yield return new WaitForSeconds((float)this.explosionCount * this.explosionMidDelay - (float)this.bigExplosionCount * this.bigExplosionMidDelay);
			//GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.dualLichShake2, this, false);
			PixelCollider collider = this.specRigidbody.HitboxPixelCollider;
			for (int i = 0; i < this.bigExplosionCount; i++)
			{
				Vector2 minPos = collider.UnitBottomLeft;
				Vector2 maxPos = collider.UnitTopRight;
				Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.2f, 0.2f));
				GameObject vfxObj = SpawnManager.SpawnVFX(this.bigExplosionVfx, pos, Quaternion.identity);
				tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
				vfxSprite.HeightOffGround = 0.8f;
				this.sprite.AttachRenderer(vfxSprite);
				this.sprite.UpdateZDepth();
				yield return new WaitForSeconds(this.bigExplosionMidDelay);
			}
			//Pixelator.Instance.DoMinimap = false;
			//BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
			//if (extantCam)
			//{
			//	extantCam.ForceCancelSequence();
			//}
			yield return new WaitForSeconds(1f);
			Vector2 lichCenter = this.aiAnimator.sprite.WorldCenter;
			//Pixelator.Instance.DoFinalNonFadedLayer = true;
			//GameManager.Instance.MainCameraController.DoScreenShake(1.25f, 8f, 0.5f, 0.75f, null);
			GameObject gameObject = SpawnManager.SpawnVFX(this.bigExplosionVfx, collider.UnitCenter, Quaternion.identity);
			tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
			component.HeightOffGround = 0.8f;
			this.sprite.AttachRenderer(component);
			this.sprite.UpdateZDepth();
			yield return new WaitForSeconds(0.15f);
			this.aiActor.StealthDeath = true;
			this.healthHaver.persistsOnDeath = true;
			this.healthHaver.DeathAnimationComplete(null, null);
			UnityEngine.Object.Destroy(this.gameObject);
			//Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
			yield return new WaitForSeconds(0.15f);
			for (int j = 0; j < StaticReferenceManager.AllDebris.Count; j++)
			{
				if (StaticReferenceManager.AllDebris[j])
				{
					Vector2 flatPoint = StaticReferenceManager.AllDebris[j].transform.position.XY();
					if (GameManager.Instance.MainCameraController.PointIsVisible(flatPoint))
					{
						StaticReferenceManager.AllDebris[j].TriggerDestruction(false);
					}
				}
			}
			//GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
			//TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
			//Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
			yield return new WaitForSeconds(0.4f);
			//yield return GameManager.Instance.StartCoroutine(ttcc.HandleTimeTubeCredits(lichCenter, false, null, -1, true));
			//ttcc.CleanupLich();
			//Pixelator.Instance.DoFinalNonFadedLayer = true;
			//BraveCameraUtility.OverrideAspect = new float?(1.7777778f);
			//yield return GameManager.Instance.StartCoroutine(this.HandlePastBeingShot());
			yield break;
		}

		// Token: 0x06005BDF RID: 23519 RVA: 0x00227E98 File Offset: 0x00226098
		private IEnumerator HandlePastBeingShot()
		{
			Minimap.Instance.TemporarilyPreventMinimap = true;
			Pixelator.Instance.LerpToLetterbox(1f, 2.5f);
			yield return new WaitForSeconds(7.5f);
			TitleDioramaController tdc = UnityEngine.Object.FindObjectOfType<TitleDioramaController>();
			float elapsed = 0f;
			float duration = 10f;
			Transform targetXform = tdc.PastIslandSprite.transform.parent;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				float t = elapsed / duration;
				tdc.SkyRenderer.material.SetFloat("_SkyBoost", Mathf.Lerp(0.88f, 0f, t));
				tdc.SkyRenderer.material.SetColor("_OverrideColor", Color.Lerp(new Color(1f, 0.55f, 0.2f, 1f), new Color(0.05f, 0.08f, 0.15f, 1f), t));
				tdc.SkyRenderer.material.SetFloat("_CurvePower", Mathf.Lerp(0.3f, -0.25f, t));
				tdc.SkyRenderer.material.SetFloat("_DitherCohesionFactor", Mathf.Lerp(0.3f, 1f, t));
				tdc.SkyRenderer.material.SetFloat("_StepValue", Mathf.Lerp(0.2f, 0.01f, t));
				targetXform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0f, -60f, 0f), BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, t));
				yield return null;
			}
			AmmonomiconController.Instance.OpenAmmonomicon(true, true);
			yield break;
		}

		// Token: 0x06005BE0 RID: 23520 RVA: 0x00227EAC File Offset: 0x002260AC
		private IEnumerator HandleSplashBody(PlayerController sourcePlayer, bool isPrimaryPlayer, TitleDioramaController diorama)
		{
			AkSoundEngine.PostEvent("Play_CHR_forever_fall_01", GameManager.Instance.gameObject);
			if (sourcePlayer.healthHaver.IsDead)
			{
				yield break;
			}
			GameObject timefallCorpseInstance = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/TimefallCorpse", ".prefab"), sourcePlayer.sprite.transform.position, Quaternion.identity);
			timefallCorpseInstance.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
			tk2dSpriteAnimator targetTimefallAnimator = timefallCorpseInstance.GetComponent<tk2dSpriteAnimator>();
			SpriteOutlineManager.AddOutlineToSprite(targetTimefallAnimator.Sprite, Color.black);
			tk2dSpriteAnimation timefallSpecificLibrary = (!(sourcePlayer is PlayerSpaceshipController)) ? sourcePlayer.sprite.spriteAnimator.Library : (sourcePlayer as PlayerSpaceshipController).TimefallCorpseLibrary;
			targetTimefallAnimator.Library = timefallSpecificLibrary;
			targetTimefallAnimator.Library = timefallSpecificLibrary;
			tk2dSpriteAnimationClip clip = targetTimefallAnimator.GetClipByName("timefall");
			if (clip != null)
			{
				targetTimefallAnimator.Play("timefall");
			}
			float elapsed = 0f;
			float duration = 3f;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				Vector3 startPoint = diorama.VFX_Splash.transform.position + new Vector3(-8f, 40f, 0f);
				Vector3 endPoint = diorama.VFX_Splash.GetComponent<tk2dBaseSprite>().WorldCenter.ToVector3ZUp(startPoint.z);
				targetTimefallAnimator.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(elapsed / duration));
				timefallCorpseInstance.transform.localScale = Vector3.Lerp(Vector3.one * 1.25f, new Vector3(0.125f, 0.125f, 0.125f), Mathf.Clamp01(elapsed / duration));
				yield return null;
			}
			AkSoundEngine.PostEvent("Play_CHR_final_splash_01", GameManager.Instance.gameObject);
			diorama.VFX_Splash.SetActive(true);
			diorama.VFX_Splash.GetComponent<tk2dSpriteAnimator>().PlayAndDisableObject(string.Empty, null);
			diorama.VFX_Splash.GetComponent<tk2dSprite>().UpdateZDepth();
			UnityEngine.Object.Destroy(timefallCorpseInstance);
			yield break;
		}

		// Token: 0x04005564 RID: 21860
		public GameObject HellDragVFX;

		// Token: 0x04005565 RID: 21861
		public ScreenShakeSettings hellDragScreenShake;

		// Token: 0x04005566 RID: 21862
		public ScreenShakeSettings dualLichShake1;

		// Token: 0x04005567 RID: 21863
		public ScreenShakeSettings dualLichShake2;

		// Token: 0x04005568 RID: 21864
		public GameObject explosionVfx;

		// Token: 0x04005569 RID: 21865
		private float explosionMidDelay;

		// Token: 0x0400556A RID: 21866
		private int explosionCount;

		// Token: 0x0400556B RID: 21867
		public GameObject bigExplosionVfx;

		// Token: 0x0400556C RID: 21868
		private float bigExplosionMidDelay;

		// Token: 0x0400556D RID: 21869
		private int bigExplosionCount;

		// Token: 0x0400556F RID: 21871
		private MegalichDeathController m_megalich;

		// Token: 0x04005570 RID: 21872
		private bool m_challengesSuppressed;
	}
	public class GRandomMegalichDeathController : BraveBehaviour
	{
		// Token: 0x06005C33 RID: 23603 RVA: 0x00229F72 File Offset: 0x00228172
		public GRandomMegalichDeathController()
		{
			this.explosionMidDelay = 0.3f;
			this.explosionCount = 10;

		}

		// Token: 0x06005C34 RID: 23604 RVA: 0x00229F90 File Offset: 0x00228190
		public IEnumerator Start()
		{
			while (Dungeon.IsGenerating)
			{
				yield return null;
			}
			GameManager.Instance.Dungeon.StartCoroutine(this.LateStart());
			yield break;
		}

		// Token: 0x06005C35 RID: 23605 RVA: 0x00229FAC File Offset: 0x002281AC
		public IEnumerator LateStart()
		{
			//yield return null;
			//List<AIActor> allActors = StaticReferenceManager.AllEnemies;
			//for (int i = 0; i < allActors.Count; i++)
			//{
			//	if (allActors[i])
			//	{
			//		InfinilichDeathController component = allActors[i].GetComponent<InfinilichDeathController>();
			//		if (component)
			//		{
			//			this.m_infinilich = component;
			//			break;
			//		}
			//	}
			//}
			RoomHandler megalichRoom = this.aiActor.ParentRoom;
			//RoomHandler infinilichRoom = this.m_infinilich.aiActor.ParentRoom;
			//infinilichRoom.AddDarkSoulsRoomResetDependency(megalichRoom);
			this.healthHaver.ManualDeathHandling = true;
			this.healthHaver.OnPreDeath += this.OnBossDeath;
			this.healthHaver.OverrideKillCamTime = new float?(3.5f);
			yield break;
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x00229FC7 File Offset: 0x002281C7
		protected override void OnDestroy()
		{
			if (ChallengeManager.CHALLENGE_MODE_ACTIVE && this.m_challengesSuppressed)
			{
				ChallengeManager.Instance.SuppressChallengeStart = false;
				this.m_challengesSuppressed = false;
			}
			base.OnDestroy();
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x00229FF8 File Offset: 0x002281F8
		private void OnBossDeath(Vector2 dir)
		{
			base.aiAnimator.PlayUntilCancelled("death", false, null, -1f, false);
			base.aiAnimator.StopVfx("double_pound");
			base.aiAnimator.StopVfx("left_pound");
			base.aiAnimator.StopVfx("right_pound");
			GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());
			GameManager.Instance.StartCoroutine(this.OnDeathCR());
		}

		// Token: 0x06005C38 RID: 23608 RVA: 0x0022A070 File Offset: 0x00228270
		private IEnumerator OnDeathExplosionsCR()
		{
			PixelCollider collider = this.specRigidbody.HitboxPixelCollider;
			for (int i = 0; i < this.explosionCount; i++)
			{
				Vector2 minPos = collider.UnitBottomLeft;
				Vector2 maxPos = collider.UnitTopRight;
				GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
				Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.2f, 0.2f));
				GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
				tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
				vfxSprite.HeightOffGround = 0.8f;
				this.sprite.AttachRenderer(vfxSprite);
				this.sprite.UpdateZDepth();
				yield return new WaitForSeconds(this.explosionMidDelay);
			}
			yield break;
		}

		// Token: 0x06005C39 RID: 23609 RVA: 0x0022A08C File Offset: 0x0022828C
		private bool IsAnyPlayerFalling()
		{
			foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
			{
				if (playerController && playerController.healthHaver.IsAlive && playerController.IsFalling)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005C3A RID: 23610 RVA: 0x0022A0E8 File Offset: 0x002282E8
		private IEnumerator OnDeathCR()
		{
			//if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
			//{
			//	ChallengeManager.Instance.ForceStop();
			//}
			//SuperReaperController.PreventShooting = true;
			//yield return new WaitForSeconds(2f);
			//while (this.IsAnyPlayerFalling())
			//{
			//	yield return null;
			//}
			//Pixelator.Instance.FadeToColor(0.75f, Color.white, false, 0f);
			//Minimap.Instance.TemporarilyPreventMinimap = true;
			//GameUIRoot.Instance.HideCoreUI(string.Empty);
			//GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
			yield return new WaitForSeconds(3f);
			//MegalichIntroDoer introDoer = this.GetComponent<MegalichIntroDoer>();
			//introDoer.ModifyCamera(false);
			//introDoer.BlockPitTiles(false);
			//yield return new WaitForSeconds(0.75f);
			this.aiActor.StealthDeath = true;
			this.healthHaver.persistsOnDeath = false;
			this.healthHaver.DeathAnimationComplete(null, null);
			if (this.aiActor)
			{
				UnityEngine.Object.Destroy(this.aiActor);
			}
			if (this.healthHaver)
			{
				UnityEngine.Object.Destroy(this.healthHaver);
			}
			if (this.behaviorSpeculator)
			{
				UnityEngine.Object.Destroy(this.behaviorSpeculator);
			}
			if (this.aiAnimator.ChildAnimator)
			{
				UnityEngine.Object.Destroy(this.aiAnimator.ChildAnimator.gameObject);
			}
			if (this.aiAnimator)
			{
				UnityEngine.Object.Destroy(this.aiAnimator);
			}
			if (this.specRigidbody)
			{
				UnityEngine.Object.Destroy(this.specRigidbody);
			}
			this.RegenerateCache();
			//Minimap.Instance.TemporarilyPreventMinimap = true;
			//if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
			//{
			//	ChallengeManager.Instance.SuppressChallengeStart = true;
			//	this.m_challengesSuppressed = true;
			//}
			//AIActor infinilich = this.m_infinilich.GetComponent<AIActor>();
			//RoomHandler infinilichRoom = GameManager.Instance.Dungeon.data.rooms.Find((RoomHandler r) => r.GetRoomName() == "LichRoom03");
			//int numPlayers = GameManager.Instance.AllPlayers.Length;
			//infinilich.visibilityManager.SuppressPlayerEnteredRoom = true;
			//for (int i = 0; i < numPlayers; i++)
			//{
			//	GameManager.Instance.AllPlayers[i].SetInputOverride("lich transition");
			//}
			//while (this.IsAnyPlayerFalling())
			//{
			//	yield return null;
			//}
			//yield return new WaitForSeconds(0.1f);
			//TimeTubeCreditsController.AcquireTunnelInstanceInAdvance();
			//TimeTubeCreditsController.AcquirePastDioramaInAdvance();
			//yield return null;
			//PlayerController player = GameManager.Instance.PrimaryPlayer;
			//Vector2 targetPoint = infinilichRoom.area.Center + new Vector2(0f, -5f);
			//if (player)
			//{
			//	player.WarpToPoint(targetPoint, false, false);
			//	player.DoSpinfallSpawn(0.5f);
			//}
			//if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			//{
			//	PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
			//	if (otherPlayer)
			//	{
			//		otherPlayer.ReuniteWithOtherPlayer(player, false);
			//		otherPlayer.DoSpinfallSpawn(0.5f);
			//	}
			//}
			//this.m_infinilich.GetComponent<InfinilichIntroDoer>().ModifyWorld(true);
			//Vector2 idealCameraPosition = infinilich.GetComponent<GenericIntroDoer>().BossCenter;
			//CameraController camera = GameManager.Instance.MainCameraController;
			//camera.SetManualControl(true, false);
			//camera.OverridePosition = idealCameraPosition;
			//Pixelator.Instance.FadeToColor(1f, Color.white, true, 0f);
			//yield return new WaitForSeconds(0.4f);
			////Vector2 center = infinilich.specRigidbody.UnitCenter + new Vector2(0f, 10f);
			//for (int j = 0; j < 150; j++)
			//{
			//	this.SpawnShellCasingAtPosition(center + UnityEngine.Random.insideUnitCircle.Scale(2f, 1f) * 5f);
			//}
			//yield return new WaitForSeconds(2f);
			//for (int k = 0; k < numPlayers; k++)
			//{
			//	GameManager.Instance.AllPlayers[k].ClearInputOverride("lich transition");
			//}
			//if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
			//{
			//	ChallengeManager.Instance.SuppressChallengeStart = false;
			//	this.m_challengesSuppressed = false;
			//	ChallengeManager.Instance.EnteredCombat();
			//}
			//infinilich.visibilityManager.ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
			//Minimap.Instance.TemporarilyPreventMinimap = false;
			//infinilich.GetComponent<GenericIntroDoer>().TriggerSequence(player);
			UnityEngine.Object.Destroy(this.gameObject);
			yield break;
		}

		// Token: 0x06005C3B RID: 23611 RVA: 0x0022A104 File Offset: 0x00228304
		private void SpawnShellCasingAtPosition(Vector3 position)
		{
			if (this.shellCasing != null)
			{
				float num = UnityEngine.Random.Range(-100f, -80f);
				GameObject gameObject = SpawnManager.SpawnDebris(this.shellCasing, position, Quaternion.Euler(0f, 0f, num));
				ShellCasing component = gameObject.GetComponent<ShellCasing>();
				if (component != null)
				{
					component.Trigger();
				}
				DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
				if (component2 != null)
				{
					Vector3 startingForce = BraveMathCollege.DegreesToVector(num, UnityEngine.Random.Range(0.5f, 1f)).ToVector3ZUp(UnityEngine.Random.value * 1.5f + 1f);
					component2.Trigger(startingForce, UnityEngine.Random.Range(8f, 10f), 1f);
				}
			}
		}

		// Token: 0x040055CC RID: 21964
		public List<GameObject> explosionVfx;

		// Token: 0x040055CD RID: 21965
		public float explosionMidDelay;

		// Token: 0x040055CE RID: 21966
		public int explosionCount;

		// Token: 0x040055CF RID: 21967
		public GameObject shellCasing;

		// Token: 0x040055D0 RID: 21968
		//private InfinilichDeathController m_infinilich;

		// Token: 0x040055D1 RID: 21969
		private bool m_challengesSuppressed;
	}
	public class GRandomInfinilichDeathController : BraveBehaviour
	{
		// Token: 0x06005BA1 RID: 23457 RVA: 0x00225C7D File Offset: 0x00223E7D
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			base.healthHaver.SuppressContinuousKillCamBulletDestruction = true;
		}

		// Token: 0x06005BA2 RID: 23458 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005BA3 RID: 23459 RVA: 0x00225CAE File Offset: 0x00223EAE
		private void OnBossDeath(Vector2 dir)
		{
			if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
			{
				ChallengeManager.Instance.ForceStop();
			}
			base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
			GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());
		}

		// Token: 0x06005BA4 RID: 23460 RVA: 0x00225CF0 File Offset: 0x00223EF0
		//protected Vector2 GetTargetClockhairPosition(Vector2 currentClockhairPosition)
		//{
		//	BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(GameManager.Instance.PrimaryPlayer.PlayerIDX);
		//	Vector2 rhs;
		//	if (instanceForPlayer.IsKeyboardAndMouse(false))
		//	{
		//		rhs = GameManager.Instance.MainCameraController.Camera.ScreenToWorldPoint(Input.mousePosition).XY() + new Vector2(0.375f, -0.25f);
		//	}
		//	else
		//	{
		//		rhs = currentClockhairPosition + instanceForPlayer.ActiveActions.Aim.Vector * 10f * BraveTime.DeltaTime;
		//	}
		//	rhs = Vector2.Max(GameManager.Instance.MainCameraController.MinVisiblePoint, rhs);
		//	return Vector2.Min(GameManager.Instance.MainCameraController.MaxVisiblePoint, rhs);
		//}

		// Token: 0x06005BA5 RID: 23461 RVA: 0x00225DB0 File Offset: 0x00223FB0
		//private bool CheckTarget(GameActor target, Transform clockhairTransform)
		//{
		//	Vector2 a = clockhairTransform.position.XY() + new Vector2(-0.375f, 0.25f);
		//	return Vector2.Distance(a, target.CenterPosition + new Vector2(0f, -1.25f)) < 0.875f;
		//}

		// Token: 0x06005BA6 RID: 23462 RVA: 0x00225E04 File Offset: 0x00224004
		//private IEnumerator HandleClockhair(PlayerController interactor)
		//{
		//	GameManager.Instance.PrimaryPlayer.SetInputOverride("past");
		//	if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		//	{
		//		GameManager.Instance.SecondaryPlayer.SetInputOverride("past");
		//	}
		//	GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		//	GameUIRoot.Instance.HideCoreUI(string.Empty);
		//	CameraController camera = GameManager.Instance.MainCameraController;
		//	Vector2 currentCamCenter = camera.OverridePosition;
		//	Vector2 desiredCamCenter = this.aiAnimator.sprite.WorldCenter;
		//	camera.SetManualControl(true, true);
		//	if (Vector2.Distance(currentCamCenter, desiredCamCenter) > 2.5f)
		//	{
		//		camera.OverridePosition = desiredCamCenter;
		//	}
		//	Transform clockhairTransform = ((GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Clockhair", ".prefab"))).transform;
		//	ClockhairController clockhair = clockhairTransform.GetComponent<ClockhairController>();
		//	float elapsed = 0f;
		//	float duration = clockhair.ClockhairInDuration;
		//	Vector3 clockhairTargetPosition = GameManager.Instance.PrimaryPlayer.CenterPosition;
		//	Vector3 clockhairStartPosition = clockhairTargetPosition + new Vector3(-20f, 5f, 0f);
		//	clockhair.renderer.enabled = true;
		//	clockhair.spriteAnimator.alwaysUpdateOffscreen = true;
		//	clockhair.spriteAnimator.Play("clockhair_intro");
		//	clockhair.hourAnimator.Play("hour_hand_intro");
		//	clockhair.minuteAnimator.Play("minute_hand_intro");
		//	clockhair.secondAnimator.Play("second_hand_intro");
		//	while (elapsed < duration)
		//	{
		//		if (GameManager.INVARIANT_DELTA_TIME == 0f)
		//		{
		//			elapsed += 0.05f;
		//		}
		//		elapsed += GameManager.INVARIANT_DELTA_TIME;
		//		float t = elapsed / duration;
		//		float smoothT = Mathf.SmoothStep(0f, 1f, t);
		//		clockhairTargetPosition = this.GetTargetClockhairPosition(clockhairTargetPosition);
		//		Vector3 currentPosition = Vector3.Slerp(clockhairStartPosition, clockhairTargetPosition, smoothT);
		//		clockhairTransform.position = currentPosition.WithZ(0f);
		//		this.UpdateEyes(clockhairTransform.position, false);
		//		if (t > 0.5f)
		//		{
		//			clockhair.renderer.enabled = true;
		//		}
		//		if (t > 0.75f)
		//		{
		//			clockhair.hourAnimator.GetComponent<Renderer>().enabled = true;
		//			clockhair.minuteAnimator.GetComponent<Renderer>().enabled = true;
		//			clockhair.secondAnimator.GetComponent<Renderer>().enabled = true;
		//		}
		//		clockhair.sprite.UpdateZDepth();
		//		yield return null;
		//	}
		//	clockhair.SetMotionType(1f);
		//	BraveInput currentInput = BraveInput.GetInstanceForPlayer(GameManager.Instance.PrimaryPlayer.PlayerIDX);
		//	float shotTargetTime = 0f;
		//	float holdDuration = 4f;
		//	Vector3 lastScreenShakeAmount = Vector3.zero;
		//	Vector3 lastJitterAmount = Vector3.zero;
		//	for (; ; )
		//	{
		//		clockhair.transform.position = clockhair.transform.position - lastJitterAmount;
		//		clockhair.transform.position = this.GetTargetClockhairPosition(clockhair.transform.position.XY());
		//		clockhair.sprite.UpdateZDepth();
		//		bool isTargetingValidTarget = this.CheckTarget(this.aiActor, clockhairTransform);
		//		if (isTargetingValidTarget)
		//		{
		//			clockhair.SetMotionType(-10f);
		//		}
		//		else
		//		{
		//			clockhair.SetMotionType(1f);
		//		}
		//		if ((currentInput.ActiveActions.ShootAction.IsPressed || currentInput.ActiveActions.InteractAction.IsPressed) && isTargetingValidTarget)
		//		{
		//			shotTargetTime += BraveTime.DeltaTime;
		//		}
		//		else
		//		{
		//			shotTargetTime = Mathf.Max(0f, shotTargetTime - BraveTime.DeltaTime * 3f);
		//		}
		//		this.UpdateEyes(clockhair.transform.position, currentInput.ActiveActions.ShootAction.IsPressed || currentInput.ActiveActions.InteractAction.IsPressed);
		//		if ((currentInput.ActiveActions.ShootAction.WasReleased || currentInput.ActiveActions.InteractAction.WasReleased) && isTargetingValidTarget && shotTargetTime > holdDuration && !GameManager.Instance.IsPaused)
		//		{
		//			break;
		//		}
		//		if (shotTargetTime > 0f)
		//		{
		//			lastScreenShakeAmount = camera.DoFrameScreenShake(Mathf.Lerp(0f, 0.5f, shotTargetTime / holdDuration), Mathf.Lerp(3f, 8f, shotTargetTime / holdDuration), Vector3.right, lastScreenShakeAmount, Time.realtimeSinceStartup);
		//			float distortionPower = Mathf.Lerp(0f, 0.35f, shotTargetTime / holdDuration);
		//			float distortRadius = 0.5f;
		//			float edgeRadius = Mathf.Lerp(4f, 7f, shotTargetTime / holdDuration);
		//			clockhair.UpdateDistortion(distortionPower, distortRadius, edgeRadius);
		//			float desatRadiusUV = Mathf.Lerp(2f, 0.25f, shotTargetTime / holdDuration);
		//			clockhair.UpdateDesat(true, desatRadiusUV);
		//			shotTargetTime = Mathf.Min(holdDuration + 0.25f, shotTargetTime + BraveTime.DeltaTime);
		//			float d = Mathf.Lerp(0f, 0.5f, (shotTargetTime - 1f) / (holdDuration - 1f));
		//			Vector3 vector = (UnityEngine.Random.insideUnitCircle * d).ToVector3ZUp(0f);
		//			BraveInput.DoSustainedScreenShakeVibration(shotTargetTime / holdDuration * 0.8f);
		//			clockhair.transform.position = clockhair.transform.position + vector;
		//			lastJitterAmount = vector;
		//			clockhair.SetMotionType(Mathf.Lerp(-10f, -2400f, shotTargetTime / holdDuration));
		//		}
		//		else
		//		{
		//			lastJitterAmount = Vector3.zero;
		//			camera.ClearFrameScreenShake(lastScreenShakeAmount);
		//			lastScreenShakeAmount = Vector3.zero;
		//			clockhair.UpdateDistortion(0f, 0f, 0f);
		//			clockhair.UpdateDesat(false, 0f);
		//			shotTargetTime = 0f;
		//			BraveInput.DoSustainedScreenShakeVibration(0f);
		//		}
		//		yield return null;
		//	}
		//	camera.ClearFrameScreenShake(lastScreenShakeAmount);
		//	lastScreenShakeAmount = Vector3.zero;
		//	BraveInput.DoSustainedScreenShakeVibration(0f);
		//	BraveInput.DoVibrationForAllPlayers(Vibration.Time.Quick, Vibration.Strength.Hard);
		//	clockhair.StartCoroutine(clockhair.WipeoutDistortionAndFade(0.5f));
		//	clockhair.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
		//	Pixelator.Instance.FadeToColor(0.2f, Color.white, true, 0.2f);
		//	Pixelator.Instance.DoRenderGBuffer = false;
		//	clockhair.spriteAnimator.PlayAndDisableRenderer("clockhair_fire");
		//	clockhair.hourAnimator.GetComponent<Renderer>().enabled = false;
		//	clockhair.minuteAnimator.GetComponent<Renderer>().enabled = false;
		//	clockhair.secondAnimator.GetComponent<Renderer>().enabled = false;
		//	yield return null;
		//	yield break;
		//}

		// Token: 0x06005BA7 RID: 23463 RVA: 0x00225E20 File Offset: 0x00224020
		//private void UpdateEyes(Vector2 clockhairPosition, bool isInDanger)
		//{
		//	Vector2 vector = clockhairPosition - this.eyePos.transform.position.XY();
		//	if (isInDanger && vector.magnitude < 7f)
		//	{
		//		if (!base.aiAnimator.IsPlaying("clockhair_target"))
		//		{
		//			base.aiAnimator.PlayUntilCancelled("clockhair_target", false, null, -1f, false);
		//		}
		//		return;
		//	}
		//	base.aiAnimator.LockFacingDirection = true;
		//	base.aiAnimator.FacingDirection = vector.ToAngle();
		//	if (Mathf.Abs(vector.x) < 4f && Mathf.Abs(vector.y) < 5f)
		//	{
		//		if (vector.y > 2f)
		//		{
		//			if (!base.aiAnimator.IsPlaying("clockhair_up"))
		//			{
		//				base.aiAnimator.PlayUntilCancelled("clockhair_up", false, null, -1f, false);
		//			}
		//		}
		//		else if (vector.y < -2f)
		//		{
		//			if (!base.aiAnimator.IsPlaying("clockhair_down"))
		//			{
		//				base.aiAnimator.PlayUntilCancelled("clockhair_down", false, null, -1f, false);
		//			}
		//		}
		//		else if (!base.aiAnimator.IsPlaying("clockhair_mid"))
		//		{
		//			base.aiAnimator.PlayUntilCancelled("clockhair_mid", false, null, -1f, false);
		//		}
		//	}
		//	else if (!base.aiAnimator.IsPlaying("clockhair"))
		//	{
		//		base.aiAnimator.PlayUntilCancelled("clockhair", false, null, -1f, false);
		//	}
		//}

		// Token: 0x06005BA8 RID: 23464 RVA: 0x00225FC4 File Offset: 0x002241C4
		private IEnumerator OnDeathExplosionsCR()
		{
			//SuperReaperController.PreventShooting = true;
			while (this.aiAnimator.IsPlaying("death"))
			{
				yield return null;
			}
			//Pixelator.Instance.DoMinimap = false;
			//BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
			//if (extantCam)
			//{
			//	extantCam.ForceCancelSequence();
			//}
			//Vector2 lichCenter = this.aiAnimator.sprite.WorldCenter;
			//Pixelator.Instance.DoFinalNonFadedLayer = true;
			//yield return this.StartCoroutine(this.HandleClockhair(GameManager.Instance.BestActivePlayer));
			//if (GameManager.Instance.PrimaryPlayer != null)
			//{
			//	//GameStatsManager.Instance.SetCharacterSpecificFlag(CharacterSpecificGungeonFlags.CLEARED_BULLET_HELL, true);
			//	//if (GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Eevee && !GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLINGER_UNLOCKED))
			//	//{
			//	//	GameManager.LastUsedPlayerPrefab = (GameObject)BraveResources.Load("PlayerGunslinger", ".prefab");
			//	//	QuickRestartOptions opts = default(QuickRestartOptions);
			//	//	opts.ChallengeMode = ChallengeModeType.None;
			//	//	opts.GunGame = false;
			//	//	opts.BossRush = false;
			//	//	opts.NumMetas = 0;
			//	//	Material glitchPass = new Material(Shader.Find("Brave/Internal/GlitchUnlit"));
			//	//	Pixelator.Instance.RegisterAdditionalRenderPass(glitchPass);
			//	//	yield return new WaitForSeconds(4f);
			//	//	Pixelator.Instance.FadeToBlack(1f, false, 0f);
			//	//	yield return new WaitForSeconds(1.25f);
			//	//	GameManager.Instance.QuickRestart(opts);
			//	//	yield break;
			//	//}
			//}
			//GameManager.Instance.MainCameraController.DoScreenShake(1.25f, 8f, 0.5f, 0.75f, null);
			GameObject spawnedExplosion = SpawnManager.SpawnVFX(this.finalExplosionVfx, this.specRigidbody.HitboxPixelCollider.UnitCenter + new Vector2(-0.25f, -0.25f), Quaternion.identity);
			tk2dBaseSprite explosionSprite = spawnedExplosion.GetComponent<tk2dSprite>();
			explosionSprite.HeightOffGround = 0.8f;
			this.sprite.AttachRenderer(explosionSprite);
			this.sprite.UpdateZDepth();
			this.aiActor.StealthDeath = true;
			this.healthHaver.persistsOnDeath = false;
			this.healthHaver.DeathAnimationComplete(null, null);
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
			{
				yield return null;
				UnityEngine.Object.Destroy(this.gameObject);
				//GameManager.Instance.MainCameraController.SetManualControl(true, true);
				GameStatsManager.Instance.SetFlag(GungeonFlags.SHERPA_SUPERBOSSRUSH_COMPLETE, true);
				GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_SUPERBOSSRUSH, true);
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.META_CURRENCY, 10f);
				//GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
				//GameUIRoot.Instance.HideCoreUI(string.Empty);
				//for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				//{
				//	GameManager.Instance.AllPlayers[i].SetInputOverride("game complete");
				//}
				//Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
				//AmmonomiconController.Instance.OpenAmmonomicon(true, true);
			}
			else
			{
				UnityEngine.Object.Destroy(this.gameObject);
				//Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
				//if (GameManager.Instance.PrimaryPlayer.IsTemporaryEeveeForUnlock)
				//{
				//	GameStatsManager.Instance.SetFlag(GungeonFlags.FLAG_EEVEE_UNLOCKED, true);
				//}
				//for (int j = 0; j < StaticReferenceManager.AllDebris.Count; j++)
				//{
				//	if (StaticReferenceManager.AllDebris[j])
				//	{
				//		Vector2 flatPoint = StaticReferenceManager.AllDebris[j].transform.position.XY();
				//		if (GameManager.Instance.MainCameraController.PointIsVisible(flatPoint))
				//		{
				//			StaticReferenceManager.AllDebris[j].TriggerDestruction(false);
				//		}
				//	}
				//}
				//TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
				//Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
				yield return new WaitForSeconds(0.4f);
				//StaticReferenceManager.DestroyAllEnemyProjectiles();
				//yield return GameManager.Instance.StartCoroutine(ttcc.HandleTimeTubeCredits(lichCenter, false, null, -1, true));
				//ttcc.CleanupLich();
				//Pixelator.Instance.DoFinalNonFadedLayer = true;
				//BraveCameraUtility.OverrideAspect = new float?(1.7777778f);
				//yield return GameManager.Instance.StartCoroutine(this.HandlePastBeingShot());
			}
			yield break;
		}

		// Token: 0x06005BA9 RID: 23465 RVA: 0x00225FE0 File Offset: 0x002241E0
		//private IEnumerator HandlePastBeingShot()
		//{
		//	Minimap.Instance.TemporarilyPreventMinimap = true;
		//	Pixelator.Instance.LerpToLetterbox(1f, 2.5f);
		//	yield return new WaitForSeconds(3f);
		//	Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0f);
		//	AkSoundEngine.PostEvent("Play_ENV_final_flash_01", GameManager.Instance.gameObject);
		//	yield return new WaitForSeconds(0.15f);
		//	yield return null;
		//	Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.25f);
		//	TitleDioramaController tdc = UnityEngine.Object.FindObjectOfType<TitleDioramaController>();
		//	yield return new WaitForSeconds(1.25f);
		//	if (tdc.VFX_BulletImpact != null)
		//	{
		//		tdc.VFX_BulletImpact.SetActive(true);
		//		tdc.VFX_BulletImpact.GetComponent<tk2dSpriteAnimator>().PlayAndDisableObject(string.Empty, null);
		//		tdc.VFX_BulletImpact.GetComponent<tk2dSprite>().UpdateZDepth();
		//	}
		//	if (tdc.VFX_TrailParticles != null)
		//	{
		//		tdc.VFX_TrailParticles.SetActive(true);
		//		BraveUtility.EnableEmission(tdc.VFX_TrailParticles.GetComponent<ParticleSystem>(), true);
		//	}
		//	AkSoundEngine.PostEvent("Play_ENV_final_shot_01", GameManager.Instance.gameObject);
		//	tdc.PastIslandSprite.SetSprite("marsh_of_gungeon_past_hit_001");
		//	yield return new WaitForSeconds(1.25f);
		//	if (tdc.VFX_Splash != null)
		//	{
		//		yield return GameManager.Instance.StartCoroutine(this.HandleSplashBody(GameManager.Instance.PrimaryPlayer, true, tdc));
		//	}
		//	yield return new WaitForSeconds(2f);
		//	float elapsed = 0f;
		//	float duration = 10f;
		//	Transform targetXform = tdc.PastIslandSprite.transform.parent;
		//	while (elapsed < duration)
		//	{
		//		elapsed += BraveTime.DeltaTime;
		//		float t = elapsed / duration;
		//		tdc.SkyRenderer.material.SetFloat("_SkyBoost", Mathf.Lerp(0.88f, 0f, t));
		//		tdc.SkyRenderer.material.SetColor("_OverrideColor", Color.Lerp(new Color(1f, 0.55f, 0.2f, 1f), new Color(0.05f, 0.08f, 0.15f, 1f), t));
		//		tdc.SkyRenderer.material.SetFloat("_CurvePower", Mathf.Lerp(0.3f, -0.25f, t));
		//		tdc.SkyRenderer.material.SetFloat("_DitherCohesionFactor", Mathf.Lerp(0.3f, 1f, t));
		//		tdc.SkyRenderer.material.SetFloat("_StepValue", Mathf.Lerp(0.2f, 0.01f, t));
		//		targetXform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0f, -60f, 0f), BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, t));
		//		yield return null;
		//	}
		//	AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		//	yield break;
		//}

		// Token: 0x06005BAA RID: 23466 RVA: 0x00225FFC File Offset: 0x002241FC
		//private IEnumerator HandleSplashBody(PlayerController sourcePlayer, bool isPrimaryPlayer, TitleDioramaController diorama)
		//{
		//	AkSoundEngine.PostEvent("Play_CHR_forever_fall_01", GameManager.Instance.gameObject);
		//	if (sourcePlayer.healthHaver.IsDead)
		//	{
		//		yield break;
		//	}
		//	GameObject timefallCorpseInstance = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/TimefallCorpse", ".prefab"), sourcePlayer.sprite.transform.position, Quaternion.identity);
		//	timefallCorpseInstance.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
		//	tk2dSpriteAnimator targetTimefallAnimator = timefallCorpseInstance.GetComponent<tk2dSpriteAnimator>();
		//	SpriteOutlineManager.AddOutlineToSprite(targetTimefallAnimator.Sprite, Color.black);
		//	tk2dSpriteAnimation timefallSpecificLibrary = (!(sourcePlayer is PlayerSpaceshipController)) ? sourcePlayer.sprite.spriteAnimator.Library : (sourcePlayer as PlayerSpaceshipController).TimefallCorpseLibrary;
		//	targetTimefallAnimator.Library = timefallSpecificLibrary;
		//	targetTimefallAnimator.Library = timefallSpecificLibrary;
		//	tk2dSpriteAnimationClip clip = targetTimefallAnimator.GetClipByName("timefall");
		//	if (clip != null)
		//	{
		//		targetTimefallAnimator.Play("timefall");
		//	}
		//	float elapsed = 0f;
		//	float duration = 3f;
		//	while (elapsed < duration)
		//	{
		//		elapsed += BraveTime.DeltaTime;
		//		Vector3 startPoint = diorama.VFX_Splash.transform.position + new Vector3(-8f, 40f, 0f);
		//		Vector3 endPoint = diorama.VFX_Splash.GetComponent<tk2dBaseSprite>().WorldCenter.ToVector3ZUp(startPoint.z);
		//		targetTimefallAnimator.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(elapsed / duration));
		//		timefallCorpseInstance.transform.localScale = Vector3.Lerp(Vector3.one * 1.25f, new Vector3(0.125f, 0.125f, 0.125f), Mathf.Clamp01(elapsed / duration));
		//		yield return null;
		//	}
		//	AkSoundEngine.PostEvent("Play_CHR_final_splash_01", GameManager.Instance.gameObject);
		//	diorama.VFX_Splash.SetActive(true);
		//	diorama.VFX_Splash.GetComponent<tk2dSpriteAnimator>().PlayAndDisableObject(string.Empty, null);
		//	diorama.VFX_Splash.GetComponent<tk2dSprite>().UpdateZDepth();
		//	UnityEngine.Object.Destroy(timefallCorpseInstance);
		//	yield break;
		//}

		// Token: 0x0400551D RID: 21789
		public GameObject bigExplosionVfx;

		// Token: 0x0400551E RID: 21790
		public GameObject finalExplosionVfx;

		// Token: 0x0400551F RID: 21791
		public GameObject eyePos;

	}
	public class GRandomBunkerBossDeathController : BraveBehaviour
	{
		// Token: 0x060059CD RID: 22989 RVA: 0x00219DD4 File Offset: 0x00217FD4
		public GRandomBunkerBossDeathController()
		{
			this.explosionMidDelay = 0.3f;
			this.explosionCount = 10;
			this.debrisMinForce = 5;
			this.debrisMaxForce = 5;
			this.debrisAngleVariance = 15f;
			this.dustTime = 1f;
			this.dustMidDelay = 0.05f;
			this.shakeMidDelay = 0.1f;
			
		}

		// Token: 0x060059CE RID: 22990 RVA: 0x00219E34 File Offset: 0x00218034
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
		}

		// Token: 0x060059CF RID: 22991 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x060059D0 RID: 22992 RVA: 0x00219E59 File Offset: 0x00218059
		private void OnBossDeath(Vector2 dir)
		{
			base.StartCoroutine(this.OnDeathExplosionsCR());
			base.StartCoroutine(this.OnDeathDebrisCR());
			base.StartCoroutine(this.OnDeathAnimationCR());
		}

		// Token: 0x060059D1 RID: 22993 RVA: 0x00219E84 File Offset: 0x00218084
		private IEnumerator OnDeathExplosionsCR()
		{
			Vector2 minPos = this.specRigidbody.UnitBottomLeft;
			Vector2 maxPos = this.specRigidbody.UnitTopRight;
			for (int i = 0; i < this.explosionCount; i++)
			{
				GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
				Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(1f, 1.5f));
				SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
				yield return new WaitForSeconds(this.explosionMidDelay);
			}
			yield break;
		}

		// Token: 0x060059D2 RID: 22994 RVA: 0x00219EA0 File Offset: 0x002180A0
		private IEnumerator OnDeathDebrisCR()
		{
			Vector2 minPos = this.specRigidbody.UnitBottomLeft;
			Vector2 centerPos = this.specRigidbody.UnitCenter;
			Vector2 maxPos = this.specRigidbody.UnitTopRight;
			for (int i = 0; i < this.debrisCount; i++)
			{
				GameObject shardPrefab = BraveUtility.RandomElement<GameObject>(this.debrisObjects);
				Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(-1.5f, -1.5f));
				GameObject shardObj = SpawnManager.SpawnVFX(shardPrefab, pos, Quaternion.identity);
				if (shardObj)
				{
					shardObj.transform.parent = SpawnManager.Instance.VFX;
					DebrisObject orAddComponent = shardObj.GetOrAddComponent<DebrisObject>();
					if (this.aiActor)
					{
						this.aiActor.sprite.AttachRenderer(orAddComponent.sprite);
					}
					orAddComponent.angularVelocity = Mathf.Sign(UnityEngine.Random.value - 0.5f) * 125f;
					orAddComponent.angularVelocityVariance = 60f;
					orAddComponent.decayOnBounce = 0.5f;
					orAddComponent.bounceCount = 1;
					orAddComponent.canRotate = true;
					float angle = (pos - centerPos).ToAngle() + UnityEngine.Random.Range(-this.debrisAngleVariance, this.debrisAngleVariance);
					Vector2 vector = BraveMathCollege.DegreesToVector(angle, 1f) * (float)UnityEngine.Random.Range(this.debrisMinForce, this.debrisMaxForce);
					Vector3 startingForce = new Vector3(vector.x, (vector.y >= 0f) ? 0f : vector.y, (vector.y <= 0f) ? 0f : vector.y);
					if (orAddComponent.minorBreakable)
					{
						orAddComponent.minorBreakable.enabled = true;
					}
					orAddComponent.Trigger(startingForce, 1f, 1f);
				}
				yield return new WaitForSeconds(this.debrisMidDelay);
			}
			yield break;
		}

		// Token: 0x060059D3 RID: 22995 RVA: 0x00219EBC File Offset: 0x002180BC
		private IEnumerator OnDeathAnimationCR()
		{
			Vector2 minPos = this.specRigidbody.UnitBottomLeft + this.dustOffset;
			Vector2 maxPos = this.specRigidbody.UnitBottomLeft + this.dustOffset + this.dustDimensions;
			yield return new WaitForSeconds(this.deathAnimationDelay);
			this.aiAnimator.PlayUntilFinished(this.deathAnimation, false, null, -1f, false);
			float timer = this.dustTime;
			float intraTimer = 0f;
			float shakeTimer = 0f;
			IntVector2 shakeDir = IntVector2.Right;
			while (timer > 0f)
			{
				while (intraTimer <= 0f)
				{
					GameObject prefab = BraveUtility.RandomElement<GameObject>(this.dustVfx);
					Vector2 v = BraveUtility.RandomVector2(minPos, maxPos);
					GameObject gameObject = SpawnManager.SpawnVFX(prefab, v, Quaternion.identity);
					tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
					if (component)
					{
						this.sprite.AttachRenderer(component);
						component.HeightOffGround = 0.1f;
						component.UpdateZDepth();
					}
					intraTimer += this.dustMidDelay;
				}
				while (shakeTimer <= 0f)
				{

					//this.transform.position += PhysicsEngine.PixelToUnit(shakeDir);
					Vector3 v3 = PhysicsEngine.PixelToUnit(shakeDir);
					this.transform.position += v3;
					shakeDir *= -1;
					shakeTimer += this.shakeMidDelay;
					if (shakeTimer > 0f)
					{
						this.specRigidbody.Reinitialize();
					}
				}
				yield return null;
				timer -= BraveTime.DeltaTime;
				intraTimer -= BraveTime.DeltaTime;
				shakeTimer -= BraveTime.DeltaTime;
			}
			if (shakeDir.x < 0)
			{
				//this.transform.position += PhysicsEngine.PixelToUnit(shakeDir);
				Vector3 v3 = PhysicsEngine.PixelToUnit(shakeDir);
				this.transform.position += v3;
			}
			this.aiAnimator.PlayUntilFinished(this.flagAnimation, false, null, -1f, false);
			this.aiActor.StealthDeath = true;
			this.healthHaver.persistsOnDeath = false;
			this.healthHaver.DeathAnimationComplete(null, null);
			this.specRigidbody.PixelColliders.RemoveAt(1);
			this.specRigidbody.PixelColliders[0].ManualHeight -= 22;
			this.specRigidbody.RegenerateColliders = true;
			this.specRigidbody.Reinitialize();
			if (this.aiActor)
			{
				UnityEngine.Object.Destroy(this.aiActor);
			}
			if (this.healthHaver)
			{
				UnityEngine.Object.Destroy(this.healthHaver);
			}
			if (this.behaviorSpeculator)
			{
				UnityEngine.Object.Destroy(this.behaviorSpeculator);
			}
			this.RegenerateCache();
			yield break;
		}

		// Token: 0x0400531F RID: 21279
		public List<GameObject> explosionVfx;

		// Token: 0x04005320 RID: 21280
		public float explosionMidDelay;

		// Token: 0x04005321 RID: 21281
		public int explosionCount;

		// Token: 0x04005322 RID: 21282
		public List<GameObject> debrisObjects;

		// Token: 0x04005323 RID: 21283
		public float debrisMidDelay;

		// Token: 0x04005324 RID: 21284
		public int debrisCount;

		// Token: 0x04005325 RID: 21285
		public int debrisMinForce;

		// Token: 0x04005326 RID: 21286
		public int debrisMaxForce;

		// Token: 0x04005327 RID: 21287
		public float debrisAngleVariance;

		// Token: 0x04005328 RID: 21288
		public string deathAnimation;

		// Token: 0x04005329 RID: 21289
		public float deathAnimationDelay;

		// Token: 0x0400532A RID: 21290
		public List<GameObject> dustVfx;

		// Token: 0x0400532B RID: 21291
		public float dustTime;

		// Token: 0x0400532C RID: 21292
		public float dustMidDelay;

		// Token: 0x0400532D RID: 21293
		public Vector2 dustOffset;

		// Token: 0x0400532E RID: 21294
		public Vector2 dustDimensions;

		// Token: 0x0400532F RID: 21295
		public float shakeMidDelay;

		// Token: 0x04005330 RID: 21296
		public string flagAnimation;
	}
	public class GRandomResourcefulRatDeathController : BraveBehaviour
	{
		// Token: 0x06005C92 RID: 23698 RVA: 0x0022BEF8 File Offset: 0x0022A0F8
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			base.healthHaver.OverrideKillCamTime = new float?(5f);
			base.healthHaver.TrackDuringDeath = true;
		}

		// Token: 0x06005C93 RID: 23699 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005C94 RID: 23700 RVA: 0x0022BF49 File Offset: 0x0022A149
		private void OnBossDeath(Vector2 dir)
		{
			base.StartCoroutine(this.BossDeathCR());
			base.healthHaver.OnPreDeath -= this.OnBossDeath;
		}

		// Token: 0x06005C95 RID: 23701 RVA: 0x0022BF70 File Offset: 0x0022A170
		private IEnumerator BossDeathCR()
		{
			yield return new WaitForSeconds(0.66f);
			//ResourcefulRatBossRoomController roomController = UnityEngine.Object.FindObjectOfType<ResourcefulRatBossRoomController>();
			//roomController.OpenGrate();
			yield return new WaitForSeconds(0.33f);
			Vector2 target = this.aiActor.ParentRoom.area.UnitBottomLeft + new Vector2(17f, 12f);
			Vector2 toTarget = target - this.specRigidbody.UnitCenter;
			this.aiAnimator.LockFacingDirection = true;
			this.aiAnimator.FacingDirection = toTarget.ToAngle();
			bool ratOnGrate = !this.specRigidbody.UnitCenter.IsWithin(target + new Vector2(-2f, -2f), target + new Vector2(2f, 2f));
			if (ratOnGrate)
			{
				this.aiAnimator.PlayUntilCancelled("move", false, null, -1f, false);
				float moveSpeed = 7f;
				bool hasDove = false;
				Vector2 velocity = toTarget.normalized * moveSpeed;
				float timer = toTarget.magnitude / moveSpeed;
				while (timer > 0f)
				{
					this.specRigidbody.Velocity = velocity;
					timer -= BraveTime.DeltaTime;
					if (!hasDove)
					{
						float magnitude = (target - this.specRigidbody.UnitCenter).magnitude;
						if (magnitude < 2.5f)
						{
							this.aiAnimator.PlayUntilCancelled("dodge", false, null, -1f, false);
							hasDove = true;
						}
					}
					yield return null;
				}
			}
			this.specRigidbody.Velocity = Vector2.zero;
			this.aiAnimator.PlayUntilCancelled("pitfall", false, null, -1f, false);
			yield return new WaitForSeconds(this.aiAnimator.CurrentClipLength);
			//roomController.EnablePitfalls(true);
			//BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
			//if (extantCam)
			//{
			//	extantCam.SetPhaseCountdown(0.5f);
			//}
			this.aiActor.StealthDeath = true;
			this.aiActor.healthHaver.persistsOnDeath = false;
			this.healthHaver.DeathAnimationComplete(null, null);


			UnityEngine.Object.Destroy(this.gameObject);
			yield break;
		}
	}
	public class GRandomBossDoorMimicDeathController : BraveBehaviour
	{
		// Token: 0x06005866 RID: 22630 RVA: 0x00211F37 File Offset: 0x00210137
		public void Start()
		{
			base.healthHaver.OnPreDeath += this.OnBossDeath;
		}

		// Token: 0x06005867 RID: 22631 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005868 RID: 22632 RVA: 0x00211F50 File Offset: 0x00210150
		private void OnBossDeath(Vector2 dir)
		{
			BossDoorMimicIntroDoer component = base.GetComponent<BossDoorMimicIntroDoer>();
			if (component)
			{
				//component.PhantomDoorBlocker.Unseal();
			}
		}
	}
	public class GRandomDemonWallDeathController : BraveBehaviour
	{
		// Token: 0x060059F5 RID: 23029 RVA: 0x0021B064 File Offset: 0x00219264
		public void Start()
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.OnBossDeath;
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		}

		// Token: 0x060059F6 RID: 23030 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x060059F7 RID: 23031 RVA: 0x0021B0BC File Offset: 0x002192BC
		private void OnBossDeath(Vector2 dir)
		{
			IntVector2 intVector = (base.specRigidbody.HitboxPixelCollider.UnitBottomLeft + new Vector2(0f, -1f)).ToIntVector2(VectorConversions.Floor);
			IntVector2 intVector2 = base.specRigidbody.HitboxPixelCollider.UnitTopRight.ToIntVector2(VectorConversions.Ceil);
			DungeonData data = GameManager.Instance.Dungeon.data;
			for (int i = intVector.x; i <= intVector2.x; i++)
			{
				if (i != (intVector2.x + intVector.x) / 2)
				{
					if (i != (intVector2.x + intVector.x) / 2 - 1)
					{
						for (int j = intVector.y; j <= intVector2.y; j++)
						{
							if (data.CheckInBoundsAndValid(new IntVector2(i, j)))
							{
								CellData cellData = data[i, j];
								if (cellData.type == CellType.FLOOR)
								{
									cellData.isOccupied = true;
								}
							}
						}
					}
				}
			}
			base.aiActor.ParentRoom.OverrideBossPedestalLocation = new IntVector2?(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round) + new IntVector2(-1, 7));
			base.StartCoroutine(this.OnDeathAnimationCR());
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x0021B208 File Offset: 0x00219408
		private IEnumerator OnDeathAnimationCR()
		{
			this.m_isDying = true;
			this.aiAnimator.EndAnimation();
			this.deathEyes.SetActive(true);
			tk2dSpriteAnimator deathEyesAnimator = this.deathEyes.GetComponent<tk2dSpriteAnimator>();
			deathEyesAnimator.Play();
			while (deathEyesAnimator.IsPlaying(deathEyesAnimator.DefaultClip))
			{
				yield return null;
			}
			this.deathEyes.SetActive(false);
			this.aiAnimator.PlayUntilCancelled("death", false, null, -1f, false);
			while (this.aiAnimator.IsPlaying("death"))
			{
				yield return null;
			}
			tk2dSpriteAnimator deathOilAnimator = this.deathOil.GetComponent<tk2dSpriteAnimator>();
			while (deathOilAnimator.IsPlaying(deathOilAnimator.DefaultClip))
			{
				yield return null;
			}
			this.deathOil.SetActive(false);
			this.sprite.HeightOffGround = -1.5f;
			this.sprite.UpdateZDepth();
			this.aiActor.StealthDeath = true;
			this.healthHaver.persistsOnDeath = false;
			this.healthHaver.DeathAnimationComplete(null, null);
			if (this.specRigidbody)
			{
				UnityEngine.Object.Destroy(this.specRigidbody);
			}
			if (this.aiActor)
			{
				UnityEngine.Object.Destroy(this.aiActor);
			}
			if (this.healthHaver)
			{
				UnityEngine.Object.Destroy(this.healthHaver);
			}
			if (this.behaviorSpeculator)
			{
				UnityEngine.Object.Destroy(this.behaviorSpeculator);
			}


			if (this.aiAnimator.ChildAnimator)
			{
				UnityEngine.Object.Destroy(this.aiAnimator.ChildAnimator.gameObject);
			}
			if (this.aiAnimator)
			{
				UnityEngine.Object.Destroy(this.aiAnimator);
			}

			this.RegenerateCache();
			this.m_isDying = false;
			yield return new WaitForSeconds(1f);
			UnityEngine.Object.Destroy(this.gameObject);
			yield break;
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x0021B224 File Offset: 0x00219424
		private void AnimationEventTriggered(tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip, int frame)
		{
			if (this.m_isDying && clip.GetFrame(frame).eventInfo == "oil")
			{
				this.deathOil.SetActive(true);
				tk2dSpriteAnimator component = this.deathOil.GetComponent<tk2dSpriteAnimator>();
				component.Play();
			}
		}

		// Token: 0x0400535A RID: 21338
		public GameObject deathEyes;

		// Token: 0x0400535B RID: 21339
		public GameObject deathOil;

		// Token: 0x0400535C RID: 21340
		private bool m_isDying;
	}



}