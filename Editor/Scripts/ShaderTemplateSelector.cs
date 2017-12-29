using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace uShaderTemplate
{

public class ShaderTemplateSelector
{
    public SerializedProperty prop { get; private set; }

    public class OnChangeEventHandler : UnityEvent {}
    public OnChangeEventHandler onChange = new OnChangeEventHandler();

    struct TemplateInfo
    {
        public string name;
        public string path;
    }
    List<TemplateInfo> list_ = new List<TemplateInfo>();

    public string selected
    {
        get { return prop.stringValue; }
    }

    public string text
    {
        get {
            if (string.IsNullOrEmpty(prop.stringValue)) {
                prop.stringValue = list_[0].name;
            }
            var dir = Common.Setting.templateDirectoryPath;
            var asset = Resources.Load<TextAsset>(dir + "/" + prop.stringValue);
            return asset ? asset.text : "";
        }
    }

    public ShaderTemplateSelector(SerializedProperty prop)
    {
        this.prop = prop;

        var paths = Utils.GetShaderTemplatePathList();
        foreach (var path in paths) {
            if (Path.GetExtension(path) == ".txt") {
                var name = Path.GetFileNameWithoutExtension(path);
                var info = new TemplateInfo() {
                    name = name,
                    path = path
                };
                list_.Add(info);
            }
        }
    }

    public void Draw()
    {
        var currentIndex = list_.Select(x => x.name).ToList().IndexOf(prop.stringValue);
        if (currentIndex == -1) currentIndex = 0;

        EditorGUILayout.BeginHorizontal(); {
            var selectedIndex = EditorGUILayout.Popup(
                "Shader Template", 
                currentIndex, 
                list_.Select(x => x.name).ToArray());
            var selected = list_[selectedIndex];

            var openButtonStyle = EditorStyles.miniButton;
            openButtonStyle.fixedWidth = 40;
            if (GUILayout.Button("Open", openButtonStyle)) {
                var asset = AssetDatabase.LoadAssetAtPath(selected.path, typeof(Object));
                AssetDatabase.OpenAsset(asset);
            }

            var pre = prop.stringValue;
            var cur = selected.name;
            if (pre != cur) {
                prop.stringValue = cur;
                onChange.Invoke();
            }
        } EditorGUILayout.EndHorizontal();
    }
}

}