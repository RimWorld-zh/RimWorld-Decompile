using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_AssaultColonySappers : LordToil
	{
		private static readonly FloatRange EscortRadiusRanged = new FloatRange(15f, 19f);

		private static readonly FloatRange EscortRadiusMelee = new FloatRange(23f, 26f);

		private LordToilData_AssaultColonySappers Data
		{
			get
			{
				return (LordToilData_AssaultColonySappers)base.data;
			}
		}

		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		public LordToil_AssaultColonySappers()
		{
			base.data = new LordToilData_AssaultColonySappers();
		}

		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		public override void UpdateAllDuties()
		{
			if (!this.Data.sapperDest.IsValid && base.lord.ownedPawns.Any())
			{
				this.Data.sapperDest = GenAI.RandomRaidDest(base.lord.ownedPawns[0].Position, base.Map);
			}
			List<Pawn> list = null;
			if (this.Data.sapperDest.IsValid)
			{
				list = new List<Pawn>();
				for (int i = 0; i < base.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = base.lord.ownedPawns[i];
					if (pawn.equipment.Primary != null && pawn.equipment.Primary.GetComp<CompEquippable>().AllVerbs.Any((Verb verb) => verb.verbProps.ai_IsBuildingDestroyer))
					{
						list.Add(pawn);
					}
				}
				if (list.Count == 0 && base.lord.ownedPawns.Count >= 2)
				{
					list.Add(base.lord.ownedPawns[0]);
				}
			}
			for (int j = 0; j < base.lord.ownedPawns.Count; j++)
			{
				Pawn pawn2 = base.lord.ownedPawns[j];
				if (list != null && list.Contains(pawn2))
				{
					pawn2.mindState.duty = new PawnDuty(DutyDefOf.Sapper, this.Data.sapperDest, -1f);
				}
				else if (!list.NullOrEmpty())
				{
					float radius = (pawn2.equipment == null || pawn2.equipment.Primary == null || !pawn2.equipment.Primary.def.IsRangedWeapon) ? LordToil_AssaultColonySappers.EscortRadiusMelee.RandomInRange : LordToil_AssaultColonySappers.EscortRadiusRanged.RandomInRange;
					pawn2.mindState.duty = new PawnDuty(DutyDefOf.Escort, list.RandomElement(), radius);
				}
				else
				{
					pawn2.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				}
			}
		}

		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.Data.sapperDest = IntVec3.Invalid;
			this.UpdateAllDuties();
		}
	}
}
