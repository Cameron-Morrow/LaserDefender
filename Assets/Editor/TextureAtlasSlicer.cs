using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class TextureAtlasSlicer : EditorWindow {

    [MenuItem("CONTEXT/TextureImporter/Slice Sprite Using XML")]

    public static void SliceUsingXml (MenuCommand command) {
        var textureImporter = command.context as TextureImporter;
        var window = CreateInstance<TextureAtlasSlicer>();
        window.Importer = textureImporter;
        window.ShowUtility();
    }

    [MenuItem("CONTEXT/TextureImporter/Slice Sprite Using XML", true)]
    
    public static bool ValidateSliceUsingXml (MenuCommand command) {

        var textureImporter = command.context as TextureImporter;

        //valid only if the texture Type is 'sprite' or 'advanced'.
        return textureImporter && textureImporter.textureType == TextureImporterType.Sprite ||
               textureImporter.textureType == TextureImporterType.Default;
    }

    public TextureImporter Importer;

    [SerializeField] private TextAsset _xmlAsset;

    public SpriteAlignment SpriteAlignment = SpriteAlignment.Center;

    public Vector2 CustomOffset = new Vector2(0.5f, 0.5f);

    public void OnGUI() {

        _xmlAsset = EditorGUILayout.ObjectField("XML Source", _xmlAsset, typeof (TextAsset), false) as TextAsset;

        SpriteAlignment = (SpriteAlignment) EditorGUILayout.EnumPopup("Pivot", SpriteAlignment);

        var enabled = GUI.enabled;

        if (SpriteAlignment != SpriteAlignment.Custom) {

            GUI.enabled = false;
        }

        EditorGUILayout.Vector2Field("Custom Offset", CustomOffset);

        GUI.enabled = enabled;

        if (_xmlAsset == null) {

            GUI.enabled = false;
        }

        if (GUILayout.Button("Slice")) {

            PerformSlice();
        }

        GUI.enabled = enabled;
    }

    private void PerformSlice () {

        var document = new XmlDocument();
        document.LoadXml(_xmlAsset.text);
        
        var root = document.DocumentElement;

        if (root.Name == "TextureAtlas") {

            var failed = false;

            var texture = AssetDatabase.LoadMainAssetAtPath(Importer.assetPath) as Texture2D;

            var textureHeight = texture.height;

            var metaDataList = new List<SpriteMetaData>();

            foreach (XmlNode childNode in root.ChildNodes) {

                if (childNode.Name == "SubTexture") {

                    try {

                        var width = Convert.ToInt32(childNode.Attributes["Width"].Value);

                        var height = Convert.ToInt32(childNode.Attributes["Height"].Value);

                        var x = Convert.ToInt32(childNode.Attributes["x"].Value);

                        var y = textureHeight - (height + Convert.ToInt32(childNode.Attributes["y"].Value));

                        var spriteMetaData = new SpriteMetaData {
                            alignment = (int)SpriteAlignment,
                            border = new Vector4(),
                            name = childNode.Attributes["name"].Value,
                            pivot = GetPivotValue(SpriteAlignment, CustomOffset),
                            rect = new Rect(x, y, width, height)
                        };
                        
                        metaDataList.Add(spriteMetaData);

                    } catch (Exception exception) {

                        failed = true;
                        Debug.LogException(exception);
                    }

                } else {
                    
                    Debug.LogError("Child nodes should be named 'SubTexture' !");

                    failed = true;
                }
            }
            
            if (!failed) {

                Importer.spriteImportMode = SpriteImportMode.Multiple; 

                Importer.spritesheet = metaDataList.ToArray();

                EditorUtility.SetDirty(Importer);

                try {

                    AssetDatabase.StartAssetEditing();
                    AssetDatabase.ImportAsset(Importer.assetPath);
                } finally {

                    AssetDatabase.StopAssetEditing();
                    Close();
                }
            }
        } else {

            Debug.LogError("XML needs to have a 'TextureAtlas' root node!");

        }

    }

    //SpriteEditorUtility

    private static Vector2 GetPivotValue (SpriteAlignment alignment, Vector2 customOffset) {

        switch (alignment)
        {
            case SpriteAlignment.Center:
                return new Vector2(0.5f, 0.5f);

            case SpriteAlignment.TopLeft:
                return new Vector2(0.0f, 1f);

            case SpriteAlignment.TopCenter:
                return new Vector2(0.5f, 1f);

            case SpriteAlignment.TopRight:
                return new Vector2(1f, 1f);

            case SpriteAlignment.LeftCenter:
                return new Vector2(0.0f, 0.5f);

            case SpriteAlignment.RightCenter:
                return new Vector2(1f, 0.5f);

            case SpriteAlignment.BottomLeft:
                return new Vector2(0.0f, 0.0f);

            case SpriteAlignment.BottomCenter:
                return new Vector2(0.5f, 0.0f);

            case SpriteAlignment.BottomRight:
                return new Vector2(1f, 0.0f);

            case SpriteAlignment.Custom:
                return customOffset;

            default:
                return Vector2.zero;
        }

    }

}