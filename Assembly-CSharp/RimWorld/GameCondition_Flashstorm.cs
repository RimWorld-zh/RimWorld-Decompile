using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200030F RID: 783
	public class GameCondition_Flashstorm : GameCondition
	{
		// Token: 0x04000875 RID: 2165
		private static readonly IntRange AreaRadiusRange = new IntRange(45, 60);

		// Token: 0x04000876 RID: 2166
		private static readonly IntRange TicksBetweenStrikes = new IntRange(320, 800);

		// Token: 0x04000877 RID: 2167
		private const int RainDisableTicksAfterConditionEnds = 30000;

		// Token: 0x04000878 RID: 2168
		public IntVec2 centerLocation;

		// Token: 0x04000879 RID: 2169
		private int areaRadius = 0;

		// Token: 0x0400087A RID: 2170
		private int nextLightningTicks = 0;

		// Token: 0x06000D37 RID: 3383 RVA: 0x0007272C File Offset: 0x00070B2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec2>(ref this.centerLocation, "centerLocation", default(IntVec2), false);
			Scribe_Values.Look<int>(ref this.areaRadius, "areaRadius", 0, false);
			Scribe_Values.Look<int>(ref this.nextLightningTicks, "nextLightningTicks", 0, false);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00072780 File Offset: 0x00070B80
		public override void Init()
		{
			base.Init();
			this.areaRadius = GameCondition_Flashstorm.AreaRadiusRange.RandomInRange;
			this.FindGoodCenterLocation();
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x000727B0 File Offset: 0x00070BB0
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame > this.nextLightningTicks)
			{
				Vector2 vector = Rand.UnitVector2 * Rand.Range(0f, (float)this.areaRadius);
				IntVec3 intVec = new IntVec3((int)Math.Round((double)vector.x) + this.centerLocation.x, 0, (int)Math.Round((double)vector.y) + this.centerLocation.z);
				if (this.IsGoodLocationForStrike(intVec))
				{
					base.SingleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(base.SingleMap, intVec));
					this.nextLightningTicks = Find.TickManager.TicksGame + GameCondition_Flashstorm.TicksBetweenStrikes.RandomInRange;
				}
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0007287A File Offset: 0x00070C7A
		public override void End()
		{
			base.SingleMap.weatherDecider.DisableRainFor(30000);
			base.End();
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00072898 File Offset: 0x00070C98
		private void FindGoodCenterLocation()
		{
			if (base.SingleMap.Size.x <= 16 || base.SingleMap.Size.z <= 16)
			{
				throw new Exception("Map too small for flashstorm.");
			}
			for (int i = 0; i < 10; i++)
			{
				this.centerLocation = new IntVec2(Rand.Range(8, base.SingleMap.Size.x - 8), Rand.Range(8, base.SingleMap.Size.z - 8));
				if (this.IsGoodCenterLocation(this.centerLocation))
				{
					break;
				}
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00072954 File Offset: 0x00070D54
		private bool IsGoodLocationForStrike(IntVec3 loc)
		{
			return loc.InBounds(base.SingleMap) && !loc.Roofed(base.SingleMap) && loc.Standable(base.SingleMap);
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0007299C File Offset: 0x00070D9C
		private bool IsGoodCenterLocation(IntVec2 loc)
		{
			int num = 0;
			int num2 = (int)(3.14159274f * (float)this.areaRadius * (float)this.areaRadius / 2f);
			foreach (IntVec3 loc2 in this.GetPotentiallyAffectedCells(loc))
			{
				if (this.IsGoodLocationForStrike(loc2))
				{
					num++;
				}
				if (num >= num2)
				{
					break;
				}
			}
			return num >= num2;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00072A40 File Offset: 0x00070E40
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
			yield break;
		}
	}
}
