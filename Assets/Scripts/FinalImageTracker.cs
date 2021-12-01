using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class FinalImageTracker : MonoBehaviour
{
    [SerializeField] [Tooltip("Image manager on the AR Session Origin")]
    private ARTrackedImageManager arTrackedImageManager;
    
    [SerializeField][Tooltip("Puzzle full image")]
    private GameObject puzzleImage;

    private GameObject _puzzleImage;

    private void Awake()
    {
        _puzzleImage = Instantiate(puzzleImage, Vector3.zero, Quaternion.identity);
        _puzzleImage.SetActive(false);
    }
    
    public void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    public void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }
    
    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var image in args.added)
        {
            UpdateObj(image);
        }

        foreach (var image in args.updated)
        {
            UpdateObj(image);
        }

        foreach (var image in args.removed)
        {
            _puzzleImage.SetActive(false);
        }
    }

    private void UpdateObj(ARTrackedImage image)
    {
        if (image.referenceImage.name != "Congratulations") return;
        var imgTransform = image.transform;

        _puzzleImage.transform.SetPositionAndRotation(imgTransform.position, imgTransform.rotation);

        _puzzleImage.SetActive(true);
    }
}
