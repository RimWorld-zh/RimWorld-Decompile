using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200055E RID: 1374
	public class FactionRelation : IExposable
	{
		// Token: 0x04000F37 RID: 3895
		public Faction other = null;

		// Token: 0x04000F38 RID: 3896
		public int goodwill = 100;

		// Token: 0x04000F39 RID: 3897
		public FactionRelationKind kind = FactionRelationKind.Neutral;

		// Token: 0x060019ED RID: 6637 RVA: 0x000E18F4 File Offset: 0x000DFCF4
		public void CheckKindThresholds(Faction faction, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter)
		{
			FactionRelationKind previousKind = this.kind;
			sentLetter = false;
			if (this.kind != FactionRelationKind.Hostile && this.goodwill <= -75)
			{
				this.kind = FactionRelationKind.Hostile;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind != FactionRelationKind.Ally && this.goodwill >= 75)
			{
				this.kind = FactionRelationKind.Ally;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Hostile && this.goodwill >= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Ally && this.goodwill <= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x000E19E0 File Offset: 0x000DFDE0
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.other, "other", false);
			Scribe_Values.Look<int>(ref this.goodwill, "goodwill", 0, false);
			Scribe_Values.Look<FactionRelationKind>(ref this.kind, "kind", FactionRelationKind.Neutral, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				BackCompatibility.FactionRelationLoadingVars(this);
			}
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x000E1A34 File Offset: 0x000DFE34
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.other,
				", goodwill=",
				this.goodwill.ToString("F1"),
				", kind=",
				this.kind,
				")"
			});
		}
	}
}
