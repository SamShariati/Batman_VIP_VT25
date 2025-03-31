Shader "Custom/Echolocation"
{
    Properties
    {
        _PulseColor ("Pulse Color", Color) = (1, 1, 1, 1)
        _CameraPosition ("Camera Position", Vector) = (0, 0, 0)
        _ConstantLightCameraPosition ("Constant Light Camera Position", Vector) = (0, 0, 0) // New camera position for constant light
        _PulseDistance ("Pulse Distance", Float) = 0.0
        _PulseWidth ("Pulse Width", Float) = 0.1
        _MaxDistance ("Max Distance", Float) = 50.0 // Maximum distance for pulse fading
        _EdgeStrength ("Edge Strength", Float) = 1.0 // Strength of normal-based edge highlighting
        _ConstantLightIntensity ("Constant Light Intensity", Float) = 0.5 // Intensity of constant illumination
        _ConstantLightMaxDistance ("Constant Light Max Distance", Float) = 20.0 // Max distance for constant light effect
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Include necessary libraries
            #include "UnityCG.cginc"

            // Properties
            fixed4 _PulseColor;
            float3 _CameraPosition;
            float3 _ConstantLightCameraPosition; // Separate constant light camera position
            float _PulseDistance;
            float _PulseWidth;
            float _MaxDistance;
            float _EdgeStrength;
            float _ConstantLightIntensity;
            float _ConstantLightMaxDistance;

            // Vertex input
            struct appdata
            {
                float4 vertex : POSITION;
                float3 worldPos : TEXCOORD0;
                float3 normal : NORMAL;
            };

            // Vertex output
            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate distance from fragment to pulse camera
                float distanceToPulseCamera = distance(i.worldPos, _CameraPosition);

                // Calculate distance from fragment to constant light camera
                float distanceToConstantLightCamera = distance(i.worldPos, _ConstantLightCameraPosition);

                // Constant light effect based on distance from constant light camera
                float constantLightFade = saturate(1.0 - distanceToConstantLightCamera / _ConstantLightMaxDistance);
                float constantLightEffect = constantLightFade * _ConstantLightIntensity;

                // Pulse effect based on distance to pulse camera and pulse parameter
                float pulseEffect = abs(distanceToPulseCamera - _PulseDistance);
                float pulseIntensity = smoothstep(_PulseWidth, 0.0, pulseEffect);

                // Fade out pulse based on distance (using max distance)
                float distanceFade = 1.0 - saturate(distanceToPulseCamera / _MaxDistance);
                pulseIntensity *= distanceFade;

                // Edge highlighting using surface normals and direction to pulse camera
                float3 viewDir = normalize(_CameraPosition - i.worldPos);
                float edgeEffect = pow(1.0 - dot(i.normal, viewDir), _EdgeStrength);

                // Combine pulse color with edge effect and intensity
                float finalIntensity = (pulseIntensity + constantLightEffect) * edgeEffect;
                return lerp(fixed4(0, 0, 0, 1), _PulseColor, finalIntensity);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}




