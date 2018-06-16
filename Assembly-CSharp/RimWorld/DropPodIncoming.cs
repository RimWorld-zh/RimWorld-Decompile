using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E4 RID: 1764
	[StaticConstructorOnStartup]
	public class DropPodIncoming : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06002659 RID: 9817 RVA: 0x00149370 File Offset: 0x00147770
		// (set) Token: 0x0600265A RID: 9818 RVA: 0x0014939B File Offset: 0x0014779B
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

		// Token: 0x0600265B RID: 9819 RVA: 0x001493B8 File Offset: 0x001477B8
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
