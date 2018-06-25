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
	// Token: 0x02000688 RID: 1672
	[StaticConstructorOnStartup]
	public class Building_TurretGun : Building_Turret
	{
		// Token: 0x040013CD RID: 5069
		protected int burstCooldownTicksLeft = 0;

		// Token: 0x040013CE RID: 5070
		protected int burstWarmupTicksLeft = 0;

		// Token: 0x040013CF RID: 5071
		protected LocalTargetInfo currentTargetInt = LocalTargetInfo.Invalid;

		// Token: 0x040013D0 RID: 5072
		private bool holdFire;

		// Token: 0x040013D1 RID: 5073
		public Thing gun;

		// Token: 0x040013D2 RID: 5074
		protected TurretTop top;

		// Token: 0x040013D3 RID: 5075
		protected CompPowerTrader powerComp;

		// Token: 0x040013D4 RID: 5076
		protected CompMannable mannableComp;

		// Token: 0x040013D5 RID: 5077
		private const int TryStartShootSomethingIntervalTicks = 10;

		// Token: 0x040013D6 RID: 5078
		public static Material ForcedTargetLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));

		// Token: 0x06002347 RID: 9031 RVA: 0x0012F172 File Offset: 0x0012D572
		public Building_TurretGun()
		{
			this.top = new TurretTop(this);
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x0012F1A0 File Offset: 0x0012D5A0
		public CompEquippable GunCompEq
		{
			get
			{
				return this.gun.TryGetComp<CompEquippable>();
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x0012F1C0 File Offset: 0x0012D5C0
		public override LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTargetInt;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x0600234A RID: 9034 RVA: 0x0012F1DC File Offset: 0x0012D5DC
		private bool WarmingUp
		{
			get
			{
				return this.burstWarmupTicksLeft > 0;
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x0600234B RID: 9035 RVA: 0x0012F1FC File Offset: 0x0012D5FC
		public override Verb AttackVerb
		{
			get
			{
				return this.GunCompEq.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x0600234C RID: 9036 RVA: 0x0012F224 File Offset: 0x0012D624
		private bool PlayerControlled
		{
			get
			{
				return (base.Faction == Faction.OfPlayer || this.MannedByColonist) && !this.MannedByNonColonist;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x0600234D RID: 9037 RVA: 0x0012F260 File Offset: 0x0012D660
		private bool CanSetForcedTarget
		{
			get
			{
				return this.mannableComp != null && this.PlayerControlled;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x0600234E RID: 9038 RVA: 0x0012F28C File Offset: 0x0012D68C
		private bool CanToggleHoldFire
		{
			get
			{
				return this.PlayerControlled;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x0600234F RID: 9039 RVA: 0x0012F2A8 File Offset: 0x0012D6A8
		private bool IsMortar
		{
			get
			{
				return this.def.building.IsMortar;
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06002350 RID: 9040 RVA: 0x0012F2D0 File Offset: 0x0012D6D0
		private bool IsMortarOrProjectileFliesOverhead
		{
			get
			{
				return this.GunCompEq.PrimaryVerb.ProjectileFliesOverhead() || this.IsMortar;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06002351 RID: 9041 RVA: 0x0012F304 File Offset: 0x0012D704
		private bool CanExtractShell
		{
			get
			{
				bool result;
				if (!this.PlayerControlled)
				{
					result = false;
				}
				else
				{
					CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
					result = (compChangeableProjectile != null && compChangeableProjectile.Loaded);
				}
				return result;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06002352 RID: 9042 RVA: 0x0012F348 File Offset: 0x0012D748
		private bool MannedByColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06002353 RID: 9043 RVA: 0x0012F394 File Offset: 0x0012D794
		private bool MannedByNonColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x0012F3E1 File Offset: 0x0012D7E1
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.mannableComp = base.GetComp<CompMannable>();
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x0012F404 File Offset: 0x0012D804
		public override void PostMake()
		{
			base.PostMake();
			this.MakeGun();
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x0012F413 File Offset: 0x0012D813
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.ResetCurrentTarget();
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x0012F424 File Offset: 0x0012D824
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.burstCooldownTicksLeft, "burstCooldownTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.burstWarmupTicksLeft, "burstWarmupTicksLeft", 0, false);
			Scribe_TargetInfo.Look(ref this.currentTargetInt, "currentTarget");
			Scribe_Values.Look<bool>(ref this.holdFire, "holdFire", false, false);
			Scribe_Deep.Look<Thing>(ref this.gun, "gun", new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.TurretPostLoadInit(this);
				this.UpdateGunVerbs();
			}
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0012F4B0 File Offset: 0x0012D8B0
		public override bool ClaimableBy(Faction by)
		{
			return base.ClaimableBy(by) && (this.mannableComp == null || this.mannableComp.ManningPawn == null) && (this.powerComp == null || !this.powerComp.PowerOn);
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0012F520 File Offset: 0x0012D920
		public override void OrderAttack(LocalTargetInfo targ)
		{
			if (!targ.IsValid)
			{
				if (this.forcedTarget.IsValid)
				{
					this.ResetForcedTarget();
				}
			}
			else if ((targ.Cell - base.Position).LengthHorizontal < this.GunCompEq.PrimaryVerb.verbProps.minRange)
			{
				Messages.Message("MessageTargetBelowMinimumRange".Translate(), this, MessageTypeDefOf.RejectInput, false);
			}
			else if ((targ.Cell - base.Position).LengthHorizontal > this.GunCompEq.PrimaryVerb.verbProps.range)
			{
				Messages.Message("MessageTargetBeyondMaximumRange".Translate(), this, MessageTypeDefOf.RejectInput, false);
			}
			else
			{
				if (this.forcedTarget != targ)
				{
					this.forcedTarget = targ;
					if (this.burstCooldownTicksLeft <= 0)
					{
						this.TryStartShootSomething(false);
					}
				}
				if (this.holdFire)
				{
					Messages.Message("MessageTurretWontFireBecauseHoldFire".Translate(new object[]
					{
						this.def.label
					}), this, MessageTypeDefOf.RejectInput, false);
				}
			}
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x0012F66C File Offset: 0x0012DA6C
		public override void Tick()
		{
			base.Tick();
			if (this.forcedTarget.IsValid && !this.CanSetForcedTarget)
			{
				this.ResetForcedTarget();
			}
			if (!this.CanToggleHoldFire)
			{
				this.holdFire = false;
			}
			if (this.forcedTarget.ThingDestroyed)
			{
				this.ResetForcedTarget();
			}
			bool flag = (this.powerComp == null || this.powerComp.PowerOn) && (this.mannableComp == null || this.mannableComp.MannedNow);
			if (flag && base.Spawned)
			{
				this.GunCompEq.verbTracker.VerbsTick();
				if (!this.stunner.Stunned && this.GunCompEq.PrimaryVerb.state != VerbState.Bursting)
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

		// Token: 0x0600235B RID: 9051 RVA: 0x0012F7D0 File Offset: 0x0012DBD0
		protected void TryStartShootSomething(bool canBeginBurstImmediately)
		{
			if (!base.Spawned || (this.holdFire && this.CanToggleHoldFire) || (this.GunCompEq.PrimaryVerb.ProjectileFliesOverhead() && base.Map.roofGrid.Roofed(base.Position)) || !this.GunCompEq.PrimaryVerb.Available())
			{
				this.ResetCurrentTarget();
			}
			else
			{
				bool isValid = this.currentTargetInt.IsValid;
				if (this.forcedTarget.IsValid)
				{
					this.currentTargetInt = this.forcedTarget;
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
					if (this.def.building.turretBurstWarmupTime > 0f)
					{
						this.burstWarmupTicksLeft = this.def.building.turretBurstWarmupTime.SecondsToTicks();
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

		// Token: 0x0600235C RID: 9052 RVA: 0x0012F930 File Offset: 0x0012DD30
		protected LocalTargetInfo TryFindNewTarget()
		{
			IAttackTargetSearcher attackTargetSearcher = this.TargSearcher();
			Faction faction = attackTargetSearcher.Thing.Faction;
			float range = this.GunCompEq.PrimaryVerb.verbProps.range;
			float minRange = this.GunCompEq.PrimaryVerb.verbProps.minRange;
			if (Rand.Value < 0.5f && this.GunCompEq.PrimaryVerb.ProjectileFliesOverhead() && faction.HostileTo(Faction.OfPlayer))
			{
				Building t;
				if (base.Map.listerBuildings.allBuildingsColonist.Where(delegate(Building x)
				{
					float num = (float)x.Position.DistanceToSquared(this.Position);
					return num > minRange * minRange && num < range * range;
				}).TryRandomElement(out t))
				{
					return t;
				}
			}
			TargetScanFlags targetScanFlags = TargetScanFlags.NeedThreat;
			if (!this.GunCompEq.PrimaryVerb.ProjectileFliesOverhead())
			{
				targetScanFlags |= TargetScanFlags.NeedLOSToAll;
				targetScanFlags |= TargetScanFlags.LOSBlockableByGas;
			}
			if (this.GunCompEq.PrimaryVerb.IsIncendiary())
			{
				targetScanFlags |= TargetScanFlags.NeedNonBurning;
			}
			return (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(attackTargetSearcher, new Predicate<Thing>(this.IsValidTarget), range, minRange, targetScanFlags);
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x0012FA80 File Offset: 0x0012DE80
		private IAttackTargetSearcher TargSearcher()
		{
			IAttackTargetSearcher result;
			if (this.mannableComp != null && this.mannableComp.MannedNow)
			{
				result = this.mannableComp.ManningPawn;
			}
			else
			{
				result = this;
			}
			return result;
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0012FAC4 File Offset: 0x0012DEC4
		private bool IsValidTarget(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (this.GunCompEq.PrimaryVerb.ProjectileFliesOverhead())
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

		// Token: 0x0600235F RID: 9055 RVA: 0x0012FB76 File Offset: 0x0012DF76
		protected void BeginBurst()
		{
			this.GunCompEq.PrimaryVerb.TryStartCastOn(this.CurrentTarget, false, true);
			base.OnAttackedTarget(this.CurrentTarget);
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0012FB9E File Offset: 0x0012DF9E
		protected void BurstComplete()
		{
			this.burstCooldownTicksLeft = this.BurstCooldownTime().SecondsToTicks();
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0012FBB4 File Offset: 0x0012DFB4
		protected float BurstCooldownTime()
		{
			float result;
			if (this.def.building.turretBurstCooldownTime >= 0f)
			{
				result = this.def.building.turretBurstCooldownTime;
			}
			else
			{
				result = this.GunCompEq.PrimaryVerb.verbProps.defaultCooldownTime;
			}
			return result;
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0012FC10 File Offset: 0x0012E010
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			if (this.GunCompEq.PrimaryVerb.verbProps.minRange > 0f)
			{
				stringBuilder.AppendLine("MinimumRange".Translate() + ": " + this.GunCompEq.PrimaryVerb.verbProps.minRange.ToString("F0"));
			}
			if (base.Spawned && this.IsMortarOrProjectileFliesOverhead && base.Position.Roofed(base.Map))
			{
				stringBuilder.AppendLine("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst());
			}
			else if (base.Spawned && this.burstCooldownTicksLeft > 0 && this.BurstCooldownTime() > 5f)
			{
				stringBuilder.AppendLine("CanFireIn".Translate() + ": " + this.burstCooldownTicksLeft.TicksToSecondsString());
			}
			CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
			if (compChangeableProjectile != null)
			{
				if (compChangeableProjectile.Loaded)
				{
					stringBuilder.AppendLine("ShellLoaded".Translate(new object[]
					{
						compChangeableProjectile.LoadedShell.LabelCap
					}));
				}
				else
				{
					stringBuilder.AppendLine("ShellNotLoaded".Translate());
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x0012FDAB File Offset: 0x0012E1AB
		public override void Draw()
		{
			this.top.DrawTurret();
			base.Draw();
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x0012FDC0 File Offset: 0x0012E1C0
		public override void DrawExtraSelectionOverlays()
		{
			float range = this.GunCompEq.PrimaryVerb.verbProps.range;
			if (range < 90f)
			{
				GenDraw.DrawRadiusRing(base.Position, range);
			}
			float minRange = this.GunCompEq.PrimaryVerb.verbProps.minRange;
			if (minRange < 90f && minRange > 0.1f)
			{
				GenDraw.DrawRadiusRing(base.Position, minRange);
			}
			if (this.WarmingUp)
			{
				int degreesWide = (int)((float)this.burstWarmupTicksLeft * 0.5f);
				GenDraw.DrawAimPie(this, this.CurrentTarget, degreesWide, (float)this.def.size.x * 0.5f);
			}
			if (this.forcedTarget.IsValid && (!this.forcedTarget.HasThing || this.forcedTarget.Thing.Spawned))
			{
				Vector3 b;
				if (this.forcedTarget.HasThing)
				{
					b = this.forcedTarget.Thing.TrueCenter();
				}
				else
				{
					b = this.forcedTarget.Cell.ToVector3Shifted();
				}
				Vector3 a = this.TrueCenter();
				b.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				a.y = b.y;
				GenDraw.DrawLineBetween(a, b, Building_TurretGun.ForcedTargetLineMat);
			}
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x0012FF18 File Offset: 0x0012E318
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return c;
			}
			if (this.CanExtractShell)
			{
				CompChangeableProjectile changeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				yield return new Command_Action
				{
					defaultLabel = "CommandExtractShell".Translate(),
					defaultDesc = "CommandExtractShellDesc".Translate(),
					icon = changeableProjectile.LoadedShell.uiIcon,
					iconAngle = changeableProjectile.LoadedShell.uiIconAngle,
					iconOffset = changeableProjectile.LoadedShell.uiIconOffset,
					iconDrawScale = GenUI.IconDrawScale(changeableProjectile.LoadedShell),
					alsoClickIfOtherInGroupClicked = false,
					action = delegate()
					{
						GenPlace.TryPlaceThing(changeableProjectile.RemoveShell(), this.Position, this.Map, ThingPlaceMode.Near, null, null);
					}
				};
			}
			if (this.CanSetForcedTarget)
			{
				Command_VerbTarget attack = new Command_VerbTarget();
				attack.defaultLabel = "CommandSetForceAttackTarget".Translate();
				attack.defaultDesc = "CommandSetForceAttackTargetDesc".Translate();
				attack.icon = ContentFinder<Texture2D>.Get("UI/Commands/Attack", true);
				attack.verb = this.GunCompEq.PrimaryVerb;
				attack.hotKey = KeyBindingDefOf.Misc4;
				if (base.Spawned && this.IsMortarOrProjectileFliesOverhead && base.Position.Roofed(base.Map))
				{
					attack.Disable("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst());
				}
				yield return attack;
			}
			if (this.forcedTarget.IsValid)
			{
				Command_Action stop = new Command_Action();
				stop.defaultLabel = "CommandStopForceAttack".Translate();
				stop.defaultDesc = "CommandStopForceAttackDesc".Translate();
				stop.icon = ContentFinder<Texture2D>.Get("UI/Commands/Halt", true);
				stop.action = delegate()
				{
					this.ResetForcedTarget();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				};
				if (!this.forcedTarget.IsValid)
				{
					stop.Disable("CommandStopAttackFailNotForceAttacking".Translate());
				}
				stop.hotKey = KeyBindingDefOf.Misc5;
				yield return stop;
			}
			if (this.CanToggleHoldFire)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandHoldFire".Translate(),
					defaultDesc = "CommandHoldFireDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/HoldFire", true),
					hotKey = KeyBindingDefOf.Misc6,
					toggleAction = delegate()
					{
						this.holdFire = !this.holdFire;
						if (this.holdFire)
						{
							this.ResetForcedTarget();
						}
					},
					isActive = (() => this.holdFire)
				};
			}
			yield break;
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0012FF42 File Offset: 0x0012E342
		private void ResetForcedTarget()
		{
			this.forcedTarget = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
			if (this.burstCooldownTicksLeft <= 0)
			{
				this.TryStartShootSomething(false);
			}
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x0012FF6A File Offset: 0x0012E36A
		private void ResetCurrentTarget()
		{
			this.currentTargetInt = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0012FF7F File Offset: 0x0012E37F
		public void MakeGun()
		{
			this.gun = ThingMaker.MakeThing(this.def.building.turretGunDef, null);
			this.UpdateGunVerbs();
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x0012FFA4 File Offset: 0x0012E3A4
		private void UpdateGunVerbs()
		{
			List<Verb> allVerbs = this.gun.TryGetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				Verb verb = allVerbs[i];
				verb.caster = this;
				verb.castCompleteCallback = new Action(this.BurstComplete);
			}
		}
	}
}
