using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020005EB RID: 1515
	public class Caravan_Tweener
	{
		// Token: 0x06001E1F RID: 7711 RVA: 0x00103818 File Offset: 0x00101C18
		public Caravan_Tweener(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001E20 RID: 7712 RVA: 0x00103834 File Offset: 0x00101C34
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001E21 RID: 7713 RVA: 0x00103850 File Offset: 0x00101C50
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001E22 RID: 7714 RVA: 0x00103878 File Offset: 0x00101C78
		public Vector3 TweenedPosRoot
		{
			get
			{
				return CaravanTweenerUtility.PatherTweenedPosRoot(this.caravan) + CaravanTweenerUtility.CaravanCollisionPosOffsetFor(this.caravan);
			}
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x001038A8 File Offset: 0x00101CA8
		public void TweenerTick()
		{
			this.lastTickSpringPos = this.tweenedPos;
			Vector3 a = this.TweenedPosRoot - this.tweenedPos;
			this.tweenedPos += a * 0.09f;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x001038F0 File Offset: 0x00101CF0
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot;
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x040011CF RID: 4559
		private Caravan caravan;

		// Token: 0x040011D0 RID: 4560
		private Vector3 tweenedPos = Vector3.zero;

		// Token: 0x040011D1 RID: 4561
		private Vector3 lastTickSpringPos;

		// Token: 0x040011D2 RID: 4562
		private const float SpringTightness = 0.09f;
	}
}
