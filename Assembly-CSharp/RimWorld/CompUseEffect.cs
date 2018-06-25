using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075F RID: 1887
	public abstract class CompUseEffect : ThingComp
	{
		// Token: 0x0400169E RID: 5790
		private const float CameraShakeMag = 1f;

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060029B2 RID: 10674 RVA: 0x00161B18 File Offset: 0x0015FF18
		public virtual float OrderPriority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060029B3 RID: 10675 RVA: 0x00161B34 File Offset: 0x0015FF34
		private CompProperties_UseEffect Props
		{
			get
			{
				return (CompProperties_UseEffect)this.props;
			}
		}

		// Token: 0x060029B4 RID: 10676 RVA: 0x00161B54 File Offset: 0x0015FF54
		public virtual void DoEffect(Pawn usedBy)
		{
			if (this.Props.doCameraShake && usedBy.Spawned && usedBy.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.DoShake(1f);
			}
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x00161BA4 File Offset: 0x0015FFA4
		public virtual bool SelectedUseOption(Pawn p)
		{
			return false;
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x00161BBC File Offset: 0x0015FFBC
		public virtual bool CanBeUsedBy(Pawn p, out string failReason)
		{
			failReason = null;
			return true;
		}
	}
}
