using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075D RID: 1885
	public abstract class CompUseEffect : ThingComp
	{
		// Token: 0x0400169A RID: 5786
		private const float CameraShakeMag = 1f;

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060029AF RID: 10671 RVA: 0x00161768 File Offset: 0x0015FB68
		public virtual float OrderPriority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060029B0 RID: 10672 RVA: 0x00161784 File Offset: 0x0015FB84
		private CompProperties_UseEffect Props
		{
			get
			{
				return (CompProperties_UseEffect)this.props;
			}
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x001617A4 File Offset: 0x0015FBA4
		public virtual void DoEffect(Pawn usedBy)
		{
			if (this.Props.doCameraShake && usedBy.Spawned && usedBy.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.DoShake(1f);
			}
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x001617F4 File Offset: 0x0015FBF4
		public virtual bool SelectedUseOption(Pawn p)
		{
			return false;
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x0016180C File Offset: 0x0015FC0C
		public virtual bool CanBeUsedBy(Pawn p, out string failReason)
		{
			failReason = null;
			return true;
		}
	}
}
