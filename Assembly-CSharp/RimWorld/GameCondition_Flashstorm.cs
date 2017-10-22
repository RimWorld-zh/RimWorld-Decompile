using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GameCondition_Flashstorm : GameCondition
	{
		private const int RainDisableTicksAfterConditionEnds = 30000;

		private static readonly IntRange AreaRadiusRange = new IntRange(45, 60);

		private static readonly IntRange TicksBetweenStrikes = new IntRange(320, 800);

		public IntVec2 centerLocation;

		private int areaRadius;

		private int nextLightningTicks;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec2>(ref this.centerLocation, "centerLocation", default(IntVec2), false);
			Scribe_Values.Look<int>(ref this.areaRadius, "areaRadius", 0, false);
			Scribe_Values.Look<int>(ref this.nextLightningTicks, "nextLightningTicks", 0, false);
		}

		public override void Init()
		{
			base.Init();
			this.areaRadius = GameCondition_Flashstorm.AreaRadiusRange.RandomInRange;
			this.FindGoodCenterLocation();
		}

		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame > this.nextLightningTicks)
			{
				Vector2 a = new Vector2(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f));
				a.Normalize();
				a *= Rand.Range(0f, (float)this.areaRadius);
				IntVec3 intVec = new IntVec3((int)Math.Round((double)a.x) + this.centerLocation.x, 0, (int)Math.Round((double)a.y) + this.centerLocation.z);
				if (this.IsGoodLocationForStrike(intVec))
				{
					base.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(base.Map, intVec));
					this.nextLightningTicks = Find.TickManager.TicksGame + GameCondition_Flashstorm.TicksBetweenStrikes.RandomInRange;
				}
			}
		}

		public override void End()
		{
			base.Map.weatherDecider.DisableRainFor(30000);
			base.End();
		}

		private void FindGoodCenterLocation()
		{
			IntVec3 size = base.Map.Size;
			if (size.x > 16)
			{
				IntVec3 size2 = base.Map.Size;
				if (size2.z <= 16)
					goto IL_0034;
				int num = 0;
				while (num < 10)
				{
					IntVec3 size3 = base.Map.Size;
					int newX = Rand.Range(8, size3.x - 8);
					IntVec3 size4 = base.Map.Size;
					this.centerLocation = new IntVec2(newX, Rand.Range(8, size4.z - 8));
					if (!this.IsGoodCenterLocation(this.centerLocation))
					{
						num++;
						continue;
					}
					break;
				}
				return;
			}
			goto IL_0034;
			IL_0034:
			throw new Exception("Map too small for flashstorm.");
		}

		private bool IsGoodLocationForStrike(IntVec3 loc)
		{
			return loc.InBounds(base.Map) && !loc.Roofed(base.Map) && loc.Standable(base.Map);
		}

		private bool IsGoodCenterLocation(IntVec2 loc)
		{
			int num = 0;
			int num2 = (int)(3.1415927410125732 * (float)this.areaRadius * (float)this.areaRadius / 2.0);
			foreach (IntVec3 potentiallyAffectedCell in this.GetPotentiallyAffectedCells(loc))
			{
				if (this.IsGoodLocationForStrike(potentiallyAffectedCell))
				{
					num++;
				}
				if (num >= num2)
					break;
			}
			return num >= num2;
		}

		private IEnumerable<IntVec3> GetPotentiallyAffectedCells(IntVec2 center)
		{
			for (int x = center.x - this.areaRadius; x <= center.x + this.areaRadius; x++)
			{
				for (int z = center.z - this.areaRadius; z <= center.z + this.areaRadius; z++)
				{
					if ((center.x - x) * (center.x - x) + (center.z - z) * (center.z - z) <= this.areaRadius * this.areaRadius)
					{
						yield return new IntVec3(x, 0, z);
					}
				}
			}
		}
	}
}
