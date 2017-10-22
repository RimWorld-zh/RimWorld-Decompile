using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	public class TreeNode_Editor : TreeNode
	{
		public object obj;

		public FieldInfo owningField;

		public int owningIndex = -1;

		private MethodInfo editWidgetsMethod = null;

		public EditTreeNodeType nodeType;

		private int indexToDelete = -1;

		public object ParentObj
		{
			get
			{
				return ((TreeNode_Editor)base.parentNode).obj;
			}
		}

		public Type ObjectType
		{
			get
			{
				Type result;
				if (this.owningField != null)
				{
					result = this.owningField.FieldType;
					goto IL_0062;
				}
				if (this.IsListItem)
				{
					result = this.ListRootObject.GetType().GetGenericArguments()[0];
					goto IL_0062;
				}
				if (this.obj != null)
				{
					result = this.obj.GetType();
					goto IL_0062;
				}
				throw new InvalidOperationException();
				IL_0062:
				return result;
			}
		}

		public object Value
		{
			get
			{
				object value;
				if (this.owningField != null)
				{
					value = this.owningField.GetValue(this.ParentObj);
					goto IL_006e;
				}
				if (this.IsListItem)
				{
					value = this.ListRootObject.GetType().GetProperty("Item").GetValue(this.ListRootObject, new object[1]
					{
						this.owningIndex
					});
					goto IL_006e;
				}
				throw new InvalidOperationException();
				IL_006e:
				return value;
			}
			set
			{
				if (this.owningField != null)
				{
					this.owningField.SetValue(this.ParentObj, value);
				}
				if (this.IsListItem)
				{
					this.ListRootObject.GetType().GetProperty("Item").SetValue(this.ListRootObject, value, new object[1]
					{
						this.owningIndex
					});
				}
			}
		}

		public bool IsListItem
		{
			get
			{
				return this.owningIndex >= 0;
			}
		}

		private object ListRootObject
		{
			get
			{
				return this.ParentObj;
			}
		}

		public override bool Openable
		{
			get
			{
				return (byte)((this.obj != null) ? ((this.nodeType != EditTreeNodeType.TerminalValue) ? ((((this.nodeType != EditTreeNodeType.ListRoot) ? 1 : ((int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null))) != 0) ? 1 : 0) : 0) : 0) != 0;
			}
		}

		public bool HasContentLines
		{
			get
			{
				return this.nodeType != EditTreeNodeType.TerminalValue;
			}
		}

		public bool HasNewButton
		{
			get
			{
				return (byte)((this.nodeType == EditTreeNodeType.ComplexObject && this.obj == null) ? 1 : ((this.owningField != null && this.owningField.FieldType.HasAttribute<EditorReplaceableAttribute>()) ? 1 : 0)) != 0;
			}
		}

		public bool HasDeleteButton
		{
			get
			{
				return (byte)(this.IsListItem ? 1 : ((this.owningField != null && this.owningField.FieldType.HasAttribute<EditorNullableAttribute>()) ? 1 : 0)) != 0;
			}
		}

		public string ExtraInfoText
		{
			get
			{
				string result;
				if (this.obj == null)
				{
					result = "null";
				}
				else if (this.obj.GetType().HasAttribute<EditorShowClassNameAttribute>())
				{
					result = this.obj.GetType().Name;
				}
				else if (this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
				{
					int num = (int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null);
					result = "(" + num.ToString() + " " + ((num != 1) ? "elements" : "element") + ")";
				}
				else
				{
					result = "";
				}
				return result;
			}
		}

		public string LabelText
		{
			get
			{
				return (this.owningField == null) ? ((!this.IsListItem) ? this.ObjectType.Name : this.owningIndex.ToString()) : this.owningField.Name;
			}
		}

		private TreeNode_Editor()
		{
		}

		public static TreeNode_Editor NewRootNode(object rootObj)
		{
			if (rootObj.GetType().IsValueEditable())
			{
				throw new ArgumentException();
			}
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.owningField = null;
			treeNode_Editor.obj = rootObj;
			treeNode_Editor.nestDepth = 0;
			treeNode_Editor.RebuildChildNodes();
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		public static TreeNode_Editor NewChildNodeFromField(TreeNode_Editor parent, FieldInfo fieldInfo)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningField = fieldInfo;
			if (!fieldInfo.FieldType.IsValueEditable())
			{
				treeNode_Editor.obj = fieldInfo.GetValue(parent.obj);
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		private static TreeNode_Editor NewChildNodeFromListItem(TreeNode_Editor parent, int listIndex)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningIndex = listIndex;
			object obj = parent.obj;
			Type type = obj.GetType();
			Type type2 = type.GetGenericArguments()[0];
			if (!type2.IsValueEditable())
			{
				object obj2 = treeNode_Editor.obj = type.GetProperty("Item").GetValue(obj, new object[1]
				{
					listIndex
				});
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		private void InitiallyCacheData()
		{
			if (this.obj != null && this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
			{
				this.nodeType = EditTreeNodeType.ListRoot;
			}
			else if (this.ObjectType.IsValueEditable())
			{
				this.nodeType = EditTreeNodeType.TerminalValue;
			}
			else
			{
				this.nodeType = EditTreeNodeType.ComplexObject;
			}
			if (this.obj != null)
			{
				this.editWidgetsMethod = this.obj.GetType().GetMethod("DoEditWidgets");
			}
		}

		public void RebuildChildNodes()
		{
			if (this.obj != null)
			{
				base.children = new List<TreeNode>();
				Type objType = this.obj.GetType();
				if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
				{
					int num = (int)objType.GetProperty("Count").GetValue(this.obj, null);
					for (int num2 = 0; num2 < num; num2++)
					{
						TreeNode_Editor item = TreeNode_Editor.NewChildNodeFromListItem(this, num2);
						base.children.Add(item);
					}
				}
				else
				{
					foreach (FieldInfo item3 in from f in this.obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					orderby this.InheritanceDistanceBetween(objType, f.DeclaringType) descending
					select f)
					{
						if (item3.GetCustomAttributes(typeof(UnsavedAttribute), true).Length <= 0 && item3.GetCustomAttributes(typeof(EditorHiddenAttribute), true).Length <= 0)
						{
							TreeNode_Editor item2 = TreeNode_Editor.NewChildNodeFromField(this, item3);
							base.children.Add(item2);
						}
					}
				}
			}
		}

		private int InheritanceDistanceBetween(Type childType, Type parentType)
		{
			Type type = childType;
			int num = 0;
			int result;
			while (true)
			{
				if (type == parentType)
				{
					result = num;
				}
				else
				{
					type = type.BaseType;
					num++;
					if (type != null)
						continue;
					Log.Error(childType + " is not a subclass of " + parentType);
					result = -1;
				}
				break;
			}
			return result;
		}

		public void CheckLatentDelete()
		{
			if (this.indexToDelete >= 0)
			{
				this.obj.GetType().GetMethod("RemoveAt").Invoke(this.obj, new object[1]
				{
					this.indexToDelete
				});
				this.RebuildChildNodes();
				this.indexToDelete = -1;
			}
		}

		public void Delete()
		{
			if (this.owningField != null)
			{
				this.owningField.SetValue(this.obj, null);
				return;
			}
			if (this.IsListItem)
			{
				((TreeNode_Editor)base.parentNode).indexToDelete = this.owningIndex;
				return;
			}
			throw new InvalidOperationException();
		}

		public void DoSpecialPreElements(Listing_TreeDefs listing)
		{
			if (this.obj != null)
			{
				if (this.editWidgetsMethod != null)
				{
					WidgetRow widgetRow = listing.StartWidgetsRow(base.nestDepth);
					this.editWidgetsMethod.Invoke(this.obj, new object[1]
					{
						widgetRow
					});
				}
				Editable editable = this.obj as Editable;
				if (editable != null)
				{
					GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
					foreach (string item in editable.ConfigErrors())
					{
						listing.InfoText(item, base.nestDepth);
					}
					GUI.color = Color.white;
				}
			}
		}

		public override string ToString()
		{
			string text = "EditTreeNode(";
			if (this.ParentObj != null)
			{
				text = text + " owningObj=" + this.ParentObj;
			}
			if (this.owningField != null)
			{
				text = text + " owningField=" + this.owningField;
			}
			if (this.owningIndex >= 0)
			{
				text = text + " owningIndex=" + this.owningIndex;
			}
			return text + ")";
		}
	}
}
