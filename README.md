uShaderTemplate
===============

**uShaderTemplate** is an editor asset to create shader from templates.

Install
-------

Download the latest .unitypackage from [Releases](https://github.com/hecomi/uShaderTemplate/releases) page,
then import it.

Usage
-----

1.  Prepare **Shader Template** file.
2.  Create **Generator** from *Create > Shader > uShaderTemplate > Generator*.
3.  Input *Shader Name* and select *Shader Template* from the inspector.
4.  Edit items in *Conditions*, *Variables*, and codes in code editors.
5.  Press *Export (Ctrl+R)* button to create a shader from the Generator.

Overview
--------

Generator is an asset file that manages a generated shader, save parameters,
and provide an interface to customize the shader with some rules
written in **shader template**. The following image is an example of a Generator
inspector which is automatically generated from shader template.

![Inspector](https://raw.githubusercontent.com/wiki/hecomi/uShaderTemplate/inspector.png)

The interface is generated from
*uShaderTemplate > Examples > Editor > Resources > ShaderTemplates > 1. VertFrag.txt*.
This is the content of this file:

**1. VertFrag.txt**

```shader
Shader "Custom/<Name>"
{

Properties
{
@block Properties
_MainTex("Texture", 2D) = "white" {}
@endblock
}

SubShader
{

Tags { "Queue"="<Queue=Geometry|Transparent>" "RenderType"="<RenderType=Opaque|Transparent>" }
LOD <LOD=100>

CGINCLUDE

#include "UnityCG.cginc"

struct v2f
{
    float2 uv : TEXCOORD0;
@if UseFog : true
    UNITY_FOG_COORDS(1)
@endif
    float4 vertex : SV_POSITION;
};

@block VertexShader
sampler2D _MainTex;
float4 _MainTex_ST;

v2f vert(appdata_full v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
    UNITY_TRANSFER_FOG(o,o.vertex);
    return o;
}
@endblock

@block FragmentShader
fixed4 frag(v2f i) : SV_Target
{
    fixed4 col = tex2D(_MainTex, i.uv);
    UNITY_APPLY_FOG(i.fogCoord, col);
    return col;
}
@endblock

ENDCG

Pass
{
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
@if UseFog : true
    #pragma multi_compile_fog
@endif
    ENDCG
}

}

CustomEditor "uShaderTemplate.MaterialEditor"

}
```

You might have found some grammars like `@if foo`, `<Bar>`, `@block baz`,
and these are special grammmars available in template file.
Shader variant like `#pragma multi_compile` and `#pragma shader_feature`
cannot customize the section outside `CGPROGRAM`, but with these grammar,
you can customize the all parts of the shader.

If you put template files like this in *Resources > ShaderTemplates*,
they are automatically detected and shown in the *Shader Template* field
in *Basic* section.

Grammars in Shader Template
---------------------------

```shader
// Condition:
//   A toggle filed labeled "Hoge Hoge" will appear in Conditions section.
//   Only when checked, the content will be output.
@if HogeHoge
#define HOGEHOGE
@endif

// You can use else block and give a default condition.
@if HogeHoge2 : false
#define HOGEHOGE2
@else
#define HOGEHOGE3
@endif

// Variable:
//   The name with <> will appear in Variables section as a text field.
//   You can give a default value with =, and =| will be pull-down list.
#define Hoge <Hoge>
#define Fuga <Fuga=Hoge> //
#define Piyo <Piyo=Hoge|Fuga|Piyo>

// Block:
//    The content will be a code editor.
//    Output shader has block like // @block ~ // @endblock and
//    if you edit the content directly with your own editor like Vim,
//    the result will be applied to the code editor in inspector.
@block Moge
float Moge2() { return _Move * _Moge; }
@endblock
```

Buttons
-------

![Buttons](https://raw.githubusercontent.com/wiki/hecomi/uShaderTemplate/buttons.png)

* **Export(Ctrl+R)**
  * Export shader from Generator. You can use *Ctrl + R* as a shortcut key
    instead of pressing this button.
* **Create Material**
  * Create material from the generated shader.
* **Reset to Default**
  * Reset all parameters to the default parameters written in template.
* **Update Template**
  * If you edit the template file, please press this before the export.
* **Reconvert All**
  * Convert all generated shaders forcedly. This is useful when you edit
    template file and want to apply the change to all shaders.


Constants
---------

![Constants](https://raw.githubusercontent.com/wiki/hecomi/uShaderTemplate/constants.png)

Instead of inputting variables in each inspector, you can use **Constants** asset
as a shared variables among multiple Generators.

Select *Create > Shader > uShaderTemplate > Constants* and add *Name* and *Value*
pair to *Values* field of the created Constants asset. Then, drag and drop it to
the *Constants* field of Generators which you want to apply the parameters to.
Please remember that if you modify a parameter in Constants,
you have to *Reconvert All* to apply the change to all generated shaders.

License
-------

The MIT License (MIT)

Copyright (c) 2017 hecomi

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
