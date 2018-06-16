using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E69 RID: 3689
	public class Command_Target : Command
	{
		// Token: 0x060056C8 RID: 22216 RVA: 0x002CA91C File Offset: 0x002C8D1C
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target.Thing);
			}, null, null, null);
		}

		// Token: 0x060056C9 RID: 22217 RVA: 0x002CA950 File Offset: 0x002C8D50
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}

		// Token: 0x04003982 RID: 14722
		public Action<Thing> action;

		// Token: 0x04003983 RID: 14723
		public TargetingParameters targetingParams;
	}
}
