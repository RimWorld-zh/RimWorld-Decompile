using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000872 RID: 2162
	public abstract class MainTabWindow_PawnTable : MainTabWindow
	{
		// Token: 0x04001AA0 RID: 6816
		private PawnTable table;

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06003141 RID: 12609 RVA: 0x001AA020 File Offset: 0x001A8420
		protected virtual float ExtraBottomSpace
		{
			get
			{
				return 53f;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06003142 RID: 12610 RVA: 0x001AA03C File Offset: 0x001A843C
		protected virtual float ExtraTopSpace
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06003143 RID: 12611
		protected abstract PawnTableDef PawnTableDef { get; }

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06003144 RID: 12612 RVA: 0x001AA058 File Offset: 0x001A8458
		protected override float Margin
		{
			get
			{
				return 6f;
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06003145 RID: 12613 RVA: 0x001AA074 File Offset: 0x001A8474
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

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x001AA0F4 File Offset: 0x001A84F4
		protected virtual IEnumerable<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.FreeColonists;
			}
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x001AA118 File Offset: 0x001A8518
		public override void PostOpen()
		{
			if (this.table == null)
			{
				this.table = this.CreateTable();
			}
			this.SetDirty();
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x001AA138 File Offset: 0x001A8538
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			this.table.PawnTableOnGUI(new Vector2(rect.x, rect.y + this.ExtraTopSpace));
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x001AA167 File Offset: 0x001A8567
		public void Notify_PawnsChanged()
		{
			this.SetDirty();
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x001AA170 File Offset: 0x001A8570
		public override void Notify_ResolutionChanged()
		{
			this.table = this.CreateTable();
			base.Notify_ResolutionChanged();
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x001AA188 File Offset: 0x001A8588
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

		// Token: 0x0600314C RID: 12620 RVA: 0x001AA218 File Offset: 0x001A8618
		protected void SetDirty()
		{
			this.table.SetDirty();
			this.SetInitialSizeAndPosition();
		}
	}
}
