using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;

namespace Verse
{
	public sealed class DesignationManager : IExposable
	{
		public Map map;

		public List<Designation> allDesignations = new List<Designation>();

		[CompilerGenerated]
		private static Predicate<Designation> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<Designation> <>f__am$cache1;

		public DesignationManager(Map map)
		{
			this.map = map;
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Designation>(ref this.allDesignations, "allDesignations", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.allDesignations.RemoveAll((Designation x) => x == null) != 0)
				{
					Log.Warning("Some designations were null after loading.", false);
				}
				if (this.allDesignations.RemoveAll((Designation x) => x.def == null) != 0)
				{
					Log.Warning("Some designations had null def after loading.", false);
				}
				for (int i = 0; i < this.allDesignations.Count; i++)
				{
					this.allDesignations[i].designationManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = this.allDesignations.Count - 1; j >= 0; j--)
				{
					TargetType targetType = this.allDesignations[j].def.targetType;
					if (targetType != TargetType.Thing)
					{
						if (targetType == TargetType.Cell)
						{
							if (!this.allDesignations[j].target.Cell.IsValid)
							{
								Log.Error("Cell-needing designation " + this.allDesignations[j] + " had no cell target. Removing...", false);
								this.allDesignations.RemoveAt(j);
							}
						}
					}
					else if (!this.allDesignations[j].target.HasThing)
					{
						Log.Error("Thing-needing designation " + this.allDesignations[j] + " had no thing target. Removing...", false);
						this.allDesignations.RemoveAt(j);
					}
				}
			}
		}

