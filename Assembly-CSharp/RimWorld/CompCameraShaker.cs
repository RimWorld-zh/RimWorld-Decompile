using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000708 RID: 1800
	public class CompCameraShaker : ThingComp
	{
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x0600275F RID: 10079 RVA: 0x00152274 File Offset: 0x00150674
		public CompProperties_CameraShaker Props
		{
			get
			{
				return (CompProperties_CameraShaker)this.props;
			}
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x00152294 File Offset: 0x00150694
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
