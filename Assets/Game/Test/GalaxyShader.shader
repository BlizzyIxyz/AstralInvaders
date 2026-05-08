Shader "Hidden/Galaxy2DShader"
{
    Properties
    {
        _MainTex ("Star Texture", 2D) = "white" {}
        _Radius ("Radius", Float) = 10.0
        _ArmSpread ("Arm Spread", Float) = 2.0
        _ArmCount ("Arm Count", Float) = 2.0 // Используем Float для стабильности
        _ArmCurve ("Arm Curve", Float) = 1.5
        _RotationSpeed ("Rotation Speed", Float) = 0.5
        _SpeedVariance ("Speed Variance", Float) = 1.0
        _SizeMultiplier ("Size Multiplier", Float) = 1.0
        _SizeVariance ("Size Variance", Float) = 0.5
        
        _ColorInner ("Color Inner", Color) = (1, 0.9, 0.8, 1)
        _ColorMid ("Color Mid", Color) = (1, 0.7, 0.5, 1)
        _ColorOuter ("Color Outer", Color) = (0.5, 0.7, 1, 1)
        _ColorEdge ("Color Edge", Color) = (0.2, 0.2, 0.5, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" }
        Blend SrcAlpha One
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5 // Требуется для SV_VertexID
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _Radius, _ArmSpread, _ArmCurve, _RotationSpeed, _SpeedVariance, _SizeMultiplier, _SizeVariance;
            float _ArmCount; // Принимаем как float
            float3 _Center;
            float4 _ColorInner, _ColorMid, _ColorOuter, _ColorEdge;

            // Хэш-функция
            float random(float2 p)
            {
                return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert(uint id : SV_VertexID)
            {
                v2f o;

                // 1. Определяем индекс частицы и вершину квадрата
                uint particleIndex = id / 4;
                uint cornerIndex = id % 4;

                // 2. Случайные значения
                float rand1 = random(float2(particleIndex, 0.0));
                float rand2 = random(float2(particleIndex, 1.0));
                float rand3 = random(float2(particleIndex, 2.0));
                float rand4 = random(float2(particleIndex, 3.0));

                // 3. Распределение
                float distNorm = sqrt(rand1); 
                float r = distNorm * _Radius;

                // 4. Спирали
                float baseAngle = log(r + 0.1) * _ArmCurve;
                float armIndex = floor(rand2 * _ArmCount);
                float armAngle = (6.283185 / _ArmCount) * armIndex;
                float spreadNoise = (rand3 - 0.5) * _ArmSpread * (r / _Radius + 0.1);
                float angle = baseAngle + armAngle + spreadNoise;

                // 5. Вращение
                float orbitalSpeed = _RotationSpeed / (1.0 + r * 0.5); 
                orbitalSpeed *= (1.0 + (rand4 - 0.5) * _SpeedVariance);
                angle += _Time.y * orbitalSpeed;

                // Позиция центра звезды
                float2 centerPos = float2(cos(angle), sin(angle)) * r;

                // 6. Геометрия квада (Quad)
                float2 offsets[4];
                offsets[0] = float2(-1, -1);
                offsets[1] = float2( 1, -1);
                offsets[2] = float2(-1,  1);
                offsets[3] = float2( 1,  1);
                
                float2 uvs[4];
                uvs[0] = float2(0, 0);
                uvs[1] = float2(1, 0);
                uvs[2] = float2(0, 1);
                uvs[3] = float2(1, 1);

                // Размер звезды
                float size = (1.0 - rand3 * _SizeVariance) * _SizeMultiplier;
                size *= lerp(1.5, 0.5, distNorm); 

                float2 cornerOffset = offsets[cornerIndex] * size;
                float2 worldPos = centerPos + cornerOffset + _Center.xy;

                o.vertex = UnityObjectToClipPos(float3(worldPos, 0));
                
                // 7. Цвет
                float4 col;
                if (distNorm < 0.33) col = lerp(_ColorInner, _ColorMid, distNorm * 3.0);
                else if (distNorm < 0.66) col = lerp(_ColorMid, _ColorOuter, (distNorm - 0.33) * 3.0);
                else col = lerp(_ColorOuter, _ColorEdge, (distNorm - 0.66) * 3.0);

                o.color = col;
                o.uv = uvs[cornerIndex];

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texCol = tex2D(_MainTex, i.uv);
                return texCol * i.color;
            }
            ENDCG
        }
    }
}