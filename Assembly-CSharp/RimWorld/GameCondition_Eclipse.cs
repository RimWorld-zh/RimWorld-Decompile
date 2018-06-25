using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200030E RID: 782
	public class GameCondition_Eclipse : GameCondition
	{
		// Token: 0x04000873 RID: 2163
		private const int LerpTicks = 200;

		// Token: 0x04000874 RID: 2164
		private SkyColorSet EclipseSkyColors = new SkyColorSet(new Color(0.482f, 0.603f, 0.682f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);

		// Token: 0x06000D34 RID: 3380 RVA: 0x000726B8 File Offset: 0x00070AB8
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, 200f, 1f);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x000726E0 File Offset: 0x00070AE0
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, this.EclipseSkyColors, 1f, 0f));
		}
	}
}
