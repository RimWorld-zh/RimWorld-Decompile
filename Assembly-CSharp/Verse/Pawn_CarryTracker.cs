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
		// Token: 0x0400330A RID: 13066
		public Pawn pawn;

		// Token: 0x0400330B RID: 13067
		public ThingOwner<Thing> innerContainer;

		// Token: 0x06004C3D RID: 19517 RVA: 0x0027C5F2 File Offset: 0x0027A9F2
		public Pawn_CarryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep);
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06004C3E RID: 19518 RVA: 0x0027C610 File Offset: 0x0027AA10
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
		// (get) Token: 0x06004C3F RID: 19519 RVA: 0x0027C648 File Offset: 0x0027AA48
		public bool Full
		{
			get
			{
				return this.AvailableStackSpace(this.CarriedThing.def) <= 0;
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06004C40 RID: 19520 RVA: 0x0027C674 File Offset: 0x0027AA74
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x0027C68F File Offset: 0x0027AA8F
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x0027C6AC File Offset: 0x0027AAAC
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x0027C6C7 File Offset: 0x0027AAC7
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x0027C6D8 File Offset: 0x0027AAD8
		public int AvailableStackSpace(ThingDef td)
		{
			int num = this.MaxStackSpaceEver(td);
			if (this.CarriedThing != null)
			{
				num -= this.CarriedThing.stackCount;
			}
			return num;
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x0027C710 File Offset: 0x0027AB10
		public int MaxStackSpaceEver(ThingDef td)
		{
			float f = this.pawn.GetStatValue(StatDefOf.CarryingCapacity, true) / td.VolumePerUnit;
			int b = Mathf.RoundToInt(f);
			return Mathf.Min(td.stackLimit, b);
		}

		// Token: 0x06004C46 RID: 19526 RVA: 0x0027C754 File Offset: 0x0027AB54
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

		// Token: 0x06004C47 RID: 19527 RVA: 0x0027C7F4 File Offset: 0x0027ABF4
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

		// Token: 0x06004C48 RID: 19528 RVA: 0x0027C8FC File Offset: 0x0027ACFC
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

		// Token: 0x06004C49 RID: 19529 RVA: 0x0027C96C File Offset: 0x0027AD6C
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

		// Token: 0x06004C4A RID: 19530 RVA: 0x0027C9DF File Offset: 0x0027ADDF
		public void DestroyCarriedThing()
		{
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x0027C9EE File Offset: 0x0027ADEE
		public void CarryHandsTick()
		{
			this.innerContainer.ThingOwnerTick(true);
		}
	}
}
