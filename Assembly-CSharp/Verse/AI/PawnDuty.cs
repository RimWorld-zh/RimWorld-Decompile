using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AA0 RID: 2720
	public class PawnDuty : IExposable
	{
		// Token: 0x04002657 RID: 9815
		public DutyDef def = null;

		// Token: 0x04002658 RID: 9816
		public LocalTargetInfo focus = LocalTargetInfo.Invalid;

		// Token: 0x04002659 RID: 9817
		public LocalTargetInfo focusSecond = LocalTargetInfo.Invalid;

		// Token: 0x0400265A RID: 9818
		public float radius = -1f;

		// Token: 0x0400265B RID: 9819
		public LocomotionUrgency locomotion = LocomotionUrgency.None;

		// Token: 0x0400265C RID: 9820
		public Danger maxDanger = Danger.Unspecified;

		// Token: 0x0400265D RID: 9821
		public CellRect spectateRect = default(CellRect);

		// Token: 0x0400265E RID: 9822
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x0400265F RID: 9823
		public bool canDig = false;

		// Token: 0x04002660 RID: 9824
		public PawnsToGather pawnsToGather = PawnsToGather.None;

		// Token: 0x04002661 RID: 9825
		public int transportersGroup = -1;

		// Token: 0x04002662 RID: 9826
		public bool attackDownedIfStarving;

		// Token: 0x06003C8D RID: 15501 RVA: 0x002007F4 File Offset: 0x001FEBF4
		public PawnDuty()
		{
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x0020086C File Offset: 0x001FEC6C
		public PawnDuty(DutyDef def)
		{
			this.def = def;
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x002008E9 File Offset: 0x001FECE9
		public PawnDuty(DutyDef def, LocalTargetInfo focus, float radius = -1f) : this(def)
		{
			this.focus = focus;
			this.radius = radius;
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x00200901 File Offset: 0x001FED01
		public PawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, float radius = -1f) : this(def, focus, radius)
		{
			this.focusSecond = focusSecond;
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x00200918 File Offset: 0x001FED18
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

		// Token: 0x06003C92 RID: 15506 RVA: 0x00200A10 File Offset: 0x001FEE10
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

		// Token: 0x06003C93 RID: 15507 RVA: 0x00200AF4 File Offset: 0x001FEEF4
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
	}
}
