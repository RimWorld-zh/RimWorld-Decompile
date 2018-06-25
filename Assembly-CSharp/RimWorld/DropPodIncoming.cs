using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E2 RID: 1762
	[StaticConstructorOnStartup]
	public class DropPodIncoming : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06002657 RID: 9815 RVA: 0x001496DC File Offset: 0x00147ADC
		// (set) Token: 0x06002658 RID: 9816 RVA: 0x00149707 File Offset: 0x00147B07
		public ActiveDropPodInfo Contents
		{
			get
			{
				return ((ActiveDropPod)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveDropPod)this.innerContainer[0]).Contents = value;
			}
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x00149724 File Offset: 0x00147B24
		protected override void Impact()
		{
			for (int i = 0; i < 6; i++)
			{
				Vector3 loc = base.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(1f);
				MoteMaker.ThrowDustPuff(loc, base.Map, 1.2f);
			}
			MoteMaker.ThrowLightningGlow(base.Position.ToVector3Shifted(), base.Map, 2f);
			base.Impact();
		}
	}
}
