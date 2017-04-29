using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public abstract class ScenPart : IExposable
	{
		public ScenPartDef def;

		public bool visible = true;

		public bool summarized;

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

		[DebuggerHidden]
		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart.<GetSummaryListEntries>c__Iterator110 <GetSummaryListEntries>c__Iterator = new ScenPart.<GetSummaryListEntries>c__Iterator110();
			ScenPart.<GetSummaryListEntries>c__Iterator110 expr_07 = <GetSummaryListEntries>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
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

		[DebuggerHidden]
		public virtual IEnumerable<Page> GetConfigPages()
		{
			ScenPart.<GetConfigPages>c__Iterator111 <GetConfigPages>c__Iterator = new ScenPart.<GetConfigPages>c__Iterator111();
			ScenPart.<GetConfigPages>c__Iterator111 expr_07 = <GetConfigPages>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
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

		[DebuggerHidden]
		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			ScenPart.<PlayerStartingThings>c__Iterator112 <PlayerStartingThings>c__Iterator = new ScenPart.<PlayerStartingThings>c__Iterator112();
			ScenPart.<PlayerStartingThings>c__Iterator112 expr_07 = <PlayerStartingThings>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
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

		[DebuggerHidden]
		public virtual IEnumerable<string> ConfigErrors()
		{
			ScenPart.<ConfigErrors>c__Iterator113 <ConfigErrors>c__Iterator = new ScenPart.<ConfigErrors>c__Iterator113();
			<ConfigErrors>c__Iterator.<>f__this = this;
			ScenPart.<ConfigErrors>c__Iterator113 expr_0E = <ConfigErrors>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
