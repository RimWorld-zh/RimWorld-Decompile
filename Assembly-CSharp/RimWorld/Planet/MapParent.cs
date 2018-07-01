using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class MapParent : WorldObject, IThingHolder
	{
		private HashSet<IncidentTargetTypeDef> hibernatableIncidentTargets;

		private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);

		public MapParent()
		{
		}

		public bool HasMap
		{
			get
			{
				return this.Map != null;
			}
		}

		protected virtual bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return true;
			}
		}

		public Map Map
		{
			get
			{
				return Current.Game.FindMap(this);
			}
		}

		public virtual MapGeneratorDef MapGeneratorDef
		{
			get
			{
				return (this.def.mapGenerator == null) ? MapGeneratorDefOf.Encounter : this.def.mapGenerator;
			}
		}

		public virtual IEnumerable<GenStepWithParams> ExtraGenStepDefs
		{
			get
			{
				yield break;
			}
		}

		public override bool ExpandMore
		{
			get
			{
				return base.ExpandMore || this.HasMap;
			}
		}

		public virtual void PostMapGenerate()
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMapGenerate();
			}
		}

		public virtual void Notify_MyMapRemoved(Map map)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMyMapRemoved();
			}
		}

		public virtual void Notify_CaravanFormed(Caravan caravan)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostCaravanFormed(caravan);
			}
		}

		public virtual void Notify_HibernatableChanged()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		public virtual void FinalizeLoading()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return false;
		}

		public override void PostRemove()
		{
			base.PostRemove();
			if (this.HasMap)
			{
				Current.Game.DeinitAndRemoveMap(this.Map);
			}
		}

		public override void Tick()
		{
			base.Tick();
			this.CheckRemoveMapNow();
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (this.HasMap)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandShowMap".Translate(),
					defaultDesc = "CommandShowMapDesc".Translate(),
					icon = MapParent.ShowMapCommand,
					hotKey = KeyBindingDefOf.Misc1,
					action = delegate()
					{
						Current.Game.CurrentMap = this.Map;
						if (!CameraJumper.TryHideWorld())
						{
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
					}
				};
			}
			yield break;
		}

		public override IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			foreach (IncidentTargetTypeDef type in this.<AcceptedTypes>__BaseCallProxy1())
			{
				yield return type;
			}
			if (this.hibernatableIncidentTargets != null && this.hibernatableIncidentTargets.Count > 0)
			{
				foreach (IncidentTargetTypeDef type2 in this.hibernatableIncidentTargets)
				{
					yield return type2;
				}
			}
			yield break;
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption o in this.<GetFloatMenuOptions>__BaseCallProxy2(caravan))
			{
				yield return o;
			}
			if (this.UseGenericEnterMapFloatMenuOption)
			{
				foreach (FloatMenuOption f in CaravanArrivalAction_Enter.GetFloatMenuOptions(caravan, this))
				{
					yield return f;
				}
			}
			yield break;
		}

		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption o in this.<GetTransportPodsFloatMenuOptions>__BaseCallProxy3(pods, representative))
			{
				yield return o;
			}
			if (TransportPodsArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this))
			{
				yield return new FloatMenuOption("LandInExistingMap".Translate(new object[]
				{
					this.Label
				}), delegate()
				{
					Map myMap = representative.parent.Map;
					Map map = this.Map;
					Current.Game.CurrentMap = map;
					CameraJumper.TryHideWorld();
					Find.Targeter.BeginTargeting(TargetingParameters.ForDropPodsDestination(), delegate(LocalTargetInfo x)
					{
						representative.TryLaunch(this.Tile, new TransportPodsArrivalAction_LandInSpecificCell(this.$this, x.Cell));
					}, null, delegate()
					{
						if (Find.Maps.Contains(myMap))
						{
							Current.Game.CurrentMap = myMap;
						}
					}, CompLaunchable.TargeterMouseAttachment);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
		}

		public void CheckRemoveMapNow()
		{
			bool flag;
			if (this.HasMap && this.ShouldRemoveMapNow(out flag))
			{
				Map map = this.Map;
				Current.Game.DeinitAndRemoveMap(map);
				if (flag)
				{
					Find.WorldObjects.Remove(this);
				}
			}
		}

		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.EnterCooldownBlocksEntering())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "EnterCooldown".Translate(new object[]
				{
					this.EnterCooldownDaysLeft().ToString("0.#")
				});
			}
			return text;
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		public virtual void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.HasMap)
			{
				outChildren.Add(this.Map);
			}
		}

		private void RecalculateHibernatableIncidentTargets()
		{
			this.hibernatableIncidentTargets = null;
			foreach (ThingWithComps thing in this.Map.listerThings.AllThings.OfType<ThingWithComps>())
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Starting && compHibernatable.Props.incidentTargetWhileStarting != null)
				{
					if (this.hibernatableIncidentTargets == null)
					{
						this.hibernatableIncidentTargets = new HashSet<IncidentTargetTypeDef>();
					}
					this.hibernatableIncidentTargets.Add(compHibernatable.Props.incidentTargetWhileStarting);
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static MapParent()
		{
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy0()
		{
			return base.GetGizmos();
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<IncidentTargetTypeDef> <AcceptedTypes>__BaseCallProxy1()
		{
			return base.AcceptedTypes();
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<FloatMenuOption> <GetFloatMenuOptions>__BaseCallProxy2(Caravan caravan)
		{
			return base.GetFloatMenuOptions(caravan);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<FloatMenuOption> <GetTransportPodsFloatMenuOptions>__BaseCallProxy3(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			return base.GetTransportPodsFloatMenuOptions(pods, representative);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<GenStepWithParams>, IEnumerator, IDisposable, IEnumerator<GenStepWithParams>
		{
			internal GenStepWithParams $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
				}
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
				return new MapParent.<>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator1 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal Command_Action <showMap>__2;

			internal MapParent $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator1()
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
					enumerator = base.<GetGizmos>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_158;
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
				if (!base.HasMap)
				{
					goto IL_158;
				}
				Command_Action showMap = new Command_Action();
				showMap.defaultLabel = "CommandShowMap".Translate();
				showMap.defaultDesc = "CommandShowMapDesc".Translate();
				showMap.icon = MapParent.ShowMapCommand;
				showMap.hotKey = KeyBindingDefOf.Misc1;
				showMap.action = delegate()
				{
					Current.Game.CurrentMap = base.Map;
					if (!CameraJumper.TryHideWorld())
					{
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				};
				this.$current = showMap;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_158:
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
				MapParent.<GetGizmos>c__Iterator1 <GetGizmos>c__Iterator = new MapParent.<GetGizmos>c__Iterator1();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			internal void <>m__0()
			{
				Current.Game.CurrentMap = base.Map;
				if (!CameraJumper.TryHideWorld())
				{
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <AcceptedTypes>c__Iterator2 : IEnumerable, IEnumerable<IncidentTargetTypeDef>, IEnumerator, IDisposable, IEnumerator<IncidentTargetTypeDef>
		{
			internal IEnumerator<IncidentTargetTypeDef> $locvar0;

			internal IncidentTargetTypeDef <type>__1;

			internal HashSet<IncidentTargetTypeDef>.Enumerator $locvar1;

			internal IncidentTargetTypeDef <type>__2;

			internal MapParent $this;

			internal IncidentTargetTypeDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AcceptedTypes>c__Iterator2()
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
					enumerator = base.<AcceptedTypes>__BaseCallProxy1().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_F9;
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
						type = enumerator.Current;
						this.$current = type;
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
				if (this.hibernatableIncidentTargets == null || this.hibernatableIncidentTargets.Count <= 0)
				{
					goto IL_16B;
				}
				enumerator2 = this.hibernatableIncidentTargets.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_F9:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						type2 = enumerator2.Current;
						this.$current = type2;
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
						((IDisposable)enumerator2).Dispose();
					}
				}
				IL_16B:
				this.$PC = -1;
				return false;
			}

			IncidentTargetTypeDef IEnumerator<IncidentTargetTypeDef>.Current
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
						((IDisposable)enumerator2).Dispose();
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.IncidentTargetTypeDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IncidentTargetTypeDef> IEnumerable<IncidentTargetTypeDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				MapParent.<AcceptedTypes>c__Iterator2 <AcceptedTypes>c__Iterator = new MapParent.<AcceptedTypes>c__Iterator2();
				<AcceptedTypes>c__Iterator.$this = this;
				return <AcceptedTypes>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator3 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal Caravan caravan;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <o>__1;

			internal IEnumerator<FloatMenuOption> $locvar1;

			internal FloatMenuOption <f>__2;

			internal MapParent $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetFloatMenuOptions>c__Iterator3()
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
					enumerator = base.<GetFloatMenuOptions>__BaseCallProxy2(caravan).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_EF;
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
				if (!this.UseGenericEnterMapFloatMenuOption)
				{
					goto IL_166;
				}
				enumerator2 = CaravanArrivalAction_Enter.GetFloatMenuOptions(caravan, this).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_EF:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						f = enumerator2.Current;
						this.$current = f;
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
				IL_166:
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
				MapParent.<GetFloatMenuOptions>c__Iterator3 <GetFloatMenuOptions>c__Iterator = new MapParent.<GetFloatMenuOptions>c__Iterator3();
				<GetFloatMenuOptions>c__Iterator.$this = this;
				<GetFloatMenuOptions>c__Iterator.caravan = caravan;
				return <GetFloatMenuOptions>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetTransportPodsFloatMenuOptions>c__Iterator4 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal IEnumerable<IThingHolder> pods;

			internal CompLaunchable representative;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <o>__1;

			internal MapParent $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			private MapParent.<GetTransportPodsFloatMenuOptions>c__Iterator4.<GetTransportPodsFloatMenuOptions>c__AnonStorey5 $locvar1;

			[DebuggerHidden]
			public <GetTransportPodsFloatMenuOptions>c__Iterator4()
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
					enumerator = base.<GetTransportPodsFloatMenuOptions>__BaseCallProxy3(pods, representative).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_161;
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
				if (!TransportPodsArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this))
				{
					goto IL_161;
				}
				this.$current = new FloatMenuOption("LandInExistingMap".Translate(new object[]
				{
					this.Label
				}), delegate()
				{
					Map myMap = <GetTransportPodsFloatMenuOptions>c__AnonStorey.representative.parent.Map;
					Map map = <GetTransportPodsFloatMenuOptions>c__AnonStorey.<>f__ref$4.$this.Map;
					Current.Game.CurrentMap = map;
					CameraJumper.TryHideWorld();
					Find.Targeter.BeginTargeting(TargetingParameters.ForDropPodsDestination(), delegate(LocalTargetInfo x)
					{
						<GetTransportPodsFloatMenuOptions>c__AnonStorey.representative.TryLaunch(<GetTransportPodsFloatMenuOptions>c__AnonStorey.<>f__ref$4.$this.Tile, new TransportPodsArrivalAction_LandInSpecificCell(<GetTransportPodsFloatMenuOptions>c__AnonStorey.<>f__ref$4.$this, x.Cell));
					}, null, delegate()
					{
						if (Find.Maps.Contains(myMap))
						{
							Current.Game.CurrentMap = myMap;
						}
					}, CompLaunchable.TargeterMouseAttachment);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_161:
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
				MapParent.<GetTransportPodsFloatMenuOptions>c__Iterator4 <GetTransportPodsFloatMenuOptions>c__Iterator = new MapParent.<GetTransportPodsFloatMenuOptions>c__Iterator4();
				<GetTransportPodsFloatMenuOptions>c__Iterator.$this = this;
				<GetTransportPodsFloatMenuOptions>c__Iterator.pods = pods;
				<GetTransportPodsFloatMenuOptions>c__Iterator.representative = representative;
				return <GetTransportPodsFloatMenuOptions>c__Iterator;
			}

			private sealed class <GetTransportPodsFloatMenuOptions>c__AnonStorey5
			{
				internal CompLaunchable representative;

				internal MapParent.<GetTransportPodsFloatMenuOptions>c__Iterator4 <>f__ref$4;

				public <GetTransportPodsFloatMenuOptions>c__AnonStorey5()
				{
				}

				internal void <>m__0()
				{
					MapParent.<GetTransportPodsFloatMenuOptions>c__Iterator4 <>f__ref$4 = this.<>f__ref$4;
					MapParent.<GetTransportPodsFloatMenuOptions>c__Iterator4.<GetTransportPodsFloatMenuOptions>c__AnonStorey5 <>f__ref$5 = this;
					Map myMap = this.representative.parent.Map;
					Map map = this.<>f__ref$4.$this.Map;
					Current.Game.CurrentMap = map;
					CameraJumper.TryHideWorld();
					Find.Targeter.BeginTargeting(TargetingParameters.ForDropPodsDestination(), delegate(LocalTargetInfo x)
					{
						<>f__ref$5.representative.TryLaunch(<>f__ref$4.$this.Tile, new TransportPodsArrivalAction_LandInSpecificCell(<>f__ref$4.$this, x.Cell));
					}, null, delegate()
					{
						if (Find.Maps.Contains(myMap))
						{
							Current.Game.CurrentMap = myMap;
						}
					}, CompLaunchable.TargeterMouseAttachment);
				}

				private sealed class <GetTransportPodsFloatMenuOptions>c__AnonStorey6
				{
					internal Map myMap;

					internal MapParent.<GetTransportPodsFloatMenuOptions>c__Iterator4 <>f__ref$4;

					internal MapParent.<GetTransportPodsFloatMenuOptions>c__Iterator4.<GetTransportPodsFloatMenuOptions>c__AnonStorey5 <>f__ref$5;

					public <GetTransportPodsFloatMenuOptions>c__AnonStorey6()
					{
					}

					internal void <>m__0(LocalTargetInfo x)
					{
						this.<>f__ref$5.representative.TryLaunch(this.<>f__ref$4.$this.Tile, new TransportPodsArrivalAction_LandInSpecificCell(this.<>f__ref$4.$this, x.Cell));
					}

					internal void <>m__1()
					{
						if (Find.Maps.Contains(this.myMap))
						{
							Current.Game.CurrentMap = this.myMap;
						}
					}
				}
			}
		}
	}
}
