using UnityEngine;

[ExecuteInEditMode] // Чтобы видеть галактику в редакторе
public class GalaxyGenerator2D : MonoBehaviour
{
    [Header("Performance")]
    [Tooltip("Количество звезд. 100,000 работает отлично на современных GPU.")]
    public int starCount = 100000;

    [Header("Galaxy Shape")]
    [Range(0.1f, 50f)] public float radius = 15f;
    [Range(0.1f, 5f)] public float armSpread = 2.0f;
    [Range(1, 6)] public int armCount = 2;
    [Range(0.01f, 5f)] public float armCurve = 1.5f;

    [Header("Motion")]
    [Range(0f, 10f)] public float rotationSpeed = 0.5f;
    [Range(0f, 5f)] public float speedVariance = 1.0f;

    [Header("Visuals")]
    public Gradient starTint;
    [Range(0.1f, 5f)] public float sizeMultiplier = 1.0f;
    [Range(0.0f, 1.0f)] public float sizeVariance = 0.5f; // Разброс размеров
    public Texture2D starSprite; // Текстура звезды (мягкий круг)

    // Внутренние переменные
    private Material _material;
    private MaterialPropertyBlock _mpb;
    private Bounds _bounds;

    void OnEnable()
    {
        // Инициализация материала и блока свойств
        if (_material == null)
            _material = new Material(Shader.Find("Hidden/Galaxy2DShader"));

        _mpb = new MaterialPropertyBlock();
        UpdateProperties();
    }

    void OnDisable()
    {
        // Очистка ресурсов
        if (_material != null) DestroyImmediate(_material);
    }

    // Обновление свойств для шейдера
    void UpdateProperties()
    {
        if (_mpb == null) return;

        // Передача цветов градиента (упрощенно 4 ключевых точки)
        if (starTint != null)
        {
            _mpb.SetColor("_ColorInner", starTint.Evaluate(0f));
            _mpb.SetColor("_ColorMid", starTint.Evaluate(0.33f));
            _mpb.SetColor("_ColorOuter", starTint.Evaluate(0.66f));
            _mpb.SetColor("_ColorEdge", starTint.Evaluate(1f));
        }

        if (starSprite != null) _mpb.SetTexture("_MainTex", starSprite);

        _mpb.SetFloat("_Radius", radius);
        _mpb.SetFloat("_ArmSpread", armSpread);
        _mpb.SetInt("_ArmCount", armCount);
        _mpb.SetFloat("_ArmCurve", armCurve);
        _mpb.SetFloat("_RotationSpeed", rotationSpeed);
        _mpb.SetFloat("_SpeedVariance", speedVariance);
        _mpb.SetFloat("_SizeMultiplier", sizeMultiplier);
        _mpb.SetFloat("_SizeVariance", sizeVariance);

        // Передаем позицию объекта, чтобы галактика двигалась вместе с ним
        _mpb.SetVector("_Center", transform.position);
    }

    void Update()
    {
        UpdateProperties();

        // Границы отсечения (чтобы объект не пропадал с экрана)
        _bounds = new Bounds(transform.position, Vector3.one * radius * 2.5f);

        // Рисуем процедурный меш.
        // Мы просим GPU нарисовать Quads (квадраты).
        // На каждый Quad нужно 4 вершины.
        // MeshTopology.Quads рисует "пустой" меш, данные генерируются в шейдере.
        Graphics.DrawProcedural(_material, _bounds, MeshTopology.Quads, starCount * 4, properties: _mpb);
    }
}