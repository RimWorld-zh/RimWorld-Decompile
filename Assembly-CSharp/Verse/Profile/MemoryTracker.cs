using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Verse.Profile
{
	[HasDebugOutput]
	public static class MemoryTracker
	{
		private static Dictionary<Type, HashSet<WeakReference>> tracked = new Dictionary<Type, HashSet<WeakReference>>();

		private static List<WeakReference> foundCollections = new List<WeakReference>();

		private static bool trackedLocked = false;

		private static List<object> trackedQueue = new List<object>();

		private static List<RuntimeTypeHandle> trackedTypeQueue = new List<RuntimeTypeHandle>();

		private const int updatesPerCull = 10;

		private static int updatesSinceLastCull = 0;

		private static int cullTargetIndex = 0;

		[CompilerGenerated]
		private static Func<KeyValuePair<Type, HashSet<WeakReference>>, int> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<KeyValuePair<Type, HashSet<WeakReference>>, int> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<KeyValuePair<Type, HashSet<WeakReference>>, Type> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<KeyValuePair<Type, HashSet<WeakReference>>, IEnumerable<WeakReference>> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<WeakReference, bool> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<WeakReference, object> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<KeyValuePair<object, MemoryTracker.ReferenceData>, bool> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<KeyValuePair<object, MemoryTracker.ReferenceData>, object> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<KeyValuePair<object, MemoryTracker.ReferenceData>, bool> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<KeyValuePair<object, MemoryTracker.ReferenceData>, object> <>f__am$cache9;

		[CompilerGenerated]
		private static Func<KeyValuePair<object, MemoryTracker.ReferenceData>, bool> <>f__am$cacheA;

		[CompilerGenerated]
		private static Func<KeyValuePair<object, MemoryTracker.ReferenceData>, object> <>f__am$cacheB;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, int>, int> <>f__am$cacheC;

		[CompilerGenerated]
		private static Predicate<WeakReference> <>f__am$cacheD;

		public static bool AnythingTracked
		{
			get
			{
				return MemoryTracker.tracked.Count > 0;
			}
		}

		public static IEnumerable<WeakReference> FoundCollections
		{
			get
			{
				if (MemoryTracker.foundCollections.Count == 0)
				{
					MemoryTracker.LogObjectHoldPathsFor(null, null);
				}
				return MemoryTracker.foundCollections;
			}
		}

		public static void RegisterObject(object obj)
		{
			if (MemoryTracker.trackedLocked)
			{
				MemoryTracker.trackedQueue.Add(obj);
				return;
			}
			Type type = obj.GetType();
			HashSet<WeakReference> hashSet = null;
			if (!MemoryTracker.tracked.TryGetValue(type, out hashSet))
			{
				hashSet = new HashSet<WeakReference>();
				MemoryTracker.tracked[type] = hashSet;
			}
			hashSet.Add(new WeakReference(obj));
		}

		public static void RegisterType(RuntimeTypeHandle typeHandle)
		{
			if (MemoryTracker.trackedLocked)
			{
				MemoryTracker.trackedTypeQueue.Add(typeHandle);
				return;
			}
			Type typeFromHandle = Type.GetTypeFromHandle(typeHandle);
			if (!MemoryTracker.tracked.ContainsKey(typeFromHandle))
			{
				MemoryTracker.tracked[typeFromHandle] = new HashSet<WeakReference>();
			}
		}

		private static void LockTracking()
		{
			if (MemoryTracker.trackedLocked)
			{
				throw new NotImplementedException();
			}
			MemoryTracker.trackedLocked = true;
		}

		private static void UnlockTracking()
		{
			if (!MemoryTracker.trackedLocked)
			{
				throw new NotImplementedException();
			}
			MemoryTracker.trackedLocked = false;
			foreach (object obj in MemoryTracker.trackedQueue)
			{
				MemoryTracker.RegisterObject(obj);
			}
			MemoryTracker.trackedQueue.Clear();
			foreach (RuntimeTypeHandle typeHandle in MemoryTracker.trackedTypeQueue)
			{
				MemoryTracker.RegisterType(typeHandle);
			}
			MemoryTracker.trackedTypeQueue.Clear();
		}

		[Category("System")]
		[DebugOutput]
		private static void ObjectsLoaded()
		{
			if (MemoryTracker.tracked.Count == 0)
			{
				Log.Message("No objects tracked, memory tracker markup may not be applied.", false);
				return;
			}
			GC.Collect();
			MemoryTracker.LockTracking();
			try
			{
				foreach (HashSet<WeakReference> table in MemoryTracker.tracked.Values)
				{
					MemoryTracker.CullNulls(table);
				}
				StringBuilder stringBuilder = new StringBuilder();
				foreach (KeyValuePair<Type, HashSet<WeakReference>> keyValuePair in from kvp in MemoryTracker.tracked
				orderby -kvp.Value.Count
				select kvp)
				{
					stringBuilder.AppendLine(string.Format("{0,6} {1}", keyValuePair.Value.Count, keyValuePair.Key));
				}
				Log.Message(stringBuilder.ToString(), false);
			}
			finally
			{
				MemoryTracker.UnlockTracking();
			}
		}

		[Category("System")]
		[DebugOutput]
		private static void ObjectHoldPaths()
		{
			if (MemoryTracker.tracked.Count == 0)
			{
				Log.Message("No objects tracked, memory tracker markup may not be applied.", false);
				return;
			}
			GC.Collect();
			MemoryTracker.LockTracking();
			try
			{
				foreach (HashSet<WeakReference> table in MemoryTracker.tracked.Values)
				{
					MemoryTracker.CullNulls(table);
				}
				List<Type> list = new List<Type>();
				list.Add(typeof(Map));
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (Type type in list.Concat(from kvp in MemoryTracker.tracked
				orderby -kvp.Value.Count
				select kvp.Key).Take(30))
				{
					Type type2 = type;
					HashSet<WeakReference> trackedBatch = MemoryTracker.tracked.TryGetValue(type2, null);
					if (trackedBatch == null)
					{
						trackedBatch = new HashSet<WeakReference>();
					}
					list2.Add(new FloatMenuOption(string.Format("{0} ({1})", type2, trackedBatch.Count), delegate()
					{
						MemoryTracker.LogObjectHoldPathsFor(trackedBatch, (WeakReference _) => 1);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
					if (list2.Count == 30)
					{
						break;
					}
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			finally
			{
				MemoryTracker.UnlockTracking();
			}
		}

		public static void LogObjectHoldPathsFor(IEnumerable<WeakReference> elements, Func<WeakReference, int> weight)
		{
			GC.Collect();
			MemoryTracker.LockTracking();
			try
			{
				Dictionary<object, MemoryTracker.ReferenceData> dictionary = new Dictionary<object, MemoryTracker.ReferenceData>();
				HashSet<object> hashSet = new HashSet<object>();
				MemoryTracker.foundCollections.Clear();
				Queue<object> queue = new Queue<object>();
				foreach (object item in from weakref in MemoryTracker.tracked.SelectMany((KeyValuePair<Type, HashSet<WeakReference>> kvp) => kvp.Value)
				where weakref.IsAlive
				select weakref.Target)
				{
					if (!hashSet.Contains(item))
					{
						hashSet.Add(item);
						queue.Enqueue(item);
					}
				}
				foreach (Type type in GenTypes.AllTypes.Union(MemoryTracker.tracked.Keys))
				{
					if (!type.FullName.Contains("MemoryTracker") && !type.FullName.Contains("CollectionsTracker"))
					{
						if (!type.ContainsGenericParameters)
						{
							MemoryTracker.AccumulateStaticMembers(type, dictionary, hashSet, queue);
						}
					}
				}
				int num = 0;
				while (queue.Count > 0)
				{
					if (num % 10000 == 0)
					{
						UnityEngine.Debug.LogFormat("{0} / {1} (to process: {2})", new object[]
						{
							num,
							num + queue.Count,
							queue.Count
						});
					}
					num++;
					MemoryTracker.AccumulateReferences(queue.Dequeue(), dictionary, hashSet, queue);
				}
				if (elements != null && weight != null)
				{
					int num2 = 0;
					MemoryTracker.CalculateReferencePaths(dictionary, from kvp in dictionary
					where !kvp.Value.path.NullOrEmpty()
					select kvp.Key, num2);
					num2 += 1000;
					MemoryTracker.CalculateReferencePaths(dictionary, from kvp in dictionary
					where kvp.Value.path.NullOrEmpty() && kvp.Value.referredBy.Count == 0
					select kvp.Key, num2);
					foreach (object obj in from kvp in dictionary
					where kvp.Value.path.NullOrEmpty()
					select kvp.Key)
					{
						num2 += 1000;
						MemoryTracker.CalculateReferencePaths(dictionary, new object[]
						{
							obj
						}, num2);
					}
					Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
					foreach (WeakReference weakReference in elements)
					{
						object target = weakReference.Target;
						if (target != null)
						{
							string path = dictionary[target].path;
							if (!dictionary2.ContainsKey(path))
							{
								dictionary2[path] = 0;
							}
							Dictionary<string, int> dictionary3;
							string key;
							(dictionary3 = dictionary2)[key = path] = dictionary3[key] + weight(weakReference);
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					foreach (KeyValuePair<string, int> keyValuePair in from kvp in dictionary2
					orderby -kvp.Value
					select kvp)
					{
						stringBuilder.AppendLine(string.Format("{0}: {1}", keyValuePair.Value, keyValuePair.Key));
					}
					Log.Message(stringBuilder.ToString(), false);
				}
			}
			finally
			{
				MemoryTracker.UnlockTracking();
			}
		}

		private static void AccumulateReferences(object obj, Dictionary<object, MemoryTracker.ReferenceData> references, HashSet<object> seen, Queue<object> toProcess)
		{
			MemoryTracker.ReferenceData referenceData = null;
			if (!references.TryGetValue(obj, out referenceData))
			{
				referenceData = new MemoryTracker.ReferenceData();
				references[obj] = referenceData;
			}
			foreach (MemoryTracker.ChildReference childReference in MemoryTracker.GetAllReferencedClassesFromClassOrStruct(obj, MemoryTracker.GetFieldsFromHierarchy(obj.GetType(), BindingFlags.Instance), obj, string.Empty))
			{
				if (!childReference.child.GetType().IsClass)
				{
					throw new ApplicationException();
				}
				MemoryTracker.ReferenceData referenceData2 = null;
				if (!references.TryGetValue(childReference.child, out referenceData2))
				{
					referenceData2 = new MemoryTracker.ReferenceData();
					references[childReference.child] = referenceData2;
				}
				referenceData2.referredBy.Add(new MemoryTracker.ReferenceData.Link
				{
					target = obj,
					targetRef = referenceData,
					path = childReference.path
				});
				referenceData.refers.Add(new MemoryTracker.ReferenceData.Link
				{
					target = childReference.child,
					targetRef = referenceData2,
					path = childReference.path
				});
				if (!seen.Contains(childReference.child))
				{
					seen.Add(childReference.child);
					toProcess.Enqueue(childReference.child);
				}
			}
		}

		private static void AccumulateStaticMembers(Type type, Dictionary<object, MemoryTracker.ReferenceData> references, HashSet<object> seen, Queue<object> toProcess)
		{
			foreach (MemoryTracker.ChildReference childReference in MemoryTracker.GetAllReferencedClassesFromClassOrStruct(null, MemoryTracker.GetFields(type, BindingFlags.Static), null, type.ToString() + "."))
			{
				if (!childReference.child.GetType().IsClass)
				{
					throw new ApplicationException();
				}
				MemoryTracker.ReferenceData referenceData = null;
				if (!references.TryGetValue(childReference.child, out referenceData))
				{
					referenceData = new MemoryTracker.ReferenceData();
					referenceData.path = childReference.path;
					referenceData.pathCost = 0;
					references[childReference.child] = referenceData;
				}
				if (!seen.Contains(childReference.child))
				{
					seen.Add(childReference.child);
					toProcess.Enqueue(childReference.child);
				}
			}
		}

		private static IEnumerable<MemoryTracker.ChildReference> GetAllReferencedClassesFromClassOrStruct(object current, IEnumerable<FieldInfo> fields, object parent, string currentPath)
		{
			foreach (FieldInfo field in fields)
			{
				if (!field.FieldType.IsPrimitive)
				{
					object referenced = null;
					referenced = field.GetValue(current);
					if (referenced != null)
					{
						foreach (MemoryTracker.ChildReference child in MemoryTracker.DistillChildReferencesFromObject(referenced, parent, currentPath + field.Name))
						{
							yield return child;
						}
					}
				}
			}
			if (current != null && current is ICollection)
			{
				MemoryTracker.foundCollections.Add(new WeakReference(current));
				IEnumerator enumerator3 = (current as IEnumerable).GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object entry = enumerator3.Current;
						if (entry != null && !entry.GetType().IsPrimitive)
						{
							foreach (MemoryTracker.ChildReference child2 in MemoryTracker.DistillChildReferencesFromObject(entry, parent, currentPath + "[]"))
							{
								yield return child2;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator3 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			yield break;
		}

		private static IEnumerable<MemoryTracker.ChildReference> DistillChildReferencesFromObject(object current, object parent, string currentPath)
		{
			Type type = current.GetType();
			if (type.IsClass)
			{
				yield return new MemoryTracker.ChildReference
				{
					child = current,
					path = currentPath
				};
				yield break;
			}
			if (type.IsPrimitive)
			{
				yield break;
			}
			if (type.IsValueType)
			{
				string structPath = currentPath + ".";
				foreach (MemoryTracker.ChildReference childReference in MemoryTracker.GetAllReferencedClassesFromClassOrStruct(current, MemoryTracker.GetFieldsFromHierarchy(type, BindingFlags.Instance), parent, structPath))
				{
					yield return childReference;
				}
				yield break;
			}
			throw new NotImplementedException();
			yield break;
		}

		private static IEnumerable<FieldInfo> GetFieldsFromHierarchy(Type type, BindingFlags bindingFlags)
		{
			while (type != null)
			{
				foreach (FieldInfo field in MemoryTracker.GetFields(type, bindingFlags))
				{
					yield return field;
				}
				type = type.BaseType;
			}
			yield break;
		}

		private static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindingFlags)
		{
			foreach (FieldInfo field in type.GetFields(bindingFlags | BindingFlags.Public | BindingFlags.NonPublic))
			{
				yield return field;
			}
			yield break;
		}

		private static void CalculateReferencePaths(Dictionary<object, MemoryTracker.ReferenceData> references, IEnumerable<object> objects, int pathCost)
		{
			Queue<object> queue = new Queue<object>(objects);
			while (queue.Count > 0)
			{
				object obj = queue.Dequeue();
				if (references[obj].path.NullOrEmpty())
				{
					references[obj].path = string.Format("???.{0}", obj.GetType());
					references[obj].pathCost = pathCost;
				}
				MemoryTracker.CalculateObjectReferencePath(obj, references, queue);
			}
		}

		private static void CalculateObjectReferencePath(object obj, Dictionary<object, MemoryTracker.ReferenceData> references, Queue<object> queue)
		{
			MemoryTracker.ReferenceData referenceData = references[obj];
			foreach (MemoryTracker.ReferenceData.Link link in referenceData.refers)
			{
				MemoryTracker.ReferenceData referenceData2 = references[link.target];
				string text = referenceData.path + "." + link.path;
				int num = referenceData.pathCost + 1;
				if (referenceData2.path.NullOrEmpty())
				{
					queue.Enqueue(link.target);
					referenceData2.path = text;
					referenceData2.pathCost = num;
				}
				else if (referenceData2.pathCost == num && referenceData2.path.CompareTo(text) < 0)
				{
					referenceData2.path = text;
				}
				else if (referenceData2.pathCost > num)
				{
					throw new ApplicationException();
				}
			}
		}

		public static void Update()
		{
			if (MemoryTracker.tracked.Count == 0)
			{
				return;
			}
			if (MemoryTracker.updatesSinceLastCull++ >= 10)
			{
				MemoryTracker.updatesSinceLastCull = 0;
				HashSet<WeakReference> value = MemoryTracker.tracked.ElementAtOrDefault(MemoryTracker.cullTargetIndex++).Value;
				if (value == null)
				{
					MemoryTracker.cullTargetIndex = 0;
				}
				else
				{
					MemoryTracker.CullNulls(value);
				}
			}
		}

		private static void CullNulls(HashSet<WeakReference> table)
		{
			table.RemoveWhere((WeakReference element) => !element.IsAlive);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static MemoryTracker()
		{
		}

		[CompilerGenerated]
		private static int <ObjectsLoaded>m__0(KeyValuePair<Type, HashSet<WeakReference>> kvp)
		{
			return -kvp.Value.Count;
		}

		[CompilerGenerated]
		private static int <ObjectHoldPaths>m__1(KeyValuePair<Type, HashSet<WeakReference>> kvp)
		{
			return -kvp.Value.Count;
		}

		[CompilerGenerated]
		private static Type <ObjectHoldPaths>m__2(KeyValuePair<Type, HashSet<WeakReference>> kvp)
		{
			return kvp.Key;
		}

		[CompilerGenerated]
		private static IEnumerable<WeakReference> <LogObjectHoldPathsFor>m__3(KeyValuePair<Type, HashSet<WeakReference>> kvp)
		{
			return kvp.Value;
		}

		[CompilerGenerated]
		private static bool <LogObjectHoldPathsFor>m__4(WeakReference weakref)
		{
			return weakref.IsAlive;
		}

		[CompilerGenerated]
		private static object <LogObjectHoldPathsFor>m__5(WeakReference weakref)
		{
			return weakref.Target;
		}

		[CompilerGenerated]
		private static bool <LogObjectHoldPathsFor>m__6(KeyValuePair<object, MemoryTracker.ReferenceData> kvp)
		{
			return !kvp.Value.path.NullOrEmpty();
		}

		[CompilerGenerated]
		private static object <LogObjectHoldPathsFor>m__7(KeyValuePair<object, MemoryTracker.ReferenceData> kvp)
		{
			return kvp.Key;
		}

		[CompilerGenerated]
		private static bool <LogObjectHoldPathsFor>m__8(KeyValuePair<object, MemoryTracker.ReferenceData> kvp)
		{
			return kvp.Value.path.NullOrEmpty() && kvp.Value.referredBy.Count == 0;
		}

		[CompilerGenerated]
		private static object <LogObjectHoldPathsFor>m__9(KeyValuePair<object, MemoryTracker.ReferenceData> kvp)
		{
			return kvp.Key;
		}

		[CompilerGenerated]
		private static bool <LogObjectHoldPathsFor>m__A(KeyValuePair<object, MemoryTracker.ReferenceData> kvp)
		{
			return kvp.Value.path.NullOrEmpty();
		}

		[CompilerGenerated]
		private static object <LogObjectHoldPathsFor>m__B(KeyValuePair<object, MemoryTracker.ReferenceData> kvp)
		{
			return kvp.Key;
		}

		[CompilerGenerated]
		private static int <LogObjectHoldPathsFor>m__C(KeyValuePair<string, int> kvp)
		{
			return -kvp.Value;
		}

		[CompilerGenerated]
		private static bool <CullNulls>m__D(WeakReference element)
		{
			return !element.IsAlive;
		}

		private class ReferenceData
		{
			public List<MemoryTracker.ReferenceData.Link> refers = new List<MemoryTracker.ReferenceData.Link>();

			public List<MemoryTracker.ReferenceData.Link> referredBy = new List<MemoryTracker.ReferenceData.Link>();

			public string path;

			public int pathCost;

			public ReferenceData()
			{
			}

			public struct Link
			{
				public object target;

				public MemoryTracker.ReferenceData targetRef;

				public string path;
			}
		}

		private struct ChildReference
		{
			public object child;

			public string path;
		}

		public class MarkupComplete : Attribute
		{
			public MarkupComplete()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <ObjectHoldPaths>c__AnonStorey4
		{
			internal HashSet<WeakReference> trackedBatch;

			private static Func<WeakReference, int> <>f__am$cache0;

			public <ObjectHoldPaths>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				MemoryTracker.LogObjectHoldPathsFor(this.trackedBatch, (WeakReference _) => 1);
			}

			private static int <>m__1(WeakReference _)
			{
				return 1;
			}
		}

		[CompilerGenerated]
		private sealed class <GetAllReferencedClassesFromClassOrStruct>c__Iterator0 : IEnumerable, IEnumerable<MemoryTracker.ChildReference>, IEnumerator, IDisposable, IEnumerator<MemoryTracker.ChildReference>
		{
			internal IEnumerable<FieldInfo> fields;

			internal IEnumerator<FieldInfo> $locvar0;

			internal FieldInfo <field>__1;

			internal object <referenced>__2;

			internal object current;

			internal object parent;

			internal string currentPath;

			internal IEnumerator<MemoryTracker.ChildReference> $locvar1;

			internal MemoryTracker.ChildReference <child>__3;

			internal IEnumerator $locvar2;

			internal object <entry>__4;

			internal IDisposable $locvar3;

			internal IEnumerator<MemoryTracker.ChildReference> $locvar4;

			internal MemoryTracker.ChildReference <child>__5;

			internal MemoryTracker.ChildReference $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetAllReferencedClassesFromClassOrStruct>c__Iterator0()
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
					enumerator = fields.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_1C2;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						Block_9:
						try
						{
							switch (num)
							{
							}
							if (enumerator2.MoveNext())
							{
								child = enumerator2.Current;
								this.$current = child;
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
								if (enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
						}
						break;
					}
					while (enumerator.MoveNext())
					{
						field = enumerator.Current;
						if (!field.FieldType.IsPrimitive)
						{
							referenced = null;
							referenced = field.GetValue(current);
							if (referenced != null)
							{
								enumerator2 = MemoryTracker.DistillChildReferencesFromObject(referenced, parent, currentPath + field.Name).GetEnumerator();
								num = 4294967293u;
								goto Block_9;
							}
						}
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
				if (current == null || !(current is ICollection))
				{
					goto IL_2EA;
				}
				MemoryTracker.foundCollections.Add(new WeakReference(current));
				enumerator3 = (current as IEnumerable).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_1C2:
					switch (num)
					{
					case 2u:
						Block_22:
						try
						{
							switch (num)
							{
							}
							if (enumerator4.MoveNext())
							{
								child2 = enumerator4.Current;
								this.$current = child2;
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
								if (enumerator4 != null)
								{
									enumerator4.Dispose();
								}
							}
						}
						break;
					}
					while (enumerator3.MoveNext())
					{
						entry = enumerator3.Current;
						if (entry != null && !entry.GetType().IsPrimitive)
						{
							enumerator4 = MemoryTracker.DistillChildReferencesFromObject(entry, parent, currentPath + "[]").GetEnumerator();
							num = 4294967293u;
							goto Block_22;
						}
					}
				}
				finally
				{
					if (!flag)
					{
						if ((disposable = (enumerator3 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				IL_2EA:
				this.$PC = -1;
				return false;
			}

			MemoryTracker.ChildReference IEnumerator<MemoryTracker.ChildReference>.Current
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
						try
						{
						}
						finally
						{
							if (enumerator4 != null)
							{
								enumerator4.Dispose();
							}
						}
					}
					finally
					{
						if ((disposable = (enumerator3 as IDisposable)) != null)
						{
							disposable.Dispose();
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
				return this.System.Collections.Generic.IEnumerable<Verse.Profile.MemoryTracker.ChildReference>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<MemoryTracker.ChildReference> IEnumerable<MemoryTracker.ChildReference>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				MemoryTracker.<GetAllReferencedClassesFromClassOrStruct>c__Iterator0 <GetAllReferencedClassesFromClassOrStruct>c__Iterator = new MemoryTracker.<GetAllReferencedClassesFromClassOrStruct>c__Iterator0();
				<GetAllReferencedClassesFromClassOrStruct>c__Iterator.fields = fields;
				<GetAllReferencedClassesFromClassOrStruct>c__Iterator.current = current;
				<GetAllReferencedClassesFromClassOrStruct>c__Iterator.parent = parent;
				<GetAllReferencedClassesFromClassOrStruct>c__Iterator.currentPath = currentPath;
				return <GetAllReferencedClassesFromClassOrStruct>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <DistillChildReferencesFromObject>c__Iterator1 : IEnumerable, IEnumerable<MemoryTracker.ChildReference>, IEnumerator, IDisposable, IEnumerator<MemoryTracker.ChildReference>
		{
			internal object current;

			internal Type <type>__0;

			internal string currentPath;

			internal string <structPath>__1;

			internal object parent;

			internal IEnumerator<MemoryTracker.ChildReference> $locvar0;

			internal MemoryTracker.ChildReference <childReference>__2;

			internal MemoryTracker.ChildReference $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <DistillChildReferencesFromObject>c__Iterator1()
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
					type = current.GetType();
					if (type.IsClass)
					{
						this.$current = new MemoryTracker.ChildReference
						{
							child = current,
							path = currentPath
						};
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					if (type.IsPrimitive)
					{
						return false;
					}
					if (!type.IsValueType)
					{
						throw new NotImplementedException();
					}
					structPath = currentPath + ".";
					enumerator = MemoryTracker.GetAllReferencedClassesFromClassOrStruct(current, MemoryTracker.GetFieldsFromHierarchy(type, BindingFlags.Instance), parent, structPath).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					return false;
				case 2u:
					break;
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
						childReference = enumerator.Current;
						this.$current = childReference;
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
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				return false;
			}

			MemoryTracker.ChildReference IEnumerator<MemoryTracker.ChildReference>.Current
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
				case 2u:
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
				return this.System.Collections.Generic.IEnumerable<Verse.Profile.MemoryTracker.ChildReference>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<MemoryTracker.ChildReference> IEnumerable<MemoryTracker.ChildReference>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				MemoryTracker.<DistillChildReferencesFromObject>c__Iterator1 <DistillChildReferencesFromObject>c__Iterator = new MemoryTracker.<DistillChildReferencesFromObject>c__Iterator1();
				<DistillChildReferencesFromObject>c__Iterator.current = current;
				<DistillChildReferencesFromObject>c__Iterator.currentPath = currentPath;
				<DistillChildReferencesFromObject>c__Iterator.parent = parent;
				return <DistillChildReferencesFromObject>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetFieldsFromHierarchy>c__Iterator2 : IEnumerable, IEnumerable<FieldInfo>, IEnumerator, IDisposable, IEnumerator<FieldInfo>
		{
			internal Type type;

			internal BindingFlags bindingFlags;

			internal IEnumerator<FieldInfo> $locvar0;

			internal FieldInfo <field>__1;

			internal FieldInfo $current;

			internal bool $disposing;

			internal Type <$>type;

			internal int $PC;

			[DebuggerHidden]
			public <GetFieldsFromHierarchy>c__Iterator2()
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
					break;
				case 1u:
					Block_2:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							field = enumerator.Current;
							this.$current = field;
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
					type = type.BaseType;
					break;
				default:
					return false;
				}
				if (type != null)
				{
					enumerator = MemoryTracker.GetFields(type, bindingFlags).GetEnumerator();
					num = 4294967293u;
					goto Block_2;
				}
				this.$PC = -1;
				return false;
			}

			FieldInfo IEnumerator<FieldInfo>.Current
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
				return this.System.Collections.Generic.IEnumerable<System.Reflection.FieldInfo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FieldInfo> IEnumerable<FieldInfo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				MemoryTracker.<GetFieldsFromHierarchy>c__Iterator2 <GetFieldsFromHierarchy>c__Iterator = new MemoryTracker.<GetFieldsFromHierarchy>c__Iterator2();
				<GetFieldsFromHierarchy>c__Iterator.type = type;
				<GetFieldsFromHierarchy>c__Iterator.bindingFlags = bindingFlags;
				return <GetFieldsFromHierarchy>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetFields>c__Iterator3 : IEnumerable, IEnumerable<FieldInfo>, IEnumerator, IDisposable, IEnumerator<FieldInfo>
		{
			internal Type type;

			internal BindingFlags bindingFlags;

			internal FieldInfo[] $locvar0;

			internal int $locvar1;

			internal FieldInfo <field>__1;

			internal FieldInfo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetFields>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					fields = type.GetFields(bindingFlags | BindingFlags.Public | BindingFlags.NonPublic);
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < fields.Length)
				{
					field = fields[i];
					this.$current = field;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			FieldInfo IEnumerator<FieldInfo>.Current
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
				return this.System.Collections.Generic.IEnumerable<System.Reflection.FieldInfo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FieldInfo> IEnumerable<FieldInfo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				MemoryTracker.<GetFields>c__Iterator3 <GetFields>c__Iterator = new MemoryTracker.<GetFields>c__Iterator3();
				<GetFields>c__Iterator.type = type;
				<GetFields>c__Iterator.bindingFlags = bindingFlags;
				return <GetFields>c__Iterator;
			}
		}
	}
}
