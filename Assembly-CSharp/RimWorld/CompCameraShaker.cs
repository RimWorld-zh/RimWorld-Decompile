using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000706 RID: 1798
	public class CompCameraShaker : ThingComp
	{
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x0600275D RID: 10077 RVA: 0x001525E0 File Offset: 0x001509E0
		public CompProperties_CameraShaker Props
		{
			get
			{
				return (CompProperties_CameraShaker)this.props;
			}
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x00152600 File Offset: 0x00150A00
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
