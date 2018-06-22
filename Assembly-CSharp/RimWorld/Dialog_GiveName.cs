using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F9 RID: 2041
	public abstract class Dialog_GiveName : Window
	{
		// Token: 0x06002D72 RID: 11634 RVA: 0x0017E744 File Offset: 0x0017CB44
		public Dialog_GiveName()
		{
			if (Find.AnyPlayerHomeMap != null && Find.AnyPlayerHomeMap.mapPawns.FreeColonistsCount != 0)
			{
				if (Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
				}
				else
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonists.RandomElement<Pawn>();
				}
			}
			else
			{
				this.suggestingPawn = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.RandomElement<Pawn>();
			}
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002D73 RID: 11635 RVA: 0x0017E7F8 File Offset: 0x0017CBF8
		private float Height
		{
			get
			{
				return (!this.useSecondName) ? 200f : 330f;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002D74 RID: 11636 RVA: 0x0017E828 File Offset: 0x0017CC28
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, this.Height);
			}
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x0017E850 File Offset: 0x0017CC50
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
				Widgets.Label(new Rect(0f, 0f, rect.width, rect.height), this.nameMessageKey.Translate(new object[]
				{
					this.suggestingPawn.LabelShort
				}));
				if (this.nameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 20f, 80f, rect.width / 2f - 20f, 35f), "Randomize".Translate(), true, false, true))
				{
					this.curName = this.nameGenerator();
				}
				this.curName = Widgets.TextField(new Rect(0f, 80f, rect.width / 2f - 20f, 35f), this.curName);
				rect2 = new Rect(rect.width / 2f + 20f, rect.height - 35f, rect.width / 2f - 20f, 35f);
			}
			else
			{
				float num = 0f;
				string text = this.nameMessageKey.Translate(new object[]
				{
					this.suggestingPawn.LabelShort
				});
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num += Text.CalcHeight(text, rect.width) + 10f;
				if (this.nameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f, num, rect.width / 2f - 20f, 35f), "Randomize".Translate(), true, false, true))
				{
					this.curName = this.nameGenerator();
				}
				this.curName = Widgets.TextField(new Rect(0f, num, rect.width / 2f - 20f, 35f), this.curName);
				num += 60f;
				text = this.secondNameMessageKey.Translate(new object[]
				{
					this.suggestingPawn.LabelShort
				});
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num += Text.CalcHeight(text, rect.width) + 10f;
				if (this.secondNameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f, num, rect.width / 2f - 20f, 35f), "Randomize".Translate(), true, false, true))
				{
					this.curSecondName = this.secondNameGenerator();
				}
				this.curSecondName = Widgets.TextField(new Rect(0f, num, rect.width / 2f - 20f, 35f), this.curSecondName);
				num += 45f;
				rect2 = new Rect(0f, rect.height - 35f, rect.width / 2f - 20f, 35f);
			}
			if (Widgets.ButtonText(rect2, "OK".Translate(), true, false, true) || flag)
			{
				if (this.IsValidName(this.curName) && (!this.useSecondName || this.IsValidName(this.curSecondName)))
				{
					if (this.useSecondName)
					{
						this.Named(this.curName);
						this.NamedSecond(this.curSecondName);
						Messages.Message(this.gainedNameMessageKey.Translate(new object[]
						{
							this.curName,
							this.curSecondName
						}), MessageTypeDefOf.TaskCompletion, false);
					}
					else
					{
						this.Named(this.curName);
						Messages.Message(this.gainedNameMessageKey.Translate(new object[]
						{
							this.curName
						}), MessageTypeDefOf.TaskCompletion, false);
					}
					Find.WindowStack.TryRemove(this, true);
				}
				else
				{
					Messages.Message(this.invalidNameMessageKey.Translate(), MessageTypeDefOf.RejectInput, false);
				}
				Event.current.Use();
			}
		}

		// Token: 0x06002D76 RID: 11638
		protected abstract bool IsValidName(string s);

		// Token: 0x06002D77 RID: 11639
		protected abstract void Named(string s);

		// Token: 0x06002D78 RID: 11640 RVA: 0x0017ECEC File Offset: 0x0017D0EC
		protected virtual bool IsValidSecondName(string s)
		{
			return true;
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x0017ED02 File Offset: 0x0017D102
		protected virtual void NamedSecond(string s)
		{
		}

		// Token: 0x040017FD RID: 6141
		protected Pawn suggestingPawn;

		// Token: 0x040017FE RID: 6142
		protected string curName;

		// Token: 0x040017FF RID: 6143
		protected Func<string> nameGenerator;

		// Token: 0x04001800 RID: 6144
		protected string nameMessageKey;

		// Token: 0x04001801 RID: 6145
		protected string gainedNameMessageKey;

		// Token: 0x04001802 RID: 6146
		protected string invalidNameMessageKey;

		// Token: 0x04001803 RID: 6147
		protected bool useSecondName;

		// Token: 0x04001804 RID: 6148
		protected string curSecondName;

		// Token: 0x04001805 RID: 6149
		protected Func<string> secondNameGenerator;

		// Token: 0x04001806 RID: 6150
		protected string secondNameMessageKey;

		// Token: 0x04001807 RID: 6151
		protected string invalidSecondNameMessageKey;
	}
}
