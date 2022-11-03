Shader "Custom/LimLight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _RimPower("Rim Power", Range(0.5, 3.0)) = 1
        [Toggle] _show("Show", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
        };

        fixed4 _Color;
        float _RimPower;
        float _show;

        void surf (Input IN, inout SurfaceOutput o)
        {
            float rim = saturate(dot(normalize(IN.viewDir), o.Normal));
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            if(_show >0)
                o.Emission = c.rgb * pow(rim, _RimPower);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
