using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200030C RID: 780
	public class GameCondition_Eclipse : GameCondition
	{
		// Token: 0x06000D30 RID: 3376 RVA: 0x000724B4 File Offset: 0x000708B4
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, 200f, 1f);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x000724DC File Offset: 0x000708DC
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, this.EclipseSkyColors, 1f, 0f));
		}

		// Token: 0x04000871 RID: 2161
		private const int LerpTicks = 200;

		// Token: 0x04000872 RID: 2162
		private SkyColorSet EclipseSkyColors = new SkyColorSet(new Color(0.482f, 0.603f, 0.682f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);
	}
}
