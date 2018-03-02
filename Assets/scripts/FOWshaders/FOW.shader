// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FOW" {
    Properties {
        _PlayerPos ("PlayerPos", Vector) = (0,0,0,1)
		_SightRange("Sight Range", Float) = 1
		_MainTex("Texture", 2D) = "black" { }
		_Color("Main Color", Color) = (1,1,1,0.5)
    }
    SubShader {
		//Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Tags{ "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" "IgnoreProjector" = "True" }
        Pass {

		AlphaToMask On

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		fixed4 _Color;
		float4 _PlayerPos;
		float _SightRange;
		sampler2D _MainTex;

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 fragPos : VECTOR0;
		};

		float4 _MainTex_ST;

		v2f vert(appdata_base v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.fragPos = v.vertex;
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 texcol = tex2D(_MainTex, i.uv);
			float4 dis_vec = i.pos - _PlayerPos;
			float dis = length(dis_vec);
			float dis_from_sight = abs(dis - _SightRange);
			float4 col = texcol * _Color;// *(dis_from_sight / 1000);
			col.a = .5;
			//col.a = 1;
			
			if (dis < _SightRange)
			{
				col = Vector(0, 0, 0, 0);
				col.a = 0;
			}
			
			//clip(col.a - 100.0 / 255.0);
			//clip(length(col) - .25);
			return col;
		}
		ENDCG

        }
    }
}
