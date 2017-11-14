using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Bill_ProductionWithUft : Bill_Production
	{
		private UnfinishedThing boundUftInt;

		protected override string StatusString
		{
			get
			{
				if (this.BoundWorker == null)
				{
					return (string.Empty + base.StatusString).Trim();
				}
				return ("BoundWorkerIs".Translate(this.BoundWorker.NameStringShort) + base.StatusString).Trim();
			}
		}

		public Pawn BoundWorker
		{
			get
			{
				if (this.boundUftInt == null)
				{
					return null;
				}
				Pawn creator = this.boundUftInt.Creator;
				if (creator != null && !creator.Downed && creator.HostFaction == null && !creator.Destroyed && creator.Spawned)
				{
					Thing thing = base.billStack.billGiver as Thing;
					if (thing != null)
					{
						WorkTypeDef workTypeDef = null;
						List<WorkGiverDef> allDefsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
						int num = 0;
						while (num < allDefsListForReading.Count)
						{
							if (allDefsListForReading[num].fixedBillGiverDefs == null || !allDefsListForReading[num].fixedBillGiverDefs.Contains(thing.def))
							{
								num++;
								continue;
							}
							workTypeDef = allDefsListForReading[num].workType;
							break;
						}
						if (workTypeDef != null && !creator.workSettings.WorkIsActive(workTypeDef))
						{
							this.boundUftInt = null;
							return null;
						}
					}
					return creator;
				}
				this.boundUftInt = null;
				return null;
			}
		}

		public UnfinishedThing BoundUft
		{
			get
			{
				return this.boundUftInt;
			}
		}

		public Bill_ProductionWithUft()
		{
		}

		public Bill_ProductionWithUft(RecipeDef recipe)
			: base(recipe)
		{
		}

		public void SetBoundUft(UnfinishedThing value, bool setOtherLink = true)
		{
			if (value != this.boundUftInt)
			{
				UnfinishedThing unfinishedThing = this.boundUftInt;
				this.boundUftInt = value;
				if (setOtherLink)
				{
					if (unfinishedThing != null && unfinishedThing.BoundBill == this)
					{
						unfinishedThing.BoundBill = null;
					}
					if (value != null && value.BoundBill != this)
					{
						this.boundUftInt.BoundBill = this;
					}
				}
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<UnfinishedThing>(ref this.boundUftInt, "boundUft", false);
		}

		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			this.ClearBoundUft();
			base.Notify_IterationCompleted(billDoer, ingredients);
		}

		public void ClearBoundUft()
		{
			this.boundUftInt = null;
		}
	}
}
