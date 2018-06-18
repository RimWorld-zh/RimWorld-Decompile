using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000876 RID: 2166
	public abstract class MainTabWindow_PawnTable : MainTabWindow
	{
		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06003148 RID: 12616 RVA: 0x001A9E38 File Offset: 0x001A8238
		protected virtual float ExtraBottomSpace
		{
			get
			{
				return 53f;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06003149 RID: 12617 RVA: 0x001A9E54 File Offset: 0x001A8254
		protected virtual float ExtraTopSpace
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x0600314A RID: 12618
		protected abstract PawnTableDef PawnTableDef { get; }

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x0600314B RID: 12619 RVA: 0x001A9E70 File Offset: 0x001A8270
		protected override float Margin
		{
			get
			{
				return 6f;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x0600314C RID: 12620 RVA: 0x001A9E8C File Offset: 0x001A828C
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

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x0600314D RID: 12621 RVA: 0x001A9F0C File Offset: 0x001A830C
		protected virtual IEnumerable<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.FreeColonists;
			}
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x001A9F30 File Offset: 0x001A8330
		public override void PostOpen()
		{
			if (this.table == null)
			{
				this.table = this.CreateTable();
			}
			this.SetDirty();
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x001A9F50 File Offset: 0x001A8350
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			this.table.PawnTableOnGUI(new Vector2(rect.x, rect.y + this.ExtraTopSpace));
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x001A9F7F File Offset: 0x001A837F
		public void Notify_PawnsChanged()
		{
			this.SetDirty();
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x001A9F88 File Offset: 0x001A8388
		public override void Notify_ResolutionChanged()
		{
			this.table = this.CreateTable();
			base.Notify_ResolutionChanged();
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x001A9FA0 File Offset: 0x001A83A0
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

		// Token: 0x06003153 RID: 12627 RVA: 0x001AA030 File Offset: 0x001A8430
		protected void SetDirty()
		{
			this.table.SetDirty();
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x04001AA2 RID: 6818
		private PawnTable table;
	}
}
