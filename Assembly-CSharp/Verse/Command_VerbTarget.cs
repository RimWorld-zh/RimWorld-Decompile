using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E69 RID: 3689
	internal class Command_VerbTarget : Command
	{
		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x060056EF RID: 22255 RVA: 0x002CC6E0 File Offset: 0x002CAAE0
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

		// Token: 0x060056F0 RID: 22256 RVA: 0x002CC724 File Offset: 0x002CAB24
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

		// Token: 0x060056F1 RID: 22257 RVA: 0x002CC7C8 File Offset: 0x002CABC8
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

		// Token: 0x060056F2 RID: 22258 RVA: 0x002CC844 File Offset: 0x002CAC44
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

		// Token: 0x04003995 RID: 14741
		public Verb verb;

		// Token: 0x04003996 RID: 14742
		private List<Verb> groupedVerbs;
	}
}
