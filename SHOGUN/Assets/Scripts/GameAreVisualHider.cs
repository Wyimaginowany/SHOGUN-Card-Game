using UnityEngine;
using UnityEngine.UI;

public class GameAreVisualHider : MonoBehaviour
{
    private RawImage _area;

    private void Start()
    {
        _area = GetComponent<RawImage>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (_area.isActiveAndEnabled)
            {
                _area.enabled = false;
                return;
            }
            _area.enabled = true;
        }
    }

}
