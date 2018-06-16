using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000EC4 RID: 3780
	public class DiaOption
	{
		// Token: 0x06005945 RID: 22853 RVA: 0x002DB6A4 File Offset: 0x002D9AA4
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		// Token: 0x06005946 RID: 22854 RVA: 0x002DB704 File Offset: 0x002D9B04
		public DiaOption(string text)
		{
			this.text = text;
		}

		// Token: 0x06005947 RID: 22855 RVA: 0x002DB75C File Offset: 0x002D9B5C
		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06005948 RID: 22856 RVA: 0x002DB7D0 File Offset: 0x002D9BD0
		public static DiaOption DefaultOK
		{
			get
			{
				return new DiaOption("OK".Translate())
				{
					resolveTree = true
				};
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06005949 RID: 22857 RVA: 0x002DB800 File Offset: 0x002D9C00
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		// Token: 0x0600594A RID: 22858 RVA: 0x002DB820 File Offset: 0x002D9C20
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		// Token: 0x0600594B RID: 22859 RVA: 0x002DB834 File Offset: 0x002D9C34
		public float OptOnGUI(Rect rect, bool active = true)
		{
			Color textColor = Widgets.NormalOptionColor;
			string text = this.text;
			if (this.disabled)
			{
				textColor = this.DisabledOptionColor;
				if (this.disabledReason != null)
				{
					text = text + " (" + this.disabledReason + ")";
				}
			}
			rect.height = Text.CalcHeight(text, rect.width);
			if (Widgets.ButtonText(rect, text, false, false, textColor, active && !this.disabled))
			{
				this.Activate();
			}
			return rect.height;
		}

		// Token: 0x0600594C RID: 22860 RVA: 0x002DB8D0 File Offset: 0x002D9CD0
		protected void Activate()
		{
			if (this.clickSound != null && !this.resolveTree)
			{
				this.clickSound.PlayOneShotOnCamera(null);
			}
			if (this.resolveTree)
			{
				this.OwningDialog.Close(true);
			}
			if (this.action != null)
			{
				this.action();
			}
			if (this.linkLateBind != null)
			{
				this.OwningDialog.GotoNode(this.linkLateBind());
			}
			else if (this.link != null)
			{
				this.OwningDialog.GotoNode(this.link);
			}
		}

		// Token: 0x04003B9C RID: 15260
		public Window dialog;

		// Token: 0x04003B9D RID: 15261
		protected string text;

		// Token: 0x04003B9E RID: 15262
		public DiaNode link;

		// Token: 0x04003B9F RID: 15263
		public Func<DiaNode> linkLateBind;

		// Token: 0x04003BA0 RID: 15264
		public bool resolveTree = false;

		// Token: 0x04003BA1 RID: 15265
		public Action action;

		// Token: 0x04003BA2 RID: 15266
		public bool disabled = false;

		// Token: 0x04003BA3 RID: 15267
		public string disabledReason = null;

		// Token: 0x04003BA4 RID: 15268
		public SoundDef clickSound = SoundDefOf.PageChange;

		// Token: 0x04003BA5 RID: 15269
		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
