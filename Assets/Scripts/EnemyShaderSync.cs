using UnityEngine;

public class EnemyShaderSync : MonoBehaviour
{
    private SidewaysTrigger _sidewaysTrigger;
    private Renderer _renderer;
    private Material _matInstance;
    private const string PROP_SIDEWAYS = "_Sideways_Strength";
    private const string PROP_BACKWAYS = "_Backways_Strength";

    void Start()
    {
        _sidewaysTrigger = GameObject.FindWithTag("Player").GetComponent<SidewaysTrigger>();
        _renderer = GetComponent<Renderer>();

        if (_renderer != null)
            _matInstance = _renderer.material;
    }

    void Update()
    {
        if (_sidewaysTrigger == null || _matInstance == null) return;

        _matInstance.SetFloat(PROP_SIDEWAYS, _sidewaysTrigger.currentSideways);
        _matInstance.SetFloat(PROP_BACKWAYS, _sidewaysTrigger.currentBackways);
    }
}