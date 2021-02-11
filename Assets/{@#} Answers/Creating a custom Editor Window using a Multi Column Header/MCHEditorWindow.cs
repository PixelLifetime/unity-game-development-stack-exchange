using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;

public class MCHEditorWindow : EditorWindow
{
	/// <summary>
	/// This is a test object, just to showcase how this could be used.
	/// </summary>
	[System.Serializable]
	private class Enemy : ScriptableObject
	{
		[SerializeField] private string _name;
		public string Name => this._name;

		[SerializeField] private float _health;
		public float Health => this._health;

		[SerializeField] private Color _skinColor;
		public Color SkinColor => this._skinColor;

		[SerializeField] private List<GameObject> _gameObjects;
		public List<GameObject> GameObjects => this._gameObjects;

		public Enemy(string name, float health, Color skinColor)
		{
			this._name = name;
			this._health = health;
			this._skinColor = skinColor;
		}

		public void Initialize(string name, float health, Color skinColor)
		{
			this._name = name;
			this._health = health;
			this._skinColor = skinColor;
		}

		private void Awake()
		{
			this._name = "Default Name";
			this._health = 34.2f;
			this._skinColor = Color.white;
		}
	}

	[MenuItem(itemName: "Tools/MCH Editor Window")]
	public static MCHEditorWindow Open()
	{
		MCHEditorWindow commentsNotebookEditorWindow = EditorWindow.GetWindow<MCHEditorWindow>(
			title: "MCH Editor Window",
			focus: true
		);

		commentsNotebookEditorWindow.minSize = new Vector2(x: 450.0f, y: 100.0f);

		commentsNotebookEditorWindow.Show();

		return commentsNotebookEditorWindow;
	}

	private MultiColumnHeaderState _multiColumnHeaderState;
	private MultiColumnHeader _multiColumnHeader;

	private MultiColumnHeaderState.Column[] _columns;

	// Create a few test subjects.
	private Enemy[] _testObjects = new Enemy[3];
	//{
	//	new Enemy("Orc", 25.0f, Color.green),
	//	new Enemy("Fairy", 10.0f, Color.cyan),
	//	new Enemy("Mech Golem", 57.0f, Color.grey),
	//};

	private float _multiColumnHeaderWidth;

	private bool _firstOnGUIIterationAfterInitialize;

	private void Initialize()
	{
		this._firstOnGUIIterationAfterInitialize = true;

		this._multiColumnHeaderWidth = this.position.width;

		// We can move these columns into some ScriptableObject or some other data saving object/file to save their properties there, otherwise because of some events these settings will be recreated and state of the window won't be saved as expected.
		this._columns = new MultiColumnHeaderState.Column[]
		{
			new MultiColumnHeaderState.Column()
			{
				allowToggleVisibility = false, // At least one column must be there.
				autoResize = true,
				minWidth = 100.0f,
				maxWidth = 250.0f,
				canSort = true,
				sortingArrowAlignment = TextAlignment.Right,
				headerContent = new GUIContent("Name", "A name of an enemy."),
				headerTextAlignment = TextAlignment.Left,
			},
			new MultiColumnHeaderState.Column()
			{
				allowToggleVisibility = true,
				autoResize = true,
				minWidth = 300.0f,
				canSort = false,
				sortingArrowAlignment = TextAlignment.Right,
				headerContent = new GUIContent("Health", "A health of an enemy."),
				headerTextAlignment = TextAlignment.Center,
			},
			new MultiColumnHeaderState.Column()
			{
				allowToggleVisibility = true,
				autoResize = true,
				minWidth = 125.0f,
				maxWidth = 200.0f,
				canSort = false,
				sortingArrowAlignment = TextAlignment.Right,
				headerContent = new GUIContent("Skin Color", "A color of an enemy skin."),
				headerTextAlignment = TextAlignment.Center,
			},
			new MultiColumnHeaderState.Column()
			{
				allowToggleVisibility = false,
				autoResize = true,
				minWidth = 300.0f,
				canSort = false,
				sortingArrowAlignment = TextAlignment.Right,
				headerContent = new GUIContent("Game Objects", "An array of game objects."),
				headerTextAlignment = TextAlignment.Center,
			},
		};

		this._multiColumnHeaderState = new MultiColumnHeaderState(columns: this._columns);

		this._multiColumnHeader = new MultiColumnHeader(state: this._multiColumnHeaderState);

		// When we chagne visibility of the column we resize columns to fit in the window.
		this._multiColumnHeader.visibleColumnsChanged += (multiColumnHeader) =>
		{
			multiColumnHeader.ResizeToFit();
		};

		// Initial resizing of the content.
		this._multiColumnHeader.ResizeToFit();


		this._testObjects[0] = ScriptableObject.CreateInstance<Enemy>();
		this._testObjects[0].Initialize("Orc", 25.0f, Color.green);

		this._testObjects[1] = ScriptableObject.CreateInstance<Enemy>();
		this._testObjects[1].Initialize("Fairy", 10.0f, Color.cyan);

		this._testObjects[2] = ScriptableObject.CreateInstance<Enemy>();
		//this._testObjects[2].Initialize("Mech Golem", 57.0f, Color.grey);
	}

	private readonly Color _lighterColor = new Color(r: 1.0f, g: 1.0f, b: 1.0f, a: 0.15f);
	private readonly Color _darkerColor = new Color(r: 0.0f, g: 0.0f, b: 0.0f, a: 0.15f);

	private Vector2 _scrollPosition;

	private void OnGUI()
	{
		float columnHeight = EditorGUIUtility.singleLineHeight;

		// Basically we just draw something. Empty space. Which is `FlexibleSpace` here on top of the window.
		// We need this for - `GUILayoutUtility.GetLastRect()` because it needs at least 1 thing to be drawn before it.
		GUILayout.FlexibleSpace();

		// Get automatically aligned rect for our multi column header component.
		Rect windowRect = GUILayoutUtility.GetLastRect();

		// Here we are basically assigning the size of window to our newly positioned `windowRect`.
		windowRect.width = this.position.width;
		windowRect.height = this.position.height;

		// After compilation and some other events data of the window is lost if it's not saved in some kind of container. Usually those containers are ScriptableObject(s).
		if (this._multiColumnHeader == null)
		{
			this.Initialize();
		}

		GUIStyle groupGUIStyle = new GUIStyle(GUI.skin.box)
		{
			//padding = new RectOffset(left: 50, right: 50, top: 50, bottom: 50) // For some reason doesn't work with group.
			//border = new RectOffset(30, 30, 30, 30), // None work.
		};

		//! Here we just basically move around group. It's not really padding, we are just setting position and reducing size.
		Vector2 groupRectPaddingInWindow = new Vector2(20.0f, 20.0f);

		Rect groupRect = new Rect(source: windowRect);

		groupRect.x += groupRectPaddingInWindow.x;
		groupRect.width -= groupRectPaddingInWindow.x * 2;

		groupRect.y += groupRectPaddingInWindow.y;
		groupRect.height -= groupRectPaddingInWindow.y * 2;

		GUI.BeginGroup(position: groupRect, style: groupGUIStyle);
		{   // Group Scope.

			groupRect.x -= groupRectPaddingInWindow.x;
			groupRect.y -= groupRectPaddingInWindow.y;

			Rect positionalRectAreaOfScrollView = new Rect(source: groupRect);

			// Create a `viewRect` since it should be separate from `rect` to avoid circular dependency.
			Rect viewRect = new Rect(source: groupRect)
			{
				width = this._multiColumnHeaderState.widthOfAllVisibleColumns, // Scroll max on X is basically a sum of width of columns.
				//? Do not remove this hegiht. It's compensating for the size of bottom scroll slider when it appears, that is why the right side scroll slider appears.
				//height = groupRect.height - columnHeight, // Remove `columnHeight` - basically size of header.
			};

			groupRect.width += groupRectPaddingInWindow.x * 2;
			groupRect.height += groupRectPaddingInWindow.y * 2;

			this._scrollPosition = GUI.BeginScrollView(
				position: positionalRectAreaOfScrollView,
				scrollPosition: this._scrollPosition,
				viewRect: viewRect,
				alwaysShowHorizontal: false,
				alwaysShowVertical: false
			);
			{   // Scroll View Scope.

				//? After debugging for a few hours - this is the only hack I have found to actually work to aleviate that scaling bug.
				this._multiColumnHeaderWidth = Mathf.Max(positionalRectAreaOfScrollView.width + this._scrollPosition.x, this._multiColumnHeaderWidth);

				// This is a rect for our multi column table.
				Rect columnRectPrototype = new Rect(source: positionalRectAreaOfScrollView)
				{
					width = this._multiColumnHeaderWidth,
					height = columnHeight, // This is basically a height of each column including header.
				};

				// Draw header for columns here.
				this._multiColumnHeader.OnGUI(rect: columnRectPrototype, xScroll: 0.0f);

				float heightJump = columnHeight;

				// For each element that we have in object that we are modifying.
				//? I don't have an appropriate object here to modify, but this is just an example. In real world case I would probably use ScriptableObject here.
				for (int a = 0; a < this._testObjects.Length; a++)
				{
					SerializedObject serializedObject = new SerializedObject(obj: this._testObjects[a]);

					float calculatedRowHeight = 0.0f;

					//! We draw each type of field here separately because each column could require a different type of field as seen here.
					// This can be improved if we want to have a more robust system. Like for example, we could have logic of drawing each field moved to object itself.
					// Then here we would be able to just iterate through array of these objects and call a draw methods for these fields and use this window for many types of objects.
					// But example with such a system would be too complicated for gamedev.stackexchange, so I have decided to not overengineer and just use hard coded indices for columns - `columnIndex`.

					Rect rowRect = new Rect(source: columnRectPrototype);

					//rowRect.y += columnHeight * (a + 1);

					// Name field.
					int columnIndex = 0;

					if (this._multiColumnHeader.IsColumnVisible(columnIndex: columnIndex))
					{
						int visibleColumnIndex = this._multiColumnHeader.GetVisibleColumnIndex(columnIndex: columnIndex);

						Rect columnRect = this._multiColumnHeader.GetColumnRect(visibleColumnIndex: visibleColumnIndex);

						// This here basically is a row height, you can make it any value you like. Or you could calculate the max field height here that your object has and store it somewhere then use it here instead of `EditorGUIUtility.singleLineHeight`.
						// We move position of field on `y` by this height to get correct position.
						columnRect.y = rowRect.y + heightJump;
						//columnRect.height += heightJump;

						GUIStyle nameFieldGUIStyle = new GUIStyle(GUI.skin.label)
						{
							padding = new RectOffset(left: 10, right: 10, top: 2, bottom: 2)
						};

						EditorGUI.LabelField(
							position: this._multiColumnHeader.GetCellRect(visibleColumnIndex: visibleColumnIndex, columnRect),
							label: new GUIContent(this._testObjects[a].Name),
							style: nameFieldGUIStyle
						);
					}

					// Health slider field.
					columnIndex = 1;

					if (this._multiColumnHeader.IsColumnVisible(columnIndex: columnIndex))
					{
						int visibleColumnIndex = this._multiColumnHeader.GetVisibleColumnIndex(columnIndex: columnIndex);

						Rect columnRect = this._multiColumnHeader.GetColumnRect(visibleColumnIndex: visibleColumnIndex);

						columnRect.y = rowRect.y + heightJump;
						//columnRect.height += heightJump;

						SerializedProperty serializedProperty = serializedObject.FindProperty(propertyPath: "_health");

						calculatedRowHeight = Mathf.Max(calculatedRowHeight, EditorGUI.GetPropertyHeight(property: serializedProperty, includeChildren: true));

						EditorGUI.Slider(
							position: this._multiColumnHeader.GetCellRect(visibleColumnIndex: visibleColumnIndex, columnRect),
							property: serializedProperty,
							leftValue: 0.0f,
							rightValue: 100.0f,
							label: GUIContent.none
						);
					}

					// Skin color field.
					columnIndex = 2;

					if (this._multiColumnHeader.IsColumnVisible(columnIndex: columnIndex))
					{
						int visibleColumnIndex = this._multiColumnHeader.GetVisibleColumnIndex(columnIndex: columnIndex);

						Rect columnRect = this._multiColumnHeader.GetColumnRect(visibleColumnIndex: visibleColumnIndex);

						columnRect.y = rowRect.y + heightJump;
						//columnRect.height += heightJump;

						SerializedProperty serializedProperty = serializedObject.FindProperty(propertyPath: "_skinColor");

						calculatedRowHeight = Mathf.Max(calculatedRowHeight, EditorGUI.GetPropertyHeight(property: serializedProperty, includeChildren: true));

						EditorGUI.PropertyField(
							position: this._multiColumnHeader.GetCellRect(visibleColumnIndex: visibleColumnIndex, columnRect),
							property: serializedProperty,
							label: GUIContent.none
						);
					}

					// Skin color field.
					columnIndex = 3;

					if (this._multiColumnHeader.IsColumnVisible(columnIndex: columnIndex))
					{
						int visibleColumnIndex = this._multiColumnHeader.GetVisibleColumnIndex(columnIndex: columnIndex);

						Rect columnRect = this._multiColumnHeader.GetColumnRect(visibleColumnIndex: visibleColumnIndex);

						columnRect.y = rowRect.y + heightJump;
						//columnRect.height += heightJump;

						SerializedProperty serializedProperty = serializedObject.FindProperty(propertyPath: "_gameObjects");

						calculatedRowHeight = Mathf.Max(calculatedRowHeight, EditorGUI.GetPropertyHeight(property: serializedProperty, includeChildren: true));

						EditorGUI.PropertyField(
							position: this._multiColumnHeader.GetCellRect(visibleColumnIndex: visibleColumnIndex, columnRect),
							property: serializedProperty,
							includeChildren: true
						);
					}

					// The problem of it drawing here is that it draws over fields instead of under them.
					//? The soultion could be to cache height for each row after 1 OnGUI and use it instead, upading it each time it reaches the end.
					Rect backgroundColorRect = new Rect(source: rowRect)
					{
						y = rowRect.y + heightJump,
						height = calculatedRowHeight
					};

					// Draw a texture before drawing each of the fields for the whole row.
					if (a % 2 == 0)
						EditorGUI.DrawRect(rect: backgroundColorRect, color: this._darkerColor);
					else
						EditorGUI.DrawRect(rect: backgroundColorRect, color: this._lighterColor);

					heightJump += calculatedRowHeight;
				}
			}
			GUI.EndScrollView(handleScrollWheel: true);
		}
		GUI.EndGroup();

		if (!this._firstOnGUIIterationAfterInitialize)
		{
			//! Uncomment this if you want to have appropriate ResizeToFit(). It brings jaggedness which I didn't like, so I have removed it knowing the implications.

			//float difference = 50.0f;

			////! Magic number `difference` is just a number to ensure that this width won't be exceeded by this auto scale bug. Lowering this number could cause it to reappear.
			////? If you don't mind resizing to fit to sometimes overscale columns then just remove these next 2 lines of code.
			//if (this._multiColumnHeaderWidth - this._multiColumnHeaderState.widthOfAllVisibleColumns > difference)
			//	this._multiColumnHeaderWidth -= difference;
		}

		this._firstOnGUIIterationAfterInitialize = false;
	}

	private void Awake()
	{
		this.Initialize();
	}
}
#endif