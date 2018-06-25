using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200030E RID: 782
	public class GameCondition_Eclipse : GameCondition
	{
		// Token: 0x04000876 RID: 2166
		private const int LerpTicks = 200;

		// Token: 0x04000877 RID: 2167
		private SkyColorSet EclipseSkyColors = new SkyColorSet(new Color(0.482f, 0.603f, 0.682f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);

		// Token: 0x06000D33 RID: 3379 RVA: 0x000726C0 File Offset: 0x00070AC0
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, 200f, 1f);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x000726E8 File Offset: 0x00070AE8
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, this.EclipseSkyColors, 1f, 0f));
		}
	}
}
