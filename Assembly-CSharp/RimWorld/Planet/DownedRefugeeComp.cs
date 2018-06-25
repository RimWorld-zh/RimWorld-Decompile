using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200061D RID: 1565
	public class DownedRefugeeComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x001126E0 File Offset: 0x00110AE0
		protected override string PawnSaveKey
		{
			get
			{
				return "refugee";
			}
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x001126FC File Offset: 0x00110AFC
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

		// Token: 0x06001FCB RID: 8139 RVA: 0x00112780 File Offset: 0x00110B80
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
