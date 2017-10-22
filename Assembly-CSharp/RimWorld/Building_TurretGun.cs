using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Building_TurretGun : Building_Turret
	{
		private const int TryStartShootSomethingIntervalTicks = 10;

		protected int burstCooldownTicksLeft;

		protected int burstWarmupTicksLeft;

		protected LocalTargetInfo currentTargetInt = LocalTargetInfo.Invalid;

		public bool loaded;

		private bool holdFire;

		private Thing gunInt;

		protected TurretTop top;

		protected CompPowerTrader powerComp;

		protected CompMannable mannableComp;

		public static Material ForcedTargetLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));

		public CompEquippable GunCompEq
		{
			get
			{
				return this.Gun.TryGetComp<CompEquippable>();
			}
		}

		public override LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTargetInt;
			}
		}

		private bool WarmingUp
		{
			get
			{
				return this.burstWarmupTicksLeft > 0;
			}
		}

		public override Verb AttackVerb
		{
			get
			{
				return this.GunCompEq.verbTracker.PrimaryVerb;
			}
		}

		private bool CanSetForcedTarget
		{
			get
			{
				return (base.Faction == Faction.OfPlayer || this.MannedByColonist) && !this.MannedByNonColonist;
			}
		}

		private bool CanToggleHoldFire
		{
			get
			{
				return (base.Faction == Faction.OfPlayer || this.MannedByColonist) && !this.MannedByNonColonist;
			}
		}

		public Thing Gun
		{
			get
			{
				if (this.gunInt == null)
				{
					this.gunInt = ThingMaker.MakeThing(base.def.building.turretGunDef, null);
					List<Verb> allVerbs = this.gunInt.TryGetComp<CompEquippable>().AllVerbs;
					for (int i = 0; i < allVerbs.Count; i++)
					{
						Verb verb = allVerbs[i];
						verb.caster = this;
						verb.castCompleteCallback = new Action(this.BurstComplete);
					}
				}
				return this.gunInt;
			}
		}

		private bool MannedByColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction == Faction.OfPlayer;
			}
		}

		private bool MannedByNonColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction != Faction.OfPlayer;
			}
		}

		public Building_TurretGun()
		{
			this.top = new TurretTop(this);
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.mannableComp = base.GetComp<CompMannable>();
		}

		public override void DeSpawn()
		{
			base.DeSpawn();
			this.ResetCurrentTarget();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.burstCooldownTicksLeft, "burstCooldownTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.burstWarmupTicksLeft, "burstWarmupTicksLeft", 0, false);
			Scribe_TargetInfo.Look(ref this.currentTargetInt, "currentTarget");
			Scribe_Values.Look<bool>(ref this.loaded, "loaded", false, false);
			Scribe_Values.Look<bool>(ref this.holdFire, "holdFire", false, false);
		}

		public override void OrderAttack(LocalTargetInfo targ)
		{
			if (!targ.IsValid)
			{
				if (base.forcedTarget.IsValid)
				{
					this.ResetForcedTarget();
				}
			}
			else if ((targ.Cell - base.Position).LengthHorizontal < this.GunCompEq.PrimaryVerb.verbProps.minRange)
			{
				Messages.Message("MessageTargetBelowMinimumRange".Translate(), (Thing)this, MessageSound.RejectInput);
			}
			else if ((targ.Cell - base.Position).LengthHorizontal > this.GunCompEq.PrimaryVerb.verbProps.range)
			{
				Messages.Message("MessageTargetBeyondMaximumRange".Translate(), (Thing)this, MessageSound.RejectInput);
			}
			else if (base.forcedTarget != targ)
			{
				base.forcedTarget = targ;
				if (this.burstCooldownTicksLeft <= 0)
				{
					this.TryStartShootSomething(false);
				}
			}
		}

		public override void Tick()
		{
			base.Tick();
			if (base.forcedTarget.IsValid && !this.CanSetForcedTarget)
			{
				this.ResetForcedTarget();
			}
			if (!this.CanToggleHoldFire)
			{
				this.holdFire = false;
			}
			if (base.forcedTarget.ThingDestroyed)
			{
				this.ResetForcedTarget();
			}
			if ((this.powerComp == null || this.powerComp.PowerOn) && (this.mannableComp == null || this.mannableComp.MannedNow) && base.Spawned)
			{
				this.GunCompEq.verbTracker.VerbsTick();
				if (!base.stunner.Stunned && this.GunCompEq.PrimaryVerb.state != VerbState.Bursting)
				{
					if (this.WarmingUp)
					{
						this.burstWarmupTicksLeft--;
						if (this.burstWarmupTicksLeft == 0)
						{
							this.BeginBurst();
						}
					}
					else
					{
						if (this.burstCooldownTicksLeft > 0)
						{
							this.burstCooldownTicksLeft--;
						}
						if (this.burstCooldownTicksLeft <= 0 && this.IsHashIntervalTick(10))
						{
							this.TryStartShootSomething(true);
						}
					}
					this.top.TurretTopTick();
				}
			}
			else
			{
				this.ResetCurrentTarget();
			}
		}

		protected void TryStartShootSomething(bool canBeginBurstImmediately)
		{
			if (!base.Spawned || (this.holdFire && this.CanToggleHoldFire) || (this.GunCompEq.PrimaryVerb.verbProps.projectileDef.projectile.flyOverhead && base.Map.roofGrid.Roofed(base.Position)))
			{
				this.ResetCurrentTarget();
			}
			else
			{
				bool isValid = this.currentTargetInt.IsValid;
				if (base.forcedTarget.IsValid)
				{
					this.currentTargetInt = base.forcedTarget;
				}
				else
				{
					this.currentTargetInt = this.TryFindNewTarget();
				}
				if (!isValid && this.currentTargetInt.IsValid)
				{
					SoundDefOf.TurretAcquireTarget.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				if (this.currentTargetInt.IsValid)
				{
					if (base.def.building.turretBurstWarmupTime > 0.0)
					{
						this.burstWarmupTicksLeft = base.def.building.turretBurstWarmupTime.SecondsToTicks();
					}
					else if (canBeginBurstImmediately)
					{
						this.BeginBurst();
					}
					else
					{
						this.burstWarmupTicksLeft = 1;
					}
				}
				else
				{
					this.ResetCurrentTarget();
				}
			}
		}

		protected LocalTargetInfo TryFindNewTarget()
		{
			IAttackTargetSearcher attackTargetSearcher = this.TargSearcher();
			Faction faction = attackTargetSearcher.Thing.Faction;
			float range = this.GunCompEq.PrimaryVerb.verbProps.range;
			float minRange = this.GunCompEq.PrimaryVerb.verbProps.minRange;
			Building t = default(Building);
			if (Rand.Value < 0.5 && this.GunCompEq.PrimaryVerb.verbProps.projectileDef.projectile.flyOverhead && faction.HostileTo(Faction.OfPlayer) && base.Map.listerBuildings.allBuildingsColonist.Where((Func<Building, bool>)delegate(Building x)
			{
				float num = (float)x.Position.DistanceToSquared(base.Position);
				return num > minRange * minRange && num < range * range;
			}).TryRandomElement<Building>(out t))
			{
				return (Thing)t;
			}
			TargetScanFlags targetScanFlags = TargetScanFlags.NeedThreat;
			if (!this.GunCompEq.PrimaryVerb.verbProps.projectileDef.projectile.flyOverhead)
			{
				targetScanFlags = (TargetScanFlags)(byte)((int)targetScanFlags | 3);
				targetScanFlags = (TargetScanFlags)(byte)((int)targetScanFlags | 64);
			}
			if (this.GunCompEq.PrimaryVerb.verbProps.ai_IsIncendiary)
			{
				targetScanFlags = (TargetScanFlags)(byte)((int)targetScanFlags | 16);
			}
			return (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(attackTargetSearcher, new Predicate<Thing>(this.IsValidTarget), range, minRange, targetScanFlags);
		}

		private IAttackTargetSearcher TargSearcher()
		{
			if (this.mannableComp != null && this.mannableComp.MannedNow)
			{
				return this.mannableComp.ManningPawn;
			}
			return this;
		}

		private bool IsValidTarget(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (this.GunCompEq.PrimaryVerb.verbProps.projectileDef.projectile.flyOverhead)
				{
					RoofDef roofDef = base.Map.roofGrid.RoofAt(t.Position);
					if (roofDef != null && roofDef.isThickRoof)
					{
						return false;
					}
				}
				if (this.mannableComp == null)
				{
					return !GenAI.MachinesLike(base.Faction, pawn);
				}
				if (pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer)
				{
					return false;
				}
			}
			return true;
		}

		protected void BeginBurst()
		{
			this.GunCompEq.PrimaryVerb.TryStartCastOn(this.CurrentTarget, false, true);
			base.OnAttackedTarget(this.CurrentTarget);
		}

		protected void BurstComplete()
		{
			if (base.def.building.turretBurstCooldownTime >= 0.0)
			{
				this.burstCooldownTicksLeft = base.def.building.turretBurstCooldownTime.SecondsToTicks();
			}
			else
			{
				this.burstCooldownTicksLeft = this.GunCompEq.PrimaryVerb.verbProps.defaultCooldownTime.SecondsToTicks();
			}
			this.loaded = false;
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			stringBuilder.AppendLine("GunInstalled".Translate() + ": " + this.Gun.Label);
			if (this.GunCompEq.PrimaryVerb.verbProps.minRange > 0.0)
			{
				stringBuilder.AppendLine("MinimumRange".Translate() + ": " + this.GunCompEq.PrimaryVerb.verbProps.minRange.ToString("F0"));
			}
			if (base.Spawned && this.burstCooldownTicksLeft > 0)
			{
				stringBuilder.AppendLine("CanFireIn".Translate() + ": " + this.burstCooldownTicksLeft.TicksToSecondsString());
			}
			if (base.def.building.turretShellDef != null)
			{
				if (this.loaded)
				{
					stringBuilder.AppendLine("ShellLoaded".Translate());
				}
				else
				{
					stringBuilder.AppendLine("ShellNotLoaded".Translate());
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		public override void Draw()
		{
			this.top.DrawTurret();
			base.Draw();
		}

		public override void DrawExtraSelectionOverlays()
		{
			float range = this.GunCompEq.PrimaryVerb.verbProps.range;
			if (range < 90.0)
			{
				GenDraw.DrawRadiusRing(base.Position, range);
			}
			float minRange = this.GunCompEq.PrimaryVerb.verbProps.minRange;
			if (minRange < 90.0 && minRange > 0.10000000149011612)
			{
				GenDraw.DrawRadiusRing(base.Position, minRange);
			}
			if (this.WarmingUp)
			{
				int degreesWide = (int)((float)this.burstWarmupTicksLeft * 0.5);
				GenDraw.DrawAimPie(this, this.CurrentTarget, degreesWide, (float)((float)base.def.size.x * 0.5));
			}
			if (base.forcedTarget.IsValid)
			{
				if (base.forcedTarget.HasThing && !base.forcedTarget.Thing.Spawned)
					return;
				Vector3 b = (!base.forcedTarget.HasThing) ? base.forcedTarget.Cell.ToVector3Shifted() : base.forcedTarget.Thing.TrueCenter();
				Vector3 a = this.TrueCenter();
				b.y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
				a.y = b.y;
				GenDraw.DrawLineBetween(a, b, Building_TurretGun.ForcedTargetLineMat);
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			if (this.CanSetForcedTarget)
			{
				yield return (Gizmo)new Command_VerbTarget
				{
					defaultLabel = "CommandSetForceAttackTarget".Translate(),
					defaultDesc = "CommandSetForceAttackTargetDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/Attack", true),
					verb = this.GunCompEq.PrimaryVerb,
					hotKey = KeyBindingDefOf.Misc4
				};
			}
			if (base.forcedTarget.IsValid)
			{
				Command_Action stop = new Command_Action
				{
					defaultLabel = "CommandStopForceAttack".Translate(),
					defaultDesc = "CommandStopForceAttackDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/Halt", true),
					action = (Action)delegate
					{
						((_003CGetGizmos_003Ec__Iterator148)/*Error near IL_01b6: stateMachine*/)._003C_003Ef__this.ResetForcedTarget();
						SoundDefOf.TickLow.PlayOneShotOnCamera(null);
					}
				};
				if (!base.forcedTarget.IsValid)
				{
					stop.Disable("CommandStopAttackFailNotForceAttacking".Translate());
				}
				stop.hotKey = KeyBindingDefOf.Misc5;
				yield return (Gizmo)stop;
			}
			if (this.CanToggleHoldFire)
			{
				yield return (Gizmo)new Command_Toggle
				{
					defaultLabel = "CommandHoldFire".Translate(),
					defaultDesc = "CommandHoldFireDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/HoldFire", true),
					hotKey = KeyBindingDefOf.Misc6,
					toggleAction = (Action)delegate
					{
						((_003CGetGizmos_003Ec__Iterator148)/*Error near IL_028a: stateMachine*/)._003C_003Ef__this.holdFire = !((_003CGetGizmos_003Ec__Iterator148)/*Error near IL_028a: stateMachine*/)._003C_003Ef__this.holdFire;
						if (((_003CGetGizmos_003Ec__Iterator148)/*Error near IL_028a: stateMachine*/)._003C_003Ef__this.holdFire)
						{
							((_003CGetGizmos_003Ec__Iterator148)/*Error near IL_028a: stateMachine*/)._003C_003Ef__this.ResetForcedTarget();
						}
					},
					isActive = (Func<bool>)(() => ((_003CGetGizmos_003Ec__Iterator148)/*Error near IL_02a1: stateMachine*/)._003C_003Ef__this.holdFire)
				};
			}
		}

		private void ResetForcedTarget()
		{
			base.forcedTarget = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
			if (this.burstCooldownTicksLeft <= 0)
			{
				this.TryStartShootSomething(false);
			}
		}

		private void ResetCurrentTarget()
		{
			this.currentTargetInt = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
		}
	}
}
