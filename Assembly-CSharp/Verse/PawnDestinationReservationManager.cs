using RimWorld;
using System;
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
				if (((Scribe.mode == LoadSaveMode.PostLoadInit) ? this.list.RemoveAll((Predicate<PawnDestinationReservation>)((PawnDestinationReservation x) => x.claimant.DestroyedOrNull())) : 0) != 0)
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
			IntVec3 result;
			List<PawnDestinationReservation> list;
			int i;
			if (p.Faction == null)
			{
				result = IntVec3.Invalid;
			}
			else
			{
				list = this.reservedDestinations[p.Faction].list;
				for (i = 0; i < list.Count; i++)
				{
					if (list[i].claimant == p && !list[i].obsolete)
						goto IL_0059;
				}
				result = IntVec3.Invalid;
			}
			goto IL_0087;
			IL_0087:
			return result;
			IL_0059:
			result = list[i].target;
			goto IL_0087;
		}

		public IntVec3 FirstObsoleteReservationFor(Pawn p)
		{
			IntVec3 result;
			List<PawnDestinationReservation> list;
			int i;
			if (p.Faction == null)
			{
				result = IntVec3.Invalid;
			}
			else
			{
				list = this.reservedDestinations[p.Faction].list;
				for (i = 0; i < list.Count; i++)
				{
					if (list[i].claimant == p && list[i].obsolete)
						goto IL_0059;
				}
				result = IntVec3.Invalid;
			}
			goto IL_0087;
			IL_0087:
			return result;
			IL_0059:
			result = list[i].target;
			goto IL_0087;
		}

		public Job FirstObsoleteReservationJobFor(Pawn p)
		{
			Job result;
			List<PawnDestinationReservation> list;
			int i;
			if (p.Faction == null)
			{
				result = null;
			}
			else
			{
				list = this.reservedDestinations[p.Faction].list;
				for (i = 0; i < list.Count; i++)
				{
					if (list[i].claimant == p && list[i].obsolete)
						goto IL_0055;
				}
				result = null;
			}
			goto IL_007f;
			IL_007f:
			return result;
			IL_0055:
			result = list[i].job;
			goto IL_007f;
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
			bool result;
			if (searcher.Faction == null)
			{
				result = true;
			}
			else
			{
				List<PawnDestinationReservation> list = this.reservedDestinations[searcher.Faction].list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].target == c && list[i].claimant != searcher)
						goto IL_005b;
				}
				result = true;
			}
			goto IL_007a;
			IL_007a:
			return result;
			IL_005b:
			result = false;
			goto IL_007a;
		}

		public Pawn FirstReserverOf(IntVec3 c, Faction faction)
		{
			Pawn result;
			List<PawnDestinationReservation> list;
			int i;
			if (faction == null)
			{
				result = null;
			}
			else
			{
				list = this.reservedDestinations[faction].list;
				for (i = 0; i < list.Count; i++)
				{
					if (list[i].target == c)
						goto IL_003f;
				}
				result = null;
			}
			goto IL_0069;
			IL_003f:
			result = list[i].claimant;
			goto IL_0069;
			IL_0069:
			return result;
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
