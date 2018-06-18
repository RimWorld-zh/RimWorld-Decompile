using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000560 RID: 1376
	public class FactionRelation : IExposable
	{
		// Token: 0x060019F3 RID: 6643 RVA: 0x000E14E8 File Offset: 0x000DF8E8
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

		// Token: 0x060019F4 RID: 6644 RVA: 0x000E15D4 File Offset: 0x000DF9D4
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

		// Token: 0x060019F5 RID: 6645 RVA: 0x000E1628 File Offset: 0x000DFA28
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

		// Token: 0x04000F36 RID: 3894
		public Faction other = null;

		// Token: 0x04000F37 RID: 3895
		public int goodwill = 100;

		// Token: 0x04000F38 RID: 3896
		public FactionRelationKind kind = FactionRelationKind.Neutral;
	}
}
