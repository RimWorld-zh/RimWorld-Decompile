using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E9 RID: 1513
	public class Caravan_ForageTracker : IExposable
	{
		// Token: 0x040011B1 RID: 4529
		private Caravan caravan;

		// Token: 0x040011B2 RID: 4530
		private float progress;

		// Token: 0x040011B3 RID: 4531
		private const int UpdateProgressIntervalTicks = 10;

		// Token: 0x06001DEE RID: 7662 RVA: 0x00101DA5 File Offset: 0x001001A5
		public Caravan_ForageTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001DEF RID: 7663 RVA: 0x00101DB8 File Offset: 0x001001B8
		public Pair<ThingDef, float> ForagedFoodPerDay
		{
			get
			{
				return ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, null);
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x00101DDC File Offset: 0x001001DC
		public string ForagedFoodPerDayExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x00101E0A File Offset: 0x0010020A
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x00101E23 File Offset: 0x00100223
		public void ForageTrackerTick()
		{
			if (this.caravan.IsHashIntervalTick(10))
			{
				this.UpdateProgressInterval();
			}
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x00101E40 File Offset: 0x00100240
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Forage",
					action = new Action(this.Forage)
				};
			}
			yield break;
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x00101E6C File Offset: 0x0010026C
		private void UpdateProgressInterval()
		{
			float num = 10f * ForagedFoodPerDayCalculator.GetProgressPerTick(this.caravan, null);
			this.progress += num;
			if (this.progress >= 1f)
			{
				this.Forage();
				this.progress = 0f;
			}
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x00101EC0 File Offset: 0x001002C0
		private void Forage()
		{
			ThingDef foragedFood = this.caravan.Biome.foragedFood;
			if (foragedFood != null)
			{
				float foragedFoodCountPerInterval = ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(this.caravan, null);
				int i = GenMath.RoundRandom(foragedFoodCountPerInterval);
				int b = Mathf.FloorToInt((this.caravan.MassCapacity - this.caravan.MassUsage) / foragedFood.GetStatValueAbstract(StatDefOf.Mass, null));
				i = Mathf.Min(i, b);
				while (i > 0)
				{
					Thing thing = ThingMaker.MakeThing(foragedFood, null);
					thing.stackCount = Mathf.Min(i, foragedFood.stackLimit);
					i -= thing.stackCount;
					CaravanInventoryUtility.GiveThing(this.caravan, thing);
				}
			}
		}
	}
}
