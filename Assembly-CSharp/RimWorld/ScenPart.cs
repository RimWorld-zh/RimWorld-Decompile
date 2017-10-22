using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public abstract class ScenPart : IExposable
	{
		public ScenPartDef def;

		public bool visible = true;

		public bool summarized = false;

		public static float RowHeight
		{
			get
			{
				return Text.LineHeight;
			}
		}

		public virtual string Label
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ScenPartDef>(ref this.def, "def");
		}

		public ScenPart CopyForEditing()
		{
			ScenPart scenPart = this.CopyForEditingInner();
			scenPart.def = this.def;
			return scenPart;
		}

		protected virtual ScenPart CopyForEditingInner()
		{
			return (ScenPart)base.MemberwiseClone();
		}

		public virtual void DoEditInterface(Listing_ScenEdit listing)
		{
			listing.GetScenPartRect(this, ScenPart.RowHeight);
		}

		public virtual string Summary(Scenario scen)
		{
			return this.def.description;
		}

		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			yield break;
		}

		public virtual void Randomize()
		{
		}

		public virtual bool TryMerge(ScenPart other)
		{
			return false;
		}

		public virtual bool CanCoexistWith(ScenPart other)
		{
			return true;
		}

		public virtual IEnumerable<Page> GetConfigPages()
		{
			yield break;
		}

		public virtual bool AllowPlayerStartingPawn(Pawn pawn)
		{
			return true;
		}

		public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context)
		{
		}

		public virtual void Notify_PawnDied(Corpse corpse)
		{
		}

		public virtual void PreConfigure()
		{
		}

		public virtual void PostWorldGenerate()
		{
		}

		public virtual void PreMapGenerate()
		{
		}

		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			yield break;
		}

		public virtual void GenerateIntoMap(Map map)
		{
		}

		public virtual void PostMapGenerate(Map map)
		{
		}

		public virtual void PostGameStart()
		{
		}

		public virtual void Tick()
		{
		}

		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.def != null)
				yield break;
			yield return base.GetType().ToString() + " has null def.";
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
