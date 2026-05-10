Shader "Hidden/GalaxyShader"
{
    Properties
    {
        _MainTex ("Star Sprite", 2D) = "white" {}
        _Radius ("Galaxy Radius", Float) = 10.0
        _Thickness ("Vertical Thickness", Float) = 1.5
        _CoreBulge ("Core Bulge", Float) = 1.0
        _ArmSpread ("Arm Spread", Float) = 2.0
        _ArmCount ("Arm Count", Int) = 2
        _ArmCurve ("Arm Curve", Float) = 1.5
        _ArmSeparation ("Arm Separation", Float) = 1.0
        _RotationSpeed ("Rotation Speed", Float) = 0.5
        _SpeedVariance ("Speed Variance", Float) = 1.0
        _CoreBrightness ("Core Brightness", Float) = 2.0
        _SizeMultiplier ("Size Multiplier", Float) = 1.0
        
        // Цвета для градиента
        _ColorInner ("Color Inner", Color) = (1, 0.9, 0.8, 1)
        _ColorMid ("Color Mid", Color) = (1, 0.8, 0.6, 1)
        _ColorOuter ("Color Outer", Color) = (0.6, 0.8, 1, 1)
        _ColorEdge ("Color Edge", Color) = (0.2, 0.3, 0.5, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha One // Аддитивное смешивание для эффекта свечения
        ZWrite Off // Отключаем запись в буфер глубины для производительности
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            // --- Structs ---
            struct appdata
            {
                uint id : SV_VertexID; // Используем ID вершины как источник данных
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float size : PSIZE; // Размер точки (Point Size)
            };

            // --- Variables ---
            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float _Radius, _Thickness, _CoreBulge, _ArmSpread;
            int _ArmCount;
            float _ArmCurve, _ArmSeparation, _RotationSpeed, _SpeedVariance;
            float _CoreBrightness, _SizeMultiplier;
            float3 _Center;
            
            float4 _ColorInner, _ColorMid, _ColorOuter, _ColorEdge;

            // --- Random Helper ---
            // Генерация псевдослучайных чисел на основе ID
            float random(float2 p)
            {
                return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
            }
            
            float3 random3(float2 p)
            {
                return float3(random(p), random(p + 0.1), random(p + 0.2));
            }

            // --- Vertex Shader ---
            v2f vert(appdata v)
            {
                v2f o;
                
                // 1. Генерация уникальных параметров для каждой звезды по ID
                float3 rnd = random3(float2(v.id, 0.0));
                float3 rnd2 = random3(float2(v.id, 1.0)); // Дополнительный шум для вариаций

                // 2. Распределение расстояния от центра (экспоненциальное - плотнее в центре)
                float distFromCenter = pow(rnd.x, 0.5); // sqrt дает плотное ядро
                float r = distFromCenter * _Radius;

                // 3. Формирование спиралей
                // Логарифмическая спираль: угол зависит от логарифма расстояния
                float armIndex = floor(rnd.y * _ArmCount);
                float armAngleOffset = (2.0 * 3.14159 / _ArmCount) * armIndex;
                
                // Базовый угол спирали
                float spiralAngle = log(r + 0.1) * _ArmCurve;
                
                // Добавляем случайный разброс ("шум" рукавов)
                // Используем синус/косинус для создания областей плотности
                float noiseSpread = (rnd.z - 0.5) * _ArmSpread;
                // Добавляем вариацию угла, зависящую от расстояния (рукава шире снаружи)
                float angle = spiralAngle + armAngleOffset + noiseSpread * (r / _Radius);

                // 4. Движение (вращение)
                // Скорость зависит от расстояния: дальше -> медленнее (Keplerian-like)
                // Используем обратную зависимость, но сглаживаем, чтобы центр не вращался бесконечно быстро
                float orbitalSpeed = _RotationSpeed / (1.0 + r * 0.2); 
                orbitalSpeed *= (1.0 + (rnd2.x - 0.5) * _SpeedVariance); // Шум скорости
                
                angle += _Time.y * orbitalSpeed;

                // 5. Вычисление позиции в 3D
                float x = cos(angle) * r;
                float z = sin(angle) * r;
                
                // Высота (Y): тоньше к краям, толще в центре (ядро)
                float heightFactor = _Thickness * (1.0 - distFromCenter * (1.0 - _CoreBulge));
                float y = (rnd2.y - 0.5) * heightFactor * 2.0; // Случайная высота

                // Сдвиг к центру объекта
                float3 localPos = float3(x, y, z) + _Center;
                o.vertex = UnityObjectToClipPos(localPos);

                // 6. Цвет и размер
                // Интерполяция цвета по расстоянию
                float t = distFromCenter;
                float4 col;
                if (t < 0.33) col = lerp(_ColorInner, _ColorMid, t * 3.0);
                else if (t < 0.66) col = lerp(_ColorMid, _ColorOuter, (t - 0.33) * 3.0);
                else col = lerp(_ColorOuter, _ColorEdge, (t - 0.66) * 3.0);

                // Яркость ядра
                col.rgb *= (1.0 + (1.0 - t) * _CoreBrightness);
                
                // Добавляем немного случайности в яркость
                col.a *= 0.5 + rnd2.z * 0.5; 

                o.color = col;

                // Размер звезды
                // Ближе к центру - ярче и крупнее (визуально)
                float size = (0.5 + rnd.x * 1.5) * _SizeMultiplier;
                size *= (1.0 - t * 0.5); // Уменьшаем размер к краям
                
                o.size = size * 5.0; // Множитель для точечных спрайтов

                // UV для текстуры (все звезды используют один спрайт, но можно повернуть)
                o.uv = float2(0.5, 0.5);

                return o;
            }

            // --- Fragment Shader ---
            fixed4 frag(v2f i) : SV_Target
            {
                // Простой сэмпл текстуры (для точек UV обычно не нужны, 
                // но если рендерить как MeshTopology.Quads, то нужны)
                // Для топологии Points мы используем shape circle logic:
                
                // Рисуем круг вместо квадрата (для MeshTopology.Points)
                float2 center = i.uv; // Для Points это не сработает как текстурирование, нужно вычислять дистанцию от центра точки.
                // НО DrawProcedural с Points рисует квадраты. 
                // Чтобы сделать "мягкую" точку, можно просто вернуть цвет.
                // Для красивого спрайта лучше использовать Geometry Shader, но он тяжелый.
                // Мы используем простую формулу точки:
                
                // Примечание: При MeshTopology.Points мы не имеем доступа к UV внутри точки стандартно,
                // но можно использовать VPOS. Для простоты вернем цвет, он уже содержит альфа.
                
                return i.color;
            }
            ENDCG
        }
    }
}