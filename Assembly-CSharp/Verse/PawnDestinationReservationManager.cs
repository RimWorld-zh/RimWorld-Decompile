using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000D4D RID: 3405
	[StaticConstructorOnStartup]
	public sealed class PawnDestinationReservationManager : IExposable
	{
		// Token: 0x040032C3 RID: 12995
		private Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet> reservedDestinations = new Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet>();

		// Token: 0x040032C4 RID: 12996
		private static readonly Material DestinationMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestination");

		// Token: 0x040032C5 RID: 12997
		private static readonly Material DestinationSelectionMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationSelection");

		// Token: 0x040032C6 RID: 12998
		private List<Faction> reservedDestinationsKeysWorkingList;

		// Token: 0x040032C7 RID: 12999
		private List<PawnDestinationReservationManager.PawnDestinationSet> reservedDestinationsValuesWorkingList;

		// Token: 0x06004B98 RID: 19352 RVA: 0x00277F88 File Offset: 0x00276388
		public PawnDestinationReservationManager()
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				this.RegisterFaction(faction);
			}
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x00277FFC File Offset: 0x002763FC
		public void RegisterFaction(Faction faction)
		{
			this.reservedDestinations.Add(faction, new PawnDestinationReservationManager.PawnDestinationSet());
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x00278010 File Offset: 0x00276410
		public void Reserve(Pawn p, Job job, IntVec3 loc)
		{
			if (p.Faction != null)
			{
				this.ObsoleteAllClaimedBy(p);
				this.reservedDestinations[p.Faction].list.Add(new PawnDestinationReservationManager.PawnDestinationReservation
				{
					target = loc,
					claimant = p,
					job = job
				});
			}
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x0027806C File Offset: 0x0027646C
		public IntVec3 MostRecentReservationFor(Pawn p)
		{
			IntVec3 invalid;
			if (p.Faction == null)
			{
				invalid = IntVec3.Invalid;
			}
			else
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].claimant == p && !list[i].obsolete)
					{
						return list[i].target;
					}
				}
				invalid = IntVec3.Invalid;
			}
			return invalid;
		}

		// Token: 0x06004B9C RID: 19356 RVA: 0x00278104 File Offset: 0x00276504
		public IntVec3 FirstObsoleteReservationFor(Pawn p)
		{
			IntVec3 invalid;
			if (p.Faction == null)
			{
				invalid = IntVec3.Invalid;
			}
			else
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].claimant == p && list[i].obsolete)
					{
						return list[i].target;
					}
				}
				invalid = IntVec3.Invalid;
			}
			return invalid;
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x0027819C File Offset: 0x0027659C
		public Job FirstObsoleteReservationJobFor(Pawn p)
		{
			Job result;
			if (p.Faction == null)
			{
				result = null;
			}
			else
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].claimant == p && list[i].obsolete)
					{
						return list[i].job;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x0027822C File Offset: 0x0027662C
		public bool IsReserved(IntVec3 loc)
		{
			foreach (KeyValuePair<Faction, PawnDestinationReservationManager.PawnDestinationSet> keyValuePair in this.reservedDestinations)
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = keyValuePair.Value.list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].target == loc)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004B9F RID: 19359 RVA: 0x002782D4 File Offset: 0x002766D4
		public bool CanReserve(IntVec3 c, Pawn searcher, bool draftedOnly = false)
		{
			bool result;
			if (searcher.Faction == null)
			{
				result = true;
			}
			else
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.reservedDestinations[searcher.Faction].list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].target == c && list[i].claimant != searcher && (!draftedOnly || list[i].claimant.Drafted))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x0027837C File Offset: 0x0027677C
		public Pawn FirstReserverOf(IntVec3 c, Faction faction)
		{
			Pawn result;
			if (faction == null)
			{
				result = null;
			}
			else
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.reservedDestinations[faction].list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].target == c)
					{
						return list[i].claimant;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06004BA1 RID: 19361 RVA: 0x002783F4 File Offset: 0x002767F4
		public void ReleaseAllObsoleteClaimedBy(Pawn p)
		{
			if (p.Faction != null)
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
				int i = 0;
				while (i < list.Count)
				{
					if (list[i].claimant == p && list[i].obsolete)
					{
						list[i] = list[list.Count - 1];
						list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06004BA2 RID: 19362 RVA: 0x00278488 File Offset: 0x00276888
		public void ReleaseAllClaimedBy(Pawn p)
		{
			if (p.Faction != null)
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
				int i = 0;
				while (i < list.Count)
				{
					if (list[i].claimant == p)
					{
						list[i] = list[list.Count - 1];
						list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06004BA3 RID: 19363 RVA: 0x0027850C File Offset: 0x0027690C
		public void ReleaseClaimedBy(Pawn p, Job job)
		{
			if (p.Faction != null)
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].claimant == p && list[i].job == job)
					{
						list[i].job = null;
						if (list[i].obsolete)
						{
							list[i] = list[list.Count - 1];
							list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
							i--;
						}
					}
				}
			}
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x002785C8 File Offset: 0x002769C8
		public void ObsoleteAllClaimedBy(Pawn p)
		{
			if (p.Faction != null)
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].claimant == p)
					{
						list[i].obsolete = true;
						if (list[i].job == null)
						{
							list[i] = list[list.Count - 1];
							list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
							i--;
						}
					}
				}
			}
		}

		// Token: 0x06004BA5 RID: 19365 RVA: 0x00278670 File Offset: 0x00276A70
		public void DebugDrawDestinations()
		{
			foreach (PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation in this.reservedDestinations[Faction.OfPlayer].list)
			{
				if (!(pawnDestinationReservation.claimant.Position == pawnDestinationReservation.target))
				{
					IntVec3 target = pawnDestinationReservation.target;
					Vector3 s = new Vector3(1f, 1f, 1f);
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(target.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationMat, 0);
					if (Find.Selector.IsSelected(pawnDestinationReservation.claimant))
					{
						Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationSelectionMat, 0);
					}
				}
			}
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x00278770 File Offset: 0x00276B70
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction, PawnDestinationReservationManager.PawnDestinationSet>(ref this.reservedDestinations, "reservedDestinations", LookMode.Reference, LookMode.Deep, ref this.reservedDestinationsKeysWorkingList, ref this.reservedDestinationsValuesWorkingList);
		}

		// Token: 0x02000D4E RID: 3406
		public class PawnDestinationReservation : IExposable
		{
			// Token: 0x040032C8 RID: 13000
			public IntVec3 target;

			// Token: 0x040032C9 RID: 13001
			public Pawn claimant;

			// Token: 0x040032CA RID: 13002
			public Job job;

			// Token: 0x040032CB RID: 13003
			public bool obsolete;

			// Token: 0x06004BA9 RID: 19369 RVA: 0x002787BC File Offset: 0x00276BBC
			public void ExposeData()
			{
				Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_Values.Look<bool>(ref this.obsolete, "obsolete", false, false);
			}
		}

		// Token: 0x02000D4F RID: 3407
		public class PawnDestinationSet : IExposable
		{
			// Token: 0x040032CC RID: 13004
			public List<PawnDestinationReservationManager.PawnDestinationReservation> list = new List<PawnDestinationReservationManager.PawnDestinationReservation>();

			// Token: 0x06004BAB RID: 19371 RVA: 0x0027882C File Offset: 0x00276C2C
			public void ExposeData()
			{
				Scribe_Collections.Look<PawnDestinationReservationManager.PawnDestinationReservation>(ref this.list, "list", LookMode.Deep, new object[0]);
				if (Scribe.mode == LoadSaveMode.PostLoadInit)
				{
					if (this.list.RemoveAll((PawnDestinationReservationManager.PawnDestinationReservation x) => x.claimant.DestroyedOrNull()) != 0)
					{
						Log.Warning("Some destination reservations had null or destroyed claimant.", false);
					}
				}
			}
		}
	}
}
