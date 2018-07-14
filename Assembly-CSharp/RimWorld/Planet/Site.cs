using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class Site : MapParent
	{
		public string customLabel;

		public SiteCore core;

		public List<SitePart> parts = new List<SitePart>();

		public bool sitePartsKnown;

		public bool factionMustRemainHostile;

		public float desiredThreatPoints;

		private bool startedCountdown;

		private bool anyEnemiesInitially;

		private Material cachedMat;

		private static List<SiteCoreOrPartDefBase> tmpDefs = new List<SiteCoreOrPartDefBase>();

		private static List<SiteCoreOrPartDefBase> tmpUsedDefs = new List<SiteCoreOrPartDefBase>();

		private static List<string> tmpSitePartsLabels = new List<string>();

		public Site()
		{
		}

		public override string Label
		{
			get
			{
				string result;
				if (!this.customLabel.NullOrEmpty())
				{
					result = this.customLabel;
				}
				else if (this.MainSiteDef == SiteCoreDefOf.PreciousLump && this.core.parms.preciousLumpResources != null)
				{
					result = "PreciousLumpLabel".Translate(new object[]
					{
						this.core.parms.preciousLumpResources.label
					});
				}
				else
				{
					result = this.MainSiteDef.label;
				}
				return result;
			}
		}

		public override Texture2D ExpandingIcon
		{
			get
			{
				return this.MainSiteDef.ExpandingIconTexture;
			}
		}

		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (this.MainSiteDef.applyFactionColorToSiteTexture && base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.MainSiteDef.siteTexture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		public override bool AppendFactionToInspectString
		{
			get
			{
				return this.MainSiteDef.applyFactionColorToSiteTexture || this.MainSiteDef.showFactionInInspectString;
			}
		}

		private SiteCoreOrPartBase MainSiteCoreOrPart
		{
			get
			{
				SiteCoreOrPartBase result;
				if (this.core.def == SiteCoreDefOf.Nothing && this.parts.Any<SitePart>())
				{
					result = this.parts[0];
				}
				else
				{
					result = this.core;
				}
				return result;
			}
		}

		private SiteCoreOrPartDefBase MainSiteDef
		{
			get
			{
				return this.MainSiteCoreOrPart.Def;
			}
		}

		public override IEnumerable<GenStepWithParams> ExtraGenStepDefs
		{
			get
			{
				foreach (GenStepWithParams g in this.<get_ExtraGenStepDefs>__BaseCallProxy0())
				{
					yield return g;
				}
				GenStepParams coreGenStepParms = default(GenStepParams);
				coreGenStepParms.siteCoreOrPart = this.core;
				List<GenStepDef> coreGenStepDefs = this.core.def.ExtraGenSteps;
				for (int i = 0; i < coreGenStepDefs.Count; i++)
				{
					yield return new GenStepWithParams(coreGenStepDefs[i], coreGenStepParms);
				}
				for (int j = 0; j < this.parts.Count; j++)
				{
					GenStepParams partGenStepParams = default(GenStepParams);
					partGenStepParams.siteCoreOrPart = this.parts[j];
					List<GenStepDef> partGenStepDefs = this.parts[j].def.ExtraGenSteps;
					for (int k = 0; k < partGenStepDefs.Count; k++)
					{
						yield return new GenStepWithParams(partGenStepDefs[k], partGenStepParams);
					}
				}
				yield break;
			}
		}

		public string ApproachOrderString
		{
			get
			{
				return (!this.MainSiteDef.approachOrderString.NullOrEmpty()) ? string.Format(this.MainSiteDef.approachOrderString, this.Label) : "ApproachSite".Translate(new object[]
				{
					this.Label
				});
			}
		}

		public string ApproachingReportString
		{
			get
			{
				return (!this.MainSiteDef.approachingReportString.NullOrEmpty()) ? string.Format(this.MainSiteDef.approachingReportString, this.Label) : "ApproachingSite".Translate(new object[]
				{
					this.Label
				});
			}
		}

		public float ActualThreatPoints
		{
			get
			{
				float num = this.core.parms.threatPoints;
				for (int i = 0; i < this.parts.Count; i++)
				{
					num += this.parts[i].parms.threatPoints;
				}
				return num;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
			Scribe_Deep.Look<SiteCore>(ref this.core, "core", new object[0]);
			Scribe_Collections.Look<SitePart>(ref this.parts, "parts", LookMode.Deep, new object[0]);
			Scribe_Values.Look<bool>(ref this.startedCountdown, "startedCountdown", false, false);
			Scribe_Values.Look<bool>(ref this.anyEnemiesInitially, "anyEnemiesInitially", false, false);
			Scribe_Values.Look<bool>(ref this.sitePartsKnown, "sitePartsKnown", false, false);
			Scribe_Values.Look<bool>(ref this.factionMustRemainHostile, "factionMustRemainHostile", false, false);
			Scribe_Values.Look<float>(ref this.desiredThreatPoints, "desiredThreatPoints", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.SitePostLoadInit(this);
			}
		}

		public override void Tick()
		{
			base.Tick();
			this.core.def.Worker.SiteCoreWorkerTick(this);
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.SitePartWorkerTick(this);
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
			this.core.def.Worker.PostMapGenerate(map);
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostMapGenerate(map);
			}
			this.anyEnemiesInitially = GenHostility.AnyHostileActiveThreatToPlayer(base.Map);
			LookTargets lookTargets = new LookTargets();
			StringBuilder stringBuilder = new StringBuilder();
			Site.tmpUsedDefs.Clear();
			Site.tmpDefs.Clear();
			Site.tmpDefs.Add(this.core.def);
			for (int j = 0; j < this.parts.Count; j++)
			{
				Site.tmpDefs.Add(this.parts[j].def);
			}
			LetterDef letterDef = null;
			string text = null;
			for (int k = 0; k < Site.tmpDefs.Count; k++)
			{
				string text2;
				LetterDef letterDef2;
				LookTargets lookTargets2;
				string arrivedLetterPart = Site.tmpDefs[k].Worker.GetArrivedLetterPart(map, out text2, out letterDef2, out lookTargets2);
				if (arrivedLetterPart != null)
				{
					if (!Site.tmpUsedDefs.Contains(Site.tmpDefs[k]))
					{
						Site.tmpUsedDefs.Add(Site.tmpDefs[k]);
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine();
						}
						stringBuilder.Append(arrivedLetterPart);
					}
					if (text == null)
					{
						text = text2;
					}
					if (letterDef == null)
					{
						letterDef = letterDef2;
					}
					if (lookTargets2.IsValid())
					{
						lookTargets = new LookTargets(lookTargets.targets.Concat(lookTargets2.targets));
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				Find.LetterStack.ReceiveLetter(text ?? "LetterLabelPlayerEnteredNewSiteGeneric".Translate(), stringBuilder.ToString(), letterDef ?? LetterDefOf.NeutralEvent, (!lookTargets.IsValid()) ? this : lookTargets, null, null);
			}
		}

		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = true;
			return !base.Map.mapPawns.AnyPawnBlockingMapRemoval;
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption f in this.<GetFloatMenuOptions>__BaseCallProxy1(caravan))
			{
				yield return f;
			}
			foreach (FloatMenuOption f2 in this.core.def.Worker.GetFloatMenuOptions(caravan, this))
			{
				yield return f2;
			}
			yield break;
		}

		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption o in this.<GetTransportPodsFloatMenuOptions>__BaseCallProxy2(pods, representative))
			{
				yield return o;
			}
			foreach (FloatMenuOption o2 in this.core.def.Worker.GetTransportPodsFloatMenuOptions(pods, representative, this))
			{
				yield return o2;
			}
			yield break;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy3())
			{
				yield return g;
			}
			if (base.HasMap && Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return SettleInExistingMapUtility.SettleCommand(base.Map, true);
			}
			yield break;
		}

		private void CheckStartForceExitAndRemoveMapCountdown()
		{
			if (!this.startedCountdown)
			{
				if (!GenHostility.AnyHostileActiveThreatToPlayer(base.Map))
				{
					this.startedCountdown = true;
					int num = Mathf.RoundToInt(this.core.def.forceExitAndRemoveMapCountdownDurationDays * 60000f);
					string text = (!this.anyEnemiesInitially) ? "MessageSiteCountdownBecauseNoEnemiesInitially".Translate(new object[]
					{
						TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(num)
					}) : "MessageSiteCountdownBecauseNoMoreEnemies".Translate(new object[]
					{
						TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(num)
					});
					Messages.Message(text, this, MessageTypeDefOf.PositiveEvent, true);
					base.GetComponent<TimedForcedExit>().StartForceExitAndRemoveMapCountdown(num);
					TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, new object[]
					{
						base.Map.mapPawns.FreeColonists.RandomElement<Pawn>()
					});
				}
			}
		}

		public override bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			Site site = other as Site;
			return site != null && site.MainSiteDef == this.MainSiteDef;
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.sitePartsKnown)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				Site.tmpSitePartsLabels.Clear();
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (!this.parts[i].def.alwaysHidden)
					{
						Site.tmpSitePartsLabels.Add(this.parts[i].def.Worker.GetPostProcessedThreatLabel(this, this.parts[i]));
					}
				}
				if (Site.tmpSitePartsLabels.Count == 0)
				{
					stringBuilder.Append("KnownSiteThreatsNone".Translate());
				}
				else if (Site.tmpSitePartsLabels.Count == 1)
				{
					stringBuilder.Append("KnownSiteThreat".Translate(new object[]
					{
						Site.tmpSitePartsLabels[0].CapitalizeFirst()
					}));
				}
				else
				{
					stringBuilder.Append("KnownSiteThreats".Translate(new object[]
					{
						Site.tmpSitePartsLabels.ToCommaList(true).CapitalizeFirst()
					}));
				}
			}
			return stringBuilder.ToString();
		}

		public override string GetDescription()
		{
			string text = this.MainSiteDef.description;
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

		// Note: this type is marked as 'beforefieldinit'.
		static Site()
		{
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<GenStepWithParams> <get_ExtraGenStepDefs>__BaseCallProxy0()
		{
			return base.ExtraGenStepDefs;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<FloatMenuOption> <GetFloatMenuOptions>__BaseCallProxy1(Caravan caravan)
		{
			return base.GetFloatMenuOptions(caravan);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<FloatMenuOption> <GetTransportPodsFloatMenuOptions>__BaseCallProxy2(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			return base.GetTransportPodsFloatMenuOptions(pods, representative);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy3()
		{
			return base.GetGizmos();
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<GenStepWithParams>, IEnumerator, IDisposable, IEnumerator<GenStepWithParams>
		{
			internal IEnumerator<GenStepWithParams> $locvar0;

			internal GenStepWithParams <g>__1;

			internal GenStepParams <coreGenStepParms>__0;

			internal List<GenStepDef> <coreGenStepDefs>__0;

			internal int <i>__2;

			internal int <i>__3;

			internal GenStepParams <partGenStepParams>__4;

			internal List<GenStepDef> <partGenStepDefs>__4;

			internal int <j>__5;

			internal Site $this;

			internal GenStepWithParams $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<get_ExtraGenStepDefs>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					i++;
					goto IL_14E;
				case 3u:
					k++;
					goto IL_219;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						g = enumerator.Current;
						this.$current = g;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				coreGenStepParms = default(GenStepParams);
				coreGenStepParms.siteCoreOrPart = this.core;
				coreGenStepDefs = this.core.def.ExtraGenSteps;
				i = 0;
				IL_14E:
				if (i >= coreGenStepDefs.Count)
				{
					j = 0;
					goto IL_23E;
				}
				this.$current = new GenStepWithParams(coreGenStepDefs[i], coreGenStepParms);
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_219:
				if (k < partGenStepDefs.Count)
				{
					this.$current = new GenStepWithParams(partGenStepDefs[k], partGenStepParams);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				j++;
				IL_23E:
				if (j < this.parts.Count)
				{
					partGenStepParams = default(GenStepParams);
					partGenStepParams.siteCoreOrPart = this.parts[j];
					partGenStepDefs = this.parts[j].def.ExtraGenSteps;
					k = 0;
					goto IL_219;
				}
				this.$PC = -1;
				return false;
			}

			GenStepWithParams IEnumerator<GenStepWithParams>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.GenStepWithParams>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<GenStepWithParams> IEnumerable<GenStepWithParams>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Site.<>c__Iterator0 <>c__Iterator = new Site.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator1 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal Caravan caravan;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <f>__1;

			internal IEnumerator<FloatMenuOption> $locvar1;

			internal FloatMenuOption <f>__2;

			internal Site $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetFloatMenuOptions>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<GetFloatMenuOptions>__BaseCallProxy1(caravan).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_F3;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						f = enumerator.Current;
						this.$current = f;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				enumerator2 = this.core.def.Worker.GetFloatMenuOptions(caravan, this).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_F3:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						f2 = enumerator2.Current;
						this.$current = f2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			FloatMenuOption IEnumerator<FloatMenuOption>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Site.<GetFloatMenuOptions>c__Iterator1 <GetFloatMenuOptions>c__Iterator = new Site.<GetFloatMenuOptions>c__Iterator1();
				<GetFloatMenuOptions>c__Iterator.$this = this;
				<GetFloatMenuOptions>c__Iterator.caravan = caravan;
				return <GetFloatMenuOptions>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetTransportPodsFloatMenuOptions>c__Iterator2 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal IEnumerable<IThingHolder> pods;

			internal CompLaunchable representative;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <o>__1;

			internal IEnumerator<FloatMenuOption> $locvar1;

			internal FloatMenuOption <o>__2;

			internal Site $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetTransportPodsFloatMenuOptions>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<GetTransportPodsFloatMenuOptions>__BaseCallProxy2(pods, representative).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_FF;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						o = enumerator.Current;
						this.$current = o;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				enumerator2 = this.core.def.Worker.GetTransportPodsFloatMenuOptions(pods, representative, this).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_FF:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						o2 = enumerator2.Current;
						this.$current = o2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			FloatMenuOption IEnumerator<FloatMenuOption>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Site.<GetTransportPodsFloatMenuOptions>c__Iterator2 <GetTransportPodsFloatMenuOptions>c__Iterator = new Site.<GetTransportPodsFloatMenuOptions>c__Iterator2();
				<GetTransportPodsFloatMenuOptions>c__Iterator.$this = this;
				<GetTransportPodsFloatMenuOptions>c__Iterator.pods = pods;
				<GetTransportPodsFloatMenuOptions>c__Iterator.representative = representative;
				return <GetTransportPodsFloatMenuOptions>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator3 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal Site $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<GetGizmos>__BaseCallProxy3().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_108;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						g = enumerator.Current;
						this.$current = g;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (!base.HasMap || Find.WorldSelector.SingleSelectedObject != this)
				{
					goto IL_108;
				}
				this.$current = SettleInExistingMapUtility.SettleCommand(base.Map, true);
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_108:
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Site.<GetGizmos>c__Iterator3 <GetGizmos>c__Iterator = new Site.<GetGizmos>c__Iterator3();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}
		}
	}
}
