using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000EC2 RID: 3778
	public class DiaOption
	{
		// Token: 0x06005964 RID: 22884 RVA: 0x002DD328 File Offset: 0x002DB728
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		// Token: 0x06005965 RID: 22885 RVA: 0x002DD388 File Offset: 0x002DB788
		public DiaOption(string text)
		{
			this.text = text;
		}

		// Token: 0x06005966 RID: 22886 RVA: 0x002DD3E0 File Offset: 0x002DB7E0
		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06005967 RID: 22887 RVA: 0x002DD454 File Offset: 0x002DB854
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

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06005968 RID: 22888 RVA: 0x002DD484 File Offset: 0x002DB884
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		// Token: 0x06005969 RID: 22889 RVA: 0x002DD4A4 File Offset: 0x002DB8A4
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		// Token: 0x0600596A RID: 22890 RVA: 0x002DD4B8 File Offset: 0x002DB8B8
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

		// Token: 0x0600596B RID: 22891 RVA: 0x002DD554 File Offset: 0x002DB954
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

		// Token: 0x04003BAB RID: 15275
		public Window dialog;

		// Token: 0x04003BAC RID: 15276
		protected string text;

		// Token: 0x04003BAD RID: 15277
		public DiaNode link;

		// Token: 0x04003BAE RID: 15278
		public Func<DiaNode> linkLateBind;

		// Token: 0x04003BAF RID: 15279
		public bool resolveTree = false;

		// Token: 0x04003BB0 RID: 15280
		public Action action;

		// Token: 0x04003BB1 RID: 15281
		public bool disabled = false;

		// Token: 0x04003BB2 RID: 15282
		public string disabledReason = null;

		// Token: 0x04003BB3 RID: 15283
		public SoundDef clickSound = SoundDefOf.PageChange;

		// Token: 0x04003BB4 RID: 15284
		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
