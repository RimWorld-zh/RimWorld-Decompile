using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class FormCaravanComp : WorldObjectComp
	{
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		private WorldObjectCompProperties_FormCaravan Props
		{
			get
			{
				return (WorldObjectCompProperties_FormCaravan)base.props;
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			_003CGetGizmos_003Ec__Iterator0 _003CGetGizmos_003Ec__Iterator = (_003CGetGizmos_003Ec__Iterator0)/*Error near IL_0036: stateMachine*/;
			MapParent mapParent = base.parent as MapParent;
			if (!mapParent.HasMap)
				yield break;
			if (!this.Props.reformCaravan)
			{
				yield return (Gizmo)new Command_Action
				{
					defaultLabel = "CommandFormCaravan".Translate(),
					defaultDesc = "CommandFormCaravanDesc".Translate(),
					icon = FormCaravanComp.FormCaravanCommand,
					hotKey = KeyBindingDefOf.Misc2,
					tutorTag = "FormCaravan",
					action = delegate
					{
						Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, false, null, true, false));
					}
				};
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (mapParent.Map.mapPawns.FreeColonistsSpawnedCount == 0)
				yield break;
			Command_Action reformCaravan = new Command_Action
			{
				defaultLabel = "CommandReformCaravan".Translate(),
				defaultDesc = "CommandReformCaravanDesc".Translate(),
				icon = FormCaravanComp.FormCaravanCommand,
				hotKey = KeyBindingDefOf.Misc2,
				tutorTag = "ReformCaravan",
				action = delegate
				{
					Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, true, null, true, false));
				}
			};
			if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map))
			{
				reformCaravan.Disable("CommandReformCaravanFailHostilePawns".Translate());
			}
			yield return (Gizmo)reformCaravan;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
