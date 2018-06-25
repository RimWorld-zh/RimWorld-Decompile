using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000867 RID: 2151
	public class Targeter
	{
		// Token: 0x04001A70 RID: 6768
		public Verb targetingVerb;

		// Token: 0x04001A71 RID: 6769
		public List<Pawn> targetingVerbAdditionalPawns;

		// Token: 0x04001A72 RID: 6770
		private Action<LocalTargetInfo> action;

		// Token: 0x04001A73 RID: 6771
		private Pawn caster;

		// Token: 0x04001A74 RID: 6772
		private TargetingParameters targetParams;

		// Token: 0x04001A75 RID: 6773
		private Action actionWhenFinished;

		// Token: 0x04001A76 RID: 6774
		private Texture2D mouseAttachment;

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060030E7 RID: 12519 RVA: 0x001A9404 File Offset: 0x001A7804
		public bool IsTargeting
		{
			get
			{
				return this.targetingVerb != null || this.action != null;
			}
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x001A9434 File Offset: 0x001A7834
		public void BeginTargeting(Verb verb)
		{
			if (verb.verbProps.targetable)
			{
				this.targetingVerb = verb;
				this.targetingVerbAdditionalPawns = new List<Pawn>();
			}
			else
			{
				Job job = new Job(JobDefOf.UseVerbOnThing);
				job.verbToUse = verb;
				verb.CasterPawn.jobs.StartJob(job, JobCondition.None, null, false, true, null, null, false);
			}
			this.action = null;
			this.caster = null;
			this.targetParams = null;
			this.actionWhenFinished = null;
			this.mouseAttachment = null;
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x001A94C2 File Offset: 0x001A78C2
		public void BeginTargeting(TargetingParameters targetParams, Action<LocalTargetInfo> action, Pawn caster = null, Action actionWhenFinished = null, Texture2D mouseAttachment = null)
		{
			this.targetingVerb = null;
			this.targetingVerbAdditionalPawns = null;
			this.action = action;
			this.targetParams = targetParams;
			this.caster = caster;
			this.actionWhenFinished = actionWhenFinished;
			this.mouseAttachment = mouseAttachment;
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x001A94F8 File Offset: 0x001A78F8
		public void StopTargeting()
		{
			if (this.actionWhenFinished != null)
			{
				Action action = this.actionWhenFinished;
				this.actionWhenFinished = null;
				action();
			}
			this.targetingVerb = null;
			this.action = null;
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x001A9538 File Offset: 0x001A7938
		public void ProcessInputEvents()
		{
			this.ConfirmStillValid();
			if (this.IsTargeting)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
				{
					if (this.targetingVerb != null)
					{
						this.OrderVerbForceTarget();
					}
					if (this.action != null)
					{
						LocalTargetInfo obj = this.CurrentTargetUnderMouse(false);
						if (obj.IsValid)
						{
							this.action(obj);
						}
					}
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.StopTargeting();
					Event.current.Use();
				}
				if ((Event.current.type == EventType.MouseDown && Event.current.button == 1) || KeyBindingDefOf.Cancel.KeyDownEvent)
				{
					SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
					this.StopTargeting();
					Event.current.Use();
				}
			}
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x001A9620 File Offset: 0x001A7A20
		public void TargeterOnGUI()
		{
			if (this.targetingVerb != null)
			{
				Texture2D icon;
				if (this.CurrentTargetUnderMouse(true).IsValid)
				{
					if (this.targetingVerb.UIIcon != BaseContent.BadTex)
					{
						icon = this.targetingVerb.UIIcon;
					}
					else
					{
						icon = TexCommand.Attack;
					}
				}
				else
				{
					icon = TexCommand.CannotShoot;
				}
				GenUI.DrawMouseAttachment(icon);
			}
			if (this.action != null)
			{
				Texture2D icon2 = this.mouseAttachment ?? TexCommand.Attack;
				GenUI.DrawMouseAttachment(icon2);
			}
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x001A96BC File Offset: 0x001A7ABC
		public void TargeterUpdate()
		{
			if (this.targetingVerb != null)
			{
				this.targetingVerb.verbProps.DrawRadiusRing(this.targetingVerb.caster.Position);
				LocalTargetInfo targ = this.CurrentTargetUnderMouse(true);
				if (targ.IsValid)
				{
					GenDraw.DrawTargetHighlight(targ);
					bool flag;
					float num = this.targetingVerb.HighlightFieldRadiusAroundTarget(out flag);
					if (num > 0.2f)
					{
						ShootLine shootLine;
						if (this.targetingVerb.TryFindShootLineFromTo(this.targetingVerb.caster.Position, targ, out shootLine))
						{
							if (flag)
							{
								GenExplosion.RenderPredictedAreaOfEffect(shootLine.Dest, num);
							}
							else
							{
								GenDraw.DrawFieldEdges((from x in GenRadial.RadialCellsAround(shootLine.Dest, num, true)
								where x.InBounds(Find.CurrentMap)
								select x).ToList<IntVec3>());
							}
						}
					}
				}
			}
			if (this.action != null)
			{
				LocalTargetInfo targ2 = this.CurrentTargetUnderMouse(false);
				if (targ2.IsValid)
				{
					GenDraw.DrawTargetHighlight(targ2);
				}
			}
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x001A97D0 File Offset: 0x001A7BD0
		public bool IsPawnTargeting(Pawn p)
		{
			bool result;
			if (this.caster == p)
			{
				result = true;
			}
			else
			{
				if (this.targetingVerb != null)
				{
					if (this.targetingVerb.CasterIsPawn)
					{
						if (this.targetingVerb.CasterPawn == p)
						{
							return true;
						}
						for (int i = 0; i < this.targetingVerbAdditionalPawns.Count; i++)
						{
							if (this.targetingVerbAdditionalPawns[i] == p)
							{
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x001A9868 File Offset: 0x001A7C68
		private void ConfirmStillValid()
		{
			if (this.caster != null)
			{
				if (this.caster.Map != Find.CurrentMap || this.caster.Destroyed || !Find.Selector.IsSelected(this.caster))
				{
					this.StopTargeting();
				}
			}
			if (this.targetingVerb != null)
			{
				Selector selector = Find.Selector;
				if (this.targetingVerb.caster.Map != Find.CurrentMap || this.targetingVerb.caster.Destroyed || !selector.IsSelected(this.targetingVerb.caster))
				{
					this.StopTargeting();
				}
				else
				{
					for (int i = 0; i < this.targetingVerbAdditionalPawns.Count; i++)
					{
						if (this.targetingVerbAdditionalPawns[i].Destroyed || !selector.IsSelected(this.targetingVerbAdditionalPawns[i]))
						{
							this.StopTargeting();
							break;
						}
					}
				}
			}
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x001A9980 File Offset: 0x001A7D80
		private void OrderVerbForceTarget()
		{
			if (this.targetingVerb.CasterIsPawn)
			{
				this.OrderPawnForceTarget(this.targetingVerb);
				for (int i = 0; i < this.targetingVerbAdditionalPawns.Count; i++)
				{
					Verb verb = this.GetTargetingVerb(this.targetingVerbAdditionalPawns[i]);
					if (verb != null)
					{
						this.OrderPawnForceTarget(verb);
					}
				}
			}
			else
			{
				int numSelected = Find.Selector.NumSelected;
				List<object> selectedObjects = Find.Selector.SelectedObjects;
				for (int j = 0; j < numSelected; j++)
				{
					Building_Turret building_Turret = selectedObjects[j] as Building_Turret;
					if (building_Turret != null && building_Turret.Map == Find.CurrentMap)
					{
						LocalTargetInfo targ = this.CurrentTargetUnderMouse(true);
						building_Turret.OrderAttack(targ);
					}
				}
			}
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x001A9A5C File Offset: 0x001A7E5C
		private void OrderPawnForceTarget(Verb verb)
		{
			LocalTargetInfo targetA = this.CurrentTargetUnderMouse(true);
			if (targetA.IsValid)
			{
				if (verb.verbProps.IsMeleeAttack)
				{
					Job job = new Job(JobDefOf.AttackMelee, targetA);
					job.playerForced = true;
					Pawn pawn = targetA.Thing as Pawn;
					if (pawn != null)
					{
						job.killIncappedTarget = pawn.Downed;
					}
					verb.CasterPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				}
				else
				{
					JobDef def = (!verb.verbProps.ai_IsWeapon) ? JobDefOf.UseVerbOnThing : JobDefOf.AttackStatic;
					Job job2 = new Job(def);
					job2.verbToUse = verb;
					job2.targetA = targetA;
					verb.CasterPawn.jobs.TryTakeOrderedJob(job2, JobTag.Misc);
				}
			}
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x001A9B30 File Offset: 0x001A7F30
		private LocalTargetInfo CurrentTargetUnderMouse(bool mustBeHittableNowIfNotMelee)
		{
			LocalTargetInfo result;
			if (!this.IsTargeting)
			{
				result = LocalTargetInfo.Invalid;
			}
			else
			{
				TargetingParameters clickParams = (this.targetingVerb == null) ? this.targetParams : this.targetingVerb.verbProps.targetParams;
				LocalTargetInfo localTargetInfo = LocalTargetInfo.Invalid;
				using (IEnumerator<LocalTargetInfo> enumerator = GenUI.TargetsAtMouse(clickParams, false).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						LocalTargetInfo localTargetInfo2 = enumerator.Current;
						localTargetInfo = localTargetInfo2;
					}
				}
				if (localTargetInfo.IsValid && mustBeHittableNowIfNotMelee && !(localTargetInfo.Thing is Pawn) && this.targetingVerb != null && !this.targetingVerb.verbProps.IsMeleeAttack)
				{
					if (this.targetingVerbAdditionalPawns != null && this.targetingVerbAdditionalPawns.Any<Pawn>())
					{
						bool flag = false;
						for (int i = 0; i < this.targetingVerbAdditionalPawns.Count; i++)
						{
							Verb verb = this.GetTargetingVerb(this.targetingVerbAdditionalPawns[i]);
							if (verb != null && verb.CanHitTarget(localTargetInfo))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							localTargetInfo = LocalTargetInfo.Invalid;
						}
					}
					else if (!this.targetingVerb.CanHitTarget(localTargetInfo))
					{
						localTargetInfo = LocalTargetInfo.Invalid;
					}
				}
				result = localTargetInfo;
			}
			return result;
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x001A9CC4 File Offset: 0x001A80C4
		private Verb GetTargetingVerb(Pawn pawn)
		{
			return pawn.equipment.AllEquipmentVerbs.FirstOrDefault((Verb x) => x.verbProps == this.targetingVerb.verbProps);
		}
	}
}
