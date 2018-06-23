using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000042 RID: 66
	public class JobDriver_Deconstruct : JobDriver_RemoveBuilding
	{
		// Token: 0x040001D2 RID: 466
		private const int MaxDeconstructWork = 3000;

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0001766C File Offset: 0x00015A6C
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00017688 File Offset: 0x00015A88
		protected override int TotalNeededWork
		{
			get
			{
				Building building = base.Building;
				int value = Mathf.RoundToInt(building.GetStatValue(StatDefOf.WorkToBuild, true));
				return Mathf.Clamp(value, 20, 3000);
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000176C8 File Offset: 0x00015AC8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => base.Building == null || !base.Building.DeconstructibleBy(this.pawn.Faction));
			foreach (Toil t in this.<MakeNewToils>__BaseCallProxy0())
			{
				yield return t;
			}
			yield break;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000176F2 File Offset: 0x00015AF2
		protected override void FinishedRemoving()
		{
			base.Target.Destroy(DestroyMode.Deconstruct);
			this.pawn.records.Increment(RecordDefOf.ThingsDeconstructed);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00017718 File Offset: 0x00015B18
		protected override void TickAction()
		{
			if (base.Building.def.CostListAdjusted(base.Building.Stuff, true).Count > 0)
			{
				this.pawn.skills.Learn(SkillDefOf.Construction, 0.275f, false);
			}
		}
	}
}
