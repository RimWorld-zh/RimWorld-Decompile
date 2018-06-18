using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200061F RID: 1567
	public class DownedRefugeeComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x001122D4 File Offset: 0x001106D4
		protected override string PawnSaveKey
		{
			get
			{
				return "refugee";
			}
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x001122F0 File Offset: 0x001106F0
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			if (this.pawn.Any)
			{
				if (!this.pawn[0].Dead)
				{
					if (this.pawn[0].relations != null)
					{
						this.pawn[0].relations.Notify_FailedRescueQuest();
					}
					HealthUtility.HealNonPermanentInjuriesAndRestoreLegs(this.pawn[0]);
				}
				this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x00112374 File Offset: 0x00110774
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.pawn.Any)
			{
				result = "Refugee".Translate() + ": " + this.pawn[0].LabelCap;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
