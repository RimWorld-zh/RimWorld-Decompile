using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse.AI
{
	[StaticConstructorOnStartup]
	public sealed class ReservationManager : IExposable
	{
		public const int StackCount_All = -1;

		private Map map;

		private List<Reservation> reservations = new List<Reservation>();

		private static readonly Material DebugReservedThingIcon = MaterialPool.MatFrom("UI/Overlays/ReservedForWork", ShaderDatabase.Cutout);

		public ReservationManager(Map map)
		{
			this.map = map;
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Reservation>(ref this.reservations, "reservations", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int num = this.reservations.Count - 1; num >= 0; num--)
				{
					Reservation reservation = this.reservations[num];
					if (reservation.Target.Thing != null && reservation.Target.Thing.Destroyed)
					{
						Log.Error("Loaded reservation with destroyed target: " + reservation + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant != null && reservation.Claimant.Destroyed)
					{
						Log.Error("Loaded reservation with destroyed claimant: " + reservation + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant == null)
					{
						Log.Error("Loaded reservation with null claimant: " + reservation + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
				}
			}
		}

		public bool CanReserve(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			if (claimant == null)
			{
				Log.Error("CanReserve with claimant==null");
				return false;
			}
			if (claimant.Spawned && claimant.Map == this.map)
			{
				if (target.HasThing && target.Thing.Spawned && target.Thing.Map != this.map)
				{
					return false;
				}
				int num = 1;
				if (target.HasThing)
				{
					num = target.Thing.stackCount;
				}
				if (stackCount == -1)
				{
					stackCount = num;
				}
				if (stackCount > num)
				{
					return false;
				}
				if (ignoreOtherReservations)
				{
					return true;
				}
				if (this.map.physicalInteractionReservationManager.IsReserved(target) && !this.map.physicalInteractionReservationManager.IsReservedBy(claimant, target))
				{
					return false;
				}
				int num2 = 0;
				int count = this.reservations.Count;
				int num3 = 0;
				for (int i = 0; i < count; i++)
				{
					Reservation reservation = this.reservations[i];
					if (!(reservation.Target != target) && reservation.Layer == layer && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
					{
						if (reservation.Claimant == claimant)
						{
							return true;
						}
						if (reservation.MaxPawns != maxPawns)
						{
							return false;
						}
						num2++;
						num3 = ((reservation.StackCount != -1) ? (num3 + reservation.StackCount) : num);
						if (num2 < maxPawns && num3 + stackCount <= num)
						{
							continue;
						}
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public bool Reserve(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			if (maxPawns > 1 && stackCount == -1)
			{
				Log.ErrorOnce("Reserving with maxPawns > 1 and stackCount = All; this will not have a useful effect (suppressing future warnings)", 83269);
			}
			if (this.ReservedBy(target, claimant))
			{
				return true;
			}
			if (target.ThingDestroyed)
			{
				Log.Warning(claimant + " tried to reserve destroyed thing " + target + " for maxPawns " + maxPawns + " doing job " + ((claimant.CurJob == null) ? "null" : claimant.CurJob.ToString()));
				return false;
			}
			if (!this.CanReserve(claimant, target, maxPawns, stackCount, layer, false))
			{
				if (claimant.CurJob != null && claimant.CurJob.playerForced && this.CanReserve(claimant, target, maxPawns, stackCount, layer, true))
				{
					this.reservations.Add(new Reservation(claimant, maxPawns, stackCount, target, layer));
					List<Reservation>.Enumerator enumerator = new List<Reservation>(this.reservations).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Reservation current = enumerator.Current;
							if (current.Target == target && current.Claimant != claimant && current.Layer == layer && ReservationManager.RespectsReservationsOf(claimant, current.Claimant))
							{
								current.Claimant.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
							}
						}
					}
					finally
					{
						((IDisposable)(object)enumerator).Dispose();
					}
					return true;
				}
				this.LogCouldNotReserveError(claimant, target, maxPawns, stackCount, layer);
				return false;
			}
			this.reservations.Add(new Reservation(claimant, maxPawns, stackCount, target, layer));
			return true;
		}

		public void Release(LocalTargetInfo target, Pawn claimant)
		{
			if (target.ThingDestroyed)
			{
				Log.Warning("Releasing destroyed thing " + target + " for " + claimant);
			}
			Reservation reservation = null;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				Reservation reservation2 = this.reservations[i];
				if (reservation2.Target == target && reservation2.Claimant == claimant)
				{
					reservation = reservation2;
					break;
				}
			}
			if (reservation == null && !target.ThingDestroyed)
			{
				Log.Error("Tried to release " + target + " that wasn't reserved by " + claimant + ".");
			}
			else
			{
				this.reservations.Remove(reservation);
			}
		}

		public void ReleaseAllForTarget(Thing t)
		{
			if (t != null)
			{
				this.reservations.RemoveAll((Predicate<Reservation>)((Reservation r) => r.Target.Thing == t));
			}
		}

		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			for (int num = this.reservations.Count - 1; num >= 0; num--)
			{
				if (this.reservations[num].Claimant == claimant)
				{
					this.reservations.RemoveAt(num);
				}
			}
		}

		public bool IsReserved(LocalTargetInfo target, Faction faction)
		{
			int count = this.reservations.Count;
			for (int num = 0; num < count; num++)
			{
				Reservation reservation = this.reservations[num];
				if (reservation.Target == target && reservation.Claimant.Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsReservedByAnyoneWhoseReservationsRespects(LocalTargetInfo target, Pawn claimant)
		{
			return this.FirstReserverWhoseReservationsRespects(target, claimant) != null;
		}

		public Pawn FirstReserverWhoseReservationsRespects(LocalTargetInfo target, Pawn claimant)
		{
			int count = this.reservations.Count;
			for (int num = 0; num < count; num++)
			{
				Reservation reservation = this.reservations[num];
				if (reservation.Target == target && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
				{
					return reservation.Claimant;
				}
			}
			return null;
		}

		public bool ReservedBy(LocalTargetInfo target, Pawn claimant)
		{
			int count = this.reservations.Count;
			for (int num = 0; num < count; num++)
			{
				Reservation reservation = this.reservations[num];
				if (reservation.Target == target && reservation.Claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<Thing> AllReservedThings()
		{
			return from res in this.reservations
			select res.Target.Thing;
		}

		private static bool RespectsReservationsOf(Pawn newClaimant, Pawn oldClaimant)
		{
			if (newClaimant == oldClaimant)
			{
				return true;
			}
			if (newClaimant.Faction != null && oldClaimant.Faction != null)
			{
				if (newClaimant.Faction == oldClaimant.Faction)
				{
					return true;
				}
				if (!newClaimant.Faction.HostileTo(oldClaimant.Faction))
				{
					return true;
				}
				if (oldClaimant.HostFaction != null && oldClaimant.HostFaction == newClaimant.HostFaction)
				{
					return true;
				}
				if (newClaimant.HostFaction != null)
				{
					if (oldClaimant.HostFaction != null)
					{
						return true;
					}
					if (newClaimant.HostFaction == oldClaimant.Faction)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("All reservation in ReservationManager:");
			for (int i = 0; i < this.reservations.Count; i++)
			{
				stringBuilder.AppendLine("[" + i + "] " + this.reservations[i].ToString());
			}
			return stringBuilder.ToString();
		}

		internal void DebugDrawReservations()
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				Reservation reservation = this.reservations[i];
				if (reservation.Target.Thing != null)
				{
					if (reservation.Target.Thing.Spawned)
					{
						Thing thing = reservation.Target.Thing;
						IntVec2 rotatedSize = thing.RotatedSize;
						float x = (float)rotatedSize.x;
						IntVec2 rotatedSize2 = thing.RotatedSize;
						Vector3 s = new Vector3(x, 1f, (float)rotatedSize2.z);
						Matrix4x4 matrix = default(Matrix4x4);
						matrix.SetTRS(thing.DrawPos + Vector3.up * 0.1f, Quaternion.identity, s);
						Graphics.DrawMesh(MeshPool.plane10, matrix, ReservationManager.DebugReservedThingIcon, 0);
						GenDraw.DrawLineBetween(reservation.Claimant.DrawPos, reservation.Target.Thing.DrawPos);
					}
					else
					{
						Graphics.DrawMesh(MeshPool.plane03, reservation.Claimant.DrawPos + Vector3.up + new Vector3(0.5f, 0f, 0.5f), Quaternion.identity, ReservationManager.DebugReservedThingIcon, 0);
					}
				}
				else
				{
					Graphics.DrawMesh(MeshPool.plane10, reservation.Target.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, ReservationManager.DebugReservedThingIcon, 0);
					GenDraw.DrawLineBetween(reservation.Claimant.DrawPos, reservation.Target.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays));
				}
			}
		}

		private void LogCouldNotReserveError(Pawn claimant, LocalTargetInfo target, int maxPawns, int stackCount, ReservationLayerDef layer)
		{
			Job curJob = claimant.CurJob;
			string text = "null";
			int num = -1;
			if (curJob != null)
			{
				text = curJob.ToString();
				if (claimant.jobs.curDriver != null)
				{
					num = claimant.jobs.curDriver.CurToilIndex;
				}
			}
			Pawn pawn = this.FirstReserverWhoseReservationsRespects(target, claimant);
			string text2 = "null";
			int num2 = -1;
			if (pawn != null)
			{
				Job curJob2 = pawn.CurJob;
				if (curJob2 != null)
				{
					text2 = curJob2.ToString();
					if (pawn.jobs.curDriver != null)
					{
						num2 = pawn.jobs.curDriver.CurToilIndex;
					}
				}
			}
			Log.Error("Could not reserve " + target + "/" + layer + " for " + claimant + " doing job " + text + "(curToil=" + num + ") for maxPawns " + maxPawns + " and stackCount " + stackCount + ". Existing reserver: " + pawn + " doing job " + text2 + "(curToil=" + num2 + ")");
		}
	}
}
