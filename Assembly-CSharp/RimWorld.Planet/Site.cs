using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class Site : MapParent
	{
		public string customLabel;

		public SiteCoreDef core;

		public List<SitePartDef> parts = new List<SitePartDef>();

		public bool writeSiteParts;

		private bool startedCountdown;

		private bool anyEnemiesInitially;

		private Material cachedMat;

		public override string Label
		{
			get
			{
				return this.customLabel.NullOrEmpty() ? ((this.core != SiteCoreDefOf.Nothing || !this.parts.Any()) ? this.core.label : this.parts[0].label) : this.customLabel;
			}
		}

		public override Texture2D ExpandingIcon
		{
			get
			{
				return this.LeadingSiteDef.ExpandingIconTexture;
			}
		}

		public override bool TransportPodsCanLandAndGenerateMap
		{
			get
			{
				return this.core.transportPodsCanLandAndGenerateMap;
			}
		}

		public override IntVec3 MapSizeGeneratedByTransportPodsArrival
		{
			get
			{
				return SiteCoreWorker.MapSize;
			}
		}

		public bool KnownDanger
		{
			get
			{
				bool result;
				if (this.LeadingSiteDef.knownDanger)
				{
					result = true;
				}
				else
				{
					if (this.writeSiteParts)
					{
						for (int i = 0; i < this.parts.Count; i++)
						{
							if (this.parts[i].knownDanger)
								goto IL_0042;
						}
					}
					result = false;
				}
				goto IL_0067;
				IL_0067:
				return result;
				IL_0042:
				result = true;
				goto IL_0067;
			}
		}

		public override Material Material
		{
			get
			{
				if ((UnityEngine.Object)this.cachedMat == (UnityEngine.Object)null)
				{
					Color color = (!this.LeadingSiteDef.applyFactionColorToSiteTexture || base.Faction == null) ? Color.white : base.Faction.Color;
					this.cachedMat = MaterialPool.MatFrom(this.LeadingSiteDef.siteTexture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		public override bool AppendFactionToInspectString
		{
			get
			{
				return this.LeadingSiteDef.applyFactionColorToSiteTexture || this.LeadingSiteDef.showFactionInInspectString;
			}
		}

		private SiteDefBase LeadingSiteDef
		{
			get
			{
				return (SiteDefBase)((this.core != SiteCoreDefOf.Nothing || !this.parts.Any()) ? ((object)this.core) : ((object)this.parts[0]));
			}
		}

		public override IEnumerable<GenStepDef> ExtraGenStepDefs
		{
			get
			{
				using (IEnumerator<GenStepDef> enumerator = this._003Cget_ExtraGenStepDefs_003E__BaseCallProxy0().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						GenStepDef g = enumerator.Current;
						yield return g;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				List<GenStepDef> coreGenStepDefs = this.core.ExtraGenSteps;
				int k = 0;
				if (k < coreGenStepDefs.Count)
				{
					yield return coreGenStepDefs[k];
					/*Error: Unable to find new state assignment for yield return*/;
				}
				int j = 0;
				List<GenStepDef> partGenStepDefs;
				int i;
				while (true)
				{
					if (j < this.parts.Count)
					{
						partGenStepDefs = this.parts[j].ExtraGenSteps;
						i = 0;
						if (i < partGenStepDefs.Count)
							break;
						j++;
						continue;
					}
					yield break;
				}
				yield return partGenStepDefs[i];
				/*Error: Unable to find new state assignment for yield return*/;
				IL_01ed:
				/*Error near IL_01ee: Unexpected return in MoveNext()*/;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", (string)null, false);
			Scribe_Defs.Look<SiteCoreDef>(ref this.core, "core");
			Scribe_Collections.Look<SitePartDef>(ref this.parts, "parts", LookMode.Def, new object[0]);
			Scribe_Values.Look<bool>(ref this.startedCountdown, "startedCountdown", false, false);
			Scribe_Values.Look<bool>(ref this.anyEnemiesInitially, "anyEnemiesInitially", false, false);
			Scribe_Values.Look<bool>(ref this.writeSiteParts, "writeSiteParts", false, false);
		}

		public override void Tick()
		{
			base.Tick();
			this.core.Worker.SiteCoreWorkerTick(this);
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Worker.SitePartWorkerTick(this);
			}
			if (base.HasMap)
			{
				this.CheckStartForceExitAndRemoveMapCountdown();
			}
		}

		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			Map map = base.Map;
			this.core.Worker.PostMapGenerate(map);
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Worker.PostMapGenerate(map);
			}
			this.anyEnemiesInitially = GenHostility.AnyHostileActiveThreatToPlayer(base.Map);
		}

		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = true;
			return !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			using (IEnumerator<FloatMenuOption> enumerator = this._003CGetFloatMenuOptions_003E__BaseCallProxy1(caravan).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					FloatMenuOption f2 = enumerator.Current;
					yield return f2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			using (IEnumerator<FloatMenuOption> enumerator2 = this.core.Worker.GetFloatMenuOptions(caravan, this).GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					FloatMenuOption f = enumerator2.Current;
					yield return f;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_016d:
			/*Error near IL_016e: Unexpected return in MoveNext()*/;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy2().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!base.HasMap)
				yield break;
			if (Find.WorldSelector.SingleSelectedObject != this)
				yield break;
			yield return (Gizmo)SettleInExistingMapUtility.SettleCommand(base.Map, true);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0111:
			/*Error near IL_0112: Unexpected return in MoveNext()*/;
		}

		private void CheckStartForceExitAndRemoveMapCountdown()
		{
			if (!this.startedCountdown && !GenHostility.AnyHostileActiveThreatToPlayer(base.Map))
			{
				this.startedCountdown = true;
				int num = Mathf.RoundToInt((float)(this.core.forceExitAndRemoveMapCountdownDurationDays * 60000.0));
				string text = (!this.anyEnemiesInitially) ? "MessageSiteCountdownBecauseNoEnemiesInitially".Translate(TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(num)) : "MessageSiteCountdownBecauseNoMoreEnemies".Translate(TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(num));
				Messages.Message(text, (WorldObject)this, MessageTypeDefOf.PositiveEvent);
				base.GetComponent<TimedForcedExit>().StartForceExitAndRemoveMapCountdown(num);
				TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, base.Map.mapPawns.FreeColonists.RandomElement());
			}
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.writeSiteParts)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				if (this.parts.Count == 0)
				{
					stringBuilder.Append("KnownSiteThreatsNone".Translate());
				}
				else if (this.parts.Count == 1)
				{
					stringBuilder.Append("KnownSiteThreat".Translate(this.parts[0].LabelCap));
				}
				else
				{
					stringBuilder.Append("KnownSiteThreats".Translate(GenText.ToCommaList(from x in this.parts
					select x.LabelCap, true)));
				}
			}
			return stringBuilder.ToString();
		}

		public override string GetDescription()
		{
			string text = this.LeadingSiteDef.description;
			string description = base.GetDescription();
			if (!description.NullOrEmpty())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += description;
			}
			return text;
		}
	}
}
