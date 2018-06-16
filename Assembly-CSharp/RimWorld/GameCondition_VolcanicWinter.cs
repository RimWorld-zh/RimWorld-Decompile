using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000311 RID: 785
	public class GameCondition_VolcanicWinter : GameCondition
	{
		// Token: 0x06000D51 RID: 3409 RVA: 0x000730C0 File Offset: 0x000714C0
		public GameCondition_VolcanicWinter()
		{
			ColorInt colorInt = new ColorInt(0, 0, 0);
			this.VolcanicWinterColors = new SkyColorSet(colorInt.ToColor, Color.white, new Color(0.6f, 0.6f, 0.6f), 0.65f);
			base..ctor();
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00073124 File Offset: 0x00071524
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.LerpTicks, 0.3f);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0007314C File Offset: 0x0007154C
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.55f, this.VolcanicWinterColors, 1f, 1f));
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00073180 File Offset: 0x00071580
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.LerpTicks, this.MaxTempOffset);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000731A8 File Offset: 0x000715A8
		public override float AnimalDensityFactor(Map map)
		{
			return 1f - GameConditionUtility.LerpInOutValue(this, (float)this.LerpTicks, 0.5f);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x000731D8 File Offset: 0x000715D8
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x04000886 RID: 2182
		private int LerpTicks = 50000;

		// Token: 0x04000887 RID: 2183
		private float MaxTempOffset = -7f;

		// Token: 0x04000888 RID: 2184
		private const float AnimalDensityImpact = 0.5f;

		// Token: 0x04000889 RID: 2185
		private const float SkyGlow = 0.55f;

		// Token: 0x0400088A RID: 2186
		private const float MaxSkyLerpFactor = 0.3f;

		// Token: 0x0400088B RID: 2187
		private SkyColorSet VolcanicWinterColors;
	}
}
