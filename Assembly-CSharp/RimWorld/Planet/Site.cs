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

		public SiteCoreDef core;

		public List<SitePartDef> parts = new List<SitePartDef>();

		public bool writeSiteParts;

		public bool factionMustRemainHostile;

		private bool startedCountdown;

		private bool anyEnemiesInitially;

		private Material cachedMat;

		private static List<SiteDefBase> tmpDefs = new List<SiteDefBase>();

		private static List<SiteDefBase> tmpUsedDefs = new List<SiteDefBase>();

		[CompilerGenerated]
		private static Func<SitePartDef, string> <>f__am$cache0;

		public Site()
		{
		}

		public override string Label
		{
			get
			{
				string label;
				if (!this.customLabel.NullOrEmpty())
				{
					label = this.customLabel;
				}
				else if (this.core == SiteCoreDefOf.Nothing && this.parts.Any<SitePartDef>())
				{
					label = this.parts[0].label;
				}
				else
				{
					label = this.core.label;
				}
				return label;
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

		private SiteDefBase MainSiteDef
		{
			get
			{
				SiteDefBase result;
				if (this.core == SiteCoreDefOf.Nothing && this.parts.Any<SitePartDef>())
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

		public override IEnumerable<GenStepDef> ExtraGenStepDefs
		{
			get
			{
				foreach (GenStepDef g in this.<get_ExtraGenStepDefs>__BaseCallProxy0())
				{
					yield return g;
				}
				List<GenStepDef> coreGenStepDefs = this.core.ExtraGenSteps;
				for (int i = 0; i < coreGenStepDefs.Count; i++)
				{
					yield return coreGenStepDefs[i];
				}
				for (int j = 0; j < this.parts.Count; j++)
				{
					List<GenStepDef> partGenStepDefs = this.parts[j].ExtraGenSteps;
					for (int k = 0; k < partGenStepDefs.Count; k++)
					{
						yield return partGenStepDefs[k];
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

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
			Scribe_Defs.Look<SiteCoreDef>(ref this.core, "core");
			Scribe_Collections.Look<SitePartDef>(ref this.parts, "parts", LookMode.Def, new object[0]);
			Scribe_Values.Look<bool>(ref this.startedCountdown, "startedCountdown", false, false);
			Scribe_Values.Look<bool>(ref this.anyEnemiesInitially, "anyEnemiesInitially", false, false);
			Scribe_Values.Look<bool>(ref this.writeSiteParts, "writeSiteParts", false, false);
			Scribe_Values.Look<bool>(ref this.factionMustRemainHostile, "factionMustRemainHostile", false, false);
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
			LookTargets lookTargets = new LookTargets();
			StringBuilder stringBuilder = new StringBuilder();
			Site.tmpUsedDefs.Clear();
			Site.tmpDefs.Clear();
			Site.tmpDefs.Add(this.core);
			for (int j = 0; j < this.parts.Count; j++)
			{
				Site.tmpDefs.Add(this.parts[j]);
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
			foreach (FloatMenuOption f2 in this.core.Worker.GetFloatMenuOptions(caravan, this))
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
			foreach (FloatMenuOption o2 in this.core.Worker.GetTransportPodsFloatMenuOptions(pods, representative, this))
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
					int num = Mathf.RoundToInt(this.core.forceExitAndRemoveMapCountdownDurationDays * 60000f);
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
					stringBuilder.Append("KnownSiteThreat".Translate(new object[]
					{
						this.parts[0].LabelCap
					}));
				}
				else
				{
					StringBuilder stringBuilder2 = stringBuilder;
					string key = "KnownSiteThreats";
					object[] array = new object[1];
					array[0] = (from x in this.parts
					select x.LabelCap).ToCommaList(true);
					stringBuilder2.Append(key.Translate(array));
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
		private IEnumerable<GenStepDef> <get_ExtraGenStepDefs>__BaseCallProxy0()
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
		private static string <GetInspectString>m__0(SitePartDef x)
		{
			return x.LabelCap;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<GenStepDef>, IEnumerator, IDisposable, IEnumerator<GenStepDef>
		{
			internal IEnumerator<GenStepDef> $locvar0;

			internal GenStepDef <g>__1;

			internal List<GenStepDef> <coreGenStepDefs>__0;

			internal int <i>__2;

			internal int <i>__3;

			internal List<GenStepDef> <partGenStepDefs>__4;

			internal int <j>__5;

			internal Site $this;

			internal GenStepDef $current;

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
					goto IL_119;
				case 3u:
					k++;
					goto IL_1A4;
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
				coreGenStepDefs = this.core.ExtraGenSteps;
				i = 0;
				IL_119:
				if (i >= coreGenStepDefs.Count)
				{
					j = 0;
					goto IL_1C9;
				}
				this.$current = coreGenStepDefs[i];
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_1A4:
				if (k < partGenStepDefs.Count)
				{
					this.$current = partGenStepDefs[k];
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				j++;
				IL_1C9:
				if (j < this.parts.Count)
				{
					partGenStepDefs = this.parts[j].ExtraGenSteps;
					k = 0;
					goto IL_1A4;
				}
				this.$PC = -1;
				return false;
			}

			GenStepDef IEnumerator<GenStepDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.GenStepDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<GenStepDef> IEnumerable<GenStepDef>.GetEnumerator()
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
					goto IL_EE;
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
				enumerator2 = this.core.Worker.GetFloatMenuOptions(caravan, this).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_EE:
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
					goto IL_FA;
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
				enumerator2 = this.core.Worker.GetTransportPodsFloatMenuOptions(pods, representative, this).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_FA:
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
