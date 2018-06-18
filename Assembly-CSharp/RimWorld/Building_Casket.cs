using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A1 RID: 1697
	public class Building_Casket : Building, IThingHolder, IOpenable
	{
		// Token: 0x0600240D RID: 9229 RVA: 0x00132C8B File Offset: 0x0013108B
		public Building_Casket()
		{
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x00132CB0 File Offset: 0x001310B0
		public bool HasAnyContents
		{
			get
			{
				return this.innerContainer.Count > 0;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x00132CD4 File Offset: 0x001310D4
		public Thing ContainedThing
		{
			get
			{
				return (this.innerContainer.Count != 0) ? this.innerContainer[0] : null;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x00132D0C File Offset: 0x0013110C
		public bool CanOpen
		{
			get
			{
				return this.HasAnyContents;
			}
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x00132D28 File Offset: 0x00131128
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x00132D43 File Offset: 0x00131143
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x00132D52 File Offset: 0x00131152
		public override void TickRare()
		{
			base.TickRare();
			this.innerContainer.ThingOwnerTickRare(true);
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x00132D67 File Offset: 0x00131167
		public override void Tick()
		{
			base.Tick();
			this.innerContainer.ThingOwnerTick(true);
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x00132D7C File Offset: 0x0013117C
		public virtual void Open()
		{
			if (this.HasAnyContents)
			{
				this.EjectContents();
			}
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x00132D95 File Offset: 0x00131195
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.contentsKnown, "contentsKnown", false, false);
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x00132DCA File Offset: 0x001311CA
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				this.contentsKnown = true;
			}
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x00132DF8 File Offset: 0x001311F8
		public override bool ClaimableBy(Faction fac)
		{
			bool result;
			if (this.innerContainer.Any)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					if (this.innerContainer[i].Faction == fac)
					{
						return true;
					}
				}
				result = false;
			}
			else
			{
				result = base.ClaimableBy(fac);
			}
			return result;
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x00132E68 File Offset: 0x00131268
		public virtual bool Accepts(Thing thing)
		{
			return this.innerContainer.CanAcceptAnyOf(thing, true);
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x00132E8C File Offset: 0x0013128C
		public virtual bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			bool result;
			if (!this.Accepts(thing))
			{
				result = false;
			}
			else
			{
				bool flag;
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

		// Token: 0x0600241B RID: 9243 RVA: 0x00132F24 File Offset: 0x00131324
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.innerContainer.Count > 0)
			{
				if (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize)
				{
					if (mode != DestroyMode.Deconstruct)
					{
						List<Pawn> list = new List<Pawn>();
						foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null)
							{
								list.Add(pawn);
							}
						}
						foreach (Pawn p in list)
						{
							HealthUtility.DamageUntilDowned(p);
						}
					}
					this.EjectContents();
				}
			}
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			base.Destroy(mode);
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x00133024 File Offset: 0x00131424
		public virtual void EjectContents()
		{
			this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null);
			this.contentsKnown = true;
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x0013304C File Offset: 0x0013144C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			string str;
			if (!this.contentsKnown)
			{
				str = "UnknownLower".Translate();
			}
			else
			{
				str = this.innerContainer.ContentsString;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + "CasketContains".Translate() + ": " + str.CapitalizeFirst();
		}

		// Token: 0x04001416 RID: 5142
		protected ThingOwner innerContainer = null;

		// Token: 0x04001417 RID: 5143
		protected bool contentsKnown = false;
	}
}
