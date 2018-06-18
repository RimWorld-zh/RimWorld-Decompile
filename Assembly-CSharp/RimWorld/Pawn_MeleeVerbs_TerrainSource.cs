using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000517 RID: 1303
	public class Pawn_MeleeVerbs_TerrainSource : IExposable, IVerbOwner
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x000CE5A0 File Offset: 0x000CC9A0
		public VerbTracker VerbTracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001792 RID: 6034 RVA: 0x000CE5BC File Offset: 0x000CC9BC
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06001793 RID: 6035 RVA: 0x000CE5D4 File Offset: 0x000CC9D4
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x000CE5F4 File Offset: 0x000CC9F4
		public static Pawn_MeleeVerbs_TerrainSource Create(Pawn_MeleeVerbs parent, TerrainDef terrainDef)
		{
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = new Pawn_MeleeVerbs_TerrainSource();
			pawn_MeleeVerbs_TerrainSource.parent = parent;
			pawn_MeleeVerbs_TerrainSource.def = terrainDef;
			pawn_MeleeVerbs_TerrainSource.tracker = new VerbTracker(pawn_MeleeVerbs_TerrainSource);
			return pawn_MeleeVerbs_TerrainSource;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x000CE62A File Offset: 0x000CCA2A
		public void ExposeData()
		{
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Deep.Look<VerbTracker>(ref this.tracker, "tracker", new object[]
			{
				this
			});
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x000CE658 File Offset: 0x000CCA58
		public string UniqueVerbOwnerID()
		{
			return "TerrainVerbs_" + this.parent.Pawn.ThingID;
		}

		// Token: 0x04000DEA RID: 3562
		public Pawn_MeleeVerbs parent;

		// Token: 0x04000DEB RID: 3563
		public TerrainDef def;

		// Token: 0x04000DEC RID: 3564
		public VerbTracker tracker;
	}
}
