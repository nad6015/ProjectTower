using UnityEngine;

public class ParameterSupport : MonoBehaviour
{
    [field: SerializeField]
    public TextAsset ParamFile { get; private set; }

    [field: SerializeField]
    public TextAsset ConfigFile { get; private set; }

    [field: SerializeField]
    public TextAsset RulesetFile { get; private set; }
}
