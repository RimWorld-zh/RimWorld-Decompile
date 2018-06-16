using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DEF RID: 3567
	public class MoteText : MoteThrown
	{
		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06004FD4 RID: 20436 RVA: 0x002966EC File Offset: 0x00294AEC
		protected float TimeBeforeStartFadeout
		{
			get
			{
				return (this.overrideTimeBeforeStartFadeout < 0f) ? this.def.mote.solidTime : this.overrideTimeBeforeStartFadeout;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06004FD5 RID: 20437 RVA: 0x0029672C File Offset: 0x00294B2C
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x00296763 File Offset: 0x00294B63
		public override void Draw()
		{
		}

		// Token: 0x06004FD7 RID: 20439 RVA: 0x00296768 File Offset: 0x00294B68
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}

		// Token: 0x040034EA RID: 13546
		public string text;

		// Token: 0x040034EB RID: 13547
		public Color textColor = Color.white;

		// Token: 0x040034EC RID: 13548
		public float overrideTimeBeforeStartFadeout = -1f;
	}
}
