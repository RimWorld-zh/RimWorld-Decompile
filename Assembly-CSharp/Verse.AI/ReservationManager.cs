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
		private Map map;

		private List<Reservation> reservations = new List<Reservation>();

		private static readonly Material DebugReservedThingIcon = MaterialPool.MatFrom("UI/Overlays/ReservedForWork", ShaderDatabase.Cutout);

		public const int StackCount_All = -1;

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
					if (reservation.Job == null)
					{
						Log.Error("Loaded reservation with null job: " + reservation + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
				}
			}
		}

		public bool CanReserve(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			bool result;
			if (claimant == null)
			{
				Log.Error("CanReserve with claimant==null");
				result = false;
			}
			else if (!target.IsValid || (target.HasThing && target.Thing.Destroyed))
			{
				result = false;
			}
			else if (!claimant.Spawned || claimant.Map != this.map)
			{
				result = false;
			}
			else if (target.HasThing && target.Thing.Spawned && target.Thing.Map != this.map)
			{
				result = false;
			}
			else
			{
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
					result = false;
				}
				else if (ignoreOtherReservations)
				{
					result = true;
				}
				else if (this.map.physicalInteractionReservationManager.IsReserved(target) && !this.map.physicalInteractionReservationManager.IsReservedBy(claimant, target))
				{
					result = false;
				}
				else
				{
					int num2 = 0;
					int count = this.reservations.Count;
					int num3 = 0;
					for (int i = 0; i < count; i++)
					{
						Reservation reservation = this.reservations[i];
						if (!(reservation.Target != target) && reservation.Layer == layer && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
						{
							if (reservation.Claimant == claimant)
								goto IL_0196;
							if (reservation.MaxPawns != maxPawns)
								goto IL_01aa;
							num2++;
							num3 = ((reservation.StackCount != -1) ? (num3 + reservation.StackCount) : num);
							if (num2 < maxPawns && num3 + stackCount <= num)
							{
								continue;
							}
							goto IL_01e8;
						}
					}
					result = true;
				}
			}
			goto IL_0205;
			IL_01aa:
			result = false;
			goto IL_0205;
			IL_0196:
			result = true;
			goto IL_0205;
			IL_01e8:
			result = false;
			goto IL_0205;
			IL_0205:
			return result;
		}

		public bool Reserve(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			if (maxPawns > 1 && stackCount == -1)
			{
				Log.ErrorOnce("Reserving with maxPawns > 1 and stackCount = All; this will not have a useful effect (suppressing future warnings)", 83269);
			}
			bool result;
			if (!target.IsValid)
			{
				result = false;
			}
			else if (this.ReservedBy(target, claimant, job))
			{
				result = true;
			}
			else if (target.ThingDestroyed)
			{
				result = false;
			}
			else if (job == null)
			{
				Log.Warning(claimant + " tried to reserve thing " + target + " without a valid job");
				result = false;
			}
			else if (!this.CanReserve(claimant, target, maxPawns, stackCount, layer, false))
			{
				if (job != null && job.playerForced && this.CanReserve(claimant, target, maxPawns, stackCount, layer, true))
				{
					this.reservations.Add(new Reservation(claimant, job, maxPawns, stackCount, target, layer));
					foreach (Reservation item in new List<Reservation>(this.reservations))
					{
						if (item.Target == target && item.Claimant != claimant && item.Layer == layer && ReservationManager.RespectsReservationsOf(claimant, item.Claimant))
						{
							item.Claimant.jobs.EndJob(item.Job, JobCondition.InterruptForced);
						}
					}
					result = true;
				}
				else
				{
					this.LogCouldNotReserveError(claimant, job, target, maxPawns, stackCount, layer);
					result = false;
				}
			}
			else
			{
				this.reservations.Add(new Reservation(claimant, job, maxPawns, stackCount, target, layer));
				result = true;
			}
			return result;
		}

		public void Release(LocalTargetInfo target, Pawn claimant, Job job)
		{
			if (target.ThingDestroyed)
			{
				Log.Warning("Releasing destroyed thing " + target + " for " + claimant);
			}
			Reservation reservation = null;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				Reservation reservation2 = this.reservations[i];
				if (reservation2.Target == target && reservation2.Claimant == claimant && reservation2.Job == job)
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

		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int num = this.reservations.Count - 1; num >= 0; num--)
			{
				if (this.reservations[num].Claimant == claimant && this.reservations[num].Job == job)
				{
					this.reservations.RemoveAt(num);
				}
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

		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			int num = this.reservations.Count - 1;
			LocalTargetInfo result;
			while (true)
			{
				if (num >= 0)
				{
					if (this.reservations[num].Claimant == claimant)
					{
						result = this.reservations[num].Target;
						break;
					}
					num--;
					continue;
				}
				result = LocalTargetInfo.Invalid;
				break;
			}
			return result;
		}

		public bool IsReservedByAnyoneOf(LocalTargetInfo target, Faction faction)
		{
			int count = this.reservations.Count;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < count)
				{
					Reservation reservation = this.reservations[num];
					if (reservation.Target == target && reservation.Claimant.Faction == faction)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public bool IsReservedAndRespected(LocalTargetInfo target, Pawn claimant)
		{
			return this.FirstRespectedReserver(target, claimant) != null;
		}

		public Pawn FirstRespectedReserver(LocalTargetInfo target, Pawn claimant)
		{
			int count = this.reservations.Count;
			int num = 0;
			Pawn result;
			while (true)
			{
				if (num < count)
				{
					Reservation reservation = this.reservations[num];
					if (reservation.Target == target && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
					{
						result = reservation.Claimant;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public bool ReservedBy(LocalTargetInfo target, Pawn claimant, Job job = null)
		{
			int count = this.reservations.Count;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < count)
				{
					Reservation reservation = this.reservations[num];
					if (reservation.Target == target && reservation.Claimant == claimant && (job == null || reservation.Job == job))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public IEnumerable<Thing> AllReservedThings()
		{
			return from res in this.reservations
			select res.Target.Thing;
		}

		private static bool RespectsReservationsOf(Pawn newClaimant, Pawn oldClaimant)
		{
			bool result;
			if (newClaimant == oldClaimant)
			{
				result = true;
			}
			else if (newClaimant.Faction == null || oldClaimant.Faction == null)
			{
				result = false;
			}
			else if (newClaimant.Faction == oldClaimant.Faction)
			{
				result = true;
			}
			else if (!newClaimant.Faction.HostileTo(oldClaimant.Faction))
			{
				result = true;
			}
			else if (oldClaimant.HostFaction != null && oldClaimant.HostFaction == newClaimant.HostFaction)
			{
				result = true;
			}
			else
			{
				if (newClaimant.HostFaction != null)
				{
					if (oldClaimant.HostFaction != null)
					{
						result = true;
						goto IL_00c2;
					}
					if (newClaimant.HostFaction == oldClaimant.Faction)
					{
						result = true;
						goto IL_00c2;
					}
				}
				result = false;
			}
			goto IL_00c2;
			IL_00c2:
			return result;
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

		private void LogCouldNotReserveError(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns, int stackCount, ReservationLayerDef layer)
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
			Pawn pawn = this.FirstRespectedReserver(target, claimant);
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
			Log.Error("Could not reserve " + target + "/" + layer + " for " + claimant + " for job " + job.ToStringSafe() + " (now doing job " + text + "(curToil=" + num + ")) for maxPawns " + maxPawns + " and stackCount " + stackCount + ". Existing reserver: " + pawn + " doing job " + text2 + "(curToil=" + num2 + ")");
		}
	}
}
