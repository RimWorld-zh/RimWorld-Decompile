using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D5C RID: 3420
	public class Pawn_InventoryTracker : IThingHolder, IExposable
	{
		// Token: 0x06004C7A RID: 19578 RVA: 0x0027DAD0 File Offset: 0x0027BED0
		public Pawn_InventoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06004C7B RID: 19579 RVA: 0x0027DAFC File Offset: 0x0027BEFC
		// (set) Token: 0x06004C7C RID: 19580 RVA: 0x0027DB25 File Offset: 0x0027BF25
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
		// (get) Token: 0x06004C7D RID: 19581 RVA: 0x0027DB4C File Offset: 0x0027BF4C
		private bool HasAnyUnloadableThing
		{
			get
			{
				return this.FirstUnloadableThing != default(ThingCount);
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x06004C7E RID: 19582 RVA: 0x0027DB78 File Offset: 0x0027BF78
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
		// (get) Token: 0x06004C7F RID: 19583 RVA: 0x0027DE20 File Offset: 0x0027C220
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x0027DE3C File Offset: 0x0027C23C
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.itemsNotForSale, "itemsNotForSale", LookMode.Reference, new object[0]);
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.unloadEverything, "unloadEverything", false, false);
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x0027DE8D File Offset: 0x0027C28D
		public void InventoryTrackerTick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x0027DEB9 File Offset: 0x0027C2B9
		public void InventoryTrackerTickRare()
		{
			this.innerContainer.ThingOwnerTickRare(true);
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x0027DEC8 File Offset: 0x0027C2C8
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

		// Token: 0x06004C84 RID: 19588 RVA: 0x0027DF80 File Offset: 0x0027C380
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.innerContainer.ClearAndDestroyContents(mode);
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x0027DF90 File Offset: 0x0027C390
		public bool Contains(Thing item)
		{
			return this.innerContainer.Contains(item);
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x0027DFB4 File Offset: 0x0027C3B4
		public bool NotForSale(Thing item)
		{
			return this.itemsNotForSale.Contains(item);
		}

		// Token: 0x06004C87 RID: 19591 RVA: 0x0027DFD5 File Offset: 0x0027C3D5
		public void TryAddItemNotForSale(Thing item)
		{
			if (this.innerContainer.TryAdd(item, false))
			{
				this.itemsNotForSale.Add(item);
			}
		}

		// Token: 0x06004C88 RID: 19592 RVA: 0x0027DFF6 File Offset: 0x0027C3F6
		public void Notify_ItemRemoved(Thing item)
		{
			this.itemsNotForSale.Remove(item);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06004C89 RID: 19593 RVA: 0x0027E024 File Offset: 0x0027C424
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x0027E03F File Offset: 0x0027C43F
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04003312 RID: 13074
		public Pawn pawn;

		// Token: 0x04003313 RID: 13075
		public ThingOwner<Thing> innerContainer;

		// Token: 0x04003314 RID: 13076
		private bool unloadEverything;

		// Token: 0x04003315 RID: 13077
		private List<Thing> itemsNotForSale = new List<Thing>();

		// Token: 0x04003316 RID: 13078
		private static List<ThingDefCount> tmpDrugsToKeep = new List<ThingDefCount>();

		// Token: 0x04003317 RID: 13079
		private static List<Thing> tmpThingList = new List<Thing>();
	}
}
