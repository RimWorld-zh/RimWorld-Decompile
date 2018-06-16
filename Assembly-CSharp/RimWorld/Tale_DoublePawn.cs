using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066A RID: 1642
	public class Tale_DoublePawn : Tale
	{
		// Token: 0x0600225A RID: 8794 RVA: 0x001237A1 File Offset: 0x00121BA1
		public Tale_DoublePawn()
		{
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x001237AC File Offset: 0x00121BAC
		public Tale_DoublePawn(Pawn firstPawn, Pawn secondPawn)
		{
			this.firstPawnData = TaleData_Pawn.GenerateFrom(firstPawn);
			if (secondPawn != null)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom(secondPawn);
			}
			if (firstPawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(firstPawn.PositionHeld, firstPawn.MapHeld);
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x00123800 File Offset: 0x00121C00
		public override Pawn DominantPawn
		{
			get
			{
				return this.firstPawnData.pawn;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x0600225D RID: 8797 RVA: 0x00123820 File Offset: 0x00121C20
		public override string ShortSummary
		{
			get
			{
				string text = this.def.LabelCap + ": " + this.firstPawnData.name;
				if (this.secondPawnData != null)
				{
					text = text + ", " + this.secondPawnData.name;
				}
				return text;
			}
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x0012387C File Offset: 0x00121C7C
		public override bool Concerns(Thing th)
		{
			return (this.secondPawnData != null && this.secondPawnData.pawn == th) || base.Concerns(th) || this.firstPawnData.pawn == th;
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x001238D1 File Offset: 0x00121CD1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.firstPawnData, "firstPawnData", new object[0]);
			Scribe_Deep.Look<TaleData_Pawn>(ref this.secondPawnData, "secondPawnData", new object[0]);
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x00123908 File Offset: 0x00121D08
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			if (this.def.firstPawnSymbol.NullOrEmpty() || this.def.secondPawnSymbol.NullOrEmpty())
			{
				Log.Error(this.def + " uses DoublePawn tale class but firstPawnSymbol and secondPawnSymbol are not both set", false);
			}
			foreach (Rule r in this.firstPawnData.GetRules("ANYPAWN"))
			{
				yield return r;
			}
			foreach (Rule r2 in this.firstPawnData.GetRules(this.def.firstPawnSymbol))
			{
				yield return r2;
			}
			if (this.secondPawnData != null)
			{
				foreach (Rule r3 in this.firstPawnData.GetRules("ANYPAWN"))
				{
					yield return r3;
				}
				foreach (Rule r4 in this.secondPawnData.GetRules(this.def.secondPawnSymbol))
				{
					yield return r4;
				}
			}
			yield break;
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x00123932 File Offset: 0x00121D32
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.firstPawnData = TaleData_Pawn.GenerateRandom();
			this.secondPawnData = TaleData_Pawn.GenerateRandom();
		}

		// Token: 0x04001383 RID: 4995
		public TaleData_Pawn firstPawnData;

		// Token: 0x04001384 RID: 4996
		public TaleData_Pawn secondPawnData;
	}
}
