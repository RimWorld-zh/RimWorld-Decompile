using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AEB RID: 2795
	public class CameraMapConfig_Car : CameraMapConfig
	{
		// Token: 0x0400272D RID: 10029
		private float targetAngle;

		// Token: 0x0400272E RID: 10030
		private float angle;

		// Token: 0x0400272F RID: 10031
		private float speed;

		// Token: 0x04002730 RID: 10032
		private const float SpeedChangeSpeed = 1.2f;

		// Token: 0x04002731 RID: 10033
		private const float AngleChangeSpeed = 0.72f;

		// Token: 0x06003DF2 RID: 15858 RVA: 0x0020AF9A File Offset: 0x0020939A
		public CameraMapConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateMouseDrag = 50f;
			this.dollyRateScreenEdge = 0f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x0020AFDC File Offset: 0x002093DC
		public override void ConfigFixedUpdate_60(ref Vector3 velocity)
		{
			base.ConfigFixedUpdate_60(ref velocity);
			float num = 0.0166666675f;
			if (KeyBindingDefOf.MapDolly_Left.IsDown)
			{
				this.targetAngle += 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Right.IsDown)
			{
				this.targetAngle -= 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Up.IsDown)
			{
				this.speed += 1.2f * num;
			}
			if (KeyBindingDefOf.MapDolly_Down.IsDown)
			{
				this.speed -= 1.2f * num;
				if (this.speed < 0f)
				{
					this.speed = 0f;
				}
			}
			this.angle = Mathf.Lerp(this.angle, this.targetAngle, 0.02f);
			velocity.x = Mathf.Cos(this.angle) * this.speed;
			velocity.z = Mathf.Sin(this.angle) * this.speed;
		}
	}
}
