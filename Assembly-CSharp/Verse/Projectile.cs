using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public abstract class Projectile : ThingWithComps
	{
		protected Vector3 origin;

		protected Vector3 destination;

		protected LocalTargetInfo usedTarget;

		protected LocalTargetInfo intendedTarget;

		protected ThingDef equipmentDef;

		protected Thing launcher;

		protected ThingDef targetCoverDef;

		private ProjectileHitFlags desiredHitFlags = ProjectileHitFlags.All;

		protected bool landed;

		protected int ticksToImpact;

		private Sustainer ambientSustainer = null;

		private static List<IntVec3> checkedCells = new List<IntVec3>();

		private static readonly List<Thing> cellThingsFiltered = new List<Thing>();

		protected Projectile()
		{
		}

		public ProjectileHitFlags HitFlags
		{
			get
			{
				ProjectileHitFlags result;
				if (this.def.projectile.alwaysFreeIntercept)
				{
					result = ProjectileHitFlags.All;
				}
				else if (this.def.projectile.flyOverhead)
				{
					result = ProjectileHitFlags.None;
				}
				else
				{
					result = this.desiredHitFlags;
				}
				return result;
			}
			set
			{
				this.desiredHitFlags = value;
			}
		}

		protected int StartingTicksToImpact
		{
			get
			{
				int num = Mathf.RoundToInt((this.origin - this.destination).magnitude / (this.def.projectile.speed / 100f));
				if (num < 1)
				{
					num = 1;
				}
				return num;
			}
		}

		protected IntVec3 DestinationCell
		{
			get
			{
				return new IntVec3(this.destination);
			}
		}

		public virtual Vector3 ExactPosition
		{
			get
			{
				Vector3 b = (this.destination - this.origin) * (1f - (float)this.ticksToImpact / (float)this.StartingTicksToImpact);
				return this.origin + b + Vector3.up * this.def.Altitude;
			}
		}

		public virtual Quaternion ExactRotation
		{
			get
			{
				return Quaternion.LookRotation(this.destination - this.origin);
			}
		}

		public override Vector3 DrawPos
		{
			get
			{
				return this.ExactPosition;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
			Scribe_Values.Look<Vector3>(ref this.destination, "destination", default(Vector3), false);
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_TargetInfo.Look(ref this.usedTarget, "usedTarget");
			Scribe_TargetInfo.Look(ref this.intendedTarget, "intendedTarget");
			Scribe_References.Look<Thing>(ref this.launcher, "launcher", false);
			Scribe_Defs.Look<ThingDef>(ref this.equipmentDef, "equipmentDef");
			Scribe_Defs.Look<ThingDef>(ref this.targetCoverDef, "targetCoverDef");
			Scribe_Values.Look<ProjectileHitFlags>(ref this.desiredHitFlags, "desiredHitFlags", ProjectileHitFlags.All, false);
			Scribe_Values.Look<bool>(ref this.landed, "landed", false, false);
		}

		public void Launch(Thing launcher, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, Thing equipment = null)
		{
			this.Launch(launcher, base.Position.ToVector3Shifted(), usedTarget, intendedTarget, hitFlags, equipment, null);
		}

		public void Launch(Thing launcher, Vector3 origin, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, Thing equipment = null, ThingDef targetCoverDef = null)
		{
			this.launcher = launcher;
			this.origin = origin;
			this.usedTarget = usedTarget;
			this.intendedTarget = intendedTarget;
			this.targetCoverDef = targetCoverDef;
			this.HitFlags = hitFlags;
			if (equipment != null)
			{
				this.equipmentDef = equipment.def;
			}
			else
			{
				this.equipmentDef = null;
			}
			this.destination = usedTarget.Cell.ToVector3Shifted() + Gen.RandomHorizontalVector(0.3f);
			this.ticksToImpact = this.StartingTicksToImpact;
			if (!this.def.projectile.soundAmbient.NullOrUndefined())
			{
				SoundInfo info = SoundInfo.InMap(this, MaintenanceType.PerTick);
				this.ambientSustainer = this.def.projectile.soundAmbient.TrySpawnSustainer(info);
			}
		}

		public override void Tick()
		{
			base.Tick();
			if (!this.landed)
			{
				Vector3 exactPosition = this.ExactPosition;
				this.ticksToImpact--;
				if (!this.ExactPosition.InBounds(base.Map))
				{
					this.ticksToImpact++;
					base.Position = this.ExactPosition.ToIntVec3();
					this.Destroy(DestroyMode.Vanish);
				}
				else
				{
					Vector3 exactPosition2 = this.ExactPosition;
					if (!this.CheckForFreeInterceptBetween(exactPosition, exactPosition2))
					{
						base.Position = this.ExactPosition.ToIntVec3();
						if (this.ticksToImpact == 60 && Find.TickManager.CurTimeSpeed == TimeSpeed.Normal && this.def.projectile.soundImpactAnticipate != null)
						{
							this.def.projectile.soundImpactAnticipate.PlayOneShot(this);
						}
						if (this.ticksToImpact <= 0)
						{
							if (this.DestinationCell.InBounds(base.Map))
							{
								base.Position = this.DestinationCell;
							}
							this.ImpactSomething();
						}
						else if (this.ambientSustainer != null)
						{
							this.ambientSustainer.Maintain();
						}
					}
				}
			}
		}

		private bool CheckForFreeInterceptBetween(Vector3 lastExactPos, Vector3 newExactPos)
		{
			IntVec3 intVec = lastExactPos.ToIntVec3();
			IntVec3 intVec2 = newExactPos.ToIntVec3();
			bool result;
			if (intVec2 == intVec)
			{
				result = false;
			}
			else if (!intVec.InBounds(base.Map) || !intVec2.InBounds(base.Map))
			{
				result = false;
			}
			else if (intVec2.AdjacentToCardinal(intVec))
			{
				result = this.CheckForFreeIntercept(intVec2);
			}
			else if (VerbUtility.DistanceInterceptChance(this.origin, intVec2, this.intendedTarget.Cell) > 0f)
			{
				Vector3 vector = lastExactPos;
				Vector3 v = newExactPos - lastExactPos;
				Vector3 b = v.normalized * 0.2f;
				int num = (int)(v.MagnitudeHorizontal() / 0.2f);
				Projectile.checkedCells.Clear();
				int num2 = 0;
				for (;;)
				{
					vector += b;
					IntVec3 intVec3 = vector.ToIntVec3();
					if (!Projectile.checkedCells.Contains(intVec3))
					{
						if (this.CheckForFreeIntercept(intVec3))
						{
							break;
						}
						Projectile.checkedCells.Add(intVec3);
					}
					num2++;
					if (num2 > num)
					{
						goto Block_7;
					}
					if (intVec3 == intVec2)
					{
						goto Block_8;
					}
				}
				return true;
				Block_7:
				return false;
				Block_8:
				result = false;
			}
			else
			{
				result = false;
			}
			return result;
		}

		private bool CheckForFreeIntercept(IntVec3 c)
		{
			bool result;
			if (this.destination.ToIntVec3() == c)
			{
				result = false;
			}
			else
			{
				float num = VerbUtility.DistanceInterceptChance(this.origin, c, this.intendedTarget.Cell);
				if (num <= 0f)
				{
					result = false;
				}
				else
				{
					bool flag = false;
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Thing thing = thingList[i];
						if (this.CanHit(thing))
						{
							bool flag2 = false;
							if (thing.def.Fillage == FillCategory.Full)
							{
								Building_Door building_Door = thing as Building_Door;
								if (building_Door == null || !building_Door.Open)
								{
									this.ThrowDebugText("int-wall", c);
									this.Impact(thing);
									return true;
								}
								flag2 = true;
							}
							float num2 = 0f;
							Pawn pawn = thing as Pawn;
							if (pawn != null)
							{
								num2 = 0.4f;
								num2 *= Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
								if (pawn.GetPosture() != PawnPosture.Standing)
								{
									num2 *= 0.1f;
								}
								if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
								{
									num2 *= 0.4f;
								}
							}
							else if (thing.def.fillPercent > 0.2f)
							{
								if (flag2)
								{
									num2 = 0.05f;
								}
								else if (this.DestinationCell.AdjacentTo8Way(c))
								{
									num2 = thing.def.fillPercent * 1f;
								}
								else
								{
									num2 = thing.def.fillPercent * 0.15f;
								}
							}
							num2 *= num;
							if (num2 > 1E-05f)
							{
								if (Rand.Chance(num2))
								{
									this.ThrowDebugText("int-" + num2.ToStringPercent(), c);
									this.Impact(thing);
									return true;
								}
								flag = true;
								this.ThrowDebugText(num2.ToStringPercent(), c);
							}
						}
					}
					if (!flag)
					{
						this.ThrowDebugText("o", c);
					}
					result = false;
				}
			}
			return result;
		}

		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), base.Map, text, -1f);
			}
		}

		public override void Draw()
		{
			Graphics.DrawMesh(MeshPool.plane10, this.DrawPos, this.ExactRotation, this.def.DrawMatSingle, 0);
			base.Comps_PostDraw();
		}

		protected bool CanHit(Thing thing)
		{
			bool result;
			if (thing == this.launcher)
			{
				result = false;
			}
			else
			{
				ProjectileHitFlags hitFlags = this.HitFlags;
				if (thing == this.intendedTarget && (hitFlags & ProjectileHitFlags.IntendedTarget) != ProjectileHitFlags.None)
				{
					result = true;
				}
				else
				{
					if (thing != this.intendedTarget)
					{
						if (thing is Pawn)
						{
							if ((hitFlags & ProjectileHitFlags.NonTargetPawns) != ProjectileHitFlags.None)
							{
								return true;
							}
						}
						else if ((hitFlags & ProjectileHitFlags.NonTargetWorld) != ProjectileHitFlags.None)
						{
							return true;
						}
					}
					result = (thing == this.intendedTarget && thing.def.Fillage == FillCategory.Full);
				}
			}
			return result;
		}

		private void ImpactSomething()
		{
			if (this.def.projectile.flyOverhead)
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
				if (roofDef != null)
				{
					if (roofDef.isThickRoof)
					{
						this.ThrowDebugText("hit-thick-roof", base.Position);
						this.def.projectile.soundHitThickRoof.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
						this.Destroy(DestroyMode.Vanish);
						return;
					}
					if (base.Position.GetEdifice(base.Map) == null || base.Position.GetEdifice(base.Map).def.Fillage != FillCategory.Full)
					{
						RoofCollapserImmediate.DropRoofInCells(base.Position, base.Map, null);
					}
				}
			}
			if (this.usedTarget.HasThing && this.CanHit(this.usedTarget.Thing))
			{
				Pawn pawn = this.usedTarget.Thing as Pawn;
				if (pawn != null && pawn.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f)
				{
					if (!Rand.Chance(0.2f))
					{
						this.ThrowDebugText("miss-laying", base.Position);
						this.Impact(null);
						return;
					}
				}
				this.Impact(this.usedTarget.Thing);
			}
			else
			{
				Projectile.cellThingsFiltered.Clear();
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant)
					{
						if (this.CanHit(thing))
						{
							Projectile.cellThingsFiltered.Add(thing);
						}
					}
				}
				Projectile.cellThingsFiltered.Shuffle<Thing>();
				for (int j = 0; j < Projectile.cellThingsFiltered.Count; j++)
				{
					Thing thing2 = Projectile.cellThingsFiltered[j];
					Pawn pawn2 = thing2 as Pawn;
					float num;
					if (pawn2 != null)
					{
						num = 0.5f * Mathf.Clamp(pawn2.BodySize, 0.1f, 2f);
					}
					else
					{
						num = 1.5f * thing2.def.fillPercent;
					}
					if (Rand.Chance(num))
					{
						this.ThrowDebugText("hit-" + num.ToStringPercent(), base.Position);
						this.Impact(Projectile.cellThingsFiltered.RandomElement<Thing>());
						return;
					}
					this.ThrowDebugText("miss-" + num.ToStringPercent(), base.Position);
				}
				this.Impact(null);
			}
		}

		protected virtual void Impact(Thing hitThing)
		{
			GenClamor.DoClamor(this, 2.1f, ClamorDefOf.Impact);
			this.Destroy(DestroyMode.Vanish);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Projectile()
		{
		}
	}
}
