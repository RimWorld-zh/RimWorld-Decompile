using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	[StaticConstructorOnStartup]
	public sealed class PawnDestinationReservationManager : IExposable
	{
		public class PawnDestinationReservation : IExposable
		{
			public IntVec3 target;

			public Pawn claimant;

			public Job job;

			public bool obsolete;

			public void ExposeData()
			{
				Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_Values.Look<bool>(ref this.obsolete, "obsolete", false, false);
			}
		}

		public class PawnDestinationSet : IExposable
		{
			public List<PawnDestinationReservation> list = new List<PawnDestinationReservation>();

			public void ExposeData()
			{
				Scribe_Collections.Look<PawnDestinationReservation>(ref this.list, "list", LookMode.Deep, new object[0]);
				if (Scribe.mode == LoadSaveMode.PostLoadInit && this.list.RemoveAll((PawnDestinationReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some destination reservations had null or destroyed claimant.");
				}
			}
		}

		private Dictionary<Faction, PawnDestinationSet> reservedDestinations = new Dictionary<Faction, PawnDestinationSet>();

		private static readonly Material DestinationMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestination");

		private static readonly Material DestinationSelectionMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationSelection");

		private List<Faction> reservedDestinationsKeysWorkingList;

		private List<PawnDestinationSet> reservedDestinationsValuesWorkingList;

		public PawnDestinationReservationManager()
		{
			foreach (Faction allFaction in Find.FactionManager.AllFactions)
			{
				this.RegisterFaction(allFaction);
			}
		}

		public void RegisterFaction(Faction faction)
		{
			this.reservedDestinations.Add(faction, new PawnDestinationSet());
		}

		public void Reserve(Pawn p, Job job, IntVec3 loc)
		{
			if (p.Faction != null)
			{
				this.ObsoleteAllClaimedBy(p);
				this.reservedDestinations[p.Faction].list.Add(new PawnDestinationReservation
				{
					target = loc,
					claimant = p,
					job = job
				});
			}
		}

		public IntVec3 MostRecentReservationFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return IntVec3.Invalid;
			}
			List<PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && !list[i].obsolete)
				{
					return list[i].target;
				}
			}
			return IntVec3.Invalid;
		}

		public IntVec3 FirstObsoleteReservationFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return IntVec3.Invalid;
			}
			List<PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					return list[i].target;
				}
			}
			return IntVec3.Invalid;
		}

		public Job FirstObsoleteReservationJobFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return null;
			}
			List<PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					return list[i].job;
				}
			}
			return null;
		}

		public bool IsReserved(IntVec3 loc)
		{
			foreach (KeyValuePair<Faction, PawnDestinationSet> reservedDestination in this.reservedDestinations)
			{
				List<PawnDestinationReservation> list = reservedDestination.Value.list;
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

		public bool CanReserve(IntVec3 c, Pawn searcher)
		{
			if (searcher.Faction == null)
			{
				return true;
			}
			List<PawnDestinationReservation> list = this.reservedDestinations[searcher.Faction].list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].target == c && list[i].claimant != searcher)
				{
					return false;
				}
			}
			return true;
		}

		public Pawn FirstReserverOf(IntVec3 c, Faction faction)
		{
			if (faction == null)
			{
				return null;
			}
			List<PawnDestinationReservation> list = this.reservedDestinations[faction].list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].target == c)
				{
					return list[i].claimant;
				}
			}
			return null;
		}

		public void ReleaseAllObsoleteClaimedBy(Pawn p)
		{
			if (p.Faction != null)
			{
				List<PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
				int num = 0;
				while (num < list.Count)
				{
					if (list[num].claimant == p && list[num].obsolete)
					{
						list[num] = list[list.Count - 1];
						list.RemoveLast();
					}
					else
					{
						num++;
					}
				}
			}
		}

		public void ReleaseAllClaimedBy(Pawn p)
		{
			if (p.Faction != null)
			{
				List<PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
				int num = 0;
				while (num < list.Count)
				{
					if (list[num].claimant == p)
					{
						list[num] = list[list.Count - 1];
						list.RemoveLast();
					}
					else
					{
						num++;
					}
				}
			}
		}

		public void ReleaseClaimedBy(Pawn p, Job job)
		{
			if (p.Faction != null)
			{
				List<PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].claimant == p && list[i].job == job)
					{
						list[i].job = null;
						if (list[i].obsolete)
						{
							list[i] = list[list.Count - 1];
							list.RemoveLast();
							i--;
						}
					}
				}
			}
		}

		public void ObsoleteAllClaimedBy(Pawn p)
		{
			if (p.Faction != null)
			{
				List<PawnDestinationReservation> list = this.reservedDestinations[p.Faction].list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].claimant == p)
					{
						list[i].obsolete = true;
						if (list[i].job == null)
						{
							list[i] = list[list.Count - 1];
							list.RemoveLast();
							i--;
						}
					}
				}
			}
		}

		public void DebugDrawDestinations()
		{
			foreach (PawnDestinationReservation item in this.reservedDestinations[Faction.OfPlayer].list)
			{
				if (!(item.claimant.Position == item.target))
				{
					IntVec3 target = item.target;
					Vector3 s = new Vector3(1f, 1f, 1f);
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(target.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationMat, 0);
					if (Find.Selector.IsSelected(item.claimant))
					{
						Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationSelectionMat, 0);
					}
				}
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Faction, PawnDestinationSet>(ref this.reservedDestinations, "reservedDestinations", LookMode.Reference, LookMode.Deep, ref this.reservedDestinationsKeysWorkingList, ref this.reservedDestinationsValuesWorkingList);
		}
	}
}
