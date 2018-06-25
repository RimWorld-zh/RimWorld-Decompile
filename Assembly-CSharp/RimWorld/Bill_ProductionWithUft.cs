using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B2 RID: 1714
	public class Bill_ProductionWithUft : Bill_Production
	{
		// Token: 0x0400144F RID: 5199
		private UnfinishedThing boundUftInt;

		// Token: 0x060024D0 RID: 9424 RVA: 0x0013B82E File Offset: 0x00139C2E
		public Bill_ProductionWithUft()
		{
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x0013B837 File Offset: 0x00139C37
		public Bill_ProductionWithUft(RecipeDef recipe) : base(recipe)
		{
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x060024D2 RID: 9426 RVA: 0x0013B844 File Offset: 0x00139C44
		protected override string StatusString
		{
			get
			{
				string result;
				if (this.BoundWorker == null)
				{
					result = ("" + base.StatusString).Trim();
				}
				else
				{
					result = ("BoundWorkerIs".Translate(new object[]
					{
						this.BoundWorker.LabelShort
					}) + base.StatusString).Trim();
				}
				return result;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x060024D3 RID: 9427 RVA: 0x0013B8B0 File Offset: 0x00139CB0
		public Pawn BoundWorker
		{
			get
			{
				Pawn result;
				if (this.boundUftInt == null)
				{
					result = null;
				}
				else
				{
					Pawn creator = this.boundUftInt.Creator;
					if (creator == null || creator.Downed || creator.HostFaction != null || creator.Destroyed || !creator.Spawned)
					{
						this.boundUftInt = null;
						result = null;
					}
					else
					{
						Thing thing = this.billStack.billGiver as Thing;
						if (thing != null)
						{
							WorkTypeDef workTypeDef = null;
							List<WorkGiverDef> allDefsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
							for (int i = 0; i < allDefsListForReading.Count; i++)
							{
								if (allDefsListForReading[i].fixedBillGiverDefs != null && allDefsListForReading[i].fixedBillGiverDefs.Contains(thing.def))
								{
									workTypeDef = allDefsListForReading[i].workType;
									break;
								}
							}
							if (workTypeDef != null && !creator.workSettings.WorkIsActive(workTypeDef))
							{
								this.boundUftInt = null;
								return null;
							}
						}
						result = creator;
					}
				}
				return result;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x060024D4 RID: 9428 RVA: 0x0013B9D4 File Offset: 0x00139DD4
		public UnfinishedThing BoundUft
		{
			get
			{
				return this.boundUftInt;
			}
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x0013B9F0 File Offset: 0x00139DF0
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

		// Token: 0x060024D6 RID: 9430 RVA: 0x0013BA5C File Offset: 0x00139E5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<UnfinishedThing>(ref this.boundUftInt, "boundUft", false);
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x0013BA76 File Offset: 0x00139E76
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			this.ClearBoundUft();
			base.Notify_IterationCompleted(billDoer, ingredients);
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x0013BA87 File Offset: 0x00139E87
		public void ClearBoundUft()
		{
			this.boundUftInt = null;
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x0013BA94 File Offset: 0x00139E94
		public override Bill Clone()
		{
			return (Bill_Production)base.Clone();
		}
	}
}
