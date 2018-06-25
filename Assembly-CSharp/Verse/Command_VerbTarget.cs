using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E6C RID: 3692
	internal class Command_VerbTarget : Command
	{
		// Token: 0x0400399D RID: 14749
		public Verb verb;

		// Token: 0x0400399E RID: 14750
		private List<Verb> groupedVerbs;

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x060056F3 RID: 22259 RVA: 0x002CC9F8 File Offset: 0x002CADF8
		public override Color IconDrawColor
		{
			get
			{
				Color result;
				if (this.verb.ownerEquipment != null)
				{
					result = this.verb.ownerEquipment.DrawColor;
				}
				else
				{
					result = base.IconDrawColor;
				}
				return result;
			}
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x002CCA3C File Offset: 0x002CAE3C
		public override void GizmoUpdateOnMouseover()
		{
			this.verb.verbProps.DrawRadiusRing(this.verb.caster.Position);
			if (!this.groupedVerbs.NullOrEmpty<Verb>())
			{
				foreach (Verb verb in this.groupedVerbs)
				{
					verb.verbProps.DrawRadiusRing(verb.caster.Position);
				}
			}
		}

		// Token: 0x060056F5 RID: 22261 RVA: 0x002CCAE0 File Offset: 0x002CAEE0
		public override void MergeWith(Gizmo other)
		{
			base.MergeWith(other);
			Command_VerbTarget command_VerbTarget = other as Command_VerbTarget;
			if (command_VerbTarget == null)
			{
				Log.ErrorOnce("Tried to merge Command_VerbTarget with unexpected type", 73406263, false);
			}
			else
			{
				if (this.groupedVerbs == null)
				{
					this.groupedVerbs = new List<Verb>();
				}
				this.groupedVerbs.Add(command_VerbTarget.verb);
				if (command_VerbTarget.groupedVerbs != null)
				{
					this.groupedVerbs.AddRange(command_VerbTarget.groupedVerbs);
				}
			}
		}

		// Token: 0x060056F6 RID: 22262 RVA: 0x002CCB5C File Offset: 0x002CAF5C
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Targeter targeter = Find.Targeter;
			if (this.verb.CasterIsPawn && targeter.targetingVerb != null && targeter.targetingVerb.verbProps == this.verb.verbProps)
			{
				Pawn casterPawn = this.verb.CasterPawn;
				if (!targeter.IsPawnTargeting(casterPawn))
				{
					targeter.targetingVerbAdditionalPawns.Add(casterPawn);
				}
			}
			else
			{
				Find.Targeter.BeginTargeting(this.verb);
			}
		}
	}
}
