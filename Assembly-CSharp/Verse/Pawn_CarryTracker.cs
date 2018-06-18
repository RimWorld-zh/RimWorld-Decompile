using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000D57 RID: 3415
	public class Pawn_CarryTracker : IThingHolder, IExposable
	{
		// Token: 0x06004C25 RID: 19493 RVA: 0x0027AC4A File Offset: 0x0027904A
		public Pawn_CarryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep);
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06004C26 RID: 19494 RVA: 0x0027AC68 File Offset: 0x00279068
		public Thing CarriedThing
		{
			get
			{
				Thing result;
				if (this.innerContainer.Count == 0)
				{
					result = null;
				}
				else
				{
					result = this.innerContainer[0];
				}
				return result;
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06004C27 RID: 19495 RVA: 0x0027ACA0 File Offset: 0x002790A0
		public bool Full
		{
			get
			{
				return this.AvailableStackSpace(this.CarriedThing.def) <= 0;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06004C28 RID: 19496 RVA: 0x0027ACCC File Offset: 0x002790CC
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x0027ACE7 File Offset: 0x002790E7
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x0027AD04 File Offset: 0x00279104
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x0027AD1F File Offset: 0x0027911F
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x0027AD30 File Offset: 0x00279130
		public int AvailableStackSpace(ThingDef td)
		{
			int num = this.MaxStackSpaceEver(td);
			if (this.CarriedThing != null)
			{
				num -= this.CarriedThing.stackCount;
			}
			return num;
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x0027AD68 File Offset: 0x00279168
		public int MaxStackSpaceEver(ThingDef td)
		{
			float f = this.pawn.GetStatValue(StatDefOf.CarryingCapacity, true) / td.VolumePerUnit;
			int b = Mathf.RoundToInt(f);
			return Mathf.Min(td.stackLimit, b);
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x0027ADAC File Offset: 0x002791AC
		public bool TryStartCarry(Thing item)
		{
			bool result;
			if (this.pawn.Dead || this.pawn.Downed)
			{
				Log.Error("Dead/downed pawn " + this.pawn + " tried to start carry item.", false);
				result = false;
			}
			else if (this.innerContainer.TryAdd(item, true))
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x0027AE4C File Offset: 0x0027924C
		public int TryStartCarry(Thing item, int count, bool reserve = true)
		{
			int result;
			if (this.pawn.Dead || this.pawn.Downed)
			{
				Log.Error(string.Concat(new object[]
				{
					"Dead/downed pawn ",
					this.pawn,
					" tried to start carry ",
					item.ToStringSafe<Thing>()
				}), false);
				result = 0;
			}
			else
			{
				count = Mathf.Min(count, this.AvailableStackSpace(item.def));
				count = Mathf.Min(count, item.stackCount);
				int num = this.innerContainer.TryAdd(item.SplitOff(count), count, true);
				if (num > 0)
				{
					item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
					if (reserve)
					{
						this.pawn.Reserve(this.CarriedThing, this.pawn.CurJob, 1, -1, null);
					}
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x0027AF54 File Offset: 0x00279354
		public bool TryDropCarriedThing(IntVec3 dropLoc, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			bool result;
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x0027AFC4 File Offset: 0x002793C4
		public bool TryDropCarriedThing(IntVec3 dropLoc, int count, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			bool result;
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, count, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x0027B037 File Offset: 0x00279437
		public void DestroyCarriedThing()
		{
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x0027B046 File Offset: 0x00279446
		public void CarryHandsTick()
		{
			this.innerContainer.ThingOwnerTick(true);
		}

		// Token: 0x040032F8 RID: 13048
		public Pawn pawn;

		// Token: 0x040032F9 RID: 13049
		public ThingOwner<Thing> innerContainer;
	}
}
