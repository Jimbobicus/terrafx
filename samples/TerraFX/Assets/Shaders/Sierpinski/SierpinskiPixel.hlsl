// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "SierpinskiTypes.hlsl"

Texture3D textureInput : register(t2);
SamplerState samplerInput : register(s2);

float4 main(PSInput input) : SV_Target
{
    float4 color = textureInput.Sample(samplerInput, input.uvw);
    float3 normal = normalize(input.normal);
    float brightness = 0.2f + 0.8f * abs(normal[2]);
    return color * brightness;
}
