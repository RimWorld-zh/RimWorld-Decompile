using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000668 RID: 1640
	public class Tale_DoublePawn : Tale
	{
		// Token: 0x04001381 RID: 4993
		public TaleData_Pawn firstPawnData;

		// Token: 0x04001382 RID: 4994
		public TaleData_Pawn secondPawnData;

		// Token: 0x06002258 RID: 8792 RVA: 0x00123AA1 File Offset: 0x00121EA1
		public Tale_DoublePawn()
		{
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x00123AAC File Offset: 0x00121EAC
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
		// (get) Token: 0x0600225A RID: 8794 RVA: 0x00123B00 File Offset: 0x00121F00
		public override Pawn DominantPawn
		{
			get
			{
				return this.firstPawnData.pawn;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x0600225B RID: 8795 RVA: 0x00123B20 File Offset: 0x00121F20
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

		// Token: 0x0600225C RID: 8796 RVA: 0x00123B7C File Offset: 0x00121F7C
		public override bool Concerns(Thing th)
		{
			return (this.secondPawnData != null && this.secondPawnData.pawn == th) || base.Concerns(th) || this.firstPawnData.pawn == th;
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00123BD1 File Offset: 0x00121FD1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.firstPawnData, "firstPawnData", new object[0]);
			Scribe_Deep.Look<TaleData_Pawn>(ref this.secondPawnData, "secondPawnData", new object[0]);
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x00123C08 File Offset: 0x00122008
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

		// Token: 0x0600225F RID: 8799 RVA: 0x00123C32 File Offset: 0x00122032
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.firstPawnData = TaleData_Pawn.GenerateRandom();
			this.secondPawnData = TaleData_Pawn.GenerateRandom();
		}
	}
}
