using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020005EF RID: 1519
	public class Caravan_Tweener
	{
		// Token: 0x06001E28 RID: 7720 RVA: 0x001037C4 File Offset: 0x00101BC4
		public Caravan_Tweener(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001E29 RID: 7721 RVA: 0x001037E0 File Offset: 0x00101BE0
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001E2A RID: 7722 RVA: 0x001037FC File Offset: 0x00101BFC
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001E2B RID: 7723 RVA: 0x00103824 File Offset: 0x00101C24
		public Vector3 TweenedPosRoot
		{
			get
			{
				return CaravanTweenerUtility.PatherTweenedPosRoot(this.caravan) + CaravanTweenerUtility.CaravanCollisionPosOffsetFor(this.caravan);
			}
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x00103854 File Offset: 0x00101C54
		public void TweenerTick()
		{
			this.lastTickSpringPos = this.tweenedPos;
			Vector3 a = this.TweenedPosRoot - this.tweenedPos;
			this.tweenedPos += a * 0.09f;
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x0010389C File Offset: 0x00101C9C
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot;
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x040011D2 RID: 4562
		private Caravan caravan;

		// Token: 0x040011D3 RID: 4563
		private Vector3 tweenedPos = Vector3.zero;

		// Token: 0x040011D4 RID: 4564
		private Vector3 lastTickSpringPos;

		// Token: 0x040011D5 RID: 4565
		private const float SpringTightness = 0.09f;
	}
}
