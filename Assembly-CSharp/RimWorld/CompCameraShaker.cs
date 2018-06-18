using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000708 RID: 1800
	public class CompCameraShaker : ThingComp
	{
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x001522EC File Offset: 0x001506EC
		public CompProperties_CameraShaker Props
		{
			get
			{
				return (CompProperties_CameraShaker)this.props;
			}
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x0015230C File Offset: 0x0015070C
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
