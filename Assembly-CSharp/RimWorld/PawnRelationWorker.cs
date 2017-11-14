using System;
using Verse;

namespace RimWorld
{
	public class PawnRelationWorker
	{
		public PawnRelationDef def;

		public virtual bool InRelation(Pawn me, Pawn other)
		{
			if (this.def.implied)
			{
				throw new NotImplementedException(this.def + " lacks InRelation implementation.");
			}
			return me.relations.DirectRelationExists(this.def, other);
		}

		public virtual float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return 0f;
		}

		public virtual void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (!this.def.implied)
			{
				generated.relations.AddDirectRelation(this.def, other);
				return;
			}
			throw new NotImplementedException(this.def + " lacks CreateRelation implementation.");
		}

		public float BaseGenerationChanceFactor(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			if (generated.Faction != other.Faction)
			{
				num = (float)(num * 0.64999997615814209);
			}
			if (generated.HostileTo(other))
			{
				num = (float)(num * 0.699999988079071);
			}
			if (other.Faction != null && other.Faction.IsPlayer)
			{
				num *= request.ColonistRelationChanceFactor;
			}
			TechLevel techLevel = (generated.Faction != null) ? generated.Faction.def.techLevel : TechLevel.Undefined;
			TechLevel techLevel2 = (other.Faction != null) ? other.Faction.def.techLevel : TechLevel.Undefined;
			if (techLevel != 0 && techLevel2 != 0 && techLevel != techLevel2)
			{
				num = (float)(num * 0.85000002384185791);
			}
			if (techLevel.IsNeolithicOrWorse() && !techLevel2.IsNeolithicOrWorse())
			{
				goto IL_00e3;
			}
			if (!techLevel.IsNeolithicOrWorse() && techLevel2.IsNeolithicOrWorse())
				goto IL_00e3;
			goto IL_00eb;
			IL_00eb:
			return num;
			IL_00e3:
			num = (float)(num * 0.029999999329447746);
			goto IL_00eb;
		}
	}
}
