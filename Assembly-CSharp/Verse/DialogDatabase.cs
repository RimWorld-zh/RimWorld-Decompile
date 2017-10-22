using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class DialogDatabase
	{
		private static List<DiaNodeMold> Nodes;

		private static List<DiaNodeList> NodeLists;

		static DialogDatabase()
		{
			DialogDatabase.Nodes = new List<DiaNodeMold>();
			DialogDatabase.NodeLists = new List<DiaNodeList>();
			DialogDatabase.LoadAllDialog();
		}

		private static void LoadAllDialog()
		{
			DialogDatabase.Nodes.Clear();
			UnityEngine.Object[] array;
			UnityEngine.Object[] array2 = array = Resources.LoadAll("Dialog", typeof(TextAsset));
			for (int i = 0; i < array.Length; i++)
			{
				UnityEngine.Object @object = array[i];
				TextAsset ass = @object as TextAsset;
				if (@object.name == "BaseEncounters" || @object.name == "GeneratedDialogs")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.BaseEncounters);
				}
				if (@object.name == "InsanityBattles")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.InsanityBattles);
				}
				if (@object.name == "SpecialEncounters")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.Special);
				}
			}
			List<DiaNodeMold>.Enumerator enumerator = DialogDatabase.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DiaNodeMold current = enumerator.Current;
					current.PostLoad();
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			LayerLoader.MarkNonRootNodes(DialogDatabase.Nodes);
		}

		public static DiaNodeMold GetRandomEncounterRootNode(DiaNodeType NType)
		{
			List<DiaNodeMold> list = new List<DiaNodeMold>();
			List<DiaNodeMold>.Enumerator enumerator = DialogDatabase.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DiaNodeMold current = enumerator.Current;
					if (current.isRoot && (!current.unique || !current.used) && current.nodeType == NType)
					{
						list.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return list.RandomElement();
		}

		public static DiaNodeMold GetNodeNamed(string NodeName)
		{
			List<DiaNodeMold>.Enumerator enumerator = DialogDatabase.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DiaNodeMold current = enumerator.Current;
					if (current.name == NodeName)
					{
						return current;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			List<DiaNodeList>.Enumerator enumerator2 = DialogDatabase.NodeLists.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					DiaNodeList current2 = enumerator2.Current;
					if (current2.Name == NodeName)
					{
						return current2.RandomNodeFromList();
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
			Log.Error("Did not find node named '" + NodeName + "'.");
			return null;
		}
	}
}
