using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Verse.Profile
{
	// Token: 0x02000D66 RID: 3430
	[HasDebugOutput]
	public static class MemoryTracker
	{
		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06004CC3 RID: 19651 RVA: 0x0027F3C4 File Offset: 0x0027D7C4
		public static bool AnythingTracked
		{
			get
			{
				return MemoryTracker.tracked.Count > 0;
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06004CC4 RID: 19652 RVA: 0x0027F3E8 File Offset: 0x0027D7E8
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

		// Token: 0x06004CC5 RID: 19653 RVA: 0x0027F418 File Offset: 0x0027D818
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

		// Token: 0x06004CC6 RID: 19654 RVA: 0x0027F480 File Offset: 0x0027D880
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

		// Token: 0x06004CC7 RID: 19655 RVA: 0x0027F4D2 File Offset: 0x0027D8D2
		private static void LockTracking()
		{
			if (MemoryTracker.trackedLocked)
			{
				throw new NotImplementedException();
			}
			MemoryTracker.trackedLocked = true;
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x0027F4EC File Offset: 0x0027D8EC
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

		// Token: 0x06004CC9 RID: 19657 RVA: 0x0027F5C4 File Offset: 0x0027D9C4
		[DebugOutput]
		[Category("System")]
		private static void ObjectsLoaded()
		{
			if (MemoryTracker.tracked.Count == 0)
			{
				Log.Message("No objects tracked, memory tracker markup may not be applied.", false);
			}
			else
			{
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
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x0027F714 File Offset: 0x0027DB14
		[DebugOutput]
		[Category("System")]
		private static void ObjectHoldPaths()
		{
			if (MemoryTracker.tracked.Count == 0)
			{
				Log.Message("No objects tracked, memory tracker markup may not be applied.", false);
			}
			else
			{
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
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x0027F930 File Offset: 0x0027DD30
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
						Debug.LogFormat("{0} / {1} (to process: {2})", new object[]
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
						if (weakReference.IsAlive)
						{
							string path = dictionary[weakReference.Target].path;
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

		// Token: 0x06004CCC RID: 19660 RVA: 0x0027FE60 File Offset: 0x0027E260
		private static void AccumulateReferences(object obj, Dictionary<object, MemoryTracker.ReferenceData> references, HashSet<object> seen, Queue<object> toProcess)
		{
			MemoryTracker.ReferenceData referenceData = null;
			if (!references.TryGetValue(obj, out referenceData))
			{
				referenceData = new MemoryTracker.ReferenceData();
				references[obj] = referenceData;
			}
			foreach (MemoryTracker.ChildReference childReference in MemoryTracker.GetAllReferencedClassesFromClassOrStruct(obj, MemoryTracker.GetFieldsFromHierarchy(obj.GetType(), BindingFlags.Instance), obj, ""))
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

		// Token: 0x06004CCD RID: 19661 RVA: 0x0027FFD8 File Offset: 0x0027E3D8
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

		// Token: 0x06004CCE RID: 19662 RVA: 0x002800D4 File Offset: 0x0027E4D4
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

		// Token: 0x06004CCF RID: 19663 RVA: 0x00280114 File Offset: 0x0027E514
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

		// Token: 0x06004CD0 RID: 19664 RVA: 0x0028014C File Offset: 0x0027E54C
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

		// Token: 0x06004CD1 RID: 19665 RVA: 0x00280184 File Offset: 0x0027E584
		private static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindingFlags)
		{
			foreach (FieldInfo field in type.GetFields(bindingFlags | BindingFlags.Public | BindingFlags.NonPublic))
			{
				yield return field;
			}
			yield break;
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x002801B8 File Offset: 0x0027E5B8
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

		// Token: 0x06004CD3 RID: 19667 RVA: 0x00280230 File Offset: 0x0027E630
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

		// Token: 0x06004CD4 RID: 19668 RVA: 0x0028033C File Offset: 0x0027E73C
		public static void Update()
		{
			if (MemoryTracker.tracked.Count != 0)
			{
				if (MemoryTracker.updatesSinceLastCull++ >= 10)
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
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x002803BA File Offset: 0x0027E7BA
		private static void CullNulls(HashSet<WeakReference> table)
		{
			table.RemoveWhere((WeakReference element) => !element.IsAlive);
		}

		// Token: 0x04003338 RID: 13112
		private static Dictionary<Type, HashSet<WeakReference>> tracked = new Dictionary<Type, HashSet<WeakReference>>();

		// Token: 0x04003339 RID: 13113
		private static List<WeakReference> foundCollections = new List<WeakReference>();

		// Token: 0x0400333A RID: 13114
		private static bool trackedLocked = false;

		// Token: 0x0400333B RID: 13115
		private static List<object> trackedQueue = new List<object>();

		// Token: 0x0400333C RID: 13116
		private static List<RuntimeTypeHandle> trackedTypeQueue = new List<RuntimeTypeHandle>();

		// Token: 0x0400333D RID: 13117
		private const int updatesPerCull = 10;

		// Token: 0x0400333E RID: 13118
		private static int updatesSinceLastCull = 0;

		// Token: 0x0400333F RID: 13119
		private static int cullTargetIndex = 0;

		// Token: 0x02000D67 RID: 3431
		private class ReferenceData
		{
			// Token: 0x0400334E RID: 13134
			public List<MemoryTracker.ReferenceData.Link> refers = new List<MemoryTracker.ReferenceData.Link>();

			// Token: 0x0400334F RID: 13135
			public List<MemoryTracker.ReferenceData.Link> referredBy = new List<MemoryTracker.ReferenceData.Link>();

			// Token: 0x04003350 RID: 13136
			public string path;

			// Token: 0x04003351 RID: 13137
			public int pathCost;

			// Token: 0x02000D68 RID: 3432
			public struct Link
			{
				// Token: 0x04003352 RID: 13138
				public object target;

				// Token: 0x04003353 RID: 13139
				public MemoryTracker.ReferenceData targetRef;

				// Token: 0x04003354 RID: 13140
				public string path;
			}
		}

		// Token: 0x02000D69 RID: 3433
		private struct ChildReference
		{
			// Token: 0x04003355 RID: 13141
			public object child;

			// Token: 0x04003356 RID: 13142
			public string path;
		}

		// Token: 0x02000D6A RID: 3434
		public class MarkupComplete : Attribute
		{
		}
	}
}
