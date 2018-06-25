using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E01 RID: 3585
	public class CompColorable : ThingComp
	{
		// Token: 0x0400354C RID: 13644
		private Color color = Color.white;

		// Token: 0x0400354D RID: 13645
		private bool active = false;

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06005140 RID: 20800 RVA: 0x0029BBC4 File Offset: 0x00299FC4
		// (set) Token: 0x06005141 RID: 20801 RVA: 0x0029BC05 File Offset: 0x0029A005
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
		// (get) Token: 0x06005142 RID: 20802 RVA: 0x0029BC38 File Offset: 0x0029A038
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x0029BC54 File Offset: 0x0029A054
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators))
			{
				this.Color = this.parent.def.colorGenerator.NewRandomizedColor();
			}
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x0029BCC4 File Offset: 0x0029A0C4
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode != LoadSaveMode.Saving || this.active)
			{
				Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
				Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
			}
		}

		// Token: 0x06005145 RID: 20805 RVA: 0x0029BD1F File Offset: 0x0029A11F
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}
	}
}
