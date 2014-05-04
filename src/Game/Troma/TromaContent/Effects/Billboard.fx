//-----------------------------------------------------------------------------
// Globals.
//-----------------------------------------------------------------------------

float4x4 world;
float4x4 view;
float4x4 projection;

float2 billboardSize;

//-----------------------------------------------------------------------------
// Textures.
//-----------------------------------------------------------------------------

texture colorMap;
sampler colorMapSampler = sampler_state
{
	Texture = <colorMap>;
    MinFilter = Anisotropic;
	MagFilter = Linear;
    MipFilter = Linear;
    MaxAnisotropy = 16;
};

//-----------------------------------------------------------------------------
// Vertex shaders.
//-----------------------------------------------------------------------------

void VS_BillboardingCameraAligned(in  float3 inPosition  : POSITION,
                                  in  float4 inTexCoord  : TEXCOORD0,
			                      out float4 outPosition : POSITION,
			                      out float2 outTexCoord : TEXCOORD0)
{
	float4x4 worldViewProjection = mul(mul(world, view), projection);
	
	float2 offset = inTexCoord.zw;
	float3 xAxis = float3(view._11, view._21, view._31);
	float3 yAxis = float3(view._12, view._22, view._32);

	float3 pos = inPosition + (offset.x * xAxis) + (offset.y * yAxis);

	outPosition = mul(float4(pos, 1.0f), worldViewProjection);
	outTexCoord = inTexCoord.xy;
}

void VS_BillboardingWorldYAxisAligned(in  float3 inPosition  : POSITION,
                                      in  float4 inTexCoord  : TEXCOORD0,
			                          out float4 outPosition : POSITION,
			                          out float2 outTexCoord : TEXCOORD0)
{
	float4x4 worldViewProjection = mul(mul(world, view), projection);
	
	float2 offset = inTexCoord.zw;
	float3 xAxis = float3(view._11, view._21, view._31);
	float3 yAxis = float3(0.0f, 1.0f, 0.0f);

	float3 pos = inPosition + (offset.x * xAxis) + (offset.y * yAxis);

	outPosition = mul(float4(pos, 1.0f), worldViewProjection);
	outTexCoord = inTexCoord.xy;
}

//-----------------------------------------------------------------------------
// Pixel shaders.
//-----------------------------------------------------------------------------

void PS_Billboarding(in  float2 inTexCoord : TEXCOORD0,
                     out float4 outColor   : COLOR)
{
	outColor = tex2D(colorMapSampler, inTexCoord);
}

//-----------------------------------------------------------------------------
// Techniques.
//-----------------------------------------------------------------------------

technique BillboardingCameraAligned
{
	pass
	{
		VertexShader = compile vs_2_0 VS_BillboardingCameraAligned();
		PixelShader = compile ps_2_0 PS_Billboarding();
	}
}

technique BillboardingWorldYAxisAligned
{
	pass
	{
		VertexShader = compile vs_2_0 VS_BillboardingWorldYAxisAligned();
		PixelShader = compile ps_2_0 PS_Billboarding();
	}
}