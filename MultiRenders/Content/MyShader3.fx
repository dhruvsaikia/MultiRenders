#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;
matrix World;
float4 LightPosition; 
float4 CameraPosition;
texture2D ObjTexture;

sampler2D ObjSampler = sampler_state
{
    Texture = <ObjTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
    float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float3 WorldPos : TEXCOORD2; 
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.TexCoord = input.TexCoord;
    output.Normal = mul(input.Normal, (float3x3) World);
    output.WorldPos = mul(input.Position, World).xyz;

    return output;
}

// Pixel Shader
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float3 normal = normalize(input.Normal);
    float3 lightDir = normalize(LightPosition.xyz - input.WorldPos);
    float3 viewDir = normalize(CameraPosition.xyz - input.WorldPos);

    // Lambertian lighting
    float diff = max(dot(normal, lightDir), 0.0);
    float4 diffuseColor = diff * float4(1.0, 1.0, 1.0, 1.0); 

    // Specular highlights
    float3 reflection = reflect(-lightDir, normal);
    float spec = pow(max(dot(reflection, viewDir), 0.0), 16); 
    float4 specularColor = spec * float4(1.0, 0.0, 0.0, 1.0); 

    // Combine diffuse, specular, and texture
    float4 texColor = tex2D(ObjSampler, input.TexCoord);
    float4 finalColor = texColor * diffuseColor + specularColor;
    finalColor.a = 1.0; // Ensure full opacity

    return finalColor;
}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
}
