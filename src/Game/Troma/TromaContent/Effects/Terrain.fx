// Matrix
float4x4 World;
float4x4 View;
float4x4 Projection;
 
// Light related
float4 AmbientColor;
float AmbientIntensity;
 
float3 LightDirection;
float4 DiffuseColor;
float DiffuseIntensity;
 
float4 SpecularColor;
float3 EyePosition;

texture2D ColorMap;
sampler2D ColorMapSampler = sampler_state
{
    Texture = <ColorMap>;
    MinFilter = linear;
    MagFilter = linear;
    MipFilter = linear;
	AddressU = Wrap;
	AddressV = Wrap;
};
 
// The input for the VertexShader
struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal : NORMAL0;
};
 
// The output from the vertex shader, used for later processing
struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float3 View : TEXCOORD2;
};
 
// The VertexShader.
VertexShaderOutput VertexShaderFunction(VertexShaderInput input,float3 Normal : NORMAL)
{
    VertexShaderOutput output;
 
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.TexCoord = input.TexCoord;

    float3 normal = normalize(mul(input.Normal, World));
    output.Normal = normal;
    output.View = normalize(float4(EyePosition, 1.0) - worldPosition);
 
    return output;
}
 
// The Pixel Shader
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(ColorMapSampler, input.TexCoord);

    float4 normal = float4(input.Normal, 1.0);
    float4 diffuse = saturate(dot(-LightDirection, normal));
    float4 reflect = normalize(2 * diffuse * normal - float4(LightDirection, 1.0));
    float4 specular = pow(saturate(dot(reflect, input.View)), 15);
 
    return color * AmbientColor * AmbientIntensity + 
           color * DiffuseIntensity * DiffuseColor * diffuse + 
           color * SpecularColor * specular;
}
 
// Our Techinique
technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}