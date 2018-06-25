using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000708 RID: 1800
	public class CompCreatesInfestations : ThingComp
	{
		// Token: 0x040015C9 RID: 5577
		private int lastCreatedInfestationTick = -999999;

		// Token: 0x040015CA RID: 5578
		private const float MinRefireDays = 8f;

		// Token: 0x040015CB RID: 5579
		private const float PreventInfestationsDist = 10f;

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x0600276C RID: 10092 RVA: 0x00152B04 File Offset: 0x00150F04
		public bool CanCreateInfestationNow
		{
			get
			{
				CompDeepDrill comp = this.parent.GetComp<CompDeepDrill>();
				return (comp == null || comp.UsedRecently()) && !this.CantFireBecauseCreatedInfestationRecently && !this.CantFireBecauseSomethingElseCreatedInfestationRecently;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x0600276D RID: 10093 RVA: 0x00152B64 File Offset: 0x00150F64
		public bool CantFireBecauseCreatedInfestationRecently
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastCreatedInfestationTick + 480000;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600276E RID: 10094 RVA: 0x00152B94 File Offset: 0x00150F94
		public bool CantFireBecauseSomethingElseCreatedInfestationRecently
		{
			get
			{
				bool result;
				if (!this.parent.Spawned)
				{
					result = false;
				}
				else
				{
					List<Thing> list = this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.CreatesInfestations);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i] != this.parent)
						{
							if (list[i].Position.InHorDistOf(this.parent.Position, 10f) && list[i].TryGetComp<CompCreatesInfestations>().CantFireBecauseCreatedInfestationRecently)
							{
								return true;
							}
						}
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x00152C50 File Offset: 0x00151050
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastCreatedInfestationTick, "lastCreatedInfestationTick", -999999, false);
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x00152C69 File Offset: 0x00151069
		public void Notify_CreatedInfestation()
		{
			this.lastCreatedInfestationTick = Find.TickManager.TicksGame;
		}
	}
}
