using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066A RID: 1642
	public class Tale_SinglePawn : Tale
	{
		// Token: 0x0600226B RID: 8811 RVA: 0x00124770 File Offset: 0x00122B70
		public Tale_SinglePawn()
		{
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x00124779 File Offset: 0x00122B79
		public Tale_SinglePawn(Pawn pawn)
		{
			this.pawnData = TaleData_Pawn.GenerateFrom(pawn);
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x0600226D RID: 8813 RVA: 0x001247B0 File Offset: 0x00122BB0
		public override Pawn DominantPawn
		{
			get
			{
				return this.pawnData.pawn;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x0600226E RID: 8814 RVA: 0x001247D0 File Offset: 0x00122BD0
		public override string ShortSummary
		{
			get
			{
				return this.def.LabelCap + ": " + this.pawnData.name;
			}
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x00124808 File Offset: 0x00122C08
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.pawnData.pawn == th;
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x0012483A File Offset: 0x00122C3A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.pawnData, "pawnData", new object[0]);
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x0012485C File Offset: 0x00122C5C
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

		// Token: 0x06002272 RID: 8818 RVA: 0x00124886 File Offset: 0x00122C86
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.pawnData = TaleData_Pawn.GenerateRandom();
		}

		// Token: 0x04001385 RID: 4997
		public TaleData_Pawn pawnData;
	}
}
