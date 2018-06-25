using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066C RID: 1644
	public class Tale_SinglePawn : Tale
	{
		// Token: 0x04001389 RID: 5001
		public TaleData_Pawn pawnData;

		// Token: 0x0600226E RID: 8814 RVA: 0x00124B28 File Offset: 0x00122F28
		public Tale_SinglePawn()
		{
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x00124B31 File Offset: 0x00122F31
		public Tale_SinglePawn(Pawn pawn)
		{
			this.pawnData = TaleData_Pawn.GenerateFrom(pawn);
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06002270 RID: 8816 RVA: 0x00124B68 File Offset: 0x00122F68
		public override Pawn DominantPawn
		{
			get
			{
				return this.pawnData.pawn;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06002271 RID: 8817 RVA: 0x00124B88 File Offset: 0x00122F88
		public override string ShortSummary
		{
			get
			{
				return this.def.LabelCap + ": " + this.pawnData.name;
			}
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x00124BC0 File Offset: 0x00122FC0
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.pawnData.pawn == th;
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x00124BF2 File Offset: 0x00122FF2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.pawnData, "pawnData", new object[0]);
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x00124C14 File Offset: 0x00123014
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

		// Token: 0x06002275 RID: 8821 RVA: 0x00124C3E File Offset: 0x0012303E
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.pawnData = TaleData_Pawn.GenerateRandom();
		}
	}
}
