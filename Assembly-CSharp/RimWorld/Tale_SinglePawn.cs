using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066C RID: 1644
	public class Tale_SinglePawn : Tale
	{
		// Token: 0x04001385 RID: 4997
		public TaleData_Pawn pawnData;

		// Token: 0x0600226F RID: 8815 RVA: 0x001248C0 File Offset: 0x00122CC0
		public Tale_SinglePawn()
		{
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x001248C9 File Offset: 0x00122CC9
		public Tale_SinglePawn(Pawn pawn)
		{
			this.pawnData = TaleData_Pawn.GenerateFrom(pawn);
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06002271 RID: 8817 RVA: 0x00124900 File Offset: 0x00122D00
		public override Pawn DominantPawn
		{
			get
			{
				return this.pawnData.pawn;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06002272 RID: 8818 RVA: 0x00124920 File Offset: 0x00122D20
		public override string ShortSummary
		{
			get
			{
				return this.def.LabelCap + ": " + this.pawnData.name;
			}
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x00124958 File Offset: 0x00122D58
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.pawnData.pawn == th;
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x0012498A File Offset: 0x00122D8A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.pawnData, "pawnData", new object[0]);
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x001249AC File Offset: 0x00122DAC
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

		// Token: 0x06002276 RID: 8822 RVA: 0x001249D6 File Offset: 0x00122DD6
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.pawnData = TaleData_Pawn.GenerateRandom();
		}
	}
}
