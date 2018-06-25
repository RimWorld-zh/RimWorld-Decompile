using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000D4C RID: 3404
	[StaticConstructorOnStartup]
	public sealed class PawnDestinationReservationManager : IExposable
	{
		// Token: 0x040032BC RID: 12988
		private Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet> reservedDestinations = new Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet>();

		// Token: 0x040032BD RID: 12989
		private static readonly Material DestinationMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestination");

		// Token: 0x040032BE RID: 12990
		private static readonly Material DestinationSelectionMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationSelection");

		// Token: 0x040032BF RID: 12991
		private List<Faction> reservedDestinationsKeysWorkingList;

		// Token: 0x040032C0 RID: 12992
		private List<PawnDestinationReservationManager.PawnDestinationSet> reservedDestinationsValuesWorkingList;

		// Token: 0x06004B98 RID: 19352 RVA: 0x00277CA8 File Offset: 0x002760A8
		public PawnDestinationReservationManager()
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				this.RegisterFaction(faction);
			}
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x00277D1C File Offset: 0x0027611C
		public void RegisterFaction(Faction faction)
		{
			this.reservedDestinations.Add(faction, new PawnDestinationReservationManager.PawnDestinationSet());
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x00277D30 File Offset: 0x00276130
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

		// Token: 0x06004B9B RID: 19355 RVA: 0x00277D8C File Offset: 0x0027618C
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

		// Token: 0x06004B9C RID: 19356 RVA: 0x00277E24 File Offset: 0x00276224
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

		// Token: 0x06004B9D RID: 19357 RVA: 0x00277EBC File Offset: 0x002762BC
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

		// Token: 0x06004B9E RID: 19358 RVA: 0x00277F4C File Offset: 0x0027634C
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

		// Token: 0x06004B9F RID: 19359 RVA: 0x00277FF4 File Offset: 0x002763F4
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

		// Token: 0x06004BA0 RID: 19360 RVA: 0x0027809C File Offset: 0x0027649C
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

		// Token: 0x06004BA1 RID: 19361 RVA: 0x00278114 File Offset: 0x00276514
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

		// Token: 0x06004BA2 RID: 19362 RVA: 0x002781A8 File Offset: 0x002765A8
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

		// Token: 0x06004BA3 RID: 19363 RVA: 0x0027822C File Offset: 0x0027662C
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

		// Token: 0x06004BA4 RID: 19364 RVA: 0x002782E8 File Offset: 0x002766E8
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

		// Token: 0x06004BA5 RID: 19365 RVA: 0x00278390 File Offset: 0x00276790
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

		// Token: 0x06004BA6 RID: 19366 RVA: 0x00278490 File Offset: 0x00276890
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction, PawnDestinationReservationManager.PawnDestinationSet>(ref this.reservedDestinations, "reservedDestinations", LookMode.Reference, LookMode.Deep, ref this.reservedDestinationsKeysWorkingList, ref this.reservedDestinationsValuesWorkingList);
		}

		// Token: 0x02000D4D RID: 3405
		public class PawnDestinationReservation : IExposable
		{
			// Token: 0x040032C1 RID: 12993
			public IntVec3 target;

			// Token: 0x040032C2 RID: 12994
			public Pawn claimant;

			// Token: 0x040032C3 RID: 12995
			public Job job;

			// Token: 0x040032C4 RID: 12996
			public bool obsolete;

			// Token: 0x06004BA9 RID: 19369 RVA: 0x002784DC File Offset: 0x002768DC
			public void ExposeData()
			{
				Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_Values.Look<bool>(ref this.obsolete, "obsolete", false, false);
			}
		}

		// Token: 0x02000D4E RID: 3406
		public class PawnDestinationSet : IExposable
		{
			// Token: 0x040032C5 RID: 12997
			public List<PawnDestinationReservationManager.PawnDestinationReservation> list = new List<PawnDestinationReservationManager.PawnDestinationReservation>();

			// Token: 0x06004BAB RID: 19371 RVA: 0x0027854C File Offset: 0x0027694C
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
