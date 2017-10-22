using RimWorld;
using System;
using UnityEngine;

namespace Verse
{
	public class Dialog_ChangeNameTriple : Window
	{
		private Pawn pawn;

		private string curName;

		private const int MaxNameLength = 16;

		private NameTriple CurPawnName
		{
			get
			{
				NameTriple nameTriple = this.pawn.Name as NameTriple;
				if (nameTriple != null)
				{
					return new NameTriple(nameTriple.First, this.curName, nameTriple.Last);
				}
				throw new InvalidOperationException();
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 175f);
			}
		}

		public Dialog_ChangeNameTriple(Pawn pawn)
		{
			this.pawn = pawn;
			this.curName = ((NameTriple)pawn.Name).Nick;
			base.forcePause = true;
			base.absorbInputAroundWindow = true;
			base.closeOnClickedOutside = true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				flag = true;
				Event.current.Use();
			}
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(15f, 15f, 500f, 50f), this.CurPawnName.ToString().Replace(" '' ", " "));
			Text.Font = GameFont.Small;
			string text = Widgets.TextField(new Rect(15f, 50f, (float)(inRect.width / 2.0 - 20.0), 35f), this.curName);
			if (text.Length < 16)
			{
				this.curName = text;
			}
			if (!Widgets.ButtonText(new Rect((float)(inRect.width / 2.0 + 20.0), (float)(inRect.height - 35.0), (float)(inRect.width / 2.0 - 20.0), 35f), "OK", true, false, true) && !flag)
				return;
			if (this.curName.Length < 1)
			{
				this.curName = ((NameTriple)this.pawn.Name).First;
			}
			this.pawn.Name = this.CurPawnName;
			Find.WindowStack.TryRemove(this, true);
			Messages.Message("PawnGainsName".Translate(this.curName), (Thing)this.pawn, MessageTypeDefOf.PositiveEvent);
		}
	}
}
