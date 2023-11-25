#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;
float3 CameraPosition;
matrix World;
matrix ViewProjection;
float3 LightDirection;
float3 LightColor;
float AmbientStrength;
float3 WorldMin;
float3 WorldMax;
float3 DiffuseColor = float3(0, 1, 0);
float4 SpecularColor = float4(1, 0, 0, 1); 
float SpecularPower = 50.0f; 


texture teapotTexture;
sampler BasicTextureSampler = sampler_state
{

    texture = <teapotTexture>;
   
};
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL;
    float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float3 WorldPosition : TEXCOORD1;
    float3 Normal : NORMAL;
    float2 UV : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.UV = input.UV;
    output.WorldPosition = mul(input.Position, World).xyz;

    return output;
}

float4 MainPS(VertexShaderOutput input) : SV_Target
{
 
    float3 worldPosition = input.WorldPosition;
    float3 basecolor = tex2D(BasicTextureSampler, input.UV).rgb;
    
    //Tint base on World position
    float3 tint = float3(worldPosition.x, worldPosition.y, worldPosition.z);
    tint = 0.5f + 0.5f * normalize(tint);
    
    float3 finalcolor = basecolor * tint;
    
    
    
    return float4(finalcolor, 1.0f);

}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
