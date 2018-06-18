using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AED RID: 2797
	public class CameraMapConfig_Car : CameraMapConfig
	{
		// Token: 0x06003DF3 RID: 15859 RVA: 0x0020AB4A File Offset: 0x00208F4A
		public CameraMapConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateMouseDrag = 50f;
			this.dollyRateScreenEdge = 0f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x0020AB8C File Offset: 0x00208F8C
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

		// Token: 0x04002731 RID: 10033
		private float targetAngle;

		// Token: 0x04002732 RID: 10034
		private float angle;

		// Token: 0x04002733 RID: 10035
		private float speed;

		// Token: 0x04002734 RID: 10036
		private const float SpeedChangeSpeed = 1.2f;

		// Token: 0x04002735 RID: 10037
		private const float AngleChangeSpeed = 0.72f;
	}
}
