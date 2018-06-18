using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070A RID: 1802
	public class CompCreatesInfestations : ThingComp
	{
		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06002771 RID: 10097 RVA: 0x001525B0 File Offset: 0x001509B0
		public bool CanCreateInfestationNow
		{
			get
			{
				CompDeepDrill comp = this.parent.GetComp<CompDeepDrill>();
				return (comp == null || comp.UsedRecently()) && !this.CantFireBecauseCreatedInfestationRecently && !this.CantFireBecauseSomethingElseCreatedInfestationRecently;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06002772 RID: 10098 RVA: 0x00152610 File Offset: 0x00150A10
		public bool CantFireBecauseCreatedInfestationRecently
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastCreatedInfestationTick + 480000;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06002773 RID: 10099 RVA: 0x00152640 File Offset: 0x00150A40
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

		// Token: 0x06002774 RID: 10100 RVA: 0x001526FC File Offset: 0x00150AFC
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastCreatedInfestationTick, "lastCreatedInfestationTick", -999999, false);
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x00152715 File Offset: 0x00150B15
		public void Notify_CreatedInfestation()
		{
			this.lastCreatedInfestationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x040015C7 RID: 5575
		private int lastCreatedInfestationTick = -999999;

		// Token: 0x040015C8 RID: 5576
		private const float MinRefireDays = 8f;

		// Token: 0x040015C9 RID: 5577
		private const float PreventInfestationsDist = 10f;
	}
}
