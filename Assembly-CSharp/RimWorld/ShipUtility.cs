using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ShipUtility
	{
		private static Dictionary<ThingDef, int> requiredParts;

		private static List<Building> closedSet = new List<Building>();

		private static List<Building> openSet = new List<Building>();

		public static Dictionary<ThingDef, int> RequiredParts()
		{
			if (ShipUtility.requiredParts == null)
			{
				ShipUtility.requiredParts = new Dictionary<ThingDef, int>();
				ShipUtility.requiredParts[ThingDefOf.Ship_CryptosleepCasket] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_ComputerCore] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_Reactor] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_Engine] = 3;
				ShipUtility.requiredParts[ThingDefOf.Ship_Beam] = 1;
				ShipUtility.requiredParts[ThingDefOf.Ship_SensorCluster] = 1;
			}
			return ShipUtility.requiredParts;
		}

		public static IEnumerable<string> LaunchFailReasons(Building rootBuilding)
		{
			List<Building> shipParts = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>();
			using (Dictionary<ThingDef, int>.Enumerator enumerator = ShipUtility.RequiredParts().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ThingDef, int> partDef = enumerator.Current;
					int shipPartCount = shipParts.Count((Building pa) => pa.def == partDef.Key);
					if (shipPartCount < partDef.Value)
					{
						yield return string.Format("{0}: {1}x {2} ({3} {4})", new object[]
						{
							"ShipReportMissingPart".Translate(),
							partDef.Value - shipPartCount,
							partDef.Key.label,
							"ShipReportMissingPartRequires".Translate(),
							partDef.Value
						});
					}
				}
			}
			bool fullPodFound = false;
			foreach (Building building in shipParts)
			{
				if (building.def == ThingDefOf.Ship_CryptosleepCasket)
				{
					Building_CryptosleepCasket building_CryptosleepCasket = building as Building_CryptosleepCasket;
					if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents)
					{
						fullPodFound = true;
						break;
					}
				}
			}
			foreach (Building part in shipParts)
			{
				CompHibernatable hibernatable = part.TryGetComp<CompHibernatable>();
				if (hibernatable != null && hibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					yield return string.Format("{0}: {1}", "ShipReportHibernating".Translate(), part.LabelCap);
				}
				else if (hibernatable != null && !hibernatable.Running)
				{
					yield return string.Format("{0}: {1}", "ShipReportNotReady".Translate(), part.LabelCap);
				}
			}
			if (!fullPodFound)
			{
				yield return "ShipReportNoFullPods".Translate();
			}
			yield break;
		}

		public static bool HasHibernatingParts(Building rootBuilding)
		{
			List<Building> list = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>();
			foreach (Building thing in list)
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					return true;
				}
			}
			return false;
		}

		public static void StartupHibernatingParts(Building rootBuilding)
		{
			List<Building> list = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>();
			foreach (Building thing in list)
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
				{
					compHibernatable.Startup();
				}
			}
		}

		public static List<Building> ShipBuildingsAttachedTo(Building root)
		{
			ShipUtility.closedSet.Clear();
			List<Building> result;
			if (root == null || root.Destroyed)
			{
				result = ShipUtility.closedSet;
			}
			else
			{
				ShipUtility.openSet.Clear();
				ShipUtility.openSet.Add(root);
				while (ShipUtility.openSet.Count > 0)
				{
					Building building = ShipUtility.openSet[ShipUtility.openSet.Count - 1];
					ShipUtility.openSet.Remove(building);
					ShipUtility.closedSet.Add(building);
					foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(building))
					{
						Building edifice = c.GetEdifice(building.Map);
						if (edifice != null && edifice.def.building.shipPart && !ShipUtility.closedSet.Contains(edifice) && !ShipUtility.openSet.Contains(edifice))
						{
							ShipUtility.openSet.Add(edifice);
						}
					}
				}
				result = ShipUtility.closedSet;
			}
			return result;
		}

		public static IEnumerable<Gizmo> ShipStartupGizmos(Building building)
		{
			if (ShipUtility.HasHibernatingParts(building))
			{
				yield return new Command_Action
				{
					action = delegate()
					{
						string text = "HibernateWarning";
						if (building.Map.info.parent.GetComponent<EscapeShipComp>() == null)
						{
							text += "Standalone";
						}
						if (!Find.Storyteller.difficulty.allowBigThreats)
						{
							text += "Pacifist";
						}
						DiaNode diaNode = new DiaNode(text.Translate());
						DiaOption diaOption = new DiaOption("Confirm".Translate());
						diaOption.action = delegate()
						{
							ShipUtility.StartupHibernatingParts(building);
						};
						diaOption.resolveTree = true;
						diaNode.options.Add(diaOption);
						DiaOption diaOption2 = new DiaOption("GoBack".Translate());
						diaOption2.resolveTree = true;
						diaNode.options.Add(diaOption2);
						Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
					},
					defaultLabel = "CommandShipStartup".Translate(),
					defaultDesc = "CommandShipStartupDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc1,
					icon = ContentFinder<Texture2D>.Get("UI/Commands/DesirePower", true)
				};
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ShipUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <LaunchFailReasons>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal Building rootBuilding;

			internal List<Building> <shipParts>__0;

			internal Dictionary<ThingDef, int>.Enumerator $locvar0;

			internal int <shipPartCount>__2;

			internal bool <fullPodFound>__0;

			internal List<Building>.Enumerator $locvar1;

			internal List<Building>.Enumerator $locvar2;

			internal Building <part>__3;

			internal CompHibernatable <hibernatable>__4;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			private ShipUtility.<LaunchFailReasons>c__Iterator0.<LaunchFailReasons>c__AnonStorey2 $locvar3;

			[DebuggerHidden]
			public <LaunchFailReasons>c__Iterator0()
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
					shipParts = ShipUtility.ShipBuildingsAttachedTo(rootBuilding).ToList<Building>();
					enumerator = ShipUtility.RequiredParts().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
				case 3u:
					goto IL_22A;
				case 4u:
					goto IL_373;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_15F:
						break;
					}
					if (enumerator.MoveNext())
					{
						KeyValuePair<ThingDef, int> partDef = enumerator.Current;
						shipPartCount = shipParts.Count((Building pa) => pa.def == partDef.Key);
						if (shipPartCount < partDef.Value)
						{
							this.$current = string.Format("{0}: {1}x {2} ({3} {4})", new object[]
							{
								"ShipReportMissingPart".Translate(),
								partDef.Value - shipPartCount,
								partDef.Key.label,
								"ShipReportMissingPartRequires".Translate(),
								partDef.Value
							});
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_15F;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				fullPodFound = false;
				enumerator2 = shipParts.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Building building = enumerator2.Current;
						if (building.def == ThingDefOf.Ship_CryptosleepCasket)
						{
							Building_CryptosleepCasket building_CryptosleepCasket = building as Building_CryptosleepCasket;
							if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents)
							{
								fullPodFound = true;
								break;
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
				enumerator3 = shipParts.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_22A:
					switch (num)
					{
					}
					IL_318:
					if (enumerator3.MoveNext())
					{
						part = enumerator3.Current;
						hibernatable = part.TryGetComp<CompHibernatable>();
						if (hibernatable != null && hibernatable.State == HibernatableStateDefOf.Hibernating)
						{
							this.$current = string.Format("{0}: {1}", "ShipReportHibernating".Translate(), part.LabelCap);
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
						if (hibernatable != null && !hibernatable.Running)
						{
							this.$current = string.Format("{0}: {1}", "ShipReportNotReady".Translate(), part.LabelCap);
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							flag = true;
							return true;
						}
						goto IL_318;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator3).Dispose();
					}
				}
				if (!fullPodFound)
				{
					this.$current = "ShipReportNoFullPods".Translate();
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_373:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
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
						((IDisposable)enumerator).Dispose();
					}
					break;
				case 2u:
				case 3u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator3).Dispose();
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ShipUtility.<LaunchFailReasons>c__Iterator0 <LaunchFailReasons>c__Iterator = new ShipUtility.<LaunchFailReasons>c__Iterator0();
				<LaunchFailReasons>c__Iterator.rootBuilding = rootBuilding;
				return <LaunchFailReasons>c__Iterator;
			}

			private sealed class <LaunchFailReasons>c__AnonStorey2
			{
				internal KeyValuePair<ThingDef, int> partDef;

				public <LaunchFailReasons>c__AnonStorey2()
				{
				}

				internal bool <>m__0(Building pa)
				{
					return pa.def == this.partDef.Key;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ShipStartupGizmos>c__Iterator1 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Building building;

			internal Command_Action <wakeup>__1;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			private ShipUtility.<ShipStartupGizmos>c__Iterator1.<ShipStartupGizmos>c__AnonStorey3 $locvar0;

			[DebuggerHidden]
			public <ShipStartupGizmos>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (ShipUtility.HasHibernatingParts(building))
					{
						Command_Action wakeup = new Command_Action();
						wakeup.action = delegate()
						{
							string text = "HibernateWarning";
							if (building.Map.info.parent.GetComponent<EscapeShipComp>() == null)
							{
								text += "Standalone";
							}
							if (!Find.Storyteller.difficulty.allowBigThreats)
							{
								text += "Pacifist";
							}
							DiaNode diaNode = new DiaNode(text.Translate());
							DiaOption diaOption = new DiaOption("Confirm".Translate());
							diaOption.action = delegate()
							{
								ShipUtility.StartupHibernatingParts(building);
							};
							diaOption.resolveTree = true;
							diaNode.options.Add(diaOption);
							DiaOption diaOption2 = new DiaOption("GoBack".Translate());
							diaOption2.resolveTree = true;
							diaNode.options.Add(diaOption2);
							Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
						};
						wakeup.defaultLabel = "CommandShipStartup".Translate();
						wakeup.defaultDesc = "CommandShipStartupDesc".Translate();
						wakeup.hotKey = KeyBindingDefOf.Misc1;
						wakeup.icon = ContentFinder<Texture2D>.Get("UI/Commands/DesirePower", true);
						this.$current = wakeup;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
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
				this.$disposing = true;
				this.$PC = -1;
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
				ShipUtility.<ShipStartupGizmos>c__Iterator1 <ShipStartupGizmos>c__Iterator = new ShipUtility.<ShipStartupGizmos>c__Iterator1();
				<ShipStartupGizmos>c__Iterator.building = building;
				return <ShipStartupGizmos>c__Iterator;
			}

			private sealed class <ShipStartupGizmos>c__AnonStorey3
			{
				internal Building building;

				public <ShipStartupGizmos>c__AnonStorey3()
				{
				}

				internal void <>m__0()
				{
					string text = "HibernateWarning";
					if (this.building.Map.info.parent.GetComponent<EscapeShipComp>() == null)
					{
						text += "Standalone";
					}
					if (!Find.Storyteller.difficulty.allowBigThreats)
					{
						text += "Pacifist";
					}
					DiaNode diaNode = new DiaNode(text.Translate());
					DiaOption diaOption = new DiaOption("Confirm".Translate());
					diaOption.action = delegate()
					{
						ShipUtility.StartupHibernatingParts(this.building);
					};
					diaOption.resolveTree = true;
					diaNode.options.Add(diaOption);
					DiaOption diaOption2 = new DiaOption("GoBack".Translate());
					diaOption2.resolveTree = true;
					diaNode.options.Add(diaOption2);
					Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
				}

				internal void <>m__1()
				{
					ShipUtility.StartupHibernatingParts(this.building);
				}
			}
		}
	}
}
