using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B9 RID: 697
	public class PawnRelationWorker
	{
		// Token: 0x040006C2 RID: 1730
		public PawnRelationDef def;

		// Token: 0x06000BAF RID: 2991 RVA: 0x000690F8 File Offset: 0x000674F8
		public virtual bool InRelation(Pawn me, Pawn other)
		{
			if (this.def.implied)
			{
				throw new NotImplementedException(this.def + " lacks InRelation implementation.");
			}
			return me.relations.DirectRelationExists(this.def, other);
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00069148 File Offset: 0x00067548
		public virtual float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return 0f;
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00069162 File Offset: 0x00067562
		public virtual void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (!this.def.implied)
			{
				generated.relations.AddDirectRelation(this.def, other);
				return;
			}
			throw new NotImplementedException(this.def + " lacks CreateRelation implementation.");
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x000691A4 File Offset: 0x000675A4
		public float BaseGenerationChanceFactor(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			if (generated.Faction != other.Faction)
			{
				num *= 0.65f;
			}
			if (generated.HostileTo(other))
			{
				num *= 0.7f;
			}
			if (other.Faction != null && other.Faction.IsPlayer)
			{
				num *= request.ColonistRelationChanceFactor;
			}
			TechLevel techLevel = (generated.Faction == null) ? TechLevel.Undefined : generated.Faction.def.techLevel;
			TechLevel techLevel2 = (other.Faction == null) ? TechLevel.Undefined : other.Faction.def.techLevel;
			if (techLevel != TechLevel.Undefined && techLevel2 != TechLevel.Undefined && techLevel != techLevel2)
			{
				num *= 0.85f;
			}
			if ((techLevel.IsNeolithicOrWorse() && !techLevel2.IsNeolithicOrWorse()) || (!techLevel.IsNeolithicOrWorse() && techLevel2.IsNeolithicOrWorse()))
			{
				num *= 0.03f;
			}
			return num;
		}
	}
}
