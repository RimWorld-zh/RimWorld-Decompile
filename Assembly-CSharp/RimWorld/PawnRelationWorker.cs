using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B7 RID: 695
	public class PawnRelationWorker
	{
		// Token: 0x06000BAB RID: 2987 RVA: 0x00068FA8 File Offset: 0x000673A8
		public virtual bool InRelation(Pawn me, Pawn other)
		{
			if (this.def.implied)
			{
				throw new NotImplementedException(this.def + " lacks InRelation implementation.");
			}
			return me.relations.DirectRelationExists(this.def, other);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00068FF8 File Offset: 0x000673F8
		public virtual float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return 0f;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00069012 File Offset: 0x00067412
		public virtual void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (!this.def.implied)
			{
				generated.relations.AddDirectRelation(this.def, other);
				return;
			}
			throw new NotImplementedException(this.def + " lacks CreateRelation implementation.");
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00069054 File Offset: 0x00067454
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

		// Token: 0x040006C2 RID: 1730
		public PawnRelationDef def;
	}
}
