using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E03 RID: 3587
	public class CompColorable : ThingComp
	{
		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x0600512A RID: 20778 RVA: 0x0029A4DC File Offset: 0x002988DC
		// (set) Token: 0x0600512B RID: 20779 RVA: 0x0029A51D File Offset: 0x0029891D
		public Color Color
		{
			get
			{
				Color result;
				if (!this.active)
				{
					result = this.parent.def.graphicData.color;
				}
				else
				{
					result = this.color;
				}
				return result;
			}
			set
			{
				if (!(value == this.color))
				{
					this.active = true;
					this.color = value;
					this.parent.Notify_ColorChanged();
				}
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x0600512C RID: 20780 RVA: 0x0029A550 File Offset: 0x00298950
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x0029A56C File Offset: 0x0029896C
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators))
			{
				this.Color = this.parent.def.colorGenerator.NewRandomizedColor();
			}
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x0029A5DC File Offset: 0x002989DC
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode != LoadSaveMode.Saving || this.active)
			{
				Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
				Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
			}
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x0029A637 File Offset: 0x00298A37
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}

		// Token: 0x04003547 RID: 13639
		private Color color = Color.white;

		// Token: 0x04003548 RID: 13640
		private bool active = false;
	}
}
