using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000188 RID: 392
	public class LordToil_AssaultColonySappers : LordToil
	{
		// Token: 0x06000823 RID: 2083 RVA: 0x0004E804 File Offset: 0x0004CC04
		public LordToil_AssaultColonySappers()
		{
			this.data = new LordToilData_AssaultColonySappers();
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x0004E818 File Offset: 0x0004CC18
		private LordToilData_AssaultColonySappers Data
		{
			get
			{
				return (LordToilData_AssaultColonySappers)this.data;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x0004E838 File Offset: 0x0004CC38
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0004E850 File Offset: 0x0004CC50
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0004E866 File Offset: 0x0004CC66
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0004E87C File Offset: 0x0004CC7C
		public override void UpdateAllDuties()
		{
			if (!this.Data.sapperDest.IsValid && this.lord.ownedPawns.Any<Pawn>())
			{
				this.Data.sapperDest = GenAI.RandomRaidDest(this.lord.ownedPawns[0].Position, base.Map);
			}
			List<Pawn> list = null;
			if (this.Data.sapperDest.IsValid)
			{
				list = new List<Pawn>();
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (SappersUtility.IsGoodSapper(pawn))
					{
						list.Add(pawn);
					}
				}
				if (list.Count == 0 && this.lord.ownedPawns.Count >= 2)
				{
					Pawn pawn2 = null;
					int num = 0;
					for (int j = 0; j < this.lord.ownedPawns.Count; j++)
					{
						if (SappersUtility.IsGoodBackupSapper(this.lord.ownedPawns[j]))
						{
							int level = this.lord.ownedPawns[j].skills.GetSkill(SkillDefOf.Mining).Level;
							if (pawn2 == null || level > num)
							{
								pawn2 = this.lord.ownedPawns[j];
								num = level;
							}
						}
					}
					if (pawn2 != null)
					{
						list.Add(pawn2);
					}
				}
			}
			for (int k = 0; k < this.lord.ownedPawns.Count; k++)
			{
				Pawn pawn3 = this.lord.ownedPawns[k];
				if (list != null && list.Contains(pawn3))
				{
					pawn3.mindState.duty = new PawnDuty(DutyDefOf.Sapper, this.Data.sapperDest, -1f);
				}
				else if (!list.NullOrEmpty<Pawn>())
				{
					float randomInRange;
					if (pawn3.equipment != null && pawn3.equipment.Primary != null && pawn3.equipment.Primary.def.IsRangedWeapon)
					{
						randomInRange = LordToil_AssaultColonySappers.EscortRadiusRanged.RandomInRange;
					}
					else
					{
						randomInRange = LordToil_AssaultColonySappers.EscortRadiusMelee.RandomInRange;
					}
					pawn3.mindState.duty = new PawnDuty(DutyDefOf.Escort, list.RandomElement<Pawn>(), randomInRange);
				}
				else
				{
					pawn3.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				}
			}
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0004EB3A File Offset: 0x0004CF3A
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.Data.sapperDest = IntVec3.Invalid;
			this.UpdateAllDuties();
		}

		// Token: 0x0400037F RID: 895
		private static readonly FloatRange EscortRadiusRanged = new FloatRange(15f, 19f);

		// Token: 0x04000380 RID: 896
		private static readonly FloatRange EscortRadiusMelee = new FloatRange(23f, 26f);
	}
}
