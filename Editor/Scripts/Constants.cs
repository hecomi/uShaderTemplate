﻿using UnityEngine;

namespace uShaderTemplate
{

[System.Serializable]
public struct Constant
{
    public string name;
    public string value;
}

[CreateAssetMenu(
    menuName = "uShaderTemplate/Constants", 
    order = Common.Editor.menuOrder + 1)]
public class Constants : ScriptableObject
{
    public Constant[] values;
}

}