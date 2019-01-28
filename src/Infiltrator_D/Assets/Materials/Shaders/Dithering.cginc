#ifndef DITHERING_INCLUDED
#define DITHERING_INCLUDED

struct Input {
    float4 screenPos : TEXCOORD0;
    float2 uv_MainTex : TEXCOORD1;
};
sampler2D _MainTex;
fixed4 _Color;
void surf(Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = c.rgb;
    o.Alpha = c.a;
    // Screen-door transparency: Discard pixel if below threshold.
    float4x4 thresholdMatrix =
    { 1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
        13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
        4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
        16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
    };
    float4x4 _RowAccess = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
    float2 pos = IN.screenPos.xy / IN.screenPos.w;
    pos *= _ScreenParams.xy; // pixel position
    clip(_Color.a - thresholdMatrix[fmod(pos.x, 4)] * _RowAccess[fmod(pos.y, 4)]);
}

#endif