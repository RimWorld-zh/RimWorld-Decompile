using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DED RID: 3565
	public class MoteText : MoteThrown
	{
		// Token: 0x040034F3 RID: 13555
		public string text;

		// Token: 0x040034F4 RID: 13556
		public Color textColor = Color.white;

		// Token: 0x040034F5 RID: 13557
		public float overrideTimeBeforeStartFadeout = -1f;

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06004FEB RID: 20459 RVA: 0x00297DD4 File Offset: 0x002961D4
		protected float TimeBeforeStartFadeout
		{
			get
			{
				return (this.overrideTimeBeforeStartFadeout < 0f) ? this.def.mote.solidTime : this.overrideTimeBeforeStartFadeout;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06004FEC RID: 20460 RVA: 0x00297E14 File Offset: 0x00296214
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x00297E4B File Offset: 0x0029624B
		public override void Draw()
		{
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x00297E50 File Offset: 0x00296250
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}
	}
}
