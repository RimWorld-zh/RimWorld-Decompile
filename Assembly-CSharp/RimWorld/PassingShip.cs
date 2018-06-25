using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class PassingShip : IExposable, ICommunicable, ILoadReferenceable
	{
		public PassingShipManager passingShipManager;

		public string name = "Nameless";

		protected int loadID = -1;

		public int ticksUntilDeparture = 40000;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache0;

		public PassingShip()
		{
		}

		public virtual string FullTitle
		{
			get
			{
				return "ErrorFullTitle";
			}
		}

		public bool Departed
		{
			get
			{
				return this.ticksUntilDeparture <= 0;
			}
		}

		public Map Map
		{
			get
			{
				return (this.passingShipManager == null) ? null : this.passingShipManager.map;
			}
		}

		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.ticksUntilDeparture, "ticksUntilDeparture", 0, false);
		}

		public virtual void PassingShipTick()
		{
			this.ticksUntilDeparture--;
			if (this.Departed)
			{
				this.Depart();
			}
		}

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

		public virtual void TryOpenComms(Pawn negotiator)
		{
			throw new NotImplementedException();
		}

		public virtual string GetCallLabel()
		{
			return this.name;
		}

		public string GetInfoText()
		{
			return this.FullTitle;
		}

		Faction ICommunicable.GetFaction()
		{
			return null;
		}

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

		public string GetUniqueLoadID()
		{
			return "PassingShip_" + this.loadID;
		}

		[CompilerGenerated]
		private static bool <Depart>m__0(Thing b)
		{
			return b.def.IsCommsConsole;
		}

		[CompilerGenerated]
		private sealed class <CommFloatMenuOption>c__AnonStorey0
		{
			internal Building_CommsConsole console;

			internal Pawn negotiator;

			internal PassingShip $this;

			public <CommFloatMenuOption>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				if (!Building_OrbitalTradeBeacon.AllPowered(this.$this.Map).Any<Building_OrbitalTradeBeacon>())
				{
					Messages.Message("MessageNeedBeaconToTradeWithShip".Translate(), this.console, MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					this.console.GiveUseCommsJob(this.negotiator, this.$this);
				}
			}
		}
	}
}
