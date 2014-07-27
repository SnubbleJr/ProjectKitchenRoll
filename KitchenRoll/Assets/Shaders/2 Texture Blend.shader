Shader "TextureCombine_One_Drawcall" {
Properties {
    _Color ("Main Color", COLOR) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _SecondTex ("Overley (RGB)", 2D) = "white" {}
}
SubShader {
    Pass {			           
            // Apply base texture
            SetTexture [_MainTex] {
                combine texture
            }
 
            // Blend in the _SecondTex texture using the lerp operator
            SetTexture [_SecondTex] {
                combine texture lerp (texture) previous
            }

			SetTexture [_MainTex] {
                constantColor [_Color]
				combine previous * constant
			}
    }
}
 
Fallback "VertexLit"
}