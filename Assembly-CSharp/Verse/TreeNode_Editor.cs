using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

		private TreeNode_Editor()
		{
		}

		public object ParentObj
		{
			get
			{
				return ((TreeNode_Editor)this.parentNode).obj;
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
				}
				else if (this.IsListItem)
				{
					result = this.ListRootObject.GetType().GetGenericArguments()[0];
				}
				else
				{
					if (this.obj == null)
					{
						throw new InvalidOperationException();
					}
					result = this.obj.GetType();
				}
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
				}
				else
				{
					if (!this.IsListItem)
					{
						throw new InvalidOperationException();
					}
					value = this.ListRootObject.GetType().GetProperty("Item").GetValue(this.ListRootObject, new object[]
					{
						this.owningIndex
					});
				}
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
					this.ListRootObject.GetType().GetProperty("Item").SetValue(this.ListRootObject, value, new object[]
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
				bool result;
				if (this.obj == null)
				{
					result = false;
				}
				else if (this.nodeType == EditTreeNodeType.TerminalValue)
				{
					result = false;
				}
				else
				{
					if (this.nodeType == EditTreeNodeType.ListRoot)
					{
						if ((int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null) == 0)
						{
							return false;
						}
					}
					result = true;
				}
				return result;
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
				if (this.nodeType == EditTreeNodeType.ComplexObject)
				{
					if (this.obj == null)
					{
						return true;
					}
				}
				return this.owningField != null && this.owningField.FieldType.HasAttribute<EditorReplaceableAttribute>();
			}
		}

		public bool HasDeleteButton
		{
			get
			{
				return this.IsListItem || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorNullableAttribute>());
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
					result = string.Concat(new string[]
					{
						"(",
						num.ToString(),
						" ",
						(num != 1) ? "elements" : "element",
						")"
					});
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
				string result;
				if (this.owningField != null)
				{
					result = this.owningField.Name;
				}
				else if (this.IsListItem)
				{
					result = this.owningIndex.ToString();
				}
				else
				{
					result = this.ObjectType.Name;
				}
				return result;
			}
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
				object value = type.GetProperty("Item").GetValue(obj, new object[]
				{
					listIndex
				});
				treeNode_Editor.obj = value;
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
				this.children = new List<TreeNode>();
				Type objType = this.obj.GetType();
				if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
				{
					int num = (int)objType.GetProperty("Count").GetValue(this.obj, null);
					for (int i = 0; i < num; i++)
					{
						TreeNode_Editor item = TreeNode_Editor.NewChildNodeFromListItem(this, i);
						this.children.Add(item);
					}
				}
				else
				{
					foreach (FieldInfo fieldInfo in from f in this.obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					orderby this.InheritanceDistanceBetween(objType, f.DeclaringType) descending
					select f)
					{
						if (fieldInfo.GetCustomAttributes(typeof(UnsavedAttribute), true).Length <= 0 && fieldInfo.GetCustomAttributes(typeof(EditorHiddenAttribute), true).Length <= 0)
						{
							TreeNode_Editor item2 = TreeNode_Editor.NewChildNodeFromField(this, fieldInfo);
							this.children.Add(item2);
						}
					}
				}
			}
		}

		private int InheritanceDistanceBetween(Type childType, Type parentType)
		{
			Type type = childType;
			int num = 0;
			while (type != parentType)
			{
				type = type.BaseType;
				num++;
				if (type == null)
				{
					Log.Error(childType + " is not a subclass of " + parentType, false);
					return -1;
				}
			}
			return num;
		}

		public void CheckLatentDelete()
		{
			if (this.indexToDelete >= 0)
			{
				this.obj.GetType().GetMethod("RemoveAt").Invoke(this.obj, new object[]
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
			}
			else
			{
				if (!this.IsListItem)
				{
					throw new InvalidOperationException();
				}
				((TreeNode_Editor)this.parentNode).indexToDelete = this.owningIndex;
			}
		}

		public void DoSpecialPreElements(Listing_TreeDefs listing)
		{
			if (this.obj != null)
			{
				if (this.editWidgetsMethod != null)
				{
					WidgetRow widgetRow = listing.StartWidgetsRow(this.nestDepth);
					this.editWidgetsMethod.Invoke(this.obj, new object[]
					{
						widgetRow
					});
				}
				Editable editable = this.obj as Editable;
				if (editable != null)
				{
					GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
					foreach (string text in editable.ConfigErrors())
					{
						listing.InfoText(text, this.nestDepth);
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

		[CompilerGenerated]
		private sealed class <RebuildChildNodes>c__AnonStorey0
		{
			internal Type objType;

			internal TreeNode_Editor $this;

			public <RebuildChildNodes>c__AnonStorey0()
			{
			}

			internal int <>m__0(FieldInfo f)
			{
				return this.$this.InheritanceDistanceBetween(this.objType, f.DeclaringType);
			}
		}
	}
}
