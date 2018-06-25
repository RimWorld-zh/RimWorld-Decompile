using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D5A RID: 3418
	public class Pawn_InventoryTracker : IThingHolder, IExposable
	{
		// Token: 0x0400331B RID: 13083
		public Pawn pawn;

		// Token: 0x0400331C RID: 13084
		public ThingOwner<Thing> innerContainer;

		// Token: 0x0400331D RID: 13085
		private bool unloadEverything;

		// Token: 0x0400331E RID: 13086
		private List<Thing> itemsNotForSale = new List<Thing>();

		// Token: 0x0400331F RID: 13087
		private static List<ThingDefCount> tmpDrugsToKeep = new List<ThingDefCount>();

		// Token: 0x04003320 RID: 13088
		private static List<Thing> tmpThingList = new List<Thing>();

		// Token: 0x06004C90 RID: 19600 RVA: 0x0027F17C File Offset: 0x0027D57C
		public Pawn_InventoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06004C91 RID: 19601 RVA: 0x0027F1A8 File Offset: 0x0027D5A8
		// (set) Token: 0x06004C92 RID: 19602 RVA: 0x0027F1D1 File Offset: 0x0027D5D1
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
		// (get) Token: 0x06004C93 RID: 19603 RVA: 0x0027F1F8 File Offset: 0x0027D5F8
		private bool HasAnyUnloadableThing
		{
			get
			{
				return this.FirstUnloadableThing != default(ThingCount);
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x06004C94 RID: 19604 RVA: 0x0027F224 File Offset: 0x0027D624
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
		// (get) Token: 0x06004C95 RID: 19605 RVA: 0x0027F4CC File Offset: 0x0027D8CC
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x0027F4E8 File Offset: 0x0027D8E8
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.itemsNotForSale, "itemsNotForSale", LookMode.Reference, new object[0]);
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.unloadEverything, "unloadEverything", false, false);
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x0027F539 File Offset: 0x0027D939
		public void InventoryTrackerTick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x0027F565 File Offset: 0x0027D965
		public void InventoryTrackerTickRare()
		{
			this.innerContainer.ThingOwnerTickRare(true);
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x0027F574 File Offset: 0x0027D974
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

		// Token: 0x06004C9A RID: 19610 RVA: 0x0027F62C File Offset: 0x0027DA2C
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.innerContainer.ClearAndDestroyContents(mode);
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x0027F63C File Offset: 0x0027DA3C
		public bool Contains(Thing item)
		{
			return this.innerContainer.Contains(item);
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x0027F660 File Offset: 0x0027DA60
		public bool NotForSale(Thing item)
		{
			return this.itemsNotForSale.Contains(item);
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x0027F681 File Offset: 0x0027DA81
		public void TryAddItemNotForSale(Thing item)
		{
			if (this.innerContainer.TryAdd(item, false))
			{
				this.itemsNotForSale.Add(item);
			}
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x0027F6A2 File Offset: 0x0027DAA2
		public void Notify_ItemRemoved(Thing item)
		{
			this.itemsNotForSale.Remove(item);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x0027F6D0 File Offset: 0x0027DAD0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x0027F6EB File Offset: 0x0027DAEB
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}
	}
}
