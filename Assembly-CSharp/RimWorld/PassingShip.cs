using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000769 RID: 1897
	public class PassingShip : IExposable, ICommunicable, ILoadReferenceable
	{
		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060029E4 RID: 10724 RVA: 0x00162F24 File Offset: 0x00161324
		public virtual string FullTitle
		{
			get
			{
				return "ErrorFullTitle";
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060029E5 RID: 10725 RVA: 0x00162F40 File Offset: 0x00161340
		public bool Departed
		{
			get
			{
				return this.ticksUntilDeparture <= 0;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060029E6 RID: 10726 RVA: 0x00162F64 File Offset: 0x00161364
		public Map Map
		{
			get
			{
				return (this.passingShipManager == null) ? null : this.passingShipManager.map;
			}
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x00162F95 File Offset: 0x00161395
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.ticksUntilDeparture, "ticksUntilDeparture", 0, false);
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x00162FCE File Offset: 0x001613CE
		public virtual void PassingShipTick()
		{
			this.ticksUntilDeparture--;
			if (this.Departed)
			{
				this.Depart();
			}
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x00162FF0 File Offset: 0x001613F0
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

		// Token: 0x060029EA RID: 10730 RVA: 0x00163060 File Offset: 0x00161460
		public virtual void TryOpenComms(Pawn negotiator)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x00163068 File Offset: 0x00161468
		public virtual string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x00163084 File Offset: 0x00161484
		public string GetInfoText()
		{
			return this.FullTitle;
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x001630A0 File Offset: 0x001614A0
		Faction ICommunicable.GetFaction()
		{
			return null;
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x001630B8 File Offset: 0x001614B8
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

		// Token: 0x060029EF RID: 10735 RVA: 0x0016313C File Offset: 0x0016153C
		public string GetUniqueLoadID()
		{
			return "PassingShip_" + this.loadID;
		}

		// Token: 0x0400169F RID: 5791
		public PassingShipManager passingShipManager;

		// Token: 0x040016A0 RID: 5792
		public string name = "Nameless";

		// Token: 0x040016A1 RID: 5793
		protected int loadID = -1;

		// Token: 0x040016A2 RID: 5794
		public int ticksUntilDeparture = 40000;
	}
}
