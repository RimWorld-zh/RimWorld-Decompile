using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02000580 RID: 1408
	public class WorldCameraConfig_Car : WorldCameraConfig
	{
		// Token: 0x04000FB5 RID: 4021
		private float targetAngle;

		// Token: 0x04000FB6 RID: 4022
		private float angle;

		// Token: 0x04000FB7 RID: 4023
		private float speed;

		// Token: 0x04000FB8 RID: 4024
		private const float SpeedChangeSpeed = 1.5f;

		// Token: 0x04000FB9 RID: 4025
		private const float AngleChangeSpeed = 0.72f;

		// Token: 0x06001ADC RID: 6876 RVA: 0x000E72C2 File Offset: 0x000E56C2
		public WorldCameraConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateMouseDrag = 100f;
			this.dollyRateScreenEdge = 0f;
			this.camRotationDecayFactor = 1f;
			this.rotationSpeedScale = 0.15f;
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x000E7304 File Offset: 0x000E5704
		public override void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
			base.ConfigFixedUpdate_60(ref rotationVelocity);
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
				this.speed += 1.5f * num;
			}
			if (KeyBindingDefOf.MapDolly_Down.IsDown)
			{
				this.speed -= 1.5f * num;
				if (this.speed < 0f)
				{
					this.speed = 0f;
				}
			}
			this.angle = Mathf.Lerp(this.angle, this.targetAngle, 0.02f);
			rotationVelocity.x = Mathf.Cos(this.angle) * this.speed;
			rotationVelocity.y = Mathf.Sin(this.angle) * this.speed;
		}
	}
}
