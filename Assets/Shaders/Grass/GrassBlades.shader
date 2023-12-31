Shader "Grass/GrassBlades"
{
    Properties
    {
        _BaseColor("Base color", Color) = (0, 0.5, 0, 1)
        _TipColor("Tip color", Color) = (0, 1, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "HighDefinitionPipeline" "IgnoreProjector" = "True" }
        
        Pass 
        {
          
          Name "ForwardLit"
          Tags{"LightMode" = "UniversalForward"}
          Cull Off
          
          HLSLPROGRAM
          // Compute buffer
          #pragma prefer_hlslcc gles
          #pragma exclude_renderers d3d11_9x
          #pragma target 5.0

          // Lighting and shadow
          #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
          #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
          #pragma multi_compile _ _ADDITIONAL_LIGHTS
          #pragma multi_compile _ _ADDITIONAL_LIGHTS_SHADOWS
          #pragma multi_compile _ _SHADOW_SOFT

          // Register functions
          #pragma vertex Vertex
          #pragma fragment Fragment

          // Include file
          #include "GrassBlades.hlsl"

          ENDHLSL
        }
    }
}
