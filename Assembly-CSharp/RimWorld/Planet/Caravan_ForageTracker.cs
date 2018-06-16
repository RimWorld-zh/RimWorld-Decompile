using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005EB RID: 1515
	public class Caravan_ForageTracker : IExposable
	{
		// Token: 0x06001DF1 RID: 7665 RVA: 0x00101B89 File Offset: 0x000FFF89
		public Caravan_ForageTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001DF2 RID: 7666 RVA: 0x00101B9C File Offset: 0x000FFF9C
		public Pair<ThingDef, float> ForagedFoodPerDay
		{
			get
			{
				return ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, null);
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001DF3 RID: 7667 RVA: 0x00101BC0 File Offset: 0x000FFFC0
		public string ForagedFoodPerDayExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x00101BEE File Offset: 0x000FFFEE
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x00101C07 File Offset: 0x00100007
		public void ForageTrackerTick()
		{
			if (this.caravan.IsHashIntervalTick(10))
			{
				this.UpdateProgressInterval();
			}
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x00101C24 File Offset: 0x00100024
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

		// Token: 0x06001DF7 RID: 7671 RVA: 0x00101C50 File Offset: 0x00100050
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

		// Token: 0x06001DF8 RID: 7672 RVA: 0x00101CA4 File Offset: 0x001000A4
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

		// Token: 0x040011B4 RID: 4532
		private Caravan caravan;

		// Token: 0x040011B5 RID: 4533
		private float progress;

		// Token: 0x040011B6 RID: 4534
		private const int UpdateProgressIntervalTicks = 10;
	}
}
