using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Building_Casket : Building, IThingHolder, IOpenable
	{
		protected ThingOwner innerContainer = null;

		protected bool contentsKnown = false;

		public bool HasAnyContents
		{
			get
			{
				return this.innerContainer.Count > 0;
			}
		}

		public Thing ContainedThing
		{
			get
			{
				return (this.innerContainer.Count != 0) ? this.innerContainer[0] : null;
			}
		}

		public bool CanOpen
		{
			get
			{
				return this.HasAnyContents;
			}
		}

		public Building_Casket()
		{
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		public override void TickRare()
		{
			base.TickRare();
			this.innerContainer.ThingOwnerTickRare(true);
		}

		public override void Tick()
		{
			base.Tick();
			this.innerContainer.ThingOwnerTick(true);
		}

		public virtual void Open()
		{
			if (this.HasAnyContents)
			{
				this.EjectContents();
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[1]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.contentsKnown, "contentsKnown", false, false);
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				this.contentsKnown = true;
			}
		}

		public override bool ClaimableBy(Faction fac)
		{
			bool result;
			if (this.innerContainer.Any)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					if (this.innerContainer[i].Faction == fac)
						goto IL_0031;
				}
				result = false;
			}
			else
			{
				result = base.ClaimableBy(fac);
			}
			goto IL_0062;
			IL_0062:
			return result;
			IL_0031:
			result = true;
			goto IL_0062;
		}

		public virtual bool Accepts(Thing thing)
		{
			return this.innerContainer.CanAcceptAnyOf(thing, true);
		}

		public virtual bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			bool result;
			if (!this.Accepts(thing))
			{
				result = false;
			}
			else
			{
				bool flag = false;
				if (thing.holdingOwner != null)
				{
					thing.holdingOwner.TryTransferToContainer(thing, this.innerContainer, thing.stackCount, true);
					flag = true;
				}
				else
				{
					flag = this.innerContainer.TryAdd(thing, true);
				}
				if (flag)
				{
					if (thing.Faction != null && thing.Faction.IsPlayer)
					{
						this.contentsKnown = true;
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.innerContainer.Count > 0 && (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize))
			{
				if (mode != DestroyMode.Deconstruct)
				{
					List<Pawn> list = new List<Pawn>();
					foreach (Thing item in (IEnumerable<Thing>)this.innerContainer)
					{
						Pawn pawn = item as Pawn;
						if (pawn != null)
						{
							list.Add(pawn);
						}
					}
					foreach (Pawn item2 in list)
					{
						HealthUtility.DamageUntilDowned(item2);
					}
				}
				this.EjectContents();
			}
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			base.Destroy(mode);
		}

		public virtual void EjectContents()
		{
			this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near);
			this.contentsKnown = true;
		}

		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			string str = this.contentsKnown ? this.innerContainer.ContentsString : "UnknownLower".Translate();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + "CasketContains".Translate() + ": " + str;
		}
	}
}
