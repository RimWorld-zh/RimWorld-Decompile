using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E0 RID: 1760
	[StaticConstructorOnStartup]
	public class DropPodIncoming : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06002653 RID: 9811 RVA: 0x0014958C File Offset: 0x0014798C
		// (set) Token: 0x06002654 RID: 9812 RVA: 0x001495B7 File Offset: 0x001479B7
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

		// Token: 0x06002655 RID: 9813 RVA: 0x001495D4 File Offset: 0x001479D4
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
