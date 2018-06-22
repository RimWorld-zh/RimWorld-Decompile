using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000706 RID: 1798
	public class CompCreatesInfestations : ThingComp
	{
		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06002769 RID: 10089 RVA: 0x00152754 File Offset: 0x00150B54
		public bool CanCreateInfestationNow
		{
			get
			{
				CompDeepDrill comp = this.parent.GetComp<CompDeepDrill>();
				return (comp == null || comp.UsedRecently()) && !this.CantFireBecauseCreatedInfestationRecently && !this.CantFireBecauseSomethingElseCreatedInfestationRecently;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x0600276A RID: 10090 RVA: 0x001527B4 File Offset: 0x00150BB4
		public bool CantFireBecauseCreatedInfestationRecently
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastCreatedInfestationTick + 480000;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600276B RID: 10091 RVA: 0x001527E4 File Offset: 0x00150BE4
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

		// Token: 0x0600276C RID: 10092 RVA: 0x001528A0 File Offset: 0x00150CA0
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastCreatedInfestationTick, "lastCreatedInfestationTick", -999999, false);
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x001528B9 File Offset: 0x00150CB9
		public void Notify_CreatedInfestation()
		{
			this.lastCreatedInfestationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x040015C5 RID: 5573
		private int lastCreatedInfestationTick = -999999;

		// Token: 0x040015C6 RID: 5574
		private const float MinRefireDays = 8f;

		// Token: 0x040015C7 RID: 5575
		private const float PreventInfestationsDist = 10f;
	}
}
