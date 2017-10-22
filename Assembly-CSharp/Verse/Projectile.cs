using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public abstract class Projectile : ThingWithComps
	{
		protected Vector3 origin;

		protected Vector3 destination;

		protected Thing assignedTarget = null;

		protected Thing intendedTarget = null;

		private bool interceptWallsInt = true;

		private bool freeInterceptInt = true;

		protected ThingDef equipmentDef = null;

		protected Thing launcher = null;

		private Thing neverInterceptTargetInt = null;

		protected bool landed = false;

		protected int ticksToImpact;

		private Sustainer ambientSustainer = null;

		private const float BasePawnInterceptChance = 0.4f;

		private const float PawnInterceptChanceFactor_LayingDown = 0.1f;

		private const float PawnInterceptChanceFactor_NonWildNonEnemy = 0.4f;

		private const float InterceptChanceOnRandomObjectPerFillPercent = 0.07f;

		private const float InterceptDist_Possible = 4f;

		private const float InterceptDist_Short = 7f;

		private const float InterceptDist_Normal = 10f;

		private const float InterceptChanceFactor_VeryShort = 0.5f;

		private const float InterceptChanceFactor_Short = 0.75f;

		private static List<IntVec3> checkedCells = new List<IntVec3>();

		private static readonly List<Thing> cellThingsFiltered = new List<Thing>();

		public bool InterceptWalls
		{
			get
			{
				return base.def.projectile.alwaysFreeIntercept || (!base.def.projectile.flyOverhead && this.interceptWallsInt);
			}
			set
			{
				if (!value && base.def.projectile.alwaysFreeIntercept)
				{
					Log.Error("Tried to set interceptWalls to false on projectile with alwaysFreeIntercept=true");
				}
				else if (value && base.def.projectile.flyOverhead)
				{
					Log.Error("Tried to set interceptWalls to true on a projectile with flyOverhead=true");
				}
				else
				{
					this.interceptWallsInt = value;
					if (!this.interceptWallsInt && this is Projectile_Explosive)
					{
						Log.Message("Non interceptWallsInt explosive.");
					}
				}
			}
		}

		public bool FreeIntercept
		{
			get
			{
				return base.def.projectile.alwaysFreeIntercept || (!base.def.projectile.flyOverhead && this.freeInterceptInt);
			}
			set
			{
				if (!value && base.def.projectile.alwaysFreeIntercept)
				{
					Log.Error("Tried to set FreeIntercept to false on projectile with alwaysFreeIntercept=true");
				}
				else if (value && base.def.projectile.flyOverhead)
				{
					Log.Error("Tried to set FreeIntercept to true on a projectile with flyOverhead=true");
				}
				else
				{
					this.freeInterceptInt = value;
				}
			}
		}

		public Thing ThingToNeverIntercept
		{
			get
			{
				return this.neverInterceptTargetInt;
			}
			set
			{
				if (value.def.Fillage != FillCategory.Full)
				{
					this.neverInterceptTargetInt = value;
				}
			}
		}

		protected int StartingTicksToImpact
		{
			get
			{
				int num = Mathf.RoundToInt((float)((this.origin - this.destination).magnitude / (base.def.projectile.speed / 100.0)));
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
				Vector3 b = (this.destination - this.origin) * (float)(1.0 - (float)this.ticksToImpact / (float)this.StartingTicksToImpact);
				return this.origin + b + Vector3.up * base.def.Altitude;
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
			Scribe_References.Look<Thing>(ref this.assignedTarget, "assignedTarget", false);
			Scribe_References.Look<Thing>(ref this.intendedTarget, "intendedTarget", false);
			Scribe_References.Look<Thing>(ref this.launcher, "launcher", false);
			Scribe_Defs.Look<ThingDef>(ref this.equipmentDef, "equipmentDef");
			Scribe_Values.Look<bool>(ref this.interceptWallsInt, "interceptWalls", true, false);
			Scribe_Values.Look<bool>(ref this.freeInterceptInt, "interceptRandomTargets", true, false);
			Scribe_Values.Look<bool>(ref this.landed, "landed", false, false);
			Scribe_References.Look<Thing>(ref this.neverInterceptTargetInt, "neverInterceptTarget", false);
		}

		public void Launch(Thing launcher, LocalTargetInfo targ, Thing equipment = null)
		{
			this.Launch(launcher, base.Position.ToVector3Shifted(), targ, equipment, null);
		}

		public void Launch(Thing launcher, Vector3 origin, LocalTargetInfo targ, Thing equipment = null, Thing intendedTarget = null)
		{
			this.launcher = launcher;
			this.origin = origin;
			this.intendedTarget = intendedTarget;
			if (equipment != null)
			{
				this.equipmentDef = equipment.def;
			}
			else
			{
				this.equipmentDef = null;
			}
			if (targ.Thing != null)
			{
				this.assignedTarget = targ.Thing;
			}
			this.destination = targ.Cell.ToVector3Shifted() + new Vector3(Rand.Range(-0.3f, 0.3f), 0f, Rand.Range(-0.3f, 0.3f));
			this.ticksToImpact = this.StartingTicksToImpact;
			if (!base.def.projectile.soundAmbient.NullOrUndefined())
			{
				SoundInfo info = SoundInfo.InMap((Thing)this, MaintenanceType.PerTick);
				this.ambientSustainer = base.def.projectile.soundAmbient.TrySpawnSustainer(info);
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
						if (this.ticksToImpact == 60 && Find.TickManager.CurTimeSpeed == TimeSpeed.Normal && base.def.projectile.soundImpactAnticipate != null)
						{
							base.def.projectile.soundImpactAnticipate.PlayOneShot((Thing)this);
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
				bool flag = this.CheckForFreeIntercept(intVec2);
				if (DebugViewSettings.drawInterceptChecks)
				{
					if (flag)
					{
						MoteMaker.ThrowText(intVec2.ToVector3Shifted(), base.Map, "x", -1f);
					}
					else
					{
						MoteMaker.ThrowText(intVec2.ToVector3Shifted(), base.Map, "o", -1f);
					}
				}
				result = flag;
			}
			else if ((float)this.origin.ToIntVec3().DistanceToSquared(intVec2) > 16.0)
			{
				Vector3 vector = lastExactPos;
				Vector3 v = newExactPos - lastExactPos;
				Vector3 b = v.normalized * 0.2f;
				int num = (int)(v.MagnitudeHorizontal() / 0.20000000298023224);
				Projectile.checkedCells.Clear();
				int num2 = 0;
				while (true)
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
					if (DebugViewSettings.drawInterceptChecks)
					{
						MoteMaker.ThrowText(vector, base.Map, "o", -1f);
					}
					num2++;
					if (num2 > num)
						goto IL_01ab;
					if (!(intVec3 == intVec2))
						continue;
					goto IL_01bf;
				}
				if (DebugViewSettings.drawInterceptChecks)
				{
					MoteMaker.ThrowText(vector, base.Map, "x", -1f);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			goto IL_01d3;
			IL_01bf:
			result = false;
			goto IL_01d3;
			IL_01d3:
			return result;
			IL_01ab:
			result = false;
			goto IL_01d3;
		}

		private bool CheckForFreeIntercept(IntVec3 c)
		{
			float num = (c.ToVector3Shifted() - this.origin).MagnitudeHorizontalSquared();
			bool result;
			Thing thing;
			if (num < 16.0)
			{
				result = false;
			}
			else
			{
				List<Thing> list = base.Map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					thing = list[i];
					if (thing != this.ThingToNeverIntercept && thing != this.launcher)
					{
						if (thing.def.Fillage == FillCategory.Full && this.InterceptWalls)
							goto IL_008f;
						if (this.FreeIntercept)
						{
							float num2 = 0f;
							Pawn pawn = thing as Pawn;
							if (pawn != null)
							{
								num2 = 0.4f;
								if (pawn.GetPosture() != 0)
								{
									num2 = (float)(num2 * 0.10000000149011612);
								}
								if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
								{
									num2 = (float)(num2 * 0.40000000596046448);
								}
								num2 *= Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
							}
							else if (thing.def.fillPercent > 0.20000000298023224)
							{
								num2 = (float)(thing.def.fillPercent * 0.070000000298023224);
							}
							if (num2 > 9.9999997473787516E-06)
							{
								if (num < 49.0)
								{
									num2 = (float)(num2 * 0.5);
								}
								else if (num < 100.0)
								{
									num2 = (float)(num2 * 0.75);
								}
								if (DebugViewSettings.drawShooting)
								{
									MoteMaker.ThrowText(this.ExactPosition, base.Map, num2.ToStringPercent(), -1f);
								}
								if (Rand.Value < num2)
									goto IL_01e9;
							}
						}
					}
				}
				result = false;
			}
			goto IL_0213;
			IL_008f:
			this.Impact(thing);
			result = true;
			goto IL_0213;
			IL_01e9:
			this.Impact(thing);
			result = true;
			goto IL_0213;
			IL_0213:
			return result;
		}

		public override void Draw()
		{
			Graphics.DrawMesh(MeshPool.plane10, this.DrawPos, this.ExactRotation, base.def.DrawMatSingle, 0);
			base.Comps_PostDraw();
		}

		private void ImpactSomething()
		{
			if (base.def.projectile.flyOverhead)
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
				if (roofDef != null)
				{
					if (roofDef.isThickRoof)
					{
						base.def.projectile.soundHitThickRoof.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
						this.Destroy(DestroyMode.Vanish);
						return;
					}
					if (base.Position.GetEdifice(base.Map) == null || base.Position.GetEdifice(base.Map).def.Fillage != FillCategory.Full)
					{
						RoofCollapserImmediate.DropRoofInCells(base.Position, base.Map);
					}
				}
			}
			if (this.assignedTarget != null)
			{
				Pawn pawn = this.assignedTarget as Pawn;
				if (pawn != null && pawn.GetPosture() != 0 && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25 && Rand.Value > 0.20000000298023224)
				{
					this.Impact(null);
				}
				else
				{
					this.Impact(this.assignedTarget);
				}
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
						Projectile.cellThingsFiltered.Add(thing);
					}
				}
				Projectile.cellThingsFiltered.Shuffle();
				for (int j = 0; j < Projectile.cellThingsFiltered.Count; j++)
				{
					Thing t = Projectile.cellThingsFiltered[j];
					if (Rand.Value < Projectile.ImpactSomethingHitThingChance(t))
					{
						this.Impact(Projectile.cellThingsFiltered.RandomElement());
						return;
					}
				}
				this.Impact(null);
			}
		}

		private static float ImpactSomethingHitThingChance(Thing t)
		{
			Pawn pawn = t as Pawn;
			return (float)((pawn == null) ? (t.def.fillPercent * 1.5) : (pawn.BodySize * 0.5));
		}

		protected virtual void Impact(Thing hitThing)
		{
			this.Destroy(DestroyMode.Vanish);
		}

		public void ForceInstantImpact()
		{
			if (!this.DestinationCell.InBounds(base.Map))
			{
				this.Destroy(DestroyMode.Vanish);
			}
			else
			{
				this.ticksToImpact = 0;
				base.Position = this.DestinationCell;
				this.ImpactSomething();
			}
		}
	}
}
