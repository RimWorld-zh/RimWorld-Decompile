using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000761 RID: 1889
	public abstract class CompUseEffect : ThingComp
	{
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x001614FC File Offset: 0x0015F8FC
		public virtual float OrderPriority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060029B5 RID: 10677 RVA: 0x00161518 File Offset: 0x0015F918
		private CompProperties_UseEffect Props
		{
			get
			{
				return (CompProperties_UseEffect)this.props;
			}
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x00161538 File Offset: 0x0015F938
		public virtual void DoEffect(Pawn usedBy)
		{
			if (this.Props.doCameraShake && usedBy.Spawned && usedBy.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.DoShake(1f);
			}
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x00161588 File Offset: 0x0015F988
		public virtual bool SelectedUseOption(Pawn p)
		{
			return false;
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x001615A0 File Offset: 0x0015F9A0
		public virtual bool CanBeUsedBy(Pawn p, out string failReason)
		{
			failReason = null;
			return true;
		}

		// Token: 0x0400169C RID: 5788
		private const float CameraShakeMag = 1f;
	}
}
