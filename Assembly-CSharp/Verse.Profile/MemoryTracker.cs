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

		private const int updatesPerCull = 10;

		private static Dictionary<Type, HashSet<WeakReference>> tracked = new Dictionary<Type, HashSet<WeakReference>>();

		private static bool trackedLocked = false;

		private static List<object> trackedQueue = new List<object>();

		private static List<RuntimeTypeHandle> trackedTypeQueue = new List<RuntimeTypeHandle>();

		private static int updatesSinceLastCull = 0;

		private static int cullTargetIndex = 0;

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
			List<object>.Enumerator enumerator = MemoryTracker.trackedQueue.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					MemoryTracker.RegisterObject(current);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			MemoryTracker.trackedQueue.Clear();
			List<RuntimeTypeHandle>.Enumerator enumerator2 = MemoryTracker.trackedTypeQueue.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					RuntimeTypeHandle current2 = enumerator2.Current;
					MemoryTracker.RegisterType(current2);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
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
					Dictionary<Type, HashSet<WeakReference>>.ValueCollection.Enumerator enumerator = MemoryTracker.tracked.Values.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							HashSet<WeakReference> current = enumerator.Current;
							MemoryTracker.CullNulls(current);
						}
					}
					finally
					{
						((IDisposable)(object)enumerator).Dispose();
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
					Dictionary<Type, HashSet<WeakReference>>.ValueCollection.Enumerator enumerator = MemoryTracker.tracked.Values.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							HashSet<WeakReference> current = enumerator.Current;
							MemoryTracker.CullNulls(current);
						}
					}
					finally
					{
						((IDisposable)(object)enumerator).Dispose();
					}
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (KeyValuePair<Type, HashSet<WeakReference>> item in from kvp in MemoryTracker.tracked
					orderby -kvp.Value.Count
					select kvp)
					{
						KeyValuePair<Type, HashSet<WeakReference>> elementLocal = item;
						list.Add(new FloatMenuOption(string.Format("{0} ({1})", item.Key, item.Value.Count), (Action)delegate
						{
							MemoryTracker.LogObjectHoldPathsFor(elementLocal.Key, elementLocal.Value);
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

		public static void LogObjectHoldPathsFor(Type type, HashSet<WeakReference> elements)
		{
			GC.Collect();
			MemoryTracker.LockTracking();
			try
			{
				Dictionary<object, ReferenceData> dictionary = new Dictionary<object, ReferenceData>();
				HashSet<object> hashSet = new HashSet<object>();
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
					if (!item2.FullName.Contains("MemoryTracker") && !item2.ContainsGenericParameters)
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
				HashSet<WeakReference>.Enumerator enumerator4 = elements.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						WeakReference current4 = enumerator4.Current;
						if (current4.IsAlive)
						{
							string path = dictionary[current4.Target].path;
							if (dictionary2.ContainsKey(path))
							{
								Dictionary<string, int> dictionary3;
								Dictionary<string, int> obj = dictionary3 = dictionary2;
								string key;
								string key2 = key = path;
								int num3 = dictionary3[key];
								obj[key2] = num3 + 1;
							}
							else
							{
								dictionary2[path] = 1;
							}
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator4).Dispose();
				}
				StringBuilder stringBuilder = new StringBuilder();
				foreach (KeyValuePair<string, int> item4 in from kvp in dictionary2
				orderby -kvp.Value
				select kvp)
				{
					stringBuilder.AppendLine(string.Format("{0}: {1}", item4.Value, item4.Key));
				}
				Log.Message(stringBuilder.ToString());
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
			using (IEnumerator<ChildReference> enumerator = MemoryTracker.GetAllReferencedClassesFromClassOrStruct(obj, MemoryTracker.GetFieldsFromHierarchy(obj.GetType(), BindingFlags.Instance), obj, string.Empty).GetEnumerator())
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
						foreach (ChildReference item2 in MemoryTracker.DistillChildReferencesFromObject(referenced, parent, currentPath + item.Name))
						{
							yield return item2;
						}
					}
				}
			}
			if (current != null && current is ICollection)
			{
				foreach (object item3 in current as IEnumerable)
				{
					if (item3 != null && !item3.GetType().IsPrimitive)
					{
						foreach (ChildReference item4 in MemoryTracker.DistillChildReferencesFromObject(item3, parent, currentPath + "[]"))
						{
							yield return item4;
						}
					}
				}
			}
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
			}
			else if (!type.IsPrimitive)
			{
				if (!type.IsValueType)
				{
					throw new NotImplementedException();
				}
				string structPath = currentPath + ".";
				foreach (ChildReference item in MemoryTracker.GetAllReferencedClassesFromClassOrStruct(current, MemoryTracker.GetFieldsFromHierarchy(type, BindingFlags.Instance), parent, structPath))
				{
					yield return item;
				}
			}
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
		}

		private static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindingFlags)
		{
			FieldInfo[] fields = type.GetFields((BindingFlags)((int)bindingFlags | 16 | 32));
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo field = fields[i];
				yield return field;
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
			List<ReferenceData.Link>.Enumerator enumerator = referenceData.refers.GetEnumerator();
			try
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
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
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
