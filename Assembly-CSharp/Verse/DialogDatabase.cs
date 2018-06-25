using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class DialogDatabase
	{
		private static List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		private static List<DiaNodeList> NodeLists = new List<DiaNodeList>();

		static DialogDatabase()
		{
			DialogDatabase.LoadAllDialog();
		}

		private static void LoadAllDialog()
		{
			DialogDatabase.Nodes.Clear();
			UnityEngine.Object[] array = Resources.LoadAll("Dialog", typeof(TextAsset));
			foreach (UnityEngine.Object @object in array)
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
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				diaNodeMold.PostLoad();
			}
			LayerLoader.MarkNonRootNodes(DialogDatabase.Nodes);
		}

		public static DiaNodeMold GetRandomEncounterRootNode(DiaNodeType NType)
		{
			List<DiaNodeMold> list = new List<DiaNodeMold>();
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				if (diaNodeMold.isRoot && (!diaNodeMold.unique || !diaNodeMold.used) && diaNodeMold.nodeType == NType)
				{
					list.Add(diaNodeMold);
				}
			}
			return list.RandomElement<DiaNodeMold>();
		}

		public static DiaNodeMold GetNodeNamed(string NodeName)
		{
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				if (diaNodeMold.name == NodeName)
				{
					return diaNodeMold;
				}
			}
			foreach (DiaNodeList diaNodeList in DialogDatabase.NodeLists)
			{
				if (diaNodeList.Name == NodeName)
				{
					return diaNodeList.RandomNodeFromList();
				}
			}
			Log.Error("Did not find node named '" + NodeName + "'.", false);
			return null;
		}
	}
}
