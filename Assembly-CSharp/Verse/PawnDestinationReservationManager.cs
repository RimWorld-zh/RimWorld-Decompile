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
		// Token: 0x06004B80 RID: 19328 RVA: 0x002765E4 File Offset: 0x002749E4
		public PawnDestinationReservationManager()
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				this.RegisterFaction(faction);
			}
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x00276658 File Offset: 0x00274A58
		public void RegisterFaction(Faction faction)
		{
			this.reservedDestinations.Add(faction, new PawnDestinationReservationManager.PawnDestinationSet());
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x0027666C File Offset: 0x00274A6C
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

		// Token: 0x06004B83 RID: 19331 RVA: 0x002766C8 File Offset: 0x00274AC8
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

		// Token: 0x06004B84 RID: 19332 RVA: 0x00276760 File Offset: 0x00274B60
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

		// Token: 0x06004B85 RID: 19333 RVA: 0x002767F8 File Offset: 0x00274BF8
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

		// Token: 0x06004B86 RID: 19334 RVA: 0x00276888 File Offset: 0x00274C88
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

		// Token: 0x06004B87 RID: 19335 RVA: 0x00276930 File Offset: 0x00274D30
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

		// Token: 0x06004B88 RID: 19336 RVA: 0x002769D8 File Offset: 0x00274DD8
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

		// Token: 0x06004B89 RID: 19337 RVA: 0x00276A50 File Offset: 0x00274E50
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

		// Token: 0x06004B8A RID: 19338 RVA: 0x00276AE4 File Offset: 0x00274EE4
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

		// Token: 0x06004B8B RID: 19339 RVA: 0x00276B68 File Offset: 0x00274F68
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

		// Token: 0x06004B8C RID: 19340 RVA: 0x00276C24 File Offset: 0x00275024
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

		// Token: 0x06004B8D RID: 19341 RVA: 0x00276CCC File Offset: 0x002750CC
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

		// Token: 0x06004B8E RID: 19342 RVA: 0x00276DCC File Offset: 0x002751CC
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction, PawnDestinationReservationManager.PawnDestinationSet>(ref this.reservedDestinations, "reservedDestinations", LookMode.Reference, LookMode.Deep, ref this.reservedDestinationsKeysWorkingList, ref this.reservedDestinationsValuesWorkingList);
		}

		// Token: 0x040032B1 RID: 12977
		private Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet> reservedDestinations = new Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet>();

		// Token: 0x040032B2 RID: 12978
		private static readonly Material DestinationMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestination");

		// Token: 0x040032B3 RID: 12979
		private static readonly Material DestinationSelectionMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationSelection");

		// Token: 0x040032B4 RID: 12980
		private List<Faction> reservedDestinationsKeysWorkingList;

		// Token: 0x040032B5 RID: 12981
		private List<PawnDestinationReservationManager.PawnDestinationSet> reservedDestinationsValuesWorkingList;

		// Token: 0x02000D4E RID: 3406
		public class PawnDestinationReservation : IExposable
		{
			// Token: 0x06004B91 RID: 19345 RVA: 0x00276E18 File Offset: 0x00275218
			public void ExposeData()
			{
				Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_Values.Look<bool>(ref this.obsolete, "obsolete", false, false);
			}

			// Token: 0x040032B6 RID: 12982
			public IntVec3 target;

			// Token: 0x040032B7 RID: 12983
			public Pawn claimant;

			// Token: 0x040032B8 RID: 12984
			public Job job;

			// Token: 0x040032B9 RID: 12985
			public bool obsolete;
		}

		// Token: 0x02000D4F RID: 3407
		public class PawnDestinationSet : IExposable
		{
			// Token: 0x06004B93 RID: 19347 RVA: 0x00276E88 File Offset: 0x00275288
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

			// Token: 0x040032BA RID: 12986
			public List<PawnDestinationReservationManager.PawnDestinationReservation> list = new List<PawnDestinationReservationManager.PawnDestinationReservation>();
		}
	}
}
