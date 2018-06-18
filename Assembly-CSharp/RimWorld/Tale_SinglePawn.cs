using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066E RID: 1646
	public class Tale_SinglePawn : Tale
	{
		// Token: 0x06002273 RID: 8819 RVA: 0x00124638 File Offset: 0x00122A38
		public Tale_SinglePawn()
		{
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x00124641 File Offset: 0x00122A41
		public Tale_SinglePawn(Pawn pawn)
		{
			this.pawnData = TaleData_Pawn.GenerateFrom(pawn);
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06002275 RID: 8821 RVA: 0x00124678 File Offset: 0x00122A78
		public override Pawn DominantPawn
		{
			get
			{
				return this.pawnData.pawn;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06002276 RID: 8822 RVA: 0x00124698 File Offset: 0x00122A98
		public override string ShortSummary
		{
			get
			{
				return this.def.LabelCap + ": " + this.pawnData.name;
			}
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x001246D0 File Offset: 0x00122AD0
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.pawnData.pawn == th;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00124702 File Offset: 0x00122B02
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.pawnData, "pawnData", new object[0]);
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x00124724 File Offset: 0x00122B24
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule r in this.pawnData.GetRules("ANYPAWN"))
			{
				yield return r;
			}
			foreach (Rule r2 in this.pawnData.GetRules("PAWN"))
			{
				yield return r2;
			}
			yield break;
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x0012474E File Offset: 0x00122B4E
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.pawnData = TaleData_Pawn.GenerateRandom();
		}

		// Token: 0x04001387 RID: 4999
		public TaleData_Pawn pawnData;
	}
}
