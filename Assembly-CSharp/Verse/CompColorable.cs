using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DFF RID: 3583
	public class CompColorable : ThingComp
	{
		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x0600513C RID: 20796 RVA: 0x0029BA98 File Offset: 0x00299E98
		// (set) Token: 0x0600513D RID: 20797 RVA: 0x0029BAD9 File Offset: 0x00299ED9
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

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x0600513E RID: 20798 RVA: 0x0029BB0C File Offset: 0x00299F0C
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x0029BB28 File Offset: 0x00299F28
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators))
			{
				this.Color = this.parent.def.colorGenerator.NewRandomizedColor();
			}
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x0029BB98 File Offset: 0x00299F98
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode != LoadSaveMode.Saving || this.active)
			{
				Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
				Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
			}
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x0029BBF3 File Offset: 0x00299FF3
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}

		// Token: 0x0400354C RID: 13644
		private Color color = Color.white;

		// Token: 0x0400354D RID: 13645
		private bool active = false;
	}
}
