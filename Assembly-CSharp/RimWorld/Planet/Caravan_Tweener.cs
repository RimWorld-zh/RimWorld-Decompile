using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020005ED RID: 1517
	public class Caravan_Tweener
	{
		// Token: 0x040011CF RID: 4559
		private Caravan caravan;

		// Token: 0x040011D0 RID: 4560
		private Vector3 tweenedPos = Vector3.zero;

		// Token: 0x040011D1 RID: 4561
		private Vector3 lastTickSpringPos;

		// Token: 0x040011D2 RID: 4562
		private const float SpringTightness = 0.09f;

		// Token: 0x06001E23 RID: 7715 RVA: 0x00103968 File Offset: 0x00101D68
		public Caravan_Tweener(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001E24 RID: 7716 RVA: 0x00103984 File Offset: 0x00101D84
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001E25 RID: 7717 RVA: 0x001039A0 File Offset: 0x00101DA0
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001E26 RID: 7718 RVA: 0x001039C8 File Offset: 0x00101DC8
		public Vector3 TweenedPosRoot
		{
			get
			{
				return CaravanTweenerUtility.PatherTweenedPosRoot(this.caravan) + CaravanTweenerUtility.CaravanCollisionPosOffsetFor(this.caravan);
			}
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x001039F8 File Offset: 0x00101DF8
		public void TweenerTick()
		{
			this.lastTickSpringPos = this.tweenedPos;
			Vector3 a = this.TweenedPosRoot - this.tweenedPos;
			this.tweenedPos += a * 0.09f;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x00103A40 File Offset: 0x00101E40
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot;
			this.lastTickSpringPos = this.tweenedPos;
		}
	}
}
