using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D5B RID: 3419
	public class Pawn_InventoryTracker : IThingHolder, IExposable
	{
		// Token: 0x04003322 RID: 13090
		public Pawn pawn;

		// Token: 0x04003323 RID: 13091
		public ThingOwner<Thing> innerContainer;

		// Token: 0x04003324 RID: 13092
		private bool unloadEverything;

		// Token: 0x04003325 RID: 13093
		private List<Thing> itemsNotForSale = new List<Thing>();

		// Token: 0x04003326 RID: 13094
		private static List<ThingDefCount> tmpDrugsToKeep = new List<ThingDefCount>();

		// Token: 0x04003327 RID: 13095
		private static List<Thing> tmpThingList = new List<Thing>();

		// Token: 0x06004C90 RID: 19600 RVA: 0x0027F45C File Offset: 0x0027D85C
		public Pawn_InventoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06004C91 RID: 19601 RVA: 0x0027F488 File Offset: 0x0027D888
		// (set) Token: 0x06004C92 RID: 19602 RVA: 0x0027F4B1 File Offset: 0x0027D8B1
		public bool UnloadEverything
		{
			get
			{
				return this.unloadEverything && this.HasAnyUnloadableThing;
			}
			set
			{
				if (value && this.HasAnyUnloadableThing)
				{
					this.unloadEverything = true;
				}
				else
				{
					this.unloadEverything = false;
				}
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x06004C93 RID: 19603 RVA: 0x0027F4D8 File Offset: 0x0027D8D8
		private bool HasAnyUnloadableThing
		{
			get
			{
				return this.FirstUnloadableThing != default(ThingCount);
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x06004C94 RID: 19604 RVA: 0x0027F504 File Offset: 0x0027D904
		public ThingCount FirstUnloadableThing
		{
			get
			{
				ThingCount result;
				if (this.innerContainer.Count == 0)
				{
					result = default(ThingCount);
				}
				else if (this.pawn.drugs != null && this.pawn.drugs.CurrentPolicy != null)
				{
					DrugPolicy currentPolicy = this.pawn.drugs.CurrentPolicy;
					Pawn_InventoryTracker.tmpDrugsToKeep.Clear();
					for (int i = 0; i < currentPolicy.Count; i++)
					{
						if (currentPolicy[i].takeToInventory > 0)
						{
							Pawn_InventoryTracker.tmpDrugsToKeep.Add(new ThingDefCount(currentPolicy[i].drug, currentPolicy[i].takeToInventory));
						}
					}
					for (int j = 0; j < this.innerContainer.Count; j++)
					{
						if (!this.innerContainer[j].def.IsDrug)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount);
						}
						int num = -1;
						for (int k = 0; k < Pawn_InventoryTracker.tmpDrugsToKeep.Count; k++)
						{
							if (this.innerContainer[j].def == Pawn_InventoryTracker.tmpDrugsToKeep[k].ThingDef)
							{
								num = k;
								break;
							}
						}
						if (num < 0)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount);
						}
						if (this.innerContainer[j].stackCount > Pawn_InventoryTracker.tmpDrugsToKeep[num].Count)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount - Pawn_InventoryTracker.tmpDrugsToKeep[num].Count);
						}
						Pawn_InventoryTracker.tmpDrugsToKeep[num] = new ThingDefCount(Pawn_InventoryTracker.tmpDrugsToKeep[num].ThingDef, Pawn_InventoryTracker.tmpDrugsToKeep[num].Count - this.innerContainer[j].stackCount);
					}
					result = default(ThingCount);
				}
				else
				{
					result = new ThingCount(this.innerContainer[0], this.innerContainer[0].stackCount);
				}
				return result;
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06004C95 RID: 19605 RVA: 0x0027F7AC File Offset: 0x0027DBAC
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x0027F7C8 File Offset: 0x0027DBC8
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.itemsNotForSale, "itemsNotForSale", LookMode.Reference, new object[0]);
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.unloadEverything, "unloadEverything", false, false);
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x0027F819 File Offset: 0x0027DC19
		public void InventoryTrackerTick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x0027F845 File Offset: 0x0027DC45
		public void InventoryTrackerTickRare()
		{
			this.innerContainer.ThingOwnerTickRare(true);
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x0027F854 File Offset: 0x0027DC54
		public void DropAllNearPawn(IntVec3 pos, bool forbid = false, bool unforbid = false)
		{
			if (this.pawn.MapHeld == null)
			{
				Log.Error("Tried to drop all inventory near pawn but the pawn is unspawned. pawn=" + this.pawn, false);
			}
			else
			{
				Pawn_InventoryTracker.tmpThingList.Clear();
				Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
				for (int i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i++)
				{
					Thing thing;
					this.innerContainer.TryDrop(Pawn_InventoryTracker.tmpThingList[i], pos, this.pawn.MapHeld, ThingPlaceMode.Near, out thing, delegate(Thing t, int unused)
					{
						if (forbid)
						{
							t.SetForbiddenIfOutsideHomeArea();
						}
						if (unforbid)
						{
							t.SetForbidden(false, false);
						}
						if (t.def.IsPleasureDrug)
						{
							LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
						}
					}, null);
				}
			}
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x0027F90C File Offset: 0x0027DD0C
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.innerContainer.ClearAndDestroyContents(mode);
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x0027F91C File Offset: 0x0027DD1C
		public bool Contains(Thing item)
		{
			return this.innerContainer.Contains(item);
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x0027F940 File Offset: 0x0027DD40
		public bool NotForSale(Thing item)
		{
			return this.itemsNotForSale.Contains(item);
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x0027F961 File Offset: 0x0027DD61
		public void TryAddItemNotForSale(Thing item)
		{
			if (this.innerContainer.TryAdd(item, false))
			{
				this.itemsNotForSale.Add(item);
			}
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x0027F982 File Offset: 0x0027DD82
		public void Notify_ItemRemoved(Thing item)
		{
			this.itemsNotForSale.Remove(item);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x0027F9B0 File Offset: 0x0027DDB0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x0027F9CB File Offset: 0x0027DDCB
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}
	}
}
