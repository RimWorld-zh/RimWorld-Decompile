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
		// (get) Token: 0x0600265B RID: 9819 RVA: 0x001493E8 File Offset: 0x001477E8
		// (set) Token: 0x0600265C RID: 9820 RVA: 0x00149413 File Offset: 0x00147813
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

		// Token: 0x0600265D RID: 9821 RVA: 0x00149430 File Offset: 0x00147830
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
