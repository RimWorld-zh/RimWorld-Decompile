using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Verse.Profile
{
	// Token: 0x02000D64 RID: 3428
	[HasDebugOutput]
	public static class MemoryTracker
	{
		// Token: 0x04003341 RID: 13121
		private static Dictionary<Type, HashSet<WeakReference>> tracked = new Dictionary<Type, HashSet<WeakReference>>();

		// Token: 0x04003342 RID: 13122
		private static List<WeakReference> foundCollections = new List<WeakReference>();

		// Token: 0x04003343 RID: 13123
		private static bool trackedLocked = false;

		// Token: 0x04003344 RID: 13124
		private static List<object> trackedQueue = new List<object>();

		// Token: 0x04003345 RID: 13125
		private static List<RuntimeTypeHandle> trackedTypeQueue = new List<RuntimeTypeHandle>();

		// Token: 0x04003346 RID: 13126
		private const int updatesPerCull = 10;

		// Token: 0x04003347 RID: 13127
		private static int updatesSinceLastCull = 0;

		// Token: 0x04003348 RID: 13128
		private static int cullTargetIndex = 0;

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06004CDA RID: 19674 RVA: 0x00280A80 File Offset: 0x0027EE80
		public static bool AnythingTracked
		{
			get
			{
				return MemoryTracker.tracked.Count > 0;
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06004CDB RID: 19675 RVA: 0x00280AA4 File Offset: 0x0027EEA4
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

		// Token: 0x06004CDC RID: 19676 RVA: 0x00280AD4 File Offset: 0x0027EED4
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

		// Token: 0x06004CDD RID: 19677 RVA: 0x00280B3C File Offset: 0x0027EF3C
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

		// Token: 0x06004CDE RID: 19678 RVA: 0x00280B8E File Offset: 0x0027EF8E
		private static void LockTracking()
		{
			if (MemoryTracker.trackedLocked)
			{
				throw new NotImplementedException();
			}
			MemoryTracker.trackedLocked = true;
		}

		// Token: 0x06004CDF RID: 19679 RVA: 0x00280BA8 File Offset: 0x0027EFA8
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

		// Token: 0x06004CE0 RID: 19680 RVA: 0x00280C80 File Offset: 0x0027F080
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

		// Token: 0x06004CE1 RID: 19681 RVA: 0x00280DD0 File Offset: 0x0027F1D0
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

		// Token: 0x06004CE2 RID: 19682 RVA: 0x00280FEC File Offset: 0x0027F3EC
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

		// Token: 0x06004CE3 RID: 19683 RVA: 0x0028151C File Offset: 0x0027F91C
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

		// Token: 0x06004CE4 RID: 19684 RVA: 0x00281694 File Offset: 0x0027FA94
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

		// Token: 0x06004CE5 RID: 19685 RVA: 0x00281790 File Offset: 0x0027FB90
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

		// Token: 0x06004CE6 RID: 19686 RVA: 0x002817D0 File Offset: 0x0027FBD0
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

		// Token: 0x06004CE7 RID: 19687 RVA: 0x00281808 File Offset: 0x0027FC08
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

		// Token: 0x06004CE8 RID: 19688 RVA: 0x00281840 File Offset: 0x0027FC40
		private static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindingFlags)
		{
			foreach (FieldInfo field in type.GetFields(bindingFlags | BindingFlags.Public | BindingFlags.NonPublic))
			{
				yield return field;
			}
			yield break;
		}

		// Token: 0x06004CE9 RID: 19689 RVA: 0x00281874 File Offset: 0x0027FC74
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

		// Token: 0x06004CEA RID: 19690 RVA: 0x002818EC File Offset: 0x0027FCEC
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

		// Token: 0x06004CEB RID: 19691 RVA: 0x002819F8 File Offset: 0x0027FDF8
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

		// Token: 0x06004CEC RID: 19692 RVA: 0x00281A76 File Offset: 0x0027FE76
		private static void CullNulls(HashSet<WeakReference> table)
		{
			table.RemoveWhere((WeakReference element) => !element.IsAlive);
		}

		// Token: 0x02000D65 RID: 3429
		private class ReferenceData
		{
			// Token: 0x04003357 RID: 13143
			public List<MemoryTracker.ReferenceData.Link> refers = new List<MemoryTracker.ReferenceData.Link>();

			// Token: 0x04003358 RID: 13144
			public List<MemoryTracker.ReferenceData.Link> referredBy = new List<MemoryTracker.ReferenceData.Link>();

			// Token: 0x04003359 RID: 13145
			public string path;

			// Token: 0x0400335A RID: 13146
			public int pathCost;

			// Token: 0x02000D66 RID: 3430
			public struct Link
			{
				// Token: 0x0400335B RID: 13147
				public object target;

				// Token: 0x0400335C RID: 13148
				public MemoryTracker.ReferenceData targetRef;

				// Token: 0x0400335D RID: 13149
				public string path;
			}
		}

		// Token: 0x02000D67 RID: 3431
		private struct ChildReference
		{
			// Token: 0x0400335E RID: 13150
			public object child;

			// Token: 0x0400335F RID: 13151
			public string path;
		}

		// Token: 0x02000D68 RID: 3432
		public class MarkupComplete : Attribute
		{
		}
	}
}
