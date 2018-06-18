using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007FD RID: 2045
	public abstract class Dialog_GiveName : Window
	{
		// Token: 0x06002D79 RID: 11641 RVA: 0x0017E56C File Offset: 0x0017C96C
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

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06002D7A RID: 11642 RVA: 0x0017E620 File Offset: 0x0017CA20
		private float Height
		{
			get
			{
				return (!this.useSecondName) ? 200f : 330f;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002D7B RID: 11643 RVA: 0x0017E650 File Offset: 0x0017CA50
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, this.Height);
			}
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x0017E678 File Offset: 0x0017CA78
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

		// Token: 0x06002D7D RID: 11645
		protected abstract bool IsValidName(string s);

		// Token: 0x06002D7E RID: 11646
		protected abstract void Named(string s);

		// Token: 0x06002D7F RID: 11647 RVA: 0x0017EB14 File Offset: 0x0017CF14
		protected virtual bool IsValidSecondName(string s)
		{
			return true;
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x0017EB2A File Offset: 0x0017CF2A
		protected virtual void NamedSecond(string s)
		{
		}

		// Token: 0x040017FF RID: 6143
		protected Pawn suggestingPawn;

		// Token: 0x04001800 RID: 6144
		protected string curName;

		// Token: 0x04001801 RID: 6145
		protected Func<string> nameGenerator;

		// Token: 0x04001802 RID: 6146
		protected string nameMessageKey;

		// Token: 0x04001803 RID: 6147
		protected string gainedNameMessageKey;

		// Token: 0x04001804 RID: 6148
		protected string invalidNameMessageKey;

		// Token: 0x04001805 RID: 6149
		protected bool useSecondName;

		// Token: 0x04001806 RID: 6150
		protected string curSecondName;

		// Token: 0x04001807 RID: 6151
		protected Func<string> secondNameGenerator;

		// Token: 0x04001808 RID: 6152
		protected string secondNameMessageKey;

		// Token: 0x04001809 RID: 6153
		protected string invalidSecondNameMessageKey;
	}
}
