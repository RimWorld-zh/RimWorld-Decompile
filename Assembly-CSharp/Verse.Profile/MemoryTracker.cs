using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Verse.Profile
{
	public static class MemoryTracker
	{
		private class ReferenceData
		{
			public struct Link
			{
				public object target;

				public ReferenceData targetRef;

				public string path;
			}

			public List<Link> refers = new List<Link>();

			public List<Link> referredBy = new List<Link>();

			public string path;

			public int pathCost;
		}

		private struct ChildReference
		{
			public object child;

			public string path;
		}

		public class MarkupComplete : Attribute
		{
		}

		private static Dictionary<Type, HashSet<WeakReference>> tracked = new Dictionary<Type, HashSet<WeakReference>>();

		private static List<WeakReference> foundCollections = new List<WeakReference>();

		private static bool trackedLocked = false;

		private static List<object> trackedQueue = new List<object>();

		private static List<RuntimeTypeHandle> trackedTypeQueue = new List<RuntimeTypeHandle>();

		private const int updatesPerCull = 10;

		private static int updatesSinceLastCull = 0;

		private static int cullTargetIndex = 0;

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
			}
			else
			{
				Type type = obj.GetType();
				HashSet<WeakReference> hashSet = null;
				if (!MemoryTracker.tracked.TryGetValue(type, out hashSet))
				{
					hashSet = new HashSet<WeakReference>();
					MemoryTracker.tracked[type] = hashSet;
				}
				hashSet.Add(new WeakReference(obj));
			}
		}

		public static void RegisterType(RuntimeTypeHandle typeHandle)
		{
			if (MemoryTracker.trackedLocked)
			{
				MemoryTracker.trackedTypeQueue.Add(typeHandle);
			}
			else
			{
				Type typeFromHandle = Type.GetTypeFromHandle(typeHandle);
				if (!MemoryTracker.tracked.ContainsKey(typeFromHandle))
				{
					MemoryTracker.tracked[typeFromHandle] = new HashSet<WeakReference>();
				}
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
			foreach (object item in MemoryTracker.trackedQueue)
			{
				MemoryTracker.RegisterObject(item);
			}
			MemoryTracker.trackedQueue.Clear();
			foreach (RuntimeTypeHandle item2 in MemoryTracker.trackedTypeQueue)
			{
				MemoryTracker.RegisterType(item2);
			}
			MemoryTracker.trackedTypeQueue.Clear();
		}

		public static void LogObjectsLoaded()
		{
			if (MemoryTracker.tracked.Count == 0)
			{
				Log.Message("No objects tracked, memory tracker markup may not be applied.");
			}
			else
			{
				GC.Collect();
				MemoryTracker.LockTracking();
				try
				{
					foreach (HashSet<WeakReference> value in MemoryTracker.tracked.Values)
					{
						MemoryTracker.CullNulls(value);
					}
					StringBuilder stringBuilder = new StringBuilder();
					foreach (KeyValuePair<Type, HashSet<WeakReference>> item in from kvp in MemoryTracker.tracked
					orderby -kvp.Value.Count
					select kvp)
					{
						stringBuilder.AppendLine(string.Format("{0,6} {1}", item.Value.Count, item.Key));
					}
					Log.Message(stringBuilder.ToString());
				}
				finally
				{
					MemoryTracker.UnlockTracking();
				}
			}
		}

		public static void LogObjectHoldPaths()
		{
			if (MemoryTracker.tracked.Count == 0)
			{
				Log.Message("No objects tracked, memory tracker markup may not be applied.");
			}
			else
			{
				GC.Collect();
				MemoryTracker.LockTracking();
				try
				{
					foreach (HashSet<WeakReference> value in MemoryTracker.tracked.Values)
					{
						MemoryTracker.CullNulls(value);
					}
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (KeyValuePair<Type, HashSet<WeakReference>> item in from kvp in MemoryTracker.tracked
					orderby -kvp.Value.Count
					select kvp)
					{
						KeyValuePair<Type, HashSet<WeakReference>> elementLocal = item;
						list.Add(new FloatMenuOption(string.Format("{0} ({1})", item.Key, item.Value.Count), (Action)delegate
						{
							MemoryTracker.LogObjectHoldPathsFor(elementLocal.Value, (Func<WeakReference, int>)((WeakReference _) => 1));
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
						if (list.Count == 30)
							break;
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				finally
				{
					MemoryTracker.UnlockTracking();
				}
			}
		}

		public static void LogObjectHoldPathsFor(IEnumerable<WeakReference> elements, Func<WeakReference, int> weight)
		{
			GC.Collect();
			MemoryTracker.LockTracking();
			try
			{
				Dictionary<object, ReferenceData> dictionary = new Dictionary<object, ReferenceData>();
				HashSet<object> hashSet = new HashSet<object>();
				MemoryTracker.foundCollections.Clear();
				Queue<object> queue = new Queue<object>();
				foreach (object item in from weakref in MemoryTracker.tracked.SelectMany((Func<KeyValuePair<Type, HashSet<WeakReference>>, IEnumerable<WeakReference>>)((KeyValuePair<Type, HashSet<WeakReference>> kvp) => kvp.Value))
				where weakref.IsAlive
				select weakref.Target)
				{
					if (!hashSet.Contains(item))
					{
						hashSet.Add(item);
						queue.Enqueue(item);
					}
				}
				foreach (Type item2 in GenTypes.AllTypes.Union(MemoryTracker.tracked.Keys))
				{
					if (!item2.FullName.Contains("MemoryTracker") && !item2.FullName.Contains("CollectionsTracker") && !item2.ContainsGenericParameters)
					{
						MemoryTracker.AccumulateStaticMembers(item2, dictionary, hashSet, queue);
					}
				}
				int num = 0;
				while (queue.Count > 0)
				{
					if (num % 10000 == 0)
					{
						Debug.LogFormat("{0} / {1} (to process: {2})", num, num + queue.Count, queue.Count);
					}
					num++;
					MemoryTracker.AccumulateReferences(queue.Dequeue(), dictionary, hashSet, queue);
				}
				if (elements != null && (object)weight != null)
				{
					int num2 = 0;
					MemoryTracker.CalculateReferencePaths(dictionary, from kvp in dictionary
					where !kvp.Value.path.NullOrEmpty()
					select kvp.Key, num2);
					num2 += 1000;
					MemoryTracker.CalculateReferencePaths(dictionary, from kvp in dictionary
					where kvp.Value.path.NullOrEmpty() && kvp.Value.referredBy.Count == 0
					select kvp.Key, num2);
					foreach (object item3 in from kvp in dictionary
					where kvp.Value.path.NullOrEmpty()
					select kvp.Key)
					{
						num2 += 1000;
						MemoryTracker.CalculateReferencePaths(dictionary, new object[1]
						{
							item3
						}, num2);
					}
					Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
					foreach (WeakReference item4 in elements)
					{
						if (item4.IsAlive)
						{
							string path = dictionary[item4.Target].path;
							if (!dictionary2.ContainsKey(path))
							{
								dictionary2[path] = 0;
							}
							Dictionary<string, int> dictionary3;
							string key;
							(dictionary3 = dictionary2)[key = path] = dictionary3[key] + weight(item4);
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					foreach (KeyValuePair<string, int> item5 in from kvp in dictionary2
					orderby -kvp.Value
					select kvp)
					{
						stringBuilder.AppendLine(string.Format("{0}: {1}", item5.Value, item5.Key));
					}
					Log.Message(stringBuilder.ToString());
				}
			}
			finally
			{
				MemoryTracker.UnlockTracking();
			}
		}

		private static void AccumulateReferences(object obj, Dictionary<object, ReferenceData> references, HashSet<object> seen, Queue<object> toProcess)
		{
			ReferenceData referenceData = null;
			if (!references.TryGetValue(obj, out referenceData))
			{
				referenceData = new ReferenceData();
				references[obj] = referenceData;
			}
			using (IEnumerator<ChildReference> enumerator = MemoryTracker.GetAllReferencedClassesFromClassOrStruct(obj, MemoryTracker.GetFieldsFromHierarchy(obj.GetType(), BindingFlags.Instance), obj, "").GetEnumerator())
			{
				while (true)
				{
					if (enumerator.MoveNext())
					{
						ChildReference current = enumerator.Current;
						if (current.child.GetType().IsClass)
						{
							ReferenceData referenceData2 = null;
							if (!references.TryGetValue(current.child, out referenceData2))
							{
								referenceData2 = new ReferenceData();
								references[current.child] = referenceData2;
							}
							referenceData2.referredBy.Add(new ReferenceData.Link
							{
								target = obj,
								targetRef = referenceData,
								path = current.path
							});
							referenceData.refers.Add(new ReferenceData.Link
							{
								target = current.child,
								targetRef = referenceData2,
								path = current.path
							});
							if (!seen.Contains(current.child))
							{
								seen.Add(current.child);
								toProcess.Enqueue(current.child);
							}
							continue;
						}
						break;
					}
					return;
				}
				throw new ApplicationException();
			}
		}

		private static void AccumulateStaticMembers(Type type, Dictionary<object, ReferenceData> references, HashSet<object> seen, Queue<object> toProcess)
		{
			using (IEnumerator<ChildReference> enumerator = MemoryTracker.GetAllReferencedClassesFromClassOrStruct(null, MemoryTracker.GetFields(type, BindingFlags.Static), null, type.ToString() + ".").GetEnumerator())
			{
				while (true)
				{
					if (enumerator.MoveNext())
					{
						ChildReference current = enumerator.Current;
						if (current.child.GetType().IsClass)
						{
							ReferenceData referenceData = null;
							if (!references.TryGetValue(current.child, out referenceData))
							{
								referenceData = new ReferenceData();
								referenceData.path = current.path;
								referenceData.pathCost = 0;
								references[current.child] = referenceData;
							}
							if (!seen.Contains(current.child))
							{
								seen.Add(current.child);
								toProcess.Enqueue(current.child);
							}
							continue;
						}
						break;
					}
					return;
				}
				throw new ApplicationException();
			}
		}

		private static IEnumerable<ChildReference> GetAllReferencedClassesFromClassOrStruct(object current, IEnumerable<FieldInfo> fields, object parent, string currentPath)
		{
			foreach (FieldInfo item in fields)
			{
				if (!item.FieldType.IsPrimitive)
				{
					object referenced = item.GetValue(current);
					if (referenced != null)
					{
						using (IEnumerator<ChildReference> enumerator2 = MemoryTracker.DistillChildReferencesFromObject(referenced, parent, currentPath + item.Name).GetEnumerator())
						{
							if (enumerator2.MoveNext())
							{
								ChildReference child2 = enumerator2.Current;
								yield return child2;
								/*Error: Unable to find new state assignment for yield return*/;
							}
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
							using (IEnumerator<ChildReference> enumerator4 = MemoryTracker.DistillChildReferencesFromObject(entry, parent, currentPath + "[]").GetEnumerator())
							{
								if (enumerator4.MoveNext())
								{
									ChildReference child = enumerator4.Current;
									yield return child;
									/*Error: Unable to find new state assignment for yield return*/;
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					IDisposable disposable2 = disposable = (enumerator3 as IDisposable);
					if (disposable != null)
					{
						disposable2.Dispose();
					}
				}
			}
			yield break;
			IL_0306:
			/*Error near IL_0307: Unexpected return in MoveNext()*/;
		}

		private static IEnumerable<ChildReference> DistillChildReferencesFromObject(object current, object parent, string currentPath)
		{
			Type type = current.GetType();
			if (type.IsClass)
			{
				yield return new ChildReference
				{
					child = current,
					path = currentPath
				};
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!type.IsPrimitive)
			{
				if (!type.IsValueType)
				{
					throw new NotImplementedException();
				}
				string structPath = currentPath + ".";
				using (IEnumerator<ChildReference> enumerator = MemoryTracker.GetAllReferencedClassesFromClassOrStruct(current, MemoryTracker.GetFieldsFromHierarchy(type, BindingFlags.Instance), parent, structPath).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ChildReference childReference = enumerator.Current;
						yield return childReference;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_0185:
			/*Error near IL_0186: Unexpected return in MoveNext()*/;
		}

		private static IEnumerable<FieldInfo> GetFieldsFromHierarchy(Type type, BindingFlags bindingFlags)
		{
			while (type != null)
			{
				using (IEnumerator<FieldInfo> enumerator = MemoryTracker.GetFields(type, bindingFlags).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						FieldInfo field = enumerator.Current;
						yield return field;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				type = type.BaseType;
			}
			yield break;
			IL_00e6:
			/*Error near IL_00e7: Unexpected return in MoveNext()*/;
		}

		private static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindingFlags)
		{
			FieldInfo[] fields = type.GetFields((BindingFlags)((int)bindingFlags | 16 | 32));
			int num = 0;
			if (num < fields.Length)
			{
				FieldInfo field = fields[num];
				yield return field;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		private static void CalculateReferencePaths(Dictionary<object, ReferenceData> references, IEnumerable<object> objects, int pathCost)
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

		private static void CalculateObjectReferencePath(object obj, Dictionary<object, ReferenceData> references, Queue<object> queue)
		{
			ReferenceData referenceData = references[obj];
			using (List<ReferenceData.Link>.Enumerator enumerator = referenceData.refers.GetEnumerator())
			{
				while (true)
				{
					if (enumerator.MoveNext())
					{
						ReferenceData.Link current = enumerator.Current;
						ReferenceData referenceData2 = references[current.target];
						string text = referenceData.path + "." + current.path;
						int num = referenceData.pathCost + 1;
						if (referenceData2.path.NullOrEmpty())
						{
							queue.Enqueue(current.target);
							referenceData2.path = text;
							referenceData2.pathCost = num;
						}
						else if (referenceData2.pathCost == num && referenceData2.path.CompareTo(text) < 0)
						{
							referenceData2.path = text;
						}
						else if (referenceData2.pathCost > num)
							break;
						continue;
					}
					return;
				}
				throw new ApplicationException();
			}
		}

		public static void Update()
		{
			if (MemoryTracker.tracked.Count != 0 && MemoryTracker.updatesSinceLastCull++ >= 10)
			{
				MemoryTracker.updatesSinceLastCull = 0;
				KeyValuePair<Type, HashSet<WeakReference>> keyValuePair = MemoryTracker.tracked.ElementAtOrDefault(MemoryTracker.cullTargetIndex++);
				if (keyValuePair.Value == null)
				{
					MemoryTracker.cullTargetIndex = 0;
				}
				else
				{
					MemoryTracker.CullNulls(keyValuePair.Value);
				}
			}
		}

		private static void CullNulls(HashSet<WeakReference> table)
		{
			table.RemoveWhere((Predicate<WeakReference>)((WeakReference element) => !element.IsAlive));
		}
	}
}
