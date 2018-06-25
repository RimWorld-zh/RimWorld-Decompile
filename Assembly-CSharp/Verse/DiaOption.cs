using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000EC4 RID: 3780
	public class DiaOption
	{
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

		// Token: 0x06005967 RID: 22887 RVA: 0x002DD448 File Offset: 0x002DB848
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		// Token: 0x06005968 RID: 22888 RVA: 0x002DD4A8 File Offset: 0x002DB8A8
		public DiaOption(string text)
		{
			this.text = text;
		}

		// Token: 0x06005969 RID: 22889 RVA: 0x002DD500 File Offset: 0x002DB900
		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x0600596A RID: 22890 RVA: 0x002DD574 File Offset: 0x002DB974
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

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x0600596B RID: 22891 RVA: 0x002DD5A4 File Offset: 0x002DB9A4
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		// Token: 0x0600596C RID: 22892 RVA: 0x002DD5C4 File Offset: 0x002DB9C4
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x002DD5D8 File Offset: 0x002DB9D8
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

		// Token: 0x0600596E RID: 22894 RVA: 0x002DD674 File Offset: 0x002DBA74
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
	}
}
