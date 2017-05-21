using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace uShaderTemplate
{

public class ShaderTemplateSelector
{
    public SerializedProperty prop { get; private set; }

    public delegate void OnChangeEventHandler();
    public OnChangeEventHandler onChange = () => {};

    List<string> list_ = new List<string>();

    public string selected
    {
        get { return prop.stringValue; }
    }

    public string text
    {
        get {
            if (string.IsNullOrEmpty(prop.stringValue)) {
                prop.stringValue = list_[0];
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
                list_.Add(name);
            }
        }
    }

    public void Draw()
    {
        var currentIndex = list_.IndexOf(prop.stringValue);
        if (currentIndex == -1) currentIndex = 0;

        var selectedIndex = EditorGUILayout.Popup("Shader Template", currentIndex, list_.ToArray());

        var pre = prop.stringValue;
        var cur = list_[selectedIndex];
        if (pre != cur) {
            prop.stringValue = cur;
            onChange();
        }
    }
}

}