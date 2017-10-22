using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Building_BlastingCharge : Building
	{
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Command_Action com = new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/Detonate", true),
				defaultDesc = "CommandDetonateDesc".Translate(),
				action = new Action(this.Command_Detonate)
			};
			if (base.GetComp<CompExplosive>().wickStarted)
			{
				com.Disable((string)null);
			}
			com.defaultLabel = "CommandDetonateLabel".Translate();
			yield return (Gizmo)com;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private void Command_Detonate()
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
