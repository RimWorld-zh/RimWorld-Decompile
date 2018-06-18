using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E02 RID: 3586
	public class CompColorable : ThingComp
	{
		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06005128 RID: 20776 RVA: 0x0029A4BC File Offset: 0x002988BC
		// (set) Token: 0x06005129 RID: 20777 RVA: 0x0029A4FD File Offset: 0x002988FD
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

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x0600512A RID: 20778 RVA: 0x0029A530 File Offset: 0x00298930
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x0029A54C File Offset: 0x0029894C
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators))
			{
				this.Color = this.parent.def.colorGenerator.NewRandomizedColor();
			}
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x0029A5BC File Offset: 0x002989BC
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode != LoadSaveMode.Saving || this.active)
			{
				Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
				Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
			}
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x0029A617 File Offset: 0x00298A17
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}

		// Token: 0x04003545 RID: 13637
		private Color color = Color.white;

		// Token: 0x04003546 RID: 13638
		private bool active = false;
	}
}
