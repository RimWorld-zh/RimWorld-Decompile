using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078C RID: 1932
	public abstract class Alert_Critical : Alert
	{
		// Token: 0x06002ADB RID: 10971 RVA: 0x0016A0AE File Offset: 0x001684AE
		public Alert_Critical()
		{
			this.defaultPriority = AlertPriority.Critical;
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002ADC RID: 10972 RVA: 0x0016A0C8 File Offset: 0x001684C8
		protected override Color BGColor
		{
			get
			{
				float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
				return new Color(num, num, num) * Color.red;
			}
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x0016A10C File Offset: 0x0016850C
		public override void AlertActiveUpdate()
		{
			if (this.lastActiveFrame < Time.frameCount - 1)
			{
				Messages.Message("MessageCriticalAlert".Translate(new object[]
				{
					this.GetLabel().CapitalizeFirst()
				}), new LookTargets(this.GetReport().culprits), MessageTypeDefOf.ThreatBig, true);
			}
			this.lastActiveFrame = Time.frameCount;
		}

		// Token: 0x04001723 RID: 5923
		private int lastActiveFrame = -1;

		// Token: 0x04001724 RID: 5924
		private const float PulseFreq = 0.5f;

		// Token: 0x04001725 RID: 5925
		private const float PulseAmpCritical = 0.6f;

		// Token: 0x04001726 RID: 5926
		private const float PulseAmpTutorial = 0.2f;
	}
}
