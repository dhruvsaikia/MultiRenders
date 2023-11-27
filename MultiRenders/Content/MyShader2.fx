#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix World;
matrix View;
matrix Projection;
matrix WorldViewProjection;

float3 LightPosition;
float4 LightColor;
float4 AmbientColor;
float3 CameraPosition;
float4 SpecularColor;
float SpecularPower;

Texture2D ShaderTexture;
sampler TextureSampler = sampler_state
{
    Texture = <ShaderTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

// Vertex Shader Input/Output structures
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
    float3 LightDir : TEXCOORD1;
    float3 ViewVector : TEXCOORD2;
};

// Vertex Shader
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.Normal = normalize(mul(input.Normal, (float3x3) World));
    
    // Calculate Light Direction and View Vector
    float3 worldPosition = mul(input.Position, World).xyz;
    output.LightDir = normalize(LightPosition - worldPosition);
    output.ViewVector = normalize(CameraPosition - worldPosition);
    output.TexCoord = input.TexCoord;

    return output;
}

// Pixel Shader
float4 MainPS(in VertexShaderOutput input) : SV_Target
{
    // Calculate Ambient Light
    float4 ambient = AmbientColor;

    // Calculate Diffuse Light
    float3 normal = normalize(input.Normal);
    float ndotl = max(dot(normal, input.LightDir), 0);
    float4 diffuse = ndotl * LightColor * ShaderTexture.Sample(TextureSampler, input.TexCoord);

    // Calculate Specular Light
    float3 reflection = reflect(-input.LightDir, normal);
    float specFactor = pow(max(dot(reflection, input.ViewVector), 0), SpecularPower);
    float4 specular = specFactor * SpecularColor;

    // Combine results
    float4 finalColor = ambient + diffuse + specular;
    return finalColor;
}

// Techniques
technique LambertianSpecular
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
}
