using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000704 RID: 1796
	public class CompCameraShaker : ThingComp
	{
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06002759 RID: 10073 RVA: 0x00152490 File Offset: 0x00150890
		public CompProperties_CameraShaker Props
		{
			get
			{
				return (CompProperties_CameraShaker)this.props;
			}
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x001524B0 File Offset: 0x001508B0
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned && this.parent.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.SetMinShake(this.Props.mag);
			}
		}
	}
}