		public void DrawDesignations()
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				if (!this.allDesignations[i].target.HasThing || this.allDesignations[i].target.Thing.Map == this.map)
				{
					this.allDesignations[i].DesignationDraw();
				}
			}
		}

		public void AddDesignation(Designation newDes)
		{
			if (newDes.def.targetType == TargetType.Cell && this.DesignationAt(newDes.target.Cell, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation at location " + newDes.target, false);
				return;
			}
			if (newDes.def.targetType == TargetType.Thing && this.DesignationOn(newDes.target.Thing, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation on Thing " + newDes.target, false);
				return;
			}
			if (newDes.def.targetType == TargetType.Thing)
			{
				newDes.target.Thing.SetForbidden(false, false);
			}
			this.allDesignations.Add(newDes);
			newDes.designationManager = this;
			newDes.Notify_Added();
			Map map = (!newDes.target.HasThing) ? this.map : newDes.target.Thing.Map;
			if (map != null)
			{
				MoteMaker.ThrowMetaPuffs(newDes.target.ToTargetInfo(map));
			}
		}

		public Designation DesignationOn(Thing t)
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.target.Thing == t)
				{
					return designation;
				}
			}
			return null;
		}

		public Designation DesignationOn(Thing t, DesignationDef def)
		{
			if (def.targetType == TargetType.Cell)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by location only and you are trying to get one on a Thing.", false);
				return null;
			}
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.target.Thing == t && designation.def == def)
				{
					return designation;
				}
			}
			return null;
		}

		public Designation DesignationAt(IntVec3 c, DesignationDef def)
		{
			if (def.targetType == TargetType.Thing)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by Thing only and you are trying to get one on a location.", false);
				return null;
			}
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map) && designation.target.Cell == c)
				{
					return designation;
				}
			}
			return null;
		}

		public IEnumerable<Designation> AllDesignationsOn(Thing t)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.allDesignations[i].target.Thing == t)
				{
					yield return this.allDesignations[i];
				}
			}
			yield break;
		}

		public IEnumerable<Designation> AllDesignationsAt(IntVec3 c)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				Designation des = this.allDesignations[i];
				if ((!des.target.HasThing || des.target.Thing.Map == this.map) && des.target.Cell == c)
				{
					yield return des;
				}
			}
			yield break;
		}

		public bool HasMapDesignationAt(IntVec3 c)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (!designation.target.HasThing && designation.target.Cell == c)
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<Designation> SpawnedDesignationsOfDef(DesignationDef def)
		{
			int count = this.allDesignations.Count;
			for (int i = 0; i < count; i++)
			{
				Designation des = this.allDesignations[i];
				if (des.def == def && (!des.target.HasThing || des.target.Thing.Map == this.map))
				{
					yield return des;
				}
			}
			yield break;
		}

		public void RemoveDesignation(Designation des)
		{
			des.Notify_Removing();
			this.allDesignations.Remove(des);
		}

		public void TryRemoveDesignation(IntVec3 c, DesignationDef def)
		{
			Designation designation = this.DesignationAt(c, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		public void RemoveAllDesignationsOn(Thing t, bool standardCanceling = false)
		{
			for (int i = 0; i < this.allDesignations.Count; i++)
			{
				Designation designation = this.allDesignations[i];
				if (!standardCanceling || designation.def.designateCancelable)
				{
					if (designation.target.Thing == t)
					{
						designation.Notify_Removing();
					}
				}
			}
			this.allDesignations.RemoveAll((Designation d) => (!standardCanceling || d.def.designateCancelable) && d.target.Thing == t);
		}

		public void TryRemoveDesignationOn(Thing t, DesignationDef def)
		{
			Designation designation = this.DesignationOn(t, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		public void RemoveAllDesignationsOfDef(DesignationDef def)
		{
			for (int i = this.allDesignations.Count - 1; i >= 0; i--)
			{
				if (this.allDesignations[i].def == def)
				{
					this.allDesignations[i].Notify_Removing();
					this.allDesignations.RemoveAt(i);
				}
			}
		}

		public void Notify_BuildingDespawned(Thing b)
		{
			CellRect cellRect = b.OccupiedRect();
			for (int i = this.allDesignations.Count - 1; i >= 0; i--)
			{
				Designation designation = this.allDesignations[i];
				if (cellRect.Contains(designation.target.Cell) && designation.def.removeIfBuildingDespawned)
				{
					this.RemoveDesignation(designation);
				}
			}
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__0(Designation x)
		{
			return x == null;
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__1(Designation x)
		{
			return x.def == null;
		}

		[CompilerGenerated]
		private sealed class <AllDesignationsOn>c__Iterator0 : IEnumerable, IEnumerable<Designation>, IEnumerator, IDisposable, IEnumerator<Designation>
		{
			internal int <count>__0;

			internal int <i>__1;

			internal Thing t;

			internal DesignationManager $this;

			internal Designation $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllDesignationsOn>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					count = this.allDesignations.Count;
					i = 0;
					break;
				case 1u:
					IL_9E:
					i++;
					break;
				default:
					return false;
				}
				if (i >= count)
				{
					this.$PC = -1;
				}
				else
				{
					if (this.allDesignations[i].target.Thing == t)
					{
						this.$current = this.allDesignations[i];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_9E;
				}
				return false;
			}

			Designation IEnumerator<Designation>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Designation>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Designation> IEnumerable<Designation>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DesignationManager.<AllDesignationsOn>c__Iterator0 <AllDesignationsOn>c__Iterator = new DesignationManager.<AllDesignationsOn>c__Iterator0();
				<AllDesignationsOn>c__Iterator.$this = this;
				<AllDesignationsOn>c__Iterator.t = t;
				return <AllDesignationsOn>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <AllDesignationsAt>c__Iterator1 : IEnumerable, IEnumerable<Designation>, IEnumerator, IDisposable, IEnumerator<Designation>
		{
			internal int <count>__0;

			internal int <i>__1;

			internal Designation <des>__2;

			internal IntVec3 c;

			internal DesignationManager $this;

			internal Designation $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllDesignationsAt>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					count = this.allDesignations.Count;
					i = 0;
					break;
				case 1u:
					IL_D9:
					i++;
					break;
				default:
					return false;
				}
				if (i >= count)
				{
					this.$PC = -1;
				}
				else
				{
					des = this.allDesignations[i];
					if ((!des.target.HasThing || des.target.Thing.Map == this.map) && des.target.Cell == c)
					{
						this.$current = des;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_D9;
				}
				return false;
			}

			Designation IEnumerator<Designation>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Designation>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Designation> IEnumerable<Designation>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DesignationManager.<AllDesignationsAt>c__Iterator1 <AllDesignationsAt>c__Iterator = new DesignationManager.<AllDesignationsAt>c__Iterator1();
				<AllDesignationsAt>c__Iterator.$this = this;
				<AllDesignationsAt>c__Iterator.c = c;
				return <AllDesignationsAt>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <SpawnedDesignationsOfDef>c__Iterator2 : IEnumerable, IEnumerable<Designation>, IEnumerator, IDisposable, IEnumerator<Designation>
		{
			internal int <count>__0;

			internal int <i>__1;

			internal Designation <des>__2;

			internal DesignationDef def;

			internal DesignationManager $this;

			internal Designation $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <SpawnedDesignationsOfDef>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					count = this.allDesignations.Count;
					i = 0;
					break;
				case 1u:
					IL_CF:
					i++;
					break;
				default:
					return false;
				}
				if (i >= count)
				{
					this.$PC = -1;
				}
				else
				{
					des = this.allDesignations[i];
					if (des.def == def && (!des.target.HasThing || des.target.Thing.Map == this.map))
					{
						this.$current = des;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_CF;
				}
				return false;
			}

			Designation IEnumerator<Designation>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Designation>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Designation> IEnumerable<Designation>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DesignationManager.<SpawnedDesignationsOfDef>c__Iterator2 <SpawnedDesignationsOfDef>c__Iterator = new DesignationManager.<SpawnedDesignationsOfDef>c__Iterator2();
				<SpawnedDesignationsOfDef>c__Iterator.$this = this;
				<SpawnedDesignationsOfDef>c__Iterator.def = def;
				return <SpawnedDesignationsOfDef>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <RemoveAllDesignationsOn>c__AnonStorey3
		{
			internal bool standardCanceling;

			internal Thing t;

			public <RemoveAllDesignationsOn>c__AnonStorey3()
			{
			}

			internal bool <>m__0(Designation d)
			{
				return (!this.standardCanceling || d.def.designateCancelable) && d.target.Thing == this.t;
			}
		}
	}
}
