using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000AA9 RID: 2729
	[StaticConstructorOnStartup]
	public sealed class ReservationManager : IExposable
	{
		// Token: 0x04002685 RID: 9861
		private Map map;

		// Token: 0x04002686 RID: 9862
		private List<Reservation> reservations = new List<Reservation>();

		// Token: 0x04002687 RID: 9863
		private static readonly Material DebugReservedThingIcon = MaterialPool.MatFrom("UI/Overlays/ReservedForWork", ShaderDatabase.Cutout);

		// Token: 0x04002688 RID: 9864
		public const int StackCount_All = -1;

		// Token: 0x06003CEA RID: 15594 RVA: 0x002036A8 File Offset: 0x00201AA8
		public ReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x002036C4 File Offset: 0x00201AC4
		public void ExposeData()
		{
			Scribe_Collections.Look<Reservation>(ref this.reservations, "reservations", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.reservations.Count - 1; i >= 0; i--)
				{
					Reservation reservation = this.reservations[i];
					if (reservation.Target.Thing != null && reservation.Target.Thing.Destroyed)
					{
						Log.Error("Loaded reservation with destroyed target: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant != null && reservation.Claimant.Destroyed)
					{
						Log.Error("Loaded reservation with destroyed claimant: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant == null)
					{
						Log.Error("Loaded reservation with null claimant: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
					if (reservation.Job == null)
					{
						Log.Error("Loaded reservation with null job: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
				}
			}
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x00203814 File Offset: 0x00201C14
		public bool CanReserve(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			bool result;
			if (claimant == null)
			{
				Log.Error("CanReserve with claimant==null", false);
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
						if (!(reservation.Target != target))
						{
							if (reservation.Layer == layer)
							{
								if (ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
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
									if (reservation.StackCount == -1)
									{
										num3 = num;
									}
									else
									{
										num3 += reservation.StackCount;
									}
									if (num2 >= maxPawns || num3 + stackCount > num)
									{
										return false;
									}
								}
							}
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x00203A28 File Offset: 0x00201E28
		public int CanReserveStack(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			int result;
			if (claimant == null)
			{
				Log.Error("CanReserve with claimant==null", false);
				result = 0;
			}
			else if (!target.IsValid || (target.HasThing && target.Thing.Destroyed))
			{
				result = 0;
			}
			else if (!claimant.Spawned || claimant.Map != this.map)
			{
				result = 0;
			}
			else if (target.HasThing && target.Thing.Spawned && target.Thing.Map != this.map)
			{
				result = 0;
			}
			else
			{
				int num = 1;
				if (target.HasThing)
				{
					num = target.Thing.stackCount;
				}
				if (ignoreOtherReservations)
				{
					result = num;
				}
				else if (this.map.physicalInteractionReservationManager.IsReserved(target) && !this.map.physicalInteractionReservationManager.IsReservedBy(claimant, target))
				{
					result = 0;
				}
				else
				{
					int num2 = 0;
					int count = this.reservations.Count;
					int num3 = 0;
					for (int i = 0; i < count; i++)
					{
						Reservation reservation = this.reservations[i];
						if (!(reservation.Target != target))
						{
							if (reservation.Layer == layer)
							{
								if (ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
								{
									if (reservation.Claimant != claimant)
									{
										if (reservation.MaxPawns != maxPawns)
										{
											return 0;
										}
										num2++;
										if (reservation.StackCount == -1)
										{
											num3 = num;
										}
										else
										{
											num3 += reservation.StackCount;
										}
										if (num2 >= maxPawns || num3 >= num)
										{
											return 0;
										}
									}
								}
							}
						}
					}
					result = num - num3;
				}
			}
			return result;
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x00203C20 File Offset: 0x00202020
		public bool Reserve(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			if (maxPawns > 1 && stackCount == -1)
			{
				Log.ErrorOnce("Reserving with maxPawns > 1 and stackCount = All; this will not have a useful effect (suppressing future warnings)", 83269, false);
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
				Log.Warning(claimant.ToStringSafe<Pawn>() + " tried to reserve thing " + target.ToStringSafe<LocalTargetInfo>() + " without a valid job", false);
				result = false;
			}
			else if (!this.CanReserve(claimant, target, maxPawns, stackCount, layer, false))
			{
				if (job != null && job.playerForced && this.CanReserve(claimant, target, maxPawns, stackCount, layer, true))
				{
					this.reservations.Add(new Reservation(claimant, job, maxPawns, stackCount, target, layer));
					foreach (Reservation reservation in new List<Reservation>(this.reservations))
					{
						if (reservation.Target == target && reservation.Claimant != claimant && reservation.Layer == layer && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
						{
							reservation.Claimant.jobs.EndJob(reservation.Job, JobCondition.InterruptForced);
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

		// Token: 0x06003CEF RID: 15599 RVA: 0x00203DEC File Offset: 0x002021EC
		public void Release(LocalTargetInfo target, Pawn claimant, Job job)
		{
			if (target.ThingDestroyed)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Releasing destroyed thing ",
					target,
					" for ",
					claimant
				}), false);
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
				Log.Error(string.Concat(new object[]
				{
					"Tried to release ",
					target,
					" that wasn't reserved by ",
					claimant,
					"."
				}), false);
			}
			else
			{
				this.reservations.Remove(reservation);
			}
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x00203EF0 File Offset: 0x002022F0
		public void ReleaseAllForTarget(Thing t)
		{
			if (t != null)
			{
				this.reservations.RemoveAll((Reservation r) => r.Target.Thing == t);
			}
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x00203F34 File Offset: 0x00202334
		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Claimant == claimant && this.reservations[i].Job == job)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x00203F9C File Offset: 0x0020239C
		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Claimant == claimant)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x00203FF0 File Offset: 0x002023F0
		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Claimant == claimant)
				{
					return this.reservations[i].Target;
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x00204058 File Offset: 0x00202458
		public bool IsReservedByAnyoneOf(LocalTargetInfo target, Faction faction)
		{
			int count = this.reservations.Count;
			for (int i = 0; i < count; i++)
			{
				Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant.Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x002040C4 File Offset: 0x002024C4
		public bool IsReservedAndRespected(LocalTargetInfo target, Pawn claimant)
		{
			return this.FirstRespectedReserver(target, claimant) != null;
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x002040E8 File Offset: 0x002024E8
		public Pawn FirstRespectedReserver(LocalTargetInfo target, Pawn claimant)
		{
			int count = this.reservations.Count;
			for (int i = 0; i < count; i++)
			{
				Reservation reservation = this.reservations[i];
				if (reservation.Target == target && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
				{
					return reservation.Claimant;
				}
			}
			return null;
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x0020415C File Offset: 0x0020255C
		public bool ReservedBy(LocalTargetInfo target, Pawn claimant, Job job = null)
		{
			int count = this.reservations.Count;
			for (int i = 0; i < count; i++)
			{
				Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && (job == null || reservation.Job == job))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x002041D8 File Offset: 0x002025D8
		public bool ReservedBy<TDriver>(LocalTargetInfo target, Pawn claimant, LocalTargetInfo? targetAIsNot = null, LocalTargetInfo? targetBIsNot = null, LocalTargetInfo? targetCIsNot = null)
		{
			int count = this.reservations.Count;
			for (int i = 0; i < count; i++)
			{
				Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && reservation.Job != null && reservation.Job.GetCachedDriver(claimant) is TDriver && (targetAIsNot == null || reservation.Job.targetA != targetAIsNot) && (targetBIsNot == null || reservation.Job.targetB != targetBIsNot) && (targetCIsNot == null || reservation.Job.targetC != targetCIsNot))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x0020431C File Offset: 0x0020271C
		public IEnumerable<Thing> AllReservedThings()
		{
			return from res in this.reservations
			select res.Target.Thing;
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x0020435C File Offset: 0x0020275C
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
						return true;
					}
					if (newClaimant.HostFaction == oldClaimant.Faction)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x0020442C File Offset: 0x0020282C
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("All reservation in ReservationManager:");
			for (int i = 0; i < this.reservations.Count; i++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"[",
					i,
					"] ",
					this.reservations[i].ToString()
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x002044B8 File Offset: 0x002028B8
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
						Vector3 s = new Vector3((float)thing.RotatedSize.x, 1f, (float)thing.RotatedSize.z);
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

		// Token: 0x06003CFD RID: 15613 RVA: 0x00204670 File Offset: 0x00202A70
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
			string text2;
			if (target.HasThing && target.Thing.def.stackLimit != 1)
			{
				text2 = "(current stack count: " + target.Thing.stackCount + ")";
			}
			else
			{
				text2 = "";
			}
			string text3 = string.Concat(new object[]
			{
				"Could not reserve ",
				target.ToStringSafe<LocalTargetInfo>(),
				text2,
				" (layer: ",
				layer.ToStringSafe<ReservationLayerDef>(),
				") for ",
				claimant.ToStringSafe<Pawn>(),
				" for job ",
				job.ToStringSafe<Job>(),
				" (now doing job ",
				text,
				"(curToil=",
				num,
				")) for maxPawns ",
				maxPawns,
				" and stackCount ",
				stackCount,
				"."
			});
			Pawn pawn = this.FirstRespectedReserver(target, claimant);
			if (pawn != null)
			{
				string text4 = "null";
				int num2 = -1;
				Job curJob2 = pawn.CurJob;
				if (curJob2 != null)
				{
					text4 = curJob2.ToStringSafe<Job>();
					if (pawn.jobs.curDriver != null)
					{
						num2 = pawn.jobs.curDriver.CurToilIndex;
					}
				}
				string text5 = text3;
				text3 = string.Concat(new object[]
				{
					text5,
					" Existing reserver: ",
					pawn.ToStringSafe<Pawn>(),
					" doing job ",
					text4,
					" (toilIndex=",
					num2,
					")"
				});
			}
			else
			{
				text3 += " No existing reserver.";
			}
			Pawn pawn2 = this.map.physicalInteractionReservationManager.FirstReserverOf(target);
			if (pawn2 != null)
			{
				text3 = text3 + " Physical interaction reserver: " + pawn2.ToStringSafe<Pawn>();
			}
			Log.Error(text3, false);
		}
	}
}
