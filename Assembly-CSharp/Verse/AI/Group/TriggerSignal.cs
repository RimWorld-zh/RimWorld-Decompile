using System;
using System.Text;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A11 RID: 2577
	public struct TriggerSignal
	{
		// Token: 0x06003995 RID: 14741 RVA: 0x001E7BE4 File Offset: 0x001E5FE4
		public TriggerSignal(TriggerSignalType type)
		{
			this.type = type;
			this.memo = null;
			this.thing = null;
			this.dinfo = default(DamageInfo);
			this.condition = PawnLostCondition.Undefined;
			this.faction = null;
			this.previousRelationKind = null;
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06003996 RID: 14742 RVA: 0x001E7C34 File Offset: 0x001E6034
		public Pawn Pawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x001E7C54 File Offset: 0x001E6054
		public static TriggerSignal ForTick
		{
			get
			{
				return new TriggerSignal(TriggerSignalType.Tick);
			}
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x001E7C70 File Offset: 0x001E6070
		public static TriggerSignal ForMemo(string memo)
		{
			return new TriggerSignal(TriggerSignalType.Memo)
			{
				memo = memo
			};
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x001E7C98 File Offset: 0x001E6098
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			stringBuilder.Append(this.type.ToString());
			if (this.memo != null)
			{
				stringBuilder.Append(", memo=" + this.memo);
			}
			if (this.Pawn != null)
			{
				stringBuilder.Append(", pawn=" + this.Pawn);
			}
			if (this.dinfo.Def != null)
			{
				stringBuilder.Append(", dinfo=" + this.dinfo);
			}
			if (this.condition != PawnLostCondition.Undefined)
			{
				stringBuilder.Append(", condition=" + this.condition);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x040024A0 RID: 9376
		public TriggerSignalType type;

		// Token: 0x040024A1 RID: 9377
		public string memo;

		// Token: 0x040024A2 RID: 9378
		public Thing thing;

		// Token: 0x040024A3 RID: 9379
		public DamageInfo dinfo;

		// Token: 0x040024A4 RID: 9380
		public PawnLostCondition condition;

		// Token: 0x040024A5 RID: 9381
		public Faction faction;

		// Token: 0x040024A6 RID: 9382
		public FactionRelationKind? previousRelationKind;
	}
}
