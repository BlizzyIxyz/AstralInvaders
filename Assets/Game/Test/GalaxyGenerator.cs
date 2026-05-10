using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class GalaxyGenerator : MonoBehaviour
{
    [Header("Main Settings")]
    [Tooltip("Количество звезд. Для 100к+ FPS останется высоким.")]
    public int starCount = 100000;

    [Header("Galaxy Shape")]
    [Range(0.01f, 50f)] public float radius = 10f;
    [Range(0.1f, 10f)] public float thickness = 1.5f;
    [Range(0.01f, 5f)] public float coreBulge = 1.0f; // Выпуклость ядра
    [Range(0.01f, 10f)] public float armSpread = 2.0f; // Разброс рукавов

    [Header("Spiral Arms")]
    [Range(1, 6)] public int armCount = 2; // Количество рукавов
    [Range(0.01f, 3f)] public float armCurve = 1.5f; // Извилистость рукавов
    [Range(0.1f, 2f)] public float armSeparation = 1.0f; // Разделение рукавов

    [Header("Motion")]
    [Range(0f, 10f)] public float rotationSpeed = 0.5f;
    [Range(0f, 5f)] public float speedVariance = 1.0f; // Разброс скоростей

    [Header("Visuals")]
    public Gradient starTint;
    [Range(0.1f, 10f)] public float coreBrightness = 2.0f;
    [Range(0.1f, 10f)] public float sizeMultiplier = 1.0f;
    public Texture2D starSprite; // Спрайт звезды (круг или блик)

    private Material _material;
    private MaterialPropertyBlock _mpb;
    private int _kernelId;
    private Bounds _bounds;

    void Start()
    {
        // Создаем материал из нашего шейдера
        _material = new Material(Shader.Find("Hidden/GalaxyShader"));
        _mpb = new MaterialPropertyBlock();

        // Подготовка градиента для шейдера (передаем ключевые цвета)
        UpdateGradientProperties();

        if (starSprite != null)
            _mpb.SetTexture("_MainTex", starSprite);

        // Границы для отсечения (чтобы камера не отсекла галактику)
        _bounds = new Bounds(transform.position, Vector3.one * radius * 2);
    }

    void Update()
    {
        // Обновляем параметры каждый кадр (для настройки в реальном времени)
        _mpb.SetFloat("_Radius", radius);
        _mpb.SetFloat("_Thickness", thickness);
        _mpb.SetFloat("_CoreBulge", coreBulge);
        _mpb.SetFloat("_ArmSpread", armSpread);
        _mpb.SetInt("_ArmCount", armCount);
        _mpb.SetFloat("_ArmCurve", armCurve);
        _mpb.SetFloat("_ArmSeparation", armSeparation);
        _mpb.SetFloat("_RotationSpeed", rotationSpeed);
        _mpb.SetFloat("_SpeedVariance", speedVariance);
        _mpb.SetFloat("_CoreBrightness", coreBrightness);
        _mpb.SetFloat("_SizeMultiplier", sizeMultiplier);
        _mpb.SetVector("_Center", transform.position);

        // Обновляем градиент только если он изменился (для оптимизации можно вынести в OnValidate)
#if UNITY_EDITOR
        UpdateGradientProperties();
#endif

        // Самый важный вызов: Рисуем процедурно
        // MeshTopology.Points рисует каждый вершину как точку (квадрат)
        // Но наш шейдер превращает их в спрайты с помощью Geometry Shader или просто Big Points
        Graphics.DrawProcedural(_material, _bounds, MeshTopology.Points, starCount, properties: _mpb);
    }

    void UpdateGradientProperties()
    {
        // Градиенты в шейдере передавать сложно, поэтому упростим:
        // Берем 4 ключевых цвета из градиента
        if (starTint != null)
        {
            _mpb.SetColor("_ColorInner", starTint.Evaluate(0f));
            _mpb.SetColor("_ColorMid", starTint.Evaluate(0.33f));
            _mpb.SetColor("_ColorOuter", starTint.Evaluate(0.66f));
            _mpb.SetColor("_ColorEdge", starTint.Evaluate(1f));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}