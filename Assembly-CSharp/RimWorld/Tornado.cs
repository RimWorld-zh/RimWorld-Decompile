using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006BF RID: 1727
	[StaticConstructorOnStartup]
	public class Tornado : ThingWithComps
	{
		// Token: 0x04001490 RID: 5264
		private Vector2 realPosition;

		// Token: 0x04001491 RID: 5265
		private float direction;

		// Token: 0x04001492 RID: 5266
		private int spawnTick;

		// Token: 0x04001493 RID: 5267
		private int leftFadeOutTicks = -1;

		// Token: 0x04001494 RID: 5268
		private int ticksLeftToDisappear = -1;

		// Token: 0x04001495 RID: 5269
		private Sustainer sustainer;

		// Token: 0x04001496 RID: 5270
		private static MaterialPropertyBlock matPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04001497 RID: 5271
		private static ModuleBase directionNoise;

		// Token: 0x04001498 RID: 5272
		private const float Wind = 5f;

		// Token: 0x04001499 RID: 5273
		private const int CloseDamageIntervalTicks = 15;

		// Token: 0x0400149A RID: 5274
		private const float FarDamageMTBTicks = 15f;

		// Token: 0x0400149B RID: 5275
		private const float CloseDamageRadius = 4.2f;

		// Token: 0x0400149C RID: 5276
		private const float FarDamageRadius = 10f;

		// Token: 0x0400149D RID: 5277
		private const float BaseDamage = 30f;

		// Token: 0x0400149E RID: 5278
		private const int SpawnMoteEveryTicks = 4;

		// Token: 0x0400149F RID: 5279
		private static readonly IntRange DurationTicks = new IntRange(2700, 10080);

		// Token: 0x040014A0 RID: 5280
		private const float DownedPawnDamageFactor = 0.2f;

		// Token: 0x040014A1 RID: 5281
		private const float AnimalPawnDamageFactor = 0.75f;

		// Token: 0x040014A2 RID: 5282
		private const float BuildingDamageFactor = 0.8f;

		// Token: 0x040014A3 RID: 5283
		private const float PlantDamageFactor = 1.7f;

		// Token: 0x040014A4 RID: 5284
		private const float ItemDamageFactor = 0.68f;

		// Token: 0x040014A5 RID: 5285
		private const float CellsPerSecond = 1.7f;

		// Token: 0x040014A6 RID: 5286
		private const float DirectionChangeSpeed = 0.78f;

		// Token: 0x040014A7 RID: 5287
		private const float DirectionNoiseFrequency = 0.002f;

		// Token: 0x040014A8 RID: 5288
		private const float TornadoAnimationSpeed = 25f;

		// Token: 0x040014A9 RID: 5289
		private const float ThreeDimensionalEffectStrength = 4f;

		// Token: 0x040014AA RID: 5290
		private const int FadeInTicks = 120;

		// Token: 0x040014AB RID: 5291
		private const int FadeOutTicks = 120;

		// Token: 0x040014AC RID: 5292
		private const float MaxMidOffset = 2f;

		// Token: 0x040014AD RID: 5293
		private static readonly Material TornadoMaterial = MaterialPool.MatFrom("Things/Ethereal/Tornado", ShaderDatabase.Transparent, MapMaterialRenderQueues.Tornado);

		// Token: 0x040014AE RID: 5294
		private static readonly FloatRange PartsDistanceFromCenter = new FloatRange(1f, 10f);

		// Token: 0x040014AF RID: 5295
		private static readonly float ZOffsetBias = -4f * Tornado.PartsDistanceFromCenter.min;

		// Token: 0x040014B0 RID: 5296
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x0600252A RID: 9514 RVA: 0x0013EFF4 File Offset: 0x0013D3F4
		private float FadeInOutFactor
		{
			get
			{
				float a = Mathf.Clamp01((float)(Find.TickManager.TicksGame - this.spawnTick) / 120f);
				float b = (this.leftFadeOutTicks >= 0) ? Mathf.Min((float)this.leftFadeOutTicks / 120f, 1f) : 1f;
				return Mathf.Min(a, b);
			}
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x0013F05C File Offset: 0x0013D45C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector2>(ref this.realPosition, "realPosition", default(Vector2), false);
			Scribe_Values.Look<float>(ref this.direction, "direction", 0f, false);
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
			Scribe_Values.Look<int>(ref this.leftFadeOutTicks, "leftFadeOutTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksLeftToDisappear, "ticksLeftToDisappear", 0, false);
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x0013F0D8 File Offset: 0x0013D4D8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				Vector3 vector = base.Position.ToVector3Shifted();
				this.realPosition = new Vector2(vector.x, vector.z);
				this.direction = Rand.Range(0f, 360f);
				this.spawnTick = Find.TickManager.TicksGame;
				this.leftFadeOutTicks = -1;
				this.ticksLeftToDisappear = Tornado.DurationTicks.RandomInRange;
			}
			this.CreateSustainer();
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x0013F164 File Offset: 0x0013D564
		public override void Tick()
		{
			if (base.Spawned)
			{
				if (this.sustainer == null)
				{
					Log.Error("Tornado sustainer is null.", false);
					this.CreateSustainer();
				}
				this.sustainer.Maintain();
				this.UpdateSustainerVolume();
				base.GetComp<CompWindSource>().wind = 5f * this.FadeInOutFactor;
				if (this.leftFadeOutTicks > 0)
				{
					this.leftFadeOutTicks--;
					if (this.leftFadeOutTicks == 0)
					{
						this.Destroy(DestroyMode.Vanish);
					}
				}
				else
				{
					if (Tornado.directionNoise == null)
					{
						Tornado.directionNoise = new Perlin(0.0020000000949949026, 2.0, 0.5, 4, 1948573612, QualityMode.Medium);
					}
					this.direction += (float)Tornado.directionNoise.GetValue((double)Find.TickManager.TicksAbs, (double)((float)(this.thingIDNumber % 500) * 1000f), 0.0) * 0.78f;
					this.realPosition = this.realPosition.Moved(this.direction, 0.0283333343f);
					IntVec3 intVec = new Vector3(this.realPosition.x, 0f, this.realPosition.y).ToIntVec3();
					if (intVec.InBounds(base.Map))
					{
						base.Position = intVec;
						if (this.IsHashIntervalTick(15))
						{
							this.DamageCloseThings();
						}
						if (Rand.MTBEventOccurs(15f, 1f, 1f))
						{
							this.DamageFarThings();
						}
						if (this.ticksLeftToDisappear > 0)
						{
							this.ticksLeftToDisappear--;
							if (this.ticksLeftToDisappear == 0)
							{
								this.leftFadeOutTicks = 120;
								Messages.Message("MessageTornadoDissipated".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.PositiveEvent, true);
							}
						}
						if (this.IsHashIntervalTick(4) && !this.CellImmuneToDamage(base.Position))
						{
							float num = Rand.Range(0.6f, 1f);
							MoteMaker.ThrowTornadoDustPuff(new Vector3(this.realPosition.x, 0f, this.realPosition.y)
							{
								y = AltitudeLayer.MoteOverhead.AltitudeFor()
							} + Vector3Utility.RandomHorizontalOffset(1.5f), base.Map, Rand.Range(1.5f, 3f), new Color(num, num, num));
						}
					}
					else
					{
						this.leftFadeOutTicks = 120;
						Messages.Message("MessageTornadoLeftMap".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.PositiveEvent, true);
					}
				}
			}
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x0013F430 File Offset: 0x0013D830
		public override void Draw()
		{
			Rand.PushState();
			Rand.Seed = this.thingIDNumber;
			for (int i = 0; i < 180; i++)
			{
				this.DrawTornadoPart(Tornado.PartsDistanceFromCenter.RandomInRange, Rand.Range(0f, 360f), Rand.Range(0.9f, 1.1f), Rand.Range(0.52f, 0.88f));
			}
			Rand.PopState();
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x0013F4AC File Offset: 0x0013D8AC
		private void DrawTornadoPart(float distanceFromCenter, float initialAngle, float speedMultiplier, float colorMultiplier)
		{
			int ticksGame = Find.TickManager.TicksGame;
			float num = 1f / distanceFromCenter;
			float num2 = 25f * speedMultiplier * num;
			float num3 = (initialAngle + (float)ticksGame * num2) % 360f;
			Vector2 vector = this.realPosition.Moved(num3, this.AdjustedDistanceFromCenter(distanceFromCenter));
			vector.y += distanceFromCenter * 4f;
			vector.y += Tornado.ZOffsetBias;
			Vector3 vector2 = new Vector3(vector.x, AltitudeLayer.Weather.AltitudeFor() + 0.046875f * Rand.Range(0f, 1f), vector.y);
			float num4 = distanceFromCenter * 3f;
			float num5 = 1f;
			if (num3 > 270f)
			{
				num5 = GenMath.LerpDouble(270f, 360f, 0f, 1f, num3);
			}
			else if (num3 > 180f)
			{
				num5 = GenMath.LerpDouble(180f, 270f, 1f, 0f, num3);
			}
			float num6 = Mathf.Min(distanceFromCenter / (Tornado.PartsDistanceFromCenter.max + 2f), 1f);
			float d = Mathf.InverseLerp(0.18f, 0.4f, num6);
			Vector3 a = new Vector3(Mathf.Sin((float)ticksGame / 1000f + (float)(this.thingIDNumber * 10)) * 2f, 0f, 0f);
			vector2 += a * d;
			float a2 = Mathf.Max(1f - num6, 0f) * num5 * this.FadeInOutFactor;
			Color value = new Color(colorMultiplier, colorMultiplier, colorMultiplier, a2);
			Tornado.matPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
			Matrix4x4 matrix = Matrix4x4.TRS(vector2, Quaternion.Euler(0f, num3, 0f), new Vector3(num4, 1f, num4));
			Graphics.DrawMesh(MeshPool.plane10, matrix, Tornado.TornadoMaterial, 0, null, 0, Tornado.matPropertyBlock);
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x0013F6AC File Offset: 0x0013DAAC
		private float AdjustedDistanceFromCenter(float distanceFromCenter)
		{
			float num = Mathf.Min(distanceFromCenter / 8f, 1f);
			num *= num;
			return distanceFromCenter * num;
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x0013F6DA File Offset: 0x0013DADA
		private void UpdateSustainerVolume()
		{
			this.sustainer.info.volumeFactor = this.FadeInOutFactor;
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x0013F6F3 File Offset: 0x0013DAF3
		private void CreateSustainer()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				SoundDef tornado = SoundDefOf.Tornado;
				this.sustainer = tornado.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				this.UpdateSustainerVolume();
			});
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x0013F708 File Offset: 0x0013DB08
		private void DamageCloseThings()
		{
			int num = GenRadial.NumCellsInRadius(4.2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = base.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(base.Map) && !this.CellImmuneToDamage(intVec))
				{
					Pawn firstPawn = intVec.GetFirstPawn(base.Map);
					if (firstPawn == null || !firstPawn.Downed || !Rand.Bool)
					{
						float damageFactor = GenMath.LerpDouble(0f, 4.2f, 1f, 0.2f, intVec.DistanceTo(base.Position));
						this.DoDamage(intVec, damageFactor);
					}
				}
			}
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x0013F7D0 File Offset: 0x0013DBD0
		private void DamageFarThings()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 10f, true)
			where x.InBounds(base.Map)
			select x).RandomElement<IntVec3>();
			if (!this.CellImmuneToDamage(c))
			{
				this.DoDamage(c, 0.5f);
			}
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x0013F824 File Offset: 0x0013DC24
		private bool CellImmuneToDamage(IntVec3 c)
		{
			bool result;
			if (c.Roofed(base.Map) && c.GetRoof(base.Map).isThickRoof)
			{
				result = true;
			}
			else
			{
				Building edifice = c.GetEdifice(base.Map);
				result = (edifice != null && edifice.def.category == ThingCategory.Building && (edifice.def.building.isNaturalRock || (edifice.def == ThingDefOf.Wall && edifice.Faction == null)));
			}
			return result;
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x0013F8C4 File Offset: 0x0013DCC4
		private void DoDamage(IntVec3 c, float damageFactor)
		{
			Tornado.tmpThings.Clear();
			Tornado.tmpThings.AddRange(c.GetThingList(base.Map));
			Vector3 vector = c.ToVector3Shifted();
			Vector2 b = new Vector2(vector.x, vector.z);
			float angle = -this.realPosition.AngleTo(b) + 180f;
			for (int i = 0; i < Tornado.tmpThings.Count; i++)
			{
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
				switch (Tornado.tmpThings[i].def.category)
				{
				case ThingCategory.Pawn:
				{
					Pawn pawn = (Pawn)Tornado.tmpThings[i];
					battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_Tornado, null);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
					if (pawn.RaceProps.baseHealthScale < 1f)
					{
						damageFactor *= pawn.RaceProps.baseHealthScale;
					}
					if (pawn.RaceProps.Animal)
					{
						damageFactor *= 0.75f;
					}
					if (pawn.Downed)
					{
						damageFactor *= 0.2f;
					}
					break;
				}
				case ThingCategory.Item:
					damageFactor *= 0.68f;
					break;
				case ThingCategory.Building:
					damageFactor *= 0.8f;
					break;
				case ThingCategory.Plant:
					damageFactor *= 1.7f;
					break;
				}
				int num = Mathf.Max(GenMath.RoundRandom(30f * damageFactor), 1);
				Tornado.tmpThings[i].TakeDamage(new DamageInfo(DamageDefOf.TornadoScratch, (float)num, angle, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null)).AssociateWithLog(battleLogEntry_DamageTaken);
			}
			Tornado.tmpThings.Clear();
		}
	}
}
