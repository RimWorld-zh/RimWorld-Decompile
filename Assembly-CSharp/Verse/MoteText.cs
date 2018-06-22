using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DEB RID: 3563
	public class MoteText : MoteThrown
	{
		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06004FE7 RID: 20455 RVA: 0x00297CA8 File Offset: 0x002960A8
		protected float TimeBeforeStartFadeout
		{
			get
			{
				return (this.overrideTimeBeforeStartFadeout < 0f) ? this.def.mote.solidTime : this.overrideTimeBeforeStartFadeout;
			}
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06004FE8 RID: 20456 RVA: 0x00297CE8 File Offset: 0x002960E8
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x00297D1F File Offset: 0x0029611F
		public override void Draw()
		{
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x00297D24 File Offset: 0x00296124
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}

		// Token: 0x040034F3 RID: 13555
		public string text;

		// Token: 0x040034F4 RID: 13556
		public Color textColor = Color.white;

		// Token: 0x040034F5 RID: 13557
		public float overrideTimeBeforeStartFadeout = -1f;
	}
}
