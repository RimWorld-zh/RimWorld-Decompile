using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020005ED RID: 1517
	public class Caravan_Tweener
	{
		// Token: 0x040011D3 RID: 4563
		private Caravan caravan;

		// Token: 0x040011D4 RID: 4564
		private Vector3 tweenedPos = Vector3.zero;

		// Token: 0x040011D5 RID: 4565
		private Vector3 lastTickSpringPos;

		// Token: 0x040011D6 RID: 4566
		private const float SpringTightness = 0.09f;

		// Token: 0x06001E22 RID: 7714 RVA: 0x00103BD0 File Offset: 0x00101FD0
		public Caravan_Tweener(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001E23 RID: 7715 RVA: 0x00103BEC File Offset: 0x00101FEC
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001E24 RID: 7716 RVA: 0x00103C08 File Offset: 0x00102008
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001E25 RID: 7717 RVA: 0x00103C30 File Offset: 0x00102030
		public Vector3 TweenedPosRoot
		{
			get
			{
				return CaravanTweenerUtility.PatherTweenedPosRoot(this.caravan) + CaravanTweenerUtility.CaravanCollisionPosOffsetFor(this.caravan);
			}
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x00103C60 File Offset: 0x00102060
		public void TweenerTick()
		{
			this.lastTickSpringPos = this.tweenedPos;
			Vector3 a = this.TweenedPosRoot - this.tweenedPos;
			this.tweenedPos += a * 0.09f;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x00103CA8 File Offset: 0x001020A8
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot;
			this.lastTickSpringPos = this.tweenedPos;
		}
	}
}
