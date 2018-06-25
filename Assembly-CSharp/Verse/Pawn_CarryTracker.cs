using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000D56 RID: 3414
	public class Pawn_CarryTracker : IThingHolder, IExposable
	{
		// Token: 0x04003303 RID: 13059
		public Pawn pawn;

		// Token: 0x04003304 RID: 13060
		public ThingOwner<Thing> innerContainer;

		// Token: 0x06004C3D RID: 19517 RVA: 0x0027C312 File Offset: 0x0027A712
		public Pawn_CarryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep);
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06004C3E RID: 19518 RVA: 0x0027C330 File Offset: 0x0027A730
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

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06004C3F RID: 19519 RVA: 0x0027C368 File Offset: 0x0027A768
		public bool Full
		{
			get
			{
				return this.AvailableStackSpace(this.CarriedThing.def) <= 0;
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06004C40 RID: 19520 RVA: 0x0027C394 File Offset: 0x0027A794
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x0027C3AF File Offset: 0x0027A7AF
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x0027C3CC File Offset: 0x0027A7CC
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x0027C3E7 File Offset: 0x0027A7E7
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x0027C3F8 File Offset: 0x0027A7F8
		public int AvailableStackSpace(ThingDef td)
		{
			int num = this.MaxStackSpaceEver(td);
			if (this.CarriedThing != null)
			{
				num -= this.CarriedThing.stackCount;
			}
			return num;
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x0027C430 File Offset: 0x0027A830
		public int MaxStackSpaceEver(ThingDef td)
		{
			float f = this.pawn.GetStatValue(StatDefOf.CarryingCapacity, true) / td.VolumePerUnit;
			int b = Mathf.RoundToInt(f);
			return Mathf.Min(td.stackLimit, b);
		}

		// Token: 0x06004C46 RID: 19526 RVA: 0x0027C474 File Offset: 0x0027A874
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

		// Token: 0x06004C47 RID: 19527 RVA: 0x0027C514 File Offset: 0x0027A914
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

		// Token: 0x06004C48 RID: 19528 RVA: 0x0027C61C File Offset: 0x0027AA1C
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

		// Token: 0x06004C49 RID: 19529 RVA: 0x0027C68C File Offset: 0x0027AA8C
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

		// Token: 0x06004C4A RID: 19530 RVA: 0x0027C6FF File Offset: 0x0027AAFF
		public void DestroyCarriedThing()
		{
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x0027C70E File Offset: 0x0027AB0E
		public void CarryHandsTick()
		{
			this.innerContainer.ThingOwnerTick(true);
		}
	}
}
