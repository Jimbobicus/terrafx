// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "TransformTypes.hlsl"

cbuffer PerFrameInput : register(b0)
{
    matrix frameTransform;
};

cbuffer PerPrimitiveInput : register(b1)
{
    matrix primitiveTransform;
};

PSInput main(VSInput input)
{
    PSInput output;

    output.color = input.color;

    output.position = float4(input.position, 1.0f);
    output.position = mul(output.position, primitiveTransform);
    output.position = mul(output.position, frameTransform);

    return output;
}
