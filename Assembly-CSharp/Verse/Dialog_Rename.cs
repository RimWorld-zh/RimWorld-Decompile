using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EB6 RID: 3766
	public abstract class Dialog_Rename : Window
	{
		// Token: 0x060058FF RID: 22783 RVA: 0x002D9FE3 File Offset: 0x002D83E3
		public Dialog_Rename()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06005900 RID: 22784 RVA: 0x002DA010 File Offset: 0x002D8410
		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06005901 RID: 22785 RVA: 0x002DA028 File Offset: 0x002D8428
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		// Token: 0x06005902 RID: 22786 RVA: 0x002DA04C File Offset: 0x002D844C
		protected virtual AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result;
			if (name.Length == 0)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06005903 RID: 22787 RVA: 0x002DA080 File Offset: 0x002D8480
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				flag = true;
				Event.current.Use();
			}
			GUI.SetNextControlName("RenameField");
			string text = Widgets.TextField(new Rect(0f, 15f, inRect.width, 35f), this.curName);
			if (text.Length < this.MaxNameLength)
			{
				this.curName = text;
			}
			if (!this.focusedRenameField)
			{
				UI.FocusControl("RenameField", this);
				this.focusedRenameField = true;
			}
			if (Widgets.ButtonText(new Rect(15f, inRect.height - 35f - 15f, inRect.width - 15f - 15f, 35f), "OK", true, false, true) || flag)
			{
				AcceptanceReport acceptanceReport = this.NameIsValid(this.curName);
				if (!acceptanceReport.Accepted)
				{
					if (acceptanceReport.Reason.NullOrEmpty())
					{
						Messages.Message("NameIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					}
				}
				else
				{
					this.SetName(this.curName);
					Find.WindowStack.TryRemove(this, true);
				}
			}
		}

		// Token: 0x06005904 RID: 22788
		protected abstract void SetName(string name);

		// Token: 0x04003B58 RID: 15192
		protected string curName;

		// Token: 0x04003B59 RID: 15193
		private bool focusedRenameField;
	}
}
