using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EB5 RID: 3765
	public abstract class Dialog_Rename : Window
	{
		// Token: 0x060058FD RID: 22781 RVA: 0x002DA01B File Offset: 0x002D841B
		public Dialog_Rename()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x060058FE RID: 22782 RVA: 0x002DA048 File Offset: 0x002D8448
		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x060058FF RID: 22783 RVA: 0x002DA060 File Offset: 0x002D8460
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		// Token: 0x06005900 RID: 22784 RVA: 0x002DA084 File Offset: 0x002D8484
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

		// Token: 0x06005901 RID: 22785 RVA: 0x002DA0B8 File Offset: 0x002D84B8
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

		// Token: 0x06005902 RID: 22786
		protected abstract void SetName(string name);

		// Token: 0x04003B57 RID: 15191
		protected string curName;

		// Token: 0x04003B58 RID: 15192
		private bool focusedRenameField;
	}
}
