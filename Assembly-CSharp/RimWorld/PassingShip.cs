using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000765 RID: 1893
	public class PassingShip : IExposable, ICommunicable, ILoadReferenceable
	{
		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060029DD RID: 10717 RVA: 0x001630FC File Offset: 0x001614FC
		public virtual string FullTitle
		{
			get
			{
				return "ErrorFullTitle";
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x00163118 File Offset: 0x00161518
		public bool Departed
		{
			get
			{
				return this.ticksUntilDeparture <= 0;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060029DF RID: 10719 RVA: 0x0016313C File Offset: 0x0016153C
		public Map Map
		{
			get
			{
				return (this.passingShipManager == null) ? null : this.passingShipManager.map;
			}
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x0016316D File Offset: 0x0016156D
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.ticksUntilDeparture, "ticksUntilDeparture", 0, false);
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x001631A6 File Offset: 0x001615A6
		public virtual void PassingShipTick()
		{
			this.ticksUntilDeparture--;
			if (this.Departed)
			{
				this.Depart();
			}
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x001631C8 File Offset: 0x001615C8
		public virtual void Depart()
		{
			if (this.Map.listerBuildings.ColonistsHaveBuilding((Thing b) => b.def.IsCommsConsole))
			{
				Messages.Message("MessageShipHasLeftCommsRange".Translate(new object[]
				{
					this.FullTitle
				}), MessageTypeDefOf.SituationResolved, true);
			}
			this.passingShipManager.RemoveShip(this);
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x00163238 File Offset: 0x00161638
		public virtual void TryOpenComms(Pawn negotiator)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x00163240 File Offset: 0x00161640
		public virtual string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x0016325C File Offset: 0x0016165C
		public string GetInfoText()
		{
			return this.FullTitle;
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x00163278 File Offset: 0x00161678
		Faction ICommunicable.GetFaction()
		{
			return null;
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x00163290 File Offset: 0x00161690
		public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator)
		{
			string label = "CallOnRadio".Translate(new object[]
			{
				this.GetCallLabel()
			});
			Action action = delegate()
			{
				if (!Building_OrbitalTradeBeacon.AllPowered(this.Map).Any<Building_OrbitalTradeBeacon>())
				{
					Messages.Message("MessageNeedBeaconToTradeWithShip".Translate(), console, MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					console.GiveUseCommsJob(negotiator, this);
				}
			};
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null), negotiator, console, "ReservedBy");
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x00163314 File Offset: 0x00161714
		public string GetUniqueLoadID()
		{
			return "PassingShip_" + this.loadID;
		}

		// Token: 0x0400169D RID: 5789
		public PassingShipManager passingShipManager;

		// Token: 0x0400169E RID: 5790
		public string name = "Nameless";

		// Token: 0x0400169F RID: 5791
		protected int loadID = -1;

		// Token: 0x040016A0 RID: 5792
		public int ticksUntilDeparture = 40000;
	}
}
