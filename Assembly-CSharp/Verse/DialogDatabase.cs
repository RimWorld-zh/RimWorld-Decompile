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
			Object[] array = Resources.LoadAll("Dialog", typeof(TextAsset));
			Object[] array2 = array;
			foreach (Object @object in array2)
			{
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
			foreach (DiaNodeMold node in DialogDatabase.Nodes)
			{
				node.PostLoad();
			}
			LayerLoader.MarkNonRootNodes(DialogDatabase.Nodes);
		}

		public static DiaNodeMold GetRandomEncounterRootNode(DiaNodeType NType)
		{
			List<DiaNodeMold> list = new List<DiaNodeMold>();
			foreach (DiaNodeMold node in DialogDatabase.Nodes)
			{
				if (node.isRoot && (!node.unique || !node.used) && node.nodeType == NType)
				{
					list.Add(node);
				}
			}
			return list.RandomElement();
		}

		public static DiaNodeMold GetNodeNamed(string NodeName)
		{
			foreach (DiaNodeMold node in DialogDatabase.Nodes)
			{
				if (node.name == NodeName)
				{
					return node;
				}
			}
			foreach (DiaNodeList nodeList in DialogDatabase.NodeLists)
			{
				if (nodeList.Name == NodeName)
				{
					return nodeList.RandomNodeFromList();
				}
			}
			Log.Error("Did not find node named '" + NodeName + "'.");
			return null;
		}
	}
}
