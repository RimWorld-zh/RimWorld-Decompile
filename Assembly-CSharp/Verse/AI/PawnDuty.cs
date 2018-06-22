using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A9D RID: 2717
	public class PawnDuty : IExposable
	{
		// Token: 0x06003C88 RID: 15496 RVA: 0x0020039C File Offset: 0x001FE79C
		public PawnDuty()
		{
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x00200414 File Offset: 0x001FE814
		public PawnDuty(DutyDef def)
		{
			this.def = def;
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x00200491 File Offset: 0x001FE891
		public PawnDuty(DutyDef def, LocalTargetInfo focus, float radius = -1f) : this(def)
		{
			this.focus = focus;
			this.radius = radius;
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x002004A9 File Offset: 0x001FE8A9
		public PawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, float radius = -1f) : this(def, focus, radius)
		{
			this.focusSecond = focusSecond;
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x002004C0 File Offset: 0x001FE8C0
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

		// Token: 0x06003C8D RID: 15501 RVA: 0x002005B8 File Offset: 0x001FE9B8
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

		// Token: 0x06003C8E RID: 15502 RVA: 0x0020069C File Offset: 0x001FEA9C
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

		// Token: 0x04002646 RID: 9798
		public DutyDef def = null;

		// Token: 0x04002647 RID: 9799
		public LocalTargetInfo focus = LocalTargetInfo.Invalid;

		// Token: 0x04002648 RID: 9800
		public LocalTargetInfo focusSecond = LocalTargetInfo.Invalid;

		// Token: 0x04002649 RID: 9801
		public float radius = -1f;

		// Token: 0x0400264A RID: 9802
		public LocomotionUrgency locomotion = LocomotionUrgency.None;

		// Token: 0x0400264B RID: 9803
		public Danger maxDanger = Danger.Unspecified;

		// Token: 0x0400264C RID: 9804
		public CellRect spectateRect = default(CellRect);

		// Token: 0x0400264D RID: 9805
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x0400264E RID: 9806
		public bool canDig = false;

		// Token: 0x0400264F RID: 9807
		public PawnsToGather pawnsToGather = PawnsToGather.None;

		// Token: 0x04002650 RID: 9808
		public int transportersGroup = -1;

		// Token: 0x04002651 RID: 9809
		public bool attackDownedIfStarving;
	}
}
