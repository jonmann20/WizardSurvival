Shader "RealSky/RealSky" {

    Properties {
        _Texture01 ("Base (RGB)", 2D) = "white" {}
        _Texture02 ("Base (RGB)", 2D) = "white" {}
        _Blend ("Blend", Range(0.0,1.0)) = 0.0

    }

    Category {

        ZWrite On

    SubShader {

        Pass {    
            Tags { "RenderType"="Opaque" }

            Lighting Off

            SetTexture [_Texture01] { combine texture }
            SetTexture [_Texture02] { constantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }


        }

    } 


}

}