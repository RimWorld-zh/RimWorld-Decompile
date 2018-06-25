using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000EC5 RID: 3781
	public class DiaOption
	{
		// Token: 0x04003BB3 RID: 15283
		public Window dialog;

		// Token: 0x04003BB4 RID: 15284
		protected string text;

		// Token: 0x04003BB5 RID: 15285
		public DiaNode link;

		// Token: 0x04003BB6 RID: 15286
		public Func<DiaNode> linkLateBind;

		// Token: 0x04003BB7 RID: 15287
		public bool resolveTree = false;

		// Token: 0x04003BB8 RID: 15288
		public Action action;

		// Token: 0x04003BB9 RID: 15289
		public bool disabled = false;

		// Token: 0x04003BBA RID: 15290
		public string disabledReason = null;

		// Token: 0x04003BBB RID: 15291
		public SoundDef clickSound = SoundDefOf.PageChange;

		// Token: 0x04003BBC RID: 15292
		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);

		// Token: 0x06005967 RID: 22887 RVA: 0x002DD634 File Offset: 0x002DBA34
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		// Token: 0x06005968 RID: 22888 RVA: 0x002DD694 File Offset: 0x002DBA94
		public DiaOption(string text)
		{
			this.text = text;
		}

		// Token: 0x06005969 RID: 22889 RVA: 0x002DD6EC File Offset: 0x002DBAEC
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
		// (get) Token: 0x0600596A RID: 22890 RVA: 0x002DD760 File Offset: 0x002DBB60
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
		// (get) Token: 0x0600596B RID: 22891 RVA: 0x002DD790 File Offset: 0x002DBB90
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		// Token: 0x0600596C RID: 22892 RVA: 0x002DD7B0 File Offset: 0x002DBBB0
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x002DD7C4 File Offset: 0x002DBBC4
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

		// Token: 0x0600596E RID: 22894 RVA: 0x002DD860 File Offset: 0x002DBC60
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
