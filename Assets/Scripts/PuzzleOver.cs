using UnityEngine;

public class PuzzleOver : MonoBehaviour
{
    [SerializeField] [Tooltip("Image Tracking manager that detects tracked images")]
    private ImageRecognition imageRecognitionManager;

    [SerializeField] [Tooltip("Distance Tracking Script")]
    private DistanceManager distanceManager;

    [SerializeField] [Tooltip("Puzzle over canvas")]
    private GameObject puzzleOverCanvas;

    private GameObject _puzzleOverCanvas;
    
    private bool _alreadyDisabled;

    private void Start()
    {
        _puzzleOverCanvas = Instantiate(puzzleOverCanvas, Vector3.zero, Quaternion.identity);
        _puzzleOverCanvas.SetActive(false);
    }
    
    private void Update()
    {
        if (_alreadyDisabled || !distanceManager.IsItOver) return;
        
        _alreadyDisabled = true;
        distanceManager.enabled = false;
        imageRecognitionManager.enabled = false;
        _puzzleOverCanvas.SetActive(true);
    }
}