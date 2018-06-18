using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000DF2 RID: 3570
	public abstract class Projectile : ThingWithComps
	{
		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06004FE7 RID: 20455 RVA: 0x001468E8 File Offset: 0x00144CE8
		// (set) Token: 0x06004FE8 RID: 20456 RVA: 0x0014693B File Offset: 0x00144D3B
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

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06004FE9 RID: 20457 RVA: 0x00146948 File Offset: 0x00144D48
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

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06004FEA RID: 20458 RVA: 0x001469A0 File Offset: 0x00144DA0
		protected IntVec3 DestinationCell
		{
			get
			{
				return new IntVec3(this.destination);
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06004FEB RID: 20459 RVA: 0x001469C0 File Offset: 0x00144DC0
		public virtual Vector3 ExactPosition
		{
			get
			{
				Vector3 b = (this.destination - this.origin) * (1f - (float)this.ticksToImpact / (float)this.StartingTicksToImpact);
				return this.origin + b + Vector3.up * this.def.Altitude;
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06004FEC RID: 20460 RVA: 0x00146A28 File Offset: 0x00144E28
		public virtual Quaternion ExactRotation
		{
			get
			{
				return Quaternion.LookRotation(this.destination - this.origin);
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06004FED RID: 20461 RVA: 0x00146A54 File Offset: 0x00144E54
		public override Vector3 DrawPos
		{
			get
			{
				return this.ExactPosition;
			}
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x00146A70 File Offset: 0x00144E70
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

		// Token: 0x06004FEF RID: 20463 RVA: 0x00146B40 File Offset: 0x00144F40
		public void Launch(Thing launcher, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, Thing equipment = null)
		{
			this.Launch(launcher, base.Position.ToVector3Shifted(), usedTarget, intendedTarget, hitFlags, equipment, null);
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x00146B6C File Offset: 0x00144F6C
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

		// Token: 0x06004FF1 RID: 20465 RVA: 0x00146C3C File Offset: 0x0014503C
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

		// Token: 0x06004FF2 RID: 20466 RVA: 0x00146D80 File Offset: 0x00145180
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
					if (DebugViewSettings.drawInterceptChecks)
					{
						MoteMaker.ThrowText(vector, base.Map, "o", -1f);
					}
					num2++;
					if (num2 > num)
					{
						goto Block_11;
					}
					if (intVec3 == intVec2)
					{
						goto Block_12;
					}
				}
				if (DebugViewSettings.drawInterceptChecks)
				{
					MoteMaker.ThrowText(vector, base.Map, "x", -1f);
				}
				return true;
				Block_11:
				return false;
				Block_12:
				result = false;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x00146F68 File Offset: 0x00145368
		private bool CheckForFreeIntercept(IntVec3 c)
		{
			float num = VerbUtility.DistanceInterceptChance(this.origin, c, this.intendedTarget.Cell);
			bool result;
			if (num <= 0f)
			{
				result = false;
			}
			else
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (this.CanHit(thing))
					{
						if (thing.def.Fillage == FillCategory.Full)
						{
							this.Impact(thing);
							return true;
						}
						float num2 = 0f;
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							num2 = 0.4f;
							if (pawn.GetPosture() != PawnPosture.Standing)
							{
								num2 *= 0.1f;
							}
							if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
							{
								num2 *= 0.4f;
							}
							num2 *= Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						}
						else if (thing.def.fillPercent > 0.2f)
						{
							num2 = thing.def.fillPercent * 0.07f;
						}
						num2 *= num;
						if (num2 > 1E-05f)
						{
							if (DebugViewSettings.drawShooting)
							{
								MoteMaker.ThrowText(this.ExactPosition, base.Map, num2.ToStringPercent(), -1f);
							}
							if (Rand.Chance(num2))
							{
								this.Impact(thing);
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x00147133 File Offset: 0x00145533
		public override void Draw()
		{
			Graphics.DrawMesh(MeshPool.plane10, this.DrawPos, this.ExactRotation, this.def.DrawMatSingle, 0);
			base.Comps_PostDraw();
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x00147160 File Offset: 0x00145560
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

		// Token: 0x06004FF6 RID: 20470 RVA: 0x00147230 File Offset: 0x00145630
		private void ImpactSomething()
		{
			if (this.def.projectile.flyOverhead)
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
				if (roofDef != null)
				{
					if (roofDef.isThickRoof)
					{
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
					Thing t = Projectile.cellThingsFiltered[j];
					if (Rand.Chance(Projectile.ImpactSomethingHitThingChance(t)))
					{
						this.Impact(Projectile.cellThingsFiltered.RandomElement<Thing>());
						return;
					}
				}
				this.Impact(null);
			}
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x001474A8 File Offset: 0x001458A8
		private static float ImpactSomethingHitThingChance(Thing t)
		{
			Pawn pawn = t as Pawn;
			float result;
			if (pawn != null)
			{
				result = pawn.BodySize * 0.5f;
			}
			else
			{
				result = t.def.fillPercent * 1.5f;
			}
			return result;
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x001474ED File Offset: 0x001458ED
		protected virtual void Impact(Thing hitThing)
		{
			GenClamor.DoClamor(this, 2.1f, ClamorDefOf.Impact);
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x00147507 File Offset: 0x00145907
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

		// Token: 0x040034F4 RID: 13556
		protected Vector3 origin;

		// Token: 0x040034F5 RID: 13557
		protected Vector3 destination;

		// Token: 0x040034F6 RID: 13558
		protected LocalTargetInfo usedTarget;

		// Token: 0x040034F7 RID: 13559
		protected LocalTargetInfo intendedTarget;

		// Token: 0x040034F8 RID: 13560
		protected ThingDef equipmentDef;

		// Token: 0x040034F9 RID: 13561
		protected Thing launcher;

		// Token: 0x040034FA RID: 13562
		protected ThingDef targetCoverDef;

		// Token: 0x040034FB RID: 13563
		private ProjectileHitFlags desiredHitFlags = ProjectileHitFlags.All;

		// Token: 0x040034FC RID: 13564
		protected bool landed;

		// Token: 0x040034FD RID: 13565
		protected int ticksToImpact;

		// Token: 0x040034FE RID: 13566
		private Sustainer ambientSustainer = null;

		// Token: 0x040034FF RID: 13567
		private const float BasePawnInterceptChance = 0.4f;

		// Token: 0x04003500 RID: 13568
		private const float PawnInterceptChanceFactor_LayingDown = 0.1f;

		// Token: 0x04003501 RID: 13569
		private const float PawnInterceptChanceFactor_NonWildNonEnemy = 0.4f;

		// Token: 0x04003502 RID: 13570
		private const float InterceptChanceOnRandomObjectPerFillPercent = 0.07f;

		// Token: 0x04003503 RID: 13571
		private static List<IntVec3> checkedCells = new List<IntVec3>();

		// Token: 0x04003504 RID: 13572
		private static readonly List<Thing> cellThingsFiltered = new List<Thing>();
	}
}
