using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000621 RID: 1569
	[StaticConstructorOnStartup]
	public class FormCaravanComp : WorldObjectComp
	{
		// Token: 0x0400126E RID: 4718
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x00112EF0 File Offset: 0x001112F0
		public WorldObjectCompProperties_FormCaravan Props
		{
			get
			{
				return (WorldObjectCompProperties_FormCaravan)this.props;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001FE0 RID: 8160 RVA: 0x00112F10 File Offset: 0x00111310
		private MapParent MapParent
		{
			get
			{
				return (MapParent)this.parent;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001FE1 RID: 8161 RVA: 0x00112F30 File Offset: 0x00111330
		public bool Reform
		{
			get
			{
				return !this.MapParent.HasMap || !this.MapParent.Map.IsPlayerHome;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001FE2 RID: 8162 RVA: 0x00112F6C File Offset: 0x0011136C
		public bool CanFormOrReformCaravanNow
		{
			get
			{
				MapParent mapParent = this.MapParent;
				return mapParent.HasMap && (!this.Reform || (!GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map) && mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0));
			}
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x00112FD4 File Offset: 0x001113D4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (mapParent.HasMap)
			{
				if (!this.Reform)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandFormCaravan".Translate(),
						defaultDesc = "CommandFormCaravanDesc".Translate(),
						icon = FormCaravanComp.FormCaravanCommand,
						hotKey = KeyBindingDefOf.Misc2,
						tutorTag = "FormCaravan",
						action = delegate()
						{
							Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, false, null, false));
						}
					};
				}
				else if (mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					Command_Action reformCaravan = new Command_Action();
					reformCaravan.defaultLabel = "CommandReformCaravan".Translate();
					reformCaravan.defaultDesc = "CommandReformCaravanDesc".Translate();
					reformCaravan.icon = FormCaravanComp.FormCaravanCommand;
					reformCaravan.hotKey = KeyBindingDefOf.Misc2;
					reformCaravan.tutorTag = "ReformCaravan";
					reformCaravan.action = delegate()
					{
						Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, true, null, false));
					};
					if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map))
					{
						reformCaravan.Disable("CommandReformCaravanFailHostilePawns".Translate());
					}
					yield return reformCaravan;
				}
			}
			yield break;
		}
	}
}
