using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class ScenPart_IncidentBase : ScenPart
	{
		protected IncidentDef incident;

		public IncidentDef Incident
		{
			get
			{
				return this.incident;
			}
		}

		protected abstract string IncidentTag
		{
			get;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			this.DoIncidentEditInterface(scenPartRect);
		}

		public override string Summary(Scenario scen)
		{
			string key = "ScenPart_" + this.IncidentTag;
			return ScenSummaryList.SummaryWithList(scen, this.IncidentTag, key.Translate());
		}

		[DebuggerHidden]
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_IncidentBase.<GetSummaryListEntries>c__Iterator117 <GetSummaryListEntries>c__Iterator = new ScenPart_IncidentBase.<GetSummaryListEntries>c__Iterator117();
			<GetSummaryListEntries>c__Iterator.tag = tag;
			<GetSummaryListEntries>c__Iterator.<$>tag = tag;
			<GetSummaryListEntries>c__Iterator.<>f__this = this;
			ScenPart_IncidentBase.<GetSummaryListEntries>c__Iterator117 expr_1C = <GetSummaryListEntries>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		public override void Randomize()
		{
			this.incident = this.RandomizableIncidents().RandomElement<IncidentDef>();
		}

		public override bool TryMerge(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident;
		}

		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase == null || scenPart_IncidentBase.Incident != this.incident;
		}

		protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return Enumerable.Empty<IncidentDef>();
		}

		protected void DoIncidentEditInterface(Rect rect)
		{
			if (Widgets.ButtonText(rect, this.incident.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu<IncidentDef>(DefDatabase<IncidentDef>.AllDefs, (IncidentDef id) => id.LabelCap, (IncidentDef id) => delegate
				{
					this.incident = id;
				});
			}
		}
	}
}
