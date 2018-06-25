using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075F RID: 1887
	public abstract class CompUseEffect : ThingComp
	{
		// Token: 0x0400169A RID: 5786
		private const float CameraShakeMag = 1f;

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060029B3 RID: 10675 RVA: 0x001618B8 File Offset: 0x0015FCB8
		public virtual float OrderPriority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x001618D4 File Offset: 0x0015FCD4
		private CompProperties_UseEffect Props
		{
			get
			{
				return (CompProperties_UseEffect)this.props;
			}
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x001618F4 File Offset: 0x0015FCF4
		public virtual void DoEffect(Pawn usedBy)
		{
			if (this.Props.doCameraShake && usedBy.Spawned && usedBy.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.DoShake(1f);
			}
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x00161944 File Offset: 0x0015FD44
		public virtual bool SelectedUseOption(Pawn p)
		{
			return false;
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x0016195C File Offset: 0x0015FD5C
		public virtual bool CanBeUsedBy(Pawn p, out string failReason)
		{
			failReason = null;
			return true;
		}
	}
}
