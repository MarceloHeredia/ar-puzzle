using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageRecognition : MonoBehaviour
{
    [SerializeField] [Tooltip("Image manager on the AR Session Origin")]
    private ARTrackedImageManager arTrackedImageManager;

    [SerializeField] private GameObject bottomLeft;
    [SerializeField] private GameObject bottomMid;
    [SerializeField] private GameObject bottomRight;
    [SerializeField] private GameObject upperLeft;
    [SerializeField] private GameObject upperMid;
    [SerializeField] private GameObject upperRight;

    private readonly Dictionary<string, GameObject> _spawnedPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Initialize("BottomLeft", bottomLeft);
        Initialize("BottomMid", bottomMid);
        Initialize("BottomRight", bottomRight);
        Initialize("UpperLeft", upperLeft);
        Initialize("UpperMid", upperMid);
        Initialize("UpperRight", upperRight);
    }

    private void Initialize(string objName, GameObject obj)
    {
        var ob = Instantiate(obj, Vector3.zero, Quaternion.identity);
        _spawnedPrefabs[objName] = ob;
        ob.SetActive(false);
    }

    public void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    public void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public Dictionary<string, GameObject> SpawnedObjects => _spawnedPrefabs;

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

        foreach (var image in args.removed.Where(image => _spawnedPrefabs.ContainsKey(image.name)))
        {
            _spawnedPrefabs[image.referenceImage.name].SetActive(false);
        }
    }

    private void UpdateObj(ARTrackedImage image)
    {
        var imgTransform = image.transform;
        var imgName = image.referenceImage.name;

        if (imgName == "Congratulations") return;

        var prefab = _spawnedPrefabs[imgName];

        prefab.transform.SetPositionAndRotation(imgTransform.position, imgTransform.rotation);

        prefab.SetActive(true);
    }

}