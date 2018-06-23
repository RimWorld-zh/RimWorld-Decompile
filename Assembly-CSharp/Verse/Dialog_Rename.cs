using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EB4 RID: 3764
	public abstract class Dialog_Rename : Window
	{
		// Token: 0x04003B67 RID: 15207
		protected string curName;

		// Token: 0x04003B68 RID: 15208
		private bool focusedRenameField;

		// Token: 0x0600591E RID: 22814 RVA: 0x002DBC68 File Offset: 0x002DA068
		public Dialog_Rename()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x0600591F RID: 22815 RVA: 0x002DBC94 File Offset: 0x002DA094
		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06005920 RID: 22816 RVA: 0x002DBCAC File Offset: 0x002DA0AC
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		// Token: 0x06005921 RID: 22817 RVA: 0x002DBCD0 File Offset: 0x002DA0D0
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

		// Token: 0x06005922 RID: 22818 RVA: 0x002DBD04 File Offset: 0x002DA104
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

		// Token: 0x06005923 RID: 22819
		protected abstract void SetName(string name);
	}
}
