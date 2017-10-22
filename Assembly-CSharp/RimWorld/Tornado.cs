using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Tornado : ThingWithComps
	{
		private Vector2 realPosition;

		private float direction;

		private int spawnTick;

		private int leftFadeOutTicks = -1;

		private int ticksLeftToDisappear = -1;

		private Sustainer sustainer;

		private static MaterialPropertyBlock matPropertyBlock = new MaterialPropertyBlock();

		private static ModuleBase directionNoise;

		private const float Wind = 5f;

		private const int CloseDamageIntervalTicks = 15;

		private const float FarDamageMTBTicks = 15f;

		private const int CloseDamageRadius = 3;

		private const int FarDamageRadius = 8;

		private const float BaseDamage = 30f;

		private static readonly IntRange DurationTicks = new IntRange(2700, 10080);

		private const float CellsPerSecond = 1.7f;

		private const float DirectionChangeSpeed = 0.78f;

		private const float DirectionNoiseFrequency = 0.002f;

		private const float TornadoAnimationSpeed = 25f;

		private const float ThreeDimensionalEffectStrength = 4f;

		private const int FadeInTicks = 120;

		private const int FadeOutTicks = 120;

		private static readonly Material TornadoMaterial = MaterialPool.MatFrom("Things/Ethereal/Tornado", ShaderDatabase.Transparent);

		private static readonly FloatRange PartsDistanceFromCenter = new FloatRange(1f, 10f);

		private static readonly float ZOffsetBias = (float)(-4.0 * Tornado.PartsDistanceFromCenter.min);

		private static List<Thing> tmpThings = new List<Thing>();

		private float FadeInOutFactor
		{
			get
			{
				float a = Mathf.Clamp01((float)((float)(Find.TickManager.TicksGame - this.spawnTick) / 120.0));
				float b = (float)((this.leftFadeOutTicks >= 0) ? Mathf.Min((float)((float)this.leftFadeOutTicks / 120.0), 1f) : 1.0);
				return Mathf.Min(a, b);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector2>(ref this.realPosition, "realPosition", default(Vector2), false);
			Scribe_Values.Look<float>(ref this.direction, "direction", 0f, false);
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
			Scribe_Values.Look<int>(ref this.leftFadeOutTicks, "leftFadeOutTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksLeftToDisappear, "ticksLeftToDisappear", 0, false);
		}

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

		public override void Tick()
		{
			if (base.Spawned)
			{
				if (this.sustainer == null)
				{
					Log.Error("Tornado sustainer is null.");
					this.CreateSustainer();
				}
				this.sustainer.Maintain();
				this.UpdateSustainerVolume();
				base.GetComp<CompWindSource>().wind = (float)(5.0 * this.FadeInOutFactor);
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
					this.direction += (float)((float)Tornado.directionNoise.GetValue((double)Find.TickManager.TicksAbs, (float)(base.thingIDNumber % 500) * 1000.0, 0.0) * 0.77999997138977051);
					this.realPosition = this.realPosition.Moved(this.direction, 0.0283333343f);
					IntVec3 c = new Vector3(this.realPosition.x, 0f, this.realPosition.y).ToIntVec3();
					if (c.InBounds(base.Map))
					{
						base.Position = new Vector3(this.realPosition.x, 0f, this.realPosition.y).ToIntVec3();
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
								Messages.Message("MessageTornadoDissipated".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.PositiveEvent);
							}
						}
					}
					else
					{
						this.leftFadeOutTicks = 120;
						Messages.Message("MessageTornadoLeftMap".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.PositiveEvent);
					}
				}
			}
		}

		public override void Draw()
		{
			Rand.PushState();
			Rand.Seed = base.thingIDNumber;
			for (int i = 0; i < 150; i++)
			{
				this.DrawTornadoPart(Tornado.PartsDistanceFromCenter.RandomInRange, Rand.Range(0f, 360f), Rand.Range(0.9f, 1.1f), Rand.Range(0.9f, 1f));
			}
			Rand.PopState();
		}

		private void DrawTornadoPart(float distanceFromCenter, float initialAngle, float speedMultiplier, float colorMultiplier)
		{
			int ticksGame = Find.TickManager.TicksGame;
			float num = (float)(1.0 / distanceFromCenter);
			float num2 = (float)(25.0 * speedMultiplier * num);
			float num3 = (float)((initialAngle + (float)ticksGame * num2) % 360.0);
			Vector2 vector = this.realPosition.Moved(num3, this.AdjustedDistanceFromCenter(distanceFromCenter));
			vector.y += (float)(distanceFromCenter * 4.0);
			vector.y += Tornado.ZOffsetBias;
			Vector3 pos = new Vector3(vector.x, Altitudes.AltitudeFor(AltitudeLayer.Weather), vector.y);
			float num4 = (float)(distanceFromCenter * 3.0);
			float num5 = 1f;
			if (num3 > 270.0)
			{
				num5 = GenMath.LerpDouble(270f, 360f, 0f, 1f, num3);
			}
			else if (num3 > 180.0)
			{
				num5 = GenMath.LerpDouble(180f, 270f, 1f, 0f, num3);
			}
			FloatRange partsDistanceFromCenter = Tornado.PartsDistanceFromCenter;
			float num6 = Mathf.Min((float)(distanceFromCenter / (partsDistanceFromCenter.max + 2.0)), 1f);
			float a = Mathf.Max((float)(1.0 - num6), 0f) * num5 * this.FadeInOutFactor;
			Color value = new Color(colorMultiplier, colorMultiplier, colorMultiplier, a);
			Tornado.matPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
			Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0f, num3, 0f), new Vector3(num4, 1f, num4));
			Graphics.DrawMesh(MeshPool.plane10, matrix, Tornado.TornadoMaterial, 0, null, 0, Tornado.matPropertyBlock);
		}

		private float AdjustedDistanceFromCenter(float distanceFromCenter)
		{
			float num = Mathf.Min((float)(distanceFromCenter / 8.0), 1f);
			num *= num;
			return distanceFromCenter * num;
		}

		private void UpdateSustainerVolume()
		{
			this.sustainer.info.volumeFactor = this.FadeInOutFactor;
		}

		private void CreateSustainer()
		{
			LongEventHandler.ExecuteWhenFinished((Action)delegate
			{
				SoundDef soundDef = SoundDef.Named("Tornado");
				this.sustainer = soundDef.TrySpawnSustainer(SoundInfo.InMap((Thing)this, MaintenanceType.PerTick));
				this.UpdateSustainerVolume();
			});
		}

		private void DamageCloseThings()
		{
			int num = GenRadial.NumCellsInRadius(3f);
			for (int num2 = 0; num2 < num; num2++)
			{
				IntVec3 intVec = base.Position + GenRadial.RadialPattern[num2];
				if (intVec.InBounds(base.Map) && !this.CellImmuneToDamage(intVec))
				{
					float damageFactor = GenMath.LerpDouble(0f, 3f, 1f, 0.2f, intVec.DistanceTo(base.Position));
					this.DoDamage(intVec, damageFactor);
				}
			}
		}

		private void DamageFarThings()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 8f, true)
			where x.InBounds(base.Map)
			select x).RandomElement();
			if (!this.CellImmuneToDamage(c))
			{
				this.DoDamage(c, 0.5f);
			}
		}

		private bool CellImmuneToDamage(IntVec3 c)
		{
			return c.Roofed(base.Map) && c.GetRoof(base.Map).isThickRoof;
		}

		private void DoDamage(IntVec3 c, float damageFactor)
		{
			Tornado.tmpThings.Clear();
			Tornado.tmpThings.AddRange(c.GetThingList(base.Map));
			Vector3 vector = c.ToVector3Shifted();
			Vector2 b = new Vector2(vector.x, vector.z);
			float angle = (float)(0.0 - this.realPosition.AngleTo(b) + 180.0);
			for (int i = 0; i < Tornado.tmpThings.Count; i++)
			{
				int amount = Mathf.Max(GenMath.RoundRandom((float)(30.0 * damageFactor)), 1);
				Tornado.tmpThings[i].TakeDamage(new DamageInfo(DamageDefOf.TornadoScratch, amount, angle, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			}
			Tornado.tmpThings.Clear();
		}
	}
}
