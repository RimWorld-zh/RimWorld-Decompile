using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class MainTabWindow_PawnTable : MainTabWindow
	{
		private PawnTable table;

		protected MainTabWindow_PawnTable()
		{
		}

		protected virtual float ExtraBottomSpace
		{
			get
			{
				return 53f;
			}
		}

		protected virtual float ExtraTopSpace
		{
			get
			{
				return 0f;
			}
		}

		protected abstract PawnTableDef PawnTableDef { get; }

		protected override float Margin
		{
			get
			{
				return 6f;
			}
		}

		public override Vector2 RequestedTabSize
		{
			get
			{
				Vector2 result;
				if (this.table == null)
				{
					result = Vector2.zero;
				}
				else
				{
					result = new Vector2(this.table.Size.x + this.Margin * 2f, this.table.Size.y + this.ExtraBottomSpace + this.ExtraTopSpace + this.Margin * 2f);
				}
				return result;
			}
		}

		protected virtual IEnumerable<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.FreeColonists;
			}
		}

		public override void PostOpen()
		{
			if (this.table == null)
			{
				this.table = this.CreateTable();
			}
			this.SetDirty();
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			this.table.PawnTableOnGUI(new Vector2(rect.x, rect.y + this.ExtraTopSpace));
		}

		public void Notify_PawnsChanged()
		{
			this.SetDirty();
		}

		public override void Notify_ResolutionChanged()
		{
			this.table = this.CreateTable();
			base.Notify_ResolutionChanged();
		}

		private PawnTable CreateTable()
		{
			return (PawnTable)Activator.CreateInstance(this.PawnTableDef.workerClass, new object[]
			{
				this.PawnTableDef,
				new Func<IEnumerable<Pawn>>(() => this.Pawns),
				UI.screenWidth - (int)(this.Margin * 2f),
				(int)((float)(UI.screenHeight - 35) - this.ExtraBottomSpace - this.ExtraTopSpace - this.Margin * 2f)
			});
		}

		protected void SetDirty()
		{
			this.table.SetDirty();
			this.SetInitialSizeAndPosition();
		}

		[CompilerGenerated]
		private IEnumerable<Pawn> <CreateTable>m__0()
		{
			return this.Pawns;
		}
	}
}
