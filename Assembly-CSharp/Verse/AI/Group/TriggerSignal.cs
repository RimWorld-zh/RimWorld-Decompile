using System;
using System.Text;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A10 RID: 2576
	public struct TriggerSignal
	{
		// Token: 0x040024AC RID: 9388
		public TriggerSignalType type;

		// Token: 0x040024AD RID: 9389
		public string memo;

		// Token: 0x040024AE RID: 9390
		public Thing thing;

		// Token: 0x040024AF RID: 9391
		public DamageInfo dinfo;

		// Token: 0x040024B0 RID: 9392
		public PawnLostCondition condition;

		// Token: 0x040024B1 RID: 9393
		public Faction faction;

		// Token: 0x040024B2 RID: 9394
		public FactionRelationKind? previousRelationKind;

		// Token: 0x06003996 RID: 14742 RVA: 0x001E8350 File Offset: 0x001E6750
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

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x001E83A0 File Offset: 0x001E67A0
		public Pawn Pawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06003998 RID: 14744 RVA: 0x001E83C0 File Offset: 0x001E67C0
		public static TriggerSignal ForTick
		{
			get
			{
				return new TriggerSignal(TriggerSignalType.Tick);
			}
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x001E83DC File Offset: 0x001E67DC
		public static TriggerSignal ForMemo(string memo)
		{
			return new TriggerSignal(TriggerSignalType.Memo)
			{
				memo = memo
			};
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x001E8404 File Offset: 0x001E6804
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
	}
}
