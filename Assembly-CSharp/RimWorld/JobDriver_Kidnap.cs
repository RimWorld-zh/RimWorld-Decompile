using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000071 RID: 113
	public class JobDriver_Kidnap : JobDriver_TakeAndExitMap
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00021A3C File Offset: 0x0001FE3C
		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.Item;
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00021A5C File Offset: 0x0001FE5C
		public override string GetReport()
		{
			string result;
			if (this.Takee == null || this.pawn.HostileTo(this.Takee))
			{
				result = base.GetReport();
			}
			else
			{
				result = JobDefOf.Rescue.reportString.Replace("TargetA", this.Takee.LabelShort);
			}
			return result;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00021AC0 File Offset: 0x0001FEC0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => this.Takee == null || (!this.Takee.Downed && this.Takee.Awake()));
			foreach (Toil t in this.<MakeNewToils>__BaseCallProxy0())
			{
				yield return t;
			}
			yield break;
		}
	}
}
