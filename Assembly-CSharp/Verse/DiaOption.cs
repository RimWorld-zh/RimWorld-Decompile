using RimWorld;
using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public class DiaOption
	{
		public Window dialog;

		protected string text;

		public DiaNode link;

		public Func<DiaNode> linkLateBind;

		public bool resolveTree = false;

		public Action action;

		public bool disabled = false;

		public string disabledReason = (string)null;

		public SoundDef clickSound = SoundDefOf.PageChange;

		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);

		public static DiaOption DefaultOK
		{
			get
			{
				DiaOption diaOption = new DiaOption("OK".Translate());
				diaOption.resolveTree = true;
				return diaOption;
			}
		}

		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		public DiaOption(string text)
		{
			this.text = text;
		}

		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

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
			if ((object)this.action != null)
			{
				this.action();
			}
			if ((object)this.linkLateBind != null)
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
