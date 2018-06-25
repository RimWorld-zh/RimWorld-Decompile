using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Pawn_MeleeVerbs_TerrainSource : IExposable, IVerbOwner
	{
		public Pawn_MeleeVerbs parent;

		public TerrainDef def;

		public VerbTracker tracker;

		public Pawn_MeleeVerbs_TerrainSource()
		{
		}

		public VerbTracker VerbTracker
		{
			get
			{
				return this.tracker;
			}
		}

		public List<VerbProperties> VerbProperties
		{
			get
			{
				return null;
			}
		}

		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		public static Pawn_MeleeVerbs_TerrainSource Create(Pawn_MeleeVerbs parent, TerrainDef terrainDef)
		{
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = new Pawn_MeleeVerbs_TerrainSource();
			pawn_MeleeVerbs_TerrainSource.parent = parent;
			pawn_MeleeVerbs_TerrainSource.def = terrainDef;
			pawn_MeleeVerbs_TerrainSource.tracker = new VerbTracker(pawn_MeleeVerbs_TerrainSource);
			return pawn_MeleeVerbs_TerrainSource;
		}

		public void ExposeData()
		{
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Deep.Look<VerbTracker>(ref this.tracker, "tracker", new object[]
			{
				this
			});
		}

		public string UniqueVerbOwnerID()
		{
			return "TerrainVerbs_" + this.parent.Pawn.ThingID;
		}
	}
}
