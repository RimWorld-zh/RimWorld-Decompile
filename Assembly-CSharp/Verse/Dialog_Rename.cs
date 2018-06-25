using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EB7 RID: 3767
	public abstract class Dialog_Rename : Window
	{
		// Token: 0x04003B6F RID: 15215
		protected string curName;

		// Token: 0x04003B70 RID: 15216
		private bool focusedRenameField;

		// Token: 0x06005922 RID: 22818 RVA: 0x002DBF80 File Offset: 0x002DA380
		public Dialog_Rename()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06005923 RID: 22819 RVA: 0x002DBFAC File Offset: 0x002DA3AC
		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06005924 RID: 22820 RVA: 0x002DBFC4 File Offset: 0x002DA3C4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		// Token: 0x06005925 RID: 22821 RVA: 0x002DBFE8 File Offset: 0x002DA3E8
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

		// Token: 0x06005926 RID: 22822 RVA: 0x002DC01C File Offset: 0x002DA41C
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

		// Token: 0x06005927 RID: 22823
		protected abstract void SetName(string name);
	}
}
