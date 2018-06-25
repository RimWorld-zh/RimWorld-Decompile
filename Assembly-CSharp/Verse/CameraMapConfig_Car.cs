using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AEC RID: 2796
	public class CameraMapConfig_Car : CameraMapConfig
	{
		// Token: 0x04002734 RID: 10036
		private float targetAngle;

		// Token: 0x04002735 RID: 10037
		private float angle;

		// Token: 0x04002736 RID: 10038
		private float speed;

		// Token: 0x04002737 RID: 10039
		private const float SpeedChangeSpeed = 1.2f;

		// Token: 0x04002738 RID: 10040
		private const float AngleChangeSpeed = 0.72f;

		// Token: 0x06003DF2 RID: 15858 RVA: 0x0020B27A File Offset: 0x0020967A
		public CameraMapConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateMouseDrag = 50f;
			this.dollyRateScreenEdge = 0f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x0020B2BC File Offset: 0x002096BC
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
