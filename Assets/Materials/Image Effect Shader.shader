Shader "Posterize" {
	Properties {
		_Red ("Red", Int) = 8
		_Green ("Green", Int) = 8
		_Blue ("Blue", Int) = 8
		_Alpha ("Alpha", Int) = 8

	 	_MainTex ("", 2D) = "white" {}
	}

	SubShader {
		Lighting Off
		ZTest Always
		Cull Off
		ZWrite Off

	 	Pass {
	  		CGPROGRAM
	  		#pragma exclude_renderers flash
	  		#pragma vertex vert_img
	  		#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
	  		#include "UnityCG.cginc"

	  		uniform int _Red;
	  		uniform int _Green;
	  		uniform int _Blue;
	  		uniform int _Alpha;
	  		uniform sampler2D _MainTex;

	  		fixed4 frag (v2f_img i) : COLOR
	  		{
	   			fixed3 col = tex2D (_MainTex, i.uv).rgb;
				fixed4 c = fixed4(0.0, 0.0, 0.0, 0.2);
				c.r = floor(col.r * _Red) / _Red;
				c.g = floor(col.g * _Green) / _Green;
				c.b = floor(col.b * _Blue) / _Blue;
				c.w = floor(col.w * _Alpha)/_Alpha;
				return c;
	  		}
	  		ENDCG
	 	}
	}

	FallBack "Diffuse"
}