using System;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public class Command_Target : Command
	{
		public Action<Thing> action;

		public TargetingParameters targetingParams;

		public Command_Target()
		{
		}

		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target.Thing);
			}, null, null, null);
		}

		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}

		[CompilerGenerated]
		private void <ProcessInput>m__0(LocalTargetInfo target)
		{
			this.action(target.Thing);
		}
	}
}
