using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AA1 RID: 2721
	public class PawnDuty : IExposable
	{
		// Token: 0x06003C8D RID: 15501 RVA: 0x00200078 File Offset: 0x001FE478
		public PawnDuty()
		{
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x002000F0 File Offset: 0x001FE4F0
		public PawnDuty(DutyDef def)
		{
			this.def = def;
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x0020016D File Offset: 0x001FE56D
		public PawnDuty(DutyDef def, LocalTargetInfo focus, float radius = -1f) : this(def)
		{
			this.focus = focus;
			this.radius = radius;
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x00200185 File Offset: 0x001FE585
		public PawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, float radius = -1f) : this(def, focus, radius)
		{
			this.focusSecond = focusSecond;
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x0020019C File Offset: 0x001FE59C
		public void ExposeData()
		{
			Scribe_Defs.Look<DutyDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.focus, "focus", LocalTargetInfo.Invalid);
			Scribe_TargetInfo.Look(ref this.focusSecond, "focusSecond", LocalTargetInfo.Invalid);
			Scribe_Values.Look<float>(ref this.radius, "radius", -1f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<Danger>(ref this.maxDanger, "maxDanger", Danger.Unspecified, false);
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.All, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<PawnsToGather>(ref this.pawnsToGather, "pawnsToGather", PawnsToGather.None, false);
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", -1, false);
			Scribe_Values.Look<bool>(ref this.attackDownedIfStarving, "attackDownedIfStarving", false, false);
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x00200294 File Offset: 0x001FE694
		public override string ToString()
		{
			string text = (!this.focus.IsValid) ? "" : this.focus.ToString();
			string text2 = (!this.focusSecond.IsValid) ? "" : (", second=" + this.focusSecond.ToString());
			string text3 = (this.radius <= 0f) ? "" : (", rad=" + this.radius.ToString("F2"));
			return string.Concat(new object[]
			{
				"(",
				this.def,
				" ",
				text,
				text2,
				text3,
				")"
			});
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x00200378 File Offset: 0x001FE778
		internal void DrawDebug(Pawn pawn)
		{
			if (this.focus.IsValid)
			{
				GenDraw.DrawLineBetween(pawn.DrawPos, this.focus.Cell.ToVector3Shifted());
				if (this.radius > 0f)
				{
					GenDraw.DrawRadiusRing(this.focus.Cell, this.radius);
				}
			}
		}

		// Token: 0x0400264B RID: 9803
		public DutyDef def = null;

		// Token: 0x0400264C RID: 9804
		public LocalTargetInfo focus = LocalTargetInfo.Invalid;

		// Token: 0x0400264D RID: 9805
		public LocalTargetInfo focusSecond = LocalTargetInfo.Invalid;

		// Token: 0x0400264E RID: 9806
		public float radius = -1f;

		// Token: 0x0400264F RID: 9807
		public LocomotionUrgency locomotion = LocomotionUrgency.None;

		// Token: 0x04002650 RID: 9808
		public Danger maxDanger = Danger.Unspecified;

		// Token: 0x04002651 RID: 9809
		public CellRect spectateRect = default(CellRect);

		// Token: 0x04002652 RID: 9810
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x04002653 RID: 9811
		public bool canDig = false;

		// Token: 0x04002654 RID: 9812
		public PawnsToGather pawnsToGather = PawnsToGather.None;

		// Token: 0x04002655 RID: 9813
		public int transportersGroup = -1;

		// Token: 0x04002656 RID: 9814
		public bool attackDownedIfStarving;
	}
}
