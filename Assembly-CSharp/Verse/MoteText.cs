using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DEE RID: 3566
	public class MoteText : MoteThrown
	{
		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06004FD2 RID: 20434 RVA: 0x002966CC File Offset: 0x00294ACC
		protected float TimeBeforeStartFadeout
		{
			get
			{
				return (this.overrideTimeBeforeStartFadeout < 0f) ? this.def.mote.solidTime : this.overrideTimeBeforeStartFadeout;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06004FD3 RID: 20435 RVA: 0x0029670C File Offset: 0x00294B0C
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x00296743 File Offset: 0x00294B43
		public override void Draw()
		{
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x00296748 File Offset: 0x00294B48
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}

		// Token: 0x040034E8 RID: 13544
		public string text;

		// Token: 0x040034E9 RID: 13545
		public Color textColor = Color.white;

		// Token: 0x040034EA RID: 13546
		public float overrideTimeBeforeStartFadeout = -1f;
	}
}
