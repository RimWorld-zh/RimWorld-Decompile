using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000EC3 RID: 3779
	public class DiaOption
	{
		// Token: 0x06005943 RID: 22851 RVA: 0x002DB6DC File Offset: 0x002D9ADC
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		// Token: 0x06005944 RID: 22852 RVA: 0x002DB73C File Offset: 0x002D9B3C
		public DiaOption(string text)
		{
			this.text = text;
		}

		// Token: 0x06005945 RID: 22853 RVA: 0x002DB794 File Offset: 0x002D9B94
		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06005946 RID: 22854 RVA: 0x002DB808 File Offset: 0x002D9C08
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

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06005947 RID: 22855 RVA: 0x002DB838 File Offset: 0x002D9C38
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		// Token: 0x06005948 RID: 22856 RVA: 0x002DB858 File Offset: 0x002D9C58
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		// Token: 0x06005949 RID: 22857 RVA: 0x002DB86C File Offset: 0x002D9C6C
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

		// Token: 0x0600594A RID: 22858 RVA: 0x002DB908 File Offset: 0x002D9D08
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

		// Token: 0x04003B9B RID: 15259
		public Window dialog;

		// Token: 0x04003B9C RID: 15260
		protected string text;

		// Token: 0x04003B9D RID: 15261
		public DiaNode link;

		// Token: 0x04003B9E RID: 15262
		public Func<DiaNode> linkLateBind;

		// Token: 0x04003B9F RID: 15263
		public bool resolveTree = false;

		// Token: 0x04003BA0 RID: 15264
		public Action action;

		// Token: 0x04003BA1 RID: 15265
		public bool disabled = false;

		// Token: 0x04003BA2 RID: 15266
		public string disabledReason = null;

		// Token: 0x04003BA3 RID: 15267
		public SoundDef clickSound = SoundDefOf.PageChange;

		// Token: 0x04003BA4 RID: 15268
		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
