using System;
using System.Collections.Generic;
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

		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == this.IncidentTag)
			{
				yield return this.incident.LabelCap;
			}
		}

		public override void Randomize()
		{
			this.incident = this.RandomizableIncidents().RandomElement();
		}

		public override bool TryMerge(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			if (scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident)
			{
				return true;
			}
			return false;
		}

		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			if (scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident)
			{
				return false;
			}
			return true;
		}

		protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return Enumerable.Empty<IncidentDef>();
		}

		protected void DoIncidentEditInterface(Rect rect)
		{
			if (Widgets.ButtonText(rect, this.incident.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu(DefDatabase<IncidentDef>.AllDefs, (Func<IncidentDef, string>)((IncidentDef id) => id.LabelCap), (Func<IncidentDef, Action>)((IncidentDef id) => (Action)delegate()
				{
					this.incident = id;
				}));
			}
		}
	}
}
