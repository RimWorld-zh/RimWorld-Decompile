using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class Dialog_GiveName : Window
	{
		protected Pawn suggestingPawn;

		protected string curName;

		protected string nameMessageKey;

		protected string gainedNameMessageKey;

		protected string invalidNameMessageKey;

		protected bool useSecondName;

		protected string curSecondName;

		protected string secondNameMessageKey;

		protected string invalidSecondNameMessageKey;

		private float Height
		{
			get
			{
				return (float)((!this.useSecondName) ? 200.0 : 330.0);
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, this.Height);
			}
		}

		public Dialog_GiveName()
		{
			if (Find.AnyPlayerHomeMap != null && Find.AnyPlayerHomeMap.mapPawns.FreeColonistsCount != 0)
			{
				if (Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawned.RandomElement();
				}
				else
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonists.RandomElement();
				}
			}
			else
			{
				this.suggestingPawn = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_FreeColonists.RandomElement();
			}
			base.forcePause = true;
			base.closeOnEscapeKey = false;
			base.absorbInputAroundWindow = true;
		}

		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				flag = true;
				Event.current.Use();
			}
			Rect rect2;
			if (!this.useSecondName)
			{
				Widgets.Label(new Rect(0f, 0f, rect.width, rect.height), this.nameMessageKey.Translate(this.suggestingPawn.NameStringShort));
				this.curName = Widgets.TextField(new Rect(0f, (float)(rect.height - 35.0), (float)(rect.width / 2.0 - 20.0), 35f), this.curName);
				rect2 = new Rect((float)(rect.width / 2.0 + 20.0), (float)(rect.height - 35.0), (float)(rect.width / 2.0 - 20.0), 35f);
			}
			else
			{
				float num = 0f;
				string text = this.nameMessageKey.Translate(this.suggestingPawn.NameStringShort);
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num = (float)(num + (Text.CalcHeight(text, rect.width) + 10.0));
				this.curName = Widgets.TextField(new Rect(0f, num, (float)(rect.width / 2.0 - 20.0), 35f), this.curName);
				num = (float)(num + 60.0);
				text = this.secondNameMessageKey.Translate(this.suggestingPawn.NameStringShort);
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num = (float)(num + (Text.CalcHeight(text, rect.width) + 10.0));
				this.curSecondName = Widgets.TextField(new Rect(0f, num, (float)(rect.width / 2.0 - 20.0), 35f), this.curSecondName);
				num = (float)(num + 45.0);
				rect2 = new Rect(0f, (float)(rect.height - 35.0), (float)(rect.width / 2.0 - 20.0), 35f);
			}
			if (!Widgets.ButtonText(rect2, "OK".Translate(), true, false, true) && !flag)
				return;
			if (this.IsValidName(this.curName) && (!this.useSecondName || this.IsValidName(this.curSecondName)))
			{
				if (this.useSecondName)
				{
					this.Named(this.curName);
					this.NamedSecond(this.curSecondName);
					Messages.Message(this.gainedNameMessageKey.Translate(this.curName, this.curSecondName), MessageTypeDefOf.TaskCompletion);
				}
				else
				{
					this.Named(this.curName);
					Messages.Message(this.gainedNameMessageKey.Translate(this.curName), MessageTypeDefOf.TaskCompletion);
				}
				Find.WindowStack.TryRemove(this, true);
			}
			else
			{
				Messages.Message(this.invalidNameMessageKey.Translate(), MessageTypeDefOf.RejectInput);
			}
			Event.current.Use();
		}

		protected abstract bool IsValidName(string s);

		protected abstract void Named(string s);

		protected virtual bool IsValidSecondName(string s)
		{
			return true;
		}

		protected virtual void NamedSecond(string s)
		{
		}
	}
}
