using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000195 RID: 405
	public class LordToil_KidnapCover : LordToil_DoOpportunisticTaskOrCover
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x0004FE34 File Offset: 0x0004E234
		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Kidnap;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x0004FE50 File Offset: 0x0004E250
		public override bool ForceHighStoryDanger
		{
			get
			{
				return this.cover;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x0004FE6C File Offset: 0x0004E26C
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0004FE84 File Offset: 0x0004E284
		protected override bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets)
		{
			bool result;
			if (pawn.mindState.duty != null && pawn.mindState.duty.def == this.DutyDef && pawn.carryTracker.CarriedThing is Pawn)
			{
				target = pawn.carryTracker.CarriedThing;
				result = true;
			}
			else
			{
				Pawn pawn2;
				bool flag = KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, alreadyTakenTargets);
				target = pawn2;
				result = flag;
			}
			return result;
		}
	}
}
