using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class PaintOnPlane : MonoBehaviour
{
    [SerializeField] private GameObject pencilPrefab;
    [SerializeField] private Transform pencilLocation;
    [SerializeField] private float distanceThreshold = 0.1f;
    [SerializeField] private Vector3 pencilOffset = new Vector3(1, 1);

    private LineRenderer _lineRenderer;
    private Renderer _renderer;
    private Vector3 _previousPosition;
    private Camera _camera;
    private GameObject _pencil;

    private int _positionIndex;

    private void OnEnable()
    {
        _camera = Camera.main;
        _renderer = GetComponent<Renderer>();

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
        _previousPosition = Vector3.zero;
        _positionIndex = 0;

        _pencil = Instantiate(pencilPrefab, Vector3.forward * 5,
            Quaternion.Euler(120, -70, 60));

        _pencil.transform.DOMove(transform.position + pencilOffset, 0.5f);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    var hitPoint = hit.point + Vector3.forward * 0.01f;

                    if (_positionIndex == 0 || Vector3.Distance(hitPoint, _previousPosition) > distanceThreshold)
                    {
                        _lineRenderer.positionCount++;
                        _lineRenderer.SetPosition(_positionIndex, hitPoint);
                        _previousPosition = hitPoint;
                        _positionIndex++;

                        _pencil.transform.position = hitPoint + pencilOffset;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _renderer.material.SetColor("_Color", Color.black);
            _lineRenderer.enabled = false;
            _pencil.transform.DOMove(pencilLocation.position, 1f);
            _pencil.transform.DORotate(Vector3.zero, 1f);
            enabled = false;
        }
    }
}