using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000313 RID: 787
	public class GameCondition_VolcanicWinter : GameCondition
	{
		// Token: 0x04000888 RID: 2184
		private int LerpTicks = 50000;

		// Token: 0x04000889 RID: 2185
		private float MaxTempOffset = -7f;

		// Token: 0x0400088A RID: 2186
		private const float AnimalDensityImpact = 0.5f;

		// Token: 0x0400088B RID: 2187
		private const float SkyGlow = 0.55f;

		// Token: 0x0400088C RID: 2188
		private const float MaxSkyLerpFactor = 0.3f;

		// Token: 0x0400088D RID: 2189
		private SkyColorSet VolcanicWinterColors;

		// Token: 0x06000D55 RID: 3413 RVA: 0x000732C4 File Offset: 0x000716C4
		public GameCondition_VolcanicWinter()
		{
			ColorInt colorInt = new ColorInt(0, 0, 0);
			this.VolcanicWinterColors = new SkyColorSet(colorInt.ToColor, Color.white, new Color(0.6f, 0.6f, 0.6f), 0.65f);
			base..ctor();
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00073328 File Offset: 0x00071728
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.LerpTicks, 0.3f);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00073350 File Offset: 0x00071750
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.55f, this.VolcanicWinterColors, 1f, 1f));
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00073384 File Offset: 0x00071784
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.LerpTicks, this.MaxTempOffset);
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x000733AC File Offset: 0x000717AC
		public override float AnimalDensityFactor(Map map)
		{
			return 1f - GameConditionUtility.LerpInOutValue(this, (float)this.LerpTicks, 0.5f);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x000733DC File Offset: 0x000717DC
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}
	}
}
