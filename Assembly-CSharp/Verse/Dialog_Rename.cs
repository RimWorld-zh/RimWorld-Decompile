using UnityEngine;

namespace Verse
{
	public abstract class Dialog_Rename : Window
	{
		protected string curName;

		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		public Dialog_Rename()
		{
			base.forcePause = true;
			base.doCloseX = true;
			base.closeOnEscapeKey = true;
			base.absorbInputAroundWindow = true;
			base.closeOnClickedOutside = true;
		}

		protected virtual AcceptanceReport NameIsValid(string name)
		{
			if (name.Length == 0)
			{
				return false;
			}
			return true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				flag = true;
				Event.current.Use();
			}
			string text = Widgets.TextField(new Rect(0f, 15f, inRect.width, 35f), this.curName);
			if (text.Length < this.MaxNameLength)
			{
				this.curName = text;
			}
			if (!Widgets.ButtonText(new Rect(15f, (float)(inRect.height - 35.0 - 15.0), (float)(inRect.width - 15.0 - 15.0), 35f), "OK", true, false, true) && !flag)
				return;
			AcceptanceReport acceptanceReport = this.NameIsValid(this.curName);
			if (!acceptanceReport.Accepted)
			{
				if (acceptanceReport.Reason == null)
				{
					Messages.Message("NameIsInvalid".Translate(), MessageSound.RejectInput);
				}
				else
				{
					Messages.Message(acceptanceReport.Reason, MessageSound.RejectInput);
				}
			}
			else
			{
				this.SetName(this.curName);
				Find.WindowStack.TryRemove(this, true);
			}
		}

		protected abstract void SetName(string name);
	}
}
