using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E6B RID: 3691
	internal class Command_VerbTarget : Command
	{
		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x060056D1 RID: 22225 RVA: 0x002CAAD0 File Offset: 0x002C8ED0
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

		// Token: 0x060056D2 RID: 22226 RVA: 0x002CAB14 File Offset: 0x002C8F14
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

		// Token: 0x060056D3 RID: 22227 RVA: 0x002CABB8 File Offset: 0x002C8FB8
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

		// Token: 0x060056D4 RID: 22228 RVA: 0x002CAC34 File Offset: 0x002C9034
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

		// Token: 0x04003988 RID: 14728
		public Verb verb;

		// Token: 0x04003989 RID: 14729
		private List<Verb> groupedVerbs;
	}
}
