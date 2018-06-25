using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DEE RID: 3566
	public class MoteText : MoteThrown
	{
		// Token: 0x040034FA RID: 13562
		public string text;

		// Token: 0x040034FB RID: 13563
		public Color textColor = Color.white;

		// Token: 0x040034FC RID: 13564
		public float overrideTimeBeforeStartFadeout = -1f;

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06004FEB RID: 20459 RVA: 0x002980B4 File Offset: 0x002964B4
		protected float TimeBeforeStartFadeout
		{
			get
			{
				return (this.overrideTimeBeforeStartFadeout < 0f) ? this.def.mote.solidTime : this.overrideTimeBeforeStartFadeout;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06004FEC RID: 20460 RVA: 0x002980F4 File Offset: 0x002964F4
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x0029812B File Offset: 0x0029652B
		public override void Draw()
		{
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x00298130 File Offset: 0x00296530
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}
	}
}
